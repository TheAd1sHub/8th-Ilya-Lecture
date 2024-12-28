using System.Collections.Generic;
using TheMinefield.Behaviors;
using UnityEngine;
using System.Linq;

namespace TheMinefield.Services
{
	public sealed class Bootstrap : MonoBehaviour
	{
		[Header("Services")]
		[SerializeField] private MouseInputHandler _inputHandler;
		
		[Header("Player")]
		[SerializeField] private Navigable _playerPrefab;
		[SerializeField] private Transform _playerSpawnPoint;

		[Header("Markers")]
		[SerializeField] private Transform _movementMarkerPrefab;
		[SerializeField] private float _markerLifetimeSeconds = 1.5f;

		[Header("Mines")]
		[SerializeField] private Transform _minePrefab;
		[SerializeField] private List<Transform> _minesSpawnPositions;

		private Navigable _player;
		private ManuallyControlledNavigableHandler _playerMover = new();

		private IGameObjectSpawner _markerSpawner;
		private NavigationMarkerSpawnHandler _markerSpawnHandler;

		private IGameObjectSpawner _mineSpawner;

		public bool Initialized { get; private set; } = false;

		public void Initialize()
		{
			if (Initialized)
				Deinitialize();

			// Player-related intialization
			if (_player == null)
				_player = Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity);
			else 
				_player.transform.position = _playerSpawnPoint.position;

			_playerMover.Initialize(_inputHandler, _player);

			// Marker-related initialization
			_markerSpawner ??= new Spawner(_movementMarkerPrefab);
			_markerSpawnHandler ??= new NavigationMarkerSpawnHandler(_markerSpawner, _markerLifetimeSeconds);

			_markerSpawnHandler.Initialize(_player);

			// Mine-related initialization
			_mineSpawner ??= new Spawner(_minePrefab);

			_mineSpawner.SpawnAll(_minesSpawnPositions.Select(transform => transform.position));

			Initialized = true;
		}

		private void Deinitialize()
		{
			_markerSpawnHandler.Deinitialize();
			_mineSpawner?.Clear();

			Initialized = false;
		}

		private void Start() => Initialize();
	}
}