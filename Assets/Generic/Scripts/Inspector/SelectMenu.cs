using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Unapparent {
	public class MenuEntry<T> {
		public string text = null;
		public T element;
		public MenuEntry(T element) => this.element = element;
		public MenuEntry(string text) => this.text = text;
		public static implicit operator MenuEntry<T>(T element) => new MenuEntry<T>(element);
		public static implicit operator MenuEntry<T>(string text) => new MenuEntry<T>(text);
		public void AddTo(GenericMenu menu, Action<T> callback) {
			if(text != null)
				menu.AddSeparator(text + "/");
			else
				menu.AddItem(new GUIContent(element.ToString()), false, delegate (object element) {
					callback?.Invoke((T)element);
				}, element);
		}
	}

	public interface ISelectMenu<T> {
		public void AddTo(GenericMenu menu, Action<T> callback);
	}

	public class Labelizer<T> {
		public static string Labelize(T obj) => obj.ToString();
	}

	public class SelectMenu<T, Labelizer> : List<MenuEntry<T>>, ISelectMenu<T> where Labelizer : Labelizer<T> {
		public SelectMenu() { }
		public SelectMenu(IEnumerable<T> entries) : base(
			entries.Select(
				entry => new MenuEntry<T>(entry)
			)) {
		}
		public SelectMenu(IEnumerable<MenuEntry<T>> entries) : base(entries) { }

		public void AddTo(GenericMenu menu, Action<T> callback) {
			string path = "";
			var Labelize = typeof(Labelizer).GetMethod("Labelize");
			foreach(MenuEntry<T> entry in this) {
				if(entry.text != null) {
					if(entry.text == "") {
						path = "";
					} else {
						path = entry.text + "/";
						menu.AddSeparator(path);
					}
				} else
					menu.AddItem(
						new GUIContent(path + Labelize.Invoke(null, new object[] { entry.element })),
						false,
						(object element) => callback?.Invoke((T)element),
						entry.element
					);
			}
		}

		public void Show(Action<T> callback) {
			GenericMenu menu = new GenericMenu();
			AddTo(menu, callback);
			menu.ShowAsContext();
		}
	}

	public class SelectMenu<T> : SelectMenu<T, Labelizer<T>> { }
}
