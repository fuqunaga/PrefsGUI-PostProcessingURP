using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;


namespace PrefsGUI.PostProcessingURP.SourceGenerator
{
    [Generator]
    public class PostProcessingUrpSourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            // avoid conflict warning
            if (context.Compilation.AssemblyName != "PrefsGUI-PostProcessingURP") return;
            
            var assemblySymbol = context.Compilation.SourceModule.ReferencedAssemblySymbols
                .FirstOrDefault(a => a.Name == "Unity.RenderPipelines.Universal.Runtime");

            if (assemblySymbol == null) return;

            var volumeComponentTypes = assemblySymbol.GlobalNamespace
                .GetNamespaceMembers().First(m => m.Name == "UnityEngine")
                .GetNamespaceMembers().First(m => m.Name == "Rendering")
                .GetNamespaceMembers().First(m => m.Name == "Universal")
                .GetTypeMembers()
                .Where(typeSymbol => !typeSymbol.IsAbstract && typeSymbol.BaseType?.Name == "VolumeComponent");

            var generatedVolumeComponentTypes = new List<INamedTypeSymbol>();

            foreach (var volumeComponentType in volumeComponentTypes)
            {
                var success = GeneratePrefsVolumeComponent(volumeComponentType, context);
                if (success)
                {
                    generatedVolumeComponentTypes.Add(volumeComponentType);
                }
            }

            GeneratePrefsVolume(generatedVolumeComponentTypes, context);
        }

        private static bool GeneratePrefsVolumeComponent(INamedTypeSymbol typeSymbol, GeneratorExecutionContext context)
        {
            var parameterAndValue = typeSymbol.GetMembers()
                .Where(s => s.Kind == SymbolKind.Field)
                .Cast<IFieldSymbol>()
                .Where(s => !s.GetAttributes().Any(a =>
                {
                    var attribute = a.AttributeClass;
                    return attribute?.Name is nameof(ObsoleteAttribute) or "AdditionalPropertyAttribute";
                }))
                .Select(ToParameterAndValueType)
                .Where(pair => pair.valueType is { IsValueType: true }) // ignore class type like Texture
                .ToImmutableList();
            
            if ( parameterAndValue.Count == 0 ) return false;
            
            var componentName = typeSymbol.Name;
            var className = $"Prefs{componentName}";


            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            var fieldsTexts = parameterAndValue.Select(pair =>
            {
                var valueType = pair.valueType.ToDisplayString();
                var fieldName = pair.field.Name;
                var keyText = $"$\"{{KeyPrefix}}_{textInfo.ToTitleCase(fieldName)}\"";
                return $@"
        public PrefsVolumeParameter<{valueType}> {pair.field.Name} = new ({keyText});";
            });

            var bindTexts = parameterAndValue.Select(pair => $@"
            {pair.field.Name}.Bind(component.{pair.field.Name});");

            var resetTexts = parameterAndValue.Select(pair => $@"
            {pair.field.Name}.Reset(component.{pair.field.Name});");

            var sourceText =
                $@"using System;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

namespace PrefsGUI.PostProcessingURP
{{
    [Serializable]
    public partial class {className} : PrefsVolumeComponent<{componentName}>
    {{{string.Join("", fieldsTexts)}

        private IReadOnlyDictionary<string, IPrefsVolumeParameter> _parameterDictionary;

        public override IReadOnlyDictionary<string, IPrefsVolumeParameter> ParameterDictionary => _parameterDictionary ??= new Dictionary<string, IPrefsVolumeParameter>
        {{{string.Join("", parameterAndValue.Select(pair => $@"
            [nameof({pair.field.Name})] = {pair.field.Name},"))}
        }};

        protected override void BindVolumeComponentToParameters({componentName} component)
        {{{string.Join("", bindTexts)}
        }}

        protected override void ResetVolumeComponentToParameters({componentName} component)
        {{{string.Join("", resetTexts)}
        }}  
    }}
}}
";

            context.AddSource(
                hintName: $"{className}.g.cs",
                sourceText: SourceText.From(sourceText, Encoding.UTF8));
            
            return true;

            static (IFieldSymbol field, ITypeSymbol valueType) ToParameterAndValueType(IFieldSymbol fieldSymbol)
            {
                var valueType = GetVolumeParameterValueType(fieldSymbol.Type as INamedTypeSymbol);
                return (fieldSymbol, valueType);
            }
            
            static ITypeSymbol GetVolumeParameterValueType(INamedTypeSymbol  typeSymbol)
            {
                while (true)
                {
                    if (typeSymbol == null) return null;
                    if (typeSymbol.Name == "VolumeParameter") return typeSymbol.TypeArguments.FirstOrDefault();
                    typeSymbol = typeSymbol.BaseType;
                }
            }
        }
        
        
        private void GeneratePrefsVolume(List<INamedTypeSymbol> volumeComponentTypes, GeneratorExecutionContext context)
        {
            const string className = "PrefsVolumeProfile";
            
            var fieldsTexts = volumeComponentTypes.Select(type =>
            {
                var typeName = type.Name;
                var fieldName = ToLowerOnlyFirst(typeName);
                return $@"
        public Prefs{typeName} {fieldName} = new();";
            });
       
            var sourceText =
                $@"using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace PrefsGUI.PostProcessingURP
{{
    [Serializable]
    public partial class {className}
    {{{string.Join("", fieldsTexts)}

        private IReadOnlyList<IPrefsVolumeComponent> _volumeComponentPrefsList;

        public IReadOnlyList<IPrefsVolumeComponent> VolumeComponentPrefsList
            => _volumeComponentPrefsList ??= CreateVolumeComponentPrefsList();
            
        private List<IPrefsVolumeComponent> CreateVolumeComponentPrefsList()
        {{
            return new List<IPrefsVolumeComponent>{{{string.Join(",", volumeComponentTypes.Select(type => $@"
                {ToLowerOnlyFirst(type.Name)}"))}
            }};
        }}
    }}
}}
";     
            context.AddSource(
                hintName: $"{className}.g.cs",
                sourceText: SourceText.From(sourceText, Encoding.UTF8));
            
            static string ToLowerOnlyFirst(string str)
            {
                if (string.IsNullOrEmpty(str)) return str;
                return char.ToLowerInvariant(str[0]) + str.Substring(1);
            }
        }
    }
}