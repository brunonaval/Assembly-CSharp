using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005DE RID: 1502
	[AddComponentMenu("UI/Slider Extended", 58)]
	public class UISliderExtended : Slider
	{
		// Token: 0x17000319 RID: 793
		// (get) Token: 0x0600211A RID: 8474 RVA: 0x000A5F12 File Offset: 0x000A4112
		// (set) Token: 0x0600211B RID: 8475 RVA: 0x000A5F1A File Offset: 0x000A411A
		public List<string> options
		{
			get
			{
				return this.m_Options;
			}
			set
			{
				this.m_Options = value;
				this.RebuildOptions();
				this.ValidateOptions();
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x0600211C RID: 8476 RVA: 0x000A5F2F File Offset: 0x000A412F
		public GameObject selectedOptionGameObject
		{
			get
			{
				return this.m_CurrentOptionGameObject;
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x0600211D RID: 8477 RVA: 0x000A5F38 File Offset: 0x000A4138
		public int selectedOptionIndex
		{
			get
			{
				int num = Mathf.RoundToInt(this.value);
				if (num < 0 || num >= this.m_Options.Count)
				{
					return 0;
				}
				return num;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x0600211E RID: 8478 RVA: 0x000A5F66 File Offset: 0x000A4166
		// (set) Token: 0x0600211F RID: 8479 RVA: 0x000A5F6E File Offset: 0x000A416E
		public RectOffset optionsPadding
		{
			get
			{
				return this.m_OptionsPadding;
			}
			set
			{
				this.m_OptionsPadding = value;
			}
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x000A5F77 File Offset: 0x000A4177
		public bool HasOptions()
		{
			return this.m_Options != null && this.m_Options.Count > 0;
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x000A5F91 File Offset: 0x000A4191
		protected override void OnEnable()
		{
			base.OnEnable();
			this.ValidateOptions();
			base.onValueChanged.AddListener(new UnityAction<float>(this.OnValueChanged));
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x000A5FB6 File Offset: 0x000A41B6
		protected override void OnDisable()
		{
			base.OnDisable();
			base.onValueChanged.RemoveListener(new UnityAction<float>(this.OnValueChanged));
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x000A5FD5 File Offset: 0x000A41D5
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			if (!this.IsActive())
			{
				return;
			}
			this.UpdateGridProperties();
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x000A5FEC File Offset: 0x000A41EC
		public void OnValueChanged(float value)
		{
			if (!this.IsActive() || !this.HasOptions())
			{
				return;
			}
			if (this.m_OptionTransition == UISliderExtended.OptionTransition.ColorTint)
			{
				Graphic target = (this.m_OptionTransitionTarget == UISliderExtended.TransitionTarget.Text) ? this.m_CurrentOptionGameObject.GetComponentInChildren<Text>() : this.m_CurrentOptionGameObject.GetComponent<Image>();
				this.StartColorTween(target, this.m_OptionTransitionColorNormal * this.m_OptionTransitionMultiplier, this.m_OptionTransitionDuration);
				int num = Mathf.RoundToInt(value);
				if (num < 0 || num >= this.m_Options.Count)
				{
					num = 0;
				}
				GameObject gameObject = this.m_OptionGameObjects[num];
				if (gameObject != null)
				{
					Graphic target2 = (this.m_OptionTransitionTarget == UISliderExtended.TransitionTarget.Text) ? gameObject.GetComponentInChildren<Text>() : gameObject.GetComponent<Image>();
					this.StartColorTween(target2, this.m_OptionTransitionColorActive * this.m_OptionTransitionMultiplier, this.m_OptionTransitionDuration);
				}
				this.m_CurrentOptionGameObject = gameObject;
			}
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x000A60C4 File Offset: 0x000A42C4
		private void StartColorTween(Graphic target, Color targetColor, float duration)
		{
			if (target == null)
			{
				return;
			}
			if (!Application.isPlaying || duration == 0f)
			{
				target.canvasRenderer.SetColor(targetColor);
				return;
			}
			target.CrossFadeColor(targetColor, duration, true, true);
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x000A60F8 File Offset: 0x000A42F8
		protected void ValidateOptions()
		{
			if (!this.IsActive())
			{
				return;
			}
			if (!this.HasOptions())
			{
				if (this.m_OptionsContGameObject != null)
				{
					if (Application.isPlaying)
					{
						UnityEngine.Object.Destroy(this.m_OptionsContGameObject);
						return;
					}
					UnityEngine.Object.DestroyImmediate(this.m_OptionsContGameObject);
				}
				return;
			}
			if (this.m_OptionsContGameObject == null)
			{
				this.CreateOptionsContainer();
			}
			if (!base.wholeNumbers)
			{
				base.wholeNumbers = true;
			}
			base.minValue = 0f;
			base.maxValue = (float)this.m_Options.Count - 1f;
			this.UpdateGridProperties();
			this.UpdateOptionsProperties();
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x000A6198 File Offset: 0x000A4398
		public void UpdateGridProperties()
		{
			if (this.m_OptionsContGrid == null)
			{
				return;
			}
			if (!this.m_OptionsContGrid.padding.Equals(this.m_OptionsPadding))
			{
				this.m_OptionsContGrid.padding = this.m_OptionsPadding;
			}
			Vector2 vector = (this.m_OptionSprite != null) ? new Vector2(this.m_OptionSprite.rect.width, this.m_OptionSprite.rect.height) : Vector2.zero;
			if (!this.m_OptionsContGrid.cellSize.Equals(vector))
			{
				this.m_OptionsContGrid.cellSize = vector;
			}
			float num = (this.m_OptionsContRect.rect.width - ((float)this.m_OptionsPadding.left + (float)this.m_OptionsPadding.right) - (float)this.m_Options.Count * vector.x) / ((float)this.m_Options.Count - 1f);
			if (this.m_OptionsContGrid.spacing.x != num)
			{
				this.m_OptionsContGrid.spacing = new Vector2(num, 0f);
			}
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x000A62C0 File Offset: 0x000A44C0
		public void UpdateOptionsProperties()
		{
			if (!this.HasOptions())
			{
				return;
			}
			int num = 0;
			foreach (GameObject gameObject in this.m_OptionGameObjects)
			{
				bool flag = Mathf.RoundToInt(this.value) == num;
				if (flag)
				{
					this.m_CurrentOptionGameObject = gameObject;
				}
				Image component = gameObject.GetComponent<Image>();
				if (component != null)
				{
					component.sprite = this.m_OptionSprite;
					component.rectTransform.pivot = new Vector2(0f, 1f);
					if (this.m_OptionTransition == UISliderExtended.OptionTransition.ColorTint && this.m_OptionTransitionTarget == UISliderExtended.TransitionTarget.Image)
					{
						component.canvasRenderer.SetColor(flag ? this.m_OptionTransitionColorActive : this.m_OptionTransitionColorNormal);
					}
					else
					{
						component.canvasRenderer.SetColor(Color.white);
					}
				}
				Text componentInChildren = gameObject.GetComponentInChildren<Text>();
				if (componentInChildren != null)
				{
					componentInChildren.font = this.m_OptionTextFont;
					componentInChildren.fontStyle = this.m_OptionTextStyle;
					componentInChildren.fontSize = this.m_OptionTextSize;
					componentInChildren.color = this.m_OptionTextColor;
					if (this.m_OptionTransition == UISliderExtended.OptionTransition.ColorTint && this.m_OptionTransitionTarget == UISliderExtended.TransitionTarget.Text)
					{
						componentInChildren.canvasRenderer.SetColor(flag ? this.m_OptionTransitionColorActive : this.m_OptionTransitionColorNormal);
					}
					else
					{
						componentInChildren.canvasRenderer.SetColor(Color.white);
					}
					(componentInChildren.transform as RectTransform).anchoredPosition = this.m_OptionTextOffset;
					this.UpdateTextEffect(componentInChildren.gameObject);
				}
				num++;
			}
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x000A6470 File Offset: 0x000A4670
		protected void RebuildOptions()
		{
			if (!this.HasOptions())
			{
				return;
			}
			if (this.m_OptionsContGameObject == null)
			{
				this.CreateOptionsContainer();
			}
			this.DestroyOptions();
			int num = 0;
			foreach (string text in this.m_Options)
			{
				GameObject gameObject = new GameObject("Option " + num.ToString(), new Type[]
				{
					typeof(RectTransform),
					typeof(Image)
				});
				gameObject.layer = base.gameObject.layer;
				gameObject.transform.SetParent(this.m_OptionsContGameObject.transform, false);
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localPosition = Vector3.zero;
				GameObject gameObject2 = new GameObject("Text", new Type[]
				{
					typeof(RectTransform)
				});
				gameObject2.layer = base.gameObject.layer;
				gameObject2.transform.SetParent(gameObject.transform, false);
				gameObject2.transform.localScale = Vector3.one;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.AddComponent<Text>().text = text;
				ContentSizeFitter contentSizeFitter = gameObject2.AddComponent<ContentSizeFitter>();
				contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
				contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
				this.m_OptionGameObjects.Add(gameObject);
				num++;
			}
			this.UpdateOptionsProperties();
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x000A6608 File Offset: 0x000A4808
		private void AddTextEffect(GameObject gObject)
		{
			if (this.m_OptionTextEffect == UISliderExtended.TextEffectType.Shadow)
			{
				Shadow shadow = gObject.AddComponent<Shadow>();
				shadow.effectColor = this.m_OptionTextEffectColor;
				shadow.effectDistance = this.m_OptionTextEffectDistance;
				shadow.useGraphicAlpha = this.m_OptionTextEffectUseGraphicAlpha;
				return;
			}
			if (this.m_OptionTextEffect == UISliderExtended.TextEffectType.Outline)
			{
				Outline outline = gObject.AddComponent<Outline>();
				outline.effectColor = this.m_OptionTextEffectColor;
				outline.effectDistance = this.m_OptionTextEffectDistance;
				outline.useGraphicAlpha = this.m_OptionTextEffectUseGraphicAlpha;
			}
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x000A667C File Offset: 0x000A487C
		private void UpdateTextEffect(GameObject gObject)
		{
			if (this.m_OptionTextEffect == UISliderExtended.TextEffectType.Shadow)
			{
				Shadow shadow = gObject.GetComponent<Shadow>();
				if (shadow == null)
				{
					shadow = gObject.AddComponent<Shadow>();
				}
				shadow.effectColor = this.m_OptionTextEffectColor;
				shadow.effectDistance = this.m_OptionTextEffectDistance;
				shadow.useGraphicAlpha = this.m_OptionTextEffectUseGraphicAlpha;
				return;
			}
			if (this.m_OptionTextEffect == UISliderExtended.TextEffectType.Outline)
			{
				Outline outline = gObject.GetComponent<Outline>();
				if (outline == null)
				{
					outline = gObject.AddComponent<Outline>();
				}
				outline.effectColor = this.m_OptionTextEffectColor;
				outline.effectDistance = this.m_OptionTextEffectDistance;
				outline.useGraphicAlpha = this.m_OptionTextEffectUseGraphicAlpha;
			}
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x000A6714 File Offset: 0x000A4914
		public void RebuildTextEffects()
		{
			foreach (GameObject gameObject in this.m_OptionGameObjects)
			{
				Text componentInChildren = gameObject.GetComponentInChildren<Text>();
				if (componentInChildren != null)
				{
					Shadow component = componentInChildren.gameObject.GetComponent<Shadow>();
					Outline component2 = componentInChildren.gameObject.GetComponent<Outline>();
					if (Application.isPlaying)
					{
						if (component != null)
						{
							UnityEngine.Object.Destroy(component);
						}
						if (component2 != null)
						{
							UnityEngine.Object.Destroy(component2);
						}
					}
					else
					{
						if (component != null)
						{
							UnityEngine.Object.DestroyImmediate(component);
						}
						if (component2 != null)
						{
							UnityEngine.Object.DestroyImmediate(component2);
						}
					}
					this.AddTextEffect(componentInChildren.gameObject);
				}
			}
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x000A67DC File Offset: 0x000A49DC
		protected void DestroyOptions()
		{
			foreach (GameObject obj in this.m_OptionGameObjects)
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(obj);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(obj);
				}
			}
			this.m_OptionGameObjects.Clear();
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x000A6848 File Offset: 0x000A4A48
		protected void CreateOptionsContainer()
		{
			this.m_OptionsContGameObject = new GameObject("Options Grid", new Type[]
			{
				typeof(RectTransform),
				typeof(GridLayoutGroup)
			});
			this.m_OptionsContGameObject.layer = base.gameObject.layer;
			this.m_OptionsContGameObject.transform.SetParent(base.transform, false);
			this.m_OptionsContGameObject.transform.SetAsFirstSibling();
			this.m_OptionsContGameObject.transform.localScale = Vector3.one;
			this.m_OptionsContGameObject.transform.localPosition = Vector3.zero;
			this.m_OptionsContRect = this.m_OptionsContGameObject.GetComponent<RectTransform>();
			this.m_OptionsContRect.sizeDelta = new Vector2(0f, 0f);
			this.m_OptionsContRect.anchorMin = new Vector2(0f, 0f);
			this.m_OptionsContRect.anchorMax = new Vector2(1f, 1f);
			this.m_OptionsContRect.pivot = new Vector2(0f, 1f);
			this.m_OptionsContRect.anchoredPosition = new Vector2(0f, 0f);
			this.m_OptionsContGrid = this.m_OptionsContGameObject.GetComponent<GridLayoutGroup>();
		}

		// Token: 0x04001AF7 RID: 6903
		[SerializeField]
		private List<string> m_Options = new List<string>();

		// Token: 0x04001AF8 RID: 6904
		[SerializeField]
		private List<GameObject> m_OptionGameObjects = new List<GameObject>();

		// Token: 0x04001AF9 RID: 6905
		[SerializeField]
		private GameObject m_OptionsContGameObject;

		// Token: 0x04001AFA RID: 6906
		[SerializeField]
		private RectTransform m_OptionsContRect;

		// Token: 0x04001AFB RID: 6907
		[SerializeField]
		private GridLayoutGroup m_OptionsContGrid;

		// Token: 0x04001AFC RID: 6908
		[SerializeField]
		private RectOffset m_OptionsPadding = new RectOffset();

		// Token: 0x04001AFD RID: 6909
		[SerializeField]
		private Sprite m_OptionSprite;

		// Token: 0x04001AFE RID: 6910
		[SerializeField]
		private Font m_OptionTextFont;

		// Token: 0x04001AFF RID: 6911
		[SerializeField]
		private FontStyle m_OptionTextStyle = FontData.defaultFontData.fontStyle;

		// Token: 0x04001B00 RID: 6912
		[SerializeField]
		private int m_OptionTextSize = FontData.defaultFontData.fontSize;

		// Token: 0x04001B01 RID: 6913
		[SerializeField]
		private Color m_OptionTextColor = Color.white;

		// Token: 0x04001B02 RID: 6914
		[SerializeField]
		private UISliderExtended.TextEffectType m_OptionTextEffect;

		// Token: 0x04001B03 RID: 6915
		[SerializeField]
		private Color m_OptionTextEffectColor = new Color(0f, 0f, 0f, 128f);

		// Token: 0x04001B04 RID: 6916
		[SerializeField]
		private Vector2 m_OptionTextEffectDistance = new Vector2(1f, -1f);

		// Token: 0x04001B05 RID: 6917
		[SerializeField]
		private bool m_OptionTextEffectUseGraphicAlpha = true;

		// Token: 0x04001B06 RID: 6918
		[SerializeField]
		private Vector2 m_OptionTextOffset = Vector2.zero;

		// Token: 0x04001B07 RID: 6919
		[SerializeField]
		private UISliderExtended.OptionTransition m_OptionTransition;

		// Token: 0x04001B08 RID: 6920
		[SerializeField]
		private UISliderExtended.TransitionTarget m_OptionTransitionTarget = UISliderExtended.TransitionTarget.Text;

		// Token: 0x04001B09 RID: 6921
		[SerializeField]
		private Color m_OptionTransitionColorNormal = ColorBlock.defaultColorBlock.normalColor;

		// Token: 0x04001B0A RID: 6922
		[SerializeField]
		private Color m_OptionTransitionColorActive = ColorBlock.defaultColorBlock.highlightedColor;

		// Token: 0x04001B0B RID: 6923
		[SerializeField]
		[Range(1f, 6f)]
		private float m_OptionTransitionMultiplier = 1f;

		// Token: 0x04001B0C RID: 6924
		[SerializeField]
		private float m_OptionTransitionDuration = 0.1f;

		// Token: 0x04001B0D RID: 6925
		private GameObject m_CurrentOptionGameObject;

		// Token: 0x020005DF RID: 1503
		public enum TextEffectType
		{
			// Token: 0x04001B0F RID: 6927
			None,
			// Token: 0x04001B10 RID: 6928
			Shadow,
			// Token: 0x04001B11 RID: 6929
			Outline
		}

		// Token: 0x020005E0 RID: 1504
		public enum OptionTransition
		{
			// Token: 0x04001B13 RID: 6931
			None,
			// Token: 0x04001B14 RID: 6932
			ColorTint
		}

		// Token: 0x020005E1 RID: 1505
		public enum TransitionTarget
		{
			// Token: 0x04001B16 RID: 6934
			Image,
			// Token: 0x04001B17 RID: 6935
			Text
		}
	}
}
