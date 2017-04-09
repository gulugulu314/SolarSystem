using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlanetsValues
{
    public string name;
    public ParameterValueItems parameters;
}

[Serializable]
public class ParameterValueItems
{
    public string RotatePeriod;
    public string BiasAngle;
    public string RotateDirection;

    public string NearSolarPoint;
    public string FarSolarPoint;
    public string RevolutionPeriod;
    public string TrackBiasAngle;

    public string Introductions;
}
