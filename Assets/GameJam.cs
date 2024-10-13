using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameJam : MonoBehaviour
{
    public float moveSpeed;
    public float maxSpeed;
    public Rigidbody2D rb;

    private Vector2 moveDirection;

    public float fuel = 100f; // Yak�t miktar�
    public float fuelConsumptionRate = 2f; // Yak�t t�ketim h�z� (Daha yava� yak�t t�ketimi i�in azalt�ld�)
    public float minFuelToMove = 5f; // Hareket etmek i�in minimum yak�t

    void FixedUpdate()
    {
        if (fuel > 0) // Yak�t varsa hareket et
        {
            Move();
            ConsumeFuel();
        }
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");

        moveDirection = new Vector2(moveX, 0).normalized;
    }

    void Move()
    {
        float moveY = Input.GetKey(KeyCode.W) ? 1 : 0;
        Vector2 vel = new Vector2(moveDirection.x * moveSpeed * 5 * moveY, rb.velocity.y + moveY * moveSpeed);
        vel.y = Mathf.Clamp(vel.y, -100, maxSpeed);
        rb.velocity = vel;
    }

    void ConsumeFuel()
    {
        if (fuel > 0)
        {
            fuel -= fuelConsumptionRate * Time.fixedDeltaTime; // Yak�t daha yava� t�keniyor
            if (fuel <= 0)
            {
                fuel = 0; // Yak�t s�f�ra inerse, hareketi durdur
                Debug.Log("Yak�t bitti!");
            }
        }
    }

    void Update()
    {
        ProcessInputs();
    }
}
