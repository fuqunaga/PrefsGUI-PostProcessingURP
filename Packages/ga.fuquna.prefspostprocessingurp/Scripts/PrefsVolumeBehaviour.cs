using UnityEngine;
using UnityEngine.Rendering;

namespace PrefsGUI.PostProcessingURP
{
    public partial class PrefsVolumeBehaviour : MonoBehaviour
    {
        public Volume volume;
        public PrefsFloat prefsVolumeWait = new("URP_Volume_Weight", 1f);
        public PrefsVolumeProfile prefsVolumeProfile = new();

        public void Start()
        {
            prefsVolumeWait.RegisterValueChangedCallback(() => volume.weight = prefsVolumeWait);
        
            var profile = volume.profile;
            prefsVolumeProfile.Bind(profile);
        }

        public void Reset() => SetDefaultValueFromVolume();
        

        [ContextMenu(nameof(SetDefaultValueFromVolume))]
        public void SetDefaultValueFromVolume()
        {
            if (volume == null)
            {
                if (!TryGetComponent(out volume)) return;
            }

            prefsVolumeWait = new(prefsVolumeWait.key, volume.weight);

            var profile = volume.profile;
            prefsVolumeProfile.Reset(profile);
        }
    }
}