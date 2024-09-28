using System;
using Mirror;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class TeleportBrushManager : ActionBrushManager
{
	// Token: 0x06000048 RID: 72 RVA: 0x00002D89 File Offset: 0x00000F89
	private void Awake()
	{
		base.GetComponent<SpriteRenderer>().enabled = false;
		if (this.destinationAnchor != null)
		{
			this.destinationPosition = this.destinationAnchor.transform.position;
			UnityEngine.Object.Destroy(this.destinationAnchor);
		}
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00002DC8 File Offset: 0x00000FC8
	public override void ExecuteAction(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		if (this.requiredLevel != 0)
		{
			AttributeModule attributeModule;
			player.TryGetComponent<AttributeModule>(out attributeModule);
			if (attributeModule.BaseLevel < this.requiredLevel)
			{
				EffectModule effectModule;
				player.TryGetComponent<EffectModule>(out effectModule);
				effectModule.ShowScreenMessage("level_too_low_to_enter_area", 3, 3.5f, new string[]
				{
					this.requiredLevel.ToString()
				});
				return;
			}
		}
		if (!string.IsNullOrEmpty(this.destinationName))
		{
			Vector3 locationFromSpawnPoint = GlobalUtils.GetLocationFromSpawnPoint(this.destinationName);
			if (locationFromSpawnPoint == Vector3.zero)
			{
				return;
			}
			this.destinationPosition = locationFromSpawnPoint;
		}
		MovementModule component = player.GetComponent<MovementModule>();
		component.TargetTeleport(component.connectionToClient, this.destinationPosition, default(Effect));
	}

	// Token: 0x04000027 RID: 39
	[SerializeField]
	private GameObject destinationAnchor;

	// Token: 0x04000028 RID: 40
	[SerializeField]
	private string destinationName;

	// Token: 0x04000029 RID: 41
	[SerializeField]
	private int requiredLevel;

	// Token: 0x0400002A RID: 42
	private Vector3 destinationPosition;
}
