using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMananger : MonoBehaviour
{
    public int BoardSize;
    [Range(1.5f,2.5f)]
    public float Offset;
    [Space(15)]
    public GameObject Tile;
    public Transform TileParent;
    [Space(15)]
    public GameObject[,] TileBoard;

    private void Start()
    {
        TileBoard = new GameObject[BoardSize, BoardSize];

        SpawnStartPieces();
    }

    void SpawnStartPieces() 
    {
        for (int rows = 0; rows < BoardSize; rows++)
        {
            for (int cols = 0; cols < BoardSize; cols++)
            {
                GameObject tile = Instantiate(Tile, new Vector2(BoardSize/2 * Offset - rows * Offset, BoardSize / 2 * Offset - cols * Offset), Quaternion.identity, TileParent);
                TileBoard[rows, cols] = tile;

                if (rows % 2 == 0)
                    tile.GetComponent<SpriteRenderer>().color = Color.black;

            }
        }
    }

}
