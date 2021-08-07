using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] List<GameObject> uiStateScreens;
    [SerializeField] UIState currentState;
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

}
