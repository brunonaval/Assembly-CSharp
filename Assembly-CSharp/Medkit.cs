using System;
using UnityEngine;

// Token: 0x020004C0 RID: 1216
[CreateAssetMenu(fileName = "medkit_talent", menuName = "Scriptables/Talents/Medkit")]
public class Medkit : Talent
{
	// Token: 0x06001B4E RID: 6990 RVA: 0x0008B098 File Offset: 0x00089298
	public override void ActivateOnServer(GameObject player, int talentLevel)
	{
		talentLevel = Mathf.Min(talentLevel, this.MaxLevel);
		Condition condition = new Condition(ConditionCategory.Regeneration, ConditionType.Healing, 172800f, 3f, this.PowerPerLevel * (float)talentLevel, default(Effect), 0, 0f, "");
		player.GetComponent<ConditionModule>().StartConditionAndIgnorePersistence(condition, player, true);
	}
}
