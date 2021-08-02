using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that controls the motion of the camera in this 2D mobile platformer.
/// </summary>

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject objectToFollow;

    [SerializeField] float verticalLerpRatio;

    static CameraController instance;

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
    }

    // Update is called once per frame
    void LateUpdate()
    {
        FollowVertical();
    }

    void FollowVertical()
    {
        if (objectToFollow != null)
        {
            float newYPosition = Mathf.Lerp(transform.position.y, objectToFollow.transform.position.y, verticalLerpRatio*Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
        }
    }

    /// <summary>
    /// Shakes camera 
    /// </summary>
    /// <param name="_magnitude"></param>
    void ShakeCamera(float _magnitude)
    {
        StartCoroutine(Shake(_magnitude));
    }

    IEnumerator Shake(float _magnitude)
    {
        yield return null;
    }
}
