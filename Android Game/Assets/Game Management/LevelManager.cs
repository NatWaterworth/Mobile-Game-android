using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage which levels spawn and dicate the score based on level progress.
/// 
/// Requires: 
///  - Setting start and end point.
///  - Giving a score based on distance travelled between those points.
///  - Location of player.
/// </summary>
public class LevelManager : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] Transform startpoint; // These will just be portals.
    float levelProgress;

    [SerializeField] RisingDeathBox risingWater;
    [SerializeField] LevelPartDeactivator levelPartDeactivator;

    [Header("Level Parts")]
    [SerializeField] List<LevelPart> boundaryLevelParts;
    [SerializeField] List<LevelPart> enemyLevelParts;
    [SerializeField] LevelPart ground, roof;

    [Header("Level Options")]
    [SerializeField] [Range(0.0f, 1f)] float enemySpawnChance;
    [SerializeField] bool generateLevel; 
    [SerializeField] float levelPartSize;
    [SerializeField] float levelHeight;
    [SerializeField] [Range(0.1f,10f)]float levelScale;
    [SerializeField] int levelVisualIndex;

    int activatedTimes = 1;
    const int levelPartsActiveAnytime = 4; //The number of active parts at any point  

    // Start is called before the first frame update
    void Start()
    {
        levelHeight = Mathf.Max(levelPartSize * levelPartsActiveAnytime, levelHeight);
        levelPartDeactivator.SetLevelManager(this);

        

        SpawnLevelAssets();
        if (generateLevel)
            GenerateInitialLevel();
        SetLevelVisuals();
    }

    /// <summary>
    /// Create the first part of the level from smaller level pieces.
    /// </summary>
    void GenerateInitialLevel()
    {
        //Spawn Ground
        ground.SetupAsset(startpoint.position, Quaternion.identity, Vector3.one * levelScale);

        //Spawn Starting Tiles
        for (int i = 0; i < levelPartsActiveAnytime; i++)
        {
            SpawnLevelPart(false);
        }

        //Spawn Roof
        float _height = levelScale * levelPartSize * Mathf.CeilToInt(levelHeight / (levelScale * levelPartSize));
        roof.SetupAsset(startpoint.position + new Vector3(0, _height, 0), Quaternion.identity, Vector3.one * levelScale);
    }
    /// <summary>
    /// Spawns a part of the level.
    /// </summary>
    /// <param name="canHaveEnemy">Determines if level part spawned can contain an enemy.</param>
    public void SpawnLevelPart(bool canHaveEnemy)
    {
        if (activatedTimes * levelPartSize * levelScale > levelHeight)
            return;

        Vector3 _position = startpoint.position + new Vector3(0, levelPartSize * levelScale * activatedTimes, 0);
        Quaternion _rotation = Quaternion.identity;
        Vector3 _scale = Vector3.one * levelScale;

        float chance = Random.Range(0.0f, 1.0f);
        int choice = 0;
        if (canHaveEnemy)
        {
            if (chance < enemySpawnChance) //try to get part if it's active, get the next one.
            {

                while (enemyLevelParts[Mathf.FloorToInt((chance * enemyLevelParts.Count) + choice) % enemyLevelParts.Count].InUse() || choice > 10)
                    choice++;
                enemyLevelParts[Mathf.FloorToInt((chance * enemyLevelParts.Count) + choice) % enemyLevelParts.Count].SetupAsset(_position, _rotation, _scale);

                if (choice > 10)
                    Debug.LogError("Choice too high, probably an error occuring.");
            }
            else
            {
                while (boundaryLevelParts[Mathf.FloorToInt(((chance - enemySpawnChance) / (1 - enemySpawnChance) * boundaryLevelParts.Count) + choice) % boundaryLevelParts.Count].InUse() || choice > 10)
                    choice++;
                boundaryLevelParts[Mathf.FloorToInt(((chance - enemySpawnChance) / (1 - enemySpawnChance) * boundaryLevelParts.Count) + choice) % boundaryLevelParts.Count].SetupAsset(_position, _rotation, _scale);
            }
        }
        else
        {
            while (boundaryLevelParts[Mathf.FloorToInt((chance * boundaryLevelParts.Count) + choice) % boundaryLevelParts.Count].InUse() || choice > 10)
                choice++;
            boundaryLevelParts[Mathf.FloorToInt((chance * boundaryLevelParts.Count)+choice)%boundaryLevelParts.Count].SetupAsset(_position, _rotation, _scale);
        }

        activatedTimes++;
    }

    void SpawnLevelAssets()
    {
        if (boundaryLevelParts == null || enemyLevelParts == null)
        {
            Debug.LogError("No parts to build level from!");
            return;
        }

        LevelPart _instance;

        for (int i = 0; i < boundaryLevelParts.Count; i++)
        {
            _instance = Instantiate(boundaryLevelParts[i], transform);
            boundaryLevelParts[i] = _instance;
            boundaryLevelParts[i].DisableAsset();
        }

        for (int i = 0; i < enemyLevelParts.Count; i++)
        {
            _instance = Instantiate(enemyLevelParts[i], transform);
            enemyLevelParts[i] = _instance;
            enemyLevelParts[i].DisableAsset();
        }
        if (ground == null || roof == null)
        {
            Debug.LogError("Ground AND/OR Roof has not been set!");
            return;
        }

        _instance = Instantiate(ground, transform);
        ground = _instance;
        ground.DisableAsset();

        _instance = Instantiate(roof, transform);
        roof = _instance;
        roof.DisableAsset();

    }

    void DisableLevelAssets()
    {
        if (boundaryLevelParts == null || enemyLevelParts == null)
        {
            Debug.LogError("No parts to build level from!");
            return;
        }

        for (int i = 0; i < boundaryLevelParts.Count; i++)
        {
            boundaryLevelParts[i].DisableAsset();
        }

        for (int i = 0; i < enemyLevelParts.Count; i++)
        {
            enemyLevelParts[i].DisableAsset();
        }
        if (ground == null || roof == null)
        {
            Debug.LogError("Ground AND/OR Roof has not been set!");
            return;
        }

        ground.DisableAsset();
        roof.DisableAsset();

    }


    /// <summary>
    /// Return the increase in progress for the current level.
    /// </summary>
    /// <returns></returns>
    public float GetLevelProgress()
    {
        float progress = Mathf.Clamp01( Mathf.InverseLerp(startpoint.position.y, startpoint.position.y + levelHeight, player.transform.position.y) - levelProgress);
        levelProgress += progress;
        return Mathf.Max(0, progress);
    }

    public void RestartLevel()
    {
        if(risingWater!=null)
            risingWater.RestartRisingDeathBox();
        if (player != null)
        {
            player.transform.position = startpoint.transform.position;
            player.ResetPlayer();
        }

        activatedTimes = 1;

        DisableLevelAssets();
        GenerateInitialLevel();

        levelProgress = 0;

    }

    public void LevelTransition()
    {
        if (risingWater != null)
            risingWater.TransitionWater();
    }

    public void SetLevelVisuals()
    {

        VisualModifier[] visualModifiers = FindObjectsOfType<VisualModifier>(true);

        foreach(VisualModifier vis in visualModifiers)
        {
            vis.SetSprite(levelVisualIndex);
        }

        Plant[] plants = FindObjectsOfType<Plant>(true);

        foreach (Plant plant in plants)
        {
            plant.SetRandomSprite(levelVisualIndex);
        }

    }
}
