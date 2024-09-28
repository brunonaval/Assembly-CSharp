using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x0200060D RID: 1549
	[AddComponentMenu("UI/Icon Slots/Slot Cooldown", 28)]
	public class UISlotCooldown : MonoBehaviour
	{
		// Token: 0x0600222A RID: 8746 RVA: 0x000A9D7C File Offset: 0x000A7F7C
		protected void Awake()
		{
			if (this.m_TargetSlot != null && this.m_TargetSlot is IUISlotHasCooldown)
			{
				this.m_CooldownSlot = (this.m_TargetSlot as IUISlotHasCooldown);
				this.m_CooldownSlot.SetCooldownComponent(this);
				return;
			}
			Debug.LogWarning("The slot cooldown script cannot operate without a target slot with a IUISlotHasCooldown interface, disabling script.");
			base.enabled = false;
		}

		// Token: 0x0600222B RID: 8747 RVA: 0x000A9DD4 File Offset: 0x000A7FD4
		protected void Start()
		{
			if (this.m_TargetGraphic != null)
			{
				this.m_TargetGraphic.enabled = false;
			}
			if (this.m_TargetText != null)
			{
				this.m_TargetText.enabled = false;
			}
			if (this.m_FinishGraphic != null)
			{
				this.m_FinishGraphic.enabled = false;
				this.m_FinishGraphic.rectTransform.anchorMin = new Vector2(this.m_FinishGraphic.rectTransform.anchorMin.x, 1f);
				this.m_FinishGraphic.rectTransform.anchorMax = new Vector2(this.m_FinishGraphic.rectTransform.anchorMax.x, 1f);
			}
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x000A9E8D File Offset: 0x000A808D
		protected void OnEnable()
		{
			this.CheckForActiveCooldown();
		}

		// Token: 0x0600222D RID: 8749 RVA: 0x000A9E95 File Offset: 0x000A8095
		protected void OnDisable()
		{
			this.InterruptCooldown();
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x0600222E RID: 8750 RVA: 0x000A9E9D File Offset: 0x000A809D
		public bool IsOnCooldown
		{
			get
			{
				return this.m_IsOnCooldown;
			}
		}

		// Token: 0x0600222F RID: 8751 RVA: 0x000A9E8D File Offset: 0x000A808D
		public void OnAssignSpell()
		{
			this.CheckForActiveCooldown();
		}

		// Token: 0x06002230 RID: 8752 RVA: 0x000A9E95 File Offset: 0x000A8095
		public void OnUnassignSpell()
		{
			this.InterruptCooldown();
		}

		// Token: 0x06002231 RID: 8753 RVA: 0x000A9EA8 File Offset: 0x000A80A8
		public void CheckForActiveCooldown()
		{
			if (this.m_CooldownSlot == null)
			{
				return;
			}
			UISpellInfo spellInfo = this.m_CooldownSlot.GetSpellInfo();
			if (spellInfo == null)
			{
				return;
			}
			if (UISlotCooldown.spellCooldowns.ContainsKey(spellInfo.ID))
			{
				if (UISlotCooldown.spellCooldowns[spellInfo.ID].endTime > Time.time)
				{
					this.m_IsOnCooldown = true;
					this.ResumeCooldown(spellInfo.ID);
					return;
				}
				UISlotCooldown.spellCooldowns.Remove(spellInfo.ID);
			}
		}

		// Token: 0x06002232 RID: 8754 RVA: 0x000A9F24 File Offset: 0x000A8124
		public void StartCooldown(int spellId, float duration)
		{
			if (!base.enabled || !base.gameObject.activeInHierarchy || this.m_TargetGraphic == null)
			{
				return;
			}
			this.m_CurrentSpellId = spellId;
			if (!this.m_TargetGraphic.enabled)
			{
				this.m_TargetGraphic.enabled = true;
			}
			this.m_TargetGraphic.fillAmount = 1f;
			if (this.m_TargetText != null)
			{
				if (!this.m_TargetText.enabled)
				{
					this.m_TargetText.enabled = true;
				}
				this.m_TargetText.text = duration.ToString("0");
			}
			if (this.m_FinishGraphic != null)
			{
				this.m_FinishGraphic.canvasRenderer.SetAlpha(0f);
				this.m_FinishGraphic.enabled = true;
				this.m_FinishGraphic.rectTransform.anchoredPosition = new Vector2(this.m_FinishGraphic.rectTransform.anchoredPosition.x, this.m_FinishOffsetY);
			}
			this.m_IsOnCooldown = true;
			UISlotCooldown.CooldownInfo value = new UISlotCooldown.CooldownInfo(duration, Time.time, Time.time + duration);
			if (!UISlotCooldown.spellCooldowns.ContainsKey(spellId))
			{
				UISlotCooldown.spellCooldowns.Add(spellId, value);
			}
			base.StartCoroutine("_StartCooldown", value);
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x000AA064 File Offset: 0x000A8264
		public void ResumeCooldown(int spellId)
		{
			if (!base.enabled || !base.gameObject.activeInHierarchy || this.m_TargetGraphic == null)
			{
				return;
			}
			if (!UISlotCooldown.spellCooldowns.ContainsKey(spellId))
			{
				return;
			}
			UISlotCooldown.CooldownInfo cooldownInfo = UISlotCooldown.spellCooldowns[spellId];
			float num = cooldownInfo.endTime - Time.time;
			float remainingTimePct = num / cooldownInfo.duration;
			this.m_CurrentSpellId = spellId;
			if (!this.m_TargetGraphic.enabled)
			{
				this.m_TargetGraphic.enabled = true;
			}
			this.m_TargetGraphic.fillAmount = num / cooldownInfo.duration;
			if (this.m_TargetText != null)
			{
				if (!this.m_TargetText.enabled)
				{
					this.m_TargetText.enabled = true;
				}
				this.m_TargetText.text = num.ToString("0");
			}
			if (this.m_FinishGraphic != null)
			{
				this.m_FinishGraphic.enabled = true;
				this.UpdateFinishPosition(remainingTimePct);
			}
			base.StartCoroutine("_StartCooldown", cooldownInfo);
		}

		// Token: 0x06002234 RID: 8756 RVA: 0x000AA164 File Offset: 0x000A8364
		public void InterruptCooldown()
		{
			base.StopCoroutine("_StartCooldown");
			this.OnCooldownFinished();
		}

		// Token: 0x06002235 RID: 8757 RVA: 0x000AA177 File Offset: 0x000A8377
		private IEnumerator _StartCooldown(UISlotCooldown.CooldownInfo cooldownInfo)
		{
			while (Time.time < cooldownInfo.startTime + cooldownInfo.duration)
			{
				float num = cooldownInfo.startTime + cooldownInfo.duration - Time.time;
				float num2 = num / cooldownInfo.duration;
				if (this.m_TargetGraphic != null)
				{
					this.m_TargetGraphic.fillAmount = num2;
				}
				if (this.m_TargetText != null)
				{
					this.m_TargetText.text = num.ToString("0");
				}
				this.UpdateFinishPosition(num2);
				yield return 0;
			}
			this.OnCooldownCompleted();
			yield break;
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x000AA18D File Offset: 0x000A838D
		private void OnCooldownCompleted()
		{
			if (UISlotCooldown.spellCooldowns.ContainsKey(this.m_CurrentSpellId))
			{
				UISlotCooldown.spellCooldowns.Remove(this.m_CurrentSpellId);
			}
			this.OnCooldownFinished();
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x000AA1B8 File Offset: 0x000A83B8
		private void OnCooldownFinished()
		{
			this.m_IsOnCooldown = false;
			this.m_CurrentSpellId = 0;
			if (this.m_TargetGraphic != null)
			{
				this.m_TargetGraphic.enabled = false;
			}
			if (this.m_TargetText != null)
			{
				this.m_TargetText.enabled = false;
			}
			if (this.m_FinishGraphic != null)
			{
				this.m_FinishGraphic.enabled = false;
			}
		}

		// Token: 0x06002238 RID: 8760 RVA: 0x000AA224 File Offset: 0x000A8424
		protected void UpdateFinishPosition(float RemainingTimePct)
		{
			if (this.m_FinishGraphic != null && this.m_TargetGraphic != null)
			{
				float num = 0f - this.m_TargetGraphic.rectTransform.rect.height + this.m_TargetGraphic.rectTransform.rect.height * RemainingTimePct;
				num += this.m_FinishOffsetY;
				this.m_FinishGraphic.rectTransform.anchoredPosition = new Vector2(this.m_FinishGraphic.rectTransform.anchoredPosition.x, num);
				float num2 = this.m_FinishFadingPct / 100f;
				if (RemainingTimePct <= num2)
				{
					this.m_FinishGraphic.canvasRenderer.SetAlpha(RemainingTimePct / num2);
					return;
				}
				if (RemainingTimePct >= 1f - num2)
				{
					this.m_FinishGraphic.canvasRenderer.SetAlpha(1f - (RemainingTimePct - (1f - num2)) / num2);
					return;
				}
				if (RemainingTimePct > num2 && RemainingTimePct < 1f - num2)
				{
					this.m_FinishGraphic.canvasRenderer.SetAlpha(1f);
				}
			}
		}

		// Token: 0x04001BCD RID: 7117
		private static Dictionary<int, UISlotCooldown.CooldownInfo> spellCooldowns = new Dictionary<int, UISlotCooldown.CooldownInfo>();

		// Token: 0x04001BCE RID: 7118
		[SerializeField]
		private UISlotBase m_TargetSlot;

		// Token: 0x04001BCF RID: 7119
		[SerializeField]
		private Image m_TargetGraphic;

		// Token: 0x04001BD0 RID: 7120
		[SerializeField]
		private Text m_TargetText;

		// Token: 0x04001BD1 RID: 7121
		[SerializeField]
		private Image m_FinishGraphic;

		// Token: 0x04001BD2 RID: 7122
		[SerializeField]
		private float m_FinishOffsetY;

		// Token: 0x04001BD3 RID: 7123
		[SerializeField]
		private float m_FinishFadingPct = 25f;

		// Token: 0x04001BD4 RID: 7124
		private IUISlotHasCooldown m_CooldownSlot;

		// Token: 0x04001BD5 RID: 7125
		private bool m_IsOnCooldown;

		// Token: 0x04001BD6 RID: 7126
		private int m_CurrentSpellId;

		// Token: 0x0200060E RID: 1550
		public class CooldownInfo
		{
			// Token: 0x0600223B RID: 8763 RVA: 0x000AA354 File Offset: 0x000A8554
			public CooldownInfo(float duration, float startTime, float endTime)
			{
				this.duration = duration;
				this.startTime = startTime;
				this.endTime = endTime;
			}

			// Token: 0x04001BD7 RID: 7127
			public float duration;

			// Token: 0x04001BD8 RID: 7128
			public float startTime;

			// Token: 0x04001BD9 RID: 7129
			public float endTime;
		}
	}
}
