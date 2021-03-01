using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public GameObject SwitchTile;

    private Vector3 offset = new Vector3(0,0,10);
    private Vector2 startSpot;
    private Camera Cam;

    private GameMananger gm;
    private MatchManager mm;

    private void Start()
    {
        Cam = Camera.main;

        gm = FindObjectOfType<GameMananger>();
        mm = FindObjectOfType<MatchManager>();
    }

    private void OnMouseDown()
    {
        //get og position to return to if not a valid spot
        startSpot = transform.position;

        //visual of seletecing the object
    }

    private void OnMouseDrag()
    {
        DragTileAround();
    }

    private void OnMouseUp()
    {
        SwitchTileLocation();
    }

    void DragTileAround() 
    {
        //get the current location
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1);

        //apply offset
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        //set pos
        transform.position = curPosition;
    }

    void SwitchTileLocation() 
    {
        if (SwitchTile && new Vector2(Mathf.Round(startSpot.x), Mathf.Round(startSpot.y)) != new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)))
        {
            //snap to nearest whole number
            transform.position = new Vector3(Mathf.RoundToInt(SwitchTile.transform.position.x), Mathf.RoundToInt(SwitchTile.transform.position.y), 0);
            SwitchTile.transform.position = startSpot;

            //swap board values
            mm.SwapPiecesOnBoard(gameObject, SwitchTile);
        }
        else
        {
            transform.position = startSpot;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tile")) 
        {
            SwitchTile = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tile"))
        {
            SwitchTile = null;
        }
    }
}
