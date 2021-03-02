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
    }

    // make board
    public void MakeGrid()
    {
        for (int y = 0; y < BoardSize; y++)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                GameObject tile = Instantiate(Tile, new Vector2(x - BoardSize / 2, y - BoardSize / 2) * Offset, Quaternion.identity, TileParent);
                Board[x, y] = tile;

                tile.GetComponent<Tile>().Cords = new Vector2Int(x, y);
                tile.name = "(" + x + "," + y + ")";

                if (Random.Range(0, 2) == 0)
                {
                    tile.GetComponent<SpriteRenderer>().color = Color.black;
                    tile.GetComponent<Tile>().Value = 1;
                }
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

    void MatchLengthUpAndDown(int combo, int a, int b)
    {
        if (combo >= 3)
        {
            if (combo == BoardSize)
            {
                for (int i = 0; i < combo; i++)
                {
                    if (isValid(a, b - i))
                    {
                        if (!ToBeDestroyed.Contains(Board[a, b - i]))
                            ToBeDestroyed.Add(Board[a, b - i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < combo; i++)
                {
                    if (isValid(a, b - i))
                    {
                        if (!ToBeDestroyed.Contains(Board[a, b - i]))
                            ToBeDestroyed.Add(Board[a, b - i]);
                    }
                }
            }
        }
    }

    void MatchLengthLeftAndRight(int combo, int a, int b)
    {
        if (combo >= 3)
        {
            if (combo == BoardSize)
            {
                for (int i = 0; i < combo; i++)
                {
                    if (isValid(a - i, b))
                    {
                        if (!ToBeDestroyed.Contains(Board[a - i, b]))
                            ToBeDestroyed.Add(Board[a - i, b]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < combo; i++)
                {
                    if (isValid(a - i, b))
                    {
                        if (!ToBeDestroyed.Contains(Board[a - i, b]))
                            ToBeDestroyed.Add(Board[a - i, b]);
                    }
                }
            }
        }
    }

    void FillUpBoard() 
    {
        for (int y = 0; y < BoardSize; y++)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                if (Board[x, y] == null) 
                {
                    GameObject tile = Instantiate(Tile, new Vector2(x - BoardSize / 2, y - BoardSize / 2) * Offset, Quaternion.identity, TileParent);
                    Board[x, y] = tile;

                    tile.GetComponent<Tile>().Cords = new Vector2Int(x, y);
                    tile.name = "(" + x + "," + y + ")";

                    if (Random.Range(0, 2) == 0)
                    {
                        tile.GetComponent<SpriteRenderer>().color = Color.black;
                        tile.GetComponent<Tile>().Value = 1;
                    }
                }
            }
        }
    }

    public void MatchChecker()
    {
        int combo = 1;
        int lastValue = -1;

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
                    //if current != last
                    else
                    {
                        //apply combo and reset
                        MatchLengthUpAndDown(combo, y, x);
                        MatchLengthLeftAndRight(combo, y, x);
                        combo = 1;
                    }
                    //set the current to the last
                    lastValue = Board[y, x].GetComponent<Tile>().Value;
                }
            }
            MatchLengthUpAndDown(combo, y, BoardSize - 1);
            MatchLengthLeftAndRight(combo, y, BoardSize - 1);

            combo = 1;
            lastValue = -1;
        }

        foreach (GameObject i in ToBeDestroyed)
        {
            Board[i.GetComponent<Tile>().Cords.x, i.GetComponent<Tile>().Cords.y] = null;
            Destroy(i);
        }

        ToBeDestroyed.Clear();

        FillUpBoard();
    }
}
