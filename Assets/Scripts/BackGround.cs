// Background.cs
using UnityEngine;
using ZTools;

public class Background : MonoBehaviour
{
    public float multiplier = 0.5f;
    [SerializeField] private Transform follow;
    private Material mat;

    private void Start()
    {
        #region Game NewContent24: Init Background Material
        mat = GetComponent<Renderer>().material;
        ZLog.Log("Background Start ¨C material initialized");
        #endregion
    }

    private void LateUpdate()
    {
        float offset = follow.position.z * multiplier;
        mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
