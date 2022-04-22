using System.Collections;
using System.Collections.Generic;
using Binx;
using UnityEngine;

public class WalkEnemyState : AbstractEnemyState
{
    public float minHitDistance = 1f;
    
    public override void UpdateState()
    {
        base.UpdateState();

        owner.NavMeshAgent.speed = owner.Speed;
        owner.NavMeshAgent.acceleration = owner.Acceleration;
        owner.NavMeshAgent.angularSpeed = 200f;
        owner.NavMeshAgent.SetDestination(Player.instance.Position);
        
        float distance = Vector3.Distance(Player.instance.Position, transform.position);
        if (distance < minHitDistance)
        {
            owner.ChangeState(EnemyStateType.PrepareToSwing);
        }
    }
}
