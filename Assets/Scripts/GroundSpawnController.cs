// GroundSpawnController.cs
using UnityEngine;
using System.Collections.Generic;
using ZTools;

[RequireComponent(typeof(MonoBehaviour))]
public class GroundSpawnController : MonoBehaviour
{
    [Header("Prefabs & References")]
    public GameObject lastGroundObject;
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject turnTriggerPrefab;

    [Header("Trigger Settings")]
    [Tooltip("触发器在地块上方的高度偏移（不再用于位置计算，可忽略）")]
    [SerializeField] private float triggerVerticalOffset = 0.5f;

    [Header("Turn Settings")]
    [Range(0f, 1f)] public float turnProbability = 0.2f;
    [SerializeField] private int minStraightSegments = 3;

    [Header("Pooling Settings")]
    [Tooltip("当剩余地块低于该值时，补充生成数量")]
    [SerializeField] private int refillThreshold = 90;
    [Tooltip("补充生成的块数")]
    [SerializeField] private int refillCount = 10;

    private int currentDirection = 1; // 0 = left, 1 = forward
    private int straightCount = 0;
    private Vector3 lastOffset = Vector3.forward;

    // 场上所有地块
    private readonly List<GameObject> activeGrounds = new List<GameObject>();

    private void Start()
    {
        // 初始生成100块
        GenerateRandomNewGrounds(100);
    }

    public void GenerateRandomNewGrounds(int count)
    {
        for (int i = 0; i < count; i++)
            CreateNewGround();
    }

    private void CreateNewGround()
    {
        // 1. 计算偏移
        Vector3 offset = GetNextOffset();

        // 2. 实例化地块
        GameObject origin = lastGroundObject;
        Vector3 spawnPos = origin.transform.position + offset;
        GameObject newGround = Instantiate(groundPrefab, spawnPos, Quaternion.identity);

        // 3. 如果是转角，就在 origin 上生成触发器（localPosition=0）
        if (offset != lastOffset && turnTriggerPrefab != null)
        {
            GameObject trig = Instantiate(turnTriggerPrefab, origin.transform);
            trig.transform.localPosition = Vector3.zero;
            trig.transform.localRotation = Quaternion.identity;
        }

        // 4. 维护列表、更新引用
        activeGrounds.Add(newGround);
        lastGroundObject = newGround;
        lastOffset = offset;

        ZLog.Log($"Spawned ground at {spawnPos}, corner={(offset != lastOffset)}");
    }

    public Vector3 GetNextOffset()
    {
        bool canTurn = straightCount >= minStraightSegments;
        if (canTurn && Random.value < turnProbability)
        {
            currentDirection = 1 - currentDirection;
            straightCount = 0;
        }
        else
        {
            straightCount++;
        }
        return (currentDirection == 0) ? Vector3.left : Vector3.forward;
    }

    /// <summary>
    /// 当一个地块掉落时调用：移除并根据阈值补充
    /// </summary>
    public void RemoveGround(GameObject ground)
    {
        if (activeGrounds.Remove(ground))
        {
            ZLog.Log($"Removed ground {ground.name}. Remaining: {activeGrounds.Count}");
            if (activeGrounds.Count <= refillThreshold)
            {
                ZLog.Log($"Below threshold ({activeGrounds.Count} ≤ {refillThreshold}), refilling {refillCount} grounds");
                GenerateRandomNewGrounds(refillCount);
            }
        }
    }
}
