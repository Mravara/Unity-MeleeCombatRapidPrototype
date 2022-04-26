using System.Collections;
using System.Collections.Generic;
using Binx;
using UnityEngine;

public class HoldReleaseHeavySwingPlayerState : AbstractPlayerState
{
    private float lastSpeed;
    
    public override void OnEnterState()
    {
        base.OnEnterState();

        player.blockMovement = true;
        Player.instance.ShakeCameraStrong();
    }
    
    public override void OnExitState()
    {
        base.OnExitState();

        player.blockMovement = false;
    }
}
