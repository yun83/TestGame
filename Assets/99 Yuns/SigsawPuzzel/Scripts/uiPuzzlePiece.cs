using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class uiPuzzlePiece : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public int snapOffset = 30;
    private RectTransform rectTrans;
    public int ID;
    public int LeaveId = -1;
    public Color AlphaColor = new Color(1, 1, 1, 0.3f);

    public RectTransform CloneObj;
    Image thisImage;

    SigsawManager SigManager;
    Vector2 movePoint;
    Vector2 mousePoint;
    public List<Vector2> mouseUpPoint = new List<Vector2>();

    private void Awake()
    {
        rectTrans = GetComponent<RectTransform>();
        thisImage = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PieceInit(SigsawManager getManager, int idx, Vector2 _startPos, int _size = 3)
    {
        if(rectTrans == null)
            rectTrans = GetComponent<RectTransform>();

        ID = idx;

        SigManager = getManager;
        snapOffset = getManager.snapOffset;

        mouseUpPoint.Clear();

        //좌석처럼 달라붙는 위치 셋팅
        int PuzzelSize = _size * _size;
        for (int i = 0; i < PuzzelSize; i++)
        {
            float xPos = _startPos.x + (rectTrans.sizeDelta.x * (i % _size));
            float yPos = _startPos.y - (rectTrans.sizeDelta.y * (i / _size));

            Vector2 SetPos = new Vector2(xPos, yPos);
            mouseUpPoint.Add(SetPos);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButton(0))
        //{
        //    mousePoint = Input.mousePosition;
        //    Debug.Log("mouse Position " + Input.mousePosition + " mousePoint " + mousePoint);
        //    if (CloneObj != null)
        //    {
        //        CloneObj.anchoredPosition = mousePoint;
        //    }
        //}
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down");
        CloneObj = Instantiate(this.gameObject).GetComponent<RectTransform>();
        CloneObj.transform.parent = transform.parent;
        CloneObj.anchoredPosition = rectTrans.anchoredPosition;

        thisImage.color = new Color(1, 1, 1, 0.3f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (CloneObj != null)
            CloneObj.anchoredPosition += eventData.delta;

        movePoint = CloneObj.anchoredPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        int truck = -1;
        if (CloneObj != null)
            Destroy(CloneObj.gameObject);


        for (int i = 0; i < mouseUpPoint.Count; i++)
        {
            float Dis = Vector2.Distance(movePoint, mouseUpPoint[i]);
            if (Dis < snapOffset)
            {
                truck = i;
                break;
            }
        }
        
        if(truck >= 0)
            rectTrans.anchoredPosition = mouseUpPoint[truck];
        else
            rectTrans.anchoredPosition = movePoint;

        LeaveId = truck;
        thisImage.color = new Color(1, 1, 1, 1);

        SigManager.PieceLeaveId[ID] = LeaveId;
        SigManager.DragEndCheck = true;
    }
}
