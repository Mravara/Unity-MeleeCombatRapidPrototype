using System.Collections;
using System.Collections.Generic;
using Binx;
using UnityEngine;

public class HoldReleaseHeavySwingPlayerState : AbstractPlayerState
{
    [SerializeField]
    private Vector3 boxCenter = new Vector3(0f, 1f, 2f);
    [SerializeField]
    private Vector3 boxSize = new Vector3(4f, 2f, 3f);
    [SerializeField] private LayerMask layerMask;
    
    private HashSet<Collider> ignoredColliders = new HashSet<Collider>();
    
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
        ignoredColliders.Clear();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (!isActive)
            return;

        Collider[] colliders = Physics.OverlapBox(player.transform.position + player.transform.TransformDirection(boxCenter), boxSize / 2f, player.transform.rotation, layerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider c = colliders[i];

            if (!ignoredColliders.Add(c))
                continue;

            c.TryGetComponent(out SwordmanEnemy enemy);
            enemy.DealDamage(player.HeavyDamage);
        }
    }
}
