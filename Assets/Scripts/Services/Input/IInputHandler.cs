using System;
using UnityEngine;

namespace TheMinefield.Services
{
	public interface IInputHandler
	{
		public event Action<Vector3> ScreenPointSelected;
	}
}