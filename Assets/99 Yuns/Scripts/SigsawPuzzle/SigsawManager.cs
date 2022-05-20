using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SigsawManager : MonoBehaviour
{
    public Transform WinTrans;
    public Transform WinPieceGroup;
    public Transform PieceGroup;

    int winPieceCount;

    private void Awake()
    {
        winPieceCount = WinTrans.childCount;
        for(int i = 0; i < winPieceCount; i++)
        {
            Transform wC = WinTrans.GetChild(i);
            //PieceGroup
            GameObject Piece = new GameObject("Piece Pos " + i.ToString());
            Piece.transform.parent = WinPieceGroup;
            Piece.AddComponent<PuzzlePiece>().PieceInit(wC.position);
        }
    }

}
