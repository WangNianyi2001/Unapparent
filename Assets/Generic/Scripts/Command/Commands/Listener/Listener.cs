using System.Threading.Tasks;

namespace Unapparent {
	public abstract class List : Command {
		public Statement action;

		public abstract bool Validate(params object[] args);

		public virtual void TryExecute(params object[] args) {
			if(Validate(args))
				Execute();
		}

		public override async Task<object> Execute() => await action?.Execute();
	}
}
