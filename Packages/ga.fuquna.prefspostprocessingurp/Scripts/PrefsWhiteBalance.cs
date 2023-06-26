using System;
using PrefsGUI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PrefsGUI.PostProcessingURP
{
    // [Serializable]
    // public class PrefsWhiteBalance
    // {
    //     public PrefsBool active = new("WhiteBalance_Active");
    //     public PrefsVolumeParameter<float> temperature = new ("WhiteBalance_Temperature", 0f, -100f, 100f);
    //     public PrefsVolumeParameter<float> tint = new ("WhiteBalance_Tint");
    //
    //     public void BindVolumeProfile(VolumeProfile profile)
    //     {
    //         if (!profile.TryGet<WhiteBalance>(out var volumeComponent)) return;
    //         
    //         active.RegisterValueChangedCallbackAndCallOnce(() => volumeComponent.active = active);
    //         temperature.Bind(volumeComponent.temperature);
    //         tint.Bind(volumeComponent.tint);
    //     }
    // }
}