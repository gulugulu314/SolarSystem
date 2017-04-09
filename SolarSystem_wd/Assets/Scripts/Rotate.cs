using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{


    public float RotationTime;  //自转周期
    private float RotationAngle;
    private Vector3 RotationAxis = new Vector3(0, 1, 0);
    public bool IsRotFromWestToEast = true;

    public Transform Solar;
    public float RevolutionTime;
    public float HalfLongAxis;
    public float eccentricity;
    public float TrackBiasAngle;

    private float HalfShortAxis;  //近日点

    public float trackwidth = 0.2f;
    public Color trackColor;
    public int trackpointcounts = 360;

    private float nearSolarPoint;
    private float angled;
    private List<Vector3> Points = new List<Vector3>();
    private LineRenderer linerender;

    public bool repaint;

    void Start()
    {
        trackwidth = 0.1f;
        trackColor = Color.yellow;
        trackpointcounts = 360;

        if (GetComponent<LineRenderer>() == null)
        {
            this.gameObject.AddComponent<LineRenderer>();
        }
        linerender = GetComponent<LineRenderer>();

        InitPosition();

        StartCoroutine(CalculatePoints());
        DrawLine();
    }

    void Update()
    {
        if (repaint)
        {
            ClearPoints();
            StartCoroutine(CalculatePoints());
            DrawLine();

            repaint = false;
        }
        repaint = false;
        Planet_Rotaiton(transform.position, RotationAxis, RotationTime, IsRotFromWestToEast);
        Revolution(RevolutionTime, eccentricity, HalfLongAxis, TrackBiasAngle);
    }


    public void Planet_Rotaiton(Vector3 point, Vector3 axis, float rotationTime, bool isrotfromwest2east)
    {
        //自转的速度，即每秒钟转多少度
        RotationAngle = 360 / (rotationTime * 24 * 3600);
        if (RotationAngle <= 0.0000001f)
        {
            RotationAngle = 0.0000001f;
        }
        if (!isrotfromwest2east)
        {
            RotationAngle = -RotationAngle;
        }
        transform.RotateAround(point, axis, RotationAngle);
    }


    //set the initial position
    private void InitPosition()
    {
        nearSolarPoint = HalfLongAxis - Mathf.Sqrt(HalfLongAxis * HalfLongAxis - HalfShortAxis * HalfShortAxis);
        Vector3 p = Solar.position + Vector3.right * nearSolarPoint;
        transform.position = new Vector3(p.x, Solar.position.y, p.z);
    }

    //Revolution
    private void Revolution(float _revloutionTime, float _eccentricity, float _halfLongPoint, float _trackBiasAngle)
    {
        //guarantee the revolutiontime not equal zero
        if (_revloutionTime <= 0)
        {
            _revloutionTime = 1000f;
        }

        //calculate the angle velocity,imagine the velocity is const
        float anglevelocity = 2 * Mathf.PI / (_revloutionTime);
        if (anglevelocity <= 0.00000001f)
            anglevelocity = 0.00000001f;

        angled += (anglevelocity * Time.deltaTime) % 360;

        //calculate the halfshortaxis with the halflongaxis and eccentricity
        HalfShortAxis = Mathf.Sqrt(_halfLongPoint * _halfLongPoint * (1 - _eccentricity * _eccentricity));

        //calculate points by angle
        float posx = _halfLongPoint * Mathf.Cos(angled * Mathf.Deg2Rad);
        float posz = HalfShortAxis * Mathf.Sin(angled * Mathf.Deg2Rad);

        //set track plane

        Quaternion q = Quaternion.Euler(0, 0, _trackBiasAngle);
        Vector3 newpos = q * (new Vector3(posx, 0, posz));

        //set position
        transform.position = newpos;
    }


    private void DrawLine()
    {
        linerender.material = new Material(Shader.Find("Particles/Additive"));
        linerender.startColor = trackColor;
        linerender.endColor = trackColor;
        linerender.startWidth = trackwidth;
        linerender.endWidth = trackwidth;

        linerender.numPositions = Points.Count + 1;
        for (int i = 0; i < Points.Count; i++)
        {
            linerender.SetPosition(i, Points[i]);
        }
        linerender.SetPosition(Points.Count, Points[0]);
    }

    private IEnumerator CalculatePoints()
    {
        HalfShortAxis = Mathf.Sqrt(HalfLongAxis * HalfLongAxis * (1 - eccentricity * eccentricity));
        float m_theta = 360 / trackpointcounts;

        for (int i = 0; i < trackpointcounts; i++)
        {
            float posx = HalfLongAxis * Mathf.Cos(i * m_theta * Mathf.Deg2Rad);
            float posz = HalfShortAxis * Mathf.Sin(i * m_theta * Mathf.Deg2Rad);
            Quaternion q = Quaternion.Euler(0, 0, TrackBiasAngle);
            Vector3 endpoint = q * (new Vector3(posx, Solar.position.y, posz));
            Points.Add(endpoint);
        }
        yield return Points;
    }

    private void ClearPoints()
    {
        Points.Clear();
    }
}
