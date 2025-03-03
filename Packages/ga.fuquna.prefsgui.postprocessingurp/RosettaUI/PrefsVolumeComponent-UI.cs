using System.Linq;
using PrefsGUI.RosettaUI;
using RosettaUI;
using UnityEngine.Rendering;

namespace PrefsGUI.PostProcessingURP
{
    public abstract partial class PrefsVolumeComponent<TVolumeComponent>
        where TVolumeComponent : VolumeComponent
    {
        public virtual Element CreateElement(LabelElement label)
        {
            UICustom.UnregisterPropertyOrFields<PrefsVolumeComponent<TVolumeComponent>>(nameof(active));
            
            return UI.Fold(
                UI.Row(
                    UI.Toggle(null, () => active.Get(), value => active.Set(value)),
                    label,
                    UI.Space(),
                    active.CreateDefaultButtonElement()
                ),
                ParameterDictionary.Select(kv =>
                {
                    var (name, prefs) = kv;
                    return UI.Field(name, () => prefs);
                })
            );
        }
    }

    public partial interface IPrefsVolumeComponent : IElementCreator
    {
    }
}