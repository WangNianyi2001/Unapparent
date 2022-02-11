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
						IGUI.SelectButton<Type, TypeMenu, TypeMenu.Labelizer>("Set branch", menu, delegate (Type type) {
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

		public override object Execute() {
			// TODO
			return null;
		}

		public override void Inspect(Action header, Action footer) {
			IGUI.Indent(header, delegate {
				IGUI.Inline(delegate {
					IGUI.Label("If");
					if(condition == null) {
						IGUI.SelectButton<Type, TypeMenu, TypeMenu.Labelizer>(
							"Set condition",
							Condition.menu,
							delegate (Type type) {
								condition = (Condition)Activator.CreateInstance(type);
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
