using System;
using System.Collections;

// Token: 0x02000029 RID: 41
public static class ApiDbQuests
{
	// Token: 0x06000081 RID: 129 RVA: 0x0000367A File Offset: 0x0000187A
	public static IEnumerator GetAllQuests(Action<QuestResource[]> callback)
	{
		yield return ApiManager.Get<QuestResource[]>(ApiDatabaseManager.Endpoints.GetAllQuestsEndpoint, delegate(ApiResponse<QuestResource[]> response)
		{
			callback(response.ResponseObject);
		});
		yield break;
	}
}
