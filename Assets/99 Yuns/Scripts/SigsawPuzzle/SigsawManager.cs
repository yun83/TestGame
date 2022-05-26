using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SigsawManager : MonoBehaviour
{
    [Header("결과위치")]
    public RectTransform WinBackGround;
    [Header("결과여백 사이즈")]
    public float WinBackOverSize = 5;

    [Header("조각 시작 위치")]
    public RectTransform PiecePoint;

    [Header("조각 담는 곳들")]
    public Transform WinTrans;
    public Transform PieceGroup;

    [Header("달라붙는 거리")]
    public int snapOffset = 50;
    [Header("랜덤으로 뿌리는 거리")]
    public float PieceArea = 100;

    [Header("승리결과창")]
    public GameObject WinPopup;
    public Button ReStart;

    public enum PuzzelType
    {
        Piece3x3 = 0,
        Piece4x4,
        Piece5x5,
    }
    int PuzzelSize = 9;
    int posPerSize = 3;

    [Header("퍼즐 타입")]
    public PuzzelType PlayType = PuzzelType.Piece3x3;

    public List<int> PieceLeaveId = new List<int>();
    public bool DragEndCheck = false;
    int puzzelState = 0;


    private void Awake()
    {
        UiTypeStart();

        ReStart.onClick.RemoveAllListeners();
        ReStart.onClick.AddListener(()=> {
            UiTypeStart();
        });
    }

    private void Update()
    {
        WinCheck();
    }

    void UiTypeStart()
    {
        WinPopup.SetActive(false);

        puzzelState = 0;
        switch (PlayType)
        {
            case PuzzelType.Piece3x3:
                posPerSize = 3;
                break;
            case PuzzelType.Piece4x4:
                posPerSize = 4;
                break;
            case PuzzelType.Piece5x5:
                posPerSize = 5;
                break;
        }
        PuzzelSize = posPerSize * posPerSize;

        RectTransform One;
        One = WinTrans.GetChild(0).GetComponent<RectTransform>();

        Vector2 StartPos;

        //----- 승리 판낼 위치와 크기 적용 -----
        Vector2 WinSize;
        WinSize.x = (One.sizeDelta.x * posPerSize) + (WinBackOverSize * 2);
        WinSize.y = (One.sizeDelta.y * posPerSize) + (WinBackOverSize * 2);

        WinBackGround.sizeDelta = WinSize;

        float temp = One.sizeDelta.x * (posPerSize / 2);
        Debug.Log(temp);
        StartPos.x = WinBackGround.anchoredPosition.x - (One.sizeDelta.x * (posPerSize / 2));
        StartPos.y = WinBackGround.anchoredPosition.y + (One.sizeDelta.y * (posPerSize / 2));
        if(posPerSize % 2 == 0)
        {
            //짝수일경우 좌표 수정
            Debug.Log("아아아아앜");
            StartPos.x += (One.sizeDelta.x / 2);
            StartPos.y -= (One.sizeDelta.y / 2);
        }

        PieceLeaveId.Clear();
        for (int i = 0; i < PuzzelSize; i++)
        {
            if (i < WinTrans.childCount)
            {
                //----- 승리퍼즐 위치적용 -----
                float RandomPos;
                Transform wC = WinTrans.GetChild(i);
                RectTransform wcRect = wC.GetComponent<RectTransform>();

                Vector2 SetPos;
                float xPos = StartPos.x + (One.sizeDelta.x * (i % posPerSize));
                float yPos = StartPos.y - (One.sizeDelta.y * (i / posPerSize));
                SetPos = new Vector3(xPos, yPos);

                wcRect.anchoredPosition = SetPos;

                //----- 조각 생성 -----
                //PieceGroup
                PieceLeaveId.Add(-1);
                GameObject Piece = Instantiate(wC.gameObject, PieceGroup);  //new GameObject("Piece Pos " + i.ToString());
                Piece.name = "Piece Pos " + i.ToString();

                //Sprite Image = wC.GetComponent<Image>().sprite;
                Piece.transform.parent = PieceGroup;
                Piece.AddComponent<uiPuzzlePiece>().PieceInit(this, i, StartPos, posPerSize);
                

                //----- 조각 범위내로 랜덤 배치 -----
                RandomPos = Random.Range(-PieceArea, PieceArea);
                Vector2 PieceItemPos = PiecePoint.anchoredPosition;
                PieceItemPos.x += RandomPos; 
                RandomPos = Random.Range(-PieceArea, PieceArea);
                PieceItemPos.y += RandomPos;
                Piece.GetComponent<RectTransform>().anchoredPosition = PieceItemPos; 
            }
        }
        WinTrans.gameObject.SetActive(false);
    }

    void Win()
    {
        WinTrans.gameObject.SetActive(true);

        for(int i = PieceGroup.childCount - 1; i >= 0; i --)
            Destroy(PieceGroup.GetChild(i).gameObject);

        WinPopup.SetActive(true);
    }

    void WinCheck()
    {
        if (DragEndCheck && puzzelState == 0)
        {
            if (PieceLeaveId.Count > 0)
            {
                bool win = true;
                for (int i = 0; i < PieceLeaveId.Count; i++)
                {
                    if (PieceLeaveId[i] != i)
                    {
                        Debug.Log("불일치");
                        win = false;
                        break;
                    }
                }

                if (win)
                {
                    puzzelState = 1;
                    Win();
                }
                DragEndCheck = false;
            }
        }
    }

    /// <summary>
    /// Sprite Size 가져오는 함수
    /// </summary>
    public Vector3 GetSpriteSize(GameObject _target)
    {
        Vector3 worldSize = Vector3.zero;
        if (_target.GetComponent<SpriteRenderer>())
        {
            Vector2 spriteSize = _target.GetComponent<SpriteRenderer>().sprite.rect.size;
            Vector2 localSpriteSize = spriteSize / _target.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
            worldSize = localSpriteSize;
            worldSize.x *= _target.transform.lossyScale.x;
            worldSize.y *= _target.transform.lossyScale.y;
        }
        else
        {
            Debug.Log("SpriteRenderer Null");
        }
        return worldSize;
    }
}
