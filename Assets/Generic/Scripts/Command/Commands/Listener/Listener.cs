using System.Threading.Tasks;

namespace Unapparent {
	public abstract class Listener : Command {
		public Statement action;

		public abstract bool Validate(Carrier target, params object[] args);

		public virtual async void TryExecute(Carrier target, params object[] args) {
			if(Validate(target, args))
				await Execute(target);
		}

		public override async Task<object> Execute(Carrier target) => await action?.Execute(target);
	}
}
