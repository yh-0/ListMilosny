using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class UIManager : NetworkBehaviour
{
    public PlayerManager PlayerManager;
    public GameManager GameManager;
    public GameObject Button;
    public GameObject PlayerText;
    public GameObject EnemyText;
    
    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void UpdatePlayerText()
    {
        PlayerText.GetComponent<Text>().text = "Punkty: " + GameManager.PlayerPoints;
        EnemyText.GetComponent<Text>().text = "Punkty: " + GameManager.EnemyPoints;
    }

    public void UpdateButtonText(string buttonText)
    {
        Button = GameObject.Find("Button");
        Button.GetComponentInChildren<Text>().text = buttonText;
    }
}
