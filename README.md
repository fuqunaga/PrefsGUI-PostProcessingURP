# PrefsGUI-PostProcessingURP

PrefsGUI extensions for URP PostProcessing.  
Automatic generation of UI, change of parameters at runtime and saving of their values.

![screenshot](https://github.com/fuqunaga/PrefsGUI-PostProcessingURP/assets/821072/688ec82e-2140-4bae-80f6-98496c657ae8)

# Installation

This package uses the [scoped registry] feature to resolve package
dependencies. 

[scoped registry]: https://docs.unity3d.com/Manual/upm-scoped.html


**Edit > ProjectSettings... > Package Manager > Scoped Registries**

Enter the following and click the Save button.

```
"name": "fuqunaga",
"url": "https://registry.npmjs.com",
"scopes": [ "ga.fuquna" ]
```
![Scoped Registrie Settings](https://github.com/fuqunaga/PrefsGUI-PostProcessingURP/assets/821072/1b4c1008-dd5b-469b-a270-513c50c545fb)


**Window > Package Manager**

Select `MyRegistries` in `Packages:`

![Select MyRegistries](https://github.com/fuqunaga/PrefsGUI-PostProcessingURP/assets/821072/74b3b9b4-4a75-4dfa-b5b0-06999c8ad0ac)

Select `PrefsGUI - PostProcessingURP` and click the Install button  

![image](https://github.com/fuqunaga/PrefsGUI-PostProcessingURP/assets/821072/9375e0dc-0efb-48e6-9914-a715fddc9efc)


# Usage

1. Attach `PrefsVolumeBehaviour` to any GameObject.
2. Set the target `Volume` component in `PrefsVolumeBehaviour.volume`.
3. (Optional)Call **SetDefaultValueFromVolume** from the `PrefsVolumeBehaviour` context menu to set the `Volume` values to the default values in the PrefsGUI.  
*If `PrefsVolumeBehaviour` is attached to the same GameObject as the `Volume`, it is done automatically.  
![SetDefaultValueFromVolume](https://github.com/fuqunaga/PrefsGUI-PostProcessingURP/assets/821072/2baf4342-09a7-4582-a350-b4ab3e016dca)
1. Follow the [RosettaUI](https://github.com/fuqunaga/RosettaUI) steps to display the PrefsVolumeBehaviour UI.  
   See [Example](Assets/Example/PrefsVolumeBehaviourExample.cs).


# Tips
## Changing Prefs keys for multiple instances

Prefix keys can be added or changed in batches from the editor window.  
See [PrefsGUI.EditorWindow](https://github.com/fuqunaga/PrefsGUI#editorwindow).

# Reference

- [PrefsGUI](https://github.com/fuqunaga/PrefsGUI) - Accessors and GUIs for persistent preference values using a JSON file
- [RosettaUI](https://github.com/fuqunaga/RosettaUI) - Code-based UI library for development menus for Unity
