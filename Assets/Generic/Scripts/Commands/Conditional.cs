using System;

namespace Unapparent {
	public class Conditional : Command {
		Command condition = null;

		public class Branch : ICommand {
			public Command statement = null;

			public void Dispose() => Command.Dispose(statement);
			public object Execute() => statement.Execute();

			public void Inspect(Action header, Action footer) {
				if(statement == null) {
					IGUI.Inline(() => {
						header?.Invoke();
						IGUI.SelectButton("Set branch", TypeMenu.statement,
							(Type type) => statement = Create(type),
							IGUI.exWidth);
						footer?.Invoke();
					});
				} else
					statement.Inspect(header, footer);
			}
			public void Inspect(string title) {
				Inspect(() => IGUI.Label(title), () => {
					if(IGUI.Button("Clear branch"))
						Dispose();
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
						IGUI.SelectButton("Set condition", TypeMenu.condition,
							(Type type) => condition = Create(type));
					} else
						condition.Inspect(null, () => {
							if(IGUI.Button("Clear condition"))
								Dispose(ref condition);
						});
					IGUI.FillLine();
				});
				trueBranch.Inspect("Then");
				falseBranch.Inspect("Else");
				footer?.Invoke();
			});
		}

		public override void Dispose() {
			Dispose(ref condition);
			trueBranch.Dispose();
			falseBranch.Dispose();
			base.Dispose();
		}
	}
}
