using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBehaviour : MonoBehaviour
{
    public float Speed;
    public int healthPoints;
    public TextMeshProUGUI healthText;
    public GameObject gameOverScreen;

    void Start()
    {
        healthPoints = 3;
    }

    private void Update()
    {
        healthText.text = healthPoints.ToString();
        if (healthPoints <= 0)
        {
            gameOverScreen.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveInput = new Vector3(horizontalInput, verticalInput, 0);
        transform.position += Time.deltaTime * Speed * moveInput;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            healthPoints--;
        }
        if (collision.CompareTag("PowerUp"))
        {
            healthPoints++;
            ObjectPoolManager.Instance.ReturnToPool("PowerUps", collision.gameObject);
        }
    }

    public void RestartButton()
    {
        gameOverScreen.SetActive(false);
        healthPoints = 3;
        transform.position = new Vector3(-9, 0, 0);
    }
}
