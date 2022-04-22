using System.Collections;
using System.Collections.Generic;
using Binx;
using UnityEngine;

public class HoldSwingEnemyState : AbstractEnemyState
{
    public override void OnEnterState()
    {
        base.OnEnterState();

        owner.NavMeshAgent.speed = 1f;
        owner.NavMeshAgent.angularSpeed = 20f;
    }
    
    public override void UpdateState()
    {
        if (!isActive)
        {
            if (maxCooldown > 0f)
            {
                currentCooldown += Time.deltaTime;
                
            }
            return;
        }
        
        owner.NavMeshAgent.SetDestination(Player.instance.Position);
        
        currentStateDuration += Time.deltaTime;

        if (nextState && currentStateDuration >= maxStateDuration)
        {
            float distance = Vector3.Distance(Player.instance.Position, transform.position);
            if (distance < 5f)
            {
                if (nextState.IsReady)
                    owner.ChangeState(nextState.stateType);
            }
            else
            {
                owner.ChangeState(EnemyStateType.Walk);
                owner.Animator.SetTrigger("Idle");
            }
        }
    }
}
