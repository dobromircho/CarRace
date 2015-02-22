using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveZombies : MonoBehaviour
{

    float speed = 4f;
    float jump = 20f;
    public AudioClip[] sounds;
    public GameObject blood;
    public static bool isHit;
    

    void Start()
    {
        Destroy(gameObject, 5);
    }

    void Update()
    {
       
    }
   
    void FixedUpdate()
    {
       
        if (rigidbody.velocity.magnitude < 3)
        {
            transform.Translate(Vector3.forward  * Time.deltaTime * 2);
            transform.Rotate(Vector3.up , 1);
           
        }
    }

    void OnCollisionEnter(Collision coll) 
    {
        if (coll.transform.tag == "Car")
        {
            rigidbody.AddForce(Vector3.up * jump, ForceMode.Impulse);
            Instantiate(blood, transform.position, Quaternion.identity);
            SoundPlay();
            isHit = true;
            Destroy(gameObject,2f);
        }
    }

    void SoundPlay()
    {
        audio.clip = sounds[Random.Range(0, sounds.Length)];
        audio.Play(5000);
    }
}
