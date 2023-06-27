using System.Linq;
using RosettaUI;

namespace PrefsGUI.PostProcessingURP
{
    public partial class PrefsVolumeProfile : IElementCreator
    {
        public virtual Element CreateElement(LabelElement label)
        {
            return UI.Column(
                VolumeComponentPrefsList.Select(componentPrefs =>
                    UI.DynamicElementIf(
                        () => componentPrefs.Exists,
                        () => UI.Field(componentPrefs.ComponentType.Name, () => componentPrefs))
                )
            );
        }
    }
}