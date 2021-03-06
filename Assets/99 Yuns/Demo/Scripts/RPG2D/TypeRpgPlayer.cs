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
    private bool AttKetDown = false;

    private void Awake()
    {
        anim = transform.GetComponentInChildren<Animator>();
        Box2D = transform.GetComponent<BoxCollider2D>();
        mainCam = Camera.main.transform;

        Info.ins.UserTrans = transform;
        Info.ins.UserAnim = anim;

        AttObject.transform.position = AttOffset;
        AttBox2D = AttObject.GetComponent<BoxCollider2D>();
        AttBox2D.size = AttArea;
        AttObject.SetActive(false);
        AttKetDown = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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

        if (UiCanvas.ins.AttKeyDown)
        {
            AttKetDown = true;
        }
        else
        {
            AttKetDown = false;
        }

        float offset = Time.deltaTime * moveSpeed;
        Vector2 movepos = transform.position;

        movepos.x += (moveH * offset);
        movepos.y += (moveV * offset);

        if (moveH != 0 || moveV != 0)
        {
            AttObject.SetActive(false);
            AnimState = 1;
        }
        else
            AnimState = 0;


        if (AttKetDown)
            AnimState = 5;

        if (moveH < 0)
            LookCheck = false;
        else if (moveH > 0)
            LookCheck = true;

        if (LookCheck)
            transform.eulerAngles = Vector3.zero;
        else
            transform.eulerAngles = new Vector3(0, 180, 0);
        
        if(AnimState != 5)
            transform.position = Vector2.MoveTowards(transform.position, movepos, 1);
        else
        {
            HitCheckLogic();
        }

        PlayerAniSetting(AnimState);
    }

    void HitCheckLogic()
    {
        float normalizedTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        int vealueToReduce = (int)normalizedTime;
        normalizedTime -= vealueToReduce;
        //Debug.Log(normalizedTime);
        
        if (normalizedTime >= 0.4f && normalizedTime < 0.5f)
        {
            AttObject.SetActive(true);
        }
        else if(normalizedTime >= 0.6f && normalizedTime < 0.9f)
        {
            AttObject.SetActive(true);
        }
        else
        {
            AttObject.SetActive(false);
        }
    }

    void cameraTracking()
    {
        Vector3 camMove = new Vector3(0, 0, -10);
        Vector2 temvec2 = Vector2.Lerp(mainCam.position, transform.position, Time.deltaTime);
        camMove.x = temvec2.x;
        camMove.y = temvec2.y;
        mainCam.position = camMove;
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


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }

    private void OnDrawGizmosSelected()
    {
    }
}