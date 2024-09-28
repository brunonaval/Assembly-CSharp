using System;
using UnityEngine;
using UnityEngine.Events;

namespace DuloGames.UI.Tweens
{
	// Token: 0x02000686 RID: 1670
	public struct Vector2Tween : ITweenValue
	{
		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x0600250F RID: 9487 RVA: 0x000B43BD File Offset: 0x000B25BD
		// (set) Token: 0x06002510 RID: 9488 RVA: 0x000B43C5 File Offset: 0x000B25C5
		public Vector2 startVector2
		{
			get
			{
				return this.m_StartVector2;
			}
			set
			{
				this.m_StartVector2 = value;
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06002511 RID: 9489 RVA: 0x000B43CE File Offset: 0x000B25CE
		// (set) Token: 0x06002512 RID: 9490 RVA: 0x000B43D6 File Offset: 0x000B25D6
		public Vector2 targetVector2
		{
			get
			{
				return this.m_TargetVector2;
			}
			set
			{
				this.m_TargetVector2 = value;
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06002513 RID: 9491 RVA: 0x000B43DF File Offset: 0x000B25DF
		// (set) Token: 0x06002514 RID: 9492 RVA: 0x000B43E7 File Offset: 0x000B25E7
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

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06002515 RID: 9493 RVA: 0x000B43F0 File Offset: 0x000B25F0
		// (set) Token: 0x06002516 RID: 9494 RVA: 0x000B43F8 File Offset: 0x000B25F8
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

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06002517 RID: 9495 RVA: 0x000B4401 File Offset: 0x000B2601
		// (set) Token: 0x06002518 RID: 9496 RVA: 0x000B4409 File Offset: 0x000B2609
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

		// Token: 0x06002519 RID: 9497 RVA: 0x000B4412 File Offset: 0x000B2612
		public void TweenValue(float floatPercentage)
		{
			if (!this.ValidTarget())
			{
				return;
			}
			this.m_Target.Invoke(Vector2.Lerp(this.m_StartVector2, this.m_TargetVector2, floatPercentage));
		}

		// Token: 0x0600251A RID: 9498 RVA: 0x000B443A File Offset: 0x000B263A
		public void AddOnChangedCallback(UnityAction<Vector2> callback)
		{
			if (this.m_Target == null)
			{
				this.m_Target = new Vector2Tween.Vector2TweenCallback();
			}
			this.m_Target.AddListener(callback);
		}

		// Token: 0x0600251B RID: 9499 RVA: 0x000B445B File Offset: 0x000B265B
		public void AddOnFinishCallback(UnityAction callback)
		{
			if (this.m_Finish == null)
			{
				this.m_Finish = new Vector2Tween.Vector2TweenFinishCallback();
			}
			this.m_Finish.AddListener(callback);
		}

		// Token: 0x0600251C RID: 9500 RVA: 0x000B43F0 File Offset: 0x000B25F0
		public bool GetIgnoreTimescale()
		{
			return this.m_IgnoreTimeScale;
		}

		// Token: 0x0600251D RID: 9501 RVA: 0x000B43DF File Offset: 0x000B25DF
		public float GetDuration()
		{
			return this.m_Duration;
		}

		// Token: 0x0600251E RID: 9502 RVA: 0x000B447C File Offset: 0x000B267C
		public bool ValidTarget()
		{
			return this.m_Target != null;
		}

		// Token: 0x0600251F RID: 9503 RVA: 0x000B4487 File Offset: 0x000B2687
		public void Finished()
		{
			if (this.m_Finish != null)
			{
				this.m_Finish.Invoke();
			}
		}

		// Token: 0x04001E25 RID: 7717
		private Vector2 m_StartVector2;

		// Token: 0x04001E26 RID: 7718
		private Vector2 m_TargetVector2;

		// Token: 0x04001E27 RID: 7719
		private float m_Duration;

		// Token: 0x04001E28 RID: 7720
		private bool m_IgnoreTimeScale;

		// Token: 0x04001E29 RID: 7721
		private TweenEasing m_Easing;

		// Token: 0x04001E2A RID: 7722
		private Vector2Tween.Vector2TweenCallback m_Target;

		// Token: 0x04001E2B RID: 7723
		private Vector2Tween.Vector2TweenFinishCallback m_Finish;

		// Token: 0x02000687 RID: 1671
		public class Vector2TweenCallback : UnityEvent<Vector2>
		{
		}

		// Token: 0x02000688 RID: 1672
		public class Vector2TweenFinishCallback : UnityEvent
		{
		}
	}
}
