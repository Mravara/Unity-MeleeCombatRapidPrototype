using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseHeavySwingPlayerState : AbstractPlayerState
{
    private float lastSpeed;
    
    public override void OnEnterState()
    {
        base.OnEnterState();

        lastSpeed = player.TPC.MoveSpeed;
        player.TPC.MoveSpeed = 0f;
        player.Animator.SetTrigger("ReleaseHeavyAttack");
    }
    
    public override void OnExitState()
    {
        base.OnEnterState();

        player.TPC.MoveSpeed = lastSpeed;
    }
}
