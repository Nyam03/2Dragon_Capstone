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

        if (player.position.x > transform.position.x && !isFacingRight) // �÷��̾� ������ ��ġ, ���� ������ �� ���
        {
            Flip();
        }
        else if (player.position.x < transform.position.x && isFacingRight) // �ݴ�
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
