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

            // 카메라 고정
            cameraFollow.LockCamera(cameraLockPosition);

            // 벽 활성화
            leftWall.SetActive(true);
            rightWall.SetActive(true);
        }
    }

    // 보스 죽으면 호출
    //public void EndBossZone()
    //{
    //    cameraFollow.UnlockCamera();
    //    leftWall.SetActive(false);
    //    rightWall.SetActive(false);
    //}
}
