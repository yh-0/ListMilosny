using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardZoom : MonoBehaviour
{
    public GameObject DisplayArea;
    public GameObject zoomCardTemplate;

    private GameObject zoomCard;

    public void Awake()
    {
        DisplayArea = GameObject.Find("DisplayArea");
    }

    public void OnHoverEnter()
    {
        zoomCard = Instantiate(zoomCardTemplate, new Vector2(0, 0), Quaternion.identity);
        zoomCard.GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
        zoomCard.transform.SetParent(DisplayArea.transform, false);
    }

    public void OnHoverExit()
    {
        Destroy(zoomCard);
    }
}