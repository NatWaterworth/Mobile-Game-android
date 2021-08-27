using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Plant : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetRandomSprite(int _index)
    {
        if (spriteRenderer != null && PlantManager.instance!=null)
        {
            Sprite sprite = PlantManager.instance.GetRandomPlantSprite(_index);
            if (sprite != null)
                spriteRenderer.sprite = sprite;
            else
                Debug.LogWarning(this+": Sprite is Null.");
        }
    }


}
