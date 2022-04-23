using System.Collections;
using System.Collections.Generic;
using Binx;
using UnityEngine;

public class HoldPrepareToThrustEnemyState : AbstractEnemyState
{
    public override void OnEnterState()
    {
        base.OnEnterState();

        owner.NavMeshAgent.speed = 1f;
        owner.NavMeshAgent.angularSpeed = 200f;
    }
    
    public override void UpdateState()
    {
        base.UpdateState();

        owner.NavMeshAgent.SetDestination(Player.instance.Position);
    }
}
