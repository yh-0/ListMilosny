using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public UIManager UIManager;

    public int TurnOrder = 0;
    public string GameState = "Start";
    public int PlayerPoints = 0;
    public int EnemyPoints = 0;

    public int readyClicks = 0;

    void Start()
    {
        UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        UIManager.UpdatePlayerText();
        UIManager.UpdateButtonText("ROZPOCZNIJ");
    }

    public void ChangeGameState(string stateRequest)
    {
        if (stateRequest == "Start")
        {
            readyClicks = 0;
            TurnOrder = 0;
            GameState = "Start";
            UIManager.UpdateButtonText("ROZPOCZNIJ");
        }
        else if (stateRequest == "Draw")
        {
            if (readyClicks > 0)
            {
                GameState = "Draw";
            }
            UIManager.UpdateButtonText("DOBIERZ");
        }
        else if (stateRequest == "Play")
        {
            GameState = "Play";
            UIManager.UpdateButtonText("ZAGRAJ");
        }
        else if (stateRequest == "End")
        {
            GameState = "End";
            UIManager.UpdateButtonText("KONIEC RUNDY");
            UIManager.UpdatePlayerText();
        }
    }

    public void ChangeReadyClicks()
    {
        readyClicks++;
    }

    public void CardPlayed()
    {
        TurnOrder++;
        if (TurnOrder >= 13)
        {
            ChangeGameState("End");
        }
        else
        {
            ChangeGameState("Draw");
        }
    }
}
