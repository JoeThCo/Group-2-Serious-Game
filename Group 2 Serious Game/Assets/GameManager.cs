using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int BoardSize;
    public float Offset;

    public GameObject[,] Board;

    [Space(15)]
    public GameObject Tile;
    public Transform TileParent;

    private void Start()
    {
        Board = new GameObject[BoardSize, BoardSize];

        StartGrid();
    }

    // make board
    void StartGrid()
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

    public void ComboChecker()
    {
        int currentCombo = Board[0, 0].GetComponent<Tile>().Value;
        int combo = 1;

        void Reset(int x, int y)
        {
            if(combo >= 3)
                Debug.Log("Combo x" + combo.ToString());

            combo = 1;
            currentCombo = Board[x, y].GetComponent<Tile>().Value;
        }

        for (int y = 0; y < BoardSize; y++)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                Debug.Log("Current Combo:" + currentCombo + " | " + Board[x, y].GetComponent<Tile>().Value);

                //if not the same value - ending the combo
                if (Board[x, y].GetComponent<Tile>().Value != currentCombo)
                {
                    //reset the combo
                    Reset(x, y);
                }
                else
                {
                    combo++;
                }
            }
            Reset(BoardSize - 1, y);
        }
    }
}
