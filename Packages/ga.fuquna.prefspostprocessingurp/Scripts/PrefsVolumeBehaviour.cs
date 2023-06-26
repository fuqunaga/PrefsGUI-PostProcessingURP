using PrefsGUI.RosettaUI;
using RosettaUI;
using UnityEngine;
using UnityEngine.Rendering;

namespace PrefsGUI.PostProcessingURP
{
    public class PrefsVolumeBehaviour : MonoBehaviour, IElementCreator
    {
        public Volume volume;
        public PrefsVolume prefsVolume;

        public PrefsFloat volumeWait = new("Volume_Weight", 1f);
        
        private void Start()
        {
            volumeWait.RegisterValueChangedCallback(() => volume.weight = volumeWait);
        
            var profile = volume.profile;
            prefsVolume.Bind(profile);
        }
        
        public Element CreateElement(LabelElement label)
        {
            return UI.Column(
                UI.Button("Save", Prefs.Save),
                volumeWait.CreateSlider(1f),
                UI.Field(() => prefsVolume)
            );
        }
    }
}