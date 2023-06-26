using System;
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
            var assemblySymbol = context.Compilation.SourceModule.ReferencedAssemblySymbols
                .FirstOrDefault(a => a.Name == "Unity.RenderPipelines.Universal.Runtime");

            if (assemblySymbol == null) return;

            var volumeComponentTypes = assemblySymbol.GlobalNamespace
                .GetNamespaceMembers().First(m => m.Name == "UnityEngine")
                .GetNamespaceMembers().First(m => m.Name == "Rendering")
                .GetNamespaceMembers().First(m => m.Name == "Universal")
                .GetTypeMembers()
                .Where(typeSymbol => !typeSymbol.IsAbstract && typeSymbol.BaseType?.Name == "VolumeComponent");


            foreach (var volumeComponentType in volumeComponentTypes)
            {
                AddPrefsVolumeComponent(volumeComponentType, context);
            }
        }

        private static void AddPrefsVolumeComponent(INamedTypeSymbol typeSymbol, GeneratorExecutionContext context)
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
            
            if ( parameterAndValue.Count == 0 ) return;
            
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


            var sourceText =
                $@"using System;
using UnityEngine.Rendering.Universal;

namespace PrefsGUI.PostProcessingURP
{{
    [Serializable]
    public class {className} : PrefsVolumeComponent<{componentName}>
    {{{string.Join("", fieldsTexts)}

        protected override void BindVolumeComponentToParameters({componentName} component)
        {{{string.Join("", bindTexts)}
        }}  
    }}
}}
";

            context.AddSource(
                hintName: $"{className}.g.cs",
                sourceText: SourceText.From(sourceText, Encoding.UTF8));


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
    }
}