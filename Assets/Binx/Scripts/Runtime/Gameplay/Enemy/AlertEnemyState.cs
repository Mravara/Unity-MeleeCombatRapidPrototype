using UnityEngine;

public class AlertEnemyState : AbstractEnemyState
{
    private static readonly int idle = Animator.StringToHash("Idle");

    public override void OnEnterState()
    {
        base.OnEnterState();
        
        owner.Animator.SetBool(idle, false);
    }
    
    public override void OnExitState()
    {
        base.OnExitState();

        
    }
    
    public override void UpdateState()
    {
        base.UpdateState();
        
        if (owner.FieldOfView.PlayerInRange())
        {
            owner.ChangeState(EnemyStateType.Walk);
        }
    }
}