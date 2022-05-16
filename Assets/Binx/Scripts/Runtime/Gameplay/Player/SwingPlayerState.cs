using System;
using System.Collections;
using System.Collections.Generic;
using Binx;
using UnityEngine;

public class SwingPlayerState : AbstractPlayerState
{
    public static int numberOfSwings = 1;
    private AbstractPlayerState defaultNextState;
    private int currentFrame = 0;
    private int maxFrames = 10;
    [SerializeField]
    private Vector3 boxCenter = new Vector3(0f, 1f, 2f);
    [SerializeField]
    private Vector3 boxSize = new Vector3(4f, 2f, 3f);
    [SerializeField] private LayerMask layerMask;
    
    private float speedWhenSwinging = 10f;
    private static readonly int combo = Animator.StringToHash("Combo");

    private bool swingAgain = false;

    // private HashSet<Collider> ignoredColliders = new HashSet<Collider>();

    public override void OnEnterState()
    {
        base.OnEnterState();

        currentFrame = 0;
        player.TPC.Stop();
        player.swordCollider.enabled = true;
        defaultNextState = nextState;
        player.Animator.applyRootMotion = true;
        Player.customMovementActive = true;
        swingAgain = false;
    }
    
    public override void OnExitState()
    {
        base.OnExitState();
            
        player.swordCollider.enabled = false;
        nextState = defaultNextState;
        // ignoredColliders.Clear();
        Player.customMovementActive = false;
        if (nextState == defaultNextState)
        {
            numberOfSwings = 1;
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (!isActive)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            player.Animator.SetTrigger(combo);
            nextState = player.GetState(PlayerStateType.Swing);
            numberOfSwings++;
            swingAgain = true;
        }
        
        player.TPC.UpdateRotation();

        // if (++currentFrame < maxFrames)
        // {
        //     player.TPC.ManualMove(player.transform.forward, speedWhenSwinging);
        // }

        // Collider[] colliders = Physics.OverlapBox(player.transform.position + player.transform.TransformDirection(boxCenter), boxSize / 2f, player.transform.rotation, layerMask);
        // for (int i = 0; i < colliders.Length; i++)
        // {
        //     Collider c = colliders[i];
        //     
        //     if (!ignoredColliders.Add(c))
        //         continue;
        //     
        //     c.TryGetComponent(out SwordmanEnemy enemy);
        //     enemy.DealDamage(player.DamageWithModifier);
        // }
    }
}
