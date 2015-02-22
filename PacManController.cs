using UnityEngine;
using System.Collections;

public class PacManController : MonoBehaviour
{
   
    public float speed;
    float v;
    float h;
    public static bool isOver;
    void Start()
    {

    }

   
    void FixedUpdate()
    {
        if (!isOver)
        {
            if (Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Horizontal") == 0)
            {
                v = Input.GetAxisRaw("Vertical");
            }
            else if (Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") == 0)
            {
                h = Input.GetAxisRaw("Horizontal");
            }
            else
            {
                v = 0;
                h = 0;
            }


            Vector3 move = new Vector3(h, 0, v);
            transform.position += move * speed;
            transform.forward += move;
        }
    }

    
}