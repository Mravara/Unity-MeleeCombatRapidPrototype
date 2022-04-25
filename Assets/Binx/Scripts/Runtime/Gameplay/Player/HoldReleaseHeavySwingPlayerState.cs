using System.Collections;
using System.Collections.Generic;
using Binx;
using UnityEngine;

public class HoldReleaseHeavySwingPlayerState : AbstractPlayerState
{
    private float lastSpeed;
    
    public override void OnEnterState()
    {
        base.OnEnterState();

        lastSpeed = player.TPC.MoveSpeed;
        player.TPC.MoveSpeed = 0f;
        Player.instance.ShakeCameraStrong();
    }
    
    public override void OnExitState()
    {
        base.OnEnterState();

        player.TPC.MoveSpeed = lastSpeed;
        player.Animator.SetTrigger("EndHeavyAttack");
    }
}
