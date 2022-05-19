using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAlphaChange : MonoBehaviour
{
    SpriteRenderer NowSpr;
    float AlphaData = 1;
    float SumAlpha = 1;

    private void Start()
    {
        NowSpr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if(SumAlpha > AlphaData)
            SumAlpha -= 0.01f;
        if(AlphaData > SumAlpha)
            SumAlpha += 0.01f;

        NowSpr.color = new Color(1, 1, 1, SumAlpha);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            AlphaData = 0.3f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            AlphaData = 1;
        }
    }
}
