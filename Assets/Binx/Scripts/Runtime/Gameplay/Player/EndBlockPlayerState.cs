using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBlockPlayerState : AbstractPlayerState
{
    public override void OnEnterState()
    {
        base.OnEnterState();
        
        player.Animator.SetBool("Block", false);
    }
}
