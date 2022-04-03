namespace Unapparent {
	public abstract class List : Command {
		public Statement action;

		public abstract bool Validate(Carrier target, params object[] args);

		public virtual void TryExecute(Carrier target, params object[] args) {
			if(Validate(target, args))
				Execute(target);
		}

		public override object Execute(Carrier target) => action?.Execute(target);
	}
}
