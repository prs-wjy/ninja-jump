using UnityEngine;

public class VerticalLoop : MonoBehaviour {
    [SerializeField]
    private Camera mainCamera;
    private Vector2 ScreenBounds;
    private GameObject[] walls;

    void Start() {
        ScreenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        walls = GameObject.FindGameObjectsWithTag("Wall");
        LoadChildObject();
    }

    void Update() {
        foreach (GameObject obj in walls) {
            repositionChildObjects(obj);
        }
    }

    void LoadChildObject() {
        foreach (GameObject obj in walls) {
            float objHeight = obj.GetComponent<SpriteRenderer>().bounds.size.y;
            int childsNeeded = (int)Mathf.Ceil(ScreenBounds.y * 2 / objHeight) + 1;
            for (int i = 0; i <= childsNeeded; i++) {
                GameObject clone = Instantiate(obj) as GameObject;
                foreach (Transform child in clone.transform) {
                    GameObject.Destroy(child.gameObject);
                }
                clone.transform.SetParent(obj.transform);
                clone.transform.position = new Vector3(obj.transform.position.x, (objHeight / 2) + (objHeight * i) - ScreenBounds.y, obj.transform.position.z);
                clone.name = obj.name + "_" + i;
            }
            Destroy(obj.GetComponent<SpriteRenderer>());
        }
    }

    void repositionChildObjects(GameObject obj) {
        Transform[] children = obj.GetComponentsInChildren<Transform>();
        if (children.Length > 1) {
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[children.Length - 1].gameObject;
            float halfObjectHeight = lastChild.GetComponent<SpriteRenderer>().bounds.extents.y;
            if (mainCamera.transform.position.y - ScreenBounds.y > firstChild.transform.position.y + halfObjectHeight) {
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(lastChild.transform.position.x, lastChild.transform.position.y + halfObjectHeight * 2, lastChild.transform.position.z);
            } else if (mainCamera.transform.position.y - ScreenBounds.y < firstChild.transform.position.y - halfObjectHeight) {
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.x, firstChild.transform.position.y - halfObjectHeight * 2, firstChild.transform.position.z);
            }
        }
    }
}
