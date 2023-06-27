using PrefsGUI.RosettaUI;
using RosettaUI;

namespace PrefsGUI.PostProcessingURP
{
    public partial class PrefsVolumeParameter<T> 
        where T : struct
    {
        public virtual Element CreateElement(LabelElement label)
        {
            var element = UI.Row(
                overrideState.CreateFieldRaw(),
                (_min.HasValue && _max.HasValue)
                    ? value.CreateSliderRaw(label, _min.Value, _max.Value)
                    : value.CreateFieldRaw(label),
                PrefsGUIElement.CreateDefaultButtonElement(
                    onClick: () =>
                    {
                        overrideState.ResetToDefault();
                        value.ResetToDefault();
                    },
                    isDefault: () => overrideState.IsDefault && value.IsDefault
                )
            );
            
            PrefsGUIExtension.SubscribeSyncedFlag(overrideState, element);
            PrefsGUIExtension.SubscribeSyncedFlag(value, element);

            return element;
        }
    }

    public partial interface IPrefsVolumeParameter : IElementCreator
    {
    }
}