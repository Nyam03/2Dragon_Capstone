using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;             // 플레이어
    public float fixedY;                 // 고정 Y값

    public Transform leftLimit;          // 왼쪽 제한 지점 (빈 GameObject)
    public Transform rightLimit;         // 오른쪽 제한 지점 (빈 GameObject)

    private float minX;
    private float maxX;

    private bool isLocked = false;
    private Vector3 lockedPosition;

    private float lockSmoothSpeed = 3f;

    void Start()
    {
        if (target != null)
        {
            fixedY = transform.position.y;
        }

        if (leftLimit != null && rightLimit != null)
        {
            minX = leftLimit.position.x;
            maxX = rightLimit.position.x;
        }
    }

    void LateUpdate()
    {
        if (isLocked)
        {
            Vector3 desiredPosition = new Vector3(lockedPosition.x, lockedPosition.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * lockSmoothSpeed);
        }
        else if(target != null)
        {
            float clampedX = Mathf.Clamp(target.position.x, minX, maxX);
            transform.position = new Vector3(clampedX, fixedY, transform.position.z);
        }
    }

    public void LockCamera(Vector3 position)
    {
        isLocked = true;
        lockedPosition = position;
    }

    public void UnlockCamera()
    {
        isLocked = false;
    }
}
