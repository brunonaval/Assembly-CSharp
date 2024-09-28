using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DuloGames.UI
{
	// Token: 0x020005B9 RID: 1465
	[AddComponentMenu("UI/Audio/Play Audio")]
	public class UIPlayAudio : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
	{
		// Token: 0x170002ED RID: 749
		// (get) Token: 0x0600202F RID: 8239 RVA: 0x000A1348 File Offset: 0x0009F548
		// (set) Token: 0x06002030 RID: 8240 RVA: 0x000A1350 File Offset: 0x0009F550
		public AudioClip audioClip
		{
			get
			{
				return this.m_AudioClip;
			}
			set
			{
				this.m_AudioClip = value;
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06002031 RID: 8241 RVA: 0x000A1359 File Offset: 0x0009F559
		// (set) Token: 0x06002032 RID: 8242 RVA: 0x000A1361 File Offset: 0x0009F561
		public float volume
		{
			get
			{
				return this.m_Volume;
			}
			set
			{
				this.m_Volume = value;
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06002033 RID: 8243 RVA: 0x000A136A File Offset: 0x0009F56A
		// (set) Token: 0x06002034 RID: 8244 RVA: 0x000A1372 File Offset: 0x0009F572
		public UIPlayAudio.Event playOnEvent
		{
			get
			{
				return this.m_PlayOnEvent;
			}
			set
			{
				this.m_PlayOnEvent = value;
			}
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x000A137B File Offset: 0x0009F57B
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!this.m_Pressed)
			{
				this.TriggerEvent(UIPlayAudio.Event.PointerEnter);
			}
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x000A138C File Offset: 0x0009F58C
		public void OnPointerExit(PointerEventData eventData)
		{
			if (!this.m_Pressed)
			{
				this.TriggerEvent(UIPlayAudio.Event.PointerExit);
			}
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x000A139D File Offset: 0x0009F59D
		public void OnPointerDown(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.TriggerEvent(UIPlayAudio.Event.PointerDown);
			this.m_Pressed = true;
		}

		// Token: 0x06002038 RID: 8248 RVA: 0x000A13B8 File Offset: 0x0009F5B8
		public void OnPointerUp(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.TriggerEvent(UIPlayAudio.Event.PointerUp);
			if (this.m_Pressed)
			{
				if (eventData.clickCount > 1)
				{
					this.TriggerEvent(UIPlayAudio.Event.DoubleClick);
					eventData.clickCount = 0;
				}
				else
				{
					this.TriggerEvent(UIPlayAudio.Event.Click);
				}
			}
			this.m_Pressed = false;
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x000A1404 File Offset: 0x0009F604
		private void TriggerEvent(UIPlayAudio.Event e)
		{
			if (e == this.m_PlayOnEvent)
			{
				this.PlayAudio();
			}
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x000A1418 File Offset: 0x0009F618
		public void PlayAudio()
		{
			if (!base.enabled || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this.m_AudioClip == null)
			{
				return;
			}
			if (UIAudioSource.Instance == null)
			{
				Debug.LogWarning("You dont have UIAudioSource in your scene. Cannot play audio clip.");
				return;
			}
			UIAudioSource.Instance.PlayAudio(this.m_AudioClip, this.m_Volume);
		}

		// Token: 0x040019FC RID: 6652
		[SerializeField]
		private AudioClip m_AudioClip;

		// Token: 0x040019FD RID: 6653
		[SerializeField]
		[Range(0f, 1f)]
		private float m_Volume = 1f;

		// Token: 0x040019FE RID: 6654
		[SerializeField]
		private UIPlayAudio.Event m_PlayOnEvent;

		// Token: 0x040019FF RID: 6655
		private bool m_Pressed;

		// Token: 0x020005BA RID: 1466
		public enum Event
		{
			// Token: 0x04001A01 RID: 6657
			None,
			// Token: 0x04001A02 RID: 6658
			PointerEnter,
			// Token: 0x04001A03 RID: 6659
			PointerExit,
			// Token: 0x04001A04 RID: 6660
			PointerDown,
			// Token: 0x04001A05 RID: 6661
			PointerUp,
			// Token: 0x04001A06 RID: 6662
			Click,
			// Token: 0x04001A07 RID: 6663
			DoubleClick
		}
	}
}
