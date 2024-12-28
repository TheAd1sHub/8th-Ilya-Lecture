using TheMinefield.Behaviors;
using UnityEngine;

namespace TheMinefield.Animation
{
	[RequireComponent(typeof(Navigable), typeof(Damageable))]
	public sealed class PlayerAnimationHandler : MonoBehaviour
	{
		private readonly int _isMovingKey = Animator.StringToHash("isMoving");
		private readonly int _isDeadKey = Animator.StringToHash("isDead");
		private readonly int _hasCriticalHealthKey = Animator.StringToHash("hasCriticalHealth");
		private readonly int _gotDamagedTriggerKey = Animator.StringToHash("gotDamaged");

		[SerializeField] private Animator _animator;

		private Navigable _playerMovementService;
		private Damageable _playerHealthService;

        private void OnMovementStateChanged() => _animator.SetBool(_isMovingKey, _playerMovementService.IsMoving);

		private void OnKilled() => _animator.SetBool(_isDeadKey, true);

		private void OnHealthChanged()
		{
			_animator.SetTrigger(_gotDamagedTriggerKey);
			_animator.SetBool(_hasCriticalHealthKey, _playerHealthService.HasCriticalHealth);
		}

        private void OnEnable()
		{
			_playerMovementService.MovementStateChanged += OnMovementStateChanged;
			_playerHealthService.Killed += OnKilled;
			_playerHealthService.Health.Changed += OnHealthChanged;
		}

		private void OnDisable()
		{
			_playerMovementService.MovementStateChanged -= OnMovementStateChanged;
            _playerHealthService.Killed -= OnKilled;
            _playerHealthService.Health.Changed -= OnHealthChanged;
        }

        private void OnValidate()
        {
            _animator ??= GetComponent<Animator>() ?? GetComponentInChildren<Animator>();
        }

        private void Awake()
		{
			_playerMovementService = GetComponent<Navigable>();
            _playerHealthService = GetComponent<Damageable>();
        }
	}
}
