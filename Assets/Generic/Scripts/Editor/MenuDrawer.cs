using UnityEditor;
using UnityEngine;

namespace Unapparent {
	public class MenuDrawer<T> : PropertyDrawer {
		Menu<T> target;

		public MenuDrawer(Menu<T> target) => this.target = target;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
			=> EditorGUIUtility.singleLineHeight;

		public void AddTo(GenericMenu menu) {
			string path = "";
			foreach(Menu<T>.Entry entry in target) {
				if(entry.text != null) {
					if(string.IsNullOrEmpty(entry.text)) {
						path = "";
						continue;
					}
					path = entry.text + "/";
					menu.AddSeparator(path);
				} else {
					GUIContent content = new GUIContent(path + target.OnLabelize(entry.target));
					menu.AddItem(content, false, target.SelectCallback, entry.target);
				}
			}
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			if(!EditorGUI.DropdownButton(position, label, FocusType.Keyboard))
				return;
			if(target == null) {
				Debug.LogWarning("Failed to show menu");
				return;
			}
			GenericMenu menu = new GenericMenu();
			AddTo(menu);
			menu.ShowAsContext();
		}
	}
}
