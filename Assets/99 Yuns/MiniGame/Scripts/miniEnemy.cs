using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class miniEnemy : MonoBehaviour
{
    int eState = 0;
    public float MoveSpeed = 3f;
    public int Hp = 0;
    private int StartHp = 0;
    public float LifeTime = 20;

    public HpBar _hpBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitEnemy(int _Lv, Vector3 _pos)
    {
        if (_hpBar != null)
            Destroy(_hpBar.gameObject);

        transform.position = _pos;
        switch (_Lv)
        {
            case 0:
            case 1:
                StartHp = 2;
                Hp = 2;
                break;
        }

        _hpBar = UiCanvas.ins.AddHpBar("적");
        Invoke("DestroyEnemy", LifeTime);

        eState = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (eState != 1)
            return;

        Vector2 movepos = transform.position;
        float offset = Time.deltaTime * MoveSpeed;
        movepos.x += (-1 * offset);
        transform.position = Vector2.MoveTowards(transform.position, movepos, 1);

        _hpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 2.5f, 0));
        //hpTrans.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.25f, 0));
    }

    public void AddHp(int _hp)
    {
        if (eState != 1)
            return;

        Hp += _hp;
        MiniGame.ins.Score++;
        _hpBar.ShowSliding(StartHp, Hp);
        if(Hp <= 0)
        {
            eState = 2;
            EnemyBoomEvent();
        }
    }

    public void DestroyEnemy()
    {
        Destroy(_hpBar.gameObject);
        Destroy(gameObject);
    }

    void EnemyBoomEvent()
    {
        //적의 죽음 등의 이벤트 호출

        //호출 종료 후의 오브젝트 제거
        DestroyEnemy();
    }
}
