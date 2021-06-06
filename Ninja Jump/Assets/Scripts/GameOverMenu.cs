using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour {
    public void PlayAgain() {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
    public void MainMenu() {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
