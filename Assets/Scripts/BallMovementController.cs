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

    [Header("Death UI")]
    [SerializeField] private GameObject deathPanel;

    [Header("Win UI")]
    [SerializeField] private GameObject winPanel;

    [Header("Win Settings")]
    [SerializeField] private float winScore = 1000f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource bgmAudio;

    private float scoreValue;
    private Vector3 previousPosition;
    private Vector3 lastDirection;
    private bool isDead;

    // 自动重启相关
    private Coroutine autoRestartCoroutine;
    [Tooltip("面板出现后等待多少秒（真实时间）自动重新开始")]
    [SerializeField] private float autoRestartDelay = 3f;

    private void Start()
    {
        #region Game NewContent19: Init Movement
        ZLog.Log("BallMovementController Start");
        previousPosition = transform.position;
        lastDirection = ballDataTransmitter.GetBallDirection();
        if (characterModel != null)
            characterModel.rotation = Quaternion.LookRotation(lastDirection, Vector3.up);
        if (deathPanel != null) deathPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (bgmAudio != null && !bgmAudio.isPlaying)
            bgmAudio.Play();
        isDead = false;
        #endregion
    }

    private void Update()
    {
        if (isDead) return;
        HandleRotation();
        MoveCharacter();
        UpdateScore();
    }

    private void HandleRotation()
    {
        Vector3 dir = ballDataTransmitter.GetBallDirection();
        if (dir != lastDirection && dir.sqrMagnitude > 0f)
        {
            if (characterModel != null)
                characterModel.rotation = Quaternion.LookRotation(dir, Vector3.up);
            lastDirection = dir;
        }
    }

    private void MoveCharacter()
    {
        Vector3 movement = lastDirection * ballMoveSpeed * Time.deltaTime;
        transform.position += movement;

        if (transform.position.y < -6f)
        {
            OnDeath();
            return;
        }

        if (transform.position.x < -checkBallSpeedPoint || transform.position.z > checkBallSpeedPoint)
        {
            ballMoveSpeed += 0.3f;
            checkBallSpeedPoint += 10f;
        }
    }

    private void UpdateScore()
    {
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

    private void OnDeath()
    {
        #region Game NewContent20: Player Death
        ZLog.Log("BallMovementController: OnDeath");
        isDead = true;
        if (deathPanel != null) deathPanel.SetActive(true);
        PauseBGM();
        GameManager.o.GameEnd();

        // 启动自动重启倒计时（真实时间）
        StartAutoRestartCountdown();
        #endregion
    }

    private void OnWin()
    {
        ZLog.Log("BallMovementController: OnWin");
        isDead = true;
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

    // 这个方法供 UI 面板上所有按钮的 OnClick 首个回调使用：
    // 在面板按钮的 OnClick 列表里，**把 BallMovementController.OnPanelButtonPressed 放在第一项**，
    // 然后再把按钮要执行的动作（RestartGame / ReturnToMainMenu / 其他）放在后面。
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
