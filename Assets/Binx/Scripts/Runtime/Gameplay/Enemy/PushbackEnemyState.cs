using Binx;
using UnityEngine;

public class PushbackEnemyState : AbstractEnemyState
{
    private int currentFrame = 0;
    private int maxFrames = 5;
    private Vector3 pushbackDirection;
    
    public override void OnEnterState()
    {
        base.OnEnterState();

        currentFrame = 0;
        pushbackDirection = Player.instance.transform.forward;
    }
    
    public override void OnExitState()
    {
        base.OnExitState();

        // tu usporiti na 0
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (++currentFrame < maxFrames)
        {
            owner.AIPath.Move(pushbackDirection * owner.Speed * 10f * Time.deltaTime);
            owner.AIPath.FinalizeMovement(owner.AIPath.position, owner.AIPath.rotation);
        }
    }
}
