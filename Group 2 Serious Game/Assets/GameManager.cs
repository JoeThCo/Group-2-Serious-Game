using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Board Init")]
    public int BoardSize;
    public float Offset;
    public GameObject[,] Board;

    [Header("Match Info")]
    public int Matches;
    public int MaxMatches;

    [Header("Time for Level")]
    public float TimeForLevel;

    [Header("All Spawnable Tiles")]
    public GameObject[] AllTiles;
    [Space(15)]
    public Transform TileParent;

    [Header("Board UI")]
    public Image ProgressBar;

    public List<GameObject> ToBeDestroyed;
    private void Start()
    {
        Board = new GameObject[BoardSize, BoardSize];

        MakeGrid();
    }

    private void FixedUpdate()
    {
        TimeForLevel -= Time.deltaTime;
    }

    void UpdateProgressBar() 
    {
        ProgressBar.fillAmount = (float)Matches / (float)MaxMatches;
    }

    // make board
    public void MakeGrid()
    {
        for (int y = 0; y < BoardSize; y++)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                SpawnRandomTile(x, y);
            }
        }
    }

    GameObject SpawnRandomTile(int x, int y)
    {
        int tileIndex = Random.Range(0, AllTiles.Length);

        GameObject tile = Instantiate(AllTiles[tileIndex], new Vector2(x - (BoardSize / 2), y - BoardSize / 2) * Offset, Quaternion.identity, TileParent);
        tile.GetComponent<Tile>().Value = tileIndex;

        Board[x, y] = tile;

        tile.GetComponent<Tile>().Cords = new Vector2Int(x, y);
        tile.name += " (" + x + "," + y + ")";

        return tile;
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
            Matches++;
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
            Matches++;
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
                    SpawnRandomTile(x, y);
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
                        MatchLengthLeftAndRight(combo, x, y);
                        combo = 1;
                    }
                    //set the current to the last
                    lastValue = Board[y, x].GetComponent<Tile>().Value;
                }
            }
            MatchLengthUpAndDown(combo, y, BoardSize - 1);
            MatchLengthLeftAndRight(combo, BoardSize - 1, y);

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
        UpdateProgressBar();
    }
}
