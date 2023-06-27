using RosettaUI;
using UnityEngine;

namespace PrefsGUI.PostProcessingURP.Example
{
    [RequireComponent(typeof(RosettaUIRoot))]
    public class PrefsVolumeBehaviourExample : MonoBehaviour
    {
        private void Start()
        {
            var root = GetComponent<RosettaUIRoot>();
            root.Build(CreateElement());
        }

        private Element CreateElement()
        {
            return UI.Window(nameof(PrefsVolumeBehaviour),
                UI.Page(
                    UI.Row(
                        UI.Button("Save", Prefs.Save),
                        UI.Button("Load", Prefs.Load)
                    ),
                    UI.FieldIfObjectFound<PrefsVolumeBehaviour>()
                )
            );
        }
    }
}