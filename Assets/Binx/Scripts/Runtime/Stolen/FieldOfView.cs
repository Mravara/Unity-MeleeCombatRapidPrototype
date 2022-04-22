using Binx;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float sightRadius = 10f;
    public float attackRadius = 5f;
    
    [Range(0,360)]
    public float angle;

    public bool PlayerInSight()
    {
        float distanceToTarget = Vector3.Distance(transform.position, Player.instance.Position);
        
        if (distanceToTarget > sightRadius)
            return false;
        
        Vector3 directionToTarget = (Player.instance.Position - transform.position).normalized;

        return (Vector3.Angle(transform.forward, directionToTarget) < angle / 2f);
    }
    
    public bool PlayerInAttackRange()
    {
        float distanceToTarget = Vector3.Distance(transform.position, Player.instance.Position);
        
        if (distanceToTarget > attackRadius)
            return false;
        
        Vector3 directionToTarget = (Player.instance.Position - transform.position).normalized;

        return (Vector3.Angle(transform.forward, directionToTarget) < angle / 2f);
    }

    public bool PlayerInRange()
    {
        return  Vector3.Distance(transform.position, Player.instance.Position) < sightRadius;
    }
}
