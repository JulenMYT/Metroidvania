using System.Collections;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    private float horizontalJumpPercent = 0.4f;
    private float wallJumpDelay = 0.2f;
    private float changeStateTime;

    public PlayerWallJumpState(Player player) : base(player) { }

    public override void Enter()
    {
        base.Enter();

        anim.SetBool("isJumping", true);
        rb.linearVelocity = Vector2.zero;
        rb.linearVelocity = new Vector2(-player.facingDirection * horizontalJumpPercent, 1f) * player.jumpForce;

        JumpPressed = false;
        JumpReleased = false;

        changeStateTime = Time.time + wallJumpDelay;
    }

    public override void Update()
    {
        base.Update();

        if (Time.time > changeStateTime)
        {
            player.ChangeState(player.fallState);
        }
        else if (player.isGrounded && rb.linearVelocity.y < 0.1f)
        {
            player.ChangeState(player.idleState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        player.ApplyVariableGravity();

        if (JumpReleased && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * player.jumpCutMultiplier);
            JumpReleased = false;
        }
    }

    public override void Exit()
    {
        base.Exit();

        anim.SetBool("isJumping", false);
    }
}
