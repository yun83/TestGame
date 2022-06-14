using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int GameType = 0;
    public float Speed = 2f;
    public float LifeTime = 5;
    public int Damege = 1;
    private Transform ThisTrans;
    private float sizeW;
    private Camera mCam;
    // Start is called before the first frame update
    void Start()
    {
        ThisTrans = transform;
        sizeW = Screen.width;
        mCam = Camera.main;
        Invoke("DeletThisObject", LifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        moveBullet();
    }

    void moveBullet()
    {
        if (ThisTrans == null)
            ThisTrans = transform;
        if (mCam == null)
            mCam = Camera.main;

        Vector2 movepos = ThisTrans.position;
        float v = Input.GetAxisRaw("Vertical");
        float offset = Time.deltaTime * Speed;
        switch (GameType)
        {
            case 1:
                movepos.x += offset;
                break;
        }
        ThisTrans.position = Vector2.MoveTowards(ThisTrans.position, movepos, 1);

        //화면 벗어남 체크
        if (ThisTrans.position.x > MiniGame.ins.screenSize.x)
        {
            DeletThisObject();
        }
    }

    void DeletThisObject()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            //Debug.Log("Bullet Trigger" + collision.name);
            collision.GetComponent<miniEnemy>().AddHp(-Damege);
            DeletThisObject();
        }
        if(collision.name == "Wall")
        {
            DeletThisObject();
        }
    }

}
