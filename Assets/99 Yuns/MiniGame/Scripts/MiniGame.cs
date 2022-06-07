using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame : MonoBehaviour
{
    public int State = 0;
    public GameObject ResPlayer;
    public GameObject ResEnemy;

    [Range(1, 10)]
    public float yAreaMax = 5;
    [Range(1f, 10)]
    public float MoveSpeed = 2f;

    public float EnemyCallTime = 3f;
    private float EnemyTime = 0;

    public GameObject PlayerClone;
    private miniCharacter charScript;
    private Vector3 StartPos;

    private void Awake()
    {
        State = 0;

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
                break;
        }
    }

    void EnemyCall()
    {
        if (EnemyTime > EnemyCallTime)
        {
            GameObject tempEnemyObj = Instantiate(ResEnemy);

            EnemyTime = 0;
        }
        EnemyTime += Time.deltaTime;
    }
}
