using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame : Single<MiniGame>
{
    public int State = 0;
    public GameObject ResPlayer;
    public GameObject ResEnemy;
    public Transform EnemyGroup;

    [Range(1, 10)]
    public float yAreaMax = 5;
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


    private void Awake()
    {
        State = 0;
        Score = 0;

        if (PlayerClone != null)
        {
            StartPos = PlayerClone.transform.position;
            charScript = PlayerClone.GetComponent<miniCharacter>();
        }
        else
            StartPos = new Vector3(-7, 0, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
                }

                charScript.MoveSpeed = this.MoveSpeed;
                charScript.yAreaMax = this.yAreaMax;
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
            Vector3 EnemyStartPoint = new Vector3(15, 0, 0);
            EnemyStartPoint.y = Random.Range(-yAreaMax, yAreaMax);

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

            LvDownTime -= 2f;
            if (LvDownTime < 3)
                LvDownTime = 3;

            LvCheckTime = 0;
        }

        EnemyTime += Time.deltaTime;
        LvCheckTime += Time.deltaTime;
    }
}
