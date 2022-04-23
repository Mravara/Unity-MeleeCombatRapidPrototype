using Binx;

public class PrepareToSwingEnemyState : AbstractEnemyState
{
    public override void OnEnterState()
    {
        base.OnEnterState();

        // owner.NavMeshAgent.speed = 3f;
        owner.SetSpeed(3f);
        owner.Animator.SetTrigger("PrepareSwing");
    }
    
    public override void UpdateState()
    {
        base.UpdateState();

        // owner.NavMeshAgent.SetDestination(Player.instance.GetStoppingPoint(transform.position, 3f));
        
    }
}
