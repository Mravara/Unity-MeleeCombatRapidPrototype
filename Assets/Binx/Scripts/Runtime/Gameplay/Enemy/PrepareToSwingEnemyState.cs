using System.Collections;
using System.Collections.Generic;
using Binx;
using UnityEngine;

public class PrepareToSwingEnemyState : AbstractEnemyState
{
    public override void OnEnterState()
    {
        base.OnEnterState();

        owner.NavMeshAgent.speed = 2f;
        owner.NavMeshAgent.angularSpeed = 50f;
        owner.Animator.SetTrigger("PrepareSwing");
    }
    
    public override void UpdateState()
    {
        base.UpdateState();

        owner.NavMeshAgent.SetDestination(Player.instance.Position);
    }
}
