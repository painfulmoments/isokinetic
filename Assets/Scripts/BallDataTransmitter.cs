// BallDataTransmitter.cs
using UnityEngine;
using ZTools;

public class BallDataTransmitter : MonoBehaviour
{
    [SerializeField] private BallInputController ballInputController;

    private void Awake()
    {
        #region Game NewContent25: Init DataTransmitter
        ZLog.Log("BallDataTransmitter Awake");
        #endregion
    }

    public Vector3 GetBallDirection()
    {
        #region Game NewContent26: Get Ball Direction
        Vector3 dir = ballInputController.ballDirection;
        ZLog.Log($"BallDataTransmitter: GetBallDirection -> {dir}");
        return dir;
        #endregion
    }
}
