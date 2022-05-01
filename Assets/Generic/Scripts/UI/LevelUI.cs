using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Unapparent {
	public class LevelUI : MonoBehaviour {
		[Header("Logue UI elements")]
		public RectTransform logue;
		GameObject logueOptionTemplate;
		public RawImage logueAvatar;
		public Text logueName;
		public Text logueText;
		public RectTransform logueOptions;

		[Header("Shapeshift UI elements")]
		public RectTransform shapeshift;
		GameObject shapeshiftOptionTemplate;

		static void ClearUIChildren(RectTransform parent) {
			foreach(Transform child in parent)
				Destroy(child.gameObject);
		}

		void Start() {
			logueOptionTemplate = Resources.Load<GameObject>("UI/Logue Option");
			shapeshiftOptionTemplate = Resources.Load<GameObject>("UI/Shapeshift Option");
		}

		void AddShapeshiftOption(TaskCompletionSource<object> promise, Identity id) {
			GameObject option = Instantiate(shapeshiftOptionTemplate);
			option.GetComponent<RawImage>().texture = id.portrait;
			option.GetComponentInChildren<Text>().text = id.name;
			option.transform.SetParent(shapeshift.transform);
			Button.ButtonClickedEvent ev = new Button.ButtonClickedEvent();
			ev.AddListener(() => {
				ClearUIChildren(shapeshift);
				shapeshift.gameObject.SetActive(false);
				Level.current.protagonist.canMoveActively = true;
				Level.current.protagonist.Appearance = id;
				promise.TrySetResult(null);
			});
			option.GetComponentInChildren<Button>().onClick = ev;
		}

		public Task<object> ShowShapeshift() {
			TaskCompletionSource<object> promise = new TaskCompletionSource<object>();
			foreach(Identity id in Level.current.protagonist.shapeshiftables)
				AddShapeshiftOption(promise, id);
			shapeshift.gameObject.SetActive(true);
			LayoutRebuilder.ForceRebuildLayoutImmediate(shapeshift);
			Level.current.protagonist.canMoveActively = false;
			return promise.Task;
		}

		void AddLogueOption(TaskCompletionSource<object> promise, Character character, Monologue.Content.Option option) {
			GameObject button = Instantiate(logueOptionTemplate);
			button.GetComponentInChildren<Text>().text = option.text;
			Button.ButtonClickedEvent ev = new Button.ButtonClickedEvent();
			ev.AddListener(() => {
				ClearUIChildren(logueOptions);
				logue.gameObject.SetActive(false);
				Level.current.protagonist.canMoveActively = true;
				option.action?.Execute(character);
				promise.TrySetResult(null);
			});
			button.GetComponentInChildren<Button>().onClick = ev;
			button.transform.SetParent(logueOptions.gameObject.transform, false);
		}

		public async Task<object> ShowMonologue(Character character, Monologue.Content content) {
			logueName.text = character.Appearance.name;
			logueAvatar.texture = character.Appearance.avatar;
			logueText.text = content.text;
			TaskCompletionSource<object> promise = new TaskCompletionSource<object>();
			foreach(Monologue.Content.Option option in content.options)
				AddLogueOption(promise, character, option);
			logue.gameObject.SetActive(true);
			LayoutRebuilder.ForceRebuildLayoutImmediate(logue);
			Level.current.protagonist.canMoveActively = false;
			return await promise.Task;
		}
	}
}