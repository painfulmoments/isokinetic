// GroundCollisionController.cs
using UnityEngine;
using ZTools;

public class GroundCollisionController : MonoBehaviour
{
    [SerializeField] private GroundDataTransmitter groundDataTransmitter;

    private void Awake()
    {
        #region Game NewContent29: Init GroundCollision
        ZLog.Log("GroundCollisionController Awake");
        #endregion
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;

        #region Game NewContent30: Ball Exited Ground
        ZLog.Log("GroundCollisionController: Ball exited, triggering fall");
        groundDataTransmitter.SetGroundRigidBody();
        #endregion
    }
}
