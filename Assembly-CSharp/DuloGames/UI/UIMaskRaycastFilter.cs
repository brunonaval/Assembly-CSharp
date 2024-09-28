using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x02000633 RID: 1587
	[AddComponentMenu("UI/Raycast Filters/Mask Raycast Filter")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Image))]
	public class UIMaskRaycastFilter : MonoBehaviour, ICanvasRaycastFilter
	{
		// Token: 0x060022EC RID: 8940 RVA: 0x000ACAF6 File Offset: 0x000AACF6
		protected void Awake()
		{
			this.m_Image = base.gameObject.GetComponent<Image>();
			if (this.m_Image != null)
			{
				this.m_Sprite = this.m_Image.sprite;
			}
		}

		// Token: 0x060022ED RID: 8941 RVA: 0x000ACB28 File Offset: 0x000AAD28
		public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
		{
			if (this.m_Image == null || this.m_Sprite == null)
			{
				return false;
			}
			RectTransform rectTransform = (RectTransform)base.transform;
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, sp, eventCamera, out vector);
			Vector2 vector2 = new Vector2(vector.x + rectTransform.pivot.x * rectTransform.rect.width, vector.y + rectTransform.pivot.y * rectTransform.rect.height);
			Rect textureRect = this.m_Sprite.textureRect;
			Rect rect = rectTransform.rect;
			Image.Type type = this.m_Image.type;
			int x;
			int y;
			if (type != Image.Type.Simple && type == Image.Type.Sliced)
			{
				Vector4 border = this.m_Sprite.border;
				if (vector2.x < border.x)
				{
					x = Mathf.FloorToInt(textureRect.x + vector2.x);
				}
				else if (vector2.x > rect.width - border.z)
				{
					x = Mathf.FloorToInt(textureRect.x + textureRect.width - (rect.width - vector2.x));
				}
				else
				{
					x = Mathf.FloorToInt(textureRect.x + border.x + (vector2.x - border.x) / (rect.width - border.x - border.z) * (textureRect.width - border.x - border.z));
				}
				if (vector2.y < border.y)
				{
					y = Mathf.FloorToInt(textureRect.y + vector2.y);
				}
				else if (vector2.y > rect.height - border.w)
				{
					y = Mathf.FloorToInt(textureRect.y + textureRect.height - (rect.height - vector2.y));
				}
				else
				{
					y = Mathf.FloorToInt(textureRect.y + border.y + (vector2.y - border.y) / (rect.height - border.y - border.w) * (textureRect.height - border.y - border.w));
				}
			}
			else
			{
				x = Mathf.FloorToInt(textureRect.x + textureRect.width * vector2.x / rect.width);
				y = Mathf.FloorToInt(textureRect.y + textureRect.height * vector2.y / rect.height);
			}
			bool result;
			try
			{
				result = (this.m_Sprite.texture.GetPixel(x, y).a > this.m_AlphaTreshold);
			}
			catch (UnityException message)
			{
				Debug.LogError(message);
				UnityEngine.Object.Destroy(this);
				result = false;
			}
			return result;
		}

		// Token: 0x04001C59 RID: 7257
		private Image m_Image;

		// Token: 0x04001C5A RID: 7258
		private Sprite m_Sprite;

		// Token: 0x04001C5B RID: 7259
		[SerializeField]
		[Range(0f, 1f)]
		private float m_AlphaTreshold = 0.01f;
	}
}
