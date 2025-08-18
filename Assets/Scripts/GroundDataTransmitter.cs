// GroundDataTransmitter.cs
using UnityEngine;
using ZTools;

public class GroundDataTransmitter : MonoBehaviour
{
    [SerializeField] private GroundFallController groundFallController;

    private void Awake()
    {
        #region Game NewContent34: Init GroundDataTransmitter
        ZLog.Log("GroundDataTransmitter Awake");
        #endregion
    }

    public void SetGroundRigidBody()
    {
        #region Game NewContent35: Transmit to Fall Controller
        ZLog.Log("GroundDataTransmitter: SetGroundRigidBody");
        StartCoroutine(groundFallController.SetRigidBodyValues());
        #endregion
    }
}
