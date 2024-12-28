using System;
using UnityEngine;

namespace TheMinefield.CoreUtils
{
	[Serializable]
	public class Property<T> : IReadOnlyProperty<T> where T : IComparable<T>
	{
		public event Action Changed;

		[SerializeField] private T _value;

        public Property(T initialValue)
        {
            _value = initialValue;
        }

        public Property() : this(default)
		{
		}

        public T Value
		{
			get => _value;
			set
			{
				if (_value.CompareTo(value) == 0)
					return;

				_value = value;

				OnChanged(value);
				Changed?.Invoke();
			}
		}

		/// <summary>
		/// Called before the <see cref="Changed"/> event;<br/>
		/// To be overridden by child classes to implement custom handling logic.
		/// </summary>
		protected virtual void OnChanged(T newValue) { }

		public static implicit operator T (Property<T> property) => property.Value;

		public static implicit operator Property<T> (T value) => new Property<T>(value);
	}
}
