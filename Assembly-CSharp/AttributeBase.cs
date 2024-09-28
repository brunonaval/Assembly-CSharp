using System;

// Token: 0x020000F5 RID: 245
public static class AttributeBase
{
	// Token: 0x06000268 RID: 616 RVA: 0x000119A0 File Offset: 0x0000FBA0
	public static int GetBaseAttributeLevel(Vocation vocation, AttributeType attributeType)
	{
		switch (attributeType)
		{
		case AttributeType.Power:
			return AttributeBase.GetBasePowerLevel(vocation);
		case AttributeType.Toughness:
			return AttributeBase.GetBaseToughnessLevel(vocation);
		case AttributeType.Vitality:
			return AttributeBase.GetBaseVitalityLevel(vocation);
		case AttributeType.Precision:
			return AttributeBase.GetBasePrecisionLevel(vocation);
		case AttributeType.Agility:
			return AttributeBase.GetBaseAgilityLevel(vocation);
		default:
			return 0;
		}
	}

	// Token: 0x06000269 RID: 617 RVA: 0x000119ED File Offset: 0x0000FBED
	private static int GetBasePowerLevel(Vocation vocation)
	{
		switch (vocation)
		{
		case Vocation.Sentinel:
			return 7;
		case Vocation.Warrior:
			return 8;
		case Vocation.Elementor:
			return 10;
		case Vocation.Protector:
			return 4;
		case Vocation.Lyrus:
			return 5;
		default:
			return 0;
		}
	}

	// Token: 0x0600026A RID: 618 RVA: 0x00011A19 File Offset: 0x0000FC19
	private static int GetBaseToughnessLevel(Vocation vocation)
	{
		switch (vocation)
		{
		case Vocation.Sentinel:
			return 4;
		case Vocation.Warrior:
			return 5;
		case Vocation.Elementor:
			return 3;
		case Vocation.Protector:
			return 8;
		case Vocation.Lyrus:
			return 3;
		default:
			return 0;
		}
	}

	// Token: 0x0600026B RID: 619 RVA: 0x00011A44 File Offset: 0x0000FC44
	private static int GetBaseAgilityLevel(Vocation vocation)
	{
		switch (vocation)
		{
		case Vocation.Sentinel:
			return 3;
		case Vocation.Warrior:
			return 1;
		case Vocation.Elementor:
			return 2;
		case Vocation.Protector:
			return 1;
		case Vocation.Lyrus:
			return 2;
		default:
			return 0;
		}
	}

	// Token: 0x0600026C RID: 620 RVA: 0x00011A6F File Offset: 0x0000FC6F
	private static int GetBasePrecisionLevel(Vocation vocation)
	{
		switch (vocation)
		{
		case Vocation.Sentinel:
			return 5;
		case Vocation.Warrior:
			return 2;
		case Vocation.Elementor:
			return 2;
		case Vocation.Protector:
			return 1;
		case Vocation.Lyrus:
			return 1;
		default:
			return 0;
		}
	}

	// Token: 0x0600026D RID: 621 RVA: 0x00011A9A File Offset: 0x0000FC9A
	private static int GetBaseVitalityLevel(Vocation vocation)
	{
		switch (vocation)
		{
		case Vocation.Sentinel:
			return 2;
		case Vocation.Warrior:
			return 4;
		case Vocation.Elementor:
			return 3;
		case Vocation.Protector:
			return 6;
		case Vocation.Lyrus:
			return 9;
		default:
			return 0;
		}
	}

	// Token: 0x0600026E RID: 622 RVA: 0x00011AC8 File Offset: 0x0000FCC8
	public static float GetBaseAttributeLevelRatio(Vocation vocation, AttributeType attributeType)
	{
		int num = AttributeBase.GetBasePowerLevel(vocation) + AttributeBase.GetBaseToughnessLevel(vocation) + AttributeBase.GetBasePrecisionLevel(vocation) + AttributeBase.GetBaseVitalityLevel(vocation) + AttributeBase.GetBaseAgilityLevel(vocation);
		return (float)AttributeBase.GetBaseAttributeLevel(vocation, attributeType) / (float)num;
	}

	// Token: 0x0600026F RID: 623 RVA: 0x00011B04 File Offset: 0x0000FD04
	public static float CalculateBaseAttributeLevelRatio(Vocation vocation, AttributeType typeToCalculate, AttributeType[] attributeTypes)
	{
		int num = 0;
		foreach (AttributeType attributeType in attributeTypes)
		{
			num += AttributeBase.GetBaseAttributeLevel(vocation, attributeType);
		}
		return (float)AttributeBase.GetBaseAttributeLevel(vocation, typeToCalculate) / (float)num;
	}
}
