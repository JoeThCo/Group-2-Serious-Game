using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    private GameMananger gm;

    private void Start()
    {
        gm = FindObjectOfType<GameMananger>();    
    }

    public void SwapPiecesOnBoard(GameObject a, GameObject b) 
    {
        gm.TileBoard[Mathf.RoundToInt(a.transform.position.x), Mathf.RoundToInt(a.transform.position.y)] = b;
        gm.TileBoard[Mathf.RoundToInt(b.transform.position.x), Mathf.RoundToInt(b.transform.position.y)] = a;
    }

    void DoMatch() 
    {

    }
}
