using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeRpgPlayer : MonoBehaviour
{
    private Transform mainCam;
    private BoxCollider2D Box2D;
    private Animator anim;
    private int oldAnimIdx = -1;

    [Header("이동속도")]
    private bool LookCheck = true;
    public float moveSpeed = 5;

    [Header("공격 관련")]
    public Vector2 AttOffset = new Vector2(0.14f, 0.28f);
    public Vector2 AttArea = new Vector2(2, 1.2f);
    public GameObject AttObject;
    private BoxCollider2D AttBox2D;
    private bool AttCheck = false;

    private int PlayerHP = 50;
    private HpBar PlayerHpBar;
    private Transform hpBarTrans;

    private void Awake()
    {
        anim = transform.GetComponentInChildren<Animator>();
        Box2D = transform.GetComponent<BoxCollider2D>();
        mainCam = Camera.main.transform;

        Info.ins.UserTrans = transform;
        Info.ins.UserAnim = anim;

        AttObject.GetComponent<AttTarget>().tPlayer = this;
        AttObject.transform.position = AttOffset;
        AttBox2D = AttObject.GetComponent<BoxCollider2D>();
        AttBox2D.size = AttArea;
        AttObject.SetActive(false);
        AttCheck = false;

        PlayerHpBar = Info.ins.GameUi.AddHpBar();
        hpBarTrans = PlayerHpBar.transform;
        PlayerHP = 0;
        HP_Plus(100);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move2D();
    }

    void Move2D()
    {
        int AnimState = 0;
        float moveH = 0;
        float moveV = 0;

#if UNITY_EDITOR
        moveH = Input.GetAxis("Horizontal");
        moveV = Input.GetAxis("Vertical");

#endif
        if (Info.ins.GameUi == null)
            return;

        if (Info.ins.GameUi.LeftHand.MoveFlag)
        {
            moveH = Info.ins.GameUi.LeftHand.JoyVec.x;
            moveV = Info.ins.GameUi.LeftHand.JoyVec.y;
        }


        float offset = Time.deltaTime * moveSpeed;
        Vector2 movepos = transform.position;

        movepos.x += (moveH * offset);
        movepos.y += (moveV * offset);

        if (moveH != 0)
            AnimState = 1;
        else
            AnimState = 0;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AnimState = 5;
            AttCheck = true;
        }

        if (moveH < 0)
            LookCheck = false;
        else if (moveH > 0)
            LookCheck = true;

        if (LookCheck)
            transform.eulerAngles = Vector3.zero;
        else
            transform.eulerAngles = new Vector3(0, 180, 0);

        transform.position = Vector2.MoveTowards(transform.position, movepos, 1);

        PlayerAniSetting(AnimState);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            float normalizedTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

            if (normalizedTime >= 0.9f)
            {
                AttObject.SetActive(false);
                AttCheck = false;
            }
            else if (normalizedTime >= 0.6f)
                AttObject.SetActive(true);
            else if (normalizedTime >= 0.5f)
                AttObject.SetActive(false);
            else if (normalizedTime >= 0.4f)
                AttObject.SetActive(true);
        }

        hpBarTrans.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.8f, 0));
    }

    void PlayerAniSetting(int num)
    {
        if (oldAnimIdx != num)
        {
            oldAnimIdx = num;
            //Debug.Log(oldAnimIdx);
            anim.SetInteger("State", num);
        }
    }

    void HP_Plus(int hp)
    {
        PlayerHP += hp;

        Info.ins.GameUi.SetDemoText(0, "HP : " + PlayerHP.ToString());

        if (PlayerHP <= 0)
        {
            //사망처리
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }

    private void OnDrawGizmosSelected()
    {
    }
}