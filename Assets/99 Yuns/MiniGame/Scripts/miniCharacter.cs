using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniCharacter : MonoBehaviour
{
    private Transform ThisTrans;
    public GameObject Bullet;
    public Transform BulletGroup;

    public Vector2 screenSize;
    public float MoveSpeed = 2f;
    public float ShootDelay = 0.5f;
    public float ShootSpeed = 10f;
    public int bulletDamege = 1;

    private bool AttKetDown = false;
    //private bool LookCheck = true;

    public Transform Shooter;
    private float shootTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        ThisTrans = this.transform;
        if(BulletGroup == null)
        {
            BulletGroup = new GameObject("BulletGroup").transform;
        }
        screenSize = MiniGame.ins.screenSize;
        SpeedShow();
    }

    // Update is called once per frame
    void Update()
    {
        Move2D();
        ShooterLogic();
    }

    void Move2D()
    {
        int AnimState = 0;
        float moveH = 0;
        float moveV = 0;

#if UNITY_EDITOR
        moveH = Input.GetAxis("Horizontal");
        moveV = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttKetDown = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            AttKetDown = false;
        }
#endif
        if (UiCanvas.ins == null)
            return;

        if (UiCanvas.ins.LeftHand.MoveFlag)
        {
            moveH = UiCanvas.ins.LeftHand.JoyVec.x;
            moveV = UiCanvas.ins.LeftHand.JoyVec.y;
        }
        float offset = Time.deltaTime * MoveSpeed;
        Vector2 movepos = ThisTrans.position;

        movepos.x += (moveH * offset);
        movepos.y += (moveV * offset);

        movepos.y = Mathf.Clamp(movepos.y, -screenSize.y, (screenSize.y - 1));


        if (AttKetDown)
            AnimState = 5;

        //if (moveH < 0)
        //    LookCheck = false;
        //else if (moveH > 0)
        //    LookCheck = true;
        //Vector3 _eulerAngles = ThisTrans.eulerAngles;
        //if (LookCheck)
        //    _eulerAngles.y = 0;
        //else
        //    _eulerAngles.y = 180;
        //ThisTrans.eulerAngles = _eulerAngles;

        if (AnimState != 5)
        {
            ThisTrans.position = Vector2.MoveTowards(ThisTrans.position, movepos, 1);
        }

    }

    void ShooterLogic()
    {
        if (ShootDelay < 0.2f)
            ShootDelay = 0.2f;
        if (shootTime > ShootDelay)
        {
            //Shooter
            shootTime = 0;
            GameObject tempBullet = Instantiate(Bullet);
            Bullet bulletScripts = tempBullet.AddComponent<Bullet>();
            bulletScripts.Speed = ShootSpeed;
            bulletScripts.GameType = 1;
            bulletScripts.Damege = 1;
            tempBullet.transform.position = Shooter.position;

            tempBullet.name = "Bullet_D[" + bulletDamege + "] S[" + ShootSpeed + "]";
            if (BulletGroup != null)
                tempBullet.transform.parent = BulletGroup;
        }
        shootTime += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet")
            return;

        if(other.name == "DropItem")
        {
            miniDropItem mItem = other.gameObject.GetComponent<miniDropItem>();
            if (mItem.ItemType == 0)
            {
                MoveSpeed += 0.5f;
            }
            SpeedShow();
            Destroy(other.gameObject);
        }
        Debug.Log("Player Trigger [" + other.name + "]");
    }

    void SpeedShow()
    {
        string textStr = string.Format("Speed : {0:0.00}", MoveSpeed);
        UiCanvas.ins.SetDemoText(1, textStr);
    }
}
