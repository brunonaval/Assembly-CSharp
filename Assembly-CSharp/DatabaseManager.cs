using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x0200003F RID: 63
public static class DatabaseManager
{
	// Token: 0x060000A3 RID: 163 RVA: 0x00003AA0 File Offset: 0x00001CA0
	static DatabaseManager()
	{
		if (string.IsNullOrEmpty(DatabaseManager.ConnectionString))
		{
			string text = Path.Combine(Environment.CurrentDirectory, "Assets");
			text = Path.Combine(text, "Miscellaneous");
			text = Path.Combine(text, "Settings");
			text = Path.Combine(text, "server.json");
			if (!File.Exists(text))
			{
				Debug.LogError("[DatabaseManager] Missing server.json file on path: " + text);
			}
			if (File.Exists(text))
			{
				ServerSettings serverSettings = JsonUtility.FromJson<ServerSettings>(File.ReadAllText(text));
				if (serverSettings != null)
				{
					DatabaseManager.ConnectionString = serverSettings.ConnectionString;
				}
			}
		}
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x00003B28 File Offset: 0x00001D28
	public static Task<int> ExecuteNonQueryAsync(string commandText, params SqlParameter[] parameters)
	{
		DatabaseManager.<ExecuteNonQueryAsync>d__2 <ExecuteNonQueryAsync>d__;
		<ExecuteNonQueryAsync>d__.<>t__builder = AsyncTaskMethodBuilder<int>.Create();
		<ExecuteNonQueryAsync>d__.commandText = commandText;
		<ExecuteNonQueryAsync>d__.parameters = parameters;
		<ExecuteNonQueryAsync>d__.<>1__state = -1;
		<ExecuteNonQueryAsync>d__.<>t__builder.Start<DatabaseManager.<ExecuteNonQueryAsync>d__2>(ref <ExecuteNonQueryAsync>d__);
		return <ExecuteNonQueryAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x00003B74 File Offset: 0x00001D74
	public static Task<int> ExecuteNonQueryAsync(SqlCommand command, string commandText, params SqlParameter[] parameters)
	{
		DatabaseManager.<ExecuteNonQueryAsync>d__3 <ExecuteNonQueryAsync>d__;
		<ExecuteNonQueryAsync>d__.<>t__builder = AsyncTaskMethodBuilder<int>.Create();
		<ExecuteNonQueryAsync>d__.command = command;
		<ExecuteNonQueryAsync>d__.commandText = commandText;
		<ExecuteNonQueryAsync>d__.parameters = parameters;
		<ExecuteNonQueryAsync>d__.<>1__state = -1;
		<ExecuteNonQueryAsync>d__.<>t__builder.Start<DatabaseManager.<ExecuteNonQueryAsync>d__3>(ref <ExecuteNonQueryAsync>d__);
		return <ExecuteNonQueryAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00003BC8 File Offset: 0x00001DC8
	public static Task<object> ExecuteScalarAsync(string commandText, params SqlParameter[] parameters)
	{
		DatabaseManager.<ExecuteScalarAsync>d__4 <ExecuteScalarAsync>d__;
		<ExecuteScalarAsync>d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
		<ExecuteScalarAsync>d__.commandText = commandText;
		<ExecuteScalarAsync>d__.parameters = parameters;
		<ExecuteScalarAsync>d__.<>1__state = -1;
		<ExecuteScalarAsync>d__.<>t__builder.Start<DatabaseManager.<ExecuteScalarAsync>d__4>(ref <ExecuteScalarAsync>d__);
		return <ExecuteScalarAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00003C14 File Offset: 0x00001E14
	public static Task<object> ExecuteScalarAsync(SqlCommand command, string commandText, params SqlParameter[] parameters)
	{
		DatabaseManager.<ExecuteScalarAsync>d__5 <ExecuteScalarAsync>d__;
		<ExecuteScalarAsync>d__.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
		<ExecuteScalarAsync>d__.command = command;
		<ExecuteScalarAsync>d__.commandText = commandText;
		<ExecuteScalarAsync>d__.parameters = parameters;
		<ExecuteScalarAsync>d__.<>1__state = -1;
		<ExecuteScalarAsync>d__.<>t__builder.Start<DatabaseManager.<ExecuteScalarAsync>d__5>(ref <ExecuteScalarAsync>d__);
		return <ExecuteScalarAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00003C68 File Offset: 0x00001E68
	public static Task<DataRow[]> ExecuteTableAsync(string commandText, params SqlParameter[] parameters)
	{
		DatabaseManager.<ExecuteTableAsync>d__6 <ExecuteTableAsync>d__;
		<ExecuteTableAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<ExecuteTableAsync>d__.commandText = commandText;
		<ExecuteTableAsync>d__.parameters = parameters;
		<ExecuteTableAsync>d__.<>1__state = -1;
		<ExecuteTableAsync>d__.<>t__builder.Start<DatabaseManager.<ExecuteTableAsync>d__6>(ref <ExecuteTableAsync>d__);
		return <ExecuteTableAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00003CB4 File Offset: 0x00001EB4
	public static Task<SqlConnection> CreateSqlConnectionAsync()
	{
		DatabaseManager.<CreateSqlConnectionAsync>d__7 <CreateSqlConnectionAsync>d__;
		<CreateSqlConnectionAsync>d__.<>t__builder = AsyncTaskMethodBuilder<SqlConnection>.Create();
		<CreateSqlConnectionAsync>d__.<>1__state = -1;
		<CreateSqlConnectionAsync>d__.<>t__builder.Start<DatabaseManager.<CreateSqlConnectionAsync>d__7>(ref <CreateSqlConnectionAsync>d__);
		return <CreateSqlConnectionAsync>d__.<>t__builder.Task;
	}

	// Token: 0x04000106 RID: 262
	public static string ConnectionString;
}
