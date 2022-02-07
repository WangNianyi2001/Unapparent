using System;

namespace Unapparent {
	public class Conditional : Statement {
		Condition condition = null;

		public class Branch : IInspectable {
			public Statement statement = null;
			public void Inspect(Action header, Action footer) {
				if(statement == null) {
					IGUI.Inline(delegate {
						header();
						IGUI.SelectButton("Set branch", types, delegate (CommandType type) {
							statement = (Statement)Activator.CreateInstance(type);
						}, IGUI.exWidth);
						footer();
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

		public override Void Execute(Void args) {
			// TODO
			return null;
		}

		public override void Inspect(Action header, Action footer) {
			IGUI.Indent(header, delegate {
				IGUI.Inline(delegate {
					IGUI.Label("If");
					if(condition == null) {
						IGUI.SelectButton("Set condition", Condition.types, delegate (CommandType type) {
							condition = (Condition)Activator.CreateInstance(type);
						});
					} else {
						condition.Inspect(IGUI.Nil, delegate {
							IGUI.Button("Clear condition");
						});
					}
					IGUI.FillLine();
				});
				trueBranch.Inspect("Then");
				falseBranch.Inspect("Else");
				footer();
			});
		}

		public override void OnAfterDeserialize() {
			// TODO
		}

		public override void OnBeforeSerialize() {
			// TODO
		}
	}
}