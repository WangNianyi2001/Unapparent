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
			GameObject prefab = Object.Instantiate(Resources.Load<GameObject>("Option Button"));
			prefab.GetComponentInChildren<Text>().text = option.text;
			Button.ButtonClickedEvent ev = new Button.ButtonClickedEvent();
			ev.AddListener(() => {
				option.command?.Execute(null);
				Level.current.CloseMonologue();
			});
			prefab.GetComponentInChildren<Button>().onClick = ev;
			return prefab;
		}
	}
}
