using System.Collections.Generic;
using UnityEngine.Rendering;

namespace PrefsGUI.PostProcessingURP
{
    public partial class PrefsVolume
    {
        // private Dictionary<string, string> hoge = new Dictionary<string, string>()
        // {
        //     ["hoge"] = "hoge"
        // };

        public void Bind(VolumeProfile profile)
        {
            foreach(var componentPrefs in VolumeComponentPrefsList)
            {
                componentPrefs.BindVolumeProfile(profile);
            }
        }
    }
}