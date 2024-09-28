using System;
using UnityEngine;

// Token: 0x0200001D RID: 29
public class BitMaskAttribute : PropertyAttribute
{
	// Token: 0x06000071 RID: 113 RVA: 0x0000346E File Offset: 0x0000166E
	public BitMaskAttribute(Type aType)
	{
		this.propType = aType;
	}

	// Token: 0x0400004C RID: 76
	public Type propType;
}
