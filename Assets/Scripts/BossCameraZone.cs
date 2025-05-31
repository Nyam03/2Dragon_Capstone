using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCameraZone : MonoBehaviour
{
    public CameraFollow cameraFollow;
    public Vector3 cameraLockPosition;

    public GameObject leftWall;
    public GameObject rightWall;

    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (other.CompareTag("Player"))
        {
            triggered = true;

            // ī�޶� ����
            cameraFollow.LockCamera(cameraLockPosition);

            // �� Ȱ��ȭ
            leftWall.SetActive(true);
            rightWall.SetActive(true);
        }
    }

    // ���� ������ ȣ��
    //public void EndBossZone()
    //{
    //    cameraFollow.UnlockCamera();
    //    leftWall.SetActive(false);
    //    rightWall.SetActive(false);
    //}
}
