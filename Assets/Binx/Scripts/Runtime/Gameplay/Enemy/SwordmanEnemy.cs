using Binx;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

public class SwordmanEnemy : MonoBehaviour
{
    [Header("References")]
    //[SerializeField] private new Renderer renderer;
    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] private ColliderLink swordColliderLink;
    [SerializeField] private Animator animator;
    // [SerializeField] private NavMeshAgent navmeshAgent;
    [SerializeField] private new Collider collider;
    [SerializeField] private Collider swordCollider;
    [SerializeField] private AIPath aiPath;
    [SerializeField] private AIDestinationSetter aiDestinationSetter;
    
    [Header("Damage")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float initialWalkingSpeed = 5f;
    [SerializeField] private float accelerationValue = 8f;
    [SerializeField] private int damage;
    [SerializeField] private int maxHealth;
    
    // states, mozda ce trebat
    [Header("States")]
    [SerializeField] private AbstractEnemyState currentState;
    [SerializeField] private IdleEnemyState idleEnemyState;
    [SerializeField] private WalkEnemyState walkEnemyState;
    [SerializeField] private PrepareToSwingEnemyState prepareToSwingEnemyState;
    [SerializeField] private HoldSwingEnemyState holdSwingEnemyState;
    [SerializeField] private SwingEnemyState swingEnemyState;

    [SerializeField] private PrepareToThrustEnemyState prepareToThrustEnemyState;
    [SerializeField] private HoldPrepareToThrustEnemyState holdPrepareToThrustEnemyState;
    [SerializeField] private HoldThrustEnemyState holdThrustEnemyState;
    [SerializeField] private ThrustEnemyState thrustEnemyState;
    
    [SerializeField] private RecoverFromSwingEnemyState recoverFromSwingEnemyState;
    [SerializeField] private StaggeredEnemyState staggeredEnemyState;
    [SerializeField] private AbstractEnemyState[] enemyStates;
    
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
    
    private void Start()
    {
        swordColliderLink.OnTriggerEnterEvent += OnSwordTriggerEnter;
        
        // navmeshAgent.speed = Speed;
        // navmeshAgent.acceleration = accelerationValue;
        currentHealth = MaxHealth;

        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();

        aiDestinationSetter.target = Player.instance.transform;

        foreach (AbstractEnemyState s in enemyStates)
        {
            s.owner = this;
        }

        ChangeState(idleEnemyState);
    }
    
    private void OnSwordTriggerEnter(Collider obj)
    {
        if (obj.gameObject.layer == Player.instance.gameObject.layer)
        {
            Player.instance.DealDamage(damage);
        }
    }

    private void Update()
    {
        currentState.UpdateState();
    }
    
    public void ChangeState(EnemyStateType t)
    {
        for (int i = 0; i < enemyStates.Length; i++)
        {
            AbstractEnemyState s = enemyStates[i];
            if (s.stateType == t)
            {
                Debug.Log($"Changing state to {t}");
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
}
