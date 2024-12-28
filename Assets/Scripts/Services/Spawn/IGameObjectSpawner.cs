using System.Collections.Generic;
using UnityEngine;

namespace TheMinefield.Services
{
	public interface IGameObjectSpawner
	{
		public Transform Spawn();

		public Transform Spawn(Vector3 position);

		public IEnumerable<Transform> SpawnAll(IEnumerable<Vector3> positions);

		public T Spawn<T>() where T : MonoBehaviour;

		public T Spawn<T>(Vector3 position) where T : MonoBehaviour;

		public IEnumerable<T> SpawnAll<T>(IEnumerable<Vector3> positions) where T : MonoBehaviour;

		public void Clear();
	}
}