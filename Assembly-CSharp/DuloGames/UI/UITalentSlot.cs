using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x02000616 RID: 1558
	[AddComponentMenu("UI/Icon Slots/Talent Slot", 12)]
	public class UITalentSlot : UISlotBase
	{
		// Token: 0x0600225F RID: 8799 RVA: 0x000AA9BD File Offset: 0x000A8BBD
		protected override void Start()
		{
			base.Start();
			base.dragAndDropEnabled = false;
		}

		// Token: 0x06002260 RID: 8800 RVA: 0x000AA9CC File Offset: 0x000A8BCC
		public UISpellInfo GetSpellInfo()
		{
			return this.m_SpellInfo;
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x000AA9D4 File Offset: 0x000A8BD4
		public UITalentInfo GetTalentInfo()
		{
			return this.m_TalentInfo;
		}

		// Token: 0x06002262 RID: 8802 RVA: 0x000AA9DC File Offset: 0x000A8BDC
		public override bool IsAssigned()
		{
			return this.m_TalentInfo != null;
		}

		// Token: 0x06002263 RID: 8803 RVA: 0x000AA9E7 File Offset: 0x000A8BE7
		public bool Assign(UITalentInfo talentInfo, UISpellInfo spellInfo)
		{
			if (talentInfo == null || spellInfo == null)
			{
				return false;
			}
			base.Assign(spellInfo.Icon);
			this.m_TalentInfo = talentInfo;
			this.m_SpellInfo = spellInfo;
			this.UpdatePointsLabel();
			return true;
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x000AAA14 File Offset: 0x000A8C14
		public void UpdatePointsLabel()
		{
			if (this.m_PointsText == null)
			{
				return;
			}
			this.m_PointsText.text = "";
			Text pointsText;
			if (this.m_CurrentPoints == 0)
			{
				pointsText = this.m_PointsText;
				pointsText.text = string.Concat(new string[]
				{
					pointsText.text,
					"<color=#",
					CommonColorBuffer.ColorToString(this.m_pointsMinColor),
					">",
					this.m_CurrentPoints.ToString(),
					"</color>"
				});
				pointsText = this.m_PointsText;
				pointsText.text = string.Concat(new string[]
				{
					pointsText.text,
					"<color=#",
					CommonColorBuffer.ColorToString(this.m_pointsMaxColor),
					">/",
					this.m_TalentInfo.maxPoints.ToString(),
					"</color>"
				});
				return;
			}
			if (this.m_CurrentPoints > 0 && this.m_CurrentPoints < this.m_TalentInfo.maxPoints)
			{
				pointsText = this.m_PointsText;
				pointsText.text = string.Concat(new string[]
				{
					pointsText.text,
					"<color=#",
					CommonColorBuffer.ColorToString(this.m_pointsMinColor),
					">",
					this.m_CurrentPoints.ToString(),
					"</color>"
				});
				pointsText = this.m_PointsText;
				pointsText.text = string.Concat(new string[]
				{
					pointsText.text,
					"<color=#",
					CommonColorBuffer.ColorToString(this.m_pointsMaxColor),
					">/",
					this.m_TalentInfo.maxPoints.ToString(),
					"</color>"
				});
				return;
			}
			pointsText = this.m_PointsText;
			pointsText.text = string.Concat(new string[]
			{
				pointsText.text,
				"<color=#",
				CommonColorBuffer.ColorToString(this.m_pointsActiveColor),
				">",
				this.m_CurrentPoints.ToString(),
				"/",
				this.m_TalentInfo.maxPoints.ToString(),
				"</color>"
			});
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x000AAC56 File Offset: 0x000A8E56
		public override void Unassign()
		{
			base.Unassign();
			this.m_TalentInfo = null;
			this.m_SpellInfo = null;
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x000AAC6C File Offset: 0x000A8E6C
		public override void OnPointerClick(PointerEventData eventData)
		{
			if (!this.IsAssigned())
			{
				return;
			}
			if (eventData.button == PointerEventData.InputButton.Right)
			{
				this.OnRightPointerClick(eventData);
				return;
			}
			if (this.m_CurrentPoints >= this.m_TalentInfo.maxPoints)
			{
				return;
			}
			this.m_CurrentPoints++;
			this.UpdatePointsLabel();
		}

		// Token: 0x06002267 RID: 8807 RVA: 0x000AACBB File Offset: 0x000A8EBB
		public virtual void OnRightPointerClick(PointerEventData eventData)
		{
			if (this.m_CurrentPoints == 0)
			{
				return;
			}
			this.m_CurrentPoints--;
			this.UpdatePointsLabel();
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x000AACDC File Offset: 0x000A8EDC
		public void AddPoints(int points)
		{
			if (!this.IsAssigned() || points == 0)
			{
				return;
			}
			this.m_CurrentPoints += points;
			if (this.m_CurrentPoints < 0)
			{
				this.m_CurrentPoints = 0;
			}
			if (this.m_CurrentPoints > this.m_TalentInfo.maxPoints)
			{
				this.m_CurrentPoints = this.m_TalentInfo.maxPoints;
			}
			this.UpdatePointsLabel();
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x000AAD3D File Offset: 0x000A8F3D
		public override void OnTooltip(bool show)
		{
			if (this.m_SpellInfo == null)
			{
				return;
			}
			if (show)
			{
				UITooltip.InstantiateIfNecessary(base.gameObject);
				UISpellSlot.PrepareTooltip(this.m_SpellInfo);
				UITooltip.AnchorToRect(base.transform as RectTransform);
				UITooltip.Show();
				return;
			}
			UITooltip.Hide();
		}

		// Token: 0x04001BF2 RID: 7154
		[SerializeField]
		private Text m_PointsText;

		// Token: 0x04001BF3 RID: 7155
		[SerializeField]
		private Color m_pointsMinColor = Color.white;

		// Token: 0x04001BF4 RID: 7156
		[SerializeField]
		private Color m_pointsMaxColor = Color.white;

		// Token: 0x04001BF5 RID: 7157
		[SerializeField]
		private Color m_pointsActiveColor = Color.white;

		// Token: 0x04001BF6 RID: 7158
		private UITalentInfo m_TalentInfo;

		// Token: 0x04001BF7 RID: 7159
		private UISpellInfo m_SpellInfo;

		// Token: 0x04001BF8 RID: 7160
		private int m_CurrentPoints;
	}
}
