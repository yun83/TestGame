using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeRpgEnemy : MonoBehaviour
{
    public TypeRpgPlayer trp;
    public HpBar hpBar;

    public bool InitCheck = false;

    public string NicName = "";
    public int MaxHp;
    public int Hp;
    public int Att;
    public int Def;
    public int Agi;

    private bool HitCheck = false;
    private float HitTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        HitCheck = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (InitCheck)
        {
            hpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.8f, 0));
        }
        else
        {
            Hp = MaxHp;
        }

        if (HitCheck)
        {
            if(HitTime < Time.time)
            {
                HitCheck = false;
            }
        }
    }

    public void HP_Plus(int hp)
    {
        Hp += hp;
        hpBar.ShowSliding(MaxHp, Hp);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            //Debug.Log("Enemy ------ Enter2D :: " + collision.name + ", " + collision.tag + " :: Hit Time :: " + Time.time);
            if (!HitCheck)
            {
                HP_Plus(-10);
                HitCheck = true;
                HitTime = Time.time + 0.19f;
            }
        }
    }
}
