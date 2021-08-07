using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameState currentState;
    [SerializeField] UIManager uiManager;

    //Time Variables
    [Header("Time")]
    [SerializeField] float pauseTimeOnGameOver;
    float transitionTime; //for timestamping the slowdown effect.
    float previousTimescale;
    Coroutine timeTransitionCoroutine;
    

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

        SwitchState(currentState);
    }

    public void GameOver()
    {
        SwitchState(GameState.GameOver);

    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log("Timescale: " + Time.timeScale);
    }

    #region Time Related Functions

    void PauseGame(bool _paused)
    {
        if(_paused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    void SlowDownToPause(float _duration)
    {
        AlterTimescale(_duration, 0);
    }

    void AlterTimescale(float _duration, float _desiredTimescale)
    {
        StopAlteringTime();
        timeTransitionCoroutine = StartCoroutine(TransitionTimescale(_duration, _desiredTimescale));

    }

    void StopAlteringTime()
    {
        if (timeTransitionCoroutine != null)
        {
            StopCoroutine(timeTransitionCoroutine);
        }
    }

    IEnumerator TransitionTimescale(float _duration, float _desiredTimescale)
    {
        transitionTime = Time.unscaledTime; // set timestamp
        previousTimescale = Time.timeScale; // set starting timescale

        while (transitionTime + _duration > Time.unscaledTime)
        {
           
            Time.timeScale = Mathf.Lerp(previousTimescale, _desiredTimescale, Mathf.InverseLerp(transitionTime, transitionTime + _duration, Time.unscaledTime));
            yield return new WaitForSecondsRealtime(0.02f);
            
        }
        Time.timeScale = _desiredTimescale;
        yield return null;

    }

    #endregion

    void SwitchState(GameState _state)
    {
        switch (_state)
        {
            case GameState.GameOver:
                uiManager.SetState(UIManager.UIState.GameOverMenu);
                SlowDownToPause(pauseTimeOnGameOver);
                break;
            case GameState.MainMenu:
                uiManager.SetState(UIManager.UIState.MainMenu);            
                break;
            case GameState.Paused:
                uiManager.SetState(UIManager.UIState.PauseMenu);
                PauseGame(true);
                break;
            case GameState.Playing:
                uiManager.SetState(UIManager.UIState.HUD);
                PauseGame(false);
                break;
        }
        currentState = _state;
    }

}
