using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Unapparent {
	/// <summary>
	/// 轨道是人物可以运动的轨迹
	/// 轨道由多个节点组成
	/// </summary>
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	[Serializable]
	public class Track : MonoBehaviour {
		/// <summary>
		/// 节点规定了一段直线轨道的位置和属性
		/// </summary>
		[Serializable]
		public struct Node {
			public enum PathType {
				Level,
				UpStair,
				DownStair,
				LeftLadder,
				RightLadder
			}
			public Vector2 position;
			/// <summary>
			/// 路径种类决定了角色在上面运动时播放的动画
			/// </summary>
			public PathType pathType;
			public static Dictionary<PathType, Color> pathGizmosColor = new Dictionary<PathType, Color> {
				{ PathType.Level, new Color(0f, 1f, 0f) },
				{ PathType.UpStair, new Color(1f, 1f, 0f) },
				{ PathType.DownStair, new Color(0f, 1f, 1f) },
				{ PathType.LeftLadder, new Color(1f, 0f, 0f) },
				{ PathType.RightLadder, new Color(0f, 0f, 1f) },
			};
		}

		public float zLayer;
		public Node[] nodes;
		public float[] distanceList;

		public Track() {
			zLayer = -.1f;
			nodes = new Node[2];
			nodes[0].position = new Vector2(-1, 0);
			nodes[1].position = new Vector2(1, 0);
		}

		private Vector3 NodePosition(int index) =>
			new Vector3(nodes[index].position.x, nodes[index].position.y, zLayer);

		private Vector3 InterPosition(int index, float distance) =>
			NodePosition(index) + (NodePosition(index + 1) - NodePosition(index)) *
			((distance - distanceList[index]) / (distanceList[index + 1] - distanceList[index]));

		/// <summary>
		/// 使用离节点的距离获取世界坐标下的位置
		/// </summary>
		/// <param name="distance">离初始点的距离</param>
		/// <param name="index">用为基准位置的节点索引</param>
		/// <returns>世界坐标下的位置</returns>
		public Vector3 GetPosition(float distance, int index = 0) {
			int size = nodes.Length;
			if(size == 0)
				return new Vector3(0, 0, zLayer);
			if(index < 0)
				return NodePosition(0);
			if(index >= size)
				return NodePosition(size - 1);
			if(distance < distanceList[index]) for(int i = index - 1; i >= 0; i--) {
				if(distance >= distanceList[i])
					return InterPosition(i, distance);
			} else for(int i = index + 1; i < size; i++) {
				if(distance < distanceList[i])
					return InterPosition(i - 1, distance);
			}
			return NodePosition(size - 1);
		}

		public void OnValidate() {
			distanceList = new float[nodes.Length];
			distanceList[0] = 0f;
			for(int i = 1, size = nodes.Length; i < size; i++)
				distanceList[i] = distanceList[i - 1] + (nodes[i].position - nodes[i - 1].position).magnitude;
		}

		void DrawGizmo() {
			if(nodes.Length == 0)
				return;
			for(int i = 1; i < nodes.Length; ++i) {
				Node node = nodes[i - 1];
				Vector3 root = node.position;
				Vector3 direction = (Vector3)nodes[i].position - root;
				root += gameObject.transform.position;
				Color color = Node.pathGizmosColor[node.pathType];
				Debug.DrawRay(root, direction, color);
			}
		}

		public void Update() {
			if(Application.isEditor) {
				DrawGizmo();
			}
		}
	}

	[CustomEditor(typeof(Track))]
	public class TrackEditor : Editor {
		private ReorderableList nodeList;

		private void OnEnable() {
			nodeList = new ReorderableList(serializedObject,
				serializedObject.FindProperty("nodes"),
				true, false, true, true);

			//定义元素的高度
			nodeList.elementHeight = 40;

			//自定义绘制列表元素
			nodeList.drawElementCallback = (Rect rect, int index, bool selected, bool focused) => {
				//根据index获取对应元素 
				SerializedProperty item = nodeList.serializedProperty.GetArrayElementAtIndex(index);
				rect.height = 20;
				rect.y += 2;
				EditorGUI.PropertyField(rect, item.FindPropertyRelative("position"), new GUIContent("Node " + index));
				if(index != nodeList.count - 1) {
					rect.y += 20;
					EditorGUI.PropertyField(rect, item.FindPropertyRelative("pathType"), GUIContent.none);
				}
			};
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("zLayer"), new GUIContent("Z coordinate"));
			nodeList.DoLayoutList();
			serializedObject.ApplyModifiedProperties();
		}
	}
}
