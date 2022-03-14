using System.Collections.Generic;
using System.Linq;

namespace Unapparent {
	public class Menu<T> : List<Menu<T>.Entry> {
		public class Entry {
			public string text = null;
			public T target;

			public Entry(T target) => this.target = target;

			public Entry(string text) => this.text = text;

			public static implicit operator Entry(T element) => new Entry(element);

			public static implicit operator Entry(string text) => new Entry(text);
		}

		public delegate string Labelize(T target);
		public Labelize OnLabelize = (T target) => target.ToString();

		public delegate void Select(T target);
		public Select OnSelect = (T target) => { };

		public void SelectCallback(object target) => OnSelect((T)target);

		public Menu() { }

		public Menu(IEnumerable<T> entries) :
			base(entries.Select(entry => new Entry(entry))) {
		}

		public Menu(IEnumerable<Entry> entries) : base(entries) { }
	}
}
