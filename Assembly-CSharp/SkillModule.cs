using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x02000418 RID: 1048
public class SkillModule : NetworkBehaviour
{
	// Token: 0x17000257 RID: 599
	// (get) Token: 0x060016B3 RID: 5811 RVA: 0x00073F36 File Offset: 0x00072136
	// (set) Token: 0x060016B4 RID: 5812 RVA: 0x00073F3E File Offset: 0x0007213E
	public float LastCastTime { get; private set; }

	// Token: 0x17000258 RID: 600
	// (get) Token: 0x060016B5 RID: 5813 RVA: 0x00073F47 File Offset: 0x00072147
	// (set) Token: 0x060016B6 RID: 5814 RVA: 0x00073F4F File Offset: 0x0007214F
	public int LastCastedNonDefaultSkillId { get; private set; }

	// Token: 0x17000259 RID: 601
	// (get) Token: 0x060016B7 RID: 5815 RVA: 0x00073F58 File Offset: 0x00072158
	public bool IsCasting
	{
		get
		{
			return this.conditionModule != null && this.conditionModule.HasActiveCondition(ConditionType.Cast);
		}
	}

	// Token: 0x14000021 RID: 33
	// (add) Token: 0x060016B8 RID: 5816 RVA: 0x00073F78 File Offset: 0x00072178
	// (remove) Token: 0x060016B9 RID: 5817 RVA: 0x00073FB0 File Offset: 0x000721B0
	public event SkillModule.OnSkillAddedToSkillBarEventHandler OnSkillAddedToSkillBar;

	// Token: 0x14000022 RID: 34
	// (add) Token: 0x060016BA RID: 5818 RVA: 0x00073FE8 File Offset: 0x000721E8
	// (remove) Token: 0x060016BB RID: 5819 RVA: 0x00074020 File Offset: 0x00072220
	public event SkillModule.OnSkillRemovedFromSkillBarEventHandler OnSkillRemovedFromSkillBar;

	// Token: 0x14000023 RID: 35
	// (add) Token: 0x060016BC RID: 5820 RVA: 0x00074058 File Offset: 0x00072258
	// (remove) Token: 0x060016BD RID: 5821 RVA: 0x00074090 File Offset: 0x00072290
	public event SkillModule.OnSkillSwapSkillBarSlotEventHandler OnSkillSwapSkillBarSlot;

	// Token: 0x14000024 RID: 36
	// (add) Token: 0x060016BE RID: 5822 RVA: 0x000740C8 File Offset: 0x000722C8
	// (remove) Token: 0x060016BF RID: 5823 RVA: 0x00074100 File Offset: 0x00072300
	public event SkillModule.OnSkillAddedToSkillBookEventHandler OnSkillAddedToSkillBook;

	// Token: 0x14000025 RID: 37
	// (add) Token: 0x060016C0 RID: 5824 RVA: 0x00074138 File Offset: 0x00072338
	// (remove) Token: 0x060016C1 RID: 5825 RVA: 0x00074170 File Offset: 0x00072370
	public event SkillModule.OnSkillSetLastUseTimeEventHandler OnSkillSetLastUseTime;

	// Token: 0x060016C2 RID: 5826 RVA: 0x000741A5 File Offset: 0x000723A5
	private void Awake()
	{
		if (NetworkClient.active)
		{
			GameObject.FindGameObjectWithTag("UISystem").TryGetComponent<UISystemModule>(out this.uiSystemModule);
		}
		if (NetworkServer.active)
		{
			GameObject.FindGameObjectWithTag("DatabaseManager").TryGetComponent<SkillDatabaseModule>(out this.skillDatabaseModule);
		}
	}

	// Token: 0x060016C3 RID: 5827 RVA: 0x000741E4 File Offset: 0x000723E4
	private void Start()
	{
		base.TryGetComponent<AreaModule>(out this.areaModule);
		base.TryGetComponent<EffectModule>(out this.effectModule);
		base.TryGetComponent<VocationModule>(out this.vocationModule);
		base.TryGetComponent<CreatureModule>(out this.creatureModule);
		base.TryGetComponent<InventoryModule>(out this.inventoryModule);
		base.TryGetComponent<AttributeModule>(out this.attributeModule);
		base.TryGetComponent<ConditionModule>(out this.conditionModule);
		base.TryGetComponent<EquipmentModule>(out this.equipmentModule);
		if (this.vocationModule.Vocation == Vocation.Elementor)
		{
			this.shootPivot = new Vector3(0f, 0.13f, 0f);
			return;
		}
		this.shootPivot = new Vector3(0f, 0.23f, 0f);
	}

	// Token: 0x060016C4 RID: 5828 RVA: 0x0007429C File Offset: 0x0007249C
	public override void OnStartServer()
	{
		this.SkillBook.Callback += delegate(SyncList<Skill>.Operation op, int index, Skill oldItem, Skill newItem)
		{
			if (op == SyncList<Skill>.Operation.OP_ADD | op == SyncList<Skill>.Operation.OP_INSERT)
			{
				this.ShouldPersistData = newItem.Learned;
			}
		};
	}

	// Token: 0x060016C5 RID: 5829 RVA: 0x000742B8 File Offset: 0x000724B8
	public override void OnStartLocalPlayer()
	{
		this.SkillBook.Callback += this.OnSkillBookUpdated;
		if (!GlobalSettings.IsMobilePlatform)
		{
			this.SkillBar.Callback += this.OnSkillBarUpdated;
			this.SecondSkillBar.Callback += this.OnSecondSkillBarUpdated;
			this.BuildSkillBarSlots();
			this.BuildSecondSkillBarSlots();
		}
		this.BuildSkillBookSlots();
	}

	// Token: 0x060016C6 RID: 5830 RVA: 0x00074324 File Offset: 0x00072524
	private void OnSkillBookUpdated(SyncList<Skill>.Operation op, int itemIndex, Skill oldItem, Skill newItem)
	{
		switch (op)
		{
		case SyncList<Skill>.Operation.OP_ADD:
		case SyncList<Skill>.Operation.OP_INSERT:
		{
			if (this.SkillBookSlots.Count < this.SkillBook.Count)
			{
				this.CreateSkillBookSlot().name = string.Format("SkillBook Slot (SkillID: {0})", newItem.Id);
			}
			int num = this.SkillBookSlots.Count - 1;
			SkillBookSlotModule componentInChildren = this.SkillBookSlots[num].GetComponentInChildren<SkillBookSlotModule>();
			componentInChildren.SlotPosition = num;
			componentInChildren.Skill = this.SkillBook[num];
			return;
		}
		case SyncList<Skill>.Operation.OP_CLEAR:
		case SyncList<Skill>.Operation.OP_REMOVEAT:
			break;
		case SyncList<Skill>.Operation.OP_SET:
			this.SkillBookSlots[itemIndex].GetComponentInChildren<SkillBookSlotModule>().Skill = newItem;
			break;
		default:
			return;
		}
	}

	// Token: 0x060016C7 RID: 5831 RVA: 0x000743D8 File Offset: 0x000725D8
	private void OnSkillBarUpdated(SyncList<Skill>.Operation op, int itemIndex, Skill oldItem, Skill newItem)
	{
		if (op == SyncList<Skill>.Operation.OP_ADD)
		{
			if (this.SkillBarSlots.Count < this.SkillBar.Count)
			{
				this.CreateSkillBarSlot().name = string.Format("SkillBar Slot ({0})", itemIndex);
			}
			SkillBarSlotModule component = this.SkillBarSlots[itemIndex].GetComponent<SkillBarSlotModule>();
			component.SlotPosition = itemIndex;
			component.Skill = this.SkillBar[itemIndex];
			return;
		}
		if (op == SyncList<Skill>.Operation.OP_SET)
		{
			SkillBarSlotModule component2 = this.SkillBarSlots[itemIndex].GetComponent<SkillBarSlotModule>();
			component2.SlotPosition = itemIndex;
			component2.Skill = this.SkillBar[itemIndex];
		}
	}

	// Token: 0x060016C8 RID: 5832 RVA: 0x00074474 File Offset: 0x00072674
	private void OnSecondSkillBarUpdated(SyncList<Skill>.Operation op, int itemIndex, Skill oldItem, Skill newItem)
	{
		if (op == SyncList<Skill>.Operation.OP_ADD)
		{
			if (this.SecondSkillBarSlots.Count < this.SkillBar.Count)
			{
				this.CreateSecondSkillBarSlot().name = string.Format("SkillBar Slot ({0})", itemIndex);
			}
			SkillBarSlotModule component = this.SecondSkillBarSlots[itemIndex].GetComponent<SkillBarSlotModule>();
			component.SlotPosition = itemIndex;
			component.Skill = this.SecondSkillBar[itemIndex];
			return;
		}
		if (op == SyncList<Skill>.Operation.OP_SET)
		{
			SkillBarSlotModule component2 = this.SecondSkillBarSlots[itemIndex].GetComponent<SkillBarSlotModule>();
			component2.SlotPosition = itemIndex;
			component2.Skill = this.SecondSkillBar[itemIndex];
		}
	}

	// Token: 0x060016C9 RID: 5833 RVA: 0x00074510 File Offset: 0x00072710
	private void BuildSkillBarSlots()
	{
		for (int i = 0; i < this.SkillBar.Count; i++)
		{
			if (this.SkillBarSlots.Count < this.SkillBar.Count)
			{
				this.CreateSkillBarSlot().name = string.Format("SkillBar Slot ({0})", i);
			}
			SkillBarSlotModule component = this.SkillBarSlots[i].GetComponent<SkillBarSlotModule>();
			component.SlotPosition = i;
			component.SkillBarId = 0;
			component.Skill = this.SkillBar[i];
		}
	}

	// Token: 0x060016CA RID: 5834 RVA: 0x00074598 File Offset: 0x00072798
	private void BuildSecondSkillBarSlots()
	{
		for (int i = 0; i < this.SecondSkillBar.Count; i++)
		{
			if (this.SecondSkillBarSlots.Count < this.SecondSkillBar.Count)
			{
				this.CreateSecondSkillBarSlot().name = string.Format("SkillBar Slot ({0})", i);
			}
			SkillBarSlotModule component = this.SecondSkillBarSlots[i].GetComponent<SkillBarSlotModule>();
			component.SlotPosition = i;
			component.SkillBarId = 1;
			component.Skill = this.SecondSkillBar[i];
		}
	}

	// Token: 0x060016CB RID: 5835 RVA: 0x00074620 File Offset: 0x00072820
	private void BuildSkillBookSlots()
	{
		for (int i = 0; i < this.SkillBook.Count; i++)
		{
			if (this.SkillBookSlots.Count < this.SkillBook.Count)
			{
				this.CreateSkillBookSlot().name = string.Format("SkillBook Slot ({0})", i);
			}
			SkillBookSlotModule componentInChildren = this.SkillBookSlots[i].GetComponentInChildren<SkillBookSlotModule>();
			componentInChildren.SlotPosition = i;
			componentInChildren.Skill = this.SkillBook[i];
		}
	}

	// Token: 0x060016CC RID: 5836 RVA: 0x000746A0 File Offset: 0x000728A0
	[Server]
	public void InitializeSkills(Skill[] skillBook, Skill[] skillBar, Skill[] secondSkillBar)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void SkillModule::InitializeSkills(Skill[],Skill[],Skill[])' called when server was not active");
			return;
		}
		foreach (Skill skill in skillBook)
		{
			this.LearnSkill(skill.Id, skill.LastUseTime, skill.EnchantLevel, skill.Learned, false);
		}
		int k;
		int i2;
		for (k = 0; k < 10; k = i2 + 1)
		{
			Skill item = skillBar.FirstOrDefault((Skill f) => f.SlotPosition == k);
			if (item.IsDefined)
			{
				this.SkillBar.Add(item);
			}
			else
			{
				this.SkillBar.Add(new Skill(k, 0));
			}
			i2 = k;
		}
		int i;
		for (i = 0; i < 10; i = i2 + 1)
		{
			Skill item2 = secondSkillBar.FirstOrDefault((Skill s) => s.SlotPosition == i);
			if (item2.IsDefined)
			{
				this.SecondSkillBar.Add(item2);
			}
			else
			{
				this.SecondSkillBar.Add(new Skill(i, 1));
			}
			i2 = i;
		}
	}

	// Token: 0x060016CD RID: 5837 RVA: 0x000747D8 File Offset: 0x000729D8
	[Client]
	private GameObject CreateSecondSkillBarSlot()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'UnityEngine.GameObject SkillModule::CreateSecondSkillBarSlot()' called when client was not active");
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.skillBarSlotPrefab);
		gameObject.transform.SetParent(this.uiSystemModule.SecondSkillBarWindow.transform, false);
		gameObject.transform.position = Vector2.zero;
		this.SecondSkillBarSlots.Add(gameObject);
		return gameObject;
	}

	// Token: 0x060016CE RID: 5838 RVA: 0x00074850 File Offset: 0x00072A50
	[Client]
	private GameObject CreateSkillBarSlot()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'UnityEngine.GameObject SkillModule::CreateSkillBarSlot()' called when client was not active");
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.skillBarSlotPrefab);
		gameObject.transform.SetParent(this.uiSystemModule.SkillBarWindow.transform, false);
		gameObject.transform.position = Vector2.zero;
		this.SkillBarSlots.Add(gameObject);
		return gameObject;
	}

	// Token: 0x060016CF RID: 5839 RVA: 0x000748C8 File Offset: 0x00072AC8
	[Client]
	private GameObject CreateSkillBookSlot()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'UnityEngine.GameObject SkillModule::CreateSkillBookSlot()' called when client was not active");
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.skillBookSlotPrefab);
		gameObject.transform.SetParent(this.uiSystemModule.SkillBookHolder.transform, false);
		gameObject.transform.position = Vector2.zero;
		this.SkillBookSlots.Add(gameObject);
		return gameObject;
	}

	// Token: 0x060016D0 RID: 5840 RVA: 0x00074940 File Offset: 0x00072B40
	[Server]
	public void AddIdToSkillBar(int skillId, int slotPosition, int skillBarId, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void SkillModule::AddIdToSkillBar(System.Int32,System.Int32,System.Int32,System.Boolean)' called when server was not active");
			return;
		}
		for (int i = 0; i < this.SkillBook.Count; i++)
		{
			if (this.SkillBook[i].Id == skillId)
			{
				Skill skill = this.SkillBook[i];
				if (skill.IsDefined)
				{
					bool overwrite = false;
					if (skillBarId == 0)
					{
						for (int j = 0; j < this.SkillBar.Count; j++)
						{
							if (this.SkillBar[j].Id == skill.Id)
							{
								this.SkillBar[j] = default(Skill);
								SkillModule.OnSkillRemovedFromSkillBarEventHandler onSkillRemovedFromSkillBar = this.OnSkillRemovedFromSkillBar;
								if (onSkillRemovedFromSkillBar != null)
								{
									onSkillRemovedFromSkillBar(slotPosition, skillBarId);
								}
							}
						}
						overwrite = this.SkillBar[slotPosition].IsDefined;
						skill.SlotPosition = slotPosition;
						skill.SkillBarId = 0;
						this.SkillBar[slotPosition] = skill;
					}
					else if (skillBarId == 1)
					{
						for (int k = 0; k < this.SecondSkillBar.Count; k++)
						{
							if (this.SecondSkillBar[k].Id == skill.Id)
							{
								this.SecondSkillBar[k] = default(Skill);
								SkillModule.OnSkillRemovedFromSkillBarEventHandler onSkillRemovedFromSkillBar2 = this.OnSkillRemovedFromSkillBar;
								if (onSkillRemovedFromSkillBar2 != null)
								{
									onSkillRemovedFromSkillBar2(slotPosition, skillBarId);
								}
							}
						}
						overwrite = this.SecondSkillBar[slotPosition].IsDefined;
						skill.SlotPosition = slotPosition;
						skill.SkillBarId = 1;
						this.SecondSkillBar[slotPosition] = skill;
					}
					if (!invokeEvents)
					{
						break;
					}
					SkillModule.OnSkillAddedToSkillBarEventHandler onSkillAddedToSkillBar = this.OnSkillAddedToSkillBar;
					if (onSkillAddedToSkillBar == null)
					{
						return;
					}
					onSkillAddedToSkillBar(slotPosition, skillBarId, overwrite, skill);
					return;
				}
			}
		}
	}

	// Token: 0x060016D1 RID: 5841 RVA: 0x00074AF8 File Offset: 0x00072CF8
	[Command]
	public void CmdRemoveFromSkillBar(int slotPosition, int skillBarId, bool invokeEvents)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(slotPosition);
		writer.WriteInt(skillBarId);
		writer.WriteBool(invokeEvents);
		base.SendCommandInternal("System.Void SkillModule::CmdRemoveFromSkillBar(System.Int32,System.Int32,System.Boolean)", 1405835977, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060016D2 RID: 5842 RVA: 0x00074B48 File Offset: 0x00072D48
	[Command]
	public void CmdSwapSkillBarSlot(int slotPosition, int draggingPosition, int skillBarId, bool invokeEvents)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(slotPosition);
		writer.WriteInt(draggingPosition);
		writer.WriteInt(skillBarId);
		writer.WriteBool(invokeEvents);
		base.SendCommandInternal("System.Void SkillModule::CmdSwapSkillBarSlot(System.Int32,System.Int32,System.Int32,System.Boolean)", -1752173449, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060016D3 RID: 5843 RVA: 0x00074BA0 File Offset: 0x00072DA0
	[Command]
	public void CmdAddToSkillBar(int skillBookSlotPosition, int skillBarSlotPosition, int skillBarId, bool invokeEvents)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(skillBookSlotPosition);
		writer.WriteInt(skillBarSlotPosition);
		writer.WriteInt(skillBarId);
		writer.WriteBool(invokeEvents);
		base.SendCommandInternal("System.Void SkillModule::CmdAddToSkillBar(System.Int32,System.Int32,System.Int32,System.Boolean)", 741972590, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060016D4 RID: 5844 RVA: 0x00074BF8 File Offset: 0x00072DF8
	[Server]
	public void LearnSkill(int skillId, double lastUseTime, int enchantLevel, bool learned, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void SkillModule::LearnSkill(System.Int32,System.Double,System.Int32,System.Boolean,System.Boolean)' called when server was not active");
			return;
		}
		Skill skill = this.skillDatabaseModule.CreateSkill(skillId);
		if (lastUseTime < 0.0)
		{
			lastUseTime = 0.0;
		}
		bool flag = false;
		for (int i = 0; i < this.SkillBook.Count; i++)
		{
			if (this.SkillBook[i].Id == skillId)
			{
				Skill value = this.SkillBook[i];
				value.EnchantLevel = enchantLevel;
				value.LastUseTime = lastUseTime;
				value.Learned = learned;
				this.SkillBook[i] = value;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			skill.EnchantLevel = enchantLevel;
			skill.LastUseTime = lastUseTime;
			skill.Learned = learned;
			this.SkillBook.Add(skill);
		}
		if (invokeEvents && this.OnSkillAddedToSkillBook != null)
		{
			this.OnSkillAddedToSkillBook(skill.Id);
		}
	}

	// Token: 0x060016D5 RID: 5845 RVA: 0x00074CE8 File Offset: 0x00072EE8
	public bool HasSkill(int skillId)
	{
		return this.SkillBook.Any((Skill a) => a.Id == skillId & a.Learned);
	}

	// Token: 0x060016D6 RID: 5846 RVA: 0x00074D1C File Offset: 0x00072F1C
	[Server]
	public bool CanLearnSkill(int skillId)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean SkillModule::CanLearnSkill(System.Int32)' called when server was not active");
			return default(bool);
		}
		Skill skill = this.skillDatabaseModule.CreateSkill(skillId);
		return skill.IsDefined & skill.RequiredLevel <= this.attributeModule.BaseLevel & skill.CasterVocation == this.vocationModule.Vocation;
	}

	// Token: 0x060016D7 RID: 5847 RVA: 0x00074D8C File Offset: 0x00072F8C
	[Server]
	public void SetLastUseTime(int skillId, double time)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void SkillModule::SetLastUseTime(System.Int32,System.Double)' called when server was not active");
			return;
		}
		Skill skill = default(Skill);
		for (int i = 0; i < this.SkillBook.Count; i++)
		{
			skill = this.SkillBook[i];
			if (skill.Id == skillId)
			{
				skill.LastUseTime = time;
				this.SkillBook[i] = skill;
				break;
			}
		}
		for (int j = 0; j < this.SkillBar.Count; j++)
		{
			skill = this.SkillBar[j];
			if (skill.Id == skillId)
			{
				skill.LastUseTime = time;
				this.SkillBar[j] = skill;
				break;
			}
		}
		for (int k = 0; k < this.SecondSkillBar.Count; k++)
		{
			skill = this.SecondSkillBar[k];
			if (skill.Id == skillId)
			{
				skill.LastUseTime = time;
				this.SecondSkillBar[k] = skill;
				break;
			}
		}
		if (skill.IsDefined && this.OnSkillSetLastUseTime != null)
		{
			this.OnSkillSetLastUseTime(time, skill);
		}
	}

	// Token: 0x060016D8 RID: 5848 RVA: 0x00074EA0 File Offset: 0x000730A0
	public Skill GetFromSkillBook(int skillId)
	{
		for (int i = 0; i < this.SkillBook.Count; i++)
		{
			if (this.SkillBook[i].Id == skillId)
			{
				return this.SkillBook[i];
			}
		}
		return default(Skill);
	}

	// Token: 0x060016D9 RID: 5849 RVA: 0x00074EF0 File Offset: 0x000730F0
	[Command]
	public void CmdCast(int skillId)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(skillId);
		base.SendCommandInternal("System.Void SkillModule::CmdCast(System.Int32)", -1954302510, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060016DA RID: 5850 RVA: 0x00074F2C File Offset: 0x0007312C
	[Server]
	private void Cast(Skill skill)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void SkillModule::Cast(Skill)' called when server was not active");
			return;
		}
		SkillBase fromCache = ClassFactory.GetFromCache<SkillBase>(skill.Script, Array.Empty<object>());
		SkillBaseConfig skillBaseConfig = new SkillBaseConfig(skill, this.shootPivot, base.gameObject);
		fromCache.Cast(skillBaseConfig);
		if (!skill.IsDefaultSkill)
		{
			this.LastCastedNonDefaultSkillId = skill.Id;
		}
	}

	// Token: 0x060016DB RID: 5851 RVA: 0x00074F90 File Offset: 0x00073190
	private bool ValidateCast(Skill skill)
	{
		if (this.IsCasting)
		{
			return false;
		}
		if (!skill.IsDefined)
		{
			return false;
		}
		if (!skill.Learned)
		{
			return false;
		}
		if (skill.CooldownTimer(NetworkTime.time) > 0.0)
		{
			return false;
		}
		if (this.conditionModule.HasActiveCondition(ConditionCategory.Paralyzing))
		{
			return false;
		}
		if (this.conditionModule.HasActiveCondition(ConditionType.Ethereal))
		{
			return false;
		}
		if (this.conditionModule.HasActiveCondition(ConditionType.Confusion))
		{
			return false;
		}
		if (this.attributeModule.BaseLevel < skill.RequiredLevel)
		{
			this.effectModule.ShowScreenMessage("skill_need_level_message", 3, 3.5f, new string[]
			{
				skill.RequiredLevel.ToString()
			});
			return false;
		}
		if (!this.creatureModule.IsAlive)
		{
			if (!skill.IsDefaultSkill)
			{
				this.effectModule.ShowScreenMessage("skill_you_are_dead_message", 3, 3.5f, Array.Empty<string>());
			}
			return false;
		}
		if (this.areaModule.AreaType == AreaType.ProtectedArea & skill.CanCauseDamage)
		{
			if (!skill.IsDefaultSkill)
			{
				this.effectModule.ShowScreenMessage("skill_cant_cast_on_protected_areas", 3, 3.5f, Array.Empty<string>());
			}
			return false;
		}
		if (skill.WeaponType != ItemType.Undefined)
		{
			int num = this.equipmentModule.IsEquipped(SlotType.LeftHand, skill.WeaponType) ? 1 : 0;
			bool flag = this.equipmentModule.IsEquipped(SlotType.RightHand, skill.WeaponType);
			if (num == 0 & !flag)
			{
				this.effectModule.ShowScreenMessage("skill_required_weapon_message", 3, 3.5f, Array.Empty<string>());
				this.lastCastedSkillId = skill.Id;
				this.LastCastTime = Time.time;
				return false;
			}
		}
		return true;
	}

	// Token: 0x060016DC RID: 5852 RVA: 0x00075124 File Offset: 0x00073324
	private void UpdateEnchantLevel(int skillId, int enchantLevel)
	{
		for (int i = 0; i < this.SkillBook.Count; i++)
		{
			if (this.SkillBook[i].Id == skillId)
			{
				Skill value = this.SkillBook[i];
				value.EnchantLevel = enchantLevel;
				this.SkillBook[i] = value;
				break;
			}
		}
		for (int j = 0; j < this.SkillBar.Count; j++)
		{
			if (this.SkillBar[j].Id == skillId)
			{
				Skill value2 = this.SkillBar[j];
				value2.EnchantLevel = enchantLevel;
				this.SkillBar[j] = value2;
				break;
			}
		}
		for (int k = 0; k < this.SecondSkillBar.Count; k++)
		{
			if (this.SecondSkillBar[k].Id == skillId)
			{
				Skill value3 = this.SecondSkillBar[k];
				value3.EnchantLevel = enchantLevel;
				this.SecondSkillBar[k] = value3;
				return;
			}
		}
	}

	// Token: 0x060016DD RID: 5853 RVA: 0x00075224 File Offset: 0x00073424
	[Command]
	public void CmdEnchantSkill(string enchantUniqueId, string reagentUniqueId, int skillId)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(enchantUniqueId);
		writer.WriteString(reagentUniqueId);
		writer.WriteInt(skillId);
		base.SendCommandInternal("System.Void SkillModule::CmdEnchantSkill(System.String,System.String,System.Int32)", 631530707, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060016DE RID: 5854 RVA: 0x00075274 File Offset: 0x00073474
	private void ProcessEnchantSuccess(Skill skill)
	{
		skill.EnchantLevel++;
		this.UpdateEnchantLevel(skill.Id, skill.EnchantLevel);
		this.effectModule.ShowScreenMessage("skill_enchant_success_message", 1, 3.5f, new string[]
		{
			skill.Name,
			skill.EnchantLevel.ToString()
		});
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "BlueStarBlast",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.2f,
			SoundEffectName = "blessing"
		};
		this.effectModule.ShowEffects(effectConfig);
	}

	// Token: 0x060016DF RID: 5855 RVA: 0x0007531C File Offset: 0x0007351C
	private void ProcessEnchantFailure(Skill skill, Item reagent)
	{
		if (reagent.IsDefined)
		{
			this.effectModule.ShowScreenMessage("skill_enchant_with_reagent_failure_message", 2, 3.5f, new string[]
			{
				skill.Name
			});
		}
		else
		{
			this.UpdateEnchantLevel(skill.Id, 0);
			this.effectModule.ShowScreenMessage("skill_enchant_failure_message", 2, 3.5f, new string[]
			{
				skill.Name
			});
		}
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "SmokeBomb",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.2f,
			SoundEffectName = "curse"
		};
		this.effectModule.ShowEffects(effectConfig);
	}

	// Token: 0x060016E0 RID: 5856 RVA: 0x000753D4 File Offset: 0x000735D4
	[TargetRpc]
	public void TargetShowItemBoostWindow(Item enchant)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_Item(writer, enchant);
		this.SendTargetRPCInternal(null, "System.Void SkillModule::TargetShowItemBoostWindow(Item)", 1676162407, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060016E1 RID: 5857 RVA: 0x00075410 File Offset: 0x00073610
	public SkillModule()
	{
		base.InitSyncObject(this.SkillBook);
		base.InitSyncObject(this.SkillBar);
		base.InitSyncObject(this.SecondSkillBar);
	}

	// Token: 0x060016E3 RID: 5859 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x060016E4 RID: 5860 RVA: 0x000754B0 File Offset: 0x000736B0
	protected void UserCode_CmdRemoveFromSkillBar__Int32__Int32__Boolean(int slotPosition, int skillBarId, bool invokeEvents)
	{
		if (skillBarId == 0)
		{
			this.SkillBar[slotPosition] = default(Skill);
		}
		else if (skillBarId == 1)
		{
			this.SecondSkillBar[slotPosition] = default(Skill);
		}
		if (invokeEvents && this.OnSkillRemovedFromSkillBar != null)
		{
			this.OnSkillRemovedFromSkillBar(slotPosition, skillBarId);
		}
	}

	// Token: 0x060016E5 RID: 5861 RVA: 0x00075508 File Offset: 0x00073708
	protected static void InvokeUserCode_CmdRemoveFromSkillBar__Int32__Int32__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdRemoveFromSkillBar called on client.");
			return;
		}
		((SkillModule)obj).UserCode_CmdRemoveFromSkillBar__Int32__Int32__Boolean(reader.ReadInt(), reader.ReadInt(), reader.ReadBool());
	}

	// Token: 0x060016E6 RID: 5862 RVA: 0x00075540 File Offset: 0x00073740
	protected void UserCode_CmdSwapSkillBarSlot__Int32__Int32__Int32__Boolean(int slotPosition, int draggingPosition, int skillBarId, bool invokeEvents)
	{
		if (skillBarId == 0)
		{
			Skill value = this.SkillBar[slotPosition];
			value.SlotPosition = draggingPosition;
			Skill value2 = this.SkillBar[draggingPosition];
			value2.SlotPosition = slotPosition;
			this.SkillBar[slotPosition] = value2;
			this.SkillBar[draggingPosition] = value;
			if (invokeEvents && this.OnSkillSwapSkillBarSlot != null)
			{
				this.OnSkillSwapSkillBarSlot(this.SkillBar[slotPosition], this.SkillBar[draggingPosition]);
				return;
			}
		}
		else if (skillBarId == 1)
		{
			Skill value3 = this.SecondSkillBar[slotPosition];
			value3.SlotPosition = draggingPosition;
			Skill value4 = this.SecondSkillBar[draggingPosition];
			value4.SlotPosition = slotPosition;
			this.SecondSkillBar[slotPosition] = value4;
			this.SecondSkillBar[draggingPosition] = value3;
			if (invokeEvents && this.OnSkillSwapSkillBarSlot != null)
			{
				this.OnSkillSwapSkillBarSlot(this.SecondSkillBar[slotPosition], this.SecondSkillBar[draggingPosition]);
			}
		}
	}

	// Token: 0x060016E7 RID: 5863 RVA: 0x00075641 File Offset: 0x00073841
	protected static void InvokeUserCode_CmdSwapSkillBarSlot__Int32__Int32__Int32__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSwapSkillBarSlot called on client.");
			return;
		}
		((SkillModule)obj).UserCode_CmdSwapSkillBarSlot__Int32__Int32__Int32__Boolean(reader.ReadInt(), reader.ReadInt(), reader.ReadInt(), reader.ReadBool());
	}

	// Token: 0x060016E8 RID: 5864 RVA: 0x0007567C File Offset: 0x0007387C
	protected void UserCode_CmdAddToSkillBar__Int32__Int32__Int32__Boolean(int skillBookSlotPosition, int skillBarSlotPosition, int skillBarId, bool invokeEvents)
	{
		Skill skill = this.SkillBook[skillBookSlotPosition];
		if (!skill.IsDefined)
		{
			return;
		}
		this.AddIdToSkillBar(skill.Id, skillBarSlotPosition, skillBarId, invokeEvents);
	}

	// Token: 0x060016E9 RID: 5865 RVA: 0x000756B0 File Offset: 0x000738B0
	protected static void InvokeUserCode_CmdAddToSkillBar__Int32__Int32__Int32__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdAddToSkillBar called on client.");
			return;
		}
		((SkillModule)obj).UserCode_CmdAddToSkillBar__Int32__Int32__Int32__Boolean(reader.ReadInt(), reader.ReadInt(), reader.ReadInt(), reader.ReadBool());
	}

	// Token: 0x060016EA RID: 5866 RVA: 0x000756EC File Offset: 0x000738EC
	protected void UserCode_CmdCast__Int32(int skillId)
	{
		if (this.lastCastedSkillId == skillId && Time.time - this.LastCastTime < this.skillDatabaseModule.MinimumSkillCooldown)
		{
			return;
		}
		Skill fromSkillBook = this.GetFromSkillBook(skillId);
		if (!this.ValidateCast(fromSkillBook))
		{
			return;
		}
		this.LastCastTime = Time.time;
		this.lastCastedSkillId = skillId;
		try
		{
			this.Cast(fromSkillBook);
		}
		catch (Exception ex)
		{
			Debug.LogError("Error: " + ex.Message + " Details: " + ex.StackTrace);
		}
	}

	// Token: 0x060016EB RID: 5867 RVA: 0x0007577C File Offset: 0x0007397C
	protected static void InvokeUserCode_CmdCast__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdCast called on client.");
			return;
		}
		((SkillModule)obj).UserCode_CmdCast__Int32(reader.ReadInt());
	}

	// Token: 0x060016EC RID: 5868 RVA: 0x000757A8 File Offset: 0x000739A8
	protected void UserCode_CmdEnchantSkill__String__String__Int32(string enchantUniqueId, string reagentUniqueId, int skillId)
	{
		Item item = this.inventoryModule.GetItem(enchantUniqueId);
		if (!item.IsDefined)
		{
			return;
		}
		if (item.Type != ItemType.SkillEnchant)
		{
			return;
		}
		Skill fromSkillBook = this.GetFromSkillBook(skillId);
		if (!fromSkillBook.IsDefined)
		{
			return;
		}
		if (fromSkillBook.EnchantLevel >= 10)
		{
			return;
		}
		Item item2 = this.inventoryModule.GetItem(reagentUniqueId);
		float num = GlobalUtils.GetSkillEnchantPercentSuccessChance(fromSkillBook.EnchantLevel + 1);
		if (item2.IsDefined)
		{
			if (item2.Type != ItemType.Reagent)
			{
				return;
			}
			num += num * 0.25f;
			num = Mathf.Min(1f, num);
			this.inventoryModule.ConsumeItem(item2, 1);
		}
		this.inventoryModule.ConsumeItem(item, 1);
		if (UnityEngine.Random.Range(0f, 1f) > num)
		{
			this.ProcessEnchantFailure(fromSkillBook, item2);
			return;
		}
		this.ProcessEnchantSuccess(fromSkillBook);
	}

	// Token: 0x060016ED RID: 5869 RVA: 0x00075875 File Offset: 0x00073A75
	protected static void InvokeUserCode_CmdEnchantSkill__String__String__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdEnchantSkill called on client.");
			return;
		}
		((SkillModule)obj).UserCode_CmdEnchantSkill__String__String__Int32(reader.ReadString(), reader.ReadString(), reader.ReadInt());
	}

	// Token: 0x060016EE RID: 5870 RVA: 0x000758AA File Offset: 0x00073AAA
	protected void UserCode_TargetShowItemBoostWindow__Item(Item enchant)
	{
		this.uiSystemModule.ShowSkillEnchantWindow(enchant);
	}

	// Token: 0x060016EF RID: 5871 RVA: 0x000758B8 File Offset: 0x00073AB8
	protected static void InvokeUserCode_TargetShowItemBoostWindow__Item(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetShowItemBoostWindow called on server.");
			return;
		}
		((SkillModule)obj).UserCode_TargetShowItemBoostWindow__Item(Mirror.GeneratedNetworkCode._Read_Item(reader));
	}

	// Token: 0x060016F0 RID: 5872 RVA: 0x000758E4 File Offset: 0x00073AE4
	static SkillModule()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(SkillModule), "System.Void SkillModule::CmdRemoveFromSkillBar(System.Int32,System.Int32,System.Boolean)", new RemoteCallDelegate(SkillModule.InvokeUserCode_CmdRemoveFromSkillBar__Int32__Int32__Boolean), true);
		RemoteProcedureCalls.RegisterCommand(typeof(SkillModule), "System.Void SkillModule::CmdSwapSkillBarSlot(System.Int32,System.Int32,System.Int32,System.Boolean)", new RemoteCallDelegate(SkillModule.InvokeUserCode_CmdSwapSkillBarSlot__Int32__Int32__Int32__Boolean), true);
		RemoteProcedureCalls.RegisterCommand(typeof(SkillModule), "System.Void SkillModule::CmdAddToSkillBar(System.Int32,System.Int32,System.Int32,System.Boolean)", new RemoteCallDelegate(SkillModule.InvokeUserCode_CmdAddToSkillBar__Int32__Int32__Int32__Boolean), true);
		RemoteProcedureCalls.RegisterCommand(typeof(SkillModule), "System.Void SkillModule::CmdCast(System.Int32)", new RemoteCallDelegate(SkillModule.InvokeUserCode_CmdCast__Int32), true);
		RemoteProcedureCalls.RegisterCommand(typeof(SkillModule), "System.Void SkillModule::CmdEnchantSkill(System.String,System.String,System.Int32)", new RemoteCallDelegate(SkillModule.InvokeUserCode_CmdEnchantSkill__String__String__Int32), true);
		RemoteProcedureCalls.RegisterRpc(typeof(SkillModule), "System.Void SkillModule::TargetShowItemBoostWindow(Item)", new RemoteCallDelegate(SkillModule.InvokeUserCode_TargetShowItemBoostWindow__Item));
	}

	// Token: 0x0400147A RID: 5242
	[SerializeField]
	private GameObject skillBookSlotPrefab;

	// Token: 0x0400147B RID: 5243
	[SerializeField]
	private GameObject skillBarSlotPrefab;

	// Token: 0x0400147C RID: 5244
	private Vector3 shootPivot;

	// Token: 0x0400147D RID: 5245
	private AreaModule areaModule;

	// Token: 0x0400147E RID: 5246
	private EffectModule effectModule;

	// Token: 0x0400147F RID: 5247
	private CreatureModule creatureModule;

	// Token: 0x04001480 RID: 5248
	private VocationModule vocationModule;

	// Token: 0x04001481 RID: 5249
	private UISystemModule uiSystemModule;

	// Token: 0x04001482 RID: 5250
	private InventoryModule inventoryModule;

	// Token: 0x04001483 RID: 5251
	private AttributeModule attributeModule;

	// Token: 0x04001484 RID: 5252
	private ConditionModule conditionModule;

	// Token: 0x04001485 RID: 5253
	private EquipmentModule equipmentModule;

	// Token: 0x04001486 RID: 5254
	private SkillDatabaseModule skillDatabaseModule;

	// Token: 0x04001487 RID: 5255
	public readonly SyncListSkill SkillBook = new SyncListSkill();

	// Token: 0x04001488 RID: 5256
	public readonly SyncListSkill SkillBar = new SyncListSkill();

	// Token: 0x04001489 RID: 5257
	public readonly SyncListSkill SecondSkillBar = new SyncListSkill();

	// Token: 0x0400148A RID: 5258
	public List<GameObject> SkillBookSlots = new List<GameObject>();

	// Token: 0x0400148B RID: 5259
	public List<GameObject> SkillBarSlots = new List<GameObject>();

	// Token: 0x0400148C RID: 5260
	public List<GameObject> SecondSkillBarSlots = new List<GameObject>();

	// Token: 0x0400148D RID: 5261
	public bool ShouldPersistData;

	// Token: 0x0400148E RID: 5262
	private int lastCastedSkillId;

	// Token: 0x04001490 RID: 5264
	private Dictionary<int, float> lastCastingTime = new Dictionary<int, float>();

	// Token: 0x02000419 RID: 1049
	// (Invoke) Token: 0x060016F2 RID: 5874
	public delegate void OnSkillAddedToSkillBarEventHandler(int skillBarSlotPosition, int skillBarId, bool overwrite, Skill skill);

	// Token: 0x0200041A RID: 1050
	// (Invoke) Token: 0x060016F6 RID: 5878
	public delegate void OnSkillRemovedFromSkillBarEventHandler(int skillBarSlotPosition, int skillBarId);

	// Token: 0x0200041B RID: 1051
	// (Invoke) Token: 0x060016FA RID: 5882
	public delegate void OnSkillSwapSkillBarSlotEventHandler(Skill draggedSkill, Skill replacedSkill);

	// Token: 0x0200041C RID: 1052
	// (Invoke) Token: 0x060016FE RID: 5886
	public delegate void OnSkillAddedToSkillBookEventHandler(int skillId);

	// Token: 0x0200041D RID: 1053
	// (Invoke) Token: 0x06001702 RID: 5890
	public delegate void OnSkillSetLastUseTimeEventHandler(double lastUseTime, Skill skill);
}
