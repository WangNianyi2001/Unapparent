using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unapparent {
	public class Monologue : Command {
		public struct Option {
			public string text;
			public Command command;
		}

		public Character character;
		public string text;
		public List<Option> options = new List<Option>();
		[NonSerialized] public Monologue next = null;

		public GameObject MakeOptionButton(Option option) {
			GameObject prefab = Instantiate(Resources.Load<GameObject>("Option Button"));
			prefab.GetComponentInChildren<Text>().text = option.text;
			Button.ButtonClickedEvent ev = new Button.ButtonClickedEvent();
			ev.AddListener(() => option.command?.Execute(character));
			prefab.GetComponentInChildren<Button>().onClick = ev;
			return prefab;
		}

		public Option NextOption() {
			Option option = new Option();
			Delegate command = Create<Delegate>(this);
			if(next == null) {
				option.text = "Done";
				command.action = () => Level.current.CloseMonologue();
			} else {
				option.text = "Next";
				command.action = () => next.Execute(null);
			}
			option.command = command;
			return option;
		}

		public override object Execute(Carrier target) {
			if(options.Count == 0)
				options.Add(NextOption());
			Level.current.ShowMonologue(this);
			return null;
		}

		public override void Inspect(ArgList<Action> elements) {
			IGUI.Inline(() => {
				elements[0]?.Invoke();
				if(IGUI.ObjectField(ref character, true))
					SetDirty();
				IGUI.Label("say");
				if(IGUI.TextField(ref text))
					SetDirty();
				ShowRefBtn();
				elements[1]?.Invoke();
			});
		}
	}
}
