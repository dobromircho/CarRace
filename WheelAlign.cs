using UnityEngine;
using System.Collections;

public class WheelAlign : MonoBehaviour
{

   public  WheelCollider coll;
    public GameObject SlipPrefab;
    public float RotationValue = 0.0f;
    public float markWidth = 0.2f;
    public Material skidMaterial;
    int skidding;
    private Vector3[] lastPos = new Vector3[2];
    public bool rearWhl;
    void Start()
    {
        
    }
    void Update()
    {
        if (networkView.isMine)
        {
            RaycastHit hit;
            Vector3 ColliderCenterPoint = coll.transform.TransformPoint(coll.center);
            if (Physics.Raycast(ColliderCenterPoint, -coll.transform.up, out hit, coll.suspensionDistance + coll.radius))
            {
                transform.position = hit.point + (coll.transform.up * coll.radius);
            }
            else
            {
                transform.position = ColliderCenterPoint - (coll.transform.up * coll.suspensionDistance);
            }
            WheelHit CorrespondingGroundHit;
            coll.GetGroundHit(out CorrespondingGroundHit);
            float rpm = coll.rpm;
            if (Mathf.Abs(CorrespondingGroundHit.sidewaysSlip) > 3 || rpm < 100 && Input.GetAxis("Vertical") > 0 && hit.collider && rearWhl)
            {
                if (SlipPrefab)
                {
                    Instantiate(SlipPrefab, CorrespondingGroundHit.point, Quaternion.identity);
                    audio.Play();
                    SkidMesh();
                }
                else
                {
                    skidding = 0;
                }
            }
        }
        

    }

    void SkidMesh()
    {
        
        WheelHit Hit;
        coll.GetGroundHit(out Hit);
        GameObject mark = new GameObject("Mark");
        mark.AddComponent<MeshFilter>();
        mark.AddComponent<MeshRenderer>();
        mark.AddComponent<DestroySkid>();
        Mesh markMesh = new Mesh();
        Vector3[] vertices = new Vector3[4];
        int[] triangles = { 0, 1, 2, 0, 2, 3 };
        if (skidding == 0 )
        {
            vertices[0] = Hit.point + Quaternion.Euler(coll.transform.eulerAngles.x, coll.transform.eulerAngles.y, coll.transform.eulerAngles.z) * new Vector3(markWidth, 0.02f, 0f);
            vertices[1] = Hit.point + Quaternion.Euler(coll.transform.eulerAngles.x, coll.transform.eulerAngles.y, coll.transform.eulerAngles.z) * new Vector3(-markWidth, 0.02f, 0f);
            vertices[2] = Hit.point + Quaternion.Euler(coll.transform.eulerAngles.x, coll.transform.eulerAngles.y, coll.transform.eulerAngles.z) * new Vector3(-markWidth, 0.02f, 0f);
            vertices[3] = Hit.point + Quaternion.Euler(coll.transform.eulerAngles.x, coll.transform.eulerAngles.y, coll.transform.eulerAngles.z) * new Vector3(markWidth, 0.02f, 0f);
            lastPos[0] = vertices[2];
            lastPos[1] = vertices[3];
            skidding = 1;
        }
        else
        {
            vertices[1] = lastPos[0];
            vertices[0] = lastPos[1];
            vertices[2] = Hit.point + Quaternion.Euler(coll.transform.eulerAngles.x, coll.transform.eulerAngles.y, coll.transform.eulerAngles.z) * new Vector3(-markWidth, 0.02f, 0f);
            vertices[3] = Hit.point + Quaternion.Euler(coll.transform.eulerAngles.x, coll.transform.eulerAngles.y, coll.transform.eulerAngles.z) * new Vector3(markWidth, 0.02f, 0f);
            lastPos[0] = vertices[2];
            lastPos[1] = vertices[3]; 
        }
        
        markMesh.vertices = vertices;
        markMesh.triangles = triangles;
        markMesh.RecalculateNormals();
        Vector2[] uvm = new Vector2[markMesh.vertices.Length];
        for (int i = 0; i < uvm.Length ; i++)
        {
            uvm[i] = new Vector2(markMesh.vertices[i].x, markMesh.vertices[i].z);
        }
        markMesh.uv = uvm;
        mark.GetComponent<MeshFilter>().mesh = markMesh;
        mark.renderer.material = skidMaterial;
       
    }
}
