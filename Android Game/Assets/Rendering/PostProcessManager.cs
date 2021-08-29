using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class PostProcessManager : MonoBehaviour
{
    [SerializeField] Volume ppVolume;
    [SerializeField] float vingetteIntensityIncrease, vignetteIntensity, vignetteRateOfChange;
    //Post Processing Overrides
    Vignette vignette; //Adjusted when drag and release player
    ChromaticAberration chromaticAberration; //Coroutine on aquiring item
    MotionBlur motionBlur; //Always active. Adjust or disable in settings
    DepthOfField depthOfField; //Coroutine on aquiring an item.
    LensDistortion lensDistortion; //Coroutine on entering portal
    ColorAdjustments colorGrading; //Corotuine on aquiring an item.

    //Coroutines to adjust Post Processing Overrides at runtime
    Coroutine currentEffect, portalEffect, vignetteEffect;


    // Start is called before the first frame update
    void Start()
    {
        SetupPostProcessingVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Get all the overrides for the Post Processing volume to be altered by scripts.
    /// </summary>
    void SetupPostProcessingVolume()
    {
        ppVolume.profile.TryGet(out vignette);
        ppVolume.profile.TryGet(out chromaticAberration);
        ppVolume.profile.TryGet(out motionBlur);
        ppVolume.profile.TryGet(out depthOfField);
        ppVolume.profile.TryGet(out lensDistortion);
        ppVolume.profile.TryGet(out colorGrading);

        if (vignette!=null)
            vignetteIntensity = vignette.intensity.value;
    }

    public void StartColourEffect(float _duration)
    {

    }

    public void StartBlurredEffect(float _duration)
    {

    }

    public void StartPortalEffect(float _duration, Color _colour)
    {
        if (chromaticAberration != null && lensDistortion!=null && motionBlur!=null)
        {
            if (portalEffect != null)
                StopCoroutine(portalEffect);

            portalEffect = StartCoroutine(PortalEffect(_duration, _colour));
        }
    }

    IEnumerator PortalEffect(float _duration, Color _colour)
    {
        float _timestamp = Time.time;
        float _intensity = 0;
        motionBlur.active = true;

        while (_timestamp + _duration > Time.time)
        {
            _intensity =  1 - Mathf.Pow((Time.time - _timestamp)/ _duration,2);
            chromaticAberration.intensity.value = _intensity;
            lensDistortion.intensity.value = -_intensity;
            colorGrading.colorFilter.value = Color.Lerp(Color.white, _colour, _intensity);
            yield return new WaitForEndOfFrame();
        }
        chromaticAberration.intensity.value = 0;
        lensDistortion.intensity.value = 0;
        colorGrading.colorFilter.value = Color.white;
        motionBlur.active = false;
    }


    public void StartVingetteEffect()
    {
        if (vignette != null)
        {
            if (vignetteEffect != null)
                StopCoroutine(vignetteEffect);

            vignetteEffect = StartCoroutine(VignetteEffectOn());
        }
    }



    public void EndVingetteEffect()
    {
        if (vignette != null)
        {
            if (vignetteEffect != null)
                StopCoroutine(vignetteEffect);

            vignetteEffect = StartCoroutine(VignetteEffectOff());
        }
    }

    IEnumerator VignetteEffectOn()
    {
        while (vignette.intensity.value < vignetteIntensity + vingetteIntensityIncrease)
        {
            vignette.intensity.value += (vingetteIntensityIncrease * vignetteRateOfChange);
            yield return new WaitForEndOfFrame();
        }
        vignette.intensity.value = vignetteIntensity + vingetteIntensityIncrease;
    }

    IEnumerator VignetteEffectOff()
    {
        while (vignette.intensity.value > vignetteIntensity)
        {
            vignette.intensity.value -= (vingetteIntensityIncrease * vignetteRateOfChange);
            yield return new WaitForEndOfFrame();
        }
        vignette.intensity.value = vignetteIntensity;
    }

}
