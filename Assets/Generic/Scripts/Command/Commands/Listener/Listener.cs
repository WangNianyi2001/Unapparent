namespace Unapparent {
	public abstract class Listener : Command {
		public Statement action;

		public abstract bool Validate(Carrier target);

		public virtual void TryExecute(Carrier target) {
			if(Validate(target))
				Execute(target);
		}

		public override object Execute(Carrier target) => action?.Execute(target);
	}
}
