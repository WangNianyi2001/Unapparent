using UnityEditor;

namespace Unapparent {
	public class CharacterProfile : ManagedAsset {
		public new string name;

		[MenuItem("Assets/Create/Unapparent/CharacterProfile", false, 2)]
		public static void CreateCharacterProfile() {
			CharacterProfile so = CreateInstance<CharacterProfile>();
			CreateAsset(so, "CharacterProfile");
			EditorGUIUtility.PingObject(so);
		}
	}
}
