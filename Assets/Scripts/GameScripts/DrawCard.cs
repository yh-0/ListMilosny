using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DrawCard : NetworkBehaviour
{
    public PlayerManager PlayerManager;
    public GameManager GameManager;

    public GameObject PlayerArea;
    public GameObject EnemyArea;

    private void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        PlayerArea = GameObject.Find("PlayerArea");
        EnemyArea = GameObject.Find("EnemyArea");
    }

    public void OnClick()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        if (GameManager.GameState == "Start")
        {

            DrawClick();
        }
        else if (GameManager.GameState == "Draw")
        {
            if (PlayerManager.IsMyTurn)
            {
                DrawClick();
            }

        }
        else if (GameManager.GameState == "End")
        {
            EndClick();
        }
    }

    void DrawClick()
    {
        PlayerManager.CmdDealCards();
    }

    void EndClick()
    {
        PlayerManager.CmdCompareCards(PlayerArea.transform.GetChild(0).gameObject, EnemyArea.transform.GetChild(0).gameObject);
        PlayerManager.CmdDiscard(PlayerArea);
        PlayerManager.CmdDiscard(EnemyArea);
        PlayerManager.CmdGMChangeState("Start");
        PlayerManager.CmdEnableTurn();
    }
}
