using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SpaceshipController : MonoBehaviour
{
    public List<EnemySpaceShooter> Enemies;

    public float Speed;
    public float BulletSpeed;
    public GameObject bulletPrefab;
    public Transform BulletSpawnHere;
    public GameObject GameClearScreen;
    public TextMeshProUGUI textValue, hpValue;
    public int score;
    public int hitponts;
    bool isGameClear = false;
    private int storeHP;
    public GameObject GameOverScreen;
    private bool canMove = true;
    private bool canShoot = true;

    void Start()
    {
        storeHP = hitponts;
    }

    void Update()
    {
        textValue.text = score.ToString();
        hpValue.text = hitponts.ToString();

        if (Input.GetKeyDown(KeyCode.Space) && canShoot)
        {
            SpawnBullet();
        }

        if (hitponts <= 0)
        {
            canShoot = false;
            canMove = false;
            GameOverScreen.SetActive(true);
            hitponts = 0;
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            Vector3 moveInput = new Vector3(horizontalInput, 0, 0);
            transform.position += Time.deltaTime * Speed * moveInput;
        }
    }

    public void SpawnBullet()
    {
        GameObject bullet = ObjectPoolManager.Instance.GetFromPool("PlayerBullets", BulletSpawnHere.position, Quaternion.identity);
        if (bullet != null)
        {
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.linearVelocity = new Vector2(0f, BulletSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            hitponts--;
            ObjectPoolManager.Instance.ReturnToPool("EnemyBullets", collision.gameObject);
        }
    }

    public void RestartGame()
    {
        foreach (EnemySpaceShooter enemy in Enemies)
        {
            enemy.transform.position = enemy.InitialPosition;
            enemy.gameObject.SetActive(false);
        }
        StartCoroutine(DelayEnemiesActive());
        canMove = true;
        canShoot = true;
        hitponts = storeHP;
        score = 0;
        isGameClear = false;
        GameOverScreen.SetActive(false);
    }

    IEnumerator DelayEnemiesActive()
    {
        yield return new WaitForSeconds(0.25f);
        foreach (EnemySpaceShooter enemy in Enemies)
        {
            enemy.gameObject.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnGameClear()
    {
        isGameClear = true;
        foreach (EnemySpaceShooter enemy in Enemies)
        {
            if (enemy.gameObject.activeSelf)
            {
                isGameClear = false;
                break;
            }
        }
        if (isGameClear)
        {
            GameClearScreen.SetActive(true);
        }
    }
}
