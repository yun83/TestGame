using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : MonoBehaviour
{
    public int State = 0;

    public int PlayerHP = 0;

    [Header("이동속도")]
    public float moveSpeed = 5;
    public bool JumpCheck = false;
    public int JumpMaxCount = 50;
    private int JumpCount = 0;
    public float JumpDis = 1.25f;

    [Header("바닥 체크")]
    public bool isGround = false; 
    public LayerMask GroundLayers;
    public float GravityValue = 0.001f;
    public float GroundedOffset = 0.45f;
    public float GroundedRadius = 0.1f;
    private float verticalVelocity = 0; 

    private Animator anim;
    private int oldAnimIdx = -1;
    private BoxCollider2D Box2D;
    private Transform mainCam;

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.GetComponentInChildren<Animator>();
        Box2D = transform.GetComponent<BoxCollider2D>();
        mainCam = Camera.main.transform;

        Info.ins.UserTrans = transform;
        Info.ins.UserAnim = anim;

        State = 0;
        PlayerHP = 0;
        HP_Plus(100);
    }

    // Update is called once per frame
    void Update()
    {
        Move2D();
        cameraTracking();
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
        GroundCheck();

        movepos.x += (moveH * offset);
        Box2D.size = new Vector2(0.5f, 1f);
        Box2D.offset = new Vector2(0f, 0f);

        if (JumpCheck)
        {
            if (JumpCount < JumpMaxCount)
            {
                AnimState = 2;
                JumpCount++;
                verticalVelocity = -(Mathf.Sqrt(JumpDis * 2f * GravityValue));
            }
            else if (JumpCount < JumpMaxCount + 15)
            {
                verticalVelocity = 0;
                JumpCount++;
            }
            else
                JumpCheck = false;
        }
        else
        {
            if (isGround)
            {
                if (moveH != 0)
                {
                    AnimState = 1;
                }
                else
                {
                    AnimState = 0;
                }

                if (moveH < 0)
                    transform.eulerAngles = new Vector3(0, 180, 0);
                else if (moveH > 0)
                    transform.eulerAngles = Vector3.zero;

                if (moveV > 0.4f)
                {
                    AnimState = 2;
                    JumpCount = 0;
                    JumpCheck = true;
                    verticalVelocity = -(Mathf.Sqrt(JumpDis * 2f * GravityValue));
                }
                else if (moveV < -0.4f)
                {
                    AnimState = 3;
                    Box2D.size = new Vector2(0.5f, 0.5f);
                    Box2D.offset = new Vector2(0f, -0.25f);
                }
            }
            else
            {
                AnimState = 4;
                verticalVelocity += GravityValue;

                if (verticalVelocity > 1.5f)
                    verticalVelocity = 1.5f;

            }
        }

        movepos.y -= verticalVelocity;
        transform.position = Vector2.MoveTowards(transform.position, movepos, 1);
        PlayerAniSetting(AnimState);
    }

    void cameraTracking()
    {
        if (State == 50)
            return;

        Vector3 camMove = new Vector3(0, 0, -10);
        Vector2 temvec2 = Vector2.Lerp(mainCam.position, transform.position, Time.deltaTime);
        camMove.x = temvec2.x;
        camMove.y = temvec2.y;
        mainCam.position = camMove;
    }

    void GroundCheck()
    {
        Vector2 hitPoint = transform.position;
        hitPoint.y -= GroundedOffset;

        RaycastHit2D col = Physics2D.CircleCast(transform.position, GroundedRadius, Vector2.down, GroundedOffset, GroundLayers);

        isGround = false;
        if (col.collider != null)
        {
            verticalVelocity = 0;
            isGround = true;
        }
    }

    void PlayerAniSetting(int num)
    {
        if (oldAnimIdx != num)
        {
            oldAnimIdx = num;
            //Debug.Log(oldAnimIdx);
            Info.ins.UserAnim.SetInteger("State", num);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.transform.tag)
        {
            default:
                JumpCheck = false;
                verticalVelocity = 0;
                break;
            case "placeDie":
                Box2D.isTrigger = true;
                Invoke("fallingDeath", 0.2f);
                break;
        }
        //Debug.Log("Collision :: " + collision.transform.name);
    }

    void fallingDeath()
    {
        Debug.Log("fallingDeath Invoke");
        State = 50;
    }


    void HP_Plus(int hp)
    {
        PlayerHP += hp;

        //if (Info.ins.GameUi == null)
        //    Invoke("ResetHpText", 0.2f);
        //else
            Info.ins.GameUi.SetDemoText(0, "HP : " + PlayerHP.ToString());

        if (PlayerHP <= 0)
        {
            //사망처리
            State = 51;
        }
    }

    private void ResetHpText()
    {
        if (Info.ins.GameUi == null)
            Invoke("ResetHpText", 0.1f);
        else
            Info.ins.GameUi.SetDemoText(0, "HP : " + PlayerHP.ToString());
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (isGround) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
    }
}