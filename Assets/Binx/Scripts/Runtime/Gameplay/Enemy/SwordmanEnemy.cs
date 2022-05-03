using Binx;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

public class SwordmanEnemy : AbstractEnemy
{
    [Header("References")]
    //[SerializeField] private new Renderer renderer;
    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] private ColliderLink swordColliderLink;
    [SerializeField] private Animator animator;
    // [SerializeField] private NavMeshAgent navmeshAgent;
    [SerializeField] private Collider swordCollider;
    [SerializeField] private AIPath aiPath;
    [SerializeField] private AIDestinationSetter aiDestinationSetter;
    
    [Header("Damage")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float initialWalkingSpeed = 5f;
    [SerializeField] private float accelerationValue = 8f;
    [SerializeField] private float damage;
    [SerializeField] private float maxHealth;
    
    // states, mozda ce trebat
    [Header("States")]
    [SerializeField] private AbstractEnemyState currentState;

    [SerializeField] private PrepareToThrustEnemyState prepareToThrustEnemyState;
    [SerializeField] private HoldPrepareToThrustEnemyState holdPrepareToThrustEnemyState;
    [SerializeField] private HoldThrustEnemyState holdThrustEnemyState;
    [SerializeField] private ThrustEnemyState thrustEnemyState;
    
    [SerializeField] private RecoverFromSwingEnemyState recoverFromSwingEnemyState;
    [SerializeField] private StaggeredEnemyState staggeredEnemyState;
    [SerializeField] private AbstractEnemyState[] enemyStates;

    [SerializeField] private CharacterController characterController;

    private Transform t;
    private new Renderer renderer;
    private float speedMultiplier = 1f;
    private float maxHealthMultiplier = 1f;
    private float sizeMultiplier = 1f;
    private float currentHealth;
    private bool isDead;
    
    public float Speed => speed * speedMultiplier;
    public float Acceleration => accelerationValue;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth * maxHealthMultiplier;
    public Animator Animator => animator;
    // public NavMeshAgent NavMeshAgent => navmeshAgent;
    public Collider SwordCollider => swordCollider;
    public FieldOfView FieldOfView => fieldOfView;
    public AIPath AIPath => aiPath;
    public CharacterController CharacterController => characterController;
    
    private void Start()
    {
        swordColliderLink.OnTriggerEnterEvent += OnSwordTriggerEnter;
        
        // navmeshAgent.speed = Speed;
        // navmeshAgent.acceleration = accelerationValue;
        currentHealth = MaxHealth;

        renderer = GetComponent<Renderer>();

        aiDestinationSetter.target = Player.instance.transform;

        foreach (AbstractEnemyState s in enemyStates)
        {
            s.owner = this;
        }

        ChangeState(EnemyStateType.Idle);
    }
    
    private void OnSwordTriggerEnter(Collider obj)
    {
        if (obj.gameObject.layer == Player.instance.gameObject.layer)
        {
            Player.instance.DealDamage(this, damage);
        }
    }

    private void Update()
    {
        currentState.UpdateState();
    }
    
    private void LateUpdate()
    {
        currentState.LateUpdateState();
    }
    
    public void ChangeState(EnemyStateType t)
    {
        for (int i = 0; i < enemyStates.Length; i++)
        {
            AbstractEnemyState s = enemyStates[i];
            if (s.stateType == t)
            {
                ChangeState(s);
                return;
            }
        }
        
        Debug.LogError($"Invalid state {t}!");
    }

    public void ChangeState(AbstractEnemyState newState)
    {
        if (currentState)
        {
            currentState.OnExitState();
        }
        
        currentState = newState;
        currentState.OnEnterState();
    }
    
    public void DealDamage(float damage)
    {
        currentHealth -= Mathf.Max(0, damage);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            ChangeState(EnemyStateType.Pushback);
        }
    }
    
    private void Die()
    {
        if (isDead)
            return;
            
        isDead = true;
        Debug.Log("ENEMY DEAD!");
    }

    public void SwingStarted()
    {
        
    }

    public void SwingEnded()
    {
        
    }

    public void StartDamage()
    {
        
    }

    public void EndDamage()
    {
        
    }

    public void SetSpeed(float speed)
    {
        aiPath.maxSpeed = speed;
    }

    public bool IsStateReady(EnemyStateType state)
    {
        for (int i = 0; i < enemyStates.Length; i++)
        {
            AbstractEnemyState s = enemyStates[i];
            if (s.stateType == state)
            {
                return s.IsReady;
            }
        }

        return false;
    }

    public override void Parried()
    {
        base.Parried();
        
        ChangeState(EnemyStateType.Staggered);
    }
}