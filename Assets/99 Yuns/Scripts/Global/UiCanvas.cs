using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiCanvas : MonoBehaviour
{
    public VirtualJoystick LeftHand;

    public GameObject hpBar;
    /// <summary>
    /// 0 HP, 1 Speed, 2 Score
    /// </summary>
    public Text[] DemoText;

    public Transform insGroup;

    // Start is called before the first frame update
    void Start()
    {
        Info.ins.GameUi = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDemoText(int idx, string msg)
    {
        DemoText[idx].text = msg;
    }

    public HpBar AddHpBar()
    {
        Transform temp = Instantiate(hpBar).transform;
        temp.parent = insGroup;
        return temp.GetComponent<HpBar>();
    }
}