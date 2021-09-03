using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform startPoint, EndPoint;
    [SerializeField] float cycleTime;
    [SerializeField] float cycleOffset;



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
        PingPong();
    }


    void PingPong()
    {
        if (startPoint != null && EndPoint != null)
            transform.position = Vector3.Lerp(startPoint.position, EndPoint.position, (1 + Mathf.Sin((Time.time + cycleOffset) / cycleTime)) / 2);
    }

    public void SetCycleTime(float _cycle)
    {
        cycleTime = _cycle;
    }
}
