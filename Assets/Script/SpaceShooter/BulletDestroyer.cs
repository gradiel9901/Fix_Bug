using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            ObjectPoolManager.Instance.ReturnToPool("PlayerBullets", collision.gameObject);
        }
        if (collision.CompareTag("EnemyBullet"))
        {
            ObjectPoolManager.Instance.ReturnToPool("EnemyBullets", collision.gameObject);
        }
    }
}
