using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemyState : MonoBehaviour
{
    public EnemyStateType stateType;
    
    [HideInInspector]
    public SwordmanEnemy owner;
    
    public float currentStateDuration;
    public float minStateDuration;
    public float maxStateDuration;
    public float cooldown;
    public AbstractEnemyState nextState;
    public bool isActive = false;

    protected float nextReadyTime = 0f;

    public bool IsReady => Time.time >= nextReadyTime;

    public virtual void UpdateState()
    {
        currentStateDuration += Time.deltaTime;
        
        if (nextState && currentStateDuration >= maxStateDuration)
        {
            if (nextState.IsReady)
                owner.ChangeState(nextState.stateType);
        }
    }

    public virtual void OnEnterState()
    {
        currentStateDuration = 0f;
        isActive = true;
    }

    public virtual void OnExitState()
    {
        isActive = false;
        nextReadyTime = Time.time + cooldown;
    }
}
