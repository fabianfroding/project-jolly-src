using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    public bool CanDash { get; private set; }
    private bool isHolding; 
    private bool dashInputStop;
    private float lastDashTime;
    private Vector2 dashDirection;
    private Vector2 dashDirectionInput;
    private GameObject timeSlowSFX;
    
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();

        CanDash = false;
        player.InputHandler.UseDashInput();

        isHolding = true;
        dashDirection = Vector2.right * Movement.FacingDirection;

        Time.timeScale = playerStateData.holdTimeScale;
        startTime = Time.unscaledTime;

        timeSlowSFX = GameObject.Instantiate(playerStateData.dashTimeSlowSFX);
        timeSlowSFX.transform.position = player.transform.position;
    }

    public override void Exit()
    {
        base.Exit();
        
        // Don't want to decrease when dashing down
        if (Movement.CurrentVelocity.y > 0)
        {
            Movement.SetVelocityY(Movement.CurrentVelocity.y * playerStateData.dashEndYMultiplier);
        }

        Movement.SetDrag(0f);
        isAbilityDone = true;
        lastDashTime = Time.time;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (isHolding)
            {
                // Holding
                dashDirectionInput = player.InputHandler.DashDirectionInput;
                dashInputStop = player.InputHandler.DashInputStop;
                
                if (dashDirectionInput != Vector2.zero)
                {
                    dashDirection = dashDirectionInput;
                    dashDirection.Normalize();
                }

                // Put indicator in right direction
                //float angle = Vector2.SignedAngle(Vector2.right, dashDirection);

                if (dashInputStop || Time.unscaledTime >= startTime + playerStateData.maxHoldTime)
                {
                    isHolding = false;
                    Time.timeScale = 1f;
                    startTime = Time.time;

                    Movement.CheckIfShouldFlip(Mathf.RoundToInt(dashDirection.x));
                    Movement.SetDrag(playerStateData.drag);
                    Movement.SetVelocity(playerStateData.dashVelocity, dashDirection);

                    if (timeSlowSFX) GameObject.Destroy(timeSlowSFX);
                    GameObject tempGO = GameObject.Instantiate(playerStateData.dashSFX);
                    tempGO.transform.position = player.transform.position;
                }
            }
            else {
                // Dashing
                Movement.SetVelocity(playerStateData.dashVelocity, dashDirection);
                if (Time.time >= startTime + playerStateData.dashTime)
                {
                    Movement.SetDrag(0f);
                    isAbilityDone = true;
                    lastDashTime = Time.time;
                }
            }
        }
    }

    public bool CheckIfCanDash()
    {
        return CanDash && Time.time >= lastDashTime + playerStateData.dashCooldown;
    }

    public void ResetCanDash() => CanDash = true;
}
