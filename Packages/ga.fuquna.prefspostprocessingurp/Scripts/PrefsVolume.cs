using System.Collections.Generic;
using UnityEngine.Rendering;

namespace PrefsGUI.PostProcessingURP
{
    public partial class PrefsVolume
    {
        public void Bind(VolumeProfile profile)
        {
            foreach(var componentPrefs in VolumeComponentPrefsList)
            {
                componentPrefs.BindVolumeProfile(profile);
            }
        }
    }
}