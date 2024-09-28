using System;
using UnityEngine;
using UnityEngine.Events;

namespace DuloGames.UI.Tweens
{
	// Token: 0x02000689 RID: 1673
	public struct Vector3Tween : ITweenValue
	{
		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06002522 RID: 9506 RVA: 0x000B44A4 File Offset: 0x000B26A4
		// (set) Token: 0x06002523 RID: 9507 RVA: 0x000B44AC File Offset: 0x000B26AC
		public Vector3 startVector3
		{
			get
			{
				return this.m_StartVector3;
			}
			set
			{
				this.m_StartVector3 = value;
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06002524 RID: 9508 RVA: 0x000B44B5 File Offset: 0x000B26B5
		// (set) Token: 0x06002525 RID: 9509 RVA: 0x000B44BD File Offset: 0x000B26BD
		public Vector3 targetVector3
		{
			get
			{
				return this.m_TargetVector3;
			}
			set
			{
				this.m_TargetVector3 = value;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06002526 RID: 9510 RVA: 0x000B44C6 File Offset: 0x000B26C6
		// (set) Token: 0x06002527 RID: 9511 RVA: 0x000B44CE File Offset: 0x000B26CE
		public float duration
		{
			get
			{
				return this.m_Duration;
			}
			set
			{
				this.m_Duration = value;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06002528 RID: 9512 RVA: 0x000B44D7 File Offset: 0x000B26D7
		// (set) Token: 0x06002529 RID: 9513 RVA: 0x000B44DF File Offset: 0x000B26DF
		public bool ignoreTimeScale
		{
			get
			{
				return this.m_IgnoreTimeScale;
			}
			set
			{
				this.m_IgnoreTimeScale = value;
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x0600252A RID: 9514 RVA: 0x000B44E8 File Offset: 0x000B26E8
		// (set) Token: 0x0600252B RID: 9515 RVA: 0x000B44F0 File Offset: 0x000B26F0
		public TweenEasing easing
		{
			get
			{
				return this.m_Easing;
			}
			set
			{
				this.m_Easing = value;
			}
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x000B44F9 File Offset: 0x000B26F9
		public void TweenValue(float floatPercentage)
		{
			if (!this.ValidTarget())
			{
				return;
			}
			this.m_Target.Invoke(Vector3.Lerp(this.m_StartVector3, this.m_TargetVector3, floatPercentage));
		}

		// Token: 0x0600252D RID: 9517 RVA: 0x000B4521 File Offset: 0x000B2721
		public void AddOnChangedCallback(UnityAction<Vector3> callback)
		{
			if (this.m_Target == null)
			{
				this.m_Target = new Vector3Tween.Vector3TweenCallback();
			}
			this.m_Target.AddListener(callback);
		}

		// Token: 0x0600252E RID: 9518 RVA: 0x000B4542 File Offset: 0x000B2742
		public void AddOnFinishCallback(UnityAction callback)
		{
			if (this.m_Finish == null)
			{
				this.m_Finish = new Vector3Tween.Vector3TweenFinishCallback();
			}
			this.m_Finish.AddListener(callback);
		}

		// Token: 0x0600252F RID: 9519 RVA: 0x000B44D7 File Offset: 0x000B26D7
		public bool GetIgnoreTimescale()
		{
			return this.m_IgnoreTimeScale;
		}

		// Token: 0x06002530 RID: 9520 RVA: 0x000B44C6 File Offset: 0x000B26C6
		public float GetDuration()
		{
			return this.m_Duration;
		}

		// Token: 0x06002531 RID: 9521 RVA: 0x000B4563 File Offset: 0x000B2763
		public bool ValidTarget()
		{
			return this.m_Target != null;
		}

		// Token: 0x06002532 RID: 9522 RVA: 0x000B456E File Offset: 0x000B276E
		public void Finished()
		{
			if (this.m_Finish != null)
			{
				this.m_Finish.Invoke();
			}
		}

		// Token: 0x04001E2C RID: 7724
		private Vector3 m_StartVector3;

		// Token: 0x04001E2D RID: 7725
		private Vector3 m_TargetVector3;

		// Token: 0x04001E2E RID: 7726
		private float m_Duration;

		// Token: 0x04001E2F RID: 7727
		private bool m_IgnoreTimeScale;

		// Token: 0x04001E30 RID: 7728
		private TweenEasing m_Easing;

		// Token: 0x04001E31 RID: 7729
		private Vector3Tween.Vector3TweenCallback m_Target;

		// Token: 0x04001E32 RID: 7730
		private Vector3Tween.Vector3TweenFinishCallback m_Finish;

		// Token: 0x0200068A RID: 1674
		public class Vector3TweenCallback : UnityEvent<Vector3>
		{
		}

		// Token: 0x0200068B RID: 1675
		public class Vector3TweenFinishCallback : UnityEvent
		{
		}
	}
}
