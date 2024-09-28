using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000346 RID: 838
public class ControlsTabModule : MonoBehaviour
{
	// Token: 0x06001064 RID: 4196 RVA: 0x0004D294 File Offset: 0x0004B494
	private void Awake()
	{
		if (GlobalSettings.IsMobilePlatform)
		{
			UnityEngine.Object.Destroy(this.tabMenuObject);
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
	}

	// Token: 0x06001065 RID: 4197 RVA: 0x0004D2B4 File Offset: 0x0004B4B4
	private void Start()
	{
		GameInputModule.LoadMapping();
		this.BuildMappingControls();
	}

	// Token: 0x06001066 RID: 4198 RVA: 0x0004D2C1 File Offset: 0x0004B4C1
	private void OnDisable()
	{
		GameInputModule.SaveMapping();
	}

	// Token: 0x06001067 RID: 4199 RVA: 0x0004D2C8 File Offset: 0x0004B4C8
	public void OnRestoreDefaultButtonClicked()
	{
		this.ResetAllMappingStates();
		GameInputModule.RestoreDefault();
		this.BuildMappingControls();
		this.uiSystemModule.EffectModule.ShowScreenMessage("keymapping_restored_default_message", 3, 3.5f, Array.Empty<string>());
	}

	// Token: 0x06001068 RID: 4200 RVA: 0x0004D2FC File Offset: 0x0004B4FC
	public void BuildMappingControls()
	{
		int childCount = this.keyMappingHolder.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = this.keyMappingHolder.transform.GetChild(i);
			if (child != null)
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
		foreach (KeyValuePair<string, KeyMap> keyValuePair in GameInputModule.KeyMapping)
		{
			KeyMapModule keyMapModule;
			UnityEngine.Object.Instantiate<GameObject>(this.keyMappingPrefab, this.keyMappingHolder.transform, false).TryGetComponent<KeyMapModule>(out keyMapModule);
			keyMapModule.SetKeyMap(this, keyValuePair.Key);
		}
	}

	// Token: 0x06001069 RID: 4201 RVA: 0x0004D3BC File Offset: 0x0004B5BC
	public void ResetAllMappingStates()
	{
		int childCount = this.keyMappingHolder.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = this.keyMappingHolder.transform.GetChild(i);
			if (child != null)
			{
				KeyMapModule keyMapModule;
				child.TryGetComponent<KeyMapModule>(out keyMapModule);
				keyMapModule.ResetState();
			}
		}
	}

	// Token: 0x04000FE8 RID: 4072
	[SerializeField]
	private GameObject keyMappingPrefab;

	// Token: 0x04000FE9 RID: 4073
	[SerializeField]
	private GameObject keyMappingHolder;

	// Token: 0x04000FEA RID: 4074
	[SerializeField]
	private GameObject tabMenuObject;

	// Token: 0x04000FEB RID: 4075
	[SerializeField]
	private UISystemModule uiSystemModule;
}
