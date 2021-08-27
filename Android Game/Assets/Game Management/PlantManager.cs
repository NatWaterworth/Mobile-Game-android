using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public static PlantManager instance;

    [SerializeField] List<Sprite> sprites;
    [SerializeField] List<Sprite> summerSprites;
    [SerializeField] List<Sprite> autumnSprites;
    [SerializeField] List<Sprite> nightSprites;
    [SerializeField] List<Sprite> corruptedSprites;

    const int numberOfPlantSeasons = 5;
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

    public Sprite GetRandomPlantSprite(int _seasonIndex)
    {
        switch (_seasonIndex % numberOfPlantSeasons) //number of lists
        {
            case 0:
                if (sprites.Count > 0)
                {
                    return sprites[Random.Range(0, sprites.Count)];
                }
                return null;
            case 1:
                if (summerSprites.Count > 0)
                {
                    return summerSprites[Random.Range(0, summerSprites.Count)];
                }
                return null;
            case 2:
                if (autumnSprites.Count > 0)
                {
                    return autumnSprites[Random.Range(0, autumnSprites.Count)];
                }
                return null;
            case 3:
                if (nightSprites.Count > 0)
                {
                    return nightSprites[Random.Range(0, nightSprites.Count)];
                }
                return null;
            case 4:
                if (corruptedSprites.Count > 0)
                {
                    return corruptedSprites[Random.Range(0, corruptedSprites.Count)];
                }
                return null;
        }
        return null;
    }
}
