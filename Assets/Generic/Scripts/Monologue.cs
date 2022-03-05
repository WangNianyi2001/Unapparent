using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Unapparent {
	public class Monologue {
		public struct Option {
			public string text;
			public Command command;
		}

		public Character character;
		public string text;
		public List<Option> options = new List<Option>();

		public static GameObject MakeOptionButton(Option option) {
			GameObject prefab = Resources.Load<GameObject>("Option button");
			prefab.GetComponentInChildren<Text>().text = option.text;
			UnityEvent ev = new UnityEvent();
			ev.AddListener(() => option.command?.Execute(null));
			prefab.GetComponentInChildren<Button>().onClick = ev as Button.ButtonClickedEvent;
			return prefab;
		}
	}
}
