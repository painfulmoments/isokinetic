// BallInputController.cs
using UnityEngine;
using ZTools;

public class BallInputController : MonoBehaviour
{
    [HideInInspector] public Vector3 ballDirection;

    private void Awake()
    {
        #region Game NewContent16: Init Ball Direction
        ZLog.Log("BallInputController Awake – init dir to forward");
        #endregion
    }

    void Start()
    {
        #region Game NewContent17: Default Direction
        ballDirection = Vector3.forward;
        ZLog.Log("BallInputController Start – ballDirection = forward");
        #endregion
    }

    /// <summary>
    /// 由 UI 按钮调用，切换方向：左 ↔ 前
    /// </summary>
    public void PerformTurn()
    {
        #region Game NewContent18: Perform Turn
        ballDirection = (ballDirection.x == -1) ? Vector3.forward : Vector3.left;
        ZLog.Log($"BallInputController: PerformTurn -> {ballDirection}");
        #endregion
    }
}
