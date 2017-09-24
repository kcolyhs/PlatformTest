using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject {

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool disabled;
    

    public float jumpTakeOffSpeed = 7f;
    public float maxSpeed = 7f;


	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        disabled = false;
	}

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");
        if (Input.GetButton("Jump") && grounded && !disabled)
        {
            velocity.y = jumpTakeOffSpeed;
        }else if (Input.GetButtonUp("Jump")&&!disabled)
        {
            if (velocity.y > 0)
                velocity.y *= .5f;
        }
        //Flipping Sprite

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));

        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX; 
        }

        animator.SetBool("grounded", grounded);
		animator.SetFloat("velocityX", Mathf.Abs(velocity.x)/maxSpeed);
		animator.SetFloat("velocityY", Mathf.Abs(velocity.y)/maxSpeed);

        targetVelocity = move * maxSpeed;
    }
    
    public bool Disabled
    {
        get
        {
            return disabled;
        }
        set
        {
            disabled = value;
        }
    }
}
