using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Unapparent {
	public class LevelUI : MonoBehaviour {
		[Header("Logue UI elements")]
		public RectTransform logue;
		GameObject logueOptionTemplate;
		public Image logueAvatar;
		public Text logueName;
		public Text logueText;
		public RectTransform logueOptions;

		[Header("Shapeshift UI elements")]
		public RectTransform shapeshift;
		GameObject shapeshiftOptionTemplate;

		static Sprite CreateSprite(Texture2D tex) => Sprite.Create(
			tex,
			new Rect(0, 0, tex.width, tex.height),
			Vector2.zero
		);

		static void ExitUIPanel(RectTransform parent) {
			foreach(Transform child in parent)
				Destroy(child.gameObject);
			parent.gameObject.SetActive(false);
		}

		void Start() {
			logueOptionTemplate = Resources.Load<GameObject>("UI/Logue Option");
			shapeshiftOptionTemplate = Resources.Load<GameObject>("UI/Shapeshift Option");
		}

		void OnApplicationQuit() {
			ExitUIPanel(shapeshift);
		}

		void AddShapeshiftOption(Identity id) {
			GameObject option = Instantiate(shapeshiftOptionTemplate);
			option.GetComponent<Image>().sprite = CreateSprite(id.portrait);
			option.GetComponentInChildren<Text>().text = id.name;
			option.transform.SetParent(shapeshift.transform);
			Button.ButtonClickedEvent ev = new Button.ButtonClickedEvent();
			ev.AddListener(() => {
				ExitUIPanel(shapeshift);
			});
			option.GetComponentInChildren<Button>().onClick = ev;
		}

		public void ShowShapeshift() {
			shapeshift.gameObject.SetActive(true);
			Level.current.protagonist.shapeshiftables.ForEach(AddShapeshiftOption);
		}

		void AddLogueOption(Character character, Monologue.Content.Option option, TaskCompletionSource<object> promise) {
			GameObject button = Instantiate(logueOptionTemplate);
			button.GetComponentInChildren<Text>().text = option.text;
			Button.ButtonClickedEvent ev = new Button.ButtonClickedEvent();
			ev.AddListener(() => {
				ExitUIPanel(logueOptions);
				Level.current.protagonist.canMoveActively = true;
				option.action?.Execute(character);
				promise.TrySetResult(null);
			});
			button.GetComponentInChildren<Button>().onClick = ev;
			button.transform.SetParent(logueOptions.gameObject.transform, false);
		}

		public async Task<object> ShowMonologue(Character character, Monologue.Content content) {
			if(character == null)
				return new NullReferenceException();
			logueName.text = character.Appearance.name;
			logueAvatar.sprite = CreateSprite(character.Appearance.avatar);
			logueText.text = content.text;
			TaskCompletionSource<object> promise = new TaskCompletionSource<object>();
			foreach(Monologue.Content.Option option in content.options)
				AddLogueOption(character, option, promise);
			logue.gameObject.SetActive(true);
			Level.current.protagonist.canMoveActively = false;
			return await promise.Task;
		}
	}
}