using System;
using UnityEngine;

// Token: 0x020004BA RID: 1210
public class SoundEffectPlayer : MonoBehaviour
{
	// Token: 0x06001B3D RID: 6973 RVA: 0x0008AE80 File Offset: 0x00089080
	public void PlayOneShot(string audioName, float volume)
	{
		AudioClip effectAudioClip = AssetBundleManager.Instance.GetEffectAudioClip(audioName);
		if (effectAudioClip != null)
		{
			this.audioSource.PlayOneShot(effectAudioClip, volume);
		}
		base.InvokeRepeating("DisableIfNotPlaying", 0.5f, 1f);
	}

	// Token: 0x06001B3E RID: 6974 RVA: 0x0008AEC4 File Offset: 0x000890C4
	private void OnDisable()
	{
		base.CancelInvoke("DisableIfNotPlaying");
	}

	// Token: 0x06001B3F RID: 6975 RVA: 0x0008AED4 File Offset: 0x000890D4
	public void PlayInLoop(string audioName, float volume, float loopDuration)
	{
		AudioClip effectAudioClip = AssetBundleManager.Instance.GetEffectAudioClip(audioName);
		if (effectAudioClip == null)
		{
			return;
		}
		this.audioSource.clip = effectAudioClip;
		this.audioSource.volume = volume;
		this.audioSource.loop = true;
		this.audioSource.Play();
		base.Invoke("StopPlayingAndDisable", loopDuration);
	}

	// Token: 0x06001B40 RID: 6976 RVA: 0x0008AF34 File Offset: 0x00089134
	private void StopPlayingAndDisable()
	{
		this.audioSource.Stop();
		this.audioSource.loop = false;
		this.audioSource.clip = null;
		base.gameObject.transform.SetParent(null, true);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06001B41 RID: 6977 RVA: 0x0008AF82 File Offset: 0x00089182
	private void DisableIfNotPlaying()
	{
		if (this.audioSource.isPlaying)
		{
			return;
		}
		this.StopPlayingAndDisable();
	}

	// Token: 0x040016B2 RID: 5810
	[SerializeField]
	private AudioSource audioSource;
}
