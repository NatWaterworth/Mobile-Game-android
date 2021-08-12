using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingDeathBox : MonoBehaviour
{
    Vector3 startingPosition;

    public enum DeathBoxState
    {
        Game,
        Transition
    }
    [SerializeField] DeathBoxState currentState;

    [Header("Default Speed")]
    [SerializeField] float risingSpeed, maxRisingSpeed;
    [SerializeField] float lowerDistanceFromPlayer, maxDistanceFromPlayer;
    [SerializeField] float transitionSpeed, minTransitionDistance, maxTransitionDistance;

    [Header("Speed Over Time")]
    [SerializeField] float maxMultiplier;
    [SerializeField] float timeToReachMaxSpeed;
    float startingTime;

    [SerializeField] GameObject targetObject;
    [SerializeField] float transitionTargetDistance;
    float transitionTargetHeight;

    [Header("Splash")]
    [SerializeField] ParticleSystem splashEffect;
    [SerializeField] float splashVelocity, maxSplashVelocity, minSplashSize, maxSplashSize;
    [SerializeField] Vector3 splashOffset;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        StartRisingDeathBox();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState.Equals(DeathBoxState.Game))
            RisingObject();
        else
            Transition();
    }

    void StartRisingDeathBox()
    {
        startingTime = Time.time;
    }

    void Splash(float _size)
    {
        if (splashEffect != null && targetObject !=null)
        {
            splashEffect.transform.position = targetObject.transform.position + splashOffset;
            splashEffect.transform.localScale = Vector3.one * _size;
            splashEffect.Play();
        }
    }

    public void RestartRisingDeathBox()
    {
        StartRisingDeathBox();
        transform.position = startingPosition;
        if (GetComponent<Collider>() != null)
            GetComponent<Collider>().enabled = true;
        currentState = DeathBoxState.Game;
    }

    void RisingObject()
    {
        float distance =  targetObject.transform.position.y - transform.position.y;
        float speedMultiplier = Mathf.Lerp(1, maxMultiplier, Mathf.InverseLerp(0, timeToReachMaxSpeed, Time.time - startingTime));
        transform.position += new Vector3(0, Mathf.Lerp(risingSpeed, maxRisingSpeed, Mathf.InverseLerp(lowerDistanceFromPlayer,maxDistanceFromPlayer, distance)), 0)* speedMultiplier * Time.deltaTime;
    }

    void Transition()
    {
        float distance = Mathf.Max(0, targetObject.transform.position.y + transitionTargetHeight - (2 * transform.position.y));
        transform.position += new Vector3(0, Mathf.Lerp(0, transitionSpeed, Mathf.InverseLerp(minTransitionDistance,maxTransitionDistance, distance)), 0) * Time.deltaTime;
    }

    public void TransitionWater()
    {
        currentState = DeathBoxState.Transition;
        if (GetComponent<Collider>() != null)
            GetComponent<Collider>().enabled = false;
        transitionTargetHeight = transitionTargetDistance + transform.position.y;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(collision.attachedRigidbody.velocity.magnitude > splashVelocity)
            {
                float _size = Mathf.Lerp(minSplashSize, maxSplashSize, Mathf.InverseLerp(splashVelocity, maxSplashVelocity, collision.attachedRigidbody.velocity.magnitude));
                Splash(_size);
            }
               
        }
    }
}
