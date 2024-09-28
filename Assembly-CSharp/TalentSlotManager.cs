using System;
using DuloGames.UI;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000293 RID: 659
public class TalentSlotManager : MonoBehaviour
{
	// Token: 0x06000A4F RID: 2639 RVA: 0x0002F483 File Offset: 0x0002D683
	private void OnEnable()
	{
		this._talentModule = NetworkClient.connection.identity.GetComponent<TalentModule>();
	}

	// Token: 0x06000A50 RID: 2640 RVA: 0x0002F49A File Offset: 0x0002D69A
	private void Update()
	{
		this.selectionObject.SetActive(this._windowManager.SelectedTalent == this._talent);
		this.UpdateTalentUI();
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x0002F4C4 File Offset: 0x0002D6C4
	private void UpdateTalentUI()
	{
		this.upgradeIconObject.SetActive(this._talentModule.Talents.TryGetValue(this._talent.TalentId, out this.currentTalentLevel));
		this.levelLabel.text = this.currentTalentLevel.ToString();
		this.badgeLevelLabel.text = this.currentTalentLevel.ToString();
		this.levelProgressBar.fillAmount = (float)this.currentTalentLevel / (float)this._talent.MaxLevel;
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x0002F548 File Offset: 0x0002D748
	public void SetTalent(TalentWindowManager windowManager, Talent talent)
	{
		this._talent = talent;
		this._windowManager = windowManager;
		this.nameLabel.text = talent.TranslatedName;
		this.iconImage.sprite = talent.Icon;
		this.maxLevelLabel.text = talent.MaxLevel.ToString();
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x0002F59B File Offset: 0x0002D79B
	public void OnClick()
	{
		this._windowManager.OnSlotSelected(this._talent, this.currentTalentLevel);
	}

	// Token: 0x06000A54 RID: 2644 RVA: 0x0002F5B4 File Offset: 0x0002D7B4
	public void OnIncreaseTalentClick()
	{
		this._talentModule.CmdIncreaseTalent(this._talent.TalentId);
	}

	// Token: 0x04000BCC RID: 3020
	[SerializeField]
	private Text nameLabel;

	// Token: 0x04000BCD RID: 3021
	[SerializeField]
	private Text levelLabel;

	// Token: 0x04000BCE RID: 3022
	[SerializeField]
	private Text maxLevelLabel;

	// Token: 0x04000BCF RID: 3023
	[SerializeField]
	private Text badgeLevelLabel;

	// Token: 0x04000BD0 RID: 3024
	[SerializeField]
	private Image iconImage;

	// Token: 0x04000BD1 RID: 3025
	[SerializeField]
	private UIProgressBar levelProgressBar;

	// Token: 0x04000BD2 RID: 3026
	[SerializeField]
	private GameObject selectionObject;

	// Token: 0x04000BD3 RID: 3027
	[SerializeField]
	private GameObject upgradeIconObject;

	// Token: 0x04000BD4 RID: 3028
	private int currentTalentLevel;

	// Token: 0x04000BD5 RID: 3029
	private Talent _talent;

	// Token: 0x04000BD6 RID: 3030
	private TalentWindowManager _windowManager;

	// Token: 0x04000BD7 RID: 3031
	private TalentModule _talentModule;
}
