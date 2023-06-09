using System.Collections;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace Binx
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class ThirdPersonController : MonoBehaviour
	{
		public static int animIDSpeed;
		public static int animIDVertical;
		public static int animIDHorizontal;
		public static int animIDGrounded;
		public static int animIDJump;
		public static int animIDFreeFall;
		public static int animIDMotionSpeed;
		public static int animIDAttack;
		public static int animIDPrepareAttack;
		
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 2.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 5.335f;
		[Tooltip("How fast the character turns to face movement direction")]
		[Range(0.0f, 0.3f)]
		public float RotationSmoothTime = 0.12f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.50f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.28f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 70.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -30.0f;
		[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
		public float CameraAngleOverride = 0.0f;
		[Tooltip("For locking the camera position on all axis")]
		public bool LockCameraPosition = false;

		// cinemachine
		private float _cinemachineTargetYaw;
		private float _cinemachineTargetPitch;

		// player
		[SerializeField]
		private float _speed;
		private float _animationBlend;
		private float _targetRotation = 0.0f;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;
		private Vector3 lookAtTarget;
		private Vector3 animationDirection;
		
		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

		// animation IDs
		

		private PlayerInput _playerInput;
		private CharacterController _controller;
		private StarterAssetsInputs _input;
		[SerializeField] private GameObject _mainCamera;
		[SerializeField] private float runAnimationLerpFactor;
		private new Transform transform;

		private const float _threshold = 0.01f;

		private Animator animator;
		public Animator Animator => animator;
		public float TargetRotation => _targetRotation;
		public StarterAssetsInputs Input => _input;

		private bool hasAnimator;

		private bool IsCurrentDeviceMouse => _playerInput.currentControlScheme == "KeyboardMouse";

		private void Start()
		{
			transform = base.transform;
			hasAnimator = animator != null;
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();
			_playerInput = GetComponent<PlayerInput>();

			AssignAnimationIDs();

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;
		}

		public void Simulate()
		{
			JumpAndGravity();
			GroundedCheck();
			if (!Player.instance.isDodging && !Player.instance.blockMovement)
			{
				Move();
			}
		}

		private void LateUpdate()
		{
			CameraRotation();
		}

		private void AssignAnimationIDs()
		{
			animIDSpeed = Animator.StringToHash("Speed");
			animIDVertical = Animator.StringToHash("Vertical");
			animIDHorizontal = Animator.StringToHash("Horizontal");
			animIDGrounded = Animator.StringToHash("Grounded");
			animIDJump = Animator.StringToHash("Jump");
			animIDFreeFall = Animator.StringToHash("FreeFall");
			animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
			animIDAttack = Animator.StringToHash("Attack");
			animIDPrepareAttack = Animator.StringToHash("PrepareAttack");
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

			// update animator if using character
			if (hasAnimator)
			{
				// _animator.SetBool(_animIDGrounded, Grounded);
			}
		}

		private void CameraRotation()
		{
			// if there is an input and camera position is not fixed
			if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
			{
				//Don't multiply mouse input by Time.deltaTime;
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
				_cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
				_cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
			}

			// clamp our rotations so our values are limited 360 degrees
			_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
			_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

			// Cinemachine will follow this target
			CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
		}

		private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.move == Vector2.zero) targetSpeed = 0.0f;
			
			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			float deltaTime = Time.deltaTime;
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}
			_animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, deltaTime * SpeedChangeRate);

			// normalise input direction
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.move != Vector2.zero)
				_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg
					+ _mainCamera.transform.eulerAngles.y;
			
			transform.LookAt(lookAtTarget, Vector3.up);
			transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

			Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
			Vector3 targetAnimationDirection = transform.InverseTransformDirection(targetDirection) * _input.move.magnitude;
			animationDirection = Vector3.Lerp(animationDirection, targetAnimationDirection, runAnimationLerpFactor * deltaTime);
			
			// move the player
			_controller.Move(targetDirection.normalized * (_speed * deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * deltaTime);

				//s update animator if using character
			if (hasAnimator)
			{
				animator.SetFloat(animIDSpeed, _animationBlend);
				animator.SetFloat(animIDVertical, animationDirection.z);
				animator.SetFloat(animIDHorizontal, animationDirection.x);
				//animator.SetFloat(animIDMotionSpeed, inputMagnitude);
			}
		}

		private void JumpAndGravity()
		{
			if (Grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// update animator if using character
				if (hasAnimator)
				{
					// _animator.SetBool(_animIDJump, false);
					// _animator.SetBool(_animIDFreeFall, false);
				}

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

					// update animator if using character
					if (hasAnimator)
					{
						animator.SetBool(animIDJump, true);
					}
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}
				else
				{
					// update animator if using character
					if (hasAnimator)
					{
						// _animator.SetBool(_animIDFreeFall, true);
					}
				}

				// if we are not grounded, do not jump
				_input.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded)
				Gizmos.color = transparentGreen;
			else
				Gizmos.color = transparentRed;
			
			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			if (transform)
				Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}

		public void LookAt(Vector3 target)
		{
			lookAtTarget = target;
		}
		
		public void SetCamera(Camera camera)
		{
			_mainCamera = camera.gameObject;
		}

		public void SetTrigger(int id)
		{
			animator.SetTrigger(id);
		}

		public void SetAnimator(Animator animator)
		{
			this.animator = animator;
			hasAnimator = animator != null;
		}

		public void OnJump(InputValue val)
		{
			if (Player.instance.isDodging || _input.move.magnitude == 0f || Player.instance.CurrentState.stateType != PlayerStateType.Idle)
				return;

			// StartCoroutine(DodgeCoroutine());
			Player.instance.ChangeState(PlayerStateType.Dodge);
		}

		private IEnumerator DodgeCoroutine()
		{
			Player.instance.body.SetActive(false);
			Player.instance.dodgeBody.SetActive(true);
			Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
			Player.instance.isDodging = true;
			float duration = 0f;
			float dodgeDuration = 0.3f;
			Vector3 forwardDirection = transform.forward;
			
			if (_input.move.magnitude > 0f)
				forwardDirection = targetDirection.normalized;
			
			while (duration < dodgeDuration)
			{
				float deltaTime = Time.deltaTime;
				duration += deltaTime;
				ManualMove(forwardDirection, 15f);
				yield return null;
			}

			Player.instance.body.SetActive(true);
			Player.instance.dodgeBody.SetActive(false);
			
			duration = 0f;
			float dodgeRecovery = 0.2f;
			while (duration < dodgeRecovery)
			{
				duration += Time.deltaTime;
				ManualMove(forwardDirection, 3);
				yield return null;
			}

			Player.instance.isDodging = false;
		}

		public void ManualMove(Vector3 direction, float speed)
		{
			float deltaTime = Time.deltaTime;
			_controller.Move(direction * (speed * deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * deltaTime);
		}

		public void Stop()
		{
			_controller.SimpleMove(Vector3.zero);
		}

		public void UpdateRotation()
		{
			transform.LookAt(lookAtTarget, Vector3.up);
			transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
		}
	}
}