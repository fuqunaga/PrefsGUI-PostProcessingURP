using System;
using PrefsGUI;
using UnityEngine.Rendering;

namespace PrefsGUI.PostProcessingURP
{
    [Serializable]
    public abstract class PrefsVolumeComponent<TVolumeComponent> 
        where TVolumeComponent : VolumeComponent
    {
        protected static readonly string KeyPrefix = $"URP_{typeof(TVolumeComponent).Name}";
        
        public PrefsBool active = new($"{KeyPrefix}_Active");
        
        public virtual void BindVolumeProfile(VolumeProfile profile)
        {
            if (!profile.TryGet<TVolumeComponent>(out var component)) return;
            
            active.RegisterValueChangedCallbackAndCallOnce(() => component.active = active);
            BindVolumeComponentToParameters(component);
        }

        protected abstract void BindVolumeComponentToParameters(TVolumeComponent component);
    }
}