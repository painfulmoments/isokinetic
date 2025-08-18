// PersistentEventSystem.cs
using UnityEngine;
using UnityEngine.EventSystems;
using ZTools;

public class PersistentEventSystem : MonoBehaviour
{
    private void Awake()
    {
        #region Game NewContent10: Persist EventSystem
        if (FindObjectsOfType<EventSystem>().Length > 1)
        {
            Destroy(gameObject);
            ZLog.Log("PersistentEventSystem destroyed duplicate");
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            ZLog.Log("PersistentEventSystem will persist");
        }
        #endregion
    }
}
