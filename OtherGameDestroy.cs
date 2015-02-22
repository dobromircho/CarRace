using UnityEngine;
using System.Collections;

public class OtherGameDestroy : MonoBehaviour
{

   
    void Update()
    {
        if (Finish_2D.isFinish2D)
        {
            Destroy(gameObject, 1); 
        }
    }
}
