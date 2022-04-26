using UnityEngine;

public class IdleEnemyState : AbstractEnemyState
{
    [SerializeField] private float detectPlayerDistance = 10f;
    private static readonly int idle = Animator.StringToHash("Idle");

    public override void OnEnterState()
    {
        base.OnEnterState();
        
        owner.Animator.SetBool(idle, true);
    }
    
    public override void OnExitState()
    {
        base.OnExitState();

        owner.Animator.SetBool(idle, false);
    }
    
    public override void UpdateState()
    {
        base.UpdateState();
        
        if (owner.FieldOfView.PlayerInSight())
        {
            owner.ChangeState(EnemyStateType.Walk);
        }
    }
}