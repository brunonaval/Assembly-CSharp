using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// Token: 0x02000367 RID: 871
public class LightModule : MonoBehaviour
{
	// Token: 0x06001169 RID: 4457 RVA: 0x00052493 File Offset: 0x00050693
	private void Awake()
	{
		this.conditionModule = base.GetComponent<ConditionModule>();
	}

	// Token: 0x0600116A RID: 4458 RVA: 0x000524A4 File Offset: 0x000506A4
	public void Start()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("GameEnvironment");
		this.gameEnvironmentModule = gameObject.GetComponent<GameEnvironmentModule>();
		if (this.conditionModule.isLocalPlayer)
		{
			this.globalLight = gameObject.GetComponent<Light2D>();
			base.StartCoroutine(this.DayNightDetector());
		}
		if (!this.conditionModule.isLocalPlayer)
		{
			this.assistiveLight.enabled = false;
		}
	}

	// Token: 0x0600116B RID: 4459 RVA: 0x00052507 File Offset: 0x00050707
	private IEnumerator DayNightDetector()
	{
		float lastAmbientLight = -1f;
		WaitForSecondsRealtime delay = new WaitForSecondsRealtime(0.2f);
		for (;;)
		{
			if (SettingsManager.Instance.DisableLightSystem)
			{
				this.assistiveLight.enabled = false;
				yield return delay;
			}
			else
			{
				float ambientLight = this.gameEnvironmentModule.AmbientLight;
				bool isInvisible = this.conditionModule.IsInvisible;
				bool flag = ambientLight <= 0.6f && (this.conditionModule.isLocalPlayer | !isInvisible);
				if (lastAmbientLight != ambientLight)
				{
					lastAmbientLight = ambientLight;
					this.UpdateAssistiveLight();
				}
				if (flag && !this.assistiveLight.enabled)
				{
					this.assistiveLight.enabled = true;
				}
				else if (!flag && this.assistiveLight.enabled)
				{
					this.assistiveLight.enabled = false;
				}
				yield return delay;
			}
		}
		yield break;
	}

	// Token: 0x0600116C RID: 4460 RVA: 0x00052518 File Offset: 0x00050718
	private void UpdateAssistiveLight()
	{
		float h;
		float s;
		float num;
		Color.RGBToHSV(this.globalLight.color, out h, out s, out num);
		this.assistiveLight.color = Color.HSVToRGB(h, s, 1f);
	}

	// Token: 0x0400105E RID: 4190
	private Light2D globalLight;

	// Token: 0x0400105F RID: 4191
	[SerializeField]
	private Light2D assistiveLight;

	// Token: 0x04001060 RID: 4192
	private ConditionModule conditionModule;

	// Token: 0x04001061 RID: 4193
	private GameEnvironmentModule gameEnvironmentModule;
}
