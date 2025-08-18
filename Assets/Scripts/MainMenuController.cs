// MainMenuController.cs
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using ZTools;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;

    [Header("BGM Clips")]
    [Tooltip("Start 场景的主页面背景音乐")]
    [SerializeField] private AudioClip menuBgm;
    [Tooltip("SampleScene 的游戏背景音乐")]
    [SerializeField] private AudioClip gameBgm;

    private AudioSource bgmSource;

    void Awake()
    {
        #region Game NewContent1: ZLog Initialize
        ZLog.Log("MainMenuController Awake");
        #endregion

        SetupBgmPlayer();
    }

    private void SetupBgmPlayer()
    {
        // 找到或创建一个全局的 BGM 播放对象
        GameObject player = GameObject.Find("BgmPlayer");
        if (player == null)
        {
            player = new GameObject("BgmPlayer");
            DontDestroyOnLoad(player);
            bgmSource = player.AddComponent<AudioSource>();
            bgmSource.loop = true;
            ZLog.Log("BgmPlayer: Created new AudioSource");
        }
        else
        {
            bgmSource = player.GetComponent<AudioSource>();
            if (bgmSource == null)
            {
                bgmSource = player.AddComponent<AudioSource>();
                bgmSource.loop = true;
                ZLog.Log("BgmPlayer: Added AudioSource to existing object");
            }
        }

        // 根据当前场景播放对应的 BGM
        string scene = SceneManager.GetActiveScene().name;
        if (scene == "Start")
        {
            if (menuBgm != null)
            {
                bgmSource.clip = menuBgm;
                bgmSource.Play();
                ZLog.Log("BgmPlayer: Playing menu BGM");
            }
        }
        else if (scene == "SampleScene")
        {
            if (gameBgm != null)
            {
                bgmSource.clip = gameBgm;
                bgmSource.Play();
                ZLog.Log("BgmPlayer: Playing game BGM");
            }
        }
    }

    public void OnStartClicked()
    {
        #region Game NewContent2: Start Game via GameManager
        ZLog.Log("MainMenuController: OnStartClicked");

        // 切换到游戏 BGM
        if (bgmSource != null && gameBgm != null)
        {
            bgmSource.clip = gameBgm;
            bgmSource.Play();
            ZLog.Log("BgmPlayer: Switched to game BGM");
        }

        GameManager.o.GameStart();

        SceneManager.LoadScene("SampleScene");
        #endregion
    }

    public void OnSettingsClicked()
    {
        #region Game NewContent3: Open Settings Panel
        ZLog.Log("MainMenuController: OnSettingsClicked");
        settingsPanel.SetActive(true);
        #endregion
    }

    public void OnQuitClicked()
    {
        #region Game NewContent4: Exit Application
        ZLog.Log("MainMenuController: OnQuitClicked");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        GameManager.o.Exit();
#endif
        #endregion
    }
}
