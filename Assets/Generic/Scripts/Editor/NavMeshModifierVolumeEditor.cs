using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine.AI;
using UnityEngine;

namespace UnityEditor.AI {
	[CanEditMultipleObjects]
	[CustomEditor(typeof(NavMeshModifierVolume))]
	class NavMeshModifierVolumeEditor : Editor {
		SerializedProperty m_AffectedAgents;
		SerializedProperty m_Area;
		SerializedProperty m_Center;
		SerializedProperty m_Size;

		static Color s_HandleColor = new Color(187f, 138f, 240f, 210f) / 255;
		static Color s_HandleColorDisabled = new Color(187f * 0.75f, 138f * 0.75f, 240f * 0.75f, 100f) / 255;

		BoxBoundsHandle m_BoundsHandle = new BoxBoundsHandle();

		bool editingCollider {
			get { return EditMode.editMode == EditMode.SceneViewEditMode.Collider && EditMode.IsOwner(this); }
		}

		void OnEnable() {
			m_AffectedAgents = serializedObject.FindProperty("m_AffectedAgents");
			m_Area = serializedObject.FindProperty("m_Area");
			m_Center = serializedObject.FindProperty("m_Center");
			m_Size = serializedObject.FindProperty("m_Size");

			NavMeshVisualizationSettings.showNavigation++;
		}

		void OnDisable() {
			NavMeshVisualizationSettings.showNavigation--;
		}

		Bounds GetBounds() {
			var navModifier = (NavMeshModifierVolume)target;
			return new Bounds(navModifier.transform.position, navModifier.size);
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			EditMode.DoEditModeInspectorModeButton(EditMode.SceneViewEditMode.Collider, "Edit Volume",
				EditorGUIUtility.IconContent("EditCollider"), GetBounds, this);

			EditorGUILayout.PropertyField(m_Size);
			EditorGUILayout.PropertyField(m_Center);

			NavMeshComponentsGUIUtility.AreaPopup("Area Type", m_Area);
			NavMeshComponentsGUIUtility.AgentMaskPopup("Affected Agents", m_AffectedAgents);
			EditorGUILayout.Space();

			serializedObject.ApplyModifiedProperties();
		}

		void OnSceneGUI() {
			if(!editingCollider)
				return;

			var vol = (NavMeshModifierVolume)target;
			var color = vol.enabled ? s_HandleColor : s_HandleColorDisabled;
			using(new Handles.DrawingScope(color, vol.transform.localToWorldMatrix)) {
				m_BoundsHandle.center = vol.center;
				m_BoundsHandle.size = vol.size;

				EditorGUI.BeginChangeCheck();
				m_BoundsHandle.DrawHandle();
				if(EditorGUI.EndChangeCheck()) {
					Undo.RecordObject(vol, "Modified NavMesh Modifier Volume");
					Vector3 center = m_BoundsHandle.center;
					Vector3 size = m_BoundsHandle.size;
					vol.center = center;
					vol.size = size;
					EditorUtility.SetDirty(target);
				}
			}
		}

		[MenuItem("GameObject/AI/NavMesh Modifier Volume", false, 2001)]
		static public void CreateNavMeshModifierVolume(MenuCommand menuCommand) {
			var parent = menuCommand.context as GameObject;
			var go = NavMeshComponentsGUIUtility.CreateAndSelectGameObject("NavMesh Modifier Volume", parent);
			go.AddComponent<NavMeshModifierVolume>();
			var view = SceneView.lastActiveSceneView;
			if(view != null)
				view.MoveToView(go.transform);
		}
	}
}
