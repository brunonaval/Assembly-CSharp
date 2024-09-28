using System;
using UnityEngine;

// Token: 0x020004BF RID: 1215
[CreateAssetMenu(fileName = "fortification_talent", menuName = "Scriptables/Talents/Fortification")]
public class Fortification : Talent
{
	// Token: 0x06001B4C RID: 6988 RVA: 0x0008B034 File Offset: 0x00089234
	public override void ActivateOnServer(GameObject player, int talentLevel)
	{
		talentLevel = Mathf.Min(talentLevel, this.MaxLevel);
		Condition condition = new Condition(ConditionCategory.Blessing, ConditionType.Toughness, 172800f, 3f, this.PowerPerLevel * (float)talentLevel, default(Effect), 0, 0f, "");
		player.GetComponent<ConditionModule>().StartConditionAndIgnorePersistence(condition, player, true);
	}
}
