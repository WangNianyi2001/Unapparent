using System;

namespace Unapparent {
	public class Conditional : Command {
		public Command condition = null;

		[Serializable]
		public class Branch {
			public Command statement = null, parent;

			public Branch(Command parent) {
				this.parent = parent;
			}

			public void Dispose() => statement?.Dispose();
			public object Execute() => statement?.Execute();

			public void Inspect(string title) {
				if(statement == null) {
					IGUI.Inline(() => {
						IGUI.Label(title);
						IGUI.SelectButton("Set branch", TypeMenu.statement,
							(Type type) => {
								statement = Create(type, parent);
								statement.SetDirty();
							},
							IGUI.exWidth);
						if(IGUI.Button("Clear branch"))
							Dispose();
					});
				} else
					statement.Inspect();
			}
		}

		public Branch trueBranch, falseBranch;

		public Conditional() {
			trueBranch = new Branch(this);
			falseBranch = new Branch(this);
		}

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
							(Type type) => {
								condition = Create(type, this);
								SetDirty();
							});
					} else {
						condition.Inspect(() => {
							if(IGUI.Button("Clear condition")) {
								condition?.Dispose();
								SetDirty();
							}
						});
					}
					IGUI.FillLine();
				});
				trueBranch.Inspect("Then");
				falseBranch.Inspect("Else");
				ShowRefBtn();
				elements[1]?.Invoke();
			}, elements[0]);
		}

		public override void Dispose() {
			condition?.Dispose();
			trueBranch.Dispose();
			falseBranch.Dispose();
			base.Dispose();
		}
	}
}
