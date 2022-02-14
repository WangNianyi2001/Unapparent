using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

namespace Unapparent {
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	[Serializable]
	public class Track : MonoBehaviour {
		[Serializable]
		public class Segment {
			public readonly Node from = null, to = null;
			public Segment(Node from, Node to) {
				this.from = from;
				if(from != null)
					from.next = this;
				this.to = to;
				if(to != null)
					to.next = this;
			}
		}

		[Serializable]
		public class Node {
			[HideInInspector] public Track track = null;
			[SerializeField] [ReadOnly] public GameObject gameObject = null;
			[HideInInspector] public Segment prev = null, next = null;

			public Node(Track track) {
				this.track = track;
				prev = new Segment(null, this);
				next = new Segment(this, null);
			}

			public Vector3 LocalPos => gameObject.transform.localPosition;
			public Vector3 Pos => gameObject.transform.position;
		}

		[HideInInspector]
		public List<Node> nodes = new List<Node>();

		public void InsertNode(Node from) {
			Node node = new Node(this);
			GameObject obj = new GameObject();
			obj.name = "Node #" + nodes.Count;
			obj.transform.parent = gameObject.transform;
			obj.transform.localPosition = from == null ? Vector3.zero : (from.LocalPos + Vector3.right);
			node.gameObject = obj;
			if(from != null) {
				new Segment(node, from.next.to);
				new Segment(from, node);
			}
			nodes.Add(node);
		}

		public void RemoveNode(Node node) {
			if(node.prev != null) {
				new Segment(node.prev.from, null);
				node.prev = null;
			}
			if(node.next != null) {
				new Segment(null, node.next.to);
				node.next = null;
			}
			if(Application.isEditor) DestroyImmediate(node.gameObject);
			else Destroy(node.gameObject);
			nodes.Remove(node);
		}

		public void InsertNodeAtEnd() => InsertNode(nodes.Count == 0 ? null : nodes.Last());

		public void RemoveNodeAtEnd() { if(nodes.Count != 0) RemoveNode(nodes.Last()); }

		void DrawGizmo() {
			if(nodes.Count == 0)
				return;
			for(int i = 1; i < nodes.Count; ++i) {
				Node node = nodes[i - 1], next = nodes[i];
				Debug.DrawRay(node.Pos, next.LocalPos - node.LocalPos, Color.green);
			}
		}

		public void Update() {
			if(Application.isEditor) {
				DrawGizmo();
			}
		}
	}

	[CustomEditor(typeof(Track))]
	public class TrackEditor : Inspector<Track> {
		ReorderableList nodes;

		void OnEnable() {
			nodes = new ReorderableList(serializedObject,
				serializedObject.FindProperty("nodes"),
				false, false, true, true);

			nodes.drawElementCallback = (Rect rect, int index, bool selected, bool focused) => {
				SerializedProperty item = nodes.serializedProperty.GetArrayElementAtIndex(index);
				EditorGUI.BeginDisabledGroup(true);
				EditorGUI.ObjectField(rect, item.FindPropertyRelative("gameObject"), new GUIContent(index.ToString()));
				EditorGUI.EndDisabledGroup();
			};

			nodes.onAddCallback = (ReorderableList list) => target.InsertNodeAtEnd();

			nodes.onRemoveCallback = (ReorderableList list) => target.RemoveNodeAtEnd();
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();
			DrawDefaultInspector();
			nodes.DoLayoutList();
			serializedObject.ApplyModifiedProperties();
		}
	}
}
