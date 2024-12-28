using TheMinefield.Behaviors;
using UnityEngine;

namespace TheMinefield.Services
{
	public sealed class ManuallyControlledNavigableHandler
	{
		private IInputHandler _inputHandler;
		private Navigable _navigable;

		private bool _wasInitialized;

		public void Initialize(IInputHandler inputHandler, Navigable navigable)
		{
			if (_wasInitialized)
				UnsubscribeFromServices();

			_inputHandler = inputHandler;
			_navigable = navigable;

			SubscribeToServices();

			_wasInitialized = true;
		}

		private void SubscribeToServices()
		{
			_inputHandler.ScreenPointSelected += OnScreenPointSelected;
		}

		private void UnsubscribeFromServices()
		{
			_inputHandler.ScreenPointSelected -= OnScreenPointSelected;
		}

		private void OnScreenPointSelected(Vector3 screenPoint)
		{
			Ray selectionDirection = Camera.main.ScreenPointToRay(screenPoint);
			
			if (Physics.Raycast(selectionDirection, out RaycastHit hitInfo, Mathf.Infinity))
				_navigable.SetDestination(hitInfo.point);

			// Debug.DrawLine(selectionDirection.origin, hitInfo.point, Color.green, 20f);
		}
	}
}