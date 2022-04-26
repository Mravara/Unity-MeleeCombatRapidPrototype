using System.Collections;
using System.Collections.Generic;
using Binx;
using UnityEngine;

public class SwingEnemyState : AbstractEnemyState
{
    private static readonly int swing = Animator.StringToHash("Swing");

    public override void OnEnterState()
    {
        base.OnEnterState();

        // owner.NavMeshAgent.speed = 8f;
        // owner.NavMeshAgent.acceleration = 100f;
        owner.SetSpeed(8f);
        owner.Animator.SetTrigger(swing);
        owner.SwordCollider.enabled = true;
    }

    public override void OnExitState()
    {
        base.OnExitState();
        
        owner.SwordCollider.enabled = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        // owner.NavMeshAgent.SetDestination(Player.instance.GetStoppingPoint(transform.position, 2f));
    }
}
