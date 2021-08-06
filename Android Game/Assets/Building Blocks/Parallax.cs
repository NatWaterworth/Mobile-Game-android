using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float parallaxX, parallaxY;
    [SerializeField] GameObject targetToFollow;
    Vector2 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ParallaxEffect();
    }

    void ParallaxEffect()
    {
        float xPos = Mathf.Lerp(startingPos.x, targetToFollow.transform.position.x, parallaxX);
        float yPos = Mathf.Lerp(startingPos.y, targetToFollow.transform.position.y, parallaxY);
        transform.position = new Vector3(xPos, yPos, transform.position.z);
    }
}
