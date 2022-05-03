using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaggeredEnemyState : AbstractEnemyState
{
    private float lastSpeed;
    private static readonly int staggered = Animator.StringToHash("Staggered");
    private static readonly int startStaggered = Animator.StringToHash("StartStaggered");

    public override void OnEnterState()
    {
        base.OnEnterState();

        lastSpeed = owner.AIPath.maxSpeed;
        owner.Animator.SetTrigger(startStaggered);
        owner.Animator.SetBool(staggered, true);
        
        owner.SetSpeed(0);
    }
    
    public override void OnExitState()
    {
        base.OnExitState();

        owner.Animator.SetBool(staggered, false);
        owner.SetSpeed(lastSpeed);
    }
}
