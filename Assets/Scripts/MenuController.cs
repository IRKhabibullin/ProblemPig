using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject menuUI;
    [SerializeField] GameObject quitPanel;
    [SerializeField] GameObject gameOverPanel;

    public UnityEvent OnNewGame;

    public void OnQuitPressed()
    {
        ToggleMenu(true);
        quitPanel.SetActive(true);
    }

    public void OnQuitCancelled()
    {
        ToggleMenu(false);
        quitPanel.SetActive(false);
    }

    public void OnGameOver()
    {
        ToggleMenu(true);
        gameOverPanel.SetActive(true);
    }

    public void ToggleMenu(bool enable)
    {
        gameUI.SetActive(!enable);
        menuUI.SetActive(enable);

        if (enable)
            Pause();
        else
        {
            quitPanel.SetActive(false);
            gameOverPanel.SetActive(false);
            Unpause();
        }
    }

    public void NewGame()
    {
        OnNewGame?.Invoke();

        foreach (var bomb in FindObjectsOfType<DirtBomb>())
        {
            Destroy(bomb.gameObject);
        }
        ToggleMenu(false);
    }

    private void Pause()
    {
        Time.timeScale = 0;
    }

    private void Unpause()
    {
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
