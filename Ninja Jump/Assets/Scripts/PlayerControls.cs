using UnityEngine;

public class PlayerControls : MonoBehaviour {
    [SerializeField]
    private float StartingRunSpeed, acceleration, jumpSpeed, scoreSpeed;
    [SerializeField]
    private GameObject gameOverMenu;
    [SerializeField]
    private GameObject[] scoreTexts;
    [SerializeField]
    private AudioClip hitSoundClip, jumpSoundClip, runSoundClip;
    [SerializeField]
    private float hitSoundVol, jumpSoundVol, runSoundVol;

    private AudioSource hitSound, jumpSound, runSound;
    private float runSpeed;
    private int score;
    private Rigidbody2D rb;
    private bool midAir = false, gameOver = false;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.velocity = new Vector2(rb.velocity.x, StartingRunSpeed);

        hitSound = AddAudio(hitSoundClip, false, false, hitSoundVol);
        jumpSound = AddAudio(jumpSoundClip, false, false, jumpSoundVol);
        runSound = AddAudio(runSoundClip, true, true, runSoundVol);
    }

    void Update() {
        if (gameOver == false) {
            SpeedAndScoreUpdate();
            rb.velocity = new Vector2(rb.velocity.x, runSpeed);
            if (midAir == false) {
                if (Input.GetMouseButtonDown(0)) {
                    Jump();
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Wall Collider") {
            rb.velocity = new Vector2(0, runSpeed);
            runSound.Play();
            midAir = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Obstacle") {
            runSound.Stop();
            hitSound.Play();
            gameOver = true;
            Time.timeScale = 0;
            gameOverMenu.SetActive(true);
            if (score > PlayerPrefs.GetInt("HighScore", 0)) {
                PlayerPrefs.SetInt("HighScore", score);
            }
        }
    }

    void SpeedAndScoreUpdate() {
        score = (int)(Time.timeSinceLevelLoad * scoreSpeed);
        runSpeed = StartingRunSpeed + (acceleration * ((float)score / 1000));
        foreach (GameObject sText in scoreTexts) {
            sText.GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString();
        }
    }

    public AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol) {
        AudioSource newAudio = gameObject.AddComponent<AudioSource>();
        newAudio.clip = clip;
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.volume = vol;
        return newAudio;
    }

    void Jump() {
        midAir = true;
        runSound.Pause();
        jumpSound.Play();
        rb.velocity = new Vector2(jumpSpeed, runSpeed);
        gameObject.transform.localScale = new Vector3(
            -gameObject.transform.localScale.x,
            gameObject.transform.localScale.y,
            gameObject.transform.localScale.z);
        jumpSpeed = -jumpSpeed;
    }
}
