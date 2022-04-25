using Binx.UI;
using EZCameraShake;
using UnityEngine;

namespace Binx
{
    public class Player : MonoBehaviour
    {
        public static Player instance = null;

        [Header("Refs")] 
        [SerializeField] private Animator animator;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private ThirdPersonController thirdPersonController;
        [SerializeField] private Transform cameraFollowTransform;
        [SerializeField] private UIPlayerHealth playerHealth;
        [SerializeField] private CameraShaker cameraShaker;
        public GameObject body;
        public GameObject dodgeBody;
        
        [Header("Health")]
        [SerializeField] private int maxHealth;
        [SerializeField] private int currentHealth;
        
        [Header("Dodge")]
        public float dodgeDuration = 0.3f;
        public float dodgeRecovery = 0.1f;
        public bool isDodging = false;
        public bool blockMovement = false;
        
        public Vector3 Position => transform.position;
        public Vector3 ProjectedMousePosition => projectedMousePosition;
        public int CurrentHealth => currentHealth;
        public int MaxHealth => maxHealth;
        public Animator Animator => animator;
        public ThirdPersonController TPC => thirdPersonController;
        public AbstractPlayerState CurrentState => currentState;
        
        private new Transform transform;
        private bool customMovementActive = false;
        private bool isDead = false;
        private bool hasProjectedMousePosition;
        private Vector3 projectedMousePosition;

        [Header("States")]
        [SerializeField] private AbstractPlayerState currentState;
        [SerializeField] private AbstractPlayerState[] playerStates;

        public void DealDamage(int damage)
        {
            if (isDodging)
                return;
            
            currentHealth -= Mathf.Max(0,damage);
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
        }
        
        private void ResetHealth()
        {
            currentHealth = maxHealth;
        }

        public void Heal(int heal)
        {
            currentHealth += heal;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
        }
        
        private void Awake()
        {
            transform = GetComponent<Transform>();
            currentHealth = maxHealth;
            instance = this;
            playerHealth.SetCamera(playerCamera);
        }
        
        private void Update()
        {
            ProjectMousePosition();

            if (!customMovementActive)
            {
                thirdPersonController.LookAt(ProjectedMousePosition);
                thirdPersonController.Simulate();
            }

            HandleInputs();
            
            currentState.UpdateState();
        }

        private void HandleInputs()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TryHeavySwing();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                TryFastSwing();
            }

            if (Input.GetMouseButtonDown(1))
            {
                TryBlock();
            }
            else if (Input.GetMouseButtonUp(1))
            {
                TryEndBlock();
            }
        }

        private void TryHeavySwing()
        {
            if (currentState.stateType == PlayerStateType.Idle)
            {
                ChangeState(PlayerStateType.StartHeavy);
            }
        }
        
        private void TryFastSwing()
        {
            if (currentState.stateType == PlayerStateType.StartHeavy)
            {
                ChangeState(PlayerStateType.Swing);
            }
            else if (currentState.stateType == PlayerStateType.HoldHeavy)
            {
                ChangeState(PlayerStateType.ReleaseHeavy);
            }
        }

        private void TryBlock()
        {
            if (currentState.stateType == PlayerStateType.Idle)
            {
                ChangeState(PlayerStateType.StartBlock);
            }
        }
        
        private void TryEndBlock()
        {
            if (currentState.stateType == PlayerStateType.StartBlock)
            {
                ChangeState(PlayerStateType.EndBlock);
            }
            if (currentState.stateType == PlayerStateType.HoldBlock)
            {
                ChangeState(PlayerStateType.EndBlock);
            }
        }
        
        private void FixedUpdate()
        {
            if (Position.y < -0.1f)
                transform.position = new Vector3(Position.x, 0f, Position.z);
        }
        
        private void ProjectMousePosition()
        {
            hasProjectedMousePosition = false;

            Ray cameraRay = playerCamera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));

            if (!plane.Raycast(cameraRay, out float distance))
                return;

            hasProjectedMousePosition = true;
            projectedMousePosition = cameraRay.GetPoint(distance);
        }
        
        private void Die()
        {
            if (isDead)
                return;
            
            isDead = true;
            Debug.Log("DEAD!");
        }

        public Vector3 GetStoppingPoint(Vector3 from, float distance)
        {
            float currentDistance = Vector3.Distance(from, Position);
            
            if (distance > currentDistance)
                return from;
            
            return Vector3.MoveTowards(from, Position, currentDistance - distance);
        }

        public bool IsStateReady(PlayerStateType state)
        {
            for (int i = 0; i < playerStates.Length; i++)
            {
                AbstractPlayerState s = playerStates[i];
                if (s.stateType == state)
                {
                    return s.IsReady;
                }
            }

            return false;
        }
        
        public void ChangeState(AbstractPlayerState newState)
        {
            if (currentState)
            {
                currentState.OnExitState();
            }
        
            currentState = newState;
            currentState.OnEnterState();
        }
        
        public void ChangeState(PlayerStateType t)
        {
            for (int i = 0; i < playerStates.Length; i++)
            {
                AbstractPlayerState s = playerStates[i];
                if (s.stateType == t)
                {
                    ChangeState(s);
                    return;
                }
            }
        
            Debug.LogError($"Invalid state {t}!");
        }
        
        public void ShakeCameraSuperWeak()
        {
            if (cameraShaker)
                cameraShaker.ShakeOnce(0.2f, 2f, 0f, 0.2f);
        }
                
        public void ShakeCameraWeak()
        {
            if (cameraShaker)
                cameraShaker.ShakeOnce(0.6f, 4f, 0f, 0.5f);
        }
                
        public void ShakeCameraMedium()
        {
            if (cameraShaker)
                cameraShaker.ShakeOnce(1f, 6f, 0f, 0.7f);
        }
                
        public void ShakeCameraStrong()
        {
            if (cameraShaker)
                cameraShaker.ShakeOnce(2f, 8f, 0f, 1f);
        }
    }
}
