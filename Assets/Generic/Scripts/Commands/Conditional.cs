using System;
using UnityEngine;

namespace Unapparent {
	public class Conditional : Command {
		Command condition = null;

		public class Branch {
			public Command statement = null;

			public void Dispose() => Command.Dispose(statement);
			public object Execute() => statement.Execute();

			public void Inspect(params Action[] elements) {
				if(statement == null) {
					IGUI.Inline(() => {
						elements[0]?.Invoke();
						IGUI.SelectButton("Set branch", TypeMenu.statement,
							(Type type) => statement = Create(type),
							IGUI.exWidth);
						elements[1]?.Invoke();
					});
				} else
					statement.Inspect();
			}
			public void Inspect(string title) {
				if(statement == null) {
					IGUI.Inline(() => {
						IGUI.Label(title);
						IGUI.SelectButton("Set branch", TypeMenu.statement,
							(Type type) => statement = Create(type),
							IGUI.exWidth);
						if(IGUI.Button("Clear branch"))
							Dispose();
					});
				} else
					statement.Inspect();
			}
		}
		Branch trueBranch = new Branch(), falseBranch = new Branch();

		public override object Execute() {
			// TODO
			return null;
		}

		public override void Inspect(ArgList<Action> elements) {
			IGUI.Indent(() => {
				IGUI.Inline(() => {
					IGUI.Label("If");
					if(condition == null) {
						IGUI.SelectButton("Set condition", TypeMenu.condition,
							(Type type) => condition = Create(type));
					} else
						condition.Inspect(() => {
							if(IGUI.Button("Clear condition"))
								Dispose(ref condition);
						});
					IGUI.FillLine();
				});
				trueBranch.Inspect("Then");
				falseBranch.Inspect("Else");
				elements[1]?.Invoke();
			}, elements[0]);
		}

		public override void Dispose() {
			Dispose(ref condition);
			trueBranch.Dispose();
			falseBranch.Dispose();
			base.Dispose();
		}
	}
}
