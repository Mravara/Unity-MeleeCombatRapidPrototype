using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingEnemyState : AbstractEnemyState
{
    public override void OnEnterState()
    {
        base.OnEnterState();

        owner.NavMeshAgent.speed = 8f;
        owner.NavMeshAgent.acceleration = 100f;
        owner.NavMeshAgent.angularSpeed = 20f;
        owner.Animator.SetTrigger("Swing");
        owner.SwordCollider.enabled = true;
    }

    public override void OnExitState()
    {
        base.OnExitState();
        
        owner.SwordCollider.enabled = false;
    }
}
