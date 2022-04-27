using System;
using Binx.UI;
using EZCameraShake;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
        [SerializeField] private Image staminaImage;
        public GameObject body;
        public GameObject dodgeBody;
        
        [Header("Health")]
        [SerializeField] private float maxHealth;
        [SerializeField] private float currentHealth;
        
        [Header("Dodge")]
        public float dodgeDuration = 0.3f;
        public float dodgeRecovery = 0.1f;
        public bool isDodging = false;
        public bool blockMovement = false;

        [Header("Stamina")] 
        public float currentStamina = 0f;
        public float maxStamina = 100f;
        private float staminaUpdateTime;
        private float staminaUpdateCooldown = 1f;
        private float staminaUpdateRate = 50f;
        
        public Vector3 Position => transform.position;
        public Vector3 ProjectedMousePosition => projectedMousePosition;
        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public Animator Animator => animator;
        public ThirdPersonController TPC => thirdPersonController;
        public AbstractPlayerState CurrentState => currentState;
        public float damage = 40f;
        public float heavyDamage = 80f;
        public bool updateStamina = true;
        public float damageModifier = 1f;

        private new Transform transform;
        private bool customMovementActive = false;
        private bool isDead = false;
        private bool hasProjectedMousePosition;
        private Vector3 projectedMousePosition;

        [Header("States")]
        [SerializeField] private AbstractPlayerState currentState;
        [SerializeField] private AbstractPlayerState[] playerStates;

        public float DamageWithModifier => damage * damageModifier;
        public float HeavyDamageWithModifier => heavyDamage * damageModifier;

        private void Awake()
        {
            transform = GetComponent<Transform>();
            currentHealth = maxHealth;
            instance = this;
            playerHealth.SetCamera(playerCamera);
        }
        
        private void Start()
        {
            if (currentState)
                currentState.OnEnterState();
        }

        public void DealDamage(AbstractEnemy enemy, float damage)
        {
            if (isDodging)
                return;

            if (currentState.stateType == PlayerStateType.StartBlock || currentState.stateType == PlayerStateType.Parry)
            {
                enemy.Parried();
                return;
            }

            if (currentState.stateType == PlayerStateType.HoldBlock)
            {
                if (currentStamina > 0f)
                {
                    SpendStamina(damage);
                    return;
                }
            }
            
            currentHealth -= Mathf.Max(0f, damage);
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

        private void Update()
        {
            ProjectMousePosition();

            if (!customMovementActive)
            {
                thirdPersonController.LookAt(ProjectedMousePosition);
                thirdPersonController.Simulate();
            }

            if (updateStamina)
                UpdateStamina();
            
            currentState.UpdateState();
        }
        
        private void FixedUpdate()
        {
            if (Position.y < -0.1f)
                transform.position = new Vector3(Position.x, 0f, Position.z);
        }

        private void UpdateStamina()
        {
            if (Time.time > staminaUpdateTime)
            {
                currentStamina = Mathf.Min(currentStamina + staminaUpdateRate * Time.deltaTime, maxStamina);
                staminaImage.fillAmount = currentStamina / maxStamina;
            }
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
            Debug.Log("PLAYER DEAD!");
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
            if (newState.staminaCost > 0f && currentStamina < 0f)
                return;
            
            if (currentState)
            {
                currentState.OnExitState();
            }
        
            currentState = newState;
            currentState.OnEnterState();
        }
        
        public void ChangeState(PlayerStateType t)
        {
            ChangeState(GetState(t));
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
        
        public void SetSpeed(float speed)
        {
            thirdPersonController.MoveSpeed = speed;
        }

        public void SpendStamina(float stamina)
        {
            currentStamina -= stamina;
            staminaImage.fillAmount = currentStamina / maxStamina;
            staminaUpdateTime = Time.time + staminaUpdateCooldown;
        }

        public AbstractPlayerState GetState(PlayerStateType stateType)
        {
            for (int i = 0; i < playerStates.Length; i++)
            {
                AbstractPlayerState s = playerStates[i];
                if (s.stateType == stateType)
                {
                    return s;
                }
            }

            Debug.LogError($"Invalid state {stateType}!");
            return null;
        }
    }
}
