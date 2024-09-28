using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000519 RID: 1305
public class SummonNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001CE8 RID: 7400 RVA: 0x000915E8 File Offset: 0x0008F7E8
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		MonsterModule monsterModule;
		if (!skillBaseConfig.CasterObject.TryGetComponent<MonsterModule>(out monsterModule))
		{
			return;
		}
		CreatureModule creatureModule;
		if (!skillBaseConfig.CasterObject.TryGetComponent<CreatureModule>(out creatureModule))
		{
			return;
		}
		if (monsterModule.Summons == null)
		{
			monsterModule.Summons = new List<GameObject>();
		}
		Vector3 position = skillBaseConfig.CasterObject.transform.position;
		int num = skillBaseConfig.Skill.CastAmount - monsterModule.Summons.Count;
		for (int i = 0; i < num; i++)
		{
			GlobalUtils.RandomCircle(position, 0.32f);
			GameObject gameObject = GlobalUtils.SpawnMonster(AssetBundleManager.Instance.MonsterPrefab, (int)skillBaseConfig.Skill.SkillPower, position, creatureModule.Rank, false, false);
			MonsterModule monsterModule2;
			gameObject.TryGetComponent<MonsterModule>(out monsterModule2);
			monsterModule2.Summoner = skillBaseConfig.CasterObject;
			monsterModule.Summons.Add(gameObject);
		}
	}
}
