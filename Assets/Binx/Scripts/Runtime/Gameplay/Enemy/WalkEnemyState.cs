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
        
        // owner.NavMeshAgent.speed = owner.Speed;
        owner.SetSpeed(owner.Speed);
    }
    
    public override void UpdateState()
    {
        base.UpdateState();
        
        if (owner.FieldOfView.PlayerInShortAttackRange())
        {
            owner.ChangeState(EnemyStateType.PrepareToSwing);
        }
        else if (owner.FieldOfView.PlayerInLongAttackRange())
        {
            owner.ChangeState(EnemyStateType.PrepareToThrust);
        }
        else if (owner.FieldOfView.PlayerInRange())
        {
            // owner.NavMeshAgent.SetDestination(Player.instance.GetStoppingPoint(transform.position, 2f));
        }
    }
}
