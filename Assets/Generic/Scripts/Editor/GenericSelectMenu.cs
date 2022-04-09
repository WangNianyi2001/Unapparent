using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Unapparent {
	public class GenericSelectMenu<T> :
		List<GenericSelectMenu<T>.Entry>, GenericSelectMenu<T>.Initializer {
		public interface Initializer {
			public void AddTo(GenericSelectMenu<T> menu);
		}

		public class Entry : Initializer {
			public string text = null;
			public T target;

			public Entry(T target) => this.target = target;
			public Entry(string text) => this.text = text;

			public void AddTo(GenericSelectMenu<T> menu) => menu.Add(this);

			public static implicit operator Entry(T element) => new Entry(element);
			public static implicit operator Entry(string text) => new Entry(text);
		}

		public delegate string Labelize(T target);
		public Labelize OnLabelize = (T target) => target.ToString();

		public delegate void Select(T target);
		public Select OnSelect = (T target) => { };

		public void SelectCallback(object target) => OnSelect((T)target);

		public GenericSelectMenu() { }

		public GenericSelectMenu(IEnumerable<Initializer> initializers) {
			foreach(var initializer in initializers)
				initializer.AddTo(this);
		}

		public void AddTo(GenericSelectMenu<T> menu) => menu.AddRange(this);

		public void ApplyTo(GenericMenu menu) {
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
			ApplyTo(menu);
			menu.ShowAsContext();
		}
	}
}
