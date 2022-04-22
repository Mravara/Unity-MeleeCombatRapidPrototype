using System.Collections;
using System.Collections.Generic;
using Binx;
using UnityEngine;

public class IdleEnemyState : AbstractEnemyState
{
    [SerializeField] private float detectPlayerDistance = 10f;
    
    public override void UpdateState()
    {
        base.UpdateState();
        
        if (owner.FieldOfView.PlayerInSight())
        {
            owner.ChangeState(EnemyStateType.Walk);
        }
    }
}
