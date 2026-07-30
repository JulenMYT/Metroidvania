using UnityEngine;

public class PlayerSlideState : PlayerState
{
    private float slideTimer;
    private float slideStopTimer;
    private int facingDirection;

    public PlayerSlideState(Player player) : base(player) { }

    public override void Enter()
    {
        base.Enter();

        slideTimer = player.slideDuration;
        slideStopTimer = 0;
        facingDirection = player.facingDirection;

        player.SetColliderSlide();
        anim.SetBool("isSliding", true);
    }

    public override void Update()
    {
        base.Update();

        if (JumpPressed && !player.CheckForCeiling())
        {
            player.ChangeState(player.jumpState);
        }
        else if (slideTimer > 0)
        {
            slideTimer -= Time.deltaTime;
        }
        else if (slideStopTimer <= 0)
        {
            slideStopTimer = player.slideStopDuration;
        }
        else
        {
            slideStopTimer -= Time.deltaTime;

            if (slideStopTimer <= 0)
            {
                if (player.CheckForCeiling() || MoveInput.y < -0.1f)
                {
                    player.ChangeState(player.crouchState);
                }
                else
                {
                    player.ChangeState(player.idleState);
                }
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (slideTimer > 0)
        {
            player.facingDirection = facingDirection;
            rb.linearVelocity = new Vector2(player.slideSpeed * facingDirection, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.SetColliderNormal();
        anim.SetBool("isSliding", false);
    }
}
