using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZTools;
using System.Collections;

public class BallMovementController : MonoBehaviour
{
    [Header("Movement & Speed")]
    [SerializeField] private BallDataTransmitter ballDataTransmitter;
    [SerializeField] private float ballMoveSpeed = 5f;
    [SerializeField] private float checkBallSpeedPoint = 10f;

    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Score Settings")]
    [SerializeField] private float scoreMultiplier = 10f;

    [Header("Character Model")]
    [SerializeField] private Transform characterModel;
    [SerializeField] private Animator characterAnimator; // 动画控制器

    [Header("Win UI")]
    [SerializeField] private GameObject winPanel;

    [Header("Win Settings")]
    [SerializeField] private float winScore = 1000f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource bgmAudio;

    private float scoreValue;
    private Vector3 previousPosition;
    private Vector3 currentDirection;
    private bool isMoving = false;
    private bool isInSlowMotion = false;
    private bool waitingForTurn = false;
    private bool hasTurned = false;

    // 自动重启相关
    private Coroutine autoRestartCoroutine;
    [Tooltip("面板出现后等待多少秒（真实时间）自动重新开始")]
    [SerializeField] private float autoRestartDelay = 3f;

    private void Start()
    {
        #region Game NewContent19: Init Movement
        ZLog.Log("BallMovementController Start");
        previousPosition = transform.position;

        // 获取初始方向，如果获取失败则使用默认前方
        if (ballDataTransmitter != null)
        {
            currentDirection = ballDataTransmitter.GetBallDirection();
        }

        // 如果方向为零或无效，使用默认方向
        if (currentDirection.magnitude < 0.1f)
        {
            currentDirection = Vector3.forward; // 默认向前 (0, 0, 1)
            ZLog.Log("BallMovementController: Using default forward direction");
        }

        currentDirection = currentDirection.normalized; // 确保方向向量归一化
        ZLog.Log($"BallMovementController: Initial direction = {currentDirection}");

        if (characterModel != null)
            characterModel.rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
        if (winPanel != null) winPanel.SetActive(false);
        if (bgmAudio != null && !bgmAudio.isPlaying)
            bgmAudio.Play();
        #endregion
    }

    private void Update()
    {
        HandleInput();
        HandleMovement();
        UpdateScore();
        UpdateAnimation();
    }

    private void HandleInput()
    {
        // 检查是否在慢动作状态
        isInSlowMotion = SlowMotionUI.Instance != null && SlowMotionUI.Instance.IsActive;

        // 处理左键点击（转向）
        if (Input.GetMouseButtonDown(0)) // 左键点击
        {
            if (isInSlowMotion && waitingForTurn)
            {
                // 慢动作状态下，左键触发转向
                PerformTurn();
                waitingForTurn = false;
                hasTurned = true;

                // 转向成功后隐藏慢动作UI
                if (SlowMotionUI.Instance != null)
                {
                    SlowMotionUI.Instance.OnTurnCompleted();
                }

                ZLog.Log("BallMovementController: Turn performed via left click in slow motion");
            }
            else if (!isInSlowMotion)
            {
                // 正常状态下，左键点击会停止移动（和松开右键效果一样）
                isMoving = false;
                ZLog.Log("BallMovementController: Left click in normal state - stopping");
            }
        }

        // 处理右键按住（前进）
        if (isInSlowMotion && waitingForTurn && !hasTurned)
        {
            // 在慢动作等待转向时，即使按住右键也不能移动
            isMoving = false;
            if (Input.GetMouseButton(1) && Time.frameCount % 60 == 0)
            {
                ZLog.Log("BallMovementController: Right button held but waiting for turn in slow motion");
            }
        }
        else if (Input.GetMouseButton(1)) // 右键按住
        {
            // 正常状态或已经转向后，按住右键前进
            isMoving = true;
        }
        else
        {
            // 没按右键，停止移动
            isMoving = false;
        }

        // 慢动作结束后重置状态
        if (!isInSlowMotion && (waitingForTurn || hasTurned))
        {
            waitingForTurn = false;
            hasTurned = false;
            ZLog.Log("BallMovementController: Slow motion ended, reset turn states");
        }
    }

    private void HandleMovement()
    {
        if (!isMoving) return;

        // 确保方向有效
        if (currentDirection.magnitude < 0.1f)
        {
            ZLog.Log("BallMovementController: Warning - Invalid direction, resetting to forward");
            currentDirection = Vector3.forward;
        }

        // 移动角色
        Vector3 movement = currentDirection.normalized * ballMoveSpeed * Time.deltaTime;
        transform.position += movement;

        // 调试输出
        if (Time.frameCount % 30 == 0) // 每30帧输出一次，避免刷屏
        {
            ZLog.Log($"Moving: pos={transform.position}, dir={currentDirection}, speed={ballMoveSpeed}, deltaTime={Time.deltaTime}");
        }

        // 检查速度增加点
        if (transform.position.x < -checkBallSpeedPoint || transform.position.z > checkBallSpeedPoint)
        {
            ballMoveSpeed += 0.3f;
            checkBallSpeedPoint += 10f;
            ZLog.Log($"Speed increased to {ballMoveSpeed}");
        }
    }

    private void UpdateAnimation()
    {
        if (characterAnimator != null)
        {
            // 设置动画参数 - 立即响应
            characterAnimator.SetBool("IsRunning", isMoving);
            characterAnimator.SetFloat("Speed", isMoving ? 1f : 0f);

            // 强制立即更新动画状态（可选）
            if (isMoving && !characterAnimator.GetBool("IsRunning"))
            {
                characterAnimator.Play("Running", 0, 0f); // 直接播放奔跑动画
            }
            else if (!isMoving && characterAnimator.GetBool("IsRunning"))
            {
                characterAnimator.Play("Idle", 0, 0f); // 直接播放待机动画
            }
        }
    }

    // 由 SlowMotionUI 或转角检测调用
    public void OnSlowMotionTriggered()
    {
        isInSlowMotion = true;
        waitingForTurn = true;
        hasTurned = false;
        isMoving = false; // 立即停止移动
        ZLog.Log("BallMovementController: Slow motion triggered, waiting for left click to turn");
    }

    private void PerformTurn()
    {
        // 获取新的方向
        Vector3 newDirection = Vector3.zero;

        if (ballDataTransmitter != null)
        {
            newDirection = ballDataTransmitter.GetBallDirection();
            ZLog.Log($"BallMovementController: Got direction from transmitter: {newDirection}");
        }

        // 如果没有获取到有效方向，或者方向相同，尝试简单的90度转向
        if (newDirection.magnitude < 0.1f || Vector3.Angle(currentDirection, newDirection) < 5f)
        {
            // 根据当前方向决定转向（修正方向）
            // 如果正在向前(z+)，则转向左(x-)
            // 如果正在向后(z-)，则转向右(x+)
            // 如果正在向左(x-)，则转向后(z-)
            // 如果正在向右(x+)，则转向前(z+)
            if (Mathf.Abs(currentDirection.z) > Mathf.Abs(currentDirection.x))
            {
                // 当前主要沿Z轴移动，转向X轴（方向修正）
                newDirection = currentDirection.z > 0 ? Vector3.left : Vector3.right;
            }
            else
            {
                // 当前主要沿X轴移动，转向Z轴（方向修正）
                newDirection = currentDirection.x > 0 ? Vector3.back : Vector3.forward;
            }
            ZLog.Log($"BallMovementController: Using default turn from {currentDirection} to {newDirection}");
        }

        // 执行转向
        currentDirection = newDirection.normalized;
        if (characterModel != null)
        {
            // 平滑转向动画
            StartCoroutine(SmoothTurn(Quaternion.LookRotation(currentDirection, Vector3.up)));
        }
        ZLog.Log($"BallMovementController: Turn completed! New direction: {currentDirection}");
    }

    private IEnumerator SmoothTurn(Quaternion targetRotation)
    {
        float turnTime = 0.3f; // 转向动画时长
        float elapsed = 0f;
        Quaternion startRotation = characterModel.rotation;

        while (elapsed < turnTime)
        {
            elapsed += Time.deltaTime;
            characterModel.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / turnTime);
            yield return null;
        }

        characterModel.rotation = targetRotation;
    }

    private void UpdateScore()
    {
        if (!isMoving) return; // 只有移动时才计分

        float dist = Vector3.Distance(transform.position, previousPosition);
        scoreValue += dist * scoreMultiplier;
        if (scoreText != null)
            scoreText.text = Mathf.FloorToInt(scoreValue).ToString();
        previousPosition = transform.position;

        if (scoreValue >= winScore)
        {
            OnWin();
        }
    }

    private void OnWin()
    {
        ZLog.Log("BallMovementController: OnWin");
        if (winPanel != null) winPanel.SetActive(true);
        PauseBGM();
        Time.timeScale = 0f; // 暂停游戏时间（保持原有行为）
        GameManager.o.GameEnd();

        // 启动自动重启倒计时（使用真实时间）
        StartAutoRestartCountdown();
    }

    private void PauseBGM()
    {
        if (bgmAudio != null)
        {
            bgmAudio.Pause();
        }
    }

    // 启动 3 秒自动重启协程（可被取消）
    private void StartAutoRestartCountdown()
    {
        // 先取消已有的
        CancelAutoRestartCountdown();

        // 使用真实时间等待（WaitForSecondsRealtime），不受 Time.timeScale 影响
        autoRestartCoroutine = StartCoroutine(AutoRestartCoroutine(autoRestartDelay));
        ZLog.Log($"BallMovementController: Start auto-restart countdown ({autoRestartDelay}s)");
    }

    // 取消自动重启倒计时（在按钮被按下时调用）
    public void CancelAutoRestartCountdown()
    {
        if (autoRestartCoroutine != null)
        {
            StopCoroutine(autoRestartCoroutine);
            autoRestartCoroutine = null;
            ZLog.Log("BallMovementController: Auto-restart countdown cancelled");
        }
    }

    private IEnumerator AutoRestartCoroutine(float delay)
    {
        // 等待真实时间（不受 Time.timeScale 影响）
        yield return new WaitForSecondsRealtime(delay);

        // 如果协程没被取消，自动重启
        ZLog.Log("BallMovementController: Auto-restart timeout reached, restarting scene...");
        autoRestartCoroutine = null;
        RestartGame();
    }

    // 这个方法供 UI 面板上所有按钮的 OnClick 首个回调使用
    public void OnPanelButtonPressed()
    {
        // 玩家按了面板上的任意按钮 -> 取消自动重启
        CancelAutoRestartCountdown();
    }

    public void RestartGame()
    {
        #region Game NewContent21: Restart Game
        ZLog.Log("BallMovementController: RestartGame");
        Time.timeScale = 1f; // 恢复时间尺度，避免在胜利时卡住
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        #endregion
    }

    public void ReturnToMainMenu()
    {
        #region Game NewContent22: Return to Main Menu
        ZLog.Log("BallMovementController: ReturnToMainMenu");
        Time.timeScale = 1f; // 恢复时间尺度
        GameManager.o.GameReady();
        SceneManager.LoadScene("Start");
        #endregion
    }
}