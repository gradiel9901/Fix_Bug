using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpaceShooter : MonoBehaviour
{
    public SpaceshipController SpaceShip;
    public int health;
    public float minFR, MaxFR;
    private float FireRate;
    private float storedFireRate;
    public float BulletSpeed;
    public GameObject bulletPrefab;

    public float moveSpeed;
    public float moveInterval;

    public Vector3 InitialPosition;

    void Start()
    {
        InitialPosition = transform.position;
        FireRate = Random.Range(minFR, MaxFR);
        storedFireRate = FireRate;
        InvokeRepeating("MoveEnemy", 5, moveInterval);
    }

    void Update()
    {
        FireRate -= Time.deltaTime;
        if (FireRate <= 0)
        {
            SpawnBullet();
            FireRate = storedFireRate;
        }
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            health--;
            if (health <= 0)
            {
                SpaceShip.score++;
                gameObject.SetActive(false);
            }
            ObjectPoolManager.Instance.ReturnToPool("PlayerBullets", collision.gameObject);
        }
    }

    public void SpawnBullet()
    {
        GameObject bullet = ObjectPoolManager.Instance.GetFromPool("EnemyBullets", transform.position, Quaternion.identity);
        if (bullet != null)
        {
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.linearVelocity = new Vector2(0f, -BulletSpeed);
        }
    }

    public void MoveEnemy()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }

    // Add this method to dynamically set fire rate
    public void SetFireRate(float rate)
    {
        storedFireRate = rate;
        FireRate = rate;
    }
}
