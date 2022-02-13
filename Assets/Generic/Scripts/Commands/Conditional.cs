using System;

namespace Unapparent {
	public class Conditional : ICommand {
		ICommand condition = null;

		public class Branch : IInspectable {
			public ICommand statement = null;
			public void Inspect(Action header, Action footer) {
				if(statement == null) {
					IGUI.Inline(delegate {
						header();
						IGUI.SelectButton("Set branch", Command.statement, delegate (Type type) {
							statement = (ICommand)Activator.CreateInstance(type);
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
							Command.condition,
							delegate (Type type) {
								condition = (ICommand)Activator.CreateInstance(type);
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
