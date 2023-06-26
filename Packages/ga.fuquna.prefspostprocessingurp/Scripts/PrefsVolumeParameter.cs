using System;
using PrefsGUI;
using PrefsGUI.RosettaUI;
using RosettaUI;
using UnityEngine.Rendering;

namespace PrefsGUI.PostProcessingURP
{
    [Serializable]
    public class PrefsVolumeParameter<T> : IElementCreator
        where T : struct
    {
        public PrefsBool overrideState;
        public PrefsParam<T> value;

        private T? _min;
        private T? _max;
        
        public PrefsVolumeParameter(string key, T defaultValue = default)
        {
            overrideState = new PrefsBool($"{key}_OverrideState");
            value = new PrefsParam<T>($"{key}_Value", defaultValue);
        }
    
        public void Bind(VolumeParameter<T> parameter)
        {
            overrideState.RegisterValueChangedCallbackAndCallOnce(() => parameter.overrideState = overrideState);
            value.RegisterValueChangedCallbackAndCallOnce(() => parameter.value = value);

            switch (parameter)
            {
                case ClampedFloatParameter cf:
                    _min = (T)(object)cf.min;
                    _max = (T)(object)cf.max;
                    break;
                case ClampedIntParameter ci:
                    _min = (T)(object)ci.min;
                    _max = (T)(object)ci.max;
                    break;
            }
        }
    
        public Element CreateElement(LabelElement label)
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
}