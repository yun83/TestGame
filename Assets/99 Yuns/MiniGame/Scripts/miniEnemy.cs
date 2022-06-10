using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class miniEnemy : MonoBehaviour
{
    public SpriteRenderer nowSpr;
    int eState = 0;
    public float MoveSpeed = 3f;
    public int Hp = 0;
    private int StartHp = 0;
    public float LifeTime = 20;
    private bool aniEnd = false;
    int anicnt = 0;
    float colorA = 1;

    public HpBar _hpBar;
    // Start is called before the first frame update
    private void Awake()
    {
    }

    public void InitEnemy(int _Lv, Vector3 _pos)
    {
        if (_hpBar != null)
            Destroy(_hpBar.gameObject);

        aniEnd = false;
        transform.position = _pos;
        switch (_Lv)
        {
            case 0:
            case 1:
                StartHp = 2;
                Hp = 2;
                MoveSpeed = 3f;
                break;
            case 2:
                StartHp = 3;
                Hp = 3;
                MoveSpeed = 3f;
                break;
            case 3:
                StartHp = 4;
                Hp = 4;
                MoveSpeed = 3.5f;
                break;
            case 4:
                StartHp = 5;
                Hp = 5;
                MoveSpeed = 4f;
                break;
            default:
                StartHp = 7;
                Hp = 7;
                MoveSpeed = 4.5f;
                break;
        }

        if(nowSpr == null)
            nowSpr = GetComponent<SpriteRenderer>();

        colorA = 1;
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
            StartCoroutine(EnemyBoomEvent());
        }
    }

    public void DestroyEnemy()
    {
        Destroy(_hpBar.gameObject);
        Destroy(gameObject);
    }

    IEnumerator EnemyBoomEvent()
    {
        //적의 죽음 등의 이벤트 호출
        Destroy(gameObject.GetComponent<Collider2D>());

        yield return null;

        while (!aniEnd)
        {

            yield return new WaitForEndOfFrame();

            nowSpr.color = new Color(1, 1, 1, colorA);
            colorA -= 0.005f;

            anicnt++;
            if (anicnt >= 200)
            {
                aniEnd = true;
            }
        }

        yield return null;
        //호출 종료 후의 오브젝트 제거
        DestroyEnemy();
    }
}
