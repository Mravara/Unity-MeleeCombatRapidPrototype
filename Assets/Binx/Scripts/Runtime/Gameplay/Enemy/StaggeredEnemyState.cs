using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaggeredEnemyState : AbstractEnemyState
{
    public override void OnEnterState()
    {
        base.OnEnterState();

        owner.Animator.SetBool("Staggered", true);
    }
    
    public override void OnExitState()
    {
        base.OnExitState();

        owner.Animator.SetBool("Staggered", false);
    }
}
