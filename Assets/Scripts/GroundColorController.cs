// GroundColorController.cs
using UnityEngine;
using ZTools;

public class GroundColorController : MonoBehaviour
{
    [SerializeField] private Material groundMaterial;
    [SerializeField] private Color[] colors;
    [SerializeField] private float lerpValue;
    [SerializeField] private float time;

    private int colorIndex;
    private float currentTime;

    private void Start()
    {
        #region Game NewContent31: Init Color Controller
        colorIndex = 0;
        currentTime = time;
        ZLog.Log("GroundColorController Start");
        #endregion
    }

    private void Update()
    {
        #region Game NewContent32: Color Update Loop
        if (currentTime <= 0f)
        {
            colorIndex = (colorIndex + 1) % colors.Length;
            currentTime = time;
            ZLog.Log($"GroundColorController: Next color index {colorIndex}");
        }
        else
        {
            currentTime -= Time.deltaTime;
        }

        groundMaterial.color = Color.Lerp(groundMaterial.color, colors[colorIndex], lerpValue * Time.deltaTime);
        #endregion
    }

    private void OnDestroy()
    {
        #region Game NewContent33: Reset Material on Destroy
        if (colors.Length > 1)
            groundMaterial.color = colors[1];
        ZLog.Log("GroundColorController OnDestroy - reset color");
        #endregion
    }
}
