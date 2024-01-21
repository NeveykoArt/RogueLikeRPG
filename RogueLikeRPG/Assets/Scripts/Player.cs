using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float movingSpeed = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();
        rb.MovePosition(rb.position + inputVector.normalized * (movingSpeed * Time.fixedDeltaTime));
    }
}
