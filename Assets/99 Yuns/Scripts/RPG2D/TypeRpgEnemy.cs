using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeRpgEnemy : MonoBehaviour
{
    public TypeRpgPlayer trp;
    public HpBar hpBar;

    public bool InitCheck = false;

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
    }

    public void HP_Plus(int hp)
    {
        Hp += hp;

        if (Hp <= 0)
        {
            //»ç¸ÁÃ³¸®
        }
    }
}
