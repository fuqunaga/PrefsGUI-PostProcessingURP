using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace PrefsGUI.PostProcessingURP
{
    [Serializable]
    public abstract partial class PrefsVolumeComponent<TVolumeComponent> : IPrefsVolumeComponent
        where TVolumeComponent : VolumeComponent
    {
        protected static readonly string KeyPrefix = $"URP_{typeof(TVolumeComponent).Name}";
        
        public PrefsBool active = new($"{KeyPrefix}_Active");

        private VolumeProfile _volumeProfile;
        private TVolumeComponent _volumeComponent;
        
        
        public Type ComponentType => typeof(TVolumeComponent);

        public bool Exists
        {
            get
            {
                var exists = _volumeProfile != null && _volumeProfile.Has<TVolumeComponent>();
                
                // BindVolumeProfile()時はなかったけどあとからVolumeComponentが追加された場合を考慮
                if (exists && _volumeComponent == null)
                {
                    BindVolumeProfileToPrefs();
                }
                return exists;
            }
        }


        public abstract IReadOnlyDictionary<string, IPrefsVolumeParameter> ParameterDictionary { get; }
        
        public virtual void BindVolumeProfile(VolumeProfile profile)
        {
            UnbindVolumeProfile();

            _volumeProfile = profile;
            BindVolumeProfileToPrefs();
        }

        protected virtual void BindVolumeProfileToPrefs()
        {
            if (!_volumeProfile.TryGet(out _volumeComponent)) return;

            active.RegisterValueChangedCallbackAndCallOnce(OnValueChangedActive);
            BindVolumeComponentToParameters(_volumeComponent);
        }
        
        public virtual void UnbindVolumeProfile()
        {
            active.UnregisterValueChangedCallback(OnValueChangedActive);
            _volumeComponent = null;
            _volumeProfile = null;
        }
        
        private void OnValueChangedActive() => _volumeComponent.active = active;
        
        public void BindVolumeComponentToParameters(VolumeComponent component)
        {
            if (component is TVolumeComponent c)
            {
                BindVolumeComponentToParameters(c);
            }
        }
        
        protected abstract void BindVolumeComponentToParameters(TVolumeComponent component);

        
        public void Reset(VolumeProfile profile)
        {
            if (profile.TryGet<TVolumeComponent>(out var component))
            {
                active = new(active.key, component.active);
                ResetVolumeComponentToParameters(component);
            }
        }
        
        protected abstract void ResetVolumeComponentToParameters(TVolumeComponent component);
    }

    public partial interface IPrefsVolumeComponent
    {
        Type ComponentType { get; }
        bool Exists { get; }
        void BindVolumeProfile(VolumeProfile profile);
        void UnbindVolumeProfile();
        void Reset(VolumeProfile profile);
    }
}