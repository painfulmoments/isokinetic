using System;
using System.IO;
using UnityEngine;
public class ReadConfig : MonoBehaviour
{
    #region singleton
    public static ReadConfig instance;
    private  ReadConfig getInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        return instance; ;
    }
    #endregion
    #region Global variable
    //数据库
    //public string db_connection_type;
    public  string db_port;
    public  string db_databaseName;
    public  string db_username;
    public  string db_password;
    public  string db_host;
    public  float freqTime;
    public int equipType;
    public double initvalue1;
    public double initvalue2;
    public double initvalue3;
    public float xLeft;
    public float xRight;
    public float yUp;
    public float yDown;
    public float zLeft;
    public float zRight;
    public double tranRatio1;
    public double tranRatio2;
    public double tranRatio3;
    public int precision;
    //udp 
    public string unityIP;
    public int unityPort;
    public string QTIP;
    public int QTPort;
    //Rom
    //public float wristArea;
    public float gameTime;
    public float gameStart;
    public float evaTime;
    public float speechTime;
    public float restTime;
    public string L_an;
    public string L_wo;
    public string R_an;
    public string R_wo;
    public float holdTime;
    public float evaTime1;
    public float figNum;
    public float resTime;
    public float gameTime2;
    public float TriggerThresholdX;
    #endregion
    #region 全局静态变量
    //Configuration file path
    public static string configPath = "config/config.ini";
    #endregion
    private void Awake()
    {
        getInstance();
        ReadIni();
        DontDestroyOnLoad(gameObject);
    }
    private void ReadIni()
    {
        INIParser ini = new INIParser();
        ini.Open(configPath);
        db_port = ini.ReadValue("dataBase", "db_port", "");
        db_databaseName = ini.ReadValue("dataBase", "db_databaseName", "");
        db_username = ini.ReadValue("dataBase", "db_username", "");
        db_password = ini.ReadValue("dataBase", "db_password", "");
        db_host = ini.ReadValue("dataBase", "db_host", "");
        freqTime = (float)ini.ReadValue("unity", "freqTime", 0.1f);
        equipType = ini.ReadValue("unity", "equipType", 1);
        initvalue1 =ini.ReadValue("unity", "initvalue1", 0.0);
        initvalue2 = ini.ReadValue("unity", "initvalue2", 0.0);
        initvalue3 = ini.ReadValue("unity", "initvalue3", 0.0);
        xLeft= (float)ini.ReadValue("unity", "xLeft",-29f);
        xRight= (float)ini.ReadValue("unity", "xRight", 26f);
        yUp = (float)ini.ReadValue("unity", "yUp", -7f);
        yDown = (float)ini.ReadValue("unity", "yDown", 19f);
        zLeft = (float)ini.ReadValue("unity", "zLeft", -20f);
        zRight = (float)ini.ReadValue("unity", "zRight", 12f);
        tranRatio1 = ini.ReadValue("unity", "tranRatio1",0.1563);
        tranRatio2 = ini.ReadValue("unity", "tranRatio2", 0.1563);
        tranRatio3 = ini.ReadValue("unity", "tranRatio3", 0.1563);
        precision = ini.ReadValue("unity", "precision", 1024);
        //udp
        unityIP = ini.ReadValue("udp", "unityIP", "");
        unityPort = ini.ReadValue("udp", "unityPort", 0);
        QTIP = ini.ReadValue("udp", "QTIP", "");
        QTPort = ini.ReadValue("udp", "QTPort", 0);
        //ROM
        //wristArea = ini.ReadValue("unity", "wristArea", 0);
        gameTime = ini.ReadValue("unity", "gameTime", 30);
        gameStart = ini.ReadValue("unity", "gameStart", 15);
        evaTime = ini.ReadValue("unity", "evaTime", 15);
        speechTime = ini.ReadValue("unity", "speechTime", 10);
        restTime = ini.ReadValue("unity", "restTime", 60);
        L_an = ini.ReadValue("unity", "L_an", "");
        L_wo = ini.ReadValue("unity", "L_wo", "");
        R_an = ini.ReadValue("unity", "R_an", "");
        R_wo = ini.ReadValue("unity", "R_wo", "");
        holdTime= ini.ReadValue("unity", "holdTime", 10);
        evaTime1= ini.ReadValue("unity", "evaTime1", 300);
        figNum= ini.ReadValue("unity", "figNum", 4);
		resTime = ini.ReadValue("unity", "resTime", 5);
        gameTime2 = ini.ReadValue("unity", "gameTime2", 1);
        TriggerThresholdX = ini.ReadValue("unity", "TriggerThresholdX", 15);
        ini.Close();
    }

}