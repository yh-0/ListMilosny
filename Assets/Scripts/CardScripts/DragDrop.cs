using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DragDrop : NetworkBehaviour
{
    public GameObject Canvas;
    public GameObject DropZone;
    public PlayerManager PlayerManager;
    public GameManager GameManager;

    private bool isDragging = false;
    private bool isOverDropZone = false;
    private bool isDraggable = true;
    private GameObject dropZone;
    private GameObject startParent;
    private Vector2 startPosition;

    private void Start()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Canvas = GameObject.Find("Main Canvas");
        DropZone = GameObject.Find("DropZone");
        if (!hasAuthority)
        {
            isDraggable = false;
        }
    }

    void Update()
    {
        if(isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            transform.SetParent(Canvas.transform, true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverDropZone = true;
        dropZone = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = false;
        dropZone = null;
    }

    public void StartDrag()
    {
        if (!isDraggable) return;
        startParent = transform.parent.gameObject;
        startPosition = transform.position;
        isDragging = true;
    }

    public void EndDrag()
    {
        if (!isDraggable) return;
        isDragging = false;
        if(isOverDropZone && PlayerManager.IsMyTurn && GameManager.GameState == "Play")
        {
            transform.SetParent(dropZone.transform, false);
            isDraggable = false;
            PlayerManager.PlayCard(gameObject);
        }
        else
        {
            transform.position = startPosition;
            transform.SetParent(startParent.transform, false);
        }
    }
}
