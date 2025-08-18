using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZTools;


[Serializable]
public class Result
{
    public int molel;
    public string x11, x12, y11, y12, z11, z12;
    public string x21, x22, y21, y22, z21, z22;
    // jlzdjg/jlbdjg 保留或删掉，看你是否还用
}

[Serializable]
public class RotateDate { public List<float> date = new List<float>(); }

[Serializable]
public class ActionDate { public RotateDate[] action = new RotateDate[3] { new RotateDate(), new RotateDate(), new RotateDate() }; }

[Serializable]
public class HandDate { public ActionDate[] hand = new ActionDate[2] { new ActionDate(), new ActionDate() }; }

[Serializable]
public class TestDate { public HandDate activeDate = new HandDate(), passiveDate = new HandDate(); public int mode; }

public class GameManager : MonoBehaviour
{
    public static GameManager o;
    public TestDate date;
    public int gameState;
    public int gameMode = 3;
    public string gameId, patientId;
    public Result rs;
    // 以下如果你没有 UdpClientHandler/SqlAccess/GameSettings 定义，也可以先注释掉
    // private UdpClientHandler udpClient;
    // private SqlAccess sql;
    // public GameSettings settings;
    // public int playTimes, wristArea;

    void Awake()
    {
        o = this;
    }

    private void Start()
    {
        // 如果你有 udpConnect 方法，也可以在这里调用
        // udpConnect();

        GameReady();
        ZLog.Log("GameManager: Start()");
    }

    public void GameReady()
    {
        gameState = 0;
        // 原有：EndDialog.o.Close();
        // 原有：GameDialog.o.Close();
        // 原有：HomeDialog.o.GameReady();
        ZLog.Log("GameManager: GameReady()");
    }

    public void GameStart()
    {
        Time.timeScale = 1f;
        // 原有：HomeDialog.o.Close();
        ZLog.Log("GameManager: GameStart()");
    }

    public void GamePause()
    {
        gameState = 2;
        Time.timeScale = 0f;
        ZLog.Log("GameManager: GamePause()");
        // 原有：GameDialog.o.PauseShow();
    }

    public void GameContinue()
    {
        gameState = 1;
        Time.timeScale = 1f;
        ZLog.Log("GameManager: GameContinue()");
        // 原有：GameDialog.o.PauseClose();
    }

    public void GameEnd()
    {
        gameState = 3;
        ZLog.Log("GameManager: GameEnd()");
        // 原有：GameDialog.o.Close();
        // 原有：rs = EndDialog.o.End();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SaveDate()
    {
        ZLog.Log("GameManager: SaveDate() called");
        // 如果没有 MySQL 方法，也可以先只 Debug.Log
        Debug.Log("Result.molel = " + rs.molel);
        Exit();
    }

    // 如果你有 CreateMesh、udpConnect、saveMySQL 等方法，先把它们注释掉
    // public Mesh CreateMesh(...) { ... }
    // public void udpConnect() { ... }
    // private void saveMySQL(...) { ... }

    private void OnDestroy()
    {
        ZLog.Log("GameManager: OnDestroy()");
    }
}
