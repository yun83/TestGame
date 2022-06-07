using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniEnemy : MonoBehaviour
{
    public float MoveSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movepos = transform.position;
        float offset = Time.deltaTime * MoveSpeed;
        movepos.x += (-1 * offset);
        transform.position = Vector2.MoveTowards(transform.position, movepos, 1);
    }
}
