using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int GameType = 0;
    public float Speed = 2f;
    public float LifeTime = 5;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DeletThisObject", LifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movepos = transform.position;
        float offset = Time.deltaTime * Speed;
        switch (GameType)
        {
            case 1:
                movepos.x += (1 * offset);
                break;
        }
        transform.position = Vector2.MoveTowards(transform.position, movepos, 1);
    }

    void DeletThisObject()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("Bullet Trigger" + collision.name);
        }
    }
}
