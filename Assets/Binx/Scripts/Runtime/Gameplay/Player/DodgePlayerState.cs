using System.Collections;
using System.Collections.Generic;
using Binx;
using UnityEngine;

public class DodgePlayerState : AbstractPlayerState
{
    private float dodgeSpeed = 15f;
    private float recoverySpeed = 3f;
    private Vector3 forwardDirection;
    
    public override void OnEnterState()
    {
        base.OnEnterState();

        player.isDodging = true;
        player.blockMovement = true;
        
        player.body.SetActive(false);
        player.dodgeBody.SetActive(true);
        
        Vector3 targetDirection = Quaternion.Euler(0.0f, player.TPC.TargetRotation, 0.0f) * Vector3.forward;

        float duration = 0f;
        float dodgeDuration = 0.3f;
        
        forwardDirection = player.transform.forward;
			
        if (player.TPC.Input.move.magnitude > 0f)
            forwardDirection = targetDirection.normalized;
    }
    
    public override void OnExitState()
    {
        base.OnExitState();

        player.isDodging = false;
        player.blockMovement = false;
        
        player.body.SetActive(true);
        player.dodgeBody.SetActive(false);
    }
    
    public override void UpdateState()
    {
        base.UpdateState();
        
        if (!isActive)
            return;
        
        if (currentStateDuration < 0.3f)
            player.TPC.ManualMove(forwardDirection, dodgeSpeed);
        else
        {
            player.body.SetActive(true);
            player.dodgeBody.SetActive(false);
            player.TPC.UpdateRotation();
            player.TPC.ManualMove(forwardDirection, recoverySpeed);
        }
    }
}
