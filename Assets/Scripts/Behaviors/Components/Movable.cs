using TheMinefield.CoreUtils;
using UnityEngine;

namespace TheMinefield.Behaviors
{
	public sealed class Movable : MonoBehaviour
	{
		[SerializeField, Min(0)] private float _maxSpeed = 10f;
		[SerializeField] private Property<float> _speed = 1f;

		public IReadOnlyProperty<float> Speed => _speed;

		private void OnValidate()
		{
			if (_speed.Value > _maxSpeed)
				_speed.Value = _maxSpeed;
					  
			if (_speed.Value < 0)
				_speed.Value = 0;
		}
	}
}
