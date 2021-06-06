using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetAlert : MonoBehaviour {
    public void ResetHighScore() {
        PlayerPrefs.SetInt("HighScore", 0);
    }
}
