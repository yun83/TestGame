using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SigsawManager : MonoBehaviour
{
    [Header("�����ġ")]
    public RectTransform WinBackGround;
    [Header("������� ������")]
    public float WinBackOverSize = 5;

    [Header("���� ���� ��ġ")]
    public RectTransform PiecePoint;

    [Header("���� ��� ����")]
    public Transform WinTrans;
    public Transform PieceGroup;

    [Header("�޶�ٴ� �Ÿ�")]
    public int snapOffset = 50;
    [Header("�������� �Ѹ��� �Ÿ�")]
    public float PieceArea = 100;

    [Header("�¸����â")]
    public GameObject WinPopup;
    public Button ReStart;
    public Text PlayTimeText;

    float PlayTime = 0;
    public enum PuzzelType
    {
        Piece3x3 = 0,
        Piece4x4,
        Piece5x5,
    }
    int PuzzelSize = 9;
    int posPerSize = 3;

    [Header("���� Ÿ��")]
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
        if (puzzelState == 0)
        {
            PlayTime += Time.deltaTime;
        }

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

        //----- �¸� �ǳ� ��ġ�� ũ�� ���� -----
        Vector2 WinSize;
        WinSize.x = (One.sizeDelta.x * posPerSize) + (WinBackOverSize * 2);
        WinSize.y = (One.sizeDelta.y * posPerSize) + (WinBackOverSize * 2);

        WinBackGround.sizeDelta = WinSize;

        //float temp = One.sizeDelta.x * (posPerSize / 2);
        //Debug.Log(temp);
        //----- ���� ��ġ ���� -----
        StartPos.x = WinBackGround.anchoredPosition.x - (One.sizeDelta.x * (posPerSize / 2));
        StartPos.y = WinBackGround.anchoredPosition.y + (One.sizeDelta.y * (posPerSize / 2));
        if(posPerSize % 2 == 0)
        {
            //¦���ϰ�� ��ǥ ����
            Debug.Log("�ƾƾƾƝ�");
            StartPos.x += (One.sizeDelta.x / 2);
            StartPos.y -= (One.sizeDelta.y / 2);
        }

        PieceLeaveId.Clear();
        for (int i = 0; i < PuzzelSize; i++)
        {
            if (i < WinTrans.childCount)
            {
                //----- �¸����� ��ġ���� -----
                float RandomPos;
                Transform wC = WinTrans.GetChild(i);
                RectTransform wcRect = wC.GetComponent<RectTransform>();

                Vector2 SetPos;
                float xPos = StartPos.x + (One.sizeDelta.x * (i % posPerSize));
                float yPos = StartPos.y - (One.sizeDelta.y * (i / posPerSize));
                SetPos = new Vector3(xPos, yPos);

                wcRect.anchoredPosition = SetPos;

                //----- ���� ���� -----
                //PieceGroup
                PieceLeaveId.Add(-1);
                GameObject Piece = Instantiate(wC.gameObject, PieceGroup);  //new GameObject("Piece Pos " + i.ToString());
                Piece.name = "Piece Pos " + i.ToString();

                //Sprite Image = wC.GetComponent<Image>().sprite;
                Piece.transform.parent = PieceGroup;
                Piece.AddComponent<uiPuzzlePiece>().PieceInit(this, i, StartPos, posPerSize);
                

                //----- ���� �������� ���� ��ġ -----
                RandomPos = Random.Range(-PieceArea, PieceArea);
                Vector2 PieceItemPos = PiecePoint.anchoredPosition;
                PieceItemPos.x += RandomPos; 
                RandomPos = Random.Range(-PieceArea, PieceArea);
                PieceItemPos.y += RandomPos;
                Piece.GetComponent<RectTransform>().anchoredPosition = PieceItemPos; 
            }
        }
        WinTrans.gameObject.SetActive(false);
        PlayTime = 0;
    }

    void Win()
    {
        //�¸��˾� ����
        WinPopup.SetActive(true);

        //���� ���� �����ֱ�
        WinTrans.gameObject.SetActive(true);

        //�׷��� �̵� �ǽ� ����
        for(int i = PieceGroup.childCount - 1; i >= 0; i --)
            Destroy(PieceGroup.GetChild(i).gameObject);

        //�÷���Ÿ�� ���
        string timeStr = System.TimeSpan.FromSeconds(PlayTime).ToString(@"mm\:ss"); ;
        Debug.Log(timeStr);
        PlayTimeText.text = "Play Time : " + timeStr;
    }

    void WinCheck()
    {
        if (PieceLeaveId.Count <= 0)
            return;
        if (!DragEndCheck)
            return;
        if (puzzelState != 0)
            return;

        bool win = true;
        for (int i = 0; i < PieceLeaveId.Count; i++)
        {
            if (PieceLeaveId[i] != i)
            {
                Debug.Log("����ġ");
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

    /// <summary>
    /// Sprite Size �������� �Լ�
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
