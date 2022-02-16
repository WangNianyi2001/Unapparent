using System;
using UnityEngine;

namespace Unapparent {
	[CreateAssetMenu]
	public class Conditional : Command {
		Command condition = null;

		public class Branch : IInspectable {
			public Command statement = null;
			public void Inspect(Action header, Action footer) {
				if(statement == null) {
					IGUI.Inline(delegate {
						header?.Invoke();
						IGUI.Label("Set branch");
						ScriptableObject so = IGUI.ObjectField(
							null, typeof(ScriptableObject), false,
							GUILayout.ExpandWidth(true)
						) as ScriptableObject;
						if(so != null)
							statement = Instantiate(so) as Command;
						footer?.Invoke();
					});
				} else
					statement.Inspect(header, footer);
			}
			public void Inspect(string title) {
				Inspect(delegate {
					IGUI.Label(title);
				}, delegate {
					if(IGUI.Button("Clear branch"))
						statement = null;
				});
			}
		}
		Branch trueBranch = new Branch(), falseBranch = new Branch();

		public override object Execute() {
			// TODO
			return null;
		}

		public override void Inspect(Action header, Action footer) {
			IGUI.Indent(header, () => {
				IGUI.Inline(() => {
					IGUI.Label("If");
					if(condition == null) {
						ScriptableObject so = IGUI.ObjectField(
							null, typeof(ScriptableObject), false,
							GUILayout.ExpandWidth(true)
						) as ScriptableObject;
						if(so != null)
							condition = Instantiate(so) as Command;
					} else
						condition.Inspect(null, () => IGUI.Button("Clear condition"));
					IGUI.FillLine();
				});
				trueBranch.Inspect("Then");
				falseBranch.Inspect("Else");
				footer?.Invoke();
			});
		}
	}
}
