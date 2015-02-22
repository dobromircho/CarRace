using UnityEngine;
using System.Collections;

public class Finish_2D : MonoBehaviour
{
    public Camera other;
    public Camera main;
    public static bool isFinish2D;
    // Use this for initialization
    void Start()
    {
        other.enabled = false;
        main.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter()
    {
        other.enabled = false;
        main.enabled = true;
        isFinish2D = true;
    }
}
