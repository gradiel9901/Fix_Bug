using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed for smooth movement
    public float moveAmplitude = 1f; // How far the enemies move up and down
    public float fireRateMin = 1f; // Minimum firing rate for enemies
    public float fireRateMax = 5f; // Maximum firing rate for enemies

    private List<EnemySpaceShooter> enemies;

    void Start()
    {
        // Get all EnemySpaceShooter components in the children
        enemies = new List<EnemySpaceShooter>();
        foreach (Transform child in transform)
        {
            EnemySpaceShooter enemyScript = child.GetComponent<EnemySpaceShooter>();
            if (enemyScript != null)
            {
                enemies.Add(enemyScript);

                // Assign random firing rate to each enemy
                float randomFireRate = Random.Range(fireRateMin, fireRateMax);
                enemyScript.SetFireRate(randomFireRate);
            }
        }

        // Start smooth movement coroutine
        StartCoroutine(SmoothEnemyMovement());
    }

    IEnumerator SmoothEnemyMovement()
    {
        while (true)
        {
            // Calculate a smooth wave pattern for movement
            float verticalOffset = Mathf.Sin(Time.time * moveSpeed) * moveAmplitude;

            foreach (EnemySpaceShooter enemy in enemies)
            {
                if (enemy != null && enemy.gameObject.activeSelf)
                {
                    Vector3 newPosition = enemy.InitialPosition + new Vector3(0, verticalOffset, 0);
                    enemy.transform.position = Vector3.Lerp(enemy.transform.position, newPosition, Time.deltaTime * moveSpeed);
                }
            }

            yield return null; // Wait for the next frame
        }
    }
}
