using System;
using System.Threading.Tasks;

namespace Unapparent {
	public abstract class Listener : Command {
		public Statement action;

		public abstract bool Validate(params object[] args);

		public virtual async void TryExecute(params object[] args) {
			if(Validate(args))
				await Execute();
		}

		public override async Task<object> Execute() => await action?.Execute();
	}
}
