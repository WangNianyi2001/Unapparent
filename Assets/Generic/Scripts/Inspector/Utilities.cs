using System;
using System.Collections.Generic;

namespace Unapparent {
	public class ArgList<T> : List<T> {
		public ArgList(IEnumerable<T> collection) {
			InsertRange(0, collection);
		}
		public new T this[int i] {
			get => i >= 0 && i < Count ? base[i] : default(T);
		}
	}

	public interface IInspectable {
		public void Inspect(params Action[] elements);
	}
}
