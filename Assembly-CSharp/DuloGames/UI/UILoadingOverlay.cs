using System;
using System.Collections;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x02000618 RID: 1560
	[RequireComponent(typeof(Canvas))]
	[RequireComponent(typeof(GraphicRaycaster))]
	[AddComponentMenu("UI/Loading Overlay", 58)]
	public class UILoadingOverlay : MonoBehaviour
	{
		// Token: 0x0600226F RID: 8815 RVA: 0x000AAE3B File Offset: 0x000A903B
		protected UILoadingOverlay()
		{
			if (this.m_FloatTweenRunner == null)
			{
				this.m_FloatTweenRunner = new TweenRunner<FloatTween>();
			}
			this.m_FloatTweenRunner.Init(this);
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x000AAE78 File Offset: 0x000A9078
		protected void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			Canvas[] array = UnityEngine.Object.FindObjectsOfType<Canvas>();
			Canvas component = base.gameObject.GetComponent<Canvas>();
			foreach (Canvas canvas in array)
			{
				if (!canvas.Equals(component) && canvas.sortingOrder > component.sortingOrder)
				{
					component.sortingOrder = canvas.sortingOrder + 1;
				}
			}
			if (this.m_ProgressBar != null)
			{
				this.m_ProgressBar.fillAmount = 0f;
			}
			if (this.m_CanvasGroup != null)
			{
				this.m_CanvasGroup.alpha = 0f;
			}
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x000AAF15 File Offset: 0x000A9115
		protected void OnEnable()
		{
			SceneManager.sceneLoaded += this.OnSceneFinishedLoading;
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x000AAF28 File Offset: 0x000A9128
		protected void OnDisable()
		{
			SceneManager.sceneLoaded -= this.OnSceneFinishedLoading;
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x000AAF3C File Offset: 0x000A913C
		public void LoadScene(string sceneName)
		{
			Scene sceneByName = SceneManager.GetSceneByName(sceneName);
			if (sceneByName.IsValid())
			{
				this.LoadScene(sceneByName.buildIndex);
			}
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x000AAF68 File Offset: 0x000A9168
		public void LoadScene(int sceneIndex)
		{
			this.m_Showing = true;
			this.m_LoadSceneId = sceneIndex;
			if (this.m_ProgressBar != null)
			{
				this.m_ProgressBar.fillAmount = 0f;
			}
			if (this.m_CanvasGroup != null)
			{
				this.m_CanvasGroup.alpha = 0f;
			}
			this.StartAlphaTween(1f, this.m_TransitionDuration, true);
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x000AAFD4 File Offset: 0x000A91D4
		public void StartAlphaTween(float targetAlpha, float duration, bool ignoreTimeScale)
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
			info.AddOnFinishCallback(new UnityAction(this.OnTweenFinished));
			info.ignoreTimeScale = ignoreTimeScale;
			info.easing = this.m_TransitionEasing;
			this.m_FloatTweenRunner.StartTween(info);
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x000AB063 File Offset: 0x000A9263
		protected void SetCanvasAlpha(float alpha)
		{
			if (this.m_CanvasGroup == null)
			{
				return;
			}
			this.m_CanvasGroup.alpha = alpha;
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x000AB080 File Offset: 0x000A9280
		protected void OnTweenFinished()
		{
			if (this.m_Showing)
			{
				this.m_Showing = false;
				base.StartCoroutine(this.AsynchronousLoad());
				return;
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x000AB0AA File Offset: 0x000A92AA
		private IEnumerator AsynchronousLoad()
		{
			yield return null;
			AsyncOperation ao = SceneManager.LoadSceneAsync(this.m_LoadSceneId);
			ao.allowSceneActivation = false;
			while (!ao.isDone)
			{
				float fillAmount = Mathf.Clamp01(ao.progress / 0.9f);
				if (this.m_ProgressBar != null)
				{
					this.m_ProgressBar.fillAmount = fillAmount;
				}
				if (ao.progress == 0.9f)
				{
					ao.allowSceneActivation = true;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x000AB0B9 File Offset: 0x000A92B9
		private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
		{
			if (scene.buildIndex != this.m_LoadSceneId)
			{
				return;
			}
			this.StartAlphaTween(0f, this.m_TransitionDuration, true);
		}

		// Token: 0x04001BF9 RID: 7161
		[SerializeField]
		private UIProgressBar m_ProgressBar;

		// Token: 0x04001BFA RID: 7162
		[SerializeField]
		private CanvasGroup m_CanvasGroup;

		// Token: 0x04001BFB RID: 7163
		[Header("Transition")]
		[SerializeField]
		private TweenEasing m_TransitionEasing = TweenEasing.InOutQuint;

		// Token: 0x04001BFC RID: 7164
		[SerializeField]
		private float m_TransitionDuration = 0.4f;

		// Token: 0x04001BFD RID: 7165
		private bool m_Showing;

		// Token: 0x04001BFE RID: 7166
		private int m_LoadSceneId;

		// Token: 0x04001BFF RID: 7167
		[NonSerialized]
		private readonly TweenRunner<FloatTween> m_FloatTweenRunner;
	}
}
