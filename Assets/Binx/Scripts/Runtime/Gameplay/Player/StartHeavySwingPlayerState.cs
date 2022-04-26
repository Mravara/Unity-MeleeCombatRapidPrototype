using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHeavySwingPlayerState : AbstractPlayerState
{
    private static readonly int attack = Animator.StringToHash("Attack");

    public override void OnEnterState()
    {
        base.OnEnterState();

        player.Animator.SetBool(attack, true);
    }
    
    public override void OnExitState()
    {
        base.OnExitState();

        player.Animator.SetBool(attack, false);
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
        player.Animator.SetBool(attack, false);
        player.ChangeState(PlayerStateType.Swing);
    }
}
