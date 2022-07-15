using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public GameManager GameManager;
    public UIManager UIManager;
    
    public GameObject Card1straznik;
    public GameObject Card2kaplan;
    public GameObject Card3baron;
    public GameObject Card4sluzaca;
    public GameObject Card5ksiaze;
    public GameObject Card7krol;
    public GameObject Card8hrabina;
    public GameObject Card9krolewna;
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject DropZone;

    public bool ready = false;
    public bool IsMyTurn = false;

    public SyncList<GameObject> cards = new SyncList<GameObject>();

    public override void OnStartClient()
    {
        base.OnStartClient();

        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        PlayerArea = GameObject.Find("PlayerArea");
        EnemyArea = GameObject.Find("EnemyArea");
        DropZone = GameObject.Find("DropZone");

        if (isClientOnly)
        {
            IsMyTurn = true;
        }
    }

    private void Update()
    {
        Debug.Log(GameManager.GameState + " " + IsMyTurn + " " + GameManager.TurnOrder + " " + GameManager.PlayerPoints + " " + ready);
    }

    [Server]
    public override void OnStartServer()
    {
        for (int i = 0; i < 5; i++) 
            cards.Add(Card1straznik);
        for (int i = 0; i < 2; i++) 
            cards.Add(Card2kaplan);
        for (int i = 0; i < 2; i++) 
            cards.Add(Card3baron);
        for (int i = 0; i < 2; i++) 
            cards.Add(Card4sluzaca);
        for (int i = 0; i < 2; i++) 
            cards.Add(Card5ksiaze);
        cards.Add(Card7krol);
        cards.Add(Card8hrabina);
        cards.Add(Card9krolewna);
    }

    [Command]
    public void CmdEnableTurn()
    {
        RpcEnableTurn();
    }

    [ClientRpc]
    void RpcEnableTurn()
    {
        if (isClientOnly)
        {
            IsMyTurn = true;
        }
    }

    [Command]
    public void CmdDealCards()
    {
        if (GameManager.GameState == "Start")
        {
            if (ready == false)
            {
                DrawCard();
                RpcGMChangeState("Draw");
                ready = true;
            }
        }
        else if (GameManager.GameState == "Draw")
        {
            DrawCard();
            RpcGMChangeState("Play");
            ready = false;
        }
    }

    public void DrawCard()
    {
        int newRandom = Random.Range(0, cards.Count);
        GameObject card = Instantiate(cards[newRandom], new Vector2(0, 0), Quaternion.identity);
        NetworkServer.Spawn(card, connectionToClient);
        RpcShowCard(card, "Dealt");
    }

    public void PlayCard(GameObject card)
    {
        CmdPlayCard(card);
    }

    [Command]
    void CmdPlayCard(GameObject card)
    {
        RpcShowCard(card, "Played");
    }

    [ClientRpc]
    void RpcShowCard(GameObject card, string type)
    {
        if (type == "Dealt")
        {
            if (hasAuthority)
            {
                card.transform.SetParent(PlayerArea.transform, false);
            }
            else
            {
                card.transform.SetParent(EnemyArea.transform, false);
                card.GetComponent<CardFlipper>().Flip();
            }
        }
        else if (type == "Played")
        {
            DropZone.transform.DetachChildren();
            card.transform.SetParent(DropZone.transform, false);
            if (hasAuthority)
            {
                CmdGMCardPlayed();
            }

            if (!hasAuthority)
            {
                card.GetComponent<CardFlipper>().Flip();
            }
            PlayerManager pm = NetworkClient.connection.identity.GetComponent<PlayerManager>();
            pm.IsMyTurn = !pm.IsMyTurn;
        }
    }

    [Command]
    public void CmdGMChangeState(string stateRequest)
    {
        RpcGMChangeState(stateRequest);
    }

    [ClientRpc]
    void RpcGMChangeState(string stateRequest)
    {
        GameManager.ChangeGameState(stateRequest);
        if (stateRequest == "Draw")
        {
            GameManager.ChangeReadyClicks();
        }
    }

    [Command]
    void CmdGMCardPlayed()
    {
        RpcGMCardPlayed();
    }

    [ClientRpc]
    void RpcGMCardPlayed()
    {
        GameManager.CardPlayed();
    }

    [Command]
    public void CmdCompareCards(GameObject playerCard, GameObject enemyCard)
    {
        RpcCompareCards(playerCard, enemyCard);
    }

    [ClientRpc]
    void RpcCompareCards(GameObject playerCard, GameObject enemyCard)
    {
        GameManager.PlayerPoints+=2;
        GameManager.EnemyPoints++;
    }

    [Command]
    public void CmdDiscard(GameObject player)
    {
        RpcDiscard(player);
    }

    [ClientRpc]
    void RpcDiscard(GameObject player)
    {
        player.transform.DetachChildren();
    }
}
