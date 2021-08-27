using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameState currentState;
    [SerializeField] UIManager uiManager;
    [SerializeField] CameraController cameraController;
    [SerializeField] LevelManager levelManager;

    //Time Variables
    [Header("Time")]
    [SerializeField] float pauseTimeOnGameOver;
    float transitionTime; //for timestamping the slowdown effect.
    float previousTimescale;
    Coroutine timeTransitionCoroutine;

    [Header("Score")]
    [SerializeField] int scorePerLevel;
    [SerializeField] float scorePerLevelExponent;
    float highScore, score;

    

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
        if (currentState.Equals(GameState.Playing))
        {
            if (levelManager != null)
            {
                score += scorePerLevel * levelManager.GetLevelProgress();
                if (uiManager != null)
                    uiManager.UpdatePlayerScore(Mathf.RoundToInt( score));
            }
        }
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

    public void GoToMainMenu()
    {
        levelManager.RestartLevel(true);
        cameraController.ResetCamera();
        SwitchState(GameState.MainMenu);
    }

    public void RestartGame()
    {
        score = 0;
        UpdateScore();
        levelManager.RestartLevel(true);
        cameraController.ResetCamera();

        StopAlteringTime();
        SwitchState(GameState.Playing);

    }

    void UpdateScore()
    {
        highScore = Mathf.Max(score, highScore);
        uiManager.UpdatePlayerHighScore(Mathf.RoundToInt(highScore));
        uiManager.UpdatePlayerScore(Mathf.RoundToInt(score));
    }

    void SwitchState(GameState _state)
    {
        switch (_state)
        {
            case GameState.GameOver:
                if (uiManager != null)
                {
                    uiManager.SetState(UIManager.UIState.GameOverMenu);
                    UpdateScore();
                }
                if (levelManager != null)
                    levelManager.LevelTransition();

                SlowDownToPause(pauseTimeOnGameOver);
                
                break;
            case GameState.MainMenu:
                if (uiManager != null)
                    uiManager.SetState(UIManager.UIState.MainMenu);
                PauseGame(true);
                cameraController.ResetCamera();
                break;
            case GameState.Paused:
                if (uiManager != null)
                    uiManager.SetState(UIManager.UIState.PauseMenu);
                PauseGame(true);
                break;
            case GameState.Playing:
                if (uiManager != null)
                    uiManager.SetState(UIManager.UIState.HUD);
                PauseGame(false);
                cameraController.SetToPlaying();
                break;
        }
        currentState = _state;
    }

}
