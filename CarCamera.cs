using UnityEngine;
using System.Collections;

public class CarCamera : MonoBehaviour
{
    GameObject car;
    public float distance;
    public float height;
    public float rotationDumping ;
    public float heightDumping;
    public float zoomRatio;
    private Vector3 rotationVector;
    public float defaultFOV ;
    public float zoomHeight;
    Vector3 localVelocity;
    int get = 0;

    void Update()
    {
        if (get == 0 && GameObject.FindGameObjectWithTag("Car"))
        {
            car = GameObject.FindGameObjectWithTag("Car");
            get = 1;
        }
    }

    void LateUpdate()
    {

        
        if (get == 1)
        {
            var wantedAngle = rotationVector.y;
            var wantedHeight = car.transform.position.y + height;
            var myAngle = transform.eulerAngles.y;
            var myHeight = transform.position.y;
            myAngle = Mathf.LerpAngle(myAngle, wantedAngle, rotationDumping * Time.deltaTime);
            myHeight = Mathf.Lerp(myHeight, wantedHeight, heightDumping * Time.deltaTime);
            var currentRotation = Quaternion.Euler(0f, myAngle, 0f);
            transform.position = new Vector3(car.transform.position.x, myHeight, car.transform.position.z );
            transform.position -= currentRotation * Vector3.forward * distance;
            transform.LookAt(car.transform);
        }
        
          
    }

    void FixedUpdate()
    {
        if (get == 1)      
        {
            localVelocity = car.transform.InverseTransformDirection(car.rigidbody.velocity);
            var accelaration = car.rigidbody.velocity.magnitude;
            //if (localVelocity.z < -10f)
            //{
            //    rotationVector = new Vector3(car.transform.eulerAngles.x, car.transform.eulerAngles.y + 180f, car.transform.eulerAngles.z);
            //}
            
            if (Input.GetKey(KeyCode.G))
            {
                rotationVector = new Vector3(car.transform.eulerAngles.x, car.transform.eulerAngles.y + 80f, car.transform.eulerAngles.z);
            }
            else
            {
                rotationVector = new Vector3(car.transform.eulerAngles.x, car.transform.eulerAngles.y, car.transform.eulerAngles.z);
            }
            height = 1.5f + (car.rigidbody.velocity.magnitude / zoomHeight);
            camera.fieldOfView = defaultFOV + accelaration * zoomRatio;
        }
    }
}
