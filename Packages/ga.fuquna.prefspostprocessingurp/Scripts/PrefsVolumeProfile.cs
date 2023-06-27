using UnityEngine.Rendering;

namespace PrefsGUI.PostProcessingURP
{
    public partial class PrefsVolumeProfile
    {
        private VolumeProfile _volumeProfile;
    
    
        public void Bind(VolumeProfile profile)
        {
            _volumeProfile = profile;
            
            foreach(var componentPrefs in VolumeComponentPrefsList)
            {
                componentPrefs.BindVolumeProfile(profile);
            }
        }

        public void Unbind()
        {
            foreach(var componentPrefs in VolumeComponentPrefsList)
            {
                componentPrefs.UnbindVolumeProfile();
            }

            _volumeProfile = null;
        }
        
        public void Reset(VolumeProfile profile)
        {
            foreach(var componentPrefs in VolumeComponentPrefsList)
            {
                componentPrefs.Reset(profile);
            }
        }
    }
}