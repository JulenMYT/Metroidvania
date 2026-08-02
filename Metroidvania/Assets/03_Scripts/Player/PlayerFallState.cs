using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(Player player) : base(player) { }

    public override void Enter()
    {
        base.Enter();

        anim.SetBool("isFalling", true);
    }

    public override void Update()
    {
        base.Update();

        if (!player.isGrounded && player.isTouchingWall && Mathf.Abs(MoveInput.x) > 0.1f && MoveInput.x * player.facingDirection > 0)
        {
            player.ChangeState(player.wallSlideState);
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

        float speed = RunPressed ? player.runSpeed : player.walkSpeed;
        float targetSpeed = speed * MoveInput.x;

        rb.linearVelocity = new Vector2(targetSpeed, rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();

        anim.SetBool("isFalling", false);

        JumpPressed = false;
        JumpReleased = false;
    }
}
