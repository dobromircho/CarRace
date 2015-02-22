using UnityEngine;
using System.Collections;

public class CheckPointManager : MonoBehaviour
{
    public  bool triggerIsEnter;
    public Collider others;
  
    void Start()
    {

    }

    void LateUpdate()
    {
        triggerIsEnter = false;
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car")
        {
            others = other;
            triggerIsEnter = true;
        }
       
    }
}
