using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;             // �÷��̾�
    public float fixedY;                 // ���� Y��

    public Transform leftLimit;          // ���� ���� ���� (�� GameObject)
    public Transform rightLimit;         // ������ ���� ���� (�� GameObject)

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
