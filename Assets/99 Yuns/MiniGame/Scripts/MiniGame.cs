using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame : MonoBehaviour
{
    public static MiniGame ins;

    public int State = 0;
    public GameObject ResPlayer;
    public GameObject ResEnemy;
    public Transform EnemyGroup;

    [Range(1f, 10)]
    public float MoveSpeed = 2f;

    public int EnemyStartLv = 1;
    public float EnemyCallTime = 3f;
    private float EnemyTime = 0;
    private float LvCheckTime = 0;
    private float LvDownTime = 15;

    public GameObject PlayerClone;
    private miniCharacter charScript;
    private Vector3 StartPos;

    public float PlayTime = 0;

    public int Score = 0;
    private int checkScore = 0;
    public Vector2 screenSize;
    private Camera mainCam;


    private void Awake()
    {
        ins = this;

        State = 0;
        Score = 0;
        mainCam = Camera.main;

        if (PlayerClone != null)
        {
            StartPos = PlayerClone.transform.position;
            charScript = PlayerClone.GetComponent<miniCharacter>();

            PageSizeCheck();
        }
        else
            StartPos = new Vector3(-7, 0, 0);
    }

    private void Update()
    {
        StateLogic();
    }

    void StateLogic()
    {
        switch (State)
        {
            case 0:
                if (PlayerClone == null)
                {
                    PlayerClone = Instantiate(ResPlayer);
                    PlayerClone.name = "플레이어";
                    charScript = PlayerClone.GetComponent<miniCharacter>();
                    PlayerClone.transform.position = StartPos;
                    PageSizeCheck();
                }

                charScript.MoveSpeed = this.MoveSpeed;
                charScript.screenSize = this.screenSize;
                State = 1;
                break;
            case 1:
                EnemyCall();
                if(checkScore != Score)
                {
                    checkScore = Score;
                    UiCanvas.ins.SetDemoText(2, "Score: " + Score.ToString());
                }
                PlayTime += Time.deltaTime;
                break;
        }
    }

    void EnemyCall()
    {
        if (EnemyTime > EnemyCallTime)
        {
            GameObject tempEnemyObj = Instantiate(ResEnemy);
            Vector3 EnemyStartPoint = new Vector3(screenSize.x + 1, 0, 0);
            float yPos = screenSize.y - 0.5f;
            EnemyStartPoint.y = Random.Range(-yPos, yPos);

            if (EnemyGroup != null)
            {
                tempEnemyObj.transform.parent = EnemyGroup;
            }
            else
            {
                EnemyGroup = new GameObject("EnemyGroup").transform;
                tempEnemyObj.transform.parent = EnemyGroup;
            }

            //Debug.Log(EnemyStartPoint);
            tempEnemyObj.name = "Enemy_Lv[" + EnemyStartLv + "]";
            tempEnemyObj.GetComponent<miniEnemy>().InitEnemy(EnemyStartLv, EnemyStartPoint);

            EnemyTime = 0;
        }

        if(LvCheckTime > LvDownTime)
        {
            EnemyStartLv++;

            EnemyCallTime -= 0.5f;
            if (EnemyCallTime < 0.2f)
                EnemyCallTime = 0.2f;

            LvDownTime -= 0.5f;
            if (LvDownTime < 3)
                LvDownTime = 3;

            LvCheckTime = 0;
        }

        EnemyTime += Time.deltaTime;
        LvCheckTime += Time.deltaTime;
    }

    //화면의 3D 좌표계 넓이 높이 구하는 공식
    void PageSizeCheck()
    {
        Vector3 tempSize;
        tempSize.x = Screen.width;
        tempSize.y = Screen.height;
        //카메라에 그려질 오브젝트의 위치를 계산하여 z거리를 준다
        tempSize.z = Mathf.Abs(mainCam.transform.position.z - transform.position.z);

        //WorldToScreenPoint 반대
        screenSize = Camera.main.ScreenToWorldPoint(tempSize);
    }
}
