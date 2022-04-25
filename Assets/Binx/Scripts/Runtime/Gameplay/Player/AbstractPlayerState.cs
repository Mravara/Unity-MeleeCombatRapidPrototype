using System.Collections;
using System.Collections.Generic;
using Binx;
using UnityEngine;

public abstract class AbstractPlayerState : MonoBehaviour
{
    public Player player;
    
    public PlayerStateType stateType;

    public float currentStateDuration;
    public float minStateDuration;
    public float maxStateDuration;
    public float cooldown;
    public AbstractPlayerState nextState;
    public bool isActive = false;

    protected float nextReadyTime = 0f;

    public bool IsReady => Time.time >= nextReadyTime;

    public virtual void UpdateState()
    {
        currentStateDuration += Time.deltaTime;

        if (maxStateDuration > 0f)
        {
            if (nextState && currentStateDuration >= maxStateDuration)
            {
                if (nextState.IsReady)
                    player.ChangeState(nextState.stateType);
            }
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
