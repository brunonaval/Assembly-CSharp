using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x020003AF RID: 943
public class PlayerDatabaseModule : MonoBehaviour
{
	// Token: 0x1700021C RID: 540
	// (get) Token: 0x060013B2 RID: 5042 RVA: 0x000613D8 File Offset: 0x0005F5D8
	// (set) Token: 0x060013B3 RID: 5043 RVA: 0x000613E0 File Offset: 0x0005F5E0
	public bool IsLoaded { get; private set; }

	// Token: 0x060013B4 RID: 5044 RVA: 0x0005D4A3 File Offset: 0x0005B6A3
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	// Token: 0x060013B5 RID: 5045 RVA: 0x000613E9 File Offset: 0x0005F5E9
	private IEnumerator Start()
	{
		yield return this.ProcessPlayers().WaitAsCoroutine();
		this.IsLoaded = true;
		yield break;
	}

	// Token: 0x060013B6 RID: 5046 RVA: 0x000613F8 File Offset: 0x0005F5F8
	private Task ProcessPlayers()
	{
		PlayerDatabaseModule.<ProcessPlayers>d__7 <ProcessPlayers>d__;
		<ProcessPlayers>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ProcessPlayers>d__.<>4__this = this;
		<ProcessPlayers>d__.<>1__state = -1;
		<ProcessPlayers>d__.<>t__builder.Start<PlayerDatabaseModule.<ProcessPlayers>d__7>(ref <ProcessPlayers>d__);
		return <ProcessPlayers>d__.<>t__builder.Task;
	}

	// Token: 0x060013B7 RID: 5047 RVA: 0x0006143C File Offset: 0x0005F63C
	private Player BuildPlayer(DataRow dbPlayer)
	{
		return new Player(((int?)dbPlayer["Id"]).GetValueOrDefault(), ((int?)dbPlayer["AccountId"]).GetValueOrDefault(), dbPlayer["Name"].ToString(), ((int?)dbPlayer["BaseLevel"]) ?? 1);
	}

	// Token: 0x060013B8 RID: 5048 RVA: 0x000614B4 File Offset: 0x0005F6B4
	public void Add(Player player)
	{
		if (this.players.Any((Player a) => a.Id == player.Id))
		{
			Debug.LogErrorFormat("Can't add player, id [{0}] already exists.", new object[]
			{
				player.Id
			});
			return;
		}
		this.players.Add(player);
	}

	// Token: 0x060013B9 RID: 5049 RVA: 0x0006151C File Offset: 0x0005F71C
	public Player GetPlayer(int playerId)
	{
		return this.players.FirstOrDefault((Player f) => f.Id == playerId);
	}

	// Token: 0x04001224 RID: 4644
	private List<Player> players = new List<Player>();
}
