using System;
using System.Collections.Generic;
using UnityEngine;

namespace GooglePlayGames.BasicApi
{
	// Token: 0x020006A2 RID: 1698
	public class SignInHelper
	{
		// Token: 0x06002561 RID: 9569 RVA: 0x000B4BC4 File Offset: 0x000B2DC4
		public static SignInStatus ToSignInStatus(int code)
		{
			Dictionary<int, SignInStatus> dictionary = new Dictionary<int, SignInStatus>
			{
				{
					-12,
					SignInStatus.AlreadyInProgress
				},
				{
					0,
					SignInStatus.Success
				},
				{
					4,
					SignInStatus.UiSignInRequired
				},
				{
					7,
					SignInStatus.NetworkError
				},
				{
					8,
					SignInStatus.InternalError
				},
				{
					10,
					SignInStatus.DeveloperError
				},
				{
					16,
					SignInStatus.Canceled
				},
				{
					17,
					SignInStatus.Failed
				},
				{
					12500,
					SignInStatus.Failed
				},
				{
					12501,
					SignInStatus.Canceled
				},
				{
					12502,
					SignInStatus.AlreadyInProgress
				}
			};
			if (!dictionary.ContainsKey(code))
			{
				return SignInStatus.Failed;
			}
			return dictionary[code];
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x000B4C51 File Offset: 0x000B2E51
		public static void SetPromptUiSignIn(bool value)
		{
			PlayerPrefs.SetInt("prompt_sign_in", value ? SignInHelper.True : SignInHelper.False);
		}

		// Token: 0x06002563 RID: 9571 RVA: 0x000B4C6C File Offset: 0x000B2E6C
		public static bool ShouldPromptUiSignIn()
		{
			return PlayerPrefs.GetInt("prompt_sign_in", SignInHelper.True) != SignInHelper.False;
		}

		// Token: 0x04001E89 RID: 7817
		private static int True = 0;

		// Token: 0x04001E8A RID: 7818
		private static int False = 1;

		// Token: 0x04001E8B RID: 7819
		private const string PromptSignInKey = "prompt_sign_in";
	}
}
