using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Touch startingPoint, endPoint;
    [SerializeField] bool drag, release;
    bool canMove, dead;

    Rigidbody2D rb;
    [SerializeField] float forceMultiplier;
    [SerializeField] float maxDragDistance;
    [SerializeField] float cameraMaxImpactOnDrag, cameraMaxImpactOnCollision;

    [Header("Trigger Tags")]
    [SerializeField] string moveResetTag;
    [SerializeField] string enemyTag;
    

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
        UnStick();

        Vector3 _startPoint = Camera.main.ScreenToWorldPoint(startingPoint.position);
        Vector3 _endPoint = Camera.main.ScreenToWorldPoint(endPoint.position);

        Vector3 _force = Vector3.ClampMagnitude(_endPoint - _startPoint, maxDragDistance) * forceMultiplier;
        if (rb != null)
            rb.velocity = _force;

        #region Camera Effect 
        if (CameraController.instance != null)
        {
            CameraController.instance.ShakeCamera(Mathf.Clamp01((_endPoint - _startPoint).magnitude / maxDragDistance) * cameraMaxImpactOnDrag);
        }
        #endregion
    }

    void Stick(GameObject obj)
    {
        if (rb != null)
        {
            transform.parent = obj.transform;
            rb.isKinematic = true;

            if(!IsDead())
                canMove = true;
        }
    }

    void UnStick()
    {
        if (rb != null)
        {
            transform.parent = null;
            rb.isKinematic = false;
            canMove = false;
        }
    }

    bool IsDead()
    {
        return dead;
    }

    void Die()
    {
        dead = true;

        #region Death Visual Effects

        #endregion
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        #region Camera Effect 
        if (CameraController.instance != null)
        {
            CameraController.instance.ShakeCamera(Mathf.Clamp01(collision.relativeVelocity.magnitude / cameraMaxImpactOnCollision));
        }
        #endregion

        #region Move Check
        if (collision.gameObject.CompareTag(moveResetTag))
        {
            Stick(collision.gameObject);           
        }
        #endregion

        #region Game Over Check
        if (collision.gameObject.CompareTag(enemyTag))
        {
            Die();

            if (GameManager.instance != null)
            {
                GameManager.instance.GameOver();
            }
            else Debug.LogError("No GameManager found!");
        }
        #endregion
    }
}