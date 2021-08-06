using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitEnemy : MonoBehaviour
{

    [SerializeField] Transform startPoint, EndPoint;
    [SerializeField] EnemyBehaviour behaviour;
    [SerializeField] float cycleTime;
    [SerializeField] float cycleOffset;
    public enum EnemyBehaviour
    {
        Loop,
        PingPong
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBehaviour();
    }


    void UpdateBehaviour()
    {
        switch (behaviour)
        {
            case EnemyBehaviour.Loop:
                Loop();
                break;

            case EnemyBehaviour.PingPong:
                PingPong();
                break;
        
        }
    }

    void Loop()
    {
        if(startPoint !=null && EndPoint != null)
            transform.position = Vector3.Lerp(startPoint.position, EndPoint.position, ((Time.time+ cycleOffset) / cycleTime) % 1);
    }

    void PingPong()
    {
        if (startPoint != null && EndPoint != null)
            transform.position = Vector3.Lerp(startPoint.position, EndPoint.position, (1 + Mathf.Sin((Time.time+ cycleOffset) / cycleTime))/2);
    }
}

