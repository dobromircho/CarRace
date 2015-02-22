using UnityEngine;
using System.Collections;

public class EntranceScript : MonoBehaviour
{
    public Camera main;
    public Camera other;
    public static bool isEnter;

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Car")
        {
            main.enabled = false;
            other.enabled = true;
            isEnter = true;
        }
       
    }

    void Update()
    {
        if (Finish_2D.isFinish2D)
        {
            Destroy(gameObject, 1);
        }
    }
    
}
