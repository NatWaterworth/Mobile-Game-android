using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class PostProcessManager : MonoBehaviour
{
    [SerializeField] Volume ppVolume;
    [SerializeField] float vingetteIntensityIncrease, vignetteIntensity;
    //Post Processing Overrides
    Vignette vignette; //Adjusted when drag and release player
    ChromaticAberration chromaticAberration; //Coroutine on aquiring item
    MotionBlur motionBlur; //Always active. Adjust or disable in settings
    DepthOfField depthOfField; //Coroutine on aquiring an item.
    LensDistortion lensDistortion; //Coroutine on entering portal
    ColorAdjustments colorGrading; //Corotuine on aquiring an item.

    //Coroutines to adjust Post Processing Overrides at runtime
    Coroutine currentEffect, portalEffect, vingetteEffect;


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

    public void StartPortalEffect(float _duration)
    {

    }

    public void StartVingetteEffect()
    {
        if (vignette != null)
            vignette.intensity.value = vignetteIntensity + vingetteIntensityIncrease;
    }

    public void EndVingetteEffect()
    {
        if(vignette!=null)
            vignette.intensity.value = vignetteIntensity;
    }

}
