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
    [SerializeField] Fruit fruit;

    [Header("Level Options")]
    [SerializeField] [Range(0.0f, 1f)] float enemySpawnChance;
    [SerializeField] [Range(0.0f, 1f)] float fruitSpawnChance;
    [SerializeField] float levelPartRangeIncrement = 0.2f, fruitPartRangeIncrement = 0.3f;
    [SerializeField] [Tooltip("The range of available parts to spawn for current difficulty/progress.")] int enemylevelPartRange, boundaryLevelRange, fruitRange;
    [SerializeField] bool generateLevel; 
    [SerializeField] float levelPartSize;
    [SerializeField] float levelHeight;
    [SerializeField] [Range(0.1f,10f)]float levelScale;

    [Header("Level Visuals")]
    [SerializeField] int levelVisualIndex;
    [SerializeField] float levelTransitionTime;
    [SerializeField] LightingManager lightingManager;
    [SerializeField] PostProcessManager ppManager;
    [SerializeField] LevelVisuals visuals;
    Portal portal;

    int activatedTimes = 1, totalRunActivatedTimes;
    const int levelPartsActiveAnytime = 4; //The number of active parts at any point  

    // Start is called before the first frame update
    void Start()
    {
        levelHeight = Mathf.Max(levelPartSize * levelPartsActiveAnytime, levelHeight);
        levelPartDeactivator.SetLevelManager(this);

        //Setup Level      
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
        portal = roof.GetComponentInChildren<Portal>();

        if (portal != null)
            portal.SetLevelManager(this);

        if (fruit != null)
            fruit.DisableAsset();

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

        LevelPart _part;

        float chance = Random.Range(0.0f, 1.0f);
        int choice = 0;
        if (canHaveEnemy)
        {
            if (chance < enemySpawnChance) //try to get part if it's active, get the next one.
            {

                while (enemyLevelParts[Mathf.Min(Mathf.RoundToInt(((chance * enemyLevelParts.Count) + choice) % enemylevelPartRange + (levelPartRangeIncrement * (activatedTimes + totalRunActivatedTimes))), enemyLevelParts.Count - 1)].InUse() || choice < 10)
                    choice++;
                _part = enemyLevelParts[Mathf.Min(Mathf.RoundToInt(((chance * enemyLevelParts.Count) + choice) % enemylevelPartRange + (levelPartRangeIncrement* (activatedTimes + totalRunActivatedTimes))), enemyLevelParts.Count - 1)];
                _part.SetupAsset(_position, _rotation, _scale);

                //Debug.Log("Spawning enemy part: " + Mathf.Min(Mathf.RoundToInt(((chance * enemyLevelParts.Count) + choice) % enemylevelPartRange + (levelPartRangeIncrement * (activatedTimes + totalRunActivatedTimes))), enemyLevelParts.Count - 1));


                if (choice > 10)
                    Debug.LogError("Choice too high, probably an error occuring.");
            }
            else
            {
                while (boundaryLevelParts[Mathf.Min(Mathf.RoundToInt((((chance - enemySpawnChance) / (1 - enemySpawnChance) * boundaryLevelParts.Count) + choice) % boundaryLevelRange + (levelPartRangeIncrement * (activatedTimes + totalRunActivatedTimes))), boundaryLevelParts.Count - 1)].InUse() || choice < 10)
                    choice++;
                _part = boundaryLevelParts[Mathf.Min( Mathf.RoundToInt((((chance - enemySpawnChance) / (1 - enemySpawnChance) * boundaryLevelParts.Count) + choice) % boundaryLevelRange + (levelPartRangeIncrement * (activatedTimes + totalRunActivatedTimes))),boundaryLevelParts.Count-1)];
                _part.SetupAsset(_position, _rotation, _scale);

                //Debug.Log("Spawning boundary part: " + Mathf.Min(Mathf.RoundToInt((((chance - enemySpawnChance) / (1 - enemySpawnChance) * boundaryLevelParts.Count) + choice) % boundaryLevelRange + (levelPartRangeIncrement * (activatedTimes + totalRunActivatedTimes))), boundaryLevelParts.Count - 1));
            }
        }
        else
        {
            while (boundaryLevelParts[Mathf.Min(Mathf.RoundToInt(((chance * boundaryLevelParts.Count) + choice) % boundaryLevelRange + (levelPartRangeIncrement * (activatedTimes + totalRunActivatedTimes))), boundaryLevelParts.Count - 1)].InUse() || choice < 10)
                choice++;
            _part = boundaryLevelParts[Mathf.Min(Mathf.RoundToInt(((chance * boundaryLevelParts.Count) + choice) % boundaryLevelRange + (levelPartRangeIncrement * (activatedTimes + totalRunActivatedTimes))), boundaryLevelParts.Count - 1)];
            _part.SetupAsset(_position, _rotation, _scale);

            //Debug.Log("Spawning boundary part: " + Mathf.Min(Mathf.RoundToInt(((chance * boundaryLevelParts.Count) + choice) % boundaryLevelRange + (levelPartRangeIncrement * (activatedTimes + totalRunActivatedTimes))), boundaryLevelParts.Count - 1));
        }

        chance = Random.Range(0.0f, 1.0f);

        if (chance < fruitSpawnChance) //try to get part if it's active, get the next one.
        {
            //set fruit spawn in part.
            if(!fruit.InUse())
                fruit.SetupAsset(_part.GetSpawnPoint(), Quaternion.identity, fruit.transform.localScale, Mathf.RoundToInt(fruitPartRangeIncrement * (activatedTimes+totalRunActivatedTimes)), fruitRange);
            //Debug.Log("fruit: " + Mathf.RoundToInt(fruitPartRangeIncrement * (activatedTimes + totalRunActivatedTimes)));
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

        Fruit _fruit = Instantiate(fruit, transform);
        fruit = _fruit;
        fruit.SetPostProcessManager(ppManager);
        fruit.DisableAsset();

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
    /// Sets next level for player to traverse.
    /// </summary>
    public void StartNextLevel()
    {

        if (ppManager != null && portal != null)
            ppManager.StartPortalEffect(levelTransitionTime, portal.GetPortalColour(levelVisualIndex));

        levelVisualIndex++;
     
        RestartLevel(false);
    }

    /// <summary>
    /// Return the increase in progress for the current level.
    /// </summary>
    /// <returns></returns>
    public float GetLevelProgress()
    {
        float progress = Mathf.Clamp01( Mathf.InverseLerp(startpoint.position.y + .3f, startpoint.position.y + levelHeight, player.transform.position.y) - levelProgress);
        levelProgress += progress;
        return Mathf.Max(0, progress);
    }

    public void RestartLevel(bool _resetEntirely)
    {
        if(risingWater!=null)
            risingWater.RestartRisingDeathBox();
        

        if (ppManager != null && _resetEntirely)
            ppManager.CancelEffect();

        totalRunActivatedTimes = activatedTimes;
        activatedTimes = 1;

        DisableLevelAssets();
        GenerateInitialLevel();

        levelProgress = 0;

        if (_resetEntirely)
        {
            levelVisualIndex = 0;
            totalRunActivatedTimes = 0;
        }
        SetLevelVisuals();

        if (player != null)
        {
            player.transform.position = startpoint.transform.position + Vector3.up;
            player.ResetPlayer();
        }
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

        if (portal != null)
            portal.SetPortalVisual(levelVisualIndex);

        //Set lighting
        if (lightingManager != null)
            lightingManager.SetGlobalLighting(levelVisualIndex);

        if (visuals != null)
            visuals.SetLevelVisuals(levelVisualIndex);
    }
}
