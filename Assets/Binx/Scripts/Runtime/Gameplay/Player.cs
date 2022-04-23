using Binx.UI;
using UnityEngine;

namespace Binx
{
    public class Player : MonoBehaviour
    {
        public static Player instance = null;

        [SerializeField] private int maxHealth;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private ThirdPersonController thirdPersonController;
        [SerializeField] private Transform cameraFollowTransform;
        [SerializeField] private UIPlayerHealth playerHealth;
        private int currentHealth;
        private new Transform transform;
        private bool customMovementActive = false;
        private bool isDead = false;
        private bool hasProjectedMousePosition;
        private Vector3 projectedMousePosition;
        
        public Vector3 Position => transform.position;
        public Vector3 ProjectedMousePosition => projectedMousePosition;
        public int CurrentHealth => currentHealth;
        public int MaxHealth => maxHealth;

        public float dodgeDuration = 0.3f;
        public float dodgeRecovery = 0.1f;
        public bool isDodging = false;

        public GameObject body;
        public GameObject dodgeBody;

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
    }
}
