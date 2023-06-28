# PrefsGUI-PostProcessingURP

PrefsGUI extensions for URP PostProcessing.  
Automatic generation of UI, change of parameters at runtime and saving of their values.

![screenshot](https://github.com/fuqunaga/PrefsGUI-PostProcessingURP/assets/821072/b7010c09-e853-4d24-9718-2060d3685bb8)

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


# Usage

1. Attach `PrefsVolumeBehaviour` to any GameObject.
2. Set the target `Volume` component in `PrefsVolumeBehaviour.volume`.
3. (Optional)Call **SetDefaultValueFromVolume** from the `PrefsVolumeBehaviour` context menu sets the `Volume` values to the default values in the PrefsGUI.  
*If `PrefsVolumeBehaviour` is attached to the same GameObject as the `Volume`, it is done automatically.

![SetDefaultValueFromVolume](https://github.com/fuqunaga/PrefsGUI-PostProcessingURP/assets/821072/2baf4342-09a7-4582-a350-b4ab3e016dca)



# Reference

- [PrefsGUI](https://github.com/fuqunaga/PrefsGUI)
- [RosettaUI](https://github.com/fuqunaga/RosettaUI)
