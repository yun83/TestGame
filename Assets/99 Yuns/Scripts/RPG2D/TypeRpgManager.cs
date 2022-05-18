using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeRpgManager : MonoBehaviour
{
    public GameObject Enemy;
    private UiCanvas GameUi;
    private TypeRpgPlayer TRP;

    int GameState = 0;
    bool InitEnd = false;

    [Header("Player Setting")]
    private int PlayerHP = 0;
    private HpBar PlayerHpBar;
    private Transform hpBarTrans;

    [Header("Enemy Setting")]
    public int MaxShowEnemy = 1;
    private bool CreateEnemyCheck = false;
    public BoxCollider2D RespawnArea;
    public List<TypeRpgEnemy> EnemyList = new List<TypeRpgEnemy>();

    private void Awake()
    {
        //Debug.Log("----- 1 -----");
        InitEnd = false;
    }

    IEnumerator Start()
    {
        while (!InitEnd)
        {
            CreateEnemyCheck = false;
            EnemyList.Clear();

            yield return new WaitForEndOfFrame();
            int CheckCount = 0;

            if (Info.ins.TRP != null)
            {
                TRP = Info.ins.TRP;
                CheckCount++;
            }
            if (Info.ins.GameUi != null)
            {
                GameUi = Info.ins.GameUi;
                CheckCount++;
            }

            if (CheckCount >= 2)
            {
                //HP 바 출력
                PlayerHpBar = GameUi.AddHpBar();
                hpBarTrans = PlayerHpBar.transform;
                PlayerHP = 0;
                HP_Plus(100);

                //이속 출력
                SpeedShow(TRP.moveSpeed);

                InitEnd = true;
            }
        }
    }

    private void Update()
    {
        if (!InitEnd)
            return;

        if(EnemyList.Count < MaxShowEnemy)
        {
            StartCoroutine(CreateEnemy());
        }
        else
        {
            for(int i = 0; i < EnemyList.Count; i++)
            {
                if(EnemyList[i].Hp < 0)
                {
                    Destroy(EnemyList[i].hpBar.gameObject);
                    Destroy(EnemyList[i].gameObject);
                    EnemyList.RemoveAt(i);
                }
            }
        }

        //Player 상태바
        hpBarTrans.position = Camera.main.WorldToScreenPoint(Info.ins.UserTrans.position + new Vector3(0, 0.8f, 0));
    }

    void HP_Plus(int hp)
    {
        PlayerHP += hp;

        Info.ins.GameUi.SetDemoText(0, "HP : " + PlayerHP.ToString());

        if (PlayerHP <= 0)
        {
            //사망처리
        }
    }

    void SpeedShow(float speed)
    {
        Info.ins.GameUi.SetDemoText(1, "Speed : " + speed);
    }

    IEnumerator CreateEnemy()
    {
        if (!CreateEnemyCheck)
        {
            CreateEnemyCheck = true;

            GameObject Temp = Instantiate(Enemy);
            TypeRpgEnemy tre = Temp.GetComponent<TypeRpgEnemy>();

            Vector2 SumPosition = RespawnArea.transform.position;
            float xSize = RespawnArea.size.x / 2;
            float ySize = RespawnArea.size.y / 2;

            SumPosition.x += Random.Range(-xSize, xSize);
            SumPosition.y += Random.Range(-ySize, ySize);

            Temp.transform.position = SumPosition;
            yield return new WaitForEndOfFrame();

            Temp.name = "적";
            tre.NicName = "허수아비";
            tre.MaxHp = 100;
            tre.Hp = 100;
            tre.trp = TRP;

            yield return new WaitForEndOfFrame();

            tre.hpBar = GameUi.AddHpBar(tre.NicName);

            yield return new WaitForEndOfFrame();

            tre.InitCheck = true;

            EnemyList.Add(tre);
            CreateEnemyCheck = false;
        }
    }
}
