using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] GameManager gameManager;
    [SerializeField] PlayerBulletManager bulletManager;

    public Vector2 bulletDirection = new Vector2(0, 50);
 
    private float moveSpeed = 10;

    void Start()
    {
        transform.position = new Vector2(0, 0);
    }

    void Update()
    {
        HandleInput(Time.deltaTime);
    }

    void HandleInput(float time)
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, moveY, 0) * moveSpeed * time;

        transform.Translate(movement);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireBullet();
        }
    }

    void FireBullet()
    {
        bulletManager.FireBullet(transform.position, bulletDirection);
    }

    
}
