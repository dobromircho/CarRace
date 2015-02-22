using UnityEngine;
using System.Collections;

public class CarConrtrol_2D : MonoBehaviour
{
    public Transform startPoint;
    float speed2D = 6;
    void Start()
    {
        audio.pitch = 0.5f;
        speed2D = 0;
    }

    // Update is called once per frame
    void Update() 
    {
        if (EntranceScript.isEnter)
        {
            speed2D = 5;
        }
        float speedFactor = rigidbody.velocity.magnitude /20;
        rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed2D, rigidbody.velocity.y);
        audio.pitch = Mathf.Lerp(0.5f, 2, speedFactor);
    	
    }

    

}
