using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaggeredEnemyState : AbstractEnemyState
{
    private float lastSpeed;
    private static readonly int staggered = Animator.StringToHash("Staggered");
    private static readonly int idle = Animator.StringToHash("Idle");
    private static readonly int thrust = Animator.StringToHash("Thrust");
    private static readonly int prepareThrust = Animator.StringToHash("PrepareThrust");
    private static readonly int prepareSwing = Animator.StringToHash("PrepareSwing");
    private static readonly int swing = Animator.StringToHash("Swing");

    public override void OnEnterState()
    {
        base.OnEnterState();

        lastSpeed = owner.AIPath.maxSpeed;
        owner.Animator.ResetTrigger(idle);
        owner.Animator.ResetTrigger(thrust);
        owner.Animator.ResetTrigger(prepareThrust);
        owner.Animator.ResetTrigger(prepareSwing);
        owner.Animator.ResetTrigger(swing);
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
