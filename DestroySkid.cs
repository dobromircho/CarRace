using UnityEngine;
using System.Collections;

public class DestroySkid : MonoBehaviour
{
    MeshRenderer mark;
    void Start()
    {

        Destroy(gameObject, 2f);
    }
}
 