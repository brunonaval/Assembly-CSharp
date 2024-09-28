using System;
using System.Collections.Generic;

// Token: 0x0200002F RID: 47
[Serializable]
public class ApiBlueprint
{
	// Token: 0x04000097 RID: 151
	public int Experience;

	// Token: 0x04000098 RID: 152
	public int ProducesAmount;

	// Token: 0x04000099 RID: 153
	public ApiItem ProducesItem;

	// Token: 0x0400009A RID: 154
	public Guid PlayerBlueprintId;

	// Token: 0x0400009B RID: 155
	public int RequiredProfessionLevel;

	// Token: 0x0400009C RID: 156
	public PlayerProfession PlayerProfession;

	// Token: 0x0400009D RID: 157
	public List<ApiBlueprintMaterial> Materials;
}
