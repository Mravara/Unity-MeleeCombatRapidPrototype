using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldBlockPlayerState : AbstractPlayerState
{
    private float lastSpeed;
    private static readonly int block = Animator.StringToHash("Block");

    public override void OnEnterState()
    {
        base.OnEnterState();

        lastSpeed = player.TPC.MoveSpeed;
        player.TPC.MoveSpeed = 3f;
        player.Animator.SetBool(block, true);
        player.updateStamina = false;
    }
    
    public override void OnExitState()
    {
        base.OnExitState();

        player.TPC.MoveSpeed = lastSpeed;
        player.Animator.SetBool(block, false);
        player.updateStamina = true;
    }
    
    public override void UpdateState()
    {
        base.UpdateState();
        
        if (!isActive)
            return;
        
        if (!Input.GetMouseButton(1))
        {
            EndBlock();
        }
    }
    
    private void EndBlock()
    {
        player.ChangeState(PlayerStateType.EndBlock);
    }
}
