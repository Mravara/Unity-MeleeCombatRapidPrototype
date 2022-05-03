using UnityEngine;

public class IdleEnemyState : AbstractEnemyState
{
    private static readonly int idle = Animator.StringToHash("Idle");

    public override void OnEnterState()
    {
        base.OnEnterState();
        
        owner.Animator.SetBool(idle, true);
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
    }
}