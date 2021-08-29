using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Portal : MonoBehaviour
{
    LevelManager levelManager;
    SpriteRenderer spriteRenderer;
    Light2D light2D;
    [SerializeField] List<Sprite> sprites;

    [System.Serializable]
    struct LightSettings
    {
        public Color color;
        public float intensity;
    }
    [SerializeField] float lightIntensity, lightIntensityDeviation, lightCycleSpeed;
    [SerializeField] List<LightSettings> lightSettings;

    [Header("Portal Idle Movement")]
    [SerializeField] LeanTweenType movementType, scalingType;
    [SerializeField] float cycleTime, maxScale, yMovement, yDelay;
    [SerializeField] float portalDelay;
    [SerializeField] ParticleSystem aquireEffect;
    Coroutine nextLevelCoroutine;
    private void Awake()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null)
            col.isTrigger = true;
        else
            Debug.LogWarning(this + " needs a Collider2D to function.");

        spriteRenderer = GetComponent<SpriteRenderer>();
        light2D = GetComponent<Light2D>();

        Wobble();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(nextLevelCoroutine ==null)
                nextLevelCoroutine = StartCoroutine(StartNextLevel());
        }
            
    }

    IEnumerator StartNextLevel()
    {
        spriteRenderer.sprite = null;

        if (aquireEffect != null)
            aquireEffect.Play();

        yield return new WaitForSeconds(portalDelay);

        if (levelManager != null)
            levelManager.StartNextLevel();

        nextLevelCoroutine = null;

    }

    public void SetLevelManager(LevelManager _levelManager)
    {
        levelManager = _levelManager;
    }

    private void Update()
    {
        if (light2D != null)
            light2D.intensity = lightIntensity + (Mathf.Sin(Time.time* lightCycleSpeed) * lightIntensityDeviation);

    }

    void Wobble()
    {
        LeanTween.scale(gameObject, transform.localScale + (Vector3.one * maxScale),cycleTime).setEase(scalingType).setLoopPingPong(); //.setIgnoreTimeScale(false)
        LeanTween.moveLocalY(gameObject, transform.localPosition.y + yMovement, cycleTime).setEase(movementType).setDelay(yDelay).setLoopPingPong();
    }

    public void SetPortalVisual(int _visualIndex)
    {
        if (spriteRenderer != null && sprites!=null && sprites.Count > 0)
        {
            spriteRenderer.sprite = sprites[_visualIndex % sprites.Count];
        }

        if (light2D != null && lightSettings != null && lightSettings.Count > 0)
        {
            light2D.color = lightSettings[_visualIndex % sprites.Count].color;
            lightIntensity = lightSettings[_visualIndex % sprites.Count].intensity;
        }
    }

    public Color GetPortalColour(int _visualIndex)
    {
        return lightSettings[_visualIndex % sprites.Count].color;
    }
}
