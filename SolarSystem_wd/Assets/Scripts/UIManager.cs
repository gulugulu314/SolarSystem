using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Windows.Forms;
using System;

public class UIManager : MonoBehaviour 
{
    public static UIManager ui_instance;

    public Dropdown dropdownlist;

    public InputField RotatePeriod;
    public InputField BiasAngle;
    public Dropdown RotationDirection;

    public InputField RevolutionPeriod;
    public InputField NearSolarPoint;
    public InputField FarSolarPoint;
    public InputField TrackBiasAngle;
    public Text Title;
    public Text Introduction;

    public GameObject SettingPanel;
    public GameObject IntroductionPanel;
    private Animator settingPanel_anim;
    private Animator IntroducitonPanel_anim;

    DataManager dataManager;
    public PlanetsValues[] m_Planets = new PlanetsValues[8];

    private string DefaultFilePath = null;
    private bool isOpenNewFile = false;

    private int currentdropdownindex;

    public bool isOpenUIFlag;

    void Awake()
    {
        ui_instance = this;
    }

	void Start () 
    {
        settingPanel_anim = SettingPanel.GetComponent<Animator>();
        IntroducitonPanel_anim = IntroductionPanel.GetComponent<Animator>();

        dataManager = DataManager.inst;
        InitUI();

        CameraController.instance.ClickPlanetEvent += ShowUI;
	}

    private void InitUI()
    {
        print("init the ui data from defaultPlanetData json file");
        for (int i = 0; i < m_Planets.Length; i++)
        {
            m_Planets[i] = new PlanetsValues();
            m_Planets[i].parameters = new ParameterValueItems();
        }

        DefaultFilePath = UnityEngine.Application.streamingAssetsPath + "/DefaultPlanetsData" + ".json";
        print("the default path of default json file is :"+DefaultFilePath);
        m_Planets = dataManager.ReadDataFromJosn(DefaultFilePath);

        SetUIData();
    }

    //set ui data and update ui when dropdownlist value changed
    public void SetUIData()
    {
        print("have initialed the ui!!!");
        RotatePeriod.text = m_Planets[dropdownlist.value].parameters.RotatePeriod;
        BiasAngle.text = m_Planets[dropdownlist.value].parameters.BiasAngle;
        if (m_Planets[dropdownlist.value].parameters.RotateDirection == "自西向东")
        {
            RotationDirection.value = 0;
        }
        else if (m_Planets[dropdownlist.value].parameters.RotateDirection == "自东向西")
        {
            RotationDirection.value = 1;
        }
        RevolutionPeriod.text = m_Planets[dropdownlist.value].parameters.RevolutionPeriod;
        NearSolarPoint.text = m_Planets[dropdownlist.value].parameters.NearSolarPoint;
        FarSolarPoint.text = m_Planets[dropdownlist.value].parameters.FarSolarPoint;
        TrackBiasAngle.text = m_Planets[dropdownlist.value].parameters.TrackBiasAngle;

        Title.text = m_Planets[dropdownlist.value].name + "'s" + "  Introduction:";
        Introduction.text = m_Planets[dropdownlist.value].parameters.Introductions;

        currentdropdownindex = dropdownlist.value;
    }

    //打开
    public void OpenJsonFile()
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.InitialDirectory = "file://" + UnityEngine.Application.streamingAssetsPath;

        if (ofd.ShowDialog() == DialogResult.OK)
        {
            Debug.Log(ofd.FileName);
            m_Planets = dataManager.ReadDataFromJosn(ofd.FileName);
            isOpenNewFile = true;
        }
    }

    //save the usrs ui data to jsonfile
    public void SaveJsonFile()
    {
        SaveFileDialog sfd = new SaveFileDialog();
        sfd.InitialDirectory = "file://" + UnityEngine.Application.streamingAssetsPath;
        sfd.Filter = "json files(*.json)|*.json";
        if (sfd.ShowDialog() == DialogResult.OK)
        {
            Debug.Log(sfd.FileName);
            dataManager.SaveDataToJson(m_Planets, sfd.FileName);
        }
    }

    //get the user data from ui
    public PlanetsValues[] GetUIData()
    {
        PlanetsValues[] tempPlanetValues = new PlanetsValues[8];
        for (int i = 0; i < tempPlanetValues.Length; i++)
        {
            tempPlanetValues[i] = new PlanetsValues();
            tempPlanetValues[i].parameters = new ParameterValueItems();
        }
        for (int i = 0; i < dropdownlist.options.Count; i++)
        {
            tempPlanetValues[i].name = dropdownlist.options[i].text;
            tempPlanetValues[i].parameters.RotatePeriod = RotatePeriod.text;
            tempPlanetValues[i].parameters.BiasAngle = BiasAngle.text;
            if (RotationDirection.value == 0)
            {
                tempPlanetValues[i].parameters.RotateDirection = "自西向东";
            }
            else
            {
                tempPlanetValues[i].parameters.RotateDirection = "自东向西";
            }
            tempPlanetValues[i].parameters.RevolutionPeriod = RevolutionPeriod.text;
            tempPlanetValues[i].parameters.NearSolarPoint = NearSolarPoint.text;
            tempPlanetValues[i].parameters.FarSolarPoint = FarSolarPoint.text;
            tempPlanetValues[i].parameters.TrackBiasAngle = TrackBiasAngle.text;
            tempPlanetValues[i].parameters.Introductions = Introduction.text;
        }
        return tempPlanetValues;
    }

    void Update()
    {
        UpDateArraydata();
    }

    void UpDateArraydata()
    {
        //如果打开新的文件
        if (isOpenNewFile)
        {
            SetUIData();
        }
        //如果用户输入了新的内容
        m_Planets[currentdropdownindex].parameters.RotatePeriod = RotatePeriod.text;
        m_Planets[currentdropdownindex].parameters.BiasAngle = BiasAngle.text;

        m_Planets[currentdropdownindex].parameters.RevolutionPeriod = RevolutionPeriod.text;
        m_Planets[currentdropdownindex].parameters.NearSolarPoint = NearSolarPoint.text;
        m_Planets[currentdropdownindex].parameters.FarSolarPoint = FarSolarPoint.text;
        m_Planets[currentdropdownindex].parameters.TrackBiasAngle = TrackBiasAngle.text;
    }

    public void ShowUI(string name)
    {
        Debug.Log("传来的值为：" + name);

        settingPanel_anim.Play("SettingPanelOpen");
        IntroducitonPanel_anim.Play("IntroductionPanelOpen");
        isOpenUIFlag = true;

        //点击那个星星，更新UI中的dropdown即可

        List<string> ItemNames = new List<string>();
        for (int i = 0; i < dropdownlist.options.Count; i++)
        {
            ItemNames.Add(dropdownlist.options[i].text);
            print(ItemNames.Count + ItemNames[i]);
        }

        if (ItemNames.Contains(name))
        {
            dropdownlist.value = ItemNames.IndexOf(name);
        }
    }

    public void CloseUI()
    {
        Debug.Log("关闭ui吧");
        settingPanel_anim.Play("SettingPaneClose");
        IntroducitonPanel_anim.Play("IntroductionPanelClose");
    }

    public void OpenUI()
    {
        Debug.Log("打开ui吧");
        settingPanel_anim.Play("SettingPanelOpen");
        IntroducitonPanel_anim.Play("IntroductionPanelOpen");
    }
}
