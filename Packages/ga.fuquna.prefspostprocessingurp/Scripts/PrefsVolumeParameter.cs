using System;
using UnityEngine.Rendering;

namespace PrefsGUI.PostProcessingURP
{
    [Serializable]
    public partial class PrefsVolumeParameter<T> : IPrefsVolumeParameter
        where T : struct
    {
        public PrefsBool overrideState;
        public PrefsParam<T> value;

        private T? _min;
        private T? _max;
        private VolumeParameter<T> _volumeParameter;
        
        public Type ParameterType => typeof(T);
        
        public PrefsVolumeParameter(string key, T defaultValue = default)
        {
            overrideState = new PrefsBool($"{key}_OverrideState");
            value = new PrefsParam<T>($"{key}_Value", defaultValue);
        }
    
        public void Bind(VolumeParameter<T> parameter)
        {
            if ( parameter == null ) return;
            
            Unbind();
            
            _volumeParameter = parameter;
            overrideState.RegisterValueChangedCallbackAndCallOnce(OnValueChangedOverrideState);
            value.RegisterValueChangedCallbackAndCallOnce(OnValueChanged);

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

        private void Unbind()
        {
            overrideState.UnregisterValueChangedCallback(OnValueChangedOverrideState);
            value.UnregisterValueChangedCallback(OnValueChanged);

            _min = null;
            _max = null;
        }

        private void OnValueChangedOverrideState() => _volumeParameter.overrideState = overrideState;

        private void OnValueChanged() => _volumeParameter.value = value;
    }

    public partial interface IPrefsVolumeParameter
    {
        Type ParameterType { get; }
    }
}