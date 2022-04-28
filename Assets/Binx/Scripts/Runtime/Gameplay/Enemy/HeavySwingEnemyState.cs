using UnityEngine;

public class HeavySwingEnemyState : AbstractEnemyState
{
    private static readonly int heavySwing = Animator.StringToHash("HeavySwing");

    public override void OnEnterState()
    {
        base.OnEnterState();

        // owner.NavMeshAgent.speed = 8f;
        // owner.NavMeshAgent.acceleration = 100f;
        owner.SetSpeed(3f);
        owner.Animator.SetTrigger(heavySwing);
        owner.SwordCollider.enabled = true;
    }
    
    public override void OnExitState()
    {
        base.OnExitState();
        
        owner.SwordCollider.enabled = false;
    }
}
