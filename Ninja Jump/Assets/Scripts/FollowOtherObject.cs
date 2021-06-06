using UnityEngine;

public class FollowOtherObject : MonoBehaviour {
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float offsetY = 0f;

    private void Update() {
        Vector3 targetPos = new Vector3(transform.position.x, target.position.y + offsetY, transform.position.z);
        transform.position = targetPos;
    }
}
