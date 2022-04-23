using System.Collections;
using System.Collections.Generic;
using Binx;
using UnityEngine;

public class HoldSwingEnemyState : AbstractEnemyState
{
    public override void OnEnterState()
    {
        base.OnEnterState();

        // owner.NavMeshAgent.speed = 1f;
        owner.SetSpeed(1f);
    }
    
    public override void UpdateState()
    {
        // owner.NavMeshAgent.SetDestination(Player.instance.GetStoppingPoint(transform.position, 2f));
        
        currentStateDuration += Time.deltaTime;

        if (nextState && currentStateDuration >= maxStateDuration)
        {
            if (!owner.FieldOfView.PlayerInRange())
            {
                owner.ChangeState(EnemyStateType.Idle);
                owner.Animator.SetTrigger("Idle");
            }
            // else if (owner.FieldOfView.PlayerInShortAttackRange())
            // {
            //     owner.ChangeState(nextState.stateType);
            // }
            // else
            // {
            //     owner.ChangeState(EnemyStateType.Walk);
            //     owner.Animator.SetTrigger("Idle");
            // }
            else
            {
                owner.ChangeState(nextState.stateType);
            }
        }
    }
}
