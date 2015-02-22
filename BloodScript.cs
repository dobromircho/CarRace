using UnityEngine;
using System.Collections;

public class BloodScript : MonoBehaviour
{

    void Start()
    {
        audio.Play();
        Destroy(gameObject, 2);
    }

}
