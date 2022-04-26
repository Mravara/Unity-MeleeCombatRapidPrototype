using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBlockPlayerState : AbstractPlayerState
{
    private static readonly int block = Animator.StringToHash("Block");

    public override void OnEnterState()
    {
        base.OnEnterState();
        
        player.Animator.SetBool(block, false);
    }
}
