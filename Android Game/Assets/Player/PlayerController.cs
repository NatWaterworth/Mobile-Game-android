using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Controls")]
    Touch startingPoint, endPoint;
    [SerializeField] bool drag, release;
    bool canMove, dead;

    Rigidbody2D rb;
    [SerializeField] float forceMultiplier;
    [SerializeField] float maxDragDistance;

    [Header("Camera Shake")]
    [SerializeField] float cameraMaxImpactOnDrag, cameraMaxImpactOnCollision, cameraMaxZoomOnRelease, cameraZoomOnDrag;

    [Header("Trigger Tags")]
    [SerializeField] string moveResetTag;
    [SerializeField] string enemyTag;

    [Header("Visual Effects")]
    [SerializeField] ParticleSystem deathSplatterEffect, hitEffect;
    [SerializeField] GameObject splatter;
    Sprite playerBodySprite;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (GetComponent<SpriteRenderer>() != null)
        {
           playerBodySprite = GetComponent<SpriteRenderer>().sprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
            UpdateTouchControl();
    }

    public void ResetPlayer()
    {
        if (GetComponent<SpriteRenderer>() != null)
        {
             GetComponent<SpriteRenderer>().sprite = playerBodySprite;
        }
        dead = false;
        rb.isKinematic = false;
        canMove = true;
        transform.parent = null;

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

            if (CameraController.instance != null)
            {
                CameraController.instance.ZoomCamera(Mathf.Clamp01((endPoint.position - startingPoint.position).magnitude / maxDragDistance) * cameraZoomOnDrag * Time.deltaTime);
            }
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
            CameraController.instance.ZoomCamera(Mathf.Clamp01((_endPoint - _startPoint).magnitude / maxDragDistance));
        }
        #endregion
    }

    void Stick(GameObject obj)
    {
        if (rb != null)
        {
            if (!IsDead())
            {
                transform.parent = obj.transform;
                //rb.isKinematic = true;
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;
                canMove = true;
            }
        }
    }

    void SpawnHitSplatter(Vector3 _hitPoint, Transform _parent)
    {
        GameObject _splatter = Instantiate(splatter, _hitPoint, Quaternion.Euler(0,0,Random.Range(0,360.0f)), null);
        _splatter.transform.parent = _parent;
        Destroy(_splatter, 5);
    }

    void UnStick()
    {
        if (rb != null)
        {
            transform.parent = null;
            rb.gravityScale = 1;
            canMove = false;
        }
    }

    bool IsDead()
    {
        return dead;
    }

    void Die()
    {
        if (dead)
            return;


        dead = true;
        UnStick();
        #region Death Visual Effects
        if (deathSplatterEffect != null)
        {
            deathSplatterEffect.Play();
        }

        if (GetComponent<SpriteRenderer>() != null)
        {
            GetComponent<SpriteRenderer>().sprite= null;
        }
        #endregion

        #region Camera Effect 
        if (CameraController.instance != null)
        {
            CameraController.instance.ShakeCamera(1);
        }
        #endregion

        if (GameManager.instance != null)
        {
            GameManager.instance.GameOver();
        }
        else Debug.LogError("No GameManager found!");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        #region Hit Effects
        if (CameraController.instance != null)
        {
            CameraController.instance.ShakeCamera(Mathf.Clamp01(collision.relativeVelocity.magnitude / cameraMaxImpactOnCollision));
        }

        if(hitEffect != null)
        {
            Vector3 dir = collision.transform.position - transform.position;
            hitEffect.transform.localEulerAngles = new Vector3(0,0,-Vector3.SignedAngle(Vector3.up,dir,Vector3.forward));
            hitEffect.Play();
        }

        if (splatter != null)
            SpawnHitSplatter(collision.GetContact(0).point, collision.transform);
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
        }
        #endregion
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        #region Game Over Check
        if (collision.gameObject.CompareTag(enemyTag))
        {
            Die();  
        }
        #endregion
    }
}
