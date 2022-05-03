using Binx;
using UnityEngine;

public class HeavySwingEnemyState : AbstractEnemyState
{
    private bool frozen = false;
    private Vector3 position;
    private static readonly int heavySwing = Animator.StringToHash("HeavySwing");

    public override void OnEnterState()
    {
        base.OnEnterState();

        // owner.NavMeshAgent.speed = 8f;
        // owner.NavMeshAgent.acceleration = 100f;
        frozen = false;
        owner.SetSpeed(0f);
        owner.Animator.SetTrigger(heavySwing);
        owner.SwordCollider.enabled = true;
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
        if (Vector3.Distance(transform.position, Player.instance.Position) < minDistance && owner.FieldOfView.PlayerInSight())
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
