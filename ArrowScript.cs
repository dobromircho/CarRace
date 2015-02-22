using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour
{
    Transform car;
    // Use this for initialization
    void Start()
    {
        car = GameObject.FindGameObjectWithTag("Car").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Car") == true)
        {
            transform.position = new Vector3(car.position.x, 100f, car.position.z);
            transform.rotation = car.rotation;
        }

    } 
}
