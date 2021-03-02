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

    bool isValid(int x, int y)
    {
        if (x >= 0 && x < BoardSize && y >= 0 && y < BoardSize)
        {
            return true;
        }
        return false;
    }

    void MatchLength(int combo, int a, int b)
    {
        if (combo >= 3)
        {
            for (int i = 0; i < combo; i++)
            {
                Debug.Log(a + " " + b);
                ToBeDestroyed.Add(Board[a, b - i]);
            }

            Debug.LogWarning("Combo x " + (combo).ToString());
        }
    }

    public void MatchChecker()
    {
        int combo = 1;
        int lastValue = -1;

        //up and down
        for (int y = 0; y < BoardSize; y++)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                //if a valid cord
                if (isValid(y, x))
                {
                    //Debug.Log("Current: " + Board[y, x].GetComponent<Tile>().Value + " | Last Value: " + lastValue + " | " + x + " , " + y + " Combo: " + combo);
                    //if the current == last
                    if (Board[y, x].GetComponent<Tile>().Value == lastValue)
                    {
                        combo++;
                    }
                    else
                    {
                        MatchLength(combo, y, x);
                        combo = 1;
                    }
                    lastValue = Board[y, x].GetComponent<Tile>().Value;
                }
            }

            MatchLength(combo, y, BoardSize - 1);
            combo = 1;
            lastValue = -1;
        }

        foreach (GameObject i in ToBeDestroyed)
        {
            Board[i.GetComponent<Tile>().Cords.x, i.GetComponent<Tile>().Cords.y] = null;
            Destroy(i);
        }
    }
}
