using System.Collections;
using System.Collections.Generic;
using Binx;
using UnityEngine;

public class HoldThrustEnemyState : AbstractEnemyState
{
    public override void OnEnterState()
    {
        base.OnEnterState();

        // owner.NavMeshAgent.speed = 1f;
        // owner.NavMeshAgent.acceleration = 50f;
        owner.SetSpeed(1f);
    }
    
    public override void UpdateState()
    {
        base.UpdateState();

        // owner.NavMeshAgent.SetDestination(Player.instance.GetStoppingPoint(transform.position, 3f));
    }
}
