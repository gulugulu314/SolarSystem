using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {


    public float anglespeed = 0f;
    public Transform aroundPoint;

    public float aroundRadius;
    private float angled;

	// Use this for initialization
	void Start () 
    {
		//设置初始位置
        Vector3 p = aroundPoint.rotation * Vector3.forward * aroundRadius;
        transform.position = new Vector3(p.x, aroundPoint.position.y, p.z);
	}
	
	// Update is called once per frame
	void Update () 
    {
        angled += (anglespeed * Time.deltaTime) % 360;
        float posx = aroundRadius * Mathf.Cos(angled * Mathf.Deg2Rad);
        float posz = aroundRadius * Mathf.Sin(angled * Mathf.Deg2Rad);
        transform.position = new Vector3(posx, aroundPoint.transform.position.y, posz) + aroundPoint.position;
		
	}

    void OnDrawGizmos()
    {
        Matrix4x4 defaultMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = Color.green;

        float m_theta = 0.1f;
        float m_radius = 1f;

        Vector3 startpoint = Vector3.zero;
        Vector3 firstpoint = Vector3.zero;

        for (float theta = 0; theta < 2 * Mathf.PI; theta += m_theta)
        {
            float x = m_radius * Mathf.Cos(theta);
            float z = m_radius * Mathf.Sin(theta);

            Vector3 endpoint = new Vector3(x, transform.position.y, z);

            if (theta == 0)
            {
                startpoint = endpoint;
            }
            else
            {
                Gizmos.DrawLine(firstpoint, endpoint);
            }
            firstpoint = endpoint;            
        }
        Gizmos.DrawLine(firstpoint, startpoint);

    }
}
