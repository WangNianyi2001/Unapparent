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

			public object Execute(Carrier target) => statement?.Execute(target);
		}

		public Branch trueBranch, falseBranch;

		public Conditional() {
			trueBranch = new Branch(this);
			falseBranch = new Branch(this);
		}

		public override object Execute(Carrier target) =>
			((bool)condition?.Execute(target) ? trueBranch : falseBranch)?.Execute(target);
	}
}
