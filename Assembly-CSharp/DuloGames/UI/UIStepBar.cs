using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005C8 RID: 1480
	[AddComponentMenu("UI/Bars/Step Bar")]
	public class UIStepBar : MonoBehaviour
	{
		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06002081 RID: 8321 RVA: 0x000A25F1 File Offset: 0x000A07F1
		// (set) Token: 0x06002082 RID: 8322 RVA: 0x000A25F9 File Offset: 0x000A07F9
		public int step
		{
			get
			{
				return this.m_CurrentStep;
			}
			set
			{
				this.GoToStep(value);
			}
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x000A2602 File Offset: 0x000A0802
		public virtual bool IsActive()
		{
			return base.enabled && base.gameObject.activeInHierarchy;
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x000A2619 File Offset: 0x000A0819
		protected virtual void Start()
		{
			this.UpdateBubble();
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x000A2621 File Offset: 0x000A0821
		public List<UIStepBar.StepFillInfo> GetOverrideFillList()
		{
			return this.m_OverrideFillList;
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x000A2629 File Offset: 0x000A0829
		public void SetOverrideFillList(List<UIStepBar.StepFillInfo> list)
		{
			this.m_OverrideFillList = list;
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x000A2634 File Offset: 0x000A0834
		public void ValidateOverrideFillList()
		{
			List<UIStepBar.StepFillInfo> list = new List<UIStepBar.StepFillInfo>();
			foreach (UIStepBar.StepFillInfo stepFillInfo in this.m_OverrideFillList.ToArray())
			{
				if (stepFillInfo.index > 1 && stepFillInfo.index <= this.m_StepsCount && stepFillInfo.amount > 0f)
				{
					list.Add(stepFillInfo);
				}
			}
			this.m_OverrideFillList = list;
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x000A269B File Offset: 0x000A089B
		protected virtual void OnRectTransformDimensionsChange()
		{
			if (!this.IsActive())
			{
				return;
			}
			this.UpdateGridProperties();
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x000A26AC File Offset: 0x000A08AC
		public void GoToStep(int step)
		{
			if (step < 0)
			{
				step = 0;
			}
			if (step > this.m_StepsCount)
			{
				step = this.m_StepsCount + 1;
			}
			this.m_CurrentStep = step;
			this.UpdateStepsProperties();
			this.UpdateFillImage();
			this.UpdateBubble();
		}

		// Token: 0x0600208A RID: 8330 RVA: 0x000A26E4 File Offset: 0x000A08E4
		public void UpdateFillImage()
		{
			if (this.m_FillImage == null)
			{
				return;
			}
			int num = this.m_OverrideFillList.FindIndex((UIStepBar.StepFillInfo x) => x.index == this.m_CurrentStep);
			this.m_FillImage.fillAmount = ((num >= 0) ? this.m_OverrideFillList[num].amount : this.GetStepFillAmount(this.m_CurrentStep));
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x000A2748 File Offset: 0x000A0948
		public void UpdateBubble()
		{
			if (this.m_BubbleRect == null)
			{
				return;
			}
			if (this.m_CurrentStep > 0 && this.m_CurrentStep <= this.m_StepsCount)
			{
				if (!this.m_BubbleRect.gameObject.activeSelf)
				{
					this.m_BubbleRect.gameObject.SetActive(true);
				}
				GameObject gameObject = this.m_StepsGameObjects[this.m_CurrentStep];
				if (gameObject != null)
				{
					RectTransform rectTransform = gameObject.transform as RectTransform;
					if (rectTransform.anchoredPosition.x != 0f)
					{
						this.m_BubbleRect.anchoredPosition = new Vector2(this.m_BubbleOffset.x + (rectTransform.anchoredPosition.x + rectTransform.rect.width / 2f), this.m_BubbleOffset.y);
					}
				}
				if (this.m_BubbleText != null)
				{
					this.m_BubbleText.text = this.m_CurrentStep.ToString();
					return;
				}
			}
			else if (this.m_BubbleRect.gameObject.activeSelf)
			{
				this.m_BubbleRect.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x000A286C File Offset: 0x000A0A6C
		public float GetStepFillAmount(int step)
		{
			return 1f / (float)(this.m_StepsCount + 1) * (float)step;
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x000A2880 File Offset: 0x000A0A80
		protected void CreateStepsGrid()
		{
			if (this.m_StepsGridGameObject != null)
			{
				return;
			}
			this.m_StepsGridGameObject = new GameObject("Steps Grid", new Type[]
			{
				typeof(RectTransform),
				typeof(GridLayoutGroup)
			});
			this.m_StepsGridGameObject.layer = base.gameObject.layer;
			this.m_StepsGridGameObject.transform.SetParent(base.transform, false);
			this.m_StepsGridGameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			this.m_StepsGridGameObject.transform.localPosition = Vector3.zero;
			this.m_StepsGridGameObject.transform.SetAsLastSibling();
			this.m_StepsGridRect = this.m_StepsGridGameObject.GetComponent<RectTransform>();
			this.m_StepsGridRect.sizeDelta = new Vector2(0f, 0f);
			this.m_StepsGridRect.anchorMin = new Vector2(0f, 0f);
			this.m_StepsGridRect.anchorMax = new Vector2(1f, 1f);
			this.m_StepsGridRect.pivot = new Vector2(0f, 1f);
			this.m_StepsGridRect.anchoredPosition = new Vector2(0f, 0f);
			this.m_StepsGrid = this.m_StepsGridGameObject.GetComponent<GridLayoutGroup>();
			if (this.m_BubbleRect != null)
			{
				this.m_BubbleRect.SetAsLastSibling();
			}
			this.m_StepsGameObjects.Clear();
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x000A2A0C File Offset: 0x000A0C0C
		public void UpdateGridProperties()
		{
			if (this.m_StepsGrid == null)
			{
				return;
			}
			int num = this.m_StepsCount + 2;
			if (!this.m_StepsGrid.padding.Equals(this.m_StepsGridPadding))
			{
				this.m_StepsGrid.padding = this.m_StepsGridPadding;
			}
			if (this.m_SeparatorAutoSize && this.m_SeparatorSprite != null)
			{
				this.m_SeparatorSize = new Vector2(this.m_SeparatorSprite.rect.width, this.m_SeparatorSprite.rect.height);
			}
			if (!this.m_StepsGrid.cellSize.Equals(this.m_SeparatorSize))
			{
				this.m_StepsGrid.cellSize = this.m_SeparatorSize;
			}
			float num2 = Mathf.Floor((this.m_StepsGridRect.rect.width - (float)this.m_StepsGridPadding.horizontal - (float)num * this.m_SeparatorSize.x) / (float)(num - 1) / 2f) * 2f;
			if (this.m_StepsGrid.spacing.x != num2)
			{
				this.m_StepsGrid.spacing = new Vector2(num2, 0f);
			}
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x000A2B3C File Offset: 0x000A0D3C
		public void RebuildSteps()
		{
			if (this.m_StepsGridGameObject == null)
			{
				return;
			}
			if (this.m_StepsGameObjects.Count == this.m_StepsCount + 2)
			{
				return;
			}
			this.DestroySteps();
			int num = this.m_StepsCount + 2;
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = new GameObject("Step " + i.ToString(), new Type[]
				{
					typeof(RectTransform)
				});
				gameObject.layer = base.gameObject.layer;
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.SetParent(this.m_StepsGridGameObject.transform, false);
				if (i > 0 && i < num - 1)
				{
					gameObject.AddComponent<Image>();
				}
				this.m_StepsGameObjects.Add(gameObject);
			}
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x000A2C30 File Offset: 0x000A0E30
		protected void UpdateStepsProperties()
		{
			foreach (GameObject gameObject in this.m_StepsGameObjects)
			{
				bool flag = this.m_StepsGameObjects.IndexOf(gameObject) + 1 <= this.m_CurrentStep;
				Image component = gameObject.GetComponent<Image>();
				if (component != null)
				{
					component.sprite = this.m_SeparatorSprite;
					component.overrideSprite = (flag ? this.m_SeparatorSpriteActive : null);
					component.color = this.m_SeparatorSpriteColor;
					component.rectTransform.pivot = new Vector2(0f, 1f);
				}
			}
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x000A2CEC File Offset: 0x000A0EEC
		protected void DestroySteps()
		{
			if (Application.isPlaying)
			{
				foreach (GameObject obj in this.m_StepsGameObjects)
				{
					UnityEngine.Object.Destroy(obj);
				}
			}
			this.m_StepsGameObjects.Clear();
		}

		// Token: 0x04001A57 RID: 6743
		[SerializeField]
		private List<GameObject> m_StepsGameObjects = new List<GameObject>();

		// Token: 0x04001A58 RID: 6744
		[SerializeField]
		private List<UIStepBar.StepFillInfo> m_OverrideFillList = new List<UIStepBar.StepFillInfo>();

		// Token: 0x04001A59 RID: 6745
		[SerializeField]
		private GameObject m_StepsGridGameObject;

		// Token: 0x04001A5A RID: 6746
		[SerializeField]
		private RectTransform m_StepsGridRect;

		// Token: 0x04001A5B RID: 6747
		[SerializeField]
		private GridLayoutGroup m_StepsGrid;

		// Token: 0x04001A5C RID: 6748
		[SerializeField]
		private int m_CurrentStep;

		// Token: 0x04001A5D RID: 6749
		[SerializeField]
		private int m_StepsCount = 1;

		// Token: 0x04001A5E RID: 6750
		[SerializeField]
		private RectOffset m_StepsGridPadding = new RectOffset();

		// Token: 0x04001A5F RID: 6751
		[SerializeField]
		private Sprite m_SeparatorSprite;

		// Token: 0x04001A60 RID: 6752
		[SerializeField]
		private Sprite m_SeparatorSpriteActive;

		// Token: 0x04001A61 RID: 6753
		[SerializeField]
		private Color m_SeparatorSpriteColor = Color.white;

		// Token: 0x04001A62 RID: 6754
		[SerializeField]
		private bool m_SeparatorAutoSize = true;

		// Token: 0x04001A63 RID: 6755
		[SerializeField]
		private Vector2 m_SeparatorSize = Vector2.zero;

		// Token: 0x04001A64 RID: 6756
		[SerializeField]
		private Image m_FillImage;

		// Token: 0x04001A65 RID: 6757
		[SerializeField]
		private RectTransform m_BubbleRect;

		// Token: 0x04001A66 RID: 6758
		[SerializeField]
		private Vector2 m_BubbleOffset = Vector2.zero;

		// Token: 0x04001A67 RID: 6759
		[SerializeField]
		private Text m_BubbleText;

		// Token: 0x020005C9 RID: 1481
		[Serializable]
		public struct StepFillInfo
		{
			// Token: 0x04001A68 RID: 6760
			public int index;

			// Token: 0x04001A69 RID: 6761
			public float amount;
		}
	}
}
