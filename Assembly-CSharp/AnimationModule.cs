using System;
using System.Collections;
using System.Linq;
using Mirror;
using UnityEngine;

// Token: 0x020002A3 RID: 675
[RequireComponent(typeof(SpriteRenderer))]
public class AnimationModule : MonoBehaviour
{
	// Token: 0x06000ABE RID: 2750 RVA: 0x00030BB0 File Offset: 0x0002EDB0
	private void Start()
	{
		this.effectModule = base.GetComponentInParent<EffectModule>();
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		this.movementModule = base.GetComponentInParent<MovementModule>();
		this.creatureModule = base.GetComponentInParent<CreatureModule>();
		this.conditionModule = base.GetComponentInParent<ConditionModule>();
		if (NetworkServer.active)
		{
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
		this.LoadGenderSkinSprites();
		if (!this.creatureModule.IsAlive)
		{
			this.spriteRenderer.enabled = true;
			this.AnimateDeath();
		}
		this.originalColor = this.spriteRenderer.color;
	}

	// Token: 0x06000ABF RID: 2751 RVA: 0x00030C4E File Offset: 0x0002EE4E
	public void LoadGenderSkinSprites()
	{
		if (this.creatureModule.Gender == CreatureGender.Male)
		{
			this.sprites = this.MaleSprites;
			return;
		}
		if (this.creatureModule.Gender == CreatureGender.Female)
		{
			this.sprites = this.FemaleSprites;
		}
	}

	// Token: 0x06000AC0 RID: 2752 RVA: 0x00030C84 File Offset: 0x0002EE84
	private void ChangeScaleByRank()
	{
		float num = GlobalUtils.RankToPercentScale(this.creatureModule.Rank);
		if (num == 0f & base.transform.localScale.x == 1f)
		{
			return;
		}
		base.transform.localScale = new Vector3(1f + num, 1f + num, 1f + num);
	}

	// Token: 0x06000AC1 RID: 2753 RVA: 0x00030CEA File Offset: 0x0002EEEA
	private void Update()
	{
		if (NetworkServer.active)
		{
			return;
		}
		if (this.UpdateRespawningVisibility())
		{
			return;
		}
		if (this.UpdateConditionVisibility())
		{
			return;
		}
		this.ChangeScaleByRank();
		this.MakeVisible();
	}

	// Token: 0x06000AC2 RID: 2754 RVA: 0x00030D12 File Offset: 0x0002EF12
	private bool UpdateRespawningVisibility()
	{
		if (!this.creatureModule.IsRespawning)
		{
			return false;
		}
		this.MakeInvisible(false);
		return true;
	}

	// Token: 0x06000AC3 RID: 2755 RVA: 0x00030D2C File Offset: 0x0002EF2C
	private bool UpdateConditionVisibility()
	{
		bool flag = this.conditionModule.HasActiveCondition(ConditionCategory.Transformation);
		bool flag2 = this.conditionModule.HasActiveCondition(ConditionCategory.Invisibility);
		if (!flag & !flag2)
		{
			return false;
		}
		this.MakeInvisible(flag);
		return true;
	}

	// Token: 0x06000AC4 RID: 2756 RVA: 0x00030D6C File Offset: 0x0002EF6C
	private void MakeInvisible(bool isTransformed)
	{
		if ((this.conditionModule.HasActiveCondition(ConditionType.Vanish) || isTransformed) | this.creatureModule.IsRespawning)
		{
			this.spriteRenderer.enabled = false;
			return;
		}
		Color color = this.spriteRenderer.color;
		bool flag = this.uiSystemModule.PartyModule.IsMember(this.conditionModule.gameObject);
		bool flag2 = this.uiSystemModule.AttributeModule.AccessLevel > AccessLevel.Player;
		if (this.conditionModule.isLocalPlayer || flag2 || flag)
		{
			color.a = 0.2f;
		}
		else
		{
			color.a = 0f;
		}
		this.spriteRenderer.color = color;
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x00030E18 File Offset: 0x0002F018
	private void MakeVisible()
	{
		Color color = this.spriteRenderer.color;
		color.a = 1f;
		this.spriteRenderer.color = color;
		if (!this.spriteRenderer.enabled)
		{
			this.spriteRenderer.enabled = true;
		}
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x00030E62 File Offset: 0x0002F062
	public void SetColorToOriginal()
	{
		if (this.spriteRenderer != null)
		{
			this.spriteRenderer.color = this.originalColor;
		}
	}

	// Token: 0x06000AC7 RID: 2759 RVA: 0x00030E83 File Offset: 0x0002F083
	public void SetColor(int colorId)
	{
		if (this.spriteRenderer != null && GlobalSettings.Colors.ContainsKey(colorId))
		{
			this.spriteRenderer.color = GlobalSettings.Colors[colorId];
		}
	}

	// Token: 0x06000AC8 RID: 2760 RVA: 0x00030EB6 File Offset: 0x0002F0B6
	private void StartAnimating()
	{
		this.isAnimating = true;
	}

	// Token: 0x06000AC9 RID: 2761 RVA: 0x00030EBF File Offset: 0x0002F0BF
	private void StopAnimating()
	{
		this.isAnimating = false;
	}

	// Token: 0x06000ACA RID: 2762 RVA: 0x00030EC8 File Offset: 0x0002F0C8
	public void AnimateWalk(AnimationConfig animationConfig)
	{
		if (this.spriteRenderer != null)
		{
			if (this.sprites != null && !this.isAnimating)
			{
				Direction direction = this.movementModule.Direction;
				if (animationConfig.ForceDirection)
				{
					direction = animationConfig.Direction;
				}
				switch (direction)
				{
				case Direction.North:
					this.RunAnimateWalk(this.WalkNorth, animationConfig);
					return;
				case Direction.West:
					this.RunAnimateWalk(this.WalkWest, animationConfig);
					return;
				case Direction.South:
					this.RunAnimateWalk(this.WalkSouth, animationConfig);
					return;
				case Direction.East:
					this.RunAnimateWalk(this.WalkEast, animationConfig);
					return;
				default:
					return;
				}
			}
			else if (this.sprites == null)
			{
				this.spriteRenderer.sprite = null;
			}
		}
	}

	// Token: 0x06000ACB RID: 2763 RVA: 0x00030F74 File Offset: 0x0002F174
	private void RunAnimateWalk(FrameConfig frameConfig, AnimationConfig animationConfig)
	{
		if (!this.creatureModule.IsAlive)
		{
			return;
		}
		float num = (animationConfig.FrameSpeed > 0f) ? animationConfig.FrameSpeed : 13f;
		if (frameConfig.Index < 0 | frameConfig.Frames < 1)
		{
			this.spriteRenderer.sprite = null;
		}
		else
		{
			Sprite[] array = this.sprites.Skip(frameConfig.Index).Take(frameConfig.Frames).ToArray<Sprite>();
			if (array.Any<Sprite>())
			{
				int num2 = (int)(Time.timeSinceLevelLoad * num);
				int num3 = num2 % array.Length;
				this.spriteRenderer.sprite = array[num3];
				if (!string.IsNullOrEmpty(this.WalkSoundEffect) & this.lastWalkSoundFrame != (float)num2 & (num3 == 0 | num3 == 4))
				{
					this.lastWalkSoundFrame = (float)num2;
					if (!this.IsInvisible)
					{
						this.effectModule.PlaySoundEffect(this.WalkSoundEffect, 0.3f, 0f, Vector3.zero);
					}
				}
			}
		}
		if (!this.creatureModule.IsAlive)
		{
			base.StartCoroutine(this.RunDying(this.Dying));
		}
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x00031090 File Offset: 0x0002F290
	public void AnimateStand()
	{
		if (this.spriteRenderer != null)
		{
			if (this.sprites != null && !this.isAnimating)
			{
				switch (this.movementModule.Direction)
				{
				case Direction.North:
					this.RunAnimateStand(this.StandNorth);
					return;
				case Direction.West:
					this.RunAnimateStand(this.StandWest);
					return;
				case Direction.South:
					this.RunAnimateStand(this.StandSouth);
					return;
				case Direction.East:
					this.RunAnimateStand(this.StandEast);
					return;
				default:
					return;
				}
			}
			else if (this.sprites == null)
			{
				this.spriteRenderer.sprite = null;
			}
		}
	}

	// Token: 0x06000ACD RID: 2765 RVA: 0x00031128 File Offset: 0x0002F328
	private void RunAnimateStand(FrameConfig frameConfig)
	{
		if (!this.creatureModule.IsAlive)
		{
			return;
		}
		if (frameConfig.Index < 0 | frameConfig.Frames < 1)
		{
			this.spriteRenderer.sprite = null;
		}
		else
		{
			Sprite[] array = this.sprites.Skip(frameConfig.Index).Take(frameConfig.Frames).ToArray<Sprite>();
			if (array.Any<Sprite>())
			{
				int num = (int)(Time.timeSinceLevelLoad * 13f);
				num %= array.Length;
				this.spriteRenderer.sprite = array[num];
			}
		}
		if (!this.creatureModule.IsAlive)
		{
			base.StartCoroutine(this.RunDying(this.Dying));
		}
	}

	// Token: 0x06000ACE RID: 2766 RVA: 0x000311D4 File Offset: 0x0002F3D4
	public void AnimateCast(AnimationConfig animationConfig)
	{
		if (this.spriteRenderer != null)
		{
			if (this.sprites != null)
			{
				Direction direction = this.movementModule.Direction;
				if (animationConfig.ForceDirection)
				{
					direction = animationConfig.Direction;
				}
				switch (direction)
				{
				case Direction.North:
					base.StartCoroutine(this.RunAnimateCast(this.CastNorth, animationConfig));
					return;
				case Direction.West:
					base.StartCoroutine(this.RunAnimateCast(this.CastWest, animationConfig));
					return;
				case Direction.South:
					base.StartCoroutine(this.RunAnimateCast(this.CastSouth, animationConfig));
					return;
				case Direction.East:
					base.StartCoroutine(this.RunAnimateCast(this.CastEast, animationConfig));
					return;
				default:
					return;
				}
			}
			else if (this.sprites == null)
			{
				this.spriteRenderer.sprite = null;
			}
		}
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x00031297 File Offset: 0x0002F497
	private IEnumerator RunAnimateCast(FrameConfig frameConfig, AnimationConfig animationConfig)
	{
		try
		{
			float seconds = (animationConfig.FrameSpeed > 0f) ? animationConfig.FrameSpeed : 0.1f;
			WaitForSeconds waitForFrameSpeed = new WaitForSeconds(seconds);
			WaitForSeconds waitForFreezeInterval = new WaitForSeconds(animationConfig.FreezeInterval);
			this.StartAnimating();
			if (frameConfig.Frames < 1)
			{
				this.spriteRenderer.sprite = null;
				this.StopAnimating();
				yield break;
			}
			int index = frameConfig.Index;
			int lastFrame = index + frameConfig.Frames;
			int frame = index;
			int frameCounter = 0;
			int num;
			for (int i = index; i < lastFrame; i = num + 1)
			{
				if (!this.creatureModule.IsAlive)
				{
					yield break;
				}
				if (frameConfig.Gaps.Contains(frameCounter) | frameConfig.Index < 0)
				{
					this.spriteRenderer.sprite = null;
					yield return waitForFrameSpeed;
					if (animationConfig.FreezeInterval > 0f && animationConfig.FreezeFrame == frameCounter)
					{
						yield return waitForFreezeInterval;
					}
				}
				else
				{
					this.spriteRenderer.sprite = this.sprites[frame];
					yield return waitForFrameSpeed;
					if (animationConfig.FreezeInterval > 0f && animationConfig.FreezeFrame == frameCounter)
					{
						yield return waitForFreezeInterval;
					}
					num = frame;
					frame = num + 1;
				}
				num = frameCounter;
				frameCounter = num + 1;
				num = i;
			}
			if (!this.creatureModule.IsAlive)
			{
				yield return this.RunDying(this.Dying);
			}
			waitForFrameSpeed = null;
			waitForFreezeInterval = null;
		}
		finally
		{
			this.StopAnimating();
		}
		yield break;
		yield break;
	}

	// Token: 0x06000AD0 RID: 2768 RVA: 0x000312B4 File Offset: 0x0002F4B4
	public void AnimateSlash(AnimationConfig animationConfig)
	{
		if (this.spriteRenderer != null)
		{
			if (this.sprites != null && !this.isAnimating)
			{
				Direction direction = this.movementModule.Direction;
				if (animationConfig.ForceDirection)
				{
					direction = animationConfig.Direction;
				}
				switch (direction)
				{
				case Direction.North:
					base.StartCoroutine(this.RunAnimateSlash(this.SlashNorth, animationConfig));
					return;
				case Direction.West:
					base.StartCoroutine(this.RunAnimateSlash(this.SlashWest, animationConfig));
					return;
				case Direction.South:
					base.StartCoroutine(this.RunAnimateSlash(this.SlashSouth, animationConfig));
					return;
				case Direction.East:
					base.StartCoroutine(this.RunAnimateSlash(this.SlashEast, animationConfig));
					return;
				default:
					return;
				}
			}
			else if (this.sprites == null)
			{
				this.spriteRenderer.sprite = null;
			}
		}
	}

	// Token: 0x06000AD1 RID: 2769 RVA: 0x00031382 File Offset: 0x0002F582
	private IEnumerator RunAnimateSlash(FrameConfig frameConfig, AnimationConfig animationConfig)
	{
		try
		{
			float seconds = (animationConfig.FrameSpeed > 0f) ? animationConfig.FrameSpeed : 0.1f;
			WaitForSeconds waitForFrameSpeed = new WaitForSeconds(seconds);
			WaitForSeconds waitForFreezeInterval = new WaitForSeconds(animationConfig.FreezeInterval);
			this.StartAnimating();
			if (frameConfig.Frames < 1)
			{
				this.spriteRenderer.sprite = null;
			}
			else
			{
				int index = frameConfig.Index;
				int lastFrame = index + frameConfig.Frames;
				int frame = index;
				int frameCounter = 0;
				int num;
				for (int i = index; i < lastFrame; i = num + 1)
				{
					if (!this.creatureModule.IsAlive)
					{
						yield break;
					}
					if (frameConfig.Gaps.Contains(frameCounter) | frameConfig.Index < 0)
					{
						this.spriteRenderer.sprite = null;
						yield return waitForFrameSpeed;
						if (animationConfig.FreezeInterval > 0f && animationConfig.FreezeFrame == frameCounter)
						{
							yield return waitForFreezeInterval;
						}
					}
					else
					{
						this.spriteRenderer.sprite = this.sprites[frame];
						yield return waitForFrameSpeed;
						if (animationConfig.FreezeInterval > 0f && animationConfig.FreezeFrame == frameCounter)
						{
							yield return waitForFreezeInterval;
						}
						num = frame;
						frame = num + 1;
					}
					if (frameCounter == 1 && !string.IsNullOrEmpty(this.SlashSoundEffect))
					{
						this.effectModule.PlaySoundEffect(this.SlashSoundEffect, 1f, 0f, Vector3.zero);
					}
					num = frameCounter;
					frameCounter = num + 1;
					num = i;
				}
			}
			if (!this.creatureModule.IsAlive)
			{
				yield return this.RunDying(this.Dying);
			}
			waitForFrameSpeed = null;
			waitForFreezeInterval = null;
		}
		finally
		{
			this.StopAnimating();
		}
		yield break;
		yield break;
	}

	// Token: 0x06000AD2 RID: 2770 RVA: 0x000313A0 File Offset: 0x0002F5A0
	public void AnimateThrust(AnimationConfig animationConfig)
	{
		if (this.spriteRenderer != null)
		{
			if (this.sprites != null)
			{
				if (this.thrustAnimation != null)
				{
					base.StopCoroutine(this.thrustAnimation);
				}
				Direction direction = this.movementModule.Direction;
				if (animationConfig.ForceDirection)
				{
					direction = animationConfig.Direction;
				}
				switch (direction)
				{
				case Direction.North:
					this.thrustAnimation = base.StartCoroutine(this.RunAnimateThrust(this.ThrustNorth, animationConfig));
					return;
				case Direction.West:
					this.thrustAnimation = base.StartCoroutine(this.RunAnimateThrust(this.ThrustWest, animationConfig));
					return;
				case Direction.South:
					this.thrustAnimation = base.StartCoroutine(this.RunAnimateThrust(this.ThrustSouth, animationConfig));
					return;
				case Direction.East:
					this.thrustAnimation = base.StartCoroutine(this.RunAnimateThrust(this.ThrustEast, animationConfig));
					return;
				default:
					return;
				}
			}
			else if (this.sprites == null)
			{
				this.spriteRenderer.sprite = null;
			}
		}
	}

	// Token: 0x06000AD3 RID: 2771 RVA: 0x0003148B File Offset: 0x0002F68B
	private IEnumerator RunAnimateThrust(FrameConfig frameConfig, AnimationConfig animationConfig)
	{
		try
		{
			float seconds = (animationConfig.FrameSpeed > 0f) ? animationConfig.FrameSpeed : 0.1f;
			WaitForSeconds waitForFrameSpeed = new WaitForSeconds(seconds);
			WaitForSeconds waitForFreezeInterval = new WaitForSeconds(animationConfig.FreezeInterval);
			this.StartAnimating();
			if (frameConfig.Frames < 1)
			{
				this.spriteRenderer.sprite = null;
			}
			else
			{
				int index = frameConfig.Index;
				int lastFrame = index + frameConfig.Frames;
				int frame = index;
				int frameCounter = 0;
				int num;
				for (int i = index; i < lastFrame; i = num + 1)
				{
					if (!this.creatureModule.IsAlive)
					{
						yield break;
					}
					if (frameConfig.Gaps.Contains(frameCounter) | frameConfig.Index < 0)
					{
						this.spriteRenderer.sprite = null;
						yield return waitForFrameSpeed;
						if (animationConfig.FreezeInterval > 0f && animationConfig.FreezeFrame == frameCounter)
						{
							yield return waitForFreezeInterval;
						}
					}
					else
					{
						this.spriteRenderer.sprite = this.sprites[frame];
						yield return waitForFrameSpeed;
						if (animationConfig.FreezeInterval > 0f && animationConfig.FreezeFrame == frameCounter)
						{
							yield return waitForFreezeInterval;
						}
						num = frame;
						frame = num + 1;
					}
					if (frameCounter == 2 && !string.IsNullOrEmpty(this.ThrustSoundEffect))
					{
						this.effectModule.PlaySoundEffect(this.ThrustSoundEffect, 1f, 0f, Vector3.zero);
					}
					num = frameCounter;
					frameCounter = num + 1;
					num = i;
				}
			}
			if (!this.creatureModule.IsAlive)
			{
				yield return this.RunDying(this.Dying);
			}
			waitForFrameSpeed = null;
			waitForFreezeInterval = null;
		}
		finally
		{
			this.StopAnimating();
		}
		yield break;
		yield break;
	}

	// Token: 0x06000AD4 RID: 2772 RVA: 0x000314A8 File Offset: 0x0002F6A8
	public void AnimateShootArrow(AnimationConfig animationConfig)
	{
		if (this.spriteRenderer != null)
		{
			if (this.sprites != null)
			{
				Direction direction = this.movementModule.Direction;
				if (animationConfig.ForceDirection)
				{
					direction = animationConfig.Direction;
				}
				switch (direction)
				{
				case Direction.North:
					base.StartCoroutine(this.RunAnimateShootArrow(this.ShootArrowNorth, animationConfig));
					return;
				case Direction.West:
					base.StartCoroutine(this.RunAnimateShootArrow(this.ShootArrowWest, animationConfig));
					return;
				case Direction.South:
					base.StartCoroutine(this.RunAnimateShootArrow(this.ShootArrowSouth, animationConfig));
					return;
				case Direction.East:
					base.StartCoroutine(this.RunAnimateShootArrow(this.ShootArrowEast, animationConfig));
					return;
				default:
					return;
				}
			}
			else if (this.sprites == null)
			{
				this.spriteRenderer.sprite = null;
			}
		}
	}

	// Token: 0x06000AD5 RID: 2773 RVA: 0x0003156B File Offset: 0x0002F76B
	private IEnumerator RunAnimateShootArrow(FrameConfig frameConfig, AnimationConfig animationConfig)
	{
		try
		{
			float seconds = (animationConfig.FrameSpeed > 0f) ? animationConfig.FrameSpeed : 0.1f;
			WaitForSeconds waitForFrameSpeed = new WaitForSeconds(seconds);
			WaitForSeconds waitForFreezeInterval = new WaitForSeconds(animationConfig.FreezeInterval);
			this.StartAnimating();
			if (frameConfig.Frames < 1)
			{
				this.spriteRenderer.sprite = null;
			}
			else
			{
				int index = frameConfig.Index;
				int lastFrame = index + frameConfig.Frames;
				int frame = index;
				int frameCounter = 0;
				int num;
				for (int i = index; i < lastFrame; i = num + 1)
				{
					if (!this.creatureModule.IsAlive)
					{
						yield break;
					}
					if (frameConfig.Gaps.Contains(frameCounter) | frameConfig.Index < 0)
					{
						this.spriteRenderer.sprite = null;
						yield return waitForFrameSpeed;
						if (animationConfig.FreezeInterval > 0f && animationConfig.FreezeFrame == frameCounter)
						{
							yield return waitForFreezeInterval;
						}
					}
					else
					{
						this.spriteRenderer.sprite = this.sprites[frame];
						yield return waitForFrameSpeed;
						if (animationConfig.FreezeInterval > 0f && animationConfig.FreezeFrame == frameCounter)
						{
							yield return waitForFreezeInterval;
						}
						num = frame;
						frame = num + 1;
					}
					if (frameCounter == 6 && !string.IsNullOrEmpty(this.ShootArrowSoundEffect))
					{
						this.effectModule.PlaySoundEffect(this.ShootArrowSoundEffect, 0.75f, 0f, Vector3.zero);
					}
					num = frameCounter;
					frameCounter = num + 1;
					num = i;
				}
			}
			if (!this.creatureModule.IsAlive)
			{
				yield return this.RunDying(this.Dying);
			}
			waitForFrameSpeed = null;
			waitForFreezeInterval = null;
		}
		finally
		{
			this.StopAnimating();
		}
		yield break;
		yield break;
	}

	// Token: 0x06000AD6 RID: 2774 RVA: 0x00031588 File Offset: 0x0002F788
	public void AnimateDeath()
	{
		if (this.spriteRenderer != null)
		{
			if (this.sprites != null)
			{
				base.StartCoroutine(this.RunDying(this.Dying));
				return;
			}
			if (this.sprites == null)
			{
				this.spriteRenderer.sprite = null;
			}
		}
	}

	// Token: 0x06000AD7 RID: 2775 RVA: 0x000315C8 File Offset: 0x0002F7C8
	private IEnumerator RunDying(FrameConfig frameConfig)
	{
		try
		{
			this.StartAnimating();
			Sprite[] array = this.sprites.Skip(frameConfig.Index).Take(frameConfig.Frames).ToArray<Sprite>();
			WaitForSeconds waitForFrameSpeed = new WaitForSeconds(0.1f);
			foreach (Sprite sprite in array)
			{
				if (frameConfig.Index < 0 | frameConfig.Frames < 1)
				{
					this.spriteRenderer.sprite = null;
					yield return waitForFrameSpeed;
				}
				else
				{
					this.spriteRenderer.sprite = sprite;
					yield return waitForFrameSpeed;
				}
			}
			Sprite[] array2 = null;
			waitForFrameSpeed = null;
		}
		finally
		{
			this.StopAnimating();
		}
		yield break;
		yield break;
	}

	// Token: 0x04000C1D RID: 3101
	public SlotType SlotType;

	// Token: 0x04000C1E RID: 3102
	public string WalkSoundEffect;

	// Token: 0x04000C1F RID: 3103
	public string SlashSoundEffect;

	// Token: 0x04000C20 RID: 3104
	public string ShootArrowSoundEffect;

	// Token: 0x04000C21 RID: 3105
	public string ThrustSoundEffect;

	// Token: 0x04000C22 RID: 3106
	public bool IsInvisible;

	// Token: 0x04000C23 RID: 3107
	public Sprite[] MaleSprites;

	// Token: 0x04000C24 RID: 3108
	public Sprite[] FemaleSprites;

	// Token: 0x04000C25 RID: 3109
	private Sprite[] sprites;

	// Token: 0x04000C26 RID: 3110
	private bool isAnimating;

	// Token: 0x04000C27 RID: 3111
	private float lastWalkSoundFrame;

	// Token: 0x04000C28 RID: 3112
	private EffectModule effectModule;

	// Token: 0x04000C29 RID: 3113
	private SpriteRenderer spriteRenderer;

	// Token: 0x04000C2A RID: 3114
	private MovementModule movementModule;

	// Token: 0x04000C2B RID: 3115
	private CreatureModule creatureModule;

	// Token: 0x04000C2C RID: 3116
	private UISystemModule uiSystemModule;

	// Token: 0x04000C2D RID: 3117
	private ConditionModule conditionModule;

	// Token: 0x04000C2E RID: 3118
	private Coroutine thrustAnimation;

	// Token: 0x04000C2F RID: 3119
	private Color originalColor;

	// Token: 0x04000C30 RID: 3120
	public FrameConfig WalkNorth;

	// Token: 0x04000C31 RID: 3121
	public FrameConfig WalkWest;

	// Token: 0x04000C32 RID: 3122
	public FrameConfig WalkSouth;

	// Token: 0x04000C33 RID: 3123
	public FrameConfig WalkEast;

	// Token: 0x04000C34 RID: 3124
	public FrameConfig SlashNorth;

	// Token: 0x04000C35 RID: 3125
	public FrameConfig SlashWest;

	// Token: 0x04000C36 RID: 3126
	public FrameConfig SlashSouth;

	// Token: 0x04000C37 RID: 3127
	public FrameConfig SlashEast;

	// Token: 0x04000C38 RID: 3128
	public FrameConfig CastNorth;

	// Token: 0x04000C39 RID: 3129
	public FrameConfig CastWest;

	// Token: 0x04000C3A RID: 3130
	public FrameConfig CastSouth;

	// Token: 0x04000C3B RID: 3131
	public FrameConfig CastEast;

	// Token: 0x04000C3C RID: 3132
	public FrameConfig StandNorth;

	// Token: 0x04000C3D RID: 3133
	public FrameConfig StandWest;

	// Token: 0x04000C3E RID: 3134
	public FrameConfig StandSouth;

	// Token: 0x04000C3F RID: 3135
	public FrameConfig StandEast;

	// Token: 0x04000C40 RID: 3136
	public FrameConfig ThrustNorth;

	// Token: 0x04000C41 RID: 3137
	public FrameConfig ThrustWest;

	// Token: 0x04000C42 RID: 3138
	public FrameConfig ThrustSouth;

	// Token: 0x04000C43 RID: 3139
	public FrameConfig ThrustEast;

	// Token: 0x04000C44 RID: 3140
	public FrameConfig ShootArrowNorth;

	// Token: 0x04000C45 RID: 3141
	public FrameConfig ShootArrowWest;

	// Token: 0x04000C46 RID: 3142
	public FrameConfig ShootArrowSouth;

	// Token: 0x04000C47 RID: 3143
	public FrameConfig ShootArrowEast;

	// Token: 0x04000C48 RID: 3144
	public FrameConfig Dying;
}
