using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that controls the motion of the camera in this 2D mobile platformer.
/// </summary>

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject objectToFollow;

    [SerializeField] float verticalLerpRatio, horizontalLerpRatio;

    [Header("Camera Shake")]
    [SerializeField] float maxAngleShake;
    [SerializeField] float maxXOffsetShake,maxYOffsetShake;
    [SerializeField] float traumaDampening, traumaExponent;
    float trauma, shake;
    Vector3 defaultCameraPosition;
    Vector3 defaultCameraRotation;

    public static CameraController instance;
    float startingXPos;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("More than 1 instance of Camera Controller exists.");
            Destroy(gameObject);
        }
        startingXPos = transform.position.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Follow();
        ReduceTraumaOverTime();
        ApplyShakeToCamera();
    }

    void Follow()
    {
        if (objectToFollow != null)
        {
            float newYPosition = Mathf.Lerp(transform.position.y, objectToFollow.transform.position.y, verticalLerpRatio*Time.deltaTime);
            float newXPosition = Mathf.Lerp(transform.position.x, startingXPos, horizontalLerpRatio * Time.deltaTime);
            defaultCameraPosition = new Vector3(newXPosition, newYPosition, transform.position.z);
        }
        transform.position = defaultCameraPosition;
    }

    void ReduceTraumaOverTime()
    {
        trauma -= Time.deltaTime * traumaDampening;
        trauma = Mathf.Clamp01(trauma);
    }

    void ApplyShakeToCamera()
    {
        shake = Mathf.Pow(trauma, traumaExponent);
        transform.position += new Vector3(Random.Range(-1.0f, 1.0f) * maxXOffsetShake, Random.Range(-1.0f, 1.0f) * maxYOffsetShake, 0)* shake;
    }

    /// <summary>
    /// Shakes camera 
    /// </summary>
    /// <param name="_addedTrauma"> a measure of how massive the camera shake is on a scale of 0 -> 1.</param>
    public void ShakeCamera(float _addedTrauma)
    {
        trauma += _addedTrauma;
        trauma = Mathf.Clamp01(trauma);
    }

}