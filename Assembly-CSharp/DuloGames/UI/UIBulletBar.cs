using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005BC RID: 1468
	[AddComponentMenu("UI/Bars/Bullet Bar")]
	[RequireComponent(typeof(RectTransform))]
	public class UIBulletBar : UIBehaviour, IUIProgressBar
	{
		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x0600203E RID: 8254 RVA: 0x000A148B File Offset: 0x0009F68B
		// (set) Token: 0x0600203F RID: 8255 RVA: 0x000A1493 File Offset: 0x0009F693
		public float fillAmount
		{
			get
			{
				return this.m_FillAmount;
			}
			set
			{
				this.m_FillAmount = Mathf.Clamp01(value);
				this.UpdateFill();
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06002040 RID: 8256 RVA: 0x000A14A7 File Offset: 0x0009F6A7
		// (set) Token: 0x06002041 RID: 8257 RVA: 0x000A14AF File Offset: 0x0009F6AF
		public bool invertFill
		{
			get
			{
				return this.m_InvertFill;
			}
			set
			{
				this.m_InvertFill = value;
				this.UpdateFill();
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06002042 RID: 8258 RVA: 0x000A14BE File Offset: 0x0009F6BE
		public RectTransform rectTransform
		{
			get
			{
				return base.transform as RectTransform;
			}
		}

		// Token: 0x06002043 RID: 8259 RVA: 0x000A14CC File Offset: 0x0009F6CC
		protected override void Start()
		{
			base.Start();
			if (this.m_BulletSprite == null || this.m_BulletSpriteActive == null)
			{
				Debug.LogWarning("The Bullet Bar script on game object " + base.gameObject.name + " requires that both bullet sprites are assigned to work.");
				base.enabled = false;
				return;
			}
			if (this.m_BulletsContainer == null)
			{
				this.ConstructBullets();
			}
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x000A1538 File Offset: 0x0009F738
		public void UpdateFill()
		{
			if (!base.isActiveAndEnabled || this.m_FillBullets == null || this.m_FillBullets.Count == 0)
			{
				return;
			}
			GameObject[] array = this.m_FillBullets.ToArray();
			if (this.m_InvertFill)
			{
				Array.Reverse<GameObject>(array);
			}
			int num = 0;
			foreach (GameObject gameObject in array)
			{
				float num2 = (float)num / (float)this.m_BulletCount;
				Image component = gameObject.GetComponent<Image>();
				if (component != null)
				{
					component.enabled = (this.m_FillAmount > 0f && num2 <= this.m_FillAmount);
				}
				num++;
			}
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x000A15D8 File Offset: 0x0009F7D8
		public void ConstructBullets()
		{
			if (this.m_BulletSprite == null || this.m_BulletSpriteActive == null || !base.isActiveAndEnabled)
			{
				return;
			}
			this.DestroyBullets();
			this.m_BulletsContainer = new GameObject("Bullets", new Type[]
			{
				typeof(RectTransform)
			});
			this.m_BulletsContainer.transform.SetParent(base.transform);
			this.m_BulletsContainer.layer = base.gameObject.layer;
			RectTransform rectTransform = this.m_BulletsContainer.transform as RectTransform;
			rectTransform.localScale = new Vector3(1f, 1f, 1f);
			rectTransform.sizeDelta = this.rectTransform.sizeDelta;
			rectTransform.localPosition = Vector3.zero;
			rectTransform.anchoredPosition = Vector2.zero;
			for (int i = 0; i < this.m_BulletCount; i++)
			{
				float num = (float)i / (float)this.m_BulletCount;
				GameObject gameObject = new GameObject("Bullet " + i.ToString(), new Type[]
				{
					typeof(RectTransform)
				});
				gameObject.transform.SetParent(this.m_BulletsContainer.transform);
				gameObject.layer = base.gameObject.layer;
				RectTransform rectTransform2 = gameObject.transform as RectTransform;
				rectTransform2.localScale = new Vector3(1f, 1f, 1f);
				rectTransform2.localPosition = Vector3.zero;
				Image image = gameObject.AddComponent<Image>();
				image.sprite = this.m_BulletSprite;
				image.color = this.m_BulletSpriteColor;
				if (this.m_FixedSize)
				{
					rectTransform2.sizeDelta = this.m_BulletSize;
				}
				else
				{
					image.SetNativeSize();
				}
				if (this.m_BarType == UIBulletBar.BarType.Radial)
				{
					float num2 = this.m_AngleMin + num * (this.m_AngleMax - this.m_AngleMin);
					Vector2 anchoredPosition;
					anchoredPosition.x = 0f + this.m_Distance * Mathf.Sin(num2 * 0.017453292f);
					anchoredPosition.y = 0f + this.m_Distance * Mathf.Cos(num2 * 0.017453292f);
					rectTransform2.anchoredPosition = anchoredPosition;
					rectTransform2.Rotate(new Vector3(0f, 0f, (this.m_SpriteRotation + num2) * -1f));
				}
				else if (this.m_BarType == UIBulletBar.BarType.Horizontal)
				{
					rectTransform2.pivot = new Vector2(0.5f, 0.5f);
					rectTransform2.anchorMin = new Vector2(1f, 0.5f);
					rectTransform2.anchorMax = new Vector2(1f, 0.5f);
					float num3 = rectTransform2.sizeDelta.x * (float)this.m_BulletCount;
					float num4 = (this.rectTransform.rect.width - num3) / (float)(this.m_BulletCount - 1);
					float num5 = rectTransform2.sizeDelta.x * (float)i + num4 * (float)i;
					Vector2 anchoredPosition2;
					anchoredPosition2.x = (num5 + rectTransform2.sizeDelta.x / 2f) * -1f;
					anchoredPosition2.y = 0f;
					rectTransform2.anchoredPosition = anchoredPosition2;
					rectTransform2.Rotate(new Vector3(0f, 0f, this.m_SpriteRotation));
				}
				else if (this.m_BarType == UIBulletBar.BarType.Vertical)
				{
					rectTransform2.pivot = new Vector2(0.5f, 0.5f);
					rectTransform2.anchorMin = new Vector2(0.5f, 1f);
					rectTransform2.anchorMax = new Vector2(0.5f, 1f);
					float num6 = rectTransform2.sizeDelta.y * (float)this.m_BulletCount;
					float num7 = (this.rectTransform.rect.height - num6) / (float)(this.m_BulletCount - 1);
					float num8 = rectTransform2.sizeDelta.y * (float)i + num7 * (float)i;
					Vector2 anchoredPosition3;
					anchoredPosition3.x = 0f;
					anchoredPosition3.y = (num8 + rectTransform2.sizeDelta.y / 2f) * -1f;
					rectTransform2.anchoredPosition = anchoredPosition3;
					rectTransform2.Rotate(new Vector3(0f, 0f, this.m_SpriteRotation));
				}
				GameObject gameObject2 = new GameObject("Fill", new Type[]
				{
					typeof(RectTransform)
				});
				gameObject2.transform.SetParent(gameObject.transform);
				gameObject2.layer = base.gameObject.layer;
				RectTransform rectTransform3 = gameObject2.transform as RectTransform;
				rectTransform3.localScale = new Vector3(1f, 1f, 1f);
				rectTransform3.localPosition = Vector3.zero;
				rectTransform3.anchoredPosition = this.m_ActivePosition;
				rectTransform3.rotation = rectTransform2.rotation;
				Image image2 = gameObject2.AddComponent<Image>();
				image2.sprite = this.m_BulletSpriteActive;
				image2.color = this.m_BulletSpriteActiveColor;
				if (this.m_FixedSize)
				{
					rectTransform3.sizeDelta = this.m_BulletSize;
				}
				else
				{
					image2.SetNativeSize();
				}
				this.m_FillBullets.Add(gameObject2);
			}
			this.UpdateFill();
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x000A1AE8 File Offset: 0x0009FCE8
		protected void DestroyBullets()
		{
			this.m_FillBullets.Clear();
			GameObject bulletsContainer = this.m_BulletsContainer;
			if (!Application.isEditor)
			{
				UnityEngine.Object.Destroy(bulletsContainer);
			}
			this.m_BulletsContainer = null;
		}

		// Token: 0x04001A08 RID: 6664
		[SerializeField]
		private UIBulletBar.BarType m_BarType;

		// Token: 0x04001A09 RID: 6665
		[SerializeField]
		private bool m_FixedSize;

		// Token: 0x04001A0A RID: 6666
		[SerializeField]
		private Vector2 m_BulletSize = Vector2.zero;

		// Token: 0x04001A0B RID: 6667
		[SerializeField]
		private Sprite m_BulletSprite;

		// Token: 0x04001A0C RID: 6668
		[SerializeField]
		private Color m_BulletSpriteColor = Color.white;

		// Token: 0x04001A0D RID: 6669
		[SerializeField]
		private Sprite m_BulletSpriteActive;

		// Token: 0x04001A0E RID: 6670
		[SerializeField]
		private Color m_BulletSpriteActiveColor = Color.white;

		// Token: 0x04001A0F RID: 6671
		[SerializeField]
		private float m_SpriteRotation;

		// Token: 0x04001A10 RID: 6672
		[SerializeField]
		private Vector2 m_ActivePosition = Vector2.zero;

		// Token: 0x04001A11 RID: 6673
		[SerializeField]
		[Range(0f, 360f)]
		private float m_AngleMin;

		// Token: 0x04001A12 RID: 6674
		[SerializeField]
		[Range(0f, 360f)]
		private float m_AngleMax = 360f;

		// Token: 0x04001A13 RID: 6675
		[SerializeField]
		private int m_BulletCount = 10;

		// Token: 0x04001A14 RID: 6676
		[SerializeField]
		private float m_Distance = 100f;

		// Token: 0x04001A15 RID: 6677
		[SerializeField]
		[Range(0f, 1f)]
		private float m_FillAmount = 1f;

		// Token: 0x04001A16 RID: 6678
		[SerializeField]
		private bool m_InvertFill = true;

		// Token: 0x04001A17 RID: 6679
		[SerializeField]
		[HideInInspector]
		private GameObject m_BulletsContainer;

		// Token: 0x04001A18 RID: 6680
		[SerializeField]
		[HideInInspector]
		private List<GameObject> m_FillBullets;

		// Token: 0x020005BD RID: 1469
		public enum BarType
		{
			// Token: 0x04001A1A RID: 6682
			Horizontal,
			// Token: 0x04001A1B RID: 6683
			Vertical,
			// Token: 0x04001A1C RID: 6684
			Radial
		}
	}
}
