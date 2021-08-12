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
    [SerializeField] Transform startpoint, endpoint; // These will just be portals.
    float levelProgress;

    [SerializeField] RisingDeathBox risingWater;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Create a level from smaller level pieces.
    /// </summary>
    void GenerateLevel()
    {

    }
    /// <summary>
    /// Return the increase in progress for the current level.
    /// </summary>
    /// <returns></returns>
    public float GetLevelProgress()
    {
        float progress = Mathf.Clamp01( Mathf.InverseLerp(startpoint.position.y, endpoint.position.y, player.transform.position.y)) - levelProgress;
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

        levelProgress = 0;

    }

    public void LevelTransition()
    {
        if (risingWater != null)
            risingWater.TransitionWater();
    }
}
