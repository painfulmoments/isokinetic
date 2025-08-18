// CameraFollowController.cs
using UnityEngine;
using ZTools;

public class CameraFollowController : MonoBehaviour
{
    [SerializeField] private Transform ballTransform;
    [Range(0, 3)][SerializeField] private float lerpValue;
    private Vector3 offset;
    private Vector3 newPosition;

    private void Start()
    {
        #region Game NewContent27: Init Camera Follow
        offset = transform.position - ballTransform.position;
        ZLog.Log("CameraFollowController Start");
        #endregion
    }

    private void LateUpdate()
    {
        #region Game NewContent28: Smooth Follow
        newPosition = Vector3.Lerp(transform.position, ballTransform.position + offset, lerpValue * Time.deltaTime);
        transform.position = newPosition;
        #endregion
    }
}
