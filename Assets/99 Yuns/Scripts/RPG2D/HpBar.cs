using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Image Bar;
    public Text NicName;

    public void ShowSliding(int Max, int Now)
    {
        int trunk = Com.ins.percentageOfTotal(Now, Max);
        Bar.fillAmount = (trunk * 0.01f);
    }

    public void ShowSliding(float Max, float Now)
    {
        float trunk = Com.ins.percentageOfTotal(Now, Max);
        Bar.fillAmount = (trunk * 0.01f);
    }
}