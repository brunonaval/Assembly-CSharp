using System;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.Events;

namespace DuloGames.UI
{
	// Token: 0x020005A9 RID: 1449
	public class Test_CompassTester : MonoBehaviour
	{
		// Token: 0x06001FF6 RID: 8182 RVA: 0x000A0734 File Offset: 0x0009E934
		protected Test_CompassTester()
		{
			if (this.m_FloatTweenRunner == null)
			{
				this.m_FloatTweenRunner = new TweenRunner<FloatTween>();
			}
			this.m_FloatTweenRunner.Init(this);
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x000A0766 File Offset: 0x0009E966
		protected void OnEnable()
		{
			this.StartTween(360f, this.m_Duration, true);
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x000A077C File Offset: 0x0009E97C
		private void StartTween(float targetRotation, float duration, bool ignoreTimeScale)
		{
			float y = base.transform.eulerAngles.y;
			if (y.Equals(targetRotation))
			{
				return;
			}
			FloatTween info = new FloatTween
			{
				duration = duration,
				startFloat = y,
				targetFloat = targetRotation
			};
			info.AddOnChangedCallback(new UnityAction<float>(this.SetRotation));
			info.AddOnFinishCallback(new UnityAction(this.OnTweenFinished));
			info.ignoreTimeScale = ignoreTimeScale;
			info.easing = this.m_Easing;
			this.m_FloatTweenRunner.StartTween(info);
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x000A080F File Offset: 0x0009EA0F
		private void SetRotation(float rotation)
		{
			base.transform.eulerAngles = new Vector3(base.transform.rotation.x, rotation, base.transform.rotation.z);
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x000A0766 File Offset: 0x0009E966
		protected virtual void OnTweenFinished()
		{
			this.StartTween(360f, this.m_Duration, true);
		}

		// Token: 0x040019C7 RID: 6599
		[SerializeField]
		private float m_Duration = 4f;

		// Token: 0x040019C8 RID: 6600
		[SerializeField]
		private TweenEasing m_Easing;

		// Token: 0x040019C9 RID: 6601
		[NonSerialized]
		private readonly TweenRunner<FloatTween> m_FloatTweenRunner;
	}
}
