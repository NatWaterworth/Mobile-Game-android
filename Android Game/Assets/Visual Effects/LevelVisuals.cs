using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelVisuals : MonoBehaviour
{
    [SerializeField] List<GameObject> levelVisuals;


    public void SetLevelVisuals(int _visualIndex)
    {
        if (levelVisuals != null && levelVisuals.Count > 0)
        {
            foreach(GameObject levelObject in levelVisuals)
            {
                if(levelObject!=null)
                    levelObject.SetActive(false);
            }
            if(levelVisuals[_visualIndex % levelVisuals.Count] !=null)
                levelVisuals[_visualIndex % levelVisuals.Count].SetActive(true);
        }
    }
}
