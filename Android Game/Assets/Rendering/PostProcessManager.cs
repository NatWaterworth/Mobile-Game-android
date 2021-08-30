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
    ColorAdjustments colourAdjustment; //Corotuine on aquiring an item.

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
        ppVolume.profile.TryGet(out colourAdjustment);

        if (vignette!=null)
            vignetteIntensity = vignette.intensity.value;
    }

    public void StartRainbowEffect(float _duration, float _cooldown, float _rate, float _lensSpeed)
    {
        if (colourAdjustment != null)
        {
            if (currentEffect != null)
                StopCoroutine(portalEffect);

            colourAdjustment.colorFilter.value = Color.white;

            currentEffect = StartCoroutine(HueChangeEffect(_duration, _cooldown, _rate, _lensSpeed));
        }
    }

    public void StartBlurredEffect(float _duration)
    {

    }

    public void StartPortalEffect(float _duration, Color _colour)
    {
        if (chromaticAberration != null && lensDistortion!=null && motionBlur!=null && colourAdjustment != null)
        {
            if (portalEffect != null)
                StopCoroutine(portalEffect);

            portalEffect = StartCoroutine(PortalEffect(_duration, _colour));
        }
    }

    public void StartColourHumEffect(float _duration, Color _colour)
    {
        if (lensDistortion != null && colourAdjustment!=null)
        {
            if (portalEffect != null)
                StopCoroutine(portalEffect);

            portalEffect = StartCoroutine(ColourHumEffect(_duration, _colour));
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
            colourAdjustment.colorFilter.value = Color.Lerp(Color.white, _colour, _intensity);
            yield return new WaitForEndOfFrame();
        }
        chromaticAberration.intensity.value = 0;
        lensDistortion.intensity.value = 0;
        colourAdjustment.colorFilter.value = Color.white;
        motionBlur.active = false;
    }

    IEnumerator ColourHumEffect(float _duration, Color _colour)
    {
        float _timestamp = Time.time;
        float _intensity = 0;

        while (_timestamp + _duration > Time.time)
        {
            _intensity = 1 - Mathf.Pow((Time.time - _timestamp) / _duration, 2);
            lensDistortion.intensity.value = -_intensity*.5f;
            colourAdjustment.colorFilter.value = Color.Lerp(Color.white, _colour, _intensity);
            yield return new WaitForEndOfFrame();
        }
        colourAdjustment.colorFilter.value = Color.white;
        lensDistortion.intensity.value = 0;
    }

    IEnumerator HueChangeEffect(float _duration,float _cooldown, float _colourRate, float _lensSpeed)
    {
        float _timestamp = Time.time;
        float _intensity = 0;

        while (_timestamp + _duration > Time.time)
        {
            _intensity += _colourRate;
            colourAdjustment.hueShift.value = Mathf.Lerp(-180, 180, _intensity%1);
            lensDistortion.intensity.value = 0.5f * Mathf.Sin(_intensity* _lensSpeed);
            yield return new WaitForEndOfFrame();
        }

        float _hueValue = colourAdjustment.hueShift.value;
        float _lensValue = lensDistortion.intensity.value;
        _timestamp = Time.time;

        while (_timestamp + _cooldown > Time.time)
        {
            _intensity =  1 - Mathf.Pow((Time.time - _timestamp)/ _cooldown, 2);
            colourAdjustment.hueShift.value = Mathf.Lerp(0, _hueValue, _intensity);
            lensDistortion.intensity.value = Mathf.Lerp(0, _lensValue, _intensity);
            yield return new WaitForEndOfFrame();
        }
        colourAdjustment.hueShift.value = 0;
    }

    public void CancelEffect()
    {
        if (portalEffect != null)
            StopCoroutine(portalEffect);
        if (vignetteEffect != null)
            StopCoroutine(vignetteEffect);
        if (currentEffect != null)
            StopCoroutine(currentEffect);

        if (colourAdjustment != null)
        {
            colourAdjustment.colorFilter.value = Color.white;
            colourAdjustment.hueShift.value = 0;
        }
        if (lensDistortion != null)
            lensDistortion.intensity.value = 0;
        if (chromaticAberration !=null)
            chromaticAberration.intensity.value = 0;
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
