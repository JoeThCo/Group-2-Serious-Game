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
    [Space(15)]
    public List<GameObject> ToBeDestroyed;
    private void Start()
    {
        Board = new GameObject[BoardSize, BoardSize];

        MakeGrid();
        //DebugGrid();
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
        int combo = 0;

        void Reset(int x, int y)
        {
            if (combo > 2)
            {
                for (int i = 0; i < combo; i++)
                {
                    Debug.Log((x - i) + " " + y);

                    if (!ToBeDestroyed.Contains(Board[x - i, y]))
                        ToBeDestroyed.Add(Board[x - i, y]);
                }
                Debug.LogWarning("Combo x " + combo.ToString() + " at col: " + y);
            }

            combo = 0;
        }

        //up and down
        for (int y = 0; y < BoardSize; y++)
        {
            Reset(0, 0);

            for (int x = 0; x < BoardSize; x++)
            {
                //if not the same value - ending the combo
                if (x > 0 && x < BoardSize - 1 && Board[y, x].GetComponent<Tile>().Value != Board[y, x + 1].GetComponent<Tile>().Value)
                {
                    //reset the combo
                    Reset(y, x);
                }
                else
                    combo++;
            }
            Reset(BoardSize - 1, y);
        }

        foreach (GameObject i in ToBeDestroyed)
        {
            Board[i.GetComponent<Tile>().Cords.x, i.GetComponent<Tile>().Cords.y] = null;
            Destroy(i);
        }
    }
}
