using System.Linq;
using RosettaUI;

namespace PrefsGUI.PostProcessingURP
{
    public partial class PrefsVolume : IElementCreator
    {
        public Element CreateElement(LabelElement label)
        {
            return UI.Fold(label,
                VolumeComponentPrefsList.Select(componentPrefs =>
                    UI.DynamicElementIf(
                        () => componentPrefs.Exists,
                        () => UI.Field(componentPrefs.ComponentType.Name, () => componentPrefs))
                )
            );
        }
    }
}