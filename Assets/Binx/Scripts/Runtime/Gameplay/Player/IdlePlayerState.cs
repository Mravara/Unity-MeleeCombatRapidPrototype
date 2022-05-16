using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePlayerState : AbstractPlayerState
{
    public override void UpdateState()
    {
        base.UpdateState();
        
        if (!isActive)
            return;
        
        if (Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            TryBlock();
        }
        
        if (currentStateDuration > 0.17f)
            player.Animator.applyRootMotion = false;
    }
    
    private void TryAttack()
    {
        player.ChangeState(PlayerStateType.StartHeavy);
    }
    
    private void TryBlock()
    {
        player.ChangeState(PlayerStateType.StartBlock);
    }
}