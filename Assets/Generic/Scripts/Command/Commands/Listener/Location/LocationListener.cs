namespace Unapparent {
	public abstract class LocationListener : Listener {
		public Location location;

		public override bool Validate(Carrier target, params object[] args) {
			if(args.Length < 1)
				return false;
			return location == args[0] as Location;
		}
	}
}
