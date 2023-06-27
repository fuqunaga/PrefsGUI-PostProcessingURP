using UnityEngine;
using UnityEngine.Rendering;

namespace PrefsGUI.PostProcessingURP
{
    public partial class PrefsVolumeBehaviour : MonoBehaviour
    {
        public Volume volume;
        public PrefsFloat prefsVolumeWait = new("URP_Volume_Weight", 1f);
        public PrefsVolumeProfile prefsVolumeProfile;

        private void Start()
        {
            prefsVolumeWait.RegisterValueChangedCallback(() => volume.weight = prefsVolumeWait);
        
            var profile = volume.profile;
            prefsVolumeProfile.Bind(profile);
        }
    }
}