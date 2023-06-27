using UnityEngine.Rendering;

namespace PrefsGUI.PostProcessingURP
{
    public partial class PrefsVolumeProfile
    {
        public void Bind(VolumeProfile profile)
        {
            foreach(var componentPrefs in VolumeComponentPrefsList)
            {
                componentPrefs.BindVolumeProfile(profile);
            }
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