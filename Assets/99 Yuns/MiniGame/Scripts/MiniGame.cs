using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame : Single<MiniGame>
{
    public int State = 0;
    public GameObject ResPlayer;
    public GameObject ResEnemy;

    [Range(1, 10)]
    public float yAreaMax = 5;
    [Range(1f, 10)]
    public float MoveSpeed = 2f;

    public int EnemyStartLv = 1;
    public float EnemyCallTime = 3f;
    private float EnemyTime = 0;

    public GameObject PlayerClone;
    private miniCharacter charScript;
    private Vector3 StartPos;

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
                break;
        }
    }

    void EnemyCall()
    {
        if (EnemyTime > EnemyCallTime)
        {
            GameObject tempEnemyObj = Instantiate(ResEnemy);
            Vector3 EnemyStartPoint = new Vector3(15, 0, 0);
            EnemyStartPoint.y = Random.RandomRange(-yAreaMax, yAreaMax);
            Debug.Log(EnemyStartPoint);


            tempEnemyObj.GetComponent<miniEnemy>().InitEnemy(EnemyStartLv, EnemyStartPoint);

            EnemyTime = 0;
        }
        EnemyTime += Time.deltaTime;
    }
}
