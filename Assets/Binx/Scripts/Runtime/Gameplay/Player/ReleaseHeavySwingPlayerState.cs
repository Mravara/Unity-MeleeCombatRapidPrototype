using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseHeavySwingPlayerState : AbstractPlayerState
{
    private static readonly int attack = Animator.StringToHash("Attack");

    public override void OnEnterState()
    {
        base.OnEnterState();

        player.Animator.SetBool(attack, false);
    }
}
