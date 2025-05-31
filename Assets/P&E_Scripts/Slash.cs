using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public int damage = 30;
    private Vector3 startPos;
    public float maxDistance = 10f;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(startPos, transform.position);
        if (distance >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Slash Ãæµ¹ °¨ÁöµÊ: " + other.name);
        if (other.CompareTag("Enemy"))
        {
            Enemy e = other.GetComponent<Enemy>();
            if (e != null)
            {
                e.TakeDamage(damage);

            }
            Boss b = other.GetComponent<Boss>();
            if (b != null)
            {
                b.TakeDamage(damage);
            }
        }
    }
}
