using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000294 RID: 660
public class TalentWindowManager : MonoBehaviour
{
	// Token: 0x06000A56 RID: 2646 RVA: 0x0002F5CC File Offset: 0x0002D7CC
	private void OnEnable()
	{
		this.talentModule = NetworkClient.connection.identity.GetComponent<TalentModule>();
		this.talentPointsLabel.text = this.talentModule.AvailableTalentPoints.ToString();
		for (int i = 0; i < this.abilitiesContainer.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(this.abilitiesContainer.transform.GetChild(i).gameObject);
		}
		foreach (Talent talent in ScriptableDatabaseModule.Singleton.GetAllTalents())
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.abilitySlotPrefab, this.abilitiesContainer.transform, false);
			gameObject.transform.position = Vector2.zero;
			gameObject.GetComponent<TalentSlotManager>().SetTalent(this, talent);
		}
	}

	// Token: 0x06000A57 RID: 2647 RVA: 0x0002F697 File Offset: 0x0002D897
	private void Update()
	{
		this.talentPointsLabel.text = string.Format(LanguageManager.Instance.GetText("talent_points_label"), this.talentModule.AvailableTalentPoints);
	}

	// Token: 0x06000A58 RID: 2648 RVA: 0x0002F6C8 File Offset: 0x0002D8C8
	public void OnSlotSelected(Talent selectedTalent, int currentTalentLevel)
	{
		this.SelectedTalent = selectedTalent;
		string text = LanguageManager.Instance.GetText("talent_description_layout");
		text = string.Format(text, new object[]
		{
			selectedTalent.TranslatedName,
			currentTalentLevel,
			selectedTalent.MaxLevel,
			selectedTalent.TranslatedDescription
		});
		this.descriptionLabel.text = text;
	}

	// Token: 0x04000BD8 RID: 3032
	[SerializeField]
	private GameObject abilitySlotPrefab;

	// Token: 0x04000BD9 RID: 3033
	[SerializeField]
	private GameObject abilitiesContainer;

	// Token: 0x04000BDA RID: 3034
	[SerializeField]
	private Text descriptionLabel;

	// Token: 0x04000BDB RID: 3035
	[SerializeField]
	private Text talentPointsLabel;

	// Token: 0x04000BDC RID: 3036
	private TalentModule talentModule;

	// Token: 0x04000BDD RID: 3037
	public Talent SelectedTalent;
}
