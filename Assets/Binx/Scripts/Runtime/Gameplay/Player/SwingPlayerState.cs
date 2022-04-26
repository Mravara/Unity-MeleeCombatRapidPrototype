using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingPlayerState : AbstractPlayerState
{
    private float lastSpeed;
    private float speedWhenSwinging = 15f;
    
    public override void OnEnterState()
    {
        base.OnEnterState();

        player.blockMovement = true;
        player.Animator.SetTrigger("StartSwing");
    }
    
    public override void OnExitState()
    {
        base.OnExitState();

        player.blockMovement = false;
        player.TPC.Stop();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (currentStateDuration < 0.1f)
        {
            player.TPC.ManualMove(player.transform.forward, speedWhenSwinging);
        }
    }
}
