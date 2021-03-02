using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tile : MonoBehaviour
{
    [HideInInspector]
    public int Value;
    public Vector2Int Cords;
    [Space(15)]
    public GameObject SwitchTile;

    private Vector3 offset = new Vector3(0, 0, 10);
    private Vector3 startSpot;
    private Camera Cam;
    private GameManager gm;

    private void Awake()
    {
        Cam = Camera.main;
        gm = FindObjectOfType<GameManager>();
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, Random.Range(.5f, 1f)).SetEase(Ease.Flash);
    }

    //2 pick up piece
    private void OnMouseDown()
    {
        //get og position to return to if not a valid spot
        startSpot = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);

        //visual of seletecing the object
    }

    private void OnMouseDrag()
    {
        DragTileAround();
    }

    private void OnMouseUp()
    {
        SwitchTileLocation();

        gm.MatchChecker();

        SwitchTile = null;
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

    //3 swap tiles on move
    void SwitchTileLocation()
    {
        if (SwitchTile && new Vector2(Mathf.Round(startSpot.x), Mathf.Round(startSpot.y)) != new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)))
        {
            //snap to nearest whole number
            transform.position = new Vector3(Mathf.RoundToInt(SwitchTile.transform.position.x), Mathf.RoundToInt(SwitchTile.transform.position.y), 0);
            SwitchTile.transform.position = startSpot;

            //swap board values
            //update cords

            Vector2Int tempcords = SwitchTile.GetComponent<Tile>().Cords;
            SwitchTile.GetComponent<Tile>().Cords = gameObject.GetComponent<Tile>().Cords;
            gameObject.GetComponent<Tile>().Cords = tempcords;

            gm.Board[gameObject.GetComponent<Tile>().Cords.x, gameObject.GetComponent<Tile>().Cords.y] = gameObject;
            gm.Board[SwitchTile.GetComponent<Tile>().Cords.x, SwitchTile.GetComponent<Tile>().Cords.y] = SwitchTile;
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
