using Binx;
using UnityEngine;

public class ShortSwingEnemyState : AbstractEnemyState
{
    private bool frozen = false;
    private Vector3 position;
    private static readonly int shortSwing = Animator.StringToHash("ShortSwing");

    public override void OnEnterState()
    {
        base.OnEnterState();

        // owner.NavMeshAgent.speed = 8f;
        // owner.NavMeshAgent.acceleration = 100f;
        owner.SetSpeed(3f);
        owner.Animator.SetTrigger(shortSwing);
        owner.SwordCollider.enabled = true;
        frozen = false;
    }
    
    public override void OnExitState()
    {
        base.OnExitState();
        
        owner.SwordCollider.enabled = false;
    }
    
    public override void LateUpdateState()
    {
        base.LateUpdateState();

        float minDistance = 2f;
        if (frozen || Vector3.Distance(transform.position, Player.instance.Position) < minDistance && owner.FieldOfView.PlayerInSight())
        {
            FreezePosition();
        }
        
        if (currentStateDuration < 1.5f)
        {
            owner.SetSpeed(1f);
        }
        else
        {
            owner.SetSpeed(0f);
        }
    }
    
    private void FreezePosition()
    {
        if (!frozen)
        {
            position = owner.transform.position;
            frozen = true;
        }

        if (frozen)
        {
            owner.transform.position = position;
            owner.AIPath.Teleport(position);
        }
    }
}