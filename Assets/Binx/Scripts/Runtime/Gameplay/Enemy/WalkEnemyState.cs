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

        if (minStateDuration > 0f && currentStateDuration < minStateDuration)
            return;
        
        if (owner.FieldOfView.PlayerInShortAttackRange())
        {
            float rnd = Random.Range(0f, 1f);
            if (rnd <= 0.33f)
            {
                if (owner.IsStateReady(EnemyStateType.ShortSwing))
                    owner.ChangeState(EnemyStateType.ShortSwing);
            }
            else if (rnd <= 0.66f)
            {
                if (owner.IsStateReady(EnemyStateType.HeavySwing))
                    owner.ChangeState(EnemyStateType.HeavySwing);
            }
            else
            {
                if (owner.IsStateReady(EnemyStateType.Thrust))
                    owner.ChangeState(EnemyStateType.Thrust);
            }
        }
        else if (owner.FieldOfView.PlayerInLongAttackRange())
        {
            // if (owner.IsStateReady(EnemyStateType.PrepareToThrust))
            //     owner.ChangeState(EnemyStateType.PrepareToThrust);
        }
        else if (owner.FieldOfView.PlayerInRange())
        {
            
        }
    }
}