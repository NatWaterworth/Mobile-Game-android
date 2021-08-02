using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Touch startingPoint, endPoint;
    [SerializeField] bool drag, release, canMove;

    Rigidbody2D rb;
    [SerializeField] float forceMultuplier;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
            UpdateTouchControl();
    }


    void UpdateTouchControl()
    {
        if (Input.touchCount > 0)
        {
            if (!drag)
            {
                startingPoint = Input.touches[0];
                drag = true;
            }
            endPoint = Input.touches[0];
        }
        else if (drag)
        {
            Release();
            drag = false;
        }

    }

    void Release()
    {
        Vector3 _startPoint = Camera.main.ScreenToWorldPoint(startingPoint.position);
        Vector3 _endPoint = Camera.main.ScreenToWorldPoint(endPoint.position);

        Vector3 _force = (_endPoint - _startPoint) * forceMultuplier;
        if (rb != null)
            rb.velocity = _force;
        //canMove = false;
    }
}
