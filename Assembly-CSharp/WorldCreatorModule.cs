using System;
using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000448 RID: 1096
public class WorldCreatorModule : MonoBehaviour
{
	// Token: 0x060018AA RID: 6314 RVA: 0x0007C1FC File Offset: 0x0007A3FC
	private void Awake()
	{
		if (NetworkClient.active)
		{
			return;
		}
		foreach (string text in this.sceneMapNames)
		{
			if (!string.IsNullOrEmpty(text))
			{
				SceneManager.LoadScene(text, LoadSceneMode.Additive);
			}
		}
	}

	// Token: 0x060018AB RID: 6315 RVA: 0x0007C239 File Offset: 0x0007A439
	private IEnumerator Start()
	{
		if (!NetworkServer.active)
		{
			yield break;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("MainCamera");
		if (gameObject != null)
		{
			gameObject.GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("UI");
		}
		Debug.Log(string.Format("Server started at {0:dd/MM/yyyy HH:mm:ss}", DateTime.Now));
		yield return new WaitForSeconds(2f);
		WorldCreatorModule.DisabledRenderers();
		Time.fixedDeltaTime = 0.075f;
		Time.maximumParticleDeltaTime = 0.075f;
		yield break;
	}

	// Token: 0x060018AC RID: 6316 RVA: 0x0007C244 File Offset: 0x0007A444
	private static void DisabledRenderers()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		if (gameObject != null)
		{
			gameObject.SetActive(false);
		}
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("UIHud");
		if (gameObject2 != null)
		{
			gameObject2.SetActive(false);
		}
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("UITweaks");
		if (gameObject3 != null)
		{
			gameObject3.SetActive(false);
		}
		Camera.main.gameObject.SetActive(false);
	}

	// Token: 0x040015B2 RID: 5554
	[SerializeField]
	public string[] sceneMapNames = new string[0];
}
