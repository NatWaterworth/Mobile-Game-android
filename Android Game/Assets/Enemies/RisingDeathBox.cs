using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingDeathBox : MonoBehaviour
{
    [Header("Default Speed")]
    [SerializeField] float risingSpeed, maxRisingSpeed;
    [SerializeField] float lowerDistanceFromPlayer, maxDistanceFromPlayer;

    [Header("Speed Over Time")]
    [SerializeField] float maxMultiplier;
    [SerializeField] float timeToReachMaxSpeed;
    float startingTime;

    [SerializeField] GameObject targetObject;

    [Header("Splash")]
    [SerializeField] ParticleSystem splashEffect;
    [SerializeField] float splashVelocity, maxSplashVelocity, minSplashSize, maxSplashSize;
    [SerializeField] Vector3 splashOffset;
    // Start is called before the first frame update
    void Start()
    {
        StartRisingDeathBox();
    }

    // Update is called once per frame
    void Update()
    {
        RisingObject();
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


    void RisingObject()
    {
        float distance =  targetObject.transform.position.y - transform.position.y;
        float speedMultiplier = Mathf.Lerp(1, maxMultiplier, Mathf.InverseLerp(0, timeToReachMaxSpeed, Time.time - startingTime));
        transform.position += new Vector3(0, Mathf.Lerp(risingSpeed, maxRisingSpeed, Mathf.InverseLerp(lowerDistanceFromPlayer,maxDistanceFromPlayer, distance)), 0)* speedMultiplier * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(collision.attachedRigidbody.velocity.magnitude > splashVelocity)
            {
                Debug.Log("Splash : " + collision.attachedRigidbody.velocity.magnitude);
                float _size = Mathf.Lerp(minSplashSize, maxSplashSize, Mathf.InverseLerp(splashVelocity, maxSplashVelocity, collision.attachedRigidbody.velocity.magnitude));
                Splash(_size);
            }
               
        }
    }
}
