using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniDropItem : MonoBehaviour
{
    public Transform TargetObj;
    public Vector3 StartPoint;
    public float journeyTime = 2.0f;
    private float startTime;
    public bool moveStart = false;

    public int ItemType = 0;
    Vector3 cenerBogan = new Vector3(0, 0.5f, 0);

    // Start is called before the first frame update
    void Start()
    {
        TargetObj = MiniGame.ins.PlayerClone.transform;
        gameObject.name = "DropItem";
        int check = Random.Range(0, 500);
        if(check < 250)
            cenerBogan = new Vector3(0, -0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        moveTarget();
    }

    public void StartMove()
    {
        moveStart = true;
        startTime = Time.time;
    }

    void moveTarget()
    {
        if (TargetObj == null)
            return;
        if (!moveStart)
            return;

        //Vector3 center = (StartPoint + TargetObj.position) * 0.5f;
        //center -= cenerBogan;
        //Vector3 StartCenter = StartPoint - center;
        //Vector3 EndCenter = TargetObj.position - center;
        //float fracComplete = (Time.time - startTime) / journeyTime;
        //transform.position = Vector3.Slerp(StartCenter, EndCenter, fracComplete);
        //transform.position += center;

        transform.position = Vector3.MoveTowards(transform.position, TargetObj.position, journeyTime);
    }
}
