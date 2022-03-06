using UnityEditor;

namespace Unapparent {
	public class LogueProfile : ManagedAsset {
		public new string name;

		[MenuItem("Assets/Create/Unapparent/LogueProfile", false, 2)]
		public static void CreateLogueProfile() {
			LogueProfile so = CreateInstance<LogueProfile>();
			CreateAsset(so, "LogueProfile");
			EditorGUIUtility.PingObject(so);
		}
	}
}
