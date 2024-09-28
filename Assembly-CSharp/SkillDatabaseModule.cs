using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000414 RID: 1044
public class SkillDatabaseModule : MonoBehaviour
{
	// Token: 0x17000254 RID: 596
	// (get) Token: 0x060016A0 RID: 5792 RVA: 0x00073846 File Offset: 0x00071A46
	// (set) Token: 0x060016A1 RID: 5793 RVA: 0x0007384E File Offset: 0x00071A4E
	public Dictionary<Vocation, List<Skill>> VocationSkills { get; private set; }

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x060016A2 RID: 5794 RVA: 0x00073857 File Offset: 0x00071A57
	// (set) Token: 0x060016A3 RID: 5795 RVA: 0x0007385F File Offset: 0x00071A5F
	public float MinimumSkillCooldown { get; private set; }

	// Token: 0x17000256 RID: 598
	// (get) Token: 0x060016A4 RID: 5796 RVA: 0x00073868 File Offset: 0x00071A68
	// (set) Token: 0x060016A5 RID: 5797 RVA: 0x00073870 File Offset: 0x00071A70
	public bool IsLoaded { get; private set; }

	// Token: 0x060016A6 RID: 5798 RVA: 0x00073879 File Offset: 0x00071A79
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
		this.skills = new List<Skill>();
		this.VocationSkills = new Dictionary<Vocation, List<Skill>>();
	}

	// Token: 0x060016A7 RID: 5799 RVA: 0x00073898 File Offset: 0x00071A98
	private void Start()
	{
		SkillDatabaseModule.<Start>d__14 <Start>d__;
		<Start>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Start>d__.<>4__this = this;
		<Start>d__.<>1__state = -1;
		<Start>d__.<>t__builder.Start<SkillDatabaseModule.<Start>d__14>(ref <Start>d__);
	}

	// Token: 0x060016A8 RID: 5800 RVA: 0x000738D0 File Offset: 0x00071AD0
	public void Add(Skill skill)
	{
		if (this.skills.Any((Skill a) => a.Id == skill.Id))
		{
			Debug.LogErrorFormat("Can't add skill, id [{0}] already exists.", new object[]
			{
				skill.Id
			});
			return;
		}
		this.skills.Add(skill);
		if (this.VocationSkills.ContainsKey(skill.CasterVocation))
		{
			this.VocationSkills[skill.CasterVocation].Add(skill);
			return;
		}
		this.VocationSkills.Add(skill.CasterVocation, new List<Skill>
		{
			skill
		});
	}

	// Token: 0x060016A9 RID: 5801 RVA: 0x0007399C File Offset: 0x00071B9C
	public Skill CreateSkill(int id)
	{
		for (int i = 0; i < this.skills.Count; i++)
		{
			if (this.skills[i].Id == id)
			{
				return this.skills[i];
			}
		}
		return default(Skill);
	}

	// Token: 0x0400146E RID: 5230
	private List<Skill> skills;
}
