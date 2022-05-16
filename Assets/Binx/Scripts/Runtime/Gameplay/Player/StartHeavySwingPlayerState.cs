using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHeavySwingPlayerState : AbstractPlayerState
{
    private static readonly int attack = Animator.StringToHash("Attack");

    private float heavyStaminaCost;

    public override void OnEnterState()
    {
        base.OnEnterState();

        player.Animator.SetBool(attack, true);
        player.SpendStamina(staminaCost);
        heavyStaminaCost = player.GetState(PlayerStateType.ReleaseHeavy).staminaCost;
    }
    
    public override void OnExitState()
    {
        base.OnExitState();

        player.Animator.SetBool(attack, false);
    }
    
    public override void UpdateState()
    {
        base.UpdateState();
        
        if (!Input.GetMouseButton(0) || player.currentStamina < heavyStaminaCost)
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
