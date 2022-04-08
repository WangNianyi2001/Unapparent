namespace Unapparent {
	public class ReceiveMessage : Listener {
		public string message;

		public override bool Validate(Carrier target, params object[] args) {
			if(args.Length < 1)
				return false;
			return message == (args[0] as string);
		}
	}
}
