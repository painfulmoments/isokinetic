// SlowMotionUI.cs
using UnityEngine;
using UnityEngine.UI;
using ZTools;

public class SlowMotionUI : MonoBehaviour
{
    public static SlowMotionUI Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private GameObject panel;
    [Tooltip("（保留以便兼容）之前的Button，现在不用也可以保留）")]
    [SerializeField] private Button clickButton;

    [Header("Encoder Trigger Settings")]
    [Tooltip("当 x_rotation 从小于此阈值上穿到 >= 此阈值时触发转向")]
    [SerializeField] private float triggerThreshold = 23f;
    [Tooltip("如果 true，要求从 < threshold 上穿到 >= threshold 时触发；如果 false，只要 x >= threshold 就触发")]
    [SerializeField] private bool requireRisingEdge = true;

    private float hideTime;
    private bool clicked;

    // 用于检测上穿（rising edge）
    private float lastSamplerX = float.NaN;

    // 防止在找不到 EncoderManager 时不断刷警告
    private bool encoderMissingWarned = false;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (panel != null) panel.SetActive(false);
        clicked = false;

        // 如果你还保留了 Button，在 Inspector 里可以不挂任何事件；我们改为由 Encoder 驱动
        if (clickButton != null)
        {
            clickButton.onClick.RemoveAllListeners();
            // 如果你想保留按钮手动触发，也可以在 Inspector 连回 OnClick 到 OnClickButton
            // clickButton.onClick.AddListener(OnClickButton);
        }

        ZLog.Log("SlowMotionUI Awake");
    }

    private void Update()
    {
        // 如果面板处于显示状态且过了 Hide 时间，自动隐藏面板
        if (panel != null && panel.activeSelf && Time.unscaledTime >= hideTime)
        {
            panel.SetActive(false);
            ZLog.Log("SlowMotionUI: Auto-hide panel after timeout");
        }

        // 如果面板正在显示，检查 Encoder 的 X 数据是否触发
        if (panel != null && panel.activeSelf && !clicked)
        {
            var enc = EncoderManager.instance;
            if (enc == null)
            {
                if (!encoderMissingWarned)
                {
                    // 用 Debug.LogWarning 报一次警告（ZLog 可能没有 LogWarning）
                    Debug.LogWarning("SlowMotionUI: EncoderManager.instance is null - cannot sample x_rotation");
                    encoderMissingWarned = true;
                }
            }
            else
            {
                float currentX = enc.x_rotation;

                // 初始化 lastSamplerX（第一次采样）
                if (float.IsNaN(lastSamplerX))
                    lastSamplerX = currentX;

                bool triggered = false;
                if (requireRisingEdge)
                {
                    // 只有在上穿（从 < threshold 到 >= threshold）时触发
                    if (lastSamplerX < triggerThreshold && currentX >= triggerThreshold)
                        triggered = true;
                }
                else
                {
                    // 只要 >= threshold 就触发
                    if (currentX >= triggerThreshold)
                        triggered = true;
                }

                if (triggered)
                {
                    clicked = true;

                    var inputCtrl = FindObjectOfType<BallInputController>();
                    inputCtrl?.PerformTurn();

                    ZLog.Log($"SlowMotionUI: Encoder trigger fired (lastX={lastSamplerX:F2}, curX={currentX:F2}), PerformTurn called");
                }

                // 更新采样历史（为了下一帧判断上穿）
                lastSamplerX = currentX;
            }
        }
    }

    /// <summary>
    /// 若你仍想保留按钮回调（调试用），可保留此方法，但现在不再自动绑定。
    /// </summary>
    private void OnClickButton()
    {
        clicked = true;
        var inputCtrl = FindObjectOfType<BallInputController>();
        inputCtrl?.PerformTurn();
        ZLog.Log("SlowMotionUI: Click detected (button), PerformTurn called");
    }

    /// <summary>
    /// 显示慢动作面板，持续 duration 秒（真实时间）
    /// </summary>
    public void Show(float duration)
    {
        clicked = false;
        lastSamplerX = float.NaN; // 重置采样历史，避免遗留上穿被误触发
        hideTime = Time.unscaledTime + duration;
        if (panel != null) panel.SetActive(true);
        ZLog.Log($"SlowMotionUI: Show panel for {duration} seconds");
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    public void Hide()
    {
        if (panel != null) panel.SetActive(false);
        ZLog.Log("SlowMotionUI: Hide panel");
    }

    /// <summary>
    /// 协程中用于判断玩家是否已经“点击/触发”
    /// </summary>
    public bool WasClicked
    {
        get { return clicked; }
    }
}
