//read data form jason ,
//save user's data into json and update the data;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using LitJson;
using UnityEngine.UI;


public class DataManager : MonoBehaviour
{
    public static DataManager inst;

    public PlanetsValues[] _planets;
    public bool isDataReady;

    void Awake()
    {
        inst = this;
    }

	void Start () 
    {
       _planets = new PlanetsValues[8];
       isDataReady = false;
	}

    //read data from json file
    public PlanetsValues[] ReadDataFromJosn(string path)
    {
        print("read data from json....");
        if (!File.Exists(path))
        {
            return null;
        }
        string json = File.ReadAllText(path, Encoding.UTF8);
        JsonData data = JsonMapper.ToObject(json);

        for (int i = 0; i < data.Count; i++)
        {
            _planets[i] = new PlanetsValues();
            _planets[i].parameters = new ParameterValueItems();
            
            _planets[i].name = data[i]["name"].ToString();

            _planets[i].parameters.RotatePeriod = data[i]["parameters"]["RotatePeriod"].ToString();
            _planets[i].parameters.BiasAngle = data[i]["parameters"]["BiasAngle"].ToString();
            _planets[i].parameters.RotateDirection = data[i]["parameters"]["RotateDirection"].ToString();//RotateDirection
            _planets[i].parameters.NearSolarPoint = data[i]["parameters"]["NearSolarPoint"].ToString();
            _planets[i].parameters.FarSolarPoint = data[i]["parameters"]["FarSolarPoint"].ToString();
            _planets[i].parameters.RevolutionPeriod = data[i]["parameters"]["RevolutionPeriod"].ToString();
            _planets[i].parameters.TrackBiasAngle = data[i]["parameters"]["TrackBiasAngle"].ToString();
            _planets[i].parameters.Introductions = data[i]["parameters"]["Introductions"].ToString();
        }
        isDataReady = true;

        return _planets;
    }

    //save data to json file
    public void SaveDataToJson(PlanetsValues[] planets,string path)
    {
        string serilizedString = JsonMapper.ToJson(planets);
        print(serilizedString);
        serilizedString.Trim();
        File.WriteAllText(path, serilizedString, Encoding.UTF8);
    }
}
