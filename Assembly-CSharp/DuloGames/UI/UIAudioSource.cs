using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x020005B8 RID: 1464
	[AddComponentMenu("UI/Audio/Audio Source")]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(AudioSource))]
	public class UIAudioSource : MonoBehaviour
	{
		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06002028 RID: 8232 RVA: 0x000A12B6 File Offset: 0x0009F4B6
		public static UIAudioSource Instance
		{
			get
			{
				return UIAudioSource.m_Instance;
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06002029 RID: 8233 RVA: 0x000A12BD File Offset: 0x0009F4BD
		// (set) Token: 0x0600202A RID: 8234 RVA: 0x000A12C5 File Offset: 0x0009F4C5
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

		// Token: 0x0600202B RID: 8235 RVA: 0x000A12CE File Offset: 0x0009F4CE
		protected void Awake()
		{
			if (UIAudioSource.m_Instance != null)
			{
				Debug.LogWarning("You have more than one UIAudioSource in the scene, please make sure you have only one.");
				return;
			}
			UIAudioSource.m_Instance = this;
			this.m_AudioSource = base.gameObject.GetComponent<AudioSource>();
			this.m_AudioSource.playOnAwake = false;
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x000A130B File Offset: 0x0009F50B
		public void PlayAudio(AudioClip clip)
		{
			this.m_AudioSource.PlayOneShot(clip, this.m_Volume);
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x000A131F File Offset: 0x0009F51F
		public void PlayAudio(AudioClip clip, float volume)
		{
			this.m_AudioSource.PlayOneShot(clip, this.m_Volume * volume);
		}

		// Token: 0x040019F9 RID: 6649
		private static UIAudioSource m_Instance;

		// Token: 0x040019FA RID: 6650
		[SerializeField]
		[Range(0f, 1f)]
		private float m_Volume = 1f;

		// Token: 0x040019FB RID: 6651
		private AudioSource m_AudioSource;
	}
}
