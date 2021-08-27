using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Portal : MonoBehaviour
{
    LevelManager levelManager;

    private void Awake()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null)
            col.isTrigger = true;
        else
            Debug.LogWarning(this + " needs a Collider2D to function.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(levelManager!=null)
                levelManager.StartNextLevel();
        }
            
    }

    public void SetLevelManager(LevelManager _levelManager)
    {
        levelManager = _levelManager;
    }
}
