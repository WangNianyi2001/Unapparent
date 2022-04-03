using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unapparent {
	public class Monologue : Statement {
		[Serializable]
		public class Option {
			public string text;
			public Statement action;
		}

		public Character character;
		public string text;
		public List<Option> options = new List<Option>();
		[NonSerialized] public Monologue next = null;

		public GameObject MakeOptionButton(Option option) {
			GameObject prefab = Instantiate(Resources.Load<GameObject>("Option Button"));
			prefab.GetComponentInChildren<Text>().text = option.text;
			Button.ButtonClickedEvent ev = new Button.ButtonClickedEvent();
			ev.AddListener(() => {
				option.action?.Execute(character);
				Level.current.CloseMonologue();
			});
			prefab.GetComponentInChildren<Button>().onClick = ev;
			return prefab;
		}

		public override object Execute(Carrier target) {
			Level.current.ShowMonologue(this);
			return null;
		}
	}
}
