using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int BoardSize;
    public float Offset;

    public GameObject[,] Board;

    [Space(15)]
    public GameObject Tile;
    public Transform TileParent;
    [Space(15)]
    public GameObject DebugTile;
    public Transform DebugTileParent;

    private void Start()
    {
        Board = new GameObject[BoardSize, BoardSize];

        MakeGrid();
        DebugGrid();
    }

    // make board
    public void MakeGrid()
    {

        for (int y = 0; y < BoardSize; y++)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                GameObject tile = Instantiate(Tile, new Vector2(BoardSize / 2 - x, BoardSize / 2 - y) * Offset, Quaternion.identity, TileParent);
                Board[x, y] = tile;

                tile.GetComponent<Tile>().Cords = new Vector2Int(x, y);
                tile.name = "(" + x + "," + y + ")";

                if (x % 2 == 0)
                {
                    tile.GetComponent<SpriteRenderer>().color = Color.black;
                    tile.GetComponent<Tile>().Value = 1;
                }
            }
        }
    }

    public void DebugGrid() 
    {
        foreach (Transform tile in DebugTileParent) 
        {
            Destroy(tile.gameObject);
        }

        for (int y = 0; y < BoardSize; y++)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                GameObject debugTile = Instantiate(DebugTile, Vector3.zero, Quaternion.identity, DebugTileParent);
                debugTile.GetComponentInChildren<Text>().text = Board[x, y].GetComponent<Tile>().Value.ToString();
            }
        }
    }

    public void ComboChecker()
    {
        int currentCombo = Board[0, 0].GetComponent<Tile>().Value;
        int combo = 0;

        void Reset(int x, int y)
        {
            if (combo >= 3)
                Debug.Log("Combo x" + combo.ToString() + " " + y);

            combo = 0;
            currentCombo = Board[x, y].GetComponent<Tile>().Value;
        }

        //up and down
        for (int y = 0; y < BoardSize; y++)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                //if not the same value - ending the combo
                if (Board[y, x].GetComponent<Tile>().Value != currentCombo)
                {
                    //reset the combo
                    Reset(y, x);
                    break;
                }
                combo++;
            }
            Reset(BoardSize - 1, y);
        }
    }
}
