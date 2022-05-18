using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttTarget : MonoBehaviour
{
    //public TypeRpgPlayer tPlayer;

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    switch (other.tag)
    //    {
    //        case "Enemy":
    //            Debug.Log("Enter2D :: " + other.name);
    //            break;
    //    }
    //}

    //private void OnTriggerStay2D(Collider2D other)
    //{
    //    switch (other.tag)
    //    {
    //        case "Enemy":
    //            Debug.Log("Stay2D :: " + other.name);
    //            break;
    //    }
    //}
                                                                                                              
    private void OnTriggerExit2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Enemy":
                Debug.Log("Exit2D :: " + other.name);
                TypeRpgEnemy TRE = other.gameObject.GetComponent<TypeRpgEnemy>();
                if (TRE != null)
                    TRE.HitCall();
                break;
        }
    }
}