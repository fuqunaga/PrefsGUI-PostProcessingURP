using PrefsGUI.RosettaUI;
using RosettaUI;

namespace PrefsGUI.PostProcessingURP
{
    public partial class PrefsVolumeBehaviour : IElementCreator
    {
        public virtual Element CreateElement(LabelElement label)
        {
            return UI.Column(
                prefsVolumeWait.CreateSlider("Weight", 1f),
                UI.Field(null, () => prefsVolumeProfile)
            );
        }
    }
}