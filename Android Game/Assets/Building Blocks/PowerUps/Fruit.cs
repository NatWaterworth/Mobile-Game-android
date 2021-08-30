using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Fruit : MonoBehaviour, IPooledAsset
{

    [System.Serializable]
    struct FruitPowerUp
    {
        public Sprite sprite;
        public int scoreChange;
        public Color effectColour;
    }

    [Header("Fruit Info")]
    SpriteRenderer spriteRenderer;
    [SerializeField] List<FruitPowerUp> fruitTypes;
    [SerializeField] int activeFruit;
    Collider2D collider;


    [Header("Visual Effects")]
    [SerializeField] ParticleSystem acquireEffect;
    [SerializeField] PostProcessManager ppManager;
    [SerializeField] float activeTimeAfterAcquisition;
    Light2D light2D;


    [Header("Idle Movement")]
    [SerializeField] LeanTweenType movementType, scalingType;
    [SerializeField] float cycleTime, maxScale;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.isTrigger = true;


        light2D = GetComponent<Light2D>();
        SetupAsset(transform.position, Quaternion.identity, transform.localScale);
        Wobble();
    }

    public void SetPostProcessManager(PostProcessManager _manager)
    {
        ppManager = _manager;
    }

    void Wobble()
    {
        LeanTween.scale(gameObject, transform.localScale + (Vector3.one * maxScale), cycleTime).setEase(scalingType).setLoopPingPong();
    }


    void TriggerEffect()
    {
        if (fruitTypes == null || fruitTypes.Count == 0)
            return;

        if (collider == null || light2D == null || spriteRenderer == null)
            return;

        StartCoroutine(FruitEffect());
    }

    IEnumerator FruitEffect()
    {       
        if (acquireEffect != null)
            acquireEffect.Play();

        if (GameManager.instance != null)
            GameManager.instance.IncreaseScore(fruitTypes[activeFruit].scoreChange);

        TriggerPostProcessEffect();

        collider.enabled = false;
        spriteRenderer.enabled = false;
        light2D.enabled = false;
        yield return new WaitForSeconds(activeTimeAfterAcquisition);

        collider.enabled = true;
        spriteRenderer.enabled = true;
        light2D.enabled = true;
        DisableAsset();
    }

    void TriggerPostProcessEffect()
    {
        if (ppManager == null)
            return;

        ppManager.StartColourHumEffect(activeTimeAfterAcquisition, fruitTypes[activeFruit % fruitTypes.Count].effectColour); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            TriggerEffect();
    }

    public void SetupAsset(Vector3 _position, Quaternion _rotation, Vector3 _localScale)
    {
        gameObject.SetActive(true);

        if (fruitTypes != null && fruitTypes.Count > 0)
        {
            activeFruit = Random.Range(0, fruitTypes.Count);

            spriteRenderer.sprite = fruitTypes[activeFruit].sprite;

            if(light2D!=null)
                light2D.color = fruitTypes[activeFruit].effectColour;
        }

        transform.position = _position;
        transform.rotation = _rotation;
        transform.localScale = _localScale;
    }

    public void DisableAsset()
    {
        gameObject.SetActive(false);
    }

    public bool InUse()
    {
        return gameObject.activeSelf;
    }
}
