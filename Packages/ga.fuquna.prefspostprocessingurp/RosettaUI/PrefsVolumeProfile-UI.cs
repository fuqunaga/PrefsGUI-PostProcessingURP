using System.Linq;
using RosettaUI;

namespace PrefsGUI.PostProcessingURP
{
    public partial class PrefsVolumeProfile : IElementCreator
    {
        public virtual Element CreateElement(LabelElement label)
        {
            return UI.DynamicElementOnStatusChanged(
                () => _volumeProfile == null ? 0 : _volumeProfile.components.Count,
                _ => UI.Column(
                    _volumeProfile.components.Select(component =>
                    {
                        var componentType = component.GetType();
                        var prefs = VolumeComponentPrefsList.FirstOrDefault(prefs =>
                            prefs.ComponentType == componentType);
                        return prefs == null
                            ? null
                            : UI.Field(prefs.ComponentType.Name, () => prefs);
                    })
                )
            );
        }
    }
}