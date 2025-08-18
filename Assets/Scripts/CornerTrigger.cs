using UnityEngine;
using System.Collections;

public class CornerTrigger : MonoBehaviour
{
    [Header("Slow Motion Settings")]
    [Tooltip("慢动作时间缩放"), Range(0f, 1f)]
    public float slowTimeScale = 0.2f;
    [Tooltip("判定时长（秒，真实时间）")]
    public float decisionTime = 1.5f;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Ball")) return;

        triggered = true;
        StartCoroutine(SlowMotionDecision());
    }

    private IEnumerator SlowMotionDecision()
    {
        // 1. 进入慢动作
        Time.timeScale = slowTimeScale;
        Time.fixedDeltaTime = 0.02f * slowTimeScale;

        // 2. 弹出慢动作 UI
        SlowMotionUI.Instance.Show(decisionTime);

        // 3. 等待点击或超时
        float timer = 0f;
        while (timer < decisionTime)
        {
            if (SlowMotionUI.Instance.WasClicked)
                break;
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        // 4. 隐藏 UI，恢复正常时间
        SlowMotionUI.Instance.Hide();
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}