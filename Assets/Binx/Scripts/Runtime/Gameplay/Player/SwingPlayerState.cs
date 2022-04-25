using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingPlayerState : AbstractPlayerState
{
    private float speedWhenSwinging = 15f;
    
    public override void OnEnterState()
    {
        base.OnEnterState();

        player.blockMovement = true;
        player.Animator.SetTrigger("StartSwing");
    }
    
    public override void OnExitState()
    {
        base.OnEnterState();

        player.blockMovement = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (currentStateDuration > 0.1f)
        {
            player.TPC.ManualMove(player.transform.forward, speedWhenSwinging);
        }
    }
}
