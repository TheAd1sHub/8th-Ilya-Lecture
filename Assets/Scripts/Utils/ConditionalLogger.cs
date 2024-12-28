using TheMinefield.Utils.ComponentValidation;
using UnityEngine;

namespace TheMinefield.Utils.ConditionalLogging
{
	public static class ConditionalLogger
	{
		public static class WarnIf
		{
			public static void HasNoTrigger<TRequiringComponent>(TRequiringComponent checkRequester) where TRequiringComponent : MonoBehaviour
			{
				if (ComponentValidator.HasTrigger(checkRequester) == false)
					Debug.LogWarning($"{typeof(TRequiringComponent)} component of '{checkRequester.name}' entity requires to have at least one Collider with isTrigger set to true");
			}
		}
	}
}
