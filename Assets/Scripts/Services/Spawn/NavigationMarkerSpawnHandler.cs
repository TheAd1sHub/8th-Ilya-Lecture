using System;
using TheMinefield.Behaviors;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheMinefield.Services
{
	public sealed class NavigationMarkerSpawnHandler
	{
		private readonly IGameObjectSpawner _markerSpawner;
		private readonly float _markerLifetimeSeconds;

		private Navigable _target;

		private bool _wasPreviouslyInitialized = false;

        public NavigationMarkerSpawnHandler(IGameObjectSpawner markerSpawner, float markerLifetimeSeconds = 1f)
        {
			if (markerLifetimeSeconds < 0)
				throw new ArgumentOutOfRangeException($"{nameof(markerLifetimeSeconds)}: Non-negative number expected. Got {markerLifetimeSeconds}.");

			_markerSpawner = markerSpawner;
			_markerLifetimeSeconds = markerLifetimeSeconds;
        }

		public void Initialize(Navigable target)
		{
			if (_wasPreviouslyInitialized)
				UnsubscribeFromServices();

			_target = target;

			SubscribeToServices();

			_wasPreviouslyInitialized = true;
		}

		public void Deinitialize()
		{
			if (_wasPreviouslyInitialized == false)
				return;
			
			UnsubscribeFromServices();
		}

		private void SubscribeToServices()
		{
			_target.DestinationChosen += OnDestinationChosen;
		}

		private void UnsubscribeFromServices()
		{
			_target.DestinationChosen -= OnDestinationChosen;
		}

		private void OnDestinationChosen(Vector3 destination)
		{
			Transform spawnedObject = _markerSpawner.Spawn(destination);
			Object.Destroy(spawnedObject.gameObject, _markerLifetimeSeconds);
		}
	}
}