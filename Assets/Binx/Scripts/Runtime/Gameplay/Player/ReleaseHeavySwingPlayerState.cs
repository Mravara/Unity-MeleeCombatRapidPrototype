using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseHeavySwingPlayerState : AbstractPlayerState
{
    public override void OnEnterState()
    {
        base.OnEnterState();

        player.Animator.SetTrigger("ReleaseHeavyAttack");
    }
}
