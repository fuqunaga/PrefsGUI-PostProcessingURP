using System;
using PrefsGUI;
using PrefsGUI.RosettaUI;
using RosettaUI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PrefsGUI.PostProcessingURP
{
    public partial class PrefsVolume : MonoBehaviour, IElementCreator
    {
        public Volume volume;

        public PrefsFloat volumeWait = new("Volume_Weight", 1f);
        public PrefsBloom bloom;
        public PrefsWhiteBalance whiteBalance = new();
        public PrefsVignette vignette = new();
        
        private void Start()
        {
            volumeWait.RegisterValueChangedCallback(() => volume.weight = volumeWait);
        
            var profile = volume.profile;
            bloom.BindVolumeProfile(profile);
            whiteBalance.BindVolumeProfile(profile);
        }
        
        public Element CreateElement(LabelElement label)
        {
            return UI.Column(
                UI.Button("Save", Prefs.Save),
                volumeWait.CreateSlider(1f),
                UI.Field("Bloom", () => bloom),
                UI.Field(() => whiteBalance),
                UI.Field(() => vignette)
            );
        }
    }
}