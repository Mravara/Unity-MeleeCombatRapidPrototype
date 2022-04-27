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
    }
    
    public override void OnExitState()
    {
        base.OnExitState();

        player.TPC.MoveSpeed = lastSpeed;
        player.Animator.SetBool(block, false);
    }
    
    public override void UpdateState()
    {
        base.UpdateState();
        
        if (!isActive)
            return;
        
        if (Input.GetMouseButtonUp(1))
        {
            TryEndBlock();
        }
    }
    
    private void TryEndBlock()
    {
        player.ChangeState(PlayerStateType.EndBlock);
    }
}
