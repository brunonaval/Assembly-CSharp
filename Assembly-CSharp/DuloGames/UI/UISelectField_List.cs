using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005D2 RID: 1490
	public class UISelectField_List : Selectable
	{
		// Token: 0x060020C6 RID: 8390 RVA: 0x000A4C38 File Offset: 0x000A2E38
		protected override void Start()
		{
			base.Start();
			base.transition = Selectable.Transition.None;
			base.navigation = new Navigation
			{
				mode = Navigation.Mode.None
			};
		}

		// Token: 0x060020C7 RID: 8391 RVA: 0x000A4C69 File Offset: 0x000A2E69
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			if (this.onDimensionsChange != null)
			{
				this.onDimensionsChange.Invoke();
			}
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x000A4C84 File Offset: 0x000A2E84
		public void SetTriggers(string openTrigger, string closeTrigger)
		{
			this.m_AnimationOpenTrigger = openTrigger;
			this.m_AnimationCloseTrigger = closeTrigger;
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x000A4C94 File Offset: 0x000A2E94
		protected void Update()
		{
			if (base.animator != null && !string.IsNullOrEmpty(this.m_AnimationOpenTrigger) && !string.IsNullOrEmpty(this.m_AnimationCloseTrigger))
			{
				AnimatorStateInfo currentAnimatorStateInfo = base.animator.GetCurrentAnimatorStateInfo(0);
				if (currentAnimatorStateInfo.IsName(this.m_AnimationOpenTrigger) && this.m_State == UISelectField_List.State.Closed)
				{
					if (currentAnimatorStateInfo.normalizedTime >= currentAnimatorStateInfo.length)
					{
						this.m_State = UISelectField_List.State.Opened;
						if (this.onAnimationFinish != null)
						{
							this.onAnimationFinish.Invoke(this.m_State);
							return;
						}
					}
				}
				else if (currentAnimatorStateInfo.IsName(this.m_AnimationCloseTrigger) && this.m_State == UISelectField_List.State.Opened && currentAnimatorStateInfo.normalizedTime >= currentAnimatorStateInfo.length)
				{
					this.m_State = UISelectField_List.State.Closed;
					if (this.onAnimationFinish != null)
					{
						this.onAnimationFinish.Invoke(this.m_State);
					}
				}
			}
		}

		// Token: 0x060020CA RID: 8394 RVA: 0x000A4D6F File Offset: 0x000A2F6F
		public new bool IsPressed()
		{
			return base.IsPressed();
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x000A4D77 File Offset: 0x000A2F77
		public bool IsHighlighted(BaseEventData eventData)
		{
			return base.IsHighlighted();
		}

		// Token: 0x04001ACA RID: 6858
		public UISelectField_List.AnimationFinishEvent onAnimationFinish = new UISelectField_List.AnimationFinishEvent();

		// Token: 0x04001ACB RID: 6859
		public UnityEvent onDimensionsChange = new UnityEvent();

		// Token: 0x04001ACC RID: 6860
		private string m_AnimationOpenTrigger = string.Empty;

		// Token: 0x04001ACD RID: 6861
		private string m_AnimationCloseTrigger = string.Empty;

		// Token: 0x04001ACE RID: 6862
		private UISelectField_List.State m_State = UISelectField_List.State.Closed;

		// Token: 0x020005D3 RID: 1491
		public enum State
		{
			// Token: 0x04001AD0 RID: 6864
			Opened,
			// Token: 0x04001AD1 RID: 6865
			Closed
		}

		// Token: 0x020005D4 RID: 1492
		[Serializable]
		public class AnimationFinishEvent : UnityEvent<UISelectField_List.State>
		{
		}
	}
}
