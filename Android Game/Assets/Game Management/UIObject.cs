using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UIObject : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] LeanTweenType easeType;
    [SerializeField] float delay;
    [SerializeField] float moveX;


    private void OnEnable()
    {
        if (GetComponent<RectTransform>() != null)
        {
            GetComponent<RectTransform>().position -= new Vector3(moveX,0,0);
            LeanTween.moveX(gameObject, GetComponent<RectTransform>().position.x +moveX, duration).setDelay(delay).setEase(easeType).setIgnoreTimeScale(true);
        }
    }

}
