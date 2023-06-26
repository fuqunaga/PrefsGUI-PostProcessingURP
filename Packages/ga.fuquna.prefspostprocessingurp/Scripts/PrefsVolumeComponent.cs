using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace PrefsGUI.PostProcessingURP
{
    [Serializable]
    public  abstract partial class PrefsVolumeComponent<TVolumeComponent> : IPrefsVolumeComponent
        where TVolumeComponent : VolumeComponent
    {
        protected static readonly string KeyPrefix = $"URP_{typeof(TVolumeComponent).Name}";
        
        public PrefsBool active = new($"{KeyPrefix}_Active");
        
        private TVolumeComponent _volumeComponent;
        
        public bool Exists => _volumeComponent != null;
        
        public abstract IReadOnlyDictionary<string, IPrefsVolumeParameter> ParameterDictionary { get; }
        
        public virtual void BindVolumeProfile(VolumeProfile profile)
        {
            UnbindVolumeProfile();
            
            if (!profile.TryGet(out _volumeComponent)) return;

            active.RegisterValueChangedCallbackAndCallOnce(OnValueChangedActive);
            BindVolumeComponentToParameters(_volumeComponent);
        }
        
        public virtual void UnbindVolumeProfile()
        {
            active.UnregisterValueChangedCallback(OnValueChangedActive);
            _volumeComponent = null;
        }
        
        private void OnValueChangedActive() => _volumeComponent.active = active;
        

        protected abstract void BindVolumeComponentToParameters(TVolumeComponent component);
        
        public void BindVolumeComponentToParameters(VolumeComponent component)
        {
            if (component is TVolumeComponent c)
            {
                BindVolumeComponentToParameters(c);
            }
        }
    }

    public interface IPrefsVolumeComponent
    {
        bool Exists { get; }
        void BindVolumeProfile(VolumeProfile profile);
        void UnbindVolumeProfile();
    }
}