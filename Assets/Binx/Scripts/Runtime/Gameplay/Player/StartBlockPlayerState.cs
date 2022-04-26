using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBlockPlayerState : AbstractPlayerState
{
    private float lastSpeed;
    
    public override void OnEnterState()
    {
        base.OnEnterState();

        player.Animator.SetBool("Block", true);
        lastSpeed = player.TPC.MoveSpeed;
        player.TPC.MoveSpeed = 3f;
    }
    
    public override void OnExitState()
    {
        base.OnExitState();

        player.TPC.MoveSpeed = lastSpeed;
    }
    
    public override void UpdateState()
    {
        base.UpdateState();
        
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
