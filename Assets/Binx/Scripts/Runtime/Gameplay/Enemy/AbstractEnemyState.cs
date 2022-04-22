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
    public float currentCooldown;
    public float maxCooldown;
    public AbstractEnemyState nextState;

    public bool IsReady => currentCooldown >= maxCooldown;
    public bool isActive = false;

    public virtual void UpdateState()
    {
        if (!isActive)
        {
            if (maxCooldown > 0f)
            {
                currentCooldown += Time.deltaTime;
                
            }
            return;
        }
        
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
    }
}
