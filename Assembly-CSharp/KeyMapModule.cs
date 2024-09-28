using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200034A RID: 842
public class KeyMapModule : MonoBehaviour
{
	// Token: 0x0600107C RID: 4220 RVA: 0x0004E30D File Offset: 0x0004C50D
	private void Awake()
	{
		GameObject.FindGameObjectWithTag("UISystem").TryGetComponent<UISystemModule>(out this.uiSystemModule);
	}

	// Token: 0x0600107D RID: 4221 RVA: 0x0004E328 File Offset: 0x0004C528
	private void Update()
	{
		if (this.editKeyMode)
		{
			KeyCode keyCode = GameInputModule.DetectKeyPressed();
			if (Input.GetKey(KeyCode.Backspace) | Input.GetKey(KeyCode.Delete))
			{
				keyCode = KeyCode.None;
				this.selectedKeyCode = keyCode;
				this.keyCodeText.text = LanguageManager.Instance.GetText(GlobalUtils.KeyCodeToString(this.selectedKeyCode));
				return;
			}
			if (keyCode != KeyCode.None)
			{
				this.selectedKeyCode = keyCode;
				this.keyCodeText.text = LanguageManager.Instance.GetText(GlobalUtils.KeyCodeToString(this.selectedKeyCode));
			}
			return;
		}
		else
		{
			if (!this.editAltKeyMode)
			{
				return;
			}
			KeyCode keyCode2 = GameInputModule.DetectKeyPressed();
			if (Input.GetKey(KeyCode.Backspace) | Input.GetKey(KeyCode.Delete))
			{
				keyCode2 = KeyCode.None;
				this.selectedAltKeyCode = keyCode2;
				this.altKeyCodeText.text = LanguageManager.Instance.GetText(GlobalUtils.KeyCodeToString(this.selectedAltKeyCode));
				return;
			}
			if (keyCode2 != KeyCode.None)
			{
				this.selectedAltKeyCode = keyCode2;
				this.altKeyCodeText.text = LanguageManager.Instance.GetText(GlobalUtils.KeyCodeToString(this.selectedAltKeyCode));
			}
			return;
		}
	}

	// Token: 0x0600107E RID: 4222 RVA: 0x0004E41C File Offset: 0x0004C61C
	public void SetKeyMap(ControlsTabModule controlsTabModule, string mapName)
	{
		this.keyMap = GameInputModule.GetKeyMap(mapName);
		this.controlsTabModule = controlsTabModule;
		this.mapName = mapName;
		this.selectedKeyCode = this.keyMap.KeyCode;
		this.selectedAltKeyCode = this.keyMap.AltKeyCode;
		this.mapNameText.text = LanguageManager.Instance.GetText(GlobalUtils.KeyMapNameToString(mapName));
		this.keyCodeText.text = LanguageManager.Instance.GetText(GlobalUtils.KeyCodeToString(this.selectedKeyCode));
		this.altKeyCodeText.text = LanguageManager.Instance.GetText(GlobalUtils.KeyCodeToString(this.selectedAltKeyCode));
	}

	// Token: 0x0600107F RID: 4223 RVA: 0x0004E4C0 File Offset: 0x0004C6C0
	public void ResetState()
	{
		this.editKeyMode = false;
		this.editAltKeyMode = false;
		this.focusKeyCode.SetActive(false);
		this.focusAltKeyCode.SetActive(false);
		this.saveKeyMappingButton.SetActive(false);
		this.saveAltKeyMappingButton.SetActive(false);
		this.editKeyMappingButton.SetActive(true);
		this.editAltKeyMappingButton.SetActive(true);
	}

	// Token: 0x06001080 RID: 4224 RVA: 0x0004E523 File Offset: 0x0004C723
	public void OnEditKeyMappingButtonClicked()
	{
		this.controlsTabModule.ResetAllMappingStates();
		this.editKeyMode = true;
		this.saveKeyMappingButton.SetActive(true);
		this.focusKeyCode.SetActive(true);
	}

	// Token: 0x06001081 RID: 4225 RVA: 0x0004E54F File Offset: 0x0004C74F
	public void OnEditAltKeyMappingButtonClicked()
	{
		this.controlsTabModule.ResetAllMappingStates();
		this.editAltKeyMode = true;
		this.saveAltKeyMappingButton.SetActive(true);
		this.focusAltKeyCode.SetActive(true);
	}

	// Token: 0x06001082 RID: 4226 RVA: 0x0004E57C File Offset: 0x0004C77C
	public void OnSaveKeyMappingButtonClicked()
	{
		try
		{
			this.controlsTabModule.ResetAllMappingStates();
			KeyMap key = GameInputModule.GetKeyMap(this.mapName);
			key.KeyCode = this.selectedKeyCode;
			GameInputModule.SetKeyMap(this.mapName, key);
		}
		catch (ArgumentException ex)
		{
			this.ProcessArgumentException(ex);
		}
	}

	// Token: 0x06001083 RID: 4227 RVA: 0x0004E5D8 File Offset: 0x0004C7D8
	public void OnSaveAltKeyMappingButtonClicked()
	{
		try
		{
			this.controlsTabModule.ResetAllMappingStates();
			KeyMap key = GameInputModule.GetKeyMap(this.mapName);
			key.AltKeyCode = this.selectedAltKeyCode;
			GameInputModule.SetKeyMap(this.mapName, key);
		}
		catch (ArgumentException ex)
		{
			this.ProcessArgumentException(ex);
		}
	}

	// Token: 0x06001084 RID: 4228 RVA: 0x0004E634 File Offset: 0x0004C834
	private void ProcessArgumentException(ArgumentException ex)
	{
		foreach (object obj in ex.Data.Values)
		{
			string text = LanguageManager.Instance.GetText(ex.Message);
			text = string.Format(text, GlobalUtils.KeyMapNameToString(obj.ToString()));
			this.uiSystemModule.EffectModule.ShowScreenMessage(text, 3, 3.5f, Array.Empty<string>());
		}
		if (ex.Data.Count == 0)
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage(ex.Message, 3, 3.5f, Array.Empty<string>());
		}
		this.controlsTabModule.BuildMappingControls();
	}

	// Token: 0x04000FF5 RID: 4085
	[SerializeField]
	private Text mapNameText;

	// Token: 0x04000FF6 RID: 4086
	[SerializeField]
	private Text keyCodeText;

	// Token: 0x04000FF7 RID: 4087
	[SerializeField]
	private Text altKeyCodeText;

	// Token: 0x04000FF8 RID: 4088
	[SerializeField]
	private GameObject focusKeyCode;

	// Token: 0x04000FF9 RID: 4089
	[SerializeField]
	private GameObject focusAltKeyCode;

	// Token: 0x04000FFA RID: 4090
	[SerializeField]
	private GameObject editKeyMappingButton;

	// Token: 0x04000FFB RID: 4091
	[SerializeField]
	private GameObject editAltKeyMappingButton;

	// Token: 0x04000FFC RID: 4092
	[SerializeField]
	private GameObject saveKeyMappingButton;

	// Token: 0x04000FFD RID: 4093
	[SerializeField]
	private GameObject saveAltKeyMappingButton;

	// Token: 0x04000FFE RID: 4094
	private bool editKeyMode;

	// Token: 0x04000FFF RID: 4095
	private bool editAltKeyMode;

	// Token: 0x04001000 RID: 4096
	private KeyMap keyMap;

	// Token: 0x04001001 RID: 4097
	private string mapName;

	// Token: 0x04001002 RID: 4098
	private KeyCode selectedKeyCode;

	// Token: 0x04001003 RID: 4099
	private KeyCode selectedAltKeyCode;

	// Token: 0x04001004 RID: 4100
	private ControlsTabModule controlsTabModule;

	// Token: 0x04001005 RID: 4101
	private UISystemModule uiSystemModule;
}
