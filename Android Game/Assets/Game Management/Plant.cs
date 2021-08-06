using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Plant : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetRandomSprite();
    }

    void SetRandomSprite()
    {
        if (GetComponent<SpriteRenderer>() != null && PlantManager.instance!=null)
        {
            Sprite sprite = PlantManager.instance.GetRandomPlantSprite();
            if (sprite != null)
                GetComponent<SpriteRenderer>().sprite = sprite;
            else
                Debug.LogWarning("Sprite is Null");
        }
    }

}
