using System;
using UnityEngine;

// Token: 0x020001EF RID: 495
public class CursorManager : MonoBehaviour
{
	// Token: 0x06000625 RID: 1573 RVA: 0x0001F7D4 File Offset: 0x0001D9D4
	private void Start()
	{
		if (!GlobalSettings.IsMobilePlatform)
		{
			Cursor.SetCursor(this.cursorTexture, this.hotSpot, this.cursorMode);
		}
	}

	// Token: 0x0400086B RID: 2155
	public Texture2D cursorTexture;

	// Token: 0x0400086C RID: 2156
	public CursorMode cursorMode;

	// Token: 0x0400086D RID: 2157
	public Vector2 hotSpot = Vector2.zero;
}
