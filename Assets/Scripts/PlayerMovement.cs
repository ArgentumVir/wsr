using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    public int MoveSpeed = 30;
    private int currentDirection = 0;
    private Rigidbody2D rb;
    private Vector2 inputVector;
    float horizontalInput, verticalInput;
    float diagonalSpeedPenalty = 0.75f;
    public Sprite FrontSprite, BackSprite;
    public SpriteRenderer sr;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = FrontSprite;
    }

    void Update()
    {
        if (!PlayerStatus.Singleton.CanMove)
        {
            horizontalInput = 0;
            verticalInput = 0;
            return;
        }
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (horizontalInput != 0 && verticalInput != 0)
        {
            horizontalInput *= diagonalSpeedPenalty;
            verticalInput *= diagonalSpeedPenalty;
        } 

        rb.velocity = new Vector2(
            horizontalInput * MoveSpeed,
            verticalInput * MoveSpeed
        );

        UpdateSprite();
    }

    void UpdateSprite()
    {
        if (rb.velocity.y > 0 ) { sr.sprite = BackSprite; }
        if (rb.velocity.y < 0 ) { sr.sprite = FrontSprite; }
    }
}
