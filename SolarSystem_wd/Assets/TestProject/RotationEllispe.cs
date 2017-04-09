using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationEllispe : MonoBehaviour
{
    public Transform aroundPoint;
    public float halflongaxis;
    public float halfshortaxis;
    public float anglespeed;

    private float nearSolarPoint;
    private float angled;

    private List<Vector3> points = new List<Vector3>();
    private LineRenderer linerender;

	// Use this for initialization
	void Start () 
    {
        if (GetComponent<LineRenderer>() == null)
        {
            this.gameObject.AddComponent<LineRenderer>();
        }
        linerender = GetComponent<LineRenderer>();  //get linerender componets;

        nearSolarPoint = halflongaxis- Mathf.Sqrt(halflongaxis * halflongaxis - halfshortaxis * halfshortaxis)/2;
        Vector3 p = aroundPoint.rotation * Vector3.right * nearSolarPoint;
        transform.position = new Vector3(p.x, aroundPoint.position.y, p.y); //set the initial point;

        CalculatePoints();
	}
	
	// Update is called once per frame
	void Update () 
    {
        angled += (anglespeed * Time.deltaTime) % 360;  //set the angles;
        float posx = halflongaxis * Mathf.Cos(angled * Mathf.Deg2Rad);
        float posz = halfshortaxis * Mathf.Sin(angled * Mathf.Deg2Rad);
        transform.position = new Vector3(posx, aroundPoint.position.y, posz);

        //draw the track
        //CalculatePoints();
        DrawLine();
	}

    void DrawLine()
    {
        linerender.material = new Material(Shader.Find("Unlit/Color"));

        linerender.startColor = Color.green;
        linerender.endColor = Color.green;

        linerender.startWidth = 0.01f;
        linerender.endWidth = 0.01f;

        linerender.numPositions = points.Count+1;
        for (int i = 0; i < points.Count; i++)
        {
            linerender.SetPosition(i, points[i]);
        }
        linerender.SetPosition(points.Count, points[0]);
        
    }

    void CalculatePoints()
    {
        Vector3 p = aroundPoint.rotation * Vector3.right * nearSolarPoint;
        Vector3 firstPoint = aroundPoint.position + p;
        points.Add(firstPoint);
        print(firstPoint);
        
        int countPoints = 360;
        float m_theta = 360/countPoints;

        for (int i = 1; i < countPoints; i++)
        {
            float posx = halflongaxis * Mathf.Cos(i * m_theta * Mathf.Deg2Rad);
            float posz = halfshortaxis * Mathf.Sin(i * m_theta * Mathf.Deg2Rad);
            Vector3 endpoint = new Vector3(posx, aroundPoint.position.y, posz);
            points.Add(endpoint);
        }
        print(points.Count);
    }
}
