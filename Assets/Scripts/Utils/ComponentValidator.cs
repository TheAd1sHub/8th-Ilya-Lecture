using UnityEngine;

namespace TheMinefield.Utils.ComponentValidation
{
	public static class ComponentValidator
	{
		/// <summary>
		/// Checks if <see cref="GameObject"/> has a <see cref="Collider"/> with <see cref="Collider.isTrigger"/> enabled <br/>
		/// Works the same way when a GameObject has more than one Collider or has no Colliders at all
		/// </summary>
		/// <param name="gameObject"></param>
		/// <returns></returns>
		public static bool HasTrigger(GameObject gameObject)
		{
			foreach (Collider collider in gameObject.GetComponents<Collider>())
			{
				if (collider.isTrigger)
					return true; 
			}

			return false;
		}

		/// <summary>
		/// Checks if <see cref="GameObject"/> has a <see cref="Collider"/> with <see cref="Collider.isTrigger"/> enabled <br/>
		/// Works the same way when a GameObject has more than one Collider or has no Colliders at all
		/// </summary>
		/// <param name="gameObject"></param>
		/// <returns></returns>
		public static bool HasTrigger(MonoBehaviour gameObject) => HasTrigger(gameObject.gameObject);
	}
}
