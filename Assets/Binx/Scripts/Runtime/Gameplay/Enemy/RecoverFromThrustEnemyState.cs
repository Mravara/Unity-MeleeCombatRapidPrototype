using System.Collections;
using System.Collections.Generic;
using Binx;
using UnityEngine;

public class RecoverFromThrustEnemyState : AbstractEnemyState
{
    public override void OnEnterState()
    {
        base.OnEnterState();

        // owner.NavMeshAgent.speed = 1f;
        owner.SetSpeed(1f);
        owner.Animator.SetTrigger("RecoverFromThrust");
        owner.SwordCollider.enabled = false;
        owner.AIPath.maxAcceleration = -2.5f;
    }
    
    public override void UpdateState()
    {
        base.UpdateState();

        // owner.NavMeshAgent.SetDestination(Player.instance.GetStoppingPoint(transform.position, 2f));
    }
}
