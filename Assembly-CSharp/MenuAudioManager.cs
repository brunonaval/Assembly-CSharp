using System;
using System.Collections;
using Mirror;
using UnityEngine;

// Token: 0x02000209 RID: 521
public class MenuAudioManager : MonoBehaviour
{
	// Token: 0x060006F6 RID: 1782 RVA: 0x000224E0 File Offset: 0x000206E0
	private void Awake()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("BGM");
		if (array.Length > 1)
		{
			for (int i = 1; i < array.Length; i++)
			{
				UnityEngine.Object.Destroy(array[i]);
			}
		}
		this.audioSource = base.GetComponent<AudioSource>();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x0002252B File Offset: 0x0002072B
	private void Update()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		base.StartCoroutine(this.FadeToDestroy());
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x00022542 File Offset: 0x00020742
	private IEnumerator FadeToDestroy()
	{
		WaitForSeconds delay = new WaitForSeconds(0.1f);
		float localVolume = this.audioSource.volume;
		while (localVolume > 0f)
		{
			localVolume -= 0.1f;
			this.audioSource.volume = localVolume;
			yield return delay;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04000902 RID: 2306
	private AudioSource audioSource;
}
