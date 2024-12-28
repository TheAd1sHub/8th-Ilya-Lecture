using System;
using UnityEngine;
using UnityEngine.AI;

namespace TheMinefield.Behaviors
{
	[RequireComponent(typeof(NavMeshAgent))]
	[RequireComponent(typeof(Movable))]
	public sealed class Navigable : MonoBehaviour, IDisablableOnDeath
	{
		public event Action<Vector3> DestinationChosen;
		public event Action MovementStateChanged;

		[SerializeField, Min(0)] private float _destinationCollisionDistance = 0.15f;

		private NavMeshAgent _agent;
		private Movable _movable;

		private bool _isMoving = false;

		public bool IsMoving
		{
			get => _isMoving;
			private set
			{
				if (_isMoving == value)
					return;

				_isMoving = value;
                MovementStateChanged?.Invoke();
            }
		}

		public bool IsEnabled { get; set; } = true;

		public void SetDestination(Vector3 destination)
		{
			if (IsEnabled == false)
				return;

			NavMeshPath path = new NavMeshPath();

			if (_agent.CalculatePath(destination, path) && path.status == NavMeshPathStatus.PathComplete)
			{ 
				_agent.SetPath(path);

				IsMoving = true;
				_agent.isStopped = false;

				DestinationChosen?.Invoke(destination);

				return;
			}
			
			throw new ArgumentException($"The given destination ({destination}) cannot be reached by '{name}' agent");
		}

		private void OnSpeedChanged() => _agent.speed = _movable.Speed.Value;

		private void Update()
		{
			if (IsEnabled == false)
				return;

			if (IsMoving == false)
				return;

			if (_agent.remainingDistance <= _destinationCollisionDistance)
			{
				IsMoving = false;
				_agent.isStopped = true;
			}
		}

		private void OnEnable()
		{
			_movable.Speed.Changed += OnSpeedChanged;
		}

		private void OnDisable()
		{
			_movable.Speed.Changed -= OnSpeedChanged;
		}

		private void Awake()
		{
			_agent = GetComponent<NavMeshAgent>();
			_movable = GetComponent<Movable>();

			_agent.speed = _movable.Speed.Value;
		}
	}
}
