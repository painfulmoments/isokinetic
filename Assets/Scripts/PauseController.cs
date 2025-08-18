// PauseController.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using ZTools;

public class PauseController : MonoBehaviour
{
    [Header("Pause Panel")]
    [SerializeField] private GameObject pausePanel;
    [Header("Gameplay UI")]
    [SerializeField] private GameObject pauseButton;

    private bool isPaused = false;

    private void Start()
    {
        #region Game NewContent5: Ensure Unpaused at Start
        ZLog.Log("PauseController Start - unpaused");
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
        #endregion
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        #region Game NewContent6: Pause via GameManager
        ZLog.Log("PauseController: PauseGame");
        GameManager.o.GamePause();
        pausePanel.SetActive(true);
        pausePanel.transform.SetAsLastSibling();
        pauseButton.SetActive(false);
        var resumeBtn = pausePanel.transform.Find("ResumeButton");
        if (resumeBtn != null)
            EventSystem.current.SetSelectedGameObject(resumeBtn.gameObject);
        isPaused = true;
        #endregion
    }

    public void ResumeGame()
    {
        #region Game NewContent7: Continue via GameManager
        ZLog.Log("PauseController: ResumeGame");
        GameManager.o.GameContinue();
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        isPaused = false;
        #endregion
    }

    public void OnResumeClicked()
    {
        #region Game NewContent8: Resume Button
        ResumeGame();
        #endregion
    }

    public void OnMainMenuClicked()
    {
        #region Game NewContent9: Return to Main Menu
        ZLog.Log("PauseController: OnMainMenuClicked");
        GameManager.o.GameEnd();
        SceneManager.LoadScene("Start");
        #endregion
    }
}
