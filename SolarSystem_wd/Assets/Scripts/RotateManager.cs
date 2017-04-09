using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateManager : MonoBehaviour 
{
    //read data from uimanager
    UIManager uiManager;

    public Transform[] m_ObjPlanets;

    private PlanetsValues[] tempData;

   

    void Start()
    {
        uiManager = UIManager.ui_instance;

        tempData = new PlanetsValues[m_ObjPlanets.Length];
        for (int i = 0; i < 8; i++)
        {
            tempData[i] = new PlanetsValues();
            tempData[i].parameters = new ParameterValueItems();
        }
    }

    void Update()
    {
        SetPlantsRotateData();
    }

    void SetPlantsRotateData()
    {
        Rotate[] tempRotate = new Rotate[8];

        tempData = uiManager.m_Planets;
        
        for (int i = 0; i < m_ObjPlanets.Length; i++)
        {
            tempRotate[i] =m_ObjPlanets[i].gameObject.GetComponent<Rotate>();

            tempRotate[i].RotationTime = float.Parse(tempData[i].parameters.RotatePeriod);
            tempRotate[i].RevolutionTime = float.Parse(tempData[i].parameters.RevolutionPeriod);
            tempRotate[i].HalfLongAxis = float.Parse(tempData[i].parameters.FarSolarPoint);
            tempRotate[i].eccentricity = float.Parse(tempData[i].parameters.NearSolarPoint);

            tempRotate[i].TrackBiasAngle = float.Parse(tempData[i].parameters.TrackBiasAngle);

            tempRotate[i].repaint = true;
        }
       // print("get the ui data and set the model");
    }
}
