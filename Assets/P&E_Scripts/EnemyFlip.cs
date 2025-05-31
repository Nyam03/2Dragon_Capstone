using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlip : MonoBehaviour
{
    public Transform player;
    private bool isFacingRight = true;

    private void Start()
    {
        player = GameObject.Find("Hero").transform;
    }

    void Update()
    {
        if (player == null) return;

        if (player.position.x > transform.position.x && !isFacingRight) // 플레이어 오른쪽 위치, 적이 왼쪽을 볼 경우
        {
            Flip();
        }
        else if (player.position.x < transform.position.x && isFacingRight) // 반대
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
