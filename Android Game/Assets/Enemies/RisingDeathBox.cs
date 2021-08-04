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


    void RisingObject()
    {
        float distance =  targetObject.transform.position.y - transform.position.y;
        float speedMultiplier = Mathf.Lerp(1, maxMultiplier, Mathf.InverseLerp(0, timeToReachMaxSpeed, Time.time - startingTime));
        transform.position += new Vector3(0, Mathf.Lerp(risingSpeed, maxRisingSpeed, Mathf.InverseLerp(lowerDistanceFromPlayer,maxDistanceFromPlayer, distance)), 0)* speedMultiplier * Time.deltaTime;
    }
}
