using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x020001D6 RID: 470
public static class ApiManager
{
	// Token: 0x0600057F RID: 1407 RVA: 0x0001D3A9 File Offset: 0x0001B5A9
	public static IEnumerator Post<T>(string url, object payload, Action<ApiResponse<T>> callback = null) where T : class
	{
		string s = JsonUtility.ToJson(payload);
		using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
		{
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			request.uploadHandler = new UploadHandlerRaw(bytes);
			request.downloadHandler = new DownloadHandlerBuffer();
			request.SetRequestHeader("Content-Type", "application/json");
			request.timeout = 60;
			yield return request.SendWebRequest();
			ApiResponse<T> apiResponse = new ApiResponse<T>();
			if (request.result == UnityWebRequest.Result.ConnectionError)
			{
				apiResponse.Success = false;
				apiResponse.Message = "NETWORK ERROR: " + request.error + " / " + request.downloadHandler.text;
				Debug.Log("(POST) Request Failure (" + url + "): " + apiResponse.Message);
				if (callback != null)
				{
					callback(apiResponse);
				}
				yield break;
			}
			if (request.result == UnityWebRequest.Result.ProtocolError)
			{
				ApiResponse<T> apiResponse2 = null;
				try
				{
					apiResponse2 = JsonUtility.FromJson<ApiResponse<T>>(request.downloadHandler.text);
				}
				catch
				{
				}
				if (apiResponse2 != null)
				{
					if (callback != null)
					{
						callback(apiResponse2);
					}
					yield break;
				}
				apiResponse.Success = false;
				apiResponse.Message = "HTTP ERROR: " + request.error + " / " + request.downloadHandler.text;
				Debug.Log("(POST) Request Failure (" + url + "): " + apiResponse.Message);
				if (callback != null)
				{
					callback(apiResponse);
				}
				yield break;
			}
			else
			{
				apiResponse = JsonUtility.FromJson<ApiResponse<T>>(request.downloadHandler.text);
				if (callback != null)
				{
					callback(apiResponse);
				}
			}
		}
		UnityWebRequest request = null;
		yield break;
		yield break;
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x0001D3C6 File Offset: 0x0001B5C6
	public static IEnumerator Get<T>(string url, Action<ApiResponse<T>> callback) where T : class
	{
		using (UnityWebRequest request = UnityWebRequest.Get(url))
		{
			request.timeout = 30;
			yield return request.SendWebRequest();
			ApiResponse<T> apiResponse = new ApiResponse<T>();
			if (request.result == UnityWebRequest.Result.ConnectionError)
			{
				apiResponse.Success = false;
				apiResponse.Message = "NETWORK ERROR: " + request.error + " / " + request.downloadHandler.text;
				Debug.Log("(GET) Request Failure (" + url + "): " + apiResponse.Message);
				if (callback != null)
				{
					callback(apiResponse);
				}
				yield break;
			}
			if (request.result == UnityWebRequest.Result.ProtocolError)
			{
				apiResponse.Success = false;
				apiResponse.Message = "HTTP ERROR: " + request.error + " / " + request.downloadHandler.text;
				Debug.Log("(GET) Request Failure (" + url + "): " + apiResponse.Message);
				if (callback != null)
				{
					callback(apiResponse);
				}
				yield break;
			}
			apiResponse = JsonUtility.FromJson<ApiResponse<T>>(request.downloadHandler.text);
			if (callback != null)
			{
				callback(apiResponse);
			}
		}
		UnityWebRequest request = null;
		yield break;
		yield break;
	}
}
