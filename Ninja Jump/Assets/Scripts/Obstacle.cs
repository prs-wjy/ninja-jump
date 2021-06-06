using UnityEngine;

public class Obstacle : MonoBehaviour {
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Transform clonesParent;
    [SerializeField]
    private float[] timeRespawnRange = { 0.8f, 2.2f };
    [SerializeField]
    private float fallObstacleSpeed = 2.2f;
    private Vector2 screenBounds;
    private GameObject[] obstacles;
    [HideInInspector]
    private bool invokeState;

    void Start() {
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obj in obstacles) {
            obj.SetActive(false);
        }
    }

    void Update() {
        if (Time.timeScale == 0) {
            CancelInvoke();
        } else {
            if (!invokeState) {
                float time = Random.Range(timeRespawnRange[0], timeRespawnRange[1]);
                Invoke("ObstacleRandomize", time);
                invokeState = true;
            }

            foreach (Transform trf in clonesParent.GetComponentsInChildren<Transform>()) {
                if (trf.GetInstanceID() != clonesParent.GetInstanceID()) {
                    float halfHeight = trf.GetComponent<SpriteRenderer>().bounds.extents.y;
                    if (trf.position.y + halfHeight < mainCamera.transform.position.y - screenBounds.y) {
                        Destroy(trf.gameObject);
                    }
                }
            }
        }
    }

    void ObstacleRandomize() {
        int obstacleIndex = Random.Range(0, obstacles.Length);
        int lr = Random.Range(0, 2);
        if (lr == 0) {
            lr = -1;
        }

        GameObject obj = Instantiate(obstacles[obstacleIndex]) as GameObject;
        obj.SetActive(true);
        float halfHeight = obj.GetComponent<SpriteRenderer>().bounds.extents.y;
        obj.transform.position = new Vector3((float)lr * obj.transform.position.x, mainCamera.transform.position.y + screenBounds.y + halfHeight, obj.transform.position.z);
        obj.transform.localScale = new Vector3((float)lr * obj.transform.localScale.x, obj.transform.localScale.y, obj.transform.localScale.z);
        obj.transform.SetParent(clonesParent);
        if (obj.name.Contains("Double")) {
            GameObject clone = Instantiate(obj) as GameObject;
            clone.transform.SetParent(obj.transform);
            clone.transform.position = new Vector3(-obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
            clone.transform.localScale = new Vector3(-clone.transform.localScale.x, clone.transform.localScale.y, clone.transform.localScale.z);
            clone.name = obj.name;
        }
        if (obj.name.Contains("Fall")) {
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, -fallObstacleSpeed);
        }
        invokeState = false;
    }
}
