using UnityEngine;

public class IdleEnemyState : AbstractEnemyState
{
    private static readonly int idle = Animator.StringToHash("Idle");

    private float nextLookAroundTime;
    private static readonly int lookAround = Animator.StringToHash("LookAround");

    public override void OnEnterState()
    {
        base.OnEnterState();
        
        owner.Animator.SetBool(idle, true);
        owner.SetSpeed(0f);
        currentStateDuration = 5f;
    }
    
    public override void OnExitState()
    {
        base.OnExitState();
        
    }
    
    public override void UpdateState()
    {
        base.UpdateState();
        
        if (owner.FieldOfView.PlayerInSight())
        {
            owner.ChangeState(EnemyStateType.Alert);
        }

        if (currentStateDuration > nextLookAroundTime)
        {
            LookAround();
        }
    }

    private void LookAround()
    {
        nextLookAroundTime = currentStateDuration + 5f;
        owner.Animator.SetTrigger(lookAround);
    }
}