using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000269 RID: 617
public class VocationToggleManager : MonoBehaviour
{
	// Token: 0x06000957 RID: 2391 RVA: 0x0002BD24 File Offset: 0x00029F24
	private void Start()
	{
		this.characterCreationManager = base.GetComponentInParent<CharacterCreationManager>();
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x0002BD32 File Offset: 0x00029F32
	public void OnToggleValueChanged()
	{
		if (this.vocationToggle.isOn & this.characterCreationManager != null)
		{
			this.characterCreationManager.SetVocation(this.vocation);
		}
	}

	// Token: 0x04000AFD RID: 2813
	[SerializeField]
	private Vocation vocation;

	// Token: 0x04000AFE RID: 2814
	[SerializeField]
	private Toggle vocationToggle;

	// Token: 0x04000AFF RID: 2815
	private CharacterCreationManager characterCreationManager;
}
