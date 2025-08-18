// GroundFallController.cs
using System.Collections;
using UnityEngine;
using ZTools;

public class GroundFallController : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        #region Game NewContent36: Init Fall Controller
        rb = GetComponent<Rigidbody>();
        ZLog.Log("GroundFallController Start");
        #endregion
    }

    public IEnumerator SetRigidBodyValues()
    {
        #region Game NewContent37: Delay and Enable Gravity
        ZLog.Log("GroundFallController: Preparing to fall");
        yield return new WaitForSeconds(0.75f);
        rb.useGravity = true;
        rb.isKinematic = false;
        ZLog.Log("GroundFallController: Gravity enabled");
        #endregion
    }
}
