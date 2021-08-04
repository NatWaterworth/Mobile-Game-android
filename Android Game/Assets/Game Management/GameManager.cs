using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState
    {
        Paused,
        Playing,
        GameOver,
        MainMenu
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
