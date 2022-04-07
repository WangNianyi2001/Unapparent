using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

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

		public void AddTo(GenericMenu menu) {
			string path = "";
			foreach(Entry entry in this) {
				if(entry.text != null) {
					if(string.IsNullOrEmpty(entry.text)) {
						path = "";
						continue;
					}
					path = entry.text + "/";
					menu.AddSeparator(path);
				}
				else {
					GUIContent content = new GUIContent(path + OnLabelize(entry.target));
					menu.AddItem(content, false, SelectCallback, entry.target);
				}
			}
		}

		public void Show() {
			GenericMenu menu = new GenericMenu();
			AddTo(menu);
			menu.ShowAsContext();
		}
	}
}
