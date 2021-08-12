using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] List<GameObject> uiStateScreens;
    [SerializeField] UIState currentState;

    [Header("UI Components")]
    [SerializeField] TextMeshProUGUI scoreHUD;
    [SerializeField] TextMeshProUGUI scoreMenu;
    [SerializeField] TextMeshProUGUI highScoreMenu;
    public enum UIState
    {
        MainMenu,
        GameOverMenu,
        HUD,
        PauseMenu
    }

    public void SetState(UIState _state)
    {
        int num = (int)_state;

        for(int i = 0; i < uiStateScreens.Count; i++)
        {
            if(i == num)
                uiStateScreens[i].SetActive(true);
            else
                uiStateScreens[i].SetActive(false);
        }
        currentState = _state;
    }

    public void UpdatePlayerScore(int _score)
    {
        if (scoreHUD != null)
        {
            scoreHUD.text = _score.ToString();
        }

        if (scoreMenu != null)
        {
            scoreMenu.text = _score.ToString();
        }
    }

    public void UpdatePlayerHighScore(int _score)
    {
        if (highScoreMenu != null)
        {
            highScoreMenu.text = _score.ToString();
        }
    }

}
