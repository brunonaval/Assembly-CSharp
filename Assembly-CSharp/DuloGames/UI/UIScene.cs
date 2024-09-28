using System;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DuloGames.UI
{
	// Token: 0x02000635 RID: 1589
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	[RequireComponent(typeof(CanvasGroup))]
	[AddComponentMenu("UI/UI Scene/Scene")]
	public class UIScene : MonoBehaviour
	{
		// Token: 0x17000363 RID: 867
		// (get) Token: 0x060022FA RID: 8954 RVA: 0x000ACFEF File Offset: 0x000AB1EF
		public int id
		{
			get
			{
				return this.m_Id;
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x060022FB RID: 8955 RVA: 0x000ACFF7 File Offset: 0x000AB1F7
		// (set) Token: 0x060022FC RID: 8956 RVA: 0x000ACFFF File Offset: 0x000AB1FF
		public bool isActivated
		{
			get
			{
				return this.m_IsActivated;
			}
			set
			{
				if (value)
				{
					this.Activate();
					return;
				}
				this.Deactivate();
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x060022FD RID: 8957 RVA: 0x000AD011 File Offset: 0x000AB211
		public UIScene.Type type
		{
			get
			{
				return this.m_Type;
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x060022FE RID: 8958 RVA: 0x000AD019 File Offset: 0x000AB219
		// (set) Token: 0x060022FF RID: 8959 RVA: 0x000AD021 File Offset: 0x000AB221
		public Transform content
		{
			get
			{
				return this.m_Content;
			}
			set
			{
				this.m_Content = value;
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06002300 RID: 8960 RVA: 0x000AD02A File Offset: 0x000AB22A
		// (set) Token: 0x06002301 RID: 8961 RVA: 0x000AD032 File Offset: 0x000AB232
		public UIScene.Transition transition
		{
			get
			{
				return this.m_Transition;
			}
			set
			{
				this.m_Transition = value;
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06002302 RID: 8962 RVA: 0x000AD03B File Offset: 0x000AB23B
		// (set) Token: 0x06002303 RID: 8963 RVA: 0x000AD043 File Offset: 0x000AB243
		public float transitionDuration
		{
			get
			{
				return this.m_TransitionDuration;
			}
			set
			{
				this.m_TransitionDuration = value;
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06002304 RID: 8964 RVA: 0x000AD04C File Offset: 0x000AB24C
		// (set) Token: 0x06002305 RID: 8965 RVA: 0x000AD054 File Offset: 0x000AB254
		public TweenEasing transitionEasing
		{
			get
			{
				return this.m_TransitionEasing;
			}
			set
			{
				this.m_TransitionEasing = value;
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06002306 RID: 8966 RVA: 0x000AD05D File Offset: 0x000AB25D
		// (set) Token: 0x06002307 RID: 8967 RVA: 0x000AD065 File Offset: 0x000AB265
		public string animateInTrigger
		{
			get
			{
				return this.m_AnimateInTrigger;
			}
			set
			{
				this.m_AnimateInTrigger = value;
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06002308 RID: 8968 RVA: 0x000AD06E File Offset: 0x000AB26E
		// (set) Token: 0x06002309 RID: 8969 RVA: 0x000AD076 File Offset: 0x000AB276
		public string animateOutTrigger
		{
			get
			{
				return this.m_AnimateOutTrigger;
			}
			set
			{
				this.m_AnimateOutTrigger = value;
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x0600230A RID: 8970 RVA: 0x000A14BE File Offset: 0x0009F6BE
		public RectTransform rectTransform
		{
			get
			{
				return base.transform as RectTransform;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x0600230B RID: 8971 RVA: 0x000AD07F File Offset: 0x000AB27F
		public Animator animator
		{
			get
			{
				return base.gameObject.GetComponent<Animator>();
			}
		}

		// Token: 0x0600230C RID: 8972 RVA: 0x000AD08C File Offset: 0x000AB28C
		protected UIScene()
		{
			if (this.m_FloatTweenRunner == null)
			{
				this.m_FloatTweenRunner = new TweenRunner<FloatTween>();
			}
			this.m_FloatTweenRunner.Init(this);
		}

		// Token: 0x0600230D RID: 8973 RVA: 0x000AD104 File Offset: 0x000AB304
		protected virtual void Awake()
		{
			this.m_SceneManager = UISceneRegistry.instance;
			if (this.m_SceneManager == null)
			{
				Debug.LogWarning("Scene registry does not exist.");
				base.enabled = false;
				return;
			}
			this.m_AnimationState = this.m_IsActivated;
			this.m_CanvasGroup = base.gameObject.GetComponent<CanvasGroup>();
			if (Application.isPlaying && this.isActivated && base.isActiveAndEnabled && this.m_FirstSelected != null)
			{
				EventSystem.current.SetSelectedGameObject(this.m_FirstSelected);
			}
		}

		// Token: 0x0600230E RID: 8974 RVA: 0x0000219A File Offset: 0x0000039A
		protected virtual void Start()
		{
		}

		// Token: 0x0600230F RID: 8975 RVA: 0x000AD188 File Offset: 0x000AB388
		protected virtual void OnEnable()
		{
			if (this.m_SceneManager != null && base.gameObject.activeInHierarchy)
			{
				this.m_SceneManager.RegisterScene(this);
			}
			if (this.isActivated && this.onActivate != null)
			{
				this.onActivate.Invoke(this);
			}
		}

		// Token: 0x06002310 RID: 8976 RVA: 0x000AD1C7 File Offset: 0x000AB3C7
		protected virtual void OnDisable()
		{
			if (this.m_SceneManager != null)
			{
				this.m_SceneManager.UnregisterScene(this);
			}
		}

		// Token: 0x06002311 RID: 8977 RVA: 0x000AD1E0 File Offset: 0x000AB3E0
		protected void Update()
		{
			if (this.animator != null && !string.IsNullOrEmpty(this.m_AnimateInTrigger) && !string.IsNullOrEmpty(this.m_AnimateOutTrigger))
			{
				AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
				if (currentAnimatorStateInfo.IsName(this.m_AnimateInTrigger) && !this.m_AnimationState)
				{
					if (currentAnimatorStateInfo.normalizedTime >= currentAnimatorStateInfo.length)
					{
						this.m_AnimationState = true;
						this.OnTransitionIn();
						return;
					}
				}
				else if (currentAnimatorStateInfo.IsName(this.m_AnimateOutTrigger) && this.m_AnimationState && currentAnimatorStateInfo.normalizedTime >= currentAnimatorStateInfo.length)
				{
					this.m_AnimationState = false;
					this.OnTransitionOut();
				}
			}
		}

		// Token: 0x06002312 RID: 8978 RVA: 0x000AD294 File Offset: 0x000AB494
		public void Activate()
		{
			if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this.m_Type == UIScene.Type.Prefab || this.m_Type == UIScene.Type.Resource)
			{
				GameObject gameObject = null;
				if (this.m_Type == UIScene.Type.Prefab)
				{
					if (this.m_Prefab == null)
					{
						Debug.LogWarning("You are activating a prefab scene and no prefab is specified.");
						return;
					}
					gameObject = this.m_Prefab;
				}
				if (this.m_Type == UIScene.Type.Resource)
				{
					if (string.IsNullOrEmpty(this.m_Resource))
					{
						Debug.LogWarning("You are activating a resource scene and no resource path is specified.");
						return;
					}
					gameObject = Resources.Load<GameObject>(this.m_Resource);
				}
				if (gameObject != null)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
					this.m_Content = gameObject2.transform;
					this.m_Content.SetParent(base.transform);
					if (this.m_Content is RectTransform)
					{
						RectTransform rectTransform = this.m_Content as RectTransform;
						rectTransform.localScale = Vector3.one;
						rectTransform.localPosition = Vector3.zero;
						rectTransform.anchorMin = new Vector2(0f, 0f);
						rectTransform.anchorMax = new Vector2(1f, 1f);
						rectTransform.pivot = new Vector2(0.5f, 0.5f);
						Canvas canvas = UIUtility.FindInParents<Canvas>(base.gameObject);
						if (canvas == null)
						{
							canvas = base.gameObject.GetComponentInChildren<Canvas>();
						}
						if (canvas != null)
						{
							RectTransform rectTransform2 = canvas.transform as RectTransform;
							rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform2.sizeDelta.x);
							rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransform2.sizeDelta.y);
						}
						rectTransform.anchoredPosition3D = new Vector3(0f, 0f, 0f);
					}
				}
			}
			if (this.m_Content != null)
			{
				this.m_Content.gameObject.SetActive(true);
			}
			if (base.isActiveAndEnabled && this.m_FirstSelected != null)
			{
				EventSystem.current.SetSelectedGameObject(this.m_FirstSelected);
			}
			this.m_IsActivated = true;
			if (this.onActivate != null)
			{
				this.onActivate.Invoke(this);
			}
		}

		// Token: 0x06002313 RID: 8979 RVA: 0x000AD49C File Offset: 0x000AB69C
		public void Deactivate()
		{
			if (this.m_Content != null)
			{
				this.m_Content.gameObject.SetActive(false);
			}
			if (this.m_Type == UIScene.Type.Prefab || this.m_Type == UIScene.Type.Resource)
			{
				UnityEngine.Object.Destroy(this.m_Content.gameObject);
			}
			Resources.UnloadUnusedAssets();
			this.m_IsActivated = false;
			if (this.onDeactivate != null)
			{
				this.onDeactivate.Invoke(this);
			}
		}

		// Token: 0x06002314 RID: 8980 RVA: 0x000AD50B File Offset: 0x000AB70B
		public void TransitionTo()
		{
			if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this.m_SceneManager != null)
			{
				this.m_SceneManager.TransitionToScene(this);
			}
		}

		// Token: 0x06002315 RID: 8981 RVA: 0x000AD537 File Offset: 0x000AB737
		public void TransitionIn()
		{
			this.TransitionIn(this.m_Transition, this.m_TransitionDuration, this.m_TransitionEasing);
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x000AD554 File Offset: 0x000AB754
		public void TransitionIn(UIScene.Transition transition, float duration, TweenEasing easing)
		{
			if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this.m_CanvasGroup == null)
			{
				return;
			}
			if (transition == UIScene.Transition.None)
			{
				this.Activate();
				return;
			}
			if (transition == UIScene.Transition.Animation)
			{
				this.Activate();
				this.TriggerAnimation(this.m_AnimateInTrigger);
				return;
			}
			Vector2 size = this.rectTransform.rect.size;
			if (transition == UIScene.Transition.SlideFromLeft || transition == UIScene.Transition.SlideFromRight || transition == UIScene.Transition.SlideFromTop || transition == UIScene.Transition.SlideFromBottom)
			{
				this.rectTransform.pivot = new Vector2(0f, 1f);
				this.rectTransform.anchorMin = new Vector2(0f, 1f);
				this.rectTransform.anchorMax = new Vector2(0f, 1f);
				this.rectTransform.sizeDelta = size;
			}
			FloatTween info = default(FloatTween);
			info.duration = duration;
			switch (transition)
			{
			case UIScene.Transition.CrossFade:
				this.m_CanvasGroup.alpha = 0f;
				info.startFloat = 0f;
				info.targetFloat = 1f;
				info.AddOnChangedCallback(new UnityAction<float>(this.SetCanvasAlpha));
				break;
			case UIScene.Transition.SlideFromRight:
				this.rectTransform.anchoredPosition = new Vector2(size.x, 0f);
				info.startFloat = size.x;
				info.targetFloat = 0f;
				info.AddOnChangedCallback(new UnityAction<float>(this.SetPositionX));
				break;
			case UIScene.Transition.SlideFromLeft:
				this.rectTransform.anchoredPosition = new Vector2(size.x * -1f, 0f);
				info.startFloat = size.x * -1f;
				info.targetFloat = 0f;
				info.AddOnChangedCallback(new UnityAction<float>(this.SetPositionX));
				break;
			case UIScene.Transition.SlideFromTop:
				this.rectTransform.anchoredPosition = new Vector2(0f, size.y);
				info.startFloat = size.y;
				info.targetFloat = 0f;
				info.AddOnChangedCallback(new UnityAction<float>(this.SetPositionY));
				break;
			case UIScene.Transition.SlideFromBottom:
				this.rectTransform.anchoredPosition = new Vector2(0f, size.y * -1f);
				info.startFloat = size.y * -1f;
				info.targetFloat = 0f;
				info.AddOnChangedCallback(new UnityAction<float>(this.SetPositionY));
				break;
			}
			this.Activate();
			info.AddOnFinishCallback(new UnityAction(this.OnTransitionIn));
			info.ignoreTimeScale = true;
			info.easing = easing;
			this.m_FloatTweenRunner.StartTween(info);
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x000AD80C File Offset: 0x000ABA0C
		public void TransitionOut()
		{
			this.TransitionOut(this.m_Transition, this.m_TransitionDuration, this.m_TransitionEasing);
		}

		// Token: 0x06002318 RID: 8984 RVA: 0x000AD828 File Offset: 0x000ABA28
		public void TransitionOut(UIScene.Transition transition, float duration, TweenEasing easing)
		{
			if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this.m_CanvasGroup == null)
			{
				return;
			}
			if (transition == UIScene.Transition.None)
			{
				this.Deactivate();
				return;
			}
			if (transition == UIScene.Transition.Animation)
			{
				this.TriggerAnimation(this.m_AnimateOutTrigger);
				return;
			}
			Vector2 size = this.rectTransform.rect.size;
			if (transition == UIScene.Transition.SlideFromLeft || transition == UIScene.Transition.SlideFromRight || transition == UIScene.Transition.SlideFromTop || transition == UIScene.Transition.SlideFromBottom)
			{
				this.rectTransform.pivot = new Vector2(0f, 1f);
				this.rectTransform.anchorMin = new Vector2(0f, 1f);
				this.rectTransform.anchorMax = new Vector2(0f, 1f);
				this.rectTransform.sizeDelta = size;
				this.rectTransform.anchoredPosition = new Vector2(0f, 0f);
			}
			FloatTween info = default(FloatTween);
			info.duration = duration;
			switch (transition)
			{
			case UIScene.Transition.CrossFade:
				this.m_CanvasGroup.alpha = 1f;
				info.startFloat = this.m_CanvasGroup.alpha;
				info.targetFloat = 0f;
				info.AddOnChangedCallback(new UnityAction<float>(this.SetCanvasAlpha));
				break;
			case UIScene.Transition.SlideFromRight:
				info.startFloat = 0f;
				info.targetFloat = size.x * -1f;
				info.AddOnChangedCallback(new UnityAction<float>(this.SetPositionX));
				break;
			case UIScene.Transition.SlideFromLeft:
				info.startFloat = 0f;
				info.targetFloat = size.x;
				info.AddOnChangedCallback(new UnityAction<float>(this.SetPositionX));
				break;
			case UIScene.Transition.SlideFromTop:
				info.startFloat = 0f;
				info.targetFloat = size.y * -1f;
				info.AddOnChangedCallback(new UnityAction<float>(this.SetPositionY));
				break;
			case UIScene.Transition.SlideFromBottom:
				info.startFloat = 0f;
				info.targetFloat = size.y;
				info.AddOnChangedCallback(new UnityAction<float>(this.SetPositionY));
				break;
			}
			info.AddOnFinishCallback(new UnityAction(this.OnTransitionOut));
			info.ignoreTimeScale = true;
			info.easing = easing;
			this.m_FloatTweenRunner.StartTween(info);
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x000ADA7C File Offset: 0x000ABC7C
		public void StartAlphaTween(float targetAlpha, float duration, TweenEasing easing, bool ignoreTimeScale, UnityAction callback)
		{
			if (this.m_CanvasGroup == null)
			{
				return;
			}
			FloatTween info = new FloatTween
			{
				duration = duration,
				startFloat = this.m_CanvasGroup.alpha,
				targetFloat = targetAlpha
			};
			info.AddOnChangedCallback(new UnityAction<float>(this.SetCanvasAlpha));
			info.AddOnFinishCallback(callback);
			info.ignoreTimeScale = ignoreTimeScale;
			info.easing = easing;
			this.m_FloatTweenRunner.StartTween(info);
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x000ADAFD File Offset: 0x000ABCFD
		public void SetCanvasAlpha(float alpha)
		{
			if (this.m_CanvasGroup == null)
			{
				return;
			}
			this.m_CanvasGroup.alpha = alpha;
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x000ADB1A File Offset: 0x000ABD1A
		public void SetPositionX(float x)
		{
			this.rectTransform.anchoredPosition = new Vector2(x, this.rectTransform.anchoredPosition.y);
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x000ADB3D File Offset: 0x000ABD3D
		public void SetPositionY(float y)
		{
			this.rectTransform.anchoredPosition = new Vector2(this.rectTransform.anchoredPosition.x, y);
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x000ADB60 File Offset: 0x000ABD60
		private void TriggerAnimation(string triggername)
		{
			Animator component = base.gameObject.GetComponent<Animator>();
			if (component == null || !component.enabled || !component.isActiveAndEnabled || component.runtimeAnimatorController == null || !component.hasBoundPlayables || string.IsNullOrEmpty(triggername))
			{
				return;
			}
			component.ResetTrigger(this.m_AnimateInTrigger);
			component.ResetTrigger(this.m_AnimateOutTrigger);
			component.SetTrigger(triggername);
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x000ADBD0 File Offset: 0x000ABDD0
		protected virtual void OnTransitionIn()
		{
			this.m_CanvasGroup != null;
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x000ADBDF File Offset: 0x000ABDDF
		protected virtual void OnTransitionOut()
		{
			this.Deactivate();
			this.m_CanvasGroup != null;
			this.SetCanvasAlpha(1f);
			this.SetPositionX(0f);
		}

		// Token: 0x04001C60 RID: 7264
		private UISceneRegistry m_SceneManager;

		// Token: 0x04001C61 RID: 7265
		private bool m_AnimationState;

		// Token: 0x04001C62 RID: 7266
		[SerializeField]
		private int m_Id;

		// Token: 0x04001C63 RID: 7267
		[SerializeField]
		private bool m_IsActivated = true;

		// Token: 0x04001C64 RID: 7268
		[SerializeField]
		private UIScene.Type m_Type;

		// Token: 0x04001C65 RID: 7269
		[SerializeField]
		private Transform m_Content;

		// Token: 0x04001C66 RID: 7270
		[SerializeField]
		private GameObject m_Prefab;

		// Token: 0x04001C67 RID: 7271
		[SerializeField]
		[ResourcePath]
		private string m_Resource;

		// Token: 0x04001C68 RID: 7272
		[SerializeField]
		private UIScene.Transition m_Transition;

		// Token: 0x04001C69 RID: 7273
		[SerializeField]
		private float m_TransitionDuration = 0.2f;

		// Token: 0x04001C6A RID: 7274
		[SerializeField]
		private TweenEasing m_TransitionEasing = TweenEasing.InOutQuint;

		// Token: 0x04001C6B RID: 7275
		[SerializeField]
		private string m_AnimateInTrigger = "AnimateIn";

		// Token: 0x04001C6C RID: 7276
		[SerializeField]
		private string m_AnimateOutTrigger = "AnimateOut";

		// Token: 0x04001C6D RID: 7277
		[SerializeField]
		private GameObject m_FirstSelected;

		// Token: 0x04001C6E RID: 7278
		public UIScene.OnActivateEvent onActivate = new UIScene.OnActivateEvent();

		// Token: 0x04001C6F RID: 7279
		public UIScene.OnDeactivateEvent onDeactivate = new UIScene.OnDeactivateEvent();

		// Token: 0x04001C70 RID: 7280
		private CanvasGroup m_CanvasGroup;

		// Token: 0x04001C71 RID: 7281
		[NonSerialized]
		private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

		// Token: 0x02000636 RID: 1590
		public enum Type
		{
			// Token: 0x04001C73 RID: 7283
			Preloaded,
			// Token: 0x04001C74 RID: 7284
			Prefab,
			// Token: 0x04001C75 RID: 7285
			Resource
		}

		// Token: 0x02000637 RID: 1591
		public enum Transition
		{
			// Token: 0x04001C77 RID: 7287
			None,
			// Token: 0x04001C78 RID: 7288
			Animation,
			// Token: 0x04001C79 RID: 7289
			CrossFade,
			// Token: 0x04001C7A RID: 7290
			SlideFromRight,
			// Token: 0x04001C7B RID: 7291
			SlideFromLeft,
			// Token: 0x04001C7C RID: 7292
			SlideFromTop,
			// Token: 0x04001C7D RID: 7293
			SlideFromBottom
		}

		// Token: 0x02000638 RID: 1592
		[Serializable]
		public class OnActivateEvent : UnityEvent<UIScene>
		{
		}

		// Token: 0x02000639 RID: 1593
		[Serializable]
		public class OnDeactivateEvent : UnityEvent<UIScene>
		{
		}
	}
}
