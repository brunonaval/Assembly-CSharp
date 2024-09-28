using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x0200042D RID: 1069
public class TitleDatabaseModule : MonoBehaviour
{
	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06001751 RID: 5969 RVA: 0x00076D38 File Offset: 0x00074F38
	// (set) Token: 0x06001752 RID: 5970 RVA: 0x00076D40 File Offset: 0x00074F40
	public bool IsLoaded { get; private set; }

	// Token: 0x06001753 RID: 5971 RVA: 0x0005D4A3 File Offset: 0x0005B6A3
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	// Token: 0x06001754 RID: 5972 RVA: 0x00076D49 File Offset: 0x00074F49
	private IEnumerator Start()
	{
		yield return this.ProcessTitles().WaitAsCoroutine();
		this.IsLoaded = true;
		yield break;
	}

	// Token: 0x06001755 RID: 5973 RVA: 0x00076D58 File Offset: 0x00074F58
	private Task ProcessTitles()
	{
		TitleDatabaseModule.<ProcessTitles>d__7 <ProcessTitles>d__;
		<ProcessTitles>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ProcessTitles>d__.<>4__this = this;
		<ProcessTitles>d__.<>1__state = -1;
		<ProcessTitles>d__.<>t__builder.Start<TitleDatabaseModule.<ProcessTitles>d__7>(ref <ProcessTitles>d__);
		return <ProcessTitles>d__.<>t__builder.Task;
	}

	// Token: 0x06001756 RID: 5974 RVA: 0x00076D9C File Offset: 0x00074F9C
	private Title BuildTitle(DataRow dbTitle)
	{
		return new Title((dbTitle["Id"] as int?).GetValueOrDefault(), dbTitle["Name"].ToString(), dbTitle["Description"].ToString());
	}

	// Token: 0x06001757 RID: 5975 RVA: 0x00076DEC File Offset: 0x00074FEC
	private void Add(Title title)
	{
		if (this.titles.Any((Title a) => a.Id == title.Id))
		{
			Debug.LogErrorFormat("Can't add title, id [{0}] already exists.", new object[]
			{
				title.Id
			});
			return;
		}
		this.titles.Add(title);
	}

	// Token: 0x06001758 RID: 5976 RVA: 0x00076E54 File Offset: 0x00075054
	public Title[] GetTitles()
	{
		return this.titles.ToArray();
	}

	// Token: 0x06001759 RID: 5977 RVA: 0x00076E64 File Offset: 0x00075064
	public Title GetTitle(int id)
	{
		return this.titles.FirstOrDefault((Title t) => t.Id == id);
	}

	// Token: 0x040014CD RID: 5325
	private readonly List<Title> titles = new List<Title>();
}
