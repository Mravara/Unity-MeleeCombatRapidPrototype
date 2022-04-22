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
        
        if (owner.FieldOfView.PlayerInAttackRange())
        {
            owner.ChangeState(EnemyStateType.PrepareToSwing);
        }
    }
}
