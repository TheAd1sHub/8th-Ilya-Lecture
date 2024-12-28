using System;
using UnityEngine;

namespace TheMinefield.Services
{
	public sealed class MouseInputHandler : MonoBehaviour, IInputHandler
	{
		public event Action<Vector3> ScreenPointSelected;

		[SerializeField] private MouseButton _selectionButton = MouseButton.Left;

		private void Update()
		{
			if (Input.GetMouseButtonDown((int)_selectionButton))
				ScreenPointSelected?.Invoke((Input.mousePosition));
		}

		private enum MouseButton
		{
			Left = 0,
			Right = 1,
			Middle = 2
		}
	}
}