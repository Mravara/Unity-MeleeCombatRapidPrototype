using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingPlayerState : AbstractPlayerState
{
    private float lastSpeed;
    private float lastSpeedChargeRate;
    
    public override void OnEnterState()
    {
        base.OnEnterState();

        lastSpeed = player.TPC.MoveSpeed;
        lastSpeedChargeRate = player.TPC.SpeedChangeRate;
        lastSpeedChargeRate = 100f;
        player.TPC.MoveSpeed = 10f;
        player.Animator.SetTrigger("StartSwing");
    }
    
    public override void OnExitState()
    {
        base.OnEnterState();

        player.TPC.MoveSpeed = lastSpeed;
        player.TPC.SpeedChangeRate = lastSpeedChargeRate;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (currentStateDuration > 0.1f)
        {
            player.TPC.MoveSpeed = lastSpeed;
            player.TPC.SpeedChangeRate = lastSpeedChargeRate;
        }
    }
}
