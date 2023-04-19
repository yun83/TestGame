using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : Single<Info>
{
    public Transform UserTrans;
    public Animator UserAnim;

    public int GameMainState = 0;

    private void Awake()
    {
    }
}