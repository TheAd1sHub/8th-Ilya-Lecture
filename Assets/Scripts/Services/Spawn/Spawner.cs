using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Linq;

namespace TheMinefield.Services
{
	public sealed class Spawner : IGameObjectSpawner
	{
		private Transform _spawnedPrefab;

		private List<Transform> _spawnedObjects = new();

        public Spawner(Transform spawnedPrefab)
        {
			_spawnedPrefab = spawnedPrefab;
        }

        public Transform Spawn(Vector3 position)
		{
			Transform marker = Object.Instantiate(_spawnedPrefab, position, Quaternion.identity);
			_spawnedObjects.Add(marker);

			return marker;
		}

		public Transform Spawn() => Spawn(Vector3.zero);

		public IEnumerable<Transform> SpawnAll(IEnumerable<Vector3> positions)
		{
			List<Transform> spawnedObjects = new List<Transform>(positions.Count());

			foreach (Vector3 spawnPosition in positions)
				spawnedObjects.Add(Spawn(spawnPosition));

			_spawnedObjects.AddRange(spawnedObjects);

			return spawnedObjects;
		}

		public T Spawn<T>(Vector3 position) where T : MonoBehaviour => Spawn(position).GetComponent<T>();

		public T Spawn<T>() where T : MonoBehaviour => Spawn<T>(Vector3.zero);

		public IEnumerable<T> SpawnAll<T>(IEnumerable<Vector3> positions) where T : MonoBehaviour
		{
			List<T> spawnedObjects = new List<T>(positions.Count());

			foreach (Vector3 spawnPosition in positions)
				spawnedObjects.Add(Spawn<T>(spawnPosition));

			_spawnedObjects.AddRange(spawnedObjects.Select(@object => @object.transform));

			return spawnedObjects;
		}

		public void Clear()
		{
            for (int i = _spawnedObjects.Count - 1; i >= 0; i--)
            {
				Transform spawnedObject = _spawnedObjects[i];
				_spawnedObjects.RemoveAt(i);

				if (spawnedObject == null)
					continue;

				Object.Destroy(spawnedObject);
            }
        }
	}
}