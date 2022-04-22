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
        
        float distance = Vector3.Distance(Player.instance.Position, transform.position);
        if (distance < detectPlayerDistance)
        {
            owner.ChangeState(EnemyStateType.Walk);
        }
    }
}
