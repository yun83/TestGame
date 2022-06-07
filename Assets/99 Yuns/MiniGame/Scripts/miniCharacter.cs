using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniCharacter : MonoBehaviour
{
    private Transform ThisTrans;
    public GameObject Bullet;

    public float yAreaMax = 5;
    public float MoveSpeed = 2f;
    public float ShootDelay = 0.5f;
    public float ShootSpeed = 10f;

    private bool AttKetDown = false;
    private bool LookCheck = true;

    public Transform Shooter;
    private float shootTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        ThisTrans = this.transform;
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

        movepos.y = Mathf.Clamp(movepos.y, -yAreaMax, yAreaMax);


        if (AttKetDown)
            AnimState = 5;

        if (moveH < 0)
            LookCheck = false;
        else if (moveH > 0)
            LookCheck = true;

        //Vector3 _eulerAngles = ThisTrans.eulerAngles;
        //if (LookCheck)
        //    _eulerAngles.y = 0;
        //else
        //    _eulerAngles.y = 180;
        //ThisTrans.eulerAngles = _eulerAngles;

        if (AnimState != 5)
            ThisTrans.position = Vector2.MoveTowards(ThisTrans.position, movepos, 1);
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
            tempBullet.transform.position = Shooter.position;
        }
        shootTime += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet")
            return;

        Debug.Log("Player Trigger" + other.name);
    }
}
