using System;
using System.Collections;
using UnityEngine;

namespace DuloGames.UI.Tweens
{
	// Token: 0x02000684 RID: 1668
	internal class TweenRunner<T> where T : struct, ITweenValue
	{
		// Token: 0x06002504 RID: 9476 RVA: 0x000B41DE File Offset: 0x000B23DE
		private static IEnumerator Start(T tweenInfo)
		{
			if (!tweenInfo.ValidTarget())
			{
				yield break;
			}
			float elapsedTime = 0f;
			while (elapsedTime < tweenInfo.duration)
			{
				elapsedTime += (tweenInfo.ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime);
				float floatPercentage = TweenEasingHandler.Apply(tweenInfo.easing, elapsedTime, 0f, 1f, tweenInfo.duration);
				tweenInfo.TweenValue(floatPercentage);
				yield return null;
			}
			tweenInfo.TweenValue(1f);
			tweenInfo.Finished();
			yield break;
		}

		// Token: 0x06002505 RID: 9477 RVA: 0x000B41ED File Offset: 0x000B23ED
		public void Init(MonoBehaviour coroutineContainer)
		{
			this.m_CoroutineContainer = coroutineContainer;
		}

		// Token: 0x06002506 RID: 9478 RVA: 0x000B41F8 File Offset: 0x000B23F8
		public void StartTween(T info)
		{
			if (this.m_CoroutineContainer == null)
			{
				Debug.LogWarning("Coroutine container not configured... did you forget to call Init?");
				return;
			}
			this.StopTween();
			if (!this.m_CoroutineContainer.gameObject.activeInHierarchy)
			{
				info.TweenValue(1f);
				return;
			}
			this.m_Tween = TweenRunner<T>.Start(info);
			this.m_CoroutineContainer.StartCoroutine(this.m_Tween);
		}

		// Token: 0x06002507 RID: 9479 RVA: 0x000B4267 File Offset: 0x000B2467
		public void StopTween()
		{
			if (this.m_Tween != null)
			{
				this.m_CoroutineContainer.StopCoroutine(this.m_Tween);
				this.m_Tween = null;
			}
		}

		// Token: 0x04001E1F RID: 7711
		protected MonoBehaviour m_CoroutineContainer;

		// Token: 0x04001E20 RID: 7712
		protected IEnumerator m_Tween;
	}
}
