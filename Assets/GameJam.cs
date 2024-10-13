using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameJam : MonoBehaviour
{
    public float moveSpeed;
    public float maxSpeed;
    public Rigidbody2D rb;

    private Vector2 moveDirection;

    public float fuel = 100f; // Yakýt miktarý
    public float fuelConsumptionRate = 2f; // Yakýt tüketim hýzý (Daha yavaþ yakýt tüketimi için azaltýldý)
    public float minFuelToMove = 5f; // Hareket etmek için minimum yakýt

    void FixedUpdate()
    {
        if (fuel > 0) // Yakýt varsa hareket et
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
            fuel -= fuelConsumptionRate * Time.fixedDeltaTime; // Yakýt daha yavaþ tükeniyor
            if (fuel <= 0)
            {
                fuel = 0; // Yakýt sýfýra inerse, hareketi durdur
                Debug.Log("Yakýt bitti!");
            }
        }
    }

    void Update()
    {
        ProcessInputs();
    }
}
