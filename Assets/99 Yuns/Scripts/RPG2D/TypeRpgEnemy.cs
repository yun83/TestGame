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

    // Start is called before the first frame update
    void Start()
    {
        
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
    }

    public void HitCall()
    {
        HP_Plus(-10);
    }

    public void HP_Plus(int hp)
    {
        Hp += hp;
        hpBar.ShowSliding(MaxHp, Hp);
    }
}
