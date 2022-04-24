using System.Collections;
using System.Collections.Generic;
using Binx;
using UnityEngine;

public class ThrustEnemyState : AbstractEnemyState
{
    public override void OnEnterState()
    {
        base.OnEnterState();

        // owner.NavMeshAgent.speed = 20f;
        // owner.NavMeshAgent.acceleration = 200f;
        owner.SetSpeed(40f);
        owner.AIPath.maxAcceleration = 2000f;
        owner.Animator.SetTrigger("Thrust");
        owner.SwordCollider.enabled = true;
    }
    
    public override void UpdateState()
    {
        base.UpdateState();

        // owner.NavMeshAgent.SetDestination(Player.instance.Position);
    }
}
