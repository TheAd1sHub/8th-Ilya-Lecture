using System;

namespace TheMinefield.CoreUtils
{
	public interface IReadOnlyProperty<T> where T : IComparable<T>
	{
		public event Action Changed;

		public T Value { get; }
	}
}
