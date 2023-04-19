using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzelGame2 : MonoBehaviour
{
    public int puzzelState = 0;

    [Header("���� Ÿ��")]
    [Tooltip("3x3 = 3, 4x4 = 4, 5x5 = 5")]
    [Range(3, 8)]
    public int PlayType = 3;
    public Transform PieceGroup;

    [Header("�����ġ")]
    public RectTransform GameBackGround;
    [Header("������� ������")]
    public float WinBackOverSize = 5;

    [Header("�¸����â")]
    public GameObject WinPopup;
    public Button ReStart;
    public Text PlayTimeText;

    [Header("���� �̹���")]
    public Sprite[] ImageSpr;
    public Vector2 PieceSize = new Vector2(100, 100);

    private Vector2 StartPos;
    private int PuzzelGameSize = 0;

    private int[][] PieceData;
    private Vector2[] anPos;
    private float PlayTime = 0;

    class puzzel2Piece
    {
        public int idx = -1;
        public GameObject Obj;
        public Image img;
        public RectTransform rectTrans = null;
        public Button but = null;
        public delegate void CallBack();

        public void rectTransSetting(Vector2 _anchor, Vector2 _size)
        {
            rectTrans.localScale = Vector3.one;
            rectTrans.anchoredPosition = _anchor;
            rectTrans.sizeDelta = _size;
        }

        public void SetImage(Sprite _spr = null)
        {
            img.sprite = _spr;
        }

        public void ButtonSetting(CallBack Evenet)
        {
            but.onClick.RemoveAllListeners();
            but.onClick.AddListener(()=> {
                Evenet();
            });
        }
    }

    [SerializeField]
    private List<puzzel2Piece> p2piece = new List<puzzel2Piece>();

    // Start is called before the first frame update
    void Start()
    {
        ReStart.onClick.RemoveAllListeners();
        ReStart.onClick.AddListener(ReSetGame);
        StartCoroutine(GameInit());
    }

    // Update is called once per frame
    void Update()
    {
        if (puzzelState == 1)
        {
            PlayTime += Time.deltaTime;
        }

    }

    IEnumerator GameInit()
    {
        int cnt = 0;

        WinPopup.SetActive(false);
        puzzelState = 0;

        //�ε� â�� �������� ������ ���� ���� ���� ����� �ִ�
        //yield return new WaitForEndOfFrame();

        yield return null;

        //----- ���� �ǳ� ��ġ�� ũ�� ���� -----
        Vector2 sumSize;
        sumSize.x = (PieceSize.x * PlayType) + (WinBackOverSize * 2);
        sumSize.y = (PieceSize.y * PlayType) + (WinBackOverSize * 2);

        GameBackGround.sizeDelta = sumSize;

        //----- ���� ��ġ ���� -----
        StartPos.x = GameBackGround.anchoredPosition.x - (PieceSize.x * (PlayType / 2));
        StartPos.y = GameBackGround.anchoredPosition.y + (PieceSize.y * (PlayType / 2));
        if (PlayType % 2 == 0)
        {
            //¦���ϰ�� ��ǥ ����
            Debug.Log("�ƾƾƾƝ�");
            StartPos.x += (PieceSize.x / 2);
            StartPos.y -= (PieceSize.y / 2);
        }

        yield return null;

        PuzzelGameSize = PlayType * PlayType;
        anPos = new Vector2[PuzzelGameSize];

        int tempIdx = -1;
        int[] tempNum = new int[PuzzelGameSize];
        for (int i = 0; i < PuzzelGameSize; i++)
            tempNum[i] = i;

        ShuffleArray(tempNum);

        for (int i = 0; i < PuzzelGameSize; i++) {
            if (tempNum[i] == PuzzelGameSize - 1)
            {
                tempIdx = i;
                break;
            }
        }

        tempNum[tempIdx] = tempNum[PuzzelGameSize - 1];
        tempNum[PuzzelGameSize - 1] = PuzzelGameSize - 1;

        //�迭 ����
        PieceData = new int[PlayType][];
        for(int i = 0; i < PlayType; i++)
        {
            PieceData[i] = new int[PlayType];
        }

        p2piece.Clear();
        cnt = 0;
        for (int i = 0; i < PlayType; i++)
        {
            float yPos = StartPos.y - (PieceSize.y * i);
            for (int k = 0; k < PlayType; k++)
            {
                puzzel2Piece p2pTemp = new puzzel2Piece();

                float xPos = StartPos.x + (PieceSize.x * k);
                //2�� �迭�� 1�� �迭�� ��ȯ
                cnt = (i * PlayType) + k;
                anPos[cnt] = new Vector2(xPos, yPos);

                //������Ʈ ����
                GameObject item = new GameObject("Piece_" + cnt);
                item.transform.parent = PieceGroup;

                p2pTemp.Obj = item;
                p2pTemp.img = item.AddComponent<Image>();
                p2pTemp.rectTrans = item.GetComponent<RectTransform>();
                p2pTemp.but = item.AddComponent<Button>();
                p2pTemp.idx = cnt;
                p2pTemp.rectTransSetting(anPos[cnt], PieceSize);
                p2pTemp.ButtonSetting(()=> {
                    ClickEventCheck(p2pTemp.idx);
                });

                p2piece.Add(p2pTemp);
                PieceData[i][k] = tempNum[cnt];
            }
        }

        PieceData[PlayType - 1][PlayType - 1] = -1;

        yield return null;
        puzzelState = 1;

        PieceDataDraw();
    }

    void ReSetGame()
    {
        int cnt = 0;
        int tempIdx = -1;
        int[] tempNum = new int[PuzzelGameSize];
        for (int i = 0; i < PuzzelGameSize; i++)
            tempNum[i] = i;

        ShuffleArray(tempNum);
        for (int i = 0; i < PuzzelGameSize; i++)
        {
            if (tempNum[i] == PuzzelGameSize - 1)
            {
                tempIdx = i;
                break;
            }
        }

        tempNum[tempIdx] = tempNum[PuzzelGameSize - 1];
        tempNum[PuzzelGameSize - 1] = PuzzelGameSize - 1;

        for (int i = 0; i < PlayType; i++)
        {
            float yPos = StartPos.y - (PieceSize.y * i);
            for (int k = 0; k < PlayType; k++)
            {
                //2�� �迭�� 1�� �迭�� ��ȯ
                cnt = (i * PlayType) + k;

                PieceData[i][k] = tempNum[cnt];
            }
        }
        PieceData[PlayType - 1][PlayType - 1] = -1;

        PlayTime = 0;
        puzzelState = 1;

        WinPopup.SetActive(false);
        PieceDataDraw();
    }

    void PieceDataDraw()
    {
        for (int i = 0; i < PlayType; i++)
        {
            for (int k = 0; k < PlayType; k++)
            {
                int index = (i * PlayType) + k;
                if (PieceData[i][k] >= 0)
                    p2piece[index].SetImage(ImageSpr[PieceData[i][k]]);
                else
                    p2piece[index].SetImage();
            }
        }
    }

    void ClickEventCheck(int _idx)
    {

        string debugStr = "";
        int xTic = _idx / PlayType;
        int yTic = _idx % PlayType;

        int item = -1;
        int itemX = -1;
        int itemY = -1;
        debugStr += "Click Event Check ------------- [" + _idx + "] ------------- [" + xTic + "][" + yTic + "]\n";

        if (puzzelState != 1)
            return;

        if (PieceData[xTic][yTic] == -1)
            return;

        if (xTic - 1 >= 0 )
        {
            debugStr += PieceData[xTic - 1][yTic] + "\n";
            if (PieceData[xTic - 1][yTic] == -1)
            {
                itemX = xTic - 1;
                itemY = yTic;
            }
        }
        if (xTic + 1 < PlayType)
        {
            debugStr += PieceData[xTic + 1][yTic] + "\n";
            if (PieceData[xTic + 1][yTic] == -1)
            {
                itemX = xTic + 1;
                itemY = yTic;
            }
        }
        if (yTic - 1 >= 0)
        {
            debugStr += PieceData[xTic][yTic - 1] + "\n";
            if (PieceData[xTic][yTic - 1] == -1)
            {
                itemX = xTic;
                itemY = yTic - 1;
            }
        }
        if (yTic + 1 < PlayType)
        {
            debugStr += PieceData[xTic][yTic + 1] + "\n";
            if (PieceData[xTic][yTic + 1] == -1)
            {
                itemX = xTic;
                itemY = yTic + 1;
            }
        }

        debugStr += "[" + itemX + "][" + itemY + "]";
        if (itemX >= 0 && itemY >= 0)
        {
            item = PieceData[itemX][itemY];
            PieceData[itemX][itemY] = PieceData[xTic][yTic];
            PieceData[xTic][yTic] = item;
        }

        //Debug.Log(debugStr);
        PieceDataDraw();
        WinCheck();
    }

    void WinCheck()
    {
        string debugStr = "";
        bool winCheckFor = true;
        for (int i = 0; i < PlayType; i++)
        {
            for (int k = 0; k < PlayType; k++)
            {
                if (i == PlayType - 1 && k == PlayType - 1)
                {
                    if (PieceData[i][k] != -1)
                    {
                        winCheckFor = false;
                        break;
                    }
                }
                else if (PieceData[i][k] != (i * PlayType) + k)
                {
                    debugStr += PieceData[i][k] + "_";
                    winCheckFor = false;
                    break;
                }
            }
        }

        if (winCheckFor)
        {
            Win();
            puzzelState = 2;
        }
        else
        {
            //Debug.Log(debugStr);
        }
    }
    void Win()
    {
        //�¸��˾� ����
        WinPopup.SetActive(true);

        //�÷���Ÿ�� ���
        string timeStr = System.TimeSpan.FromSeconds(PlayTime).ToString(@"mm\:ss"); ;
        Debug.Log(timeStr);
        PlayTimeText.text = "Play Time : " + timeStr;
    }

    public void OnClick_testWin()
    {

        for (int i = 0; i < PlayType; i++)
        {
            for (int k = 0; k < PlayType; k++)
            {
                PieceData[i][k] = (i * PlayType) + k;
                if (i == PlayType - 1 && k == PlayType - 1)
                {
                    PieceData[i][k] = -1;
                }
            }
        }
        
        PieceDataDraw();
        WinCheck();
    }
    public void ShuffleArray<T>(T[] array)
    {
        int random1;
        int random2;

        T tmp;

        for (int index = 0; index < array.Length; ++index)
        {
            random1 = Random.Range(0, array.Length);
            random2 = Random.Range(0, array.Length);

            tmp = array[random1];
            array[random1] = array[random2];
            array[random2] = tmp;
        }
    }
}
