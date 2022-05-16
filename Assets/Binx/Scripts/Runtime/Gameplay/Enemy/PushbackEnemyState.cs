using Binx;
using UnityEngine;

public class PushbackEnemyState : AbstractEnemyState
{
    private int currentFrame = 0;
    private int maxFrames = 15;
    private Vector3 pushbackDirection;
    
    private float lastSpeed;
    private static readonly int staggered = Animator.StringToHash("Staggered");
    private static readonly int startStaggered = Animator.StringToHash("StartStaggered");
    
    public override void OnEnterState()
    {
        base.OnEnterState();

        currentFrame = 0;
        pushbackDirection = Player.instance.transform.forward;
        
        lastSpeed = owner.AIPath.maxSpeed;
        owner.Animator.SetTrigger(startStaggered);
        owner.Animator.SetBool(staggered, true);
        
        owner.SetSpeed(0);
    }
    
    public override void OnExitState()
    {
        base.OnExitState();

        owner.AIPath.Move(Vector3.zero);
        owner.AIPath.FinalizeMovement(owner.AIPath.position, owner.AIPath.rotation);
        
        owner.Animator.SetBool(staggered, false);
        owner.SetSpeed(lastSpeed);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (++currentFrame < maxFrames)
        {
            owner.AIPath.Move(pushbackDirection * 30f * Time.deltaTime);
            owner.AIPath.FinalizeMovement(owner.AIPath.position, owner.AIPath.rotation);
        }
    }
}
