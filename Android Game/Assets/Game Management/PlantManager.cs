using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public static PlantManager instance;

    [SerializeField] List<Sprite> sprites;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("More than 1 isntance of Plant Manager");
            Destroy(gameObject);
        }
    }

    public Sprite GetRandomPlantSprite()
    {
        if (sprites.Count > 0)
        {
            return sprites[Random.Range(0, sprites.Count)];
        }
        return null;
    }
}
