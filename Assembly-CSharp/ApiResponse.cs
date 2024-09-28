using System;

// Token: 0x02000037 RID: 55
[Serializable]
public class ApiResponse<T> where T : class
{
	// Token: 0x040000E4 RID: 228
	public bool Success;

	// Token: 0x040000E5 RID: 229
	public string Message;

	// Token: 0x040000E6 RID: 230
	public T ResponseObject;
}
