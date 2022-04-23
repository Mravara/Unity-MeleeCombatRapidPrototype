using System.Collections;
using System.Collections.Generic;
using Binx;
using UnityEngine;

public class RecoverFromThrustEnemyState : AbstractEnemyState
{
    public override void OnEnterState()
    {
        base.OnEnterState();

        owner.NavMeshAgent.speed = 2f;
        owner.NavMeshAgent.angularSpeed = 50f;
        owner.Animator.SetTrigger("RecoverFromThrust");
        owner.SwordCollider.enabled = false;
    }
}
