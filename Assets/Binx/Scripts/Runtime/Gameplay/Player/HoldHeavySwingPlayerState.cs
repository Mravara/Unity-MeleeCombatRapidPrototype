using System.Collections;
using System.Collections.Generic;
using Binx;
using UnityEngine;

public class HoldHeavySwingPlayerState : AbstractPlayerState
{
    private float lastSpeed;
    private static readonly int attack = Animator.StringToHash("Attack");

    public override void OnEnterState()
    {
        base.OnEnterState();

        lastSpeed = player.TPC.MoveSpeed;
        player.TPC.MoveSpeed = 3f;
        player.Animator.SetBool(attack, true);
    }
    
    public override void OnExitState()
    {
        base.OnExitState();

        player.TPC.MoveSpeed = lastSpeed;
        player.Animator.SetBool(attack, false);
    }
    
    public override void UpdateState()
    {
        base.UpdateState();
        
        if (Input.GetMouseButtonUp(0))
        {
            TryEndAttack();
        }
    }
    
    private void TryEndAttack()
    {
        player.ChangeState(PlayerStateType.ReleaseHeavy);
    }
}
