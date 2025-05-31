using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelArrow : MonoBehaviour
{
    private PlayerController player;

    public LayerMask arrowLayer;

    void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        if (player == null || !player.IsZAttacking()) return;

        // �÷��̾� ���� ���� ���� ����
        Collider2D[] arrows = Physics2D.OverlapBoxAll(transform.position, GetComponent<BoxCollider2D>().bounds.size, 0f, arrowLayer
        );

        foreach (Collider2D arrow in arrows)
        {
            if (arrow.CompareTag("Arrow"))
            {
                Destroy(arrow.gameObject);
                Debug.Log("ȭ�� ����");
            }
        }
    }
}
