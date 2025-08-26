using UnityEngine;
using TMPro;
using ZTools;

public class SlowMotionUI : MonoBehaviour
{
    public static SlowMotionUI Instance { get; private set; }
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI instructionText; // 操作提示
    private float hideTime;
    private bool clicked;
    private BallMovementController ballMovementController;

    private void Awake()
    {
        #region Game NewContent43: Init SlowMotionUI
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (panel != null)
            panel.SetActive(false);
        clicked = false;

        // 初始隐藏提示文字
        if (instructionText != null)
        {
            instructionText.gameObject.SetActive(false);
        }

        ZLog.Log("SlowMotionUI Awake");
        #endregion
    }

    private void Start()
    {
        // 设置提示文字内容
        if (instructionText != null)
        {
            instructionText.text = "请完成下半程动作进行转向！";
        }
    }

    private void Update()
    {
        if (panel != null && panel.activeSelf)
        {
            // 检查是否超时自动隐藏
            if (Time.unscaledTime >= hideTime)
            {
                #region Game NewContent44: Auto Hide UI
                Hide();
                ZLog.Log("SlowMotionUI: Auto-hide panel after timeout");
                #endregion
            }
        }
    }

    public void Show(float duration)
    {
        #region Game NewContent46: Show SlowMotion Panel
        clicked = false;
        hideTime = Time.unscaledTime + duration;

        if (panel != null)
            panel.SetActive(true);

        // 显示提示文字
        if (instructionText != null)
        {
            instructionText.gameObject.SetActive(true);
        }

        // 找到移动控制器并通知进入慢动作状态
        if (ballMovementController == null)
        {
            ballMovementController = FindObjectOfType<BallMovementController>();
        }
        ballMovementController?.OnSlowMotionTriggered();

        // 可选：添加慢动作效果
        Time.timeScale = 0.3f; // 慢动作效果

        ZLog.Log($"SlowMotionUI: Show panel for {duration} seconds");
        #endregion
    }

    public void Hide()
    {
        #region Game NewContent47: Hide SlowMotion Panel
        if (panel != null)
            panel.SetActive(false);

        // 隐藏提示文字
        if (instructionText != null)
        {
            instructionText.gameObject.SetActive(false);
        }

        // 恢复正常时间速度
        Time.timeScale = 1f;

        ZLog.Log("SlowMotionUI: Hide panel");
        #endregion
    }

    public bool WasClicked
    {
        get
        {
            #region Game NewContent48: Check Clicked State
            return clicked;
            #endregion
        }
    }

    public bool IsActive
    {
        get
        {
            return panel != null && panel.activeSelf;
        }
    }

    // 由 BallMovementController 调用，在转向成功后隐藏UI
    public void OnTurnCompleted()
    {
        clicked = true;
        Hide();
        ZLog.Log("SlowMotionUI: Turn completed, hiding UI");
    }

    private void OnDestroy()
    {
        // 确保销毁时恢复时间速度
        Time.timeScale = 1f;
    }
}