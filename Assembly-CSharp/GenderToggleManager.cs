using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001F6 RID: 502
public class GenderToggleManager : MonoBehaviour
{
	// Token: 0x06000658 RID: 1624 RVA: 0x000202CD File Offset: 0x0001E4CD
	private void Start()
	{
		this.characterCreationManager = base.GetComponentInParent<CharacterCreationManager>();
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x000202DB File Offset: 0x0001E4DB
	public void OnToggleValueChanged()
	{
		if (base.GetComponent<Toggle>().isOn & this.characterCreationManager != null)
		{
			this.characterCreationManager.SetGender(this.gender);
		}
	}

	// Token: 0x0400088E RID: 2190
	[SerializeField]
	private CreatureGender gender;

	// Token: 0x0400088F RID: 2191
	private CharacterCreationManager characterCreationManager;
}
