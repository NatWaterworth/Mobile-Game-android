using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/// <summary>
/// Controls the global lighting of the level based.
/// </summary>
[RequireComponent(typeof(Light2D))]
public class LightingManager : MonoBehaviour
{
    [System.Serializable]
    struct LightSettings
    {
        public Color color;
        public float intensity;
    }

    [SerializeField] List<LightSettings> lightSettings;
    Light2D globalLight;

    private void Awake()
    {
        globalLight = GetComponent<Light2D>();
        globalLight.lightType = Light2D.LightType.Global;
    }

    /// <summary>
    /// Sets the lighting to match the visual Index. E.g. Visual index of 4 may be night which is a dark and gloomy setting.
    /// </summary>
    /// <param name="visualIndex">Represents visual setting for the lighting.</param>
    public void SetGlobalLighting(int visualIndex)
    {
        if (globalLight == null || lightSettings == null || lightSettings.Count == 0)
            return;

        visualIndex %= lightSettings.Count;
        SetLightingValues(lightSettings[visualIndex]);
    }

    void SetLightingValues(LightSettings _setting)
    {
        globalLight.intensity = _setting.intensity;
        globalLight.color = _setting.color;
    }
}
