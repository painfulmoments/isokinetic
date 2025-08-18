// PlayerDeath.cs
using UnityEngine;
using ZTools;

public class PlayerDeath : MonoBehaviour
{
    public static PlayerDeath Instance { get; private set; }
    [SerializeField] private GameObject deathPanel;

    private void Awake()
    {
        #region Game NewContent11: Singleton Init & Hide Panel
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
        deathPanel.SetActive(false);
        ZLog.Log("PlayerDeath Awake");
        #endregion
    }

    public void Die()
    {
        #region Game NewContent12: Handle Death
        ZLog.Log("PlayerDeath: Die");
        GetComponent<BallMovementController>().enabled = false;
        deathPanel.SetActive(true);
        GameManager.o.GameEnd();
        #endregion
    }
}
