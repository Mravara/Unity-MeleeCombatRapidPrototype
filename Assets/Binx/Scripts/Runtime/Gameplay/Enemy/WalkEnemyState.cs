using System.Collections;
using System.Collections.Generic;
using Binx;
using UnityEngine;

public class WalkEnemyState : AbstractEnemyState
{
    public float minHitDistance = 1f;

    public override void OnEnterState()
    {
        base.OnEnterState();
        
        owner.NavMeshAgent.speed = owner.Speed;
        owner.NavMeshAgent.angularSpeed = 400f;
    }
    
    public override void UpdateState()
    {
        base.UpdateState();

        owner.NavMeshAgent.SetDestination(Player.instance.Position);
        
        if (owner.FieldOfView.PlayerInShortAttackRange())
        {
            owner.NavMeshAgent.stoppingDistance = 3f;
            owner.ChangeState(EnemyStateType.PrepareToSwing);
        }
        else if (owner.FieldOfView.PlayerInLongAttackRange())
        {
            owner.NavMeshAgent.stoppingDistance = 3f;
            owner.ChangeState(EnemyStateType.PrepareToThrust);
        }
        else
        {
            owner.NavMeshAgent.stoppingDistance = 0f;
        }
    }
}
