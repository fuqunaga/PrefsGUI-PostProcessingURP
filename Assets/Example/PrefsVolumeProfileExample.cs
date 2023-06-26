using RosettaUI;
using UnityEngine;

namespace PrefsGUI.PostProcessingURP.Example
{
    [RequireComponent(typeof(RosettaUIRoot))]
    public class PrefsVolumeProfileExample : MonoBehaviour
    {
        private void Start()
        {
            var root = GetComponent<RosettaUIRoot>();
            root.Build(CreateElement());
        }

        private Element CreateElement()
        {
            return UI.Window(
                UI.Page(
                    UI.FieldIfObjectFound<PrefsVolume>()
                )
            );
        }
    }
}