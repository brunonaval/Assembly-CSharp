using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001F7 RID: 503
public class InputTabManager : MonoBehaviour
{
	// Token: 0x0600065B RID: 1627 RVA: 0x00020308 File Offset: 0x0001E508
	private void Awake()
	{
		this.localField = base.GetComponent<InputField>();
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x00020316 File Offset: 0x0001E516
	private void Update()
	{
		if (this.localField.isFocused && Input.GetKeyDown(KeyCode.Tab))
		{
			this.nextField.ActivateInputField();
		}
	}

	// Token: 0x04000890 RID: 2192
	public InputField nextField;

	// Token: 0x04000891 RID: 2193
	private InputField localField;
}
