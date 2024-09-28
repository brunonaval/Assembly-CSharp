using System;
using Mirror;
using UnityEngine;

// Token: 0x02000233 RID: 563
public class QuestIconManager : MonoBehaviour
{
	// Token: 0x060007E6 RID: 2022 RVA: 0x00002E81 File Offset: 0x00001081
	private void Awake()
	{
		if (NetworkServer.active)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x00026104 File Offset: 0x00024304
	private void Update()
	{
		if (!NetworkServer.active)
		{
			if (this.increasing)
			{
				if (this.animationValue < this.maxSize)
				{
					this.animationValue += Time.deltaTime * this.speed;
					base.transform.localScale = new Vector3(this.animationValue, this.animationValue, this.animationValue);
					return;
				}
				this.increasing = false;
				return;
			}
			else
			{
				if (this.animationValue > this.minSize)
				{
					this.animationValue -= Time.deltaTime * this.speed;
					base.transform.localScale = new Vector3(this.animationValue, this.animationValue, this.animationValue);
					return;
				}
				this.increasing = true;
			}
		}
	}

	// Token: 0x040009C2 RID: 2498
	[SerializeField]
	private float speed = 0.1f;

	// Token: 0x040009C3 RID: 2499
	[SerializeField]
	private float maxSize = 0.15f;

	// Token: 0x040009C4 RID: 2500
	[SerializeField]
	private float minSize = 0.1f;

	// Token: 0x040009C5 RID: 2501
	[SerializeField]
	private float animationValue = 0.1f;

	// Token: 0x040009C6 RID: 2502
	private bool increasing = true;
}
