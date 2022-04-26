using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHeavySwingPlayerState : AbstractPlayerState
{
    public override void OnEnterState()
    {
        base.OnEnterState();

        player.Animator.SetBool("Attack", true);
    }
    
    public override void UpdateState()
    {
        base.UpdateState();
        
        if (Input.GetMouseButtonUp(0))
        {
            EndAttack();
        }
    }
    
    private void EndAttack()
    {
        player.Animator.SetBool("Attack", false);
        player.ChangeState(PlayerStateType.Swing);
    }
}
