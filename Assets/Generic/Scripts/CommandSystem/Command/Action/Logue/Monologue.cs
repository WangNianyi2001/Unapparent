using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unapparent {
	public class Monologue : Command {
		public class Option : Command {
			public string text;
			public Command command;

			public override void Inspect(ArgList<Action> elements) {
				if(command == null)
					command = Create<Sequential>(this);
				IGUI.Inline(() => {
					IGUI.Label("Option");
					if(IGUI.TextField(ref text))
						SetDirty();
				});
				command?.Inspect(null, elements[1]);
			}

			public override void Dispose() => command?.Dispose();

			public override object Execute(Carrier target) => command?.Execute(target);
		}

		public GameObject characterObj;
		public Character character => characterObj.GetComponent<Character>();
		public string text;
		public bool useOptions = false;
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

		public Option NextOption(Carrier target) {
			Option option = Create<Option>(this);
			Delegate command = Create<Delegate>(this);
			if(next == null) {
				option.text = "Done";
				command.action = () => Level.current.CloseMonologue();
			} else {
				option.text = "Next";
				command.action = () => next.Execute(target);
			}
			option.command = command;
			return option;
		}

		public override object Execute(Carrier target) {
			if(options.Count == 0)
				options.Add(NextOption(target));
			Level.current.ShowMonologue(this);
			return null;
		}

		public override void Inspect(ArgList<Action> elements) {
			IGUI.Inline(() => {
				elements[0]?.Invoke();
				if(IGUI.ObjectField(ref characterObj, true))
					SetDirty();
				IGUI.Label("say");
				if(IGUI.TextField(ref text))
					SetDirty();
				if(IGUI.Toggle(ref useOptions))
					SetDirty();
				ShowRefBtn();
				elements[1]?.Invoke();
			});
			if(useOptions) IGUI.Block(() => {
				IGUI.Indent(() => {
					for(int i = 0; i < options.Count; ++i) {
						int j = i;
						Option option = options[j];
						option?.Inspect(null, () => {
							if(IGUI.Button("Remove")) {
								options[j].Dispose();
								options.RemoveAt(j);
								SetDirty();
							}
						});
					}
				});
				if(IGUI.Button("Add option", IGUI.exWidth)) {
					options.Add(Create<Option>(this));
					SetDirty();
				}
			});
		}

		public override void Dispose() {
			foreach(Option option in options)
				option?.Dispose();
			base.Dispose();
		}
	}
}
