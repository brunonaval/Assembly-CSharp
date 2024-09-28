using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Mirror;
using UnityEngine;

// Token: 0x020002AA RID: 682
public class AntiCheatModule : MonoBehaviour
{
	// Token: 0x06000B08 RID: 2824 RVA: 0x00002E81 File Offset: 0x00001081
	private void Awake()
	{
		if (NetworkServer.active)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x000329F0 File Offset: 0x00030BF0
	public void OnSpeedHackDetected()
	{
		if (this.speedHackDetected)
		{
			return;
		}
		this.speedHackDetected = true;
		Condition condition = new Condition(ConditionCategory.LevelCurse, ConditionType.Agility, 9999f, 1f, 0.9f, default(Effect), 0, 0f, "");
		this.uiSystemModule.ConditionModule.StartCondition(condition, this.uiSystemModule.ConditionModule.gameObject, true);
		this.uiSystemModule.EffectModule.ShowScreenMessage("speed_hack_detected_message", 3, 10f, Array.Empty<string>());
		base.StartCoroutine(this.QuitGame());
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x00021F50 File Offset: 0x00020150
	public void OnInjectionDetected()
	{
		Application.Quit();
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x00032A89 File Offset: 0x00030C89
	private IEnumerator QuitGame()
	{
		yield return new WaitForSeconds(3f);
		Application.Quit();
		yield break;
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x00032A91 File Offset: 0x00030C91
	private IEnumerable<string> GetRunningProcesses()
	{
		Process[] processes = Process.GetProcesses();
		foreach (Process process in processes)
		{
			yield return process.ProcessName;
		}
		Process[] array = null;
		yield break;
	}

	// Token: 0x04000C82 RID: 3202
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x04000C83 RID: 3203
	private bool speedHackDetected;
}
