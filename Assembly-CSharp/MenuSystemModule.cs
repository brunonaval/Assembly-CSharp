using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000375 RID: 885
public class MenuSystemModule : MonoBehaviour
{
	// Token: 0x060011A7 RID: 4519 RVA: 0x00053FF8 File Offset: 0x000521F8
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("NetworkManager");
		this.networkManagerModule = gameObject.GetComponent<NetworkManagerModule>();
	}

	// Token: 0x060011A8 RID: 4520 RVA: 0x0005401C File Offset: 0x0005221C
	private IEnumerator Start()
	{
		DatabaseModule databaseModule;
		GameObject.FindGameObjectWithTag("DatabaseManager").TryGetComponent<DatabaseModule>(out databaseModule);
		Debug.Log("Waiting database.");
		yield return new WaitUntil(() => databaseModule.IsLoaded | Time.time >= 225f);
		if (!databaseModule.IsLoaded)
		{
			Debug.LogError("Failure on database loading.");
			Application.Quit();
			yield break;
		}
		Debug.Log("Starting server.");
		yield return this.StartServer().WaitAsCoroutine();
		yield break;
	}

	// Token: 0x060011A9 RID: 4521 RVA: 0x0005402B File Offset: 0x0005222B
	public void ShowText(string text)
	{
		this.FeedbackPanel.SetActive(true);
		this.FeedbackText.color = Color.white;
		this.FeedbackText.text = text;
	}

	// Token: 0x060011AA RID: 4522 RVA: 0x00054055 File Offset: 0x00052255
	public void ShowError(string text)
	{
		this.FeedbackPanel.SetActive(true);
		this.FeedbackText.color = Color.red;
		this.FeedbackText.text = text;
	}

	// Token: 0x060011AB RID: 4523 RVA: 0x00054080 File Offset: 0x00052280
	public Task StartServer()
	{
		MenuSystemModule.<StartServer>d__10 <StartServer>d__;
		<StartServer>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<StartServer>d__.<>4__this = this;
		<StartServer>d__.<>1__state = -1;
		<StartServer>d__.<>t__builder.Start<MenuSystemModule.<StartServer>d__10>(ref <StartServer>d__);
		return <StartServer>d__.<>t__builder.Task;
	}

	// Token: 0x060011AC RID: 4524 RVA: 0x00021F50 File Offset: 0x00020150
	public void Exit()
	{
		Application.Quit();
	}

	// Token: 0x040010AD RID: 4269
	public GameObject FeedbackPanel;

	// Token: 0x040010AE RID: 4270
	public Text FeedbackText;

	// Token: 0x040010AF RID: 4271
	public InputField HostInput;

	// Token: 0x040010B0 RID: 4272
	[SerializeField]
	private Button startButton;

	// Token: 0x040010B1 RID: 4273
	[SerializeField]
	private Button exitButton;

	// Token: 0x040010B2 RID: 4274
	private NetworkManagerModule networkManagerModule;
}
