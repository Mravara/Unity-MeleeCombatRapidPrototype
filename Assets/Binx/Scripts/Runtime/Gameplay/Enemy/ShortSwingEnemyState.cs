using UnityEngine;

public class ShortSwingEnemyState : AbstractEnemyState
{
    private static readonly int shortSwing = Animator.StringToHash("ShortSwing");

    public override void OnEnterState()
    {
        base.OnEnterState();

        // owner.NavMeshAgent.speed = 8f;
        // owner.NavMeshAgent.acceleration = 100f;
        owner.SetSpeed(3f);
        owner.Animator.SetTrigger(shortSwing);
        owner.SwordCollider.enabled = true;
    }
    
    public override void OnExitState()
    {
        base.OnExitState();
        
        owner.SwordCollider.enabled = false;
    }
}