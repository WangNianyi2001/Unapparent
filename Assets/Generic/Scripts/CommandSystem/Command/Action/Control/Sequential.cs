using System;
using System.Collections.Generic;

namespace Unapparent {
	public class Sequential : Command {
		public List<Command> sequence = new List<Command>();
		public InspectorList<Command> iList;

		public override object Execute(Carrier target) {
			foreach(Command command in sequence)
				command.Execute(target);
			return null;
		}

		public override void Inspect(ArgList<Action> elements) {
			if(iList == null) {
				iList = new InspectorList<Command>(sequence, "Sequential");
				iList.actionButtons.Add(new InspectorList<Command>.ActionButton {
					label = "Append",
					action = () => {
						TypeMenu.statement.Show((Type type) => {
							sequence.Add(Create(type, this));
							SetDirty();
						});
					}
				});
				iList.moreOptions.Add(new InspectorList<Command>.MoreOption {
					label = "Remove",
					action = (Command command, int i) => {
						sequence[i].Dispose();
						sequence.RemoveAt(i);
						SetDirty();
					}
				});
			}
			iList.Inspect();
		}

		public override void Dispose() {
			foreach(Command command in sequence)
				command?.Dispose();
			base.Dispose();
		}
	}
}
