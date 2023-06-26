using System.Linq;
using RosettaUI;
using UnityEngine.Rendering;

namespace PrefsGUI.PostProcessingURP
{
    public abstract partial class PrefsVolumeComponent<TVolumeComponent> : IElementCreator
        where TVolumeComponent : VolumeComponent
    {
        public Element CreateElement(LabelElement label)
        {
            UICustom.UnregisterPropertyOrFields<PrefsVolumeComponent<TVolumeComponent>>(nameof(active));
            
            return UI.Fold(
                UI.Row(
                    UI.Toggle(null, () => active.Get(), value => active.Set(value)),
                    label
                ),
                ParameterDictionary.Select(kv =>
                {
                    var (name, prefs) = kv;
                    return UI.Field(name, () => prefs);
                })
            );
        }
    }
}