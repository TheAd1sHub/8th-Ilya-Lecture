using System;
using TheMinefield.CoreUtils;
using UnityEngine;

namespace TheMinefield.Behaviors
{
    public sealed class Damageable : MonoBehaviour
	{
		public event Action Killed;

		[SerializeField, Delayed, Min(1)] private int _maxHealth = 100;
		[SerializeField, Range(0, 100)] private int _criticalHealthMaxPerCent;
		[SerializeField] private Property<int> _health = 100;

		public IReadOnlyProperty<int> Health => _health;

		// Да, я помню про потерю точности при делении int'ов,
		// Но по задаче-то нам всё равно нужны *целые* проценты
		public float RemainingHealthPerCent => _maxHealth / 100f * _health.Value;
		public bool HasCriticalHealth => RemainingHealthPerCent <= _criticalHealthMaxPerCent;

		[ContextMenu("Print Health Info")]
		public void PrintHealthInfo() => Debug.Log($"Remaining: {_health.Value} ({RemainingHealthPerCent}%). Is critical: {HasCriticalHealth}");

		public void ApplyDamage(int amount)
		{
			if (amount < 0)
				throw new ArgumentOutOfRangeException($"A non-negative integer value expected. Got {amount}.");

			checked
			{
				_health.Value -= amount;
			}

			if (_health.Value > 0)
				return;

			_health.Value = 0;

			Killed?.Invoke();

			DeactivateDisablableOnDeathComponents();
        }

		public void ApplyHealing(int amount)
		{
			if (amount < 0)
				throw new ArgumentOutOfRangeException($"A non-negative integer value expected. Got {amount}.");

			checked
			{
				_health.Value += amount;
			}

			if (_health.Value > _maxHealth)
				_health.Value = _maxHealth;
		}

		private void DeactivateDisablableOnDeathComponents()
		{
			foreach (IDisablable component in GetComponents<IDisablableOnDeath>())
				component.IsEnabled = false;
		}

		private void OnValidate()
		{
			if (_health.Value > _maxHealth)
				_health.Value = _maxHealth;

			if (_health.Value < 0)
				_health.Value = 0;
		}
	}
}
