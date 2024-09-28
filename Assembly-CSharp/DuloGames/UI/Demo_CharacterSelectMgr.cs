using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x02000591 RID: 1425
	public class Demo_CharacterSelectMgr : MonoBehaviour
	{
		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06001F87 RID: 8071 RVA: 0x0009E404 File Offset: 0x0009C604
		public static Demo_CharacterSelectMgr instance
		{
			get
			{
				return Demo_CharacterSelectMgr.m_Mgr;
			}
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x0009E40B File Offset: 0x0009C60B
		protected void Awake()
		{
			Demo_CharacterSelectMgr.m_Mgr = this;
			if (this.m_Camera == null)
			{
				this.m_Camera = Camera.main;
			}
			if (this.m_InfoContainer != null)
			{
				this.m_InfoContainer.SetActive(false);
			}
		}

		// Token: 0x06001F89 RID: 8073 RVA: 0x0009E446 File Offset: 0x0009C646
		protected void OnDestroy()
		{
			Demo_CharacterSelectMgr.m_Mgr = null;
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x0009E450 File Offset: 0x0009C650
		protected void OnEnable()
		{
			if (this.m_CharacterPrefab)
			{
				for (int i = 0; i < this.m_AddCharacters; i++)
				{
					string[] array = new string[]
					{
						"Annika",
						"Evita",
						"Herb",
						"Thad",
						"Myesha",
						"Lucile",
						"Sharice",
						"Tatiana",
						"Isis",
						"Allen"
					};
					string[] array2 = new string[]
					{
						"Human",
						"Elf",
						"Orc",
						"Undead",
						"Programmer"
					};
					string[] array3 = new string[]
					{
						"Warrior",
						"Mage",
						"Hunter",
						"Priest",
						"Designer"
					};
					this.AddCharacter(new Demo_CharacterInfo
					{
						name = array[UnityEngine.Random.Range(0, 10)],
						raceString = array2[UnityEngine.Random.Range(0, 5)],
						classString = array3[UnityEngine.Random.Range(0, 5)],
						level = UnityEngine.Random.Range(1, 61)
					}, this.m_CharacterPrefab, i);
				}
			}
			this.SelectFirstAvailable();
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x0009E598 File Offset: 0x0009C798
		protected void Update()
		{
			if (base.isActiveAndEnabled && this.m_Slots.Count == 0)
			{
				return;
			}
			if (this.m_SelectedTransform != null)
			{
				Vector3 b = this.m_SelectedTransform.position + -this.m_SelectedTransform.forward * this.m_CameraDistance;
				b.y = this.m_Camera.transform.position.y;
				this.m_Camera.transform.position = Vector3.Lerp(this.m_Camera.transform.position, b, Time.deltaTime * this.m_CameraSpeed);
			}
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x0009E644 File Offset: 0x0009C844
		public void AddCharacter(Demo_CharacterInfo info, GameObject modelPrefab, int index)
		{
			if (this.m_Slots.Count == 0)
			{
				return;
			}
			if (modelPrefab == null)
			{
				return;
			}
			Transform transform = this.m_Slots[index];
			if (transform == null)
			{
				return;
			}
			Demo_CharacterSelectChar component = transform.gameObject.GetComponent<Demo_CharacterSelectChar>();
			if (component != null)
			{
				component.info = info;
				component.index = index;
			}
			foreach (object obj in transform)
			{
				UnityEngine.Object.Destroy(((Transform)obj).gameObject);
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(modelPrefab);
			gameObject.layer = transform.gameObject.layer;
			gameObject.transform.SetParent(transform, false);
			gameObject.transform.localScale = modelPrefab.transform.localScale;
			gameObject.transform.localPosition = modelPrefab.transform.localPosition;
			gameObject.transform.localRotation = modelPrefab.transform.localRotation;
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x0009E754 File Offset: 0x0009C954
		public void SelectFirstAvailable()
		{
			if (this.m_Slots.Count == 0)
			{
				return;
			}
			foreach (Transform transform in this.m_Slots)
			{
				if (!(transform == null))
				{
					Demo_CharacterSelectChar component = transform.gameObject.GetComponent<Demo_CharacterSelectChar>();
					if (component != null && component.info != null)
					{
						this.SelectCharacter(component);
						break;
					}
				}
			}
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x0009E7E0 File Offset: 0x0009C9E0
		public void SelectCharacter(int index)
		{
			if (this.m_Slots.Count == 0)
			{
				return;
			}
			Transform transform = this.m_Slots[index];
			if (transform == null)
			{
				return;
			}
			Demo_CharacterSelectChar component = transform.gameObject.GetComponent<Demo_CharacterSelectChar>();
			if (component != null)
			{
				this.SelectCharacter(component);
			}
		}

		// Token: 0x06001F8F RID: 8079 RVA: 0x0009E830 File Offset: 0x0009CA30
		public void SelectCharacter(Demo_CharacterSelectChar character)
		{
			if (this.m_SelectedIndex == character.index)
			{
				return;
			}
			this.m_SelectedIndex = character.index;
			this.m_SelectedTransform = character.transform;
			if (character.info != null)
			{
				if (this.m_InfoContainer != null)
				{
					this.m_InfoContainer.SetActive(true);
				}
				if (this.m_NameText != null)
				{
					this.m_NameText.text = character.info.name.ToLower();
				}
				if (this.m_LevelText != null)
				{
					this.m_LevelText.text = character.info.level.ToString();
				}
				if (this.m_RaceClassText != null)
				{
					this.m_RaceClassText.text = character.info.raceString.ToLower() + " " + character.info.classString.ToLower();
					return;
				}
			}
			else
			{
				if (this.m_InfoContainer != null)
				{
					this.m_InfoContainer.SetActive(false);
				}
				if (this.m_NameText != null)
				{
					this.m_NameText.text = "";
				}
				if (this.m_LevelText != null)
				{
					this.m_LevelText.text = "";
				}
				if (this.m_RaceClassText != null)
				{
					this.m_RaceClassText.text = "";
				}
			}
		}

		// Token: 0x06001F90 RID: 8080 RVA: 0x0009E998 File Offset: 0x0009CB98
		public Demo_CharacterSelectChar GetCharacterInDirection(float direction)
		{
			if (this.m_Slots.Count == 0)
			{
				return null;
			}
			if (this.m_SelectedTransform == null && this.m_Slots[0] != null)
			{
				return this.m_Slots[0].gameObject.GetComponent<Demo_CharacterSelectChar>();
			}
			Demo_CharacterSelectChar demo_CharacterSelectChar = null;
			float num = 0f;
			foreach (Transform transform in this.m_Slots)
			{
				if (!transform.Equals(this.m_SelectedTransform))
				{
					float num2 = transform.position.x - this.m_SelectedTransform.position.x;
					if ((direction > 0f && num2 > 0f) || (direction < 0f && num2 < 0f))
					{
						Demo_CharacterSelectChar component = transform.GetComponent<Demo_CharacterSelectChar>();
						if (!(component == null) && component.info != null)
						{
							if (demo_CharacterSelectChar == null)
							{
								demo_CharacterSelectChar = component;
								num = Vector3.Distance(this.m_SelectedTransform.position, transform.position);
							}
							else if (Vector3.Distance(this.m_SelectedTransform.position, transform.position) <= num)
							{
								demo_CharacterSelectChar = component;
								num = Vector3.Distance(this.m_SelectedTransform.position, transform.position);
							}
						}
					}
				}
			}
			return demo_CharacterSelectChar;
		}

		// Token: 0x06001F91 RID: 8081 RVA: 0x0009EB00 File Offset: 0x0009CD00
		public void SelectNext()
		{
			Demo_CharacterSelectChar characterInDirection = this.GetCharacterInDirection(1f);
			if (characterInDirection != null)
			{
				this.SelectCharacter(characterInDirection);
			}
		}

		// Token: 0x06001F92 RID: 8082 RVA: 0x0009EB2C File Offset: 0x0009CD2C
		public void SelectPrevious()
		{
			Demo_CharacterSelectChar characterInDirection = this.GetCharacterInDirection(-1f);
			if (characterInDirection != null)
			{
				this.SelectCharacter(characterInDirection);
			}
		}

		// Token: 0x06001F93 RID: 8083 RVA: 0x0009EB58 File Offset: 0x0009CD58
		public void RemoveCharacter(int index)
		{
			if (this.m_Slots.Count == 0)
			{
				return;
			}
			Transform transform = this.m_Slots[index];
			if (transform == null)
			{
				return;
			}
			Demo_CharacterSelectChar component = transform.gameObject.GetComponent<Demo_CharacterSelectChar>();
			if (component != null)
			{
				component.info = null;
			}
			foreach (object obj in transform)
			{
				UnityEngine.Object.Destroy(((Transform)obj).gameObject);
			}
			if (index == this.m_SelectedIndex)
			{
				if (this.m_InfoContainer != null)
				{
					this.m_InfoContainer.SetActive(false);
				}
				if (this.m_NameText != null)
				{
					this.m_NameText.text = "";
				}
				if (this.m_LevelText != null)
				{
					this.m_LevelText.text = "";
				}
				if (this.m_RaceClassText != null)
				{
					this.m_RaceClassText.text = "";
				}
				this.SelectFirstAvailable();
			}
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x0009EC74 File Offset: 0x0009CE74
		public void DeleteSelected()
		{
			if (this.m_SelectedIndex > -1)
			{
				this.RemoveCharacter(this.m_SelectedIndex);
			}
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x0009EC8C File Offset: 0x0009CE8C
		public void OnPlayClick()
		{
			UILoadingOverlay uiloadingOverlay = UILoadingOverlayManager.Instance.Create();
			if (uiloadingOverlay != null)
			{
				uiloadingOverlay.LoadScene(this.m_IngameSceneId);
			}
		}

		// Token: 0x0400196E RID: 6510
		private static Demo_CharacterSelectMgr m_Mgr;

		// Token: 0x0400196F RID: 6511
		[SerializeField]
		private int m_IngameSceneId;

		// Token: 0x04001970 RID: 6512
		[Header("Camera Properties")]
		[SerializeField]
		private Camera m_Camera;

		// Token: 0x04001971 RID: 6513
		[SerializeField]
		private float m_CameraSpeed = 10f;

		// Token: 0x04001972 RID: 6514
		[SerializeField]
		private float m_CameraDistance = 10f;

		// Token: 0x04001973 RID: 6515
		[Header("Character Slots")]
		[SerializeField]
		private List<Transform> m_Slots;

		// Token: 0x04001974 RID: 6516
		[Header("Selected Character Info")]
		[SerializeField]
		private GameObject m_InfoContainer;

		// Token: 0x04001975 RID: 6517
		[SerializeField]
		private Text m_NameText;

		// Token: 0x04001976 RID: 6518
		[SerializeField]
		private Text m_LevelText;

		// Token: 0x04001977 RID: 6519
		[SerializeField]
		private Text m_RaceClassText;

		// Token: 0x04001978 RID: 6520
		[Header("Demo Properties")]
		[SerializeField]
		private GameObject m_CharacterPrefab;

		// Token: 0x04001979 RID: 6521
		[SerializeField]
		private int m_AddCharacters = 5;

		// Token: 0x0400197A RID: 6522
		private int m_SelectedIndex = -1;

		// Token: 0x0400197B RID: 6523
		private Transform m_SelectedTransform;
	}
}
