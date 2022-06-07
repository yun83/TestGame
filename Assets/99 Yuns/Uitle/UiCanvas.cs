using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiCanvas : Single<UiCanvas>
{
    public VirtualJoystick LeftHand;

    public GameObject hpBar;
    /// <summary>
    /// 0 HP, 1 Speed, 2 Score
    /// </summary>
    public Text[] DemoText;

    public Transform insGroup;
    public Button AttButton;
    public bool AttKeyDown = false;

    // Start is called before the first frame update
    void Start()
    {
        AttKeyDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDemoText(int idx, string msg)
    {
        DemoText[idx].text = msg;
    }

    public HpBar AddHpBar(string NicName = "")
    {

        Transform temp = Instantiate(hpBar).transform;
        HpBar ret = temp.GetComponent<HpBar>();
        temp.parent = insGroup;

        if (NicName == "" || NicName == null)
            ret.NicName.gameObject.SetActive(false);
        else
            ret.NicName.text = NicName;

        return ret;
    }

    public void OnAttKeyDown() { AttKeyDown = true; }
    public void OnAttKeyUp() { AttKeyDown = false; }

}