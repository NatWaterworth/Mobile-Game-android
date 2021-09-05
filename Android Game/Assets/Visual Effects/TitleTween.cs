using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class TitleTween : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] LeanTweenType easeType;
    [SerializeField] float delay;
    [SerializeField] float moveY;


    private void OnEnable()
    {
        if (GetComponent<RectTransform>() != null)
        {           
            //GetComponent<RectTransform>().position -= new Vector3(0, moveY, 0);
            LeanTween.moveY(gameObject.GetComponent<RectTransform>(),  moveY, duration).setEase(easeType).setIgnoreTimeScale(true).setLoopPingPong().setDelay(delay);
          
        }
    }

    void OnDisable()
    {
        LeanTween.reset();
    }

}

