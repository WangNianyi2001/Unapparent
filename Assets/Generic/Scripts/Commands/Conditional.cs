using System;

namespace Unapparent {
	public class Conditional : ICommand {
		Command condition = null;

		public class Branch : IInspectable {
			public Command statement = null;
			public void Inspect(Action header, Action footer) {
				if(statement == null) {
					IGUI.Inline(delegate {
						header();
						IGUI.SelectButton("Set branch", Command.TypeMenu.statement, delegate (Type type) {
							statement = new Command(type);
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

		public object Execute() {
			// TODO
			return null;
		}

		public void Inspect(Action header, Action footer) {
			IGUI.Indent(header, delegate {
				IGUI.Inline(delegate {
					IGUI.Label("If");
					if(condition == null) {
						IGUI.SelectButton(
							"Set condition",
							Command.TypeMenu.condition,
							delegate (Type type) {
								condition = new Command(type);
							}
						);
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
	}
}
