using UnityEngine;

public class HighScore : MonoBehaviour {

    void Update() {
        GetComponent<TMPro.TextMeshProUGUI>().text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
}
