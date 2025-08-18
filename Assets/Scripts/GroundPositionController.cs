// GroundPositionController.cs
using UnityEngine;

public class GroundPositionController : MonoBehaviour
{
    [Header("回收高度阈值")]
    [SerializeField] private float endYValue = -5f;

    private GroundSpawnController spawner;
    private Rigidbody rb;

    private void Start()
    {
        spawner = FindObjectOfType<GroundSpawnController>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (transform.position.y <= endYValue)
            HandleRecycle();
    }

    private void HandleRecycle()
    {
        // 通知生成器移除、可能补充
        spawner.RemoveGround(gameObject);
        // 销毁自身：连带其上的 Trigger 一并销毁
        Destroy(gameObject);
    }
}
