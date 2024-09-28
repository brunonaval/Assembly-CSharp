using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x02000593 RID: 1427
	[RequireComponent(typeof(ToggleGroup))]
	public class Demo_CharacterSelectList : MonoBehaviour
	{
		// Token: 0x06001F98 RID: 8088 RVA: 0x0009ECE5 File Offset: 0x0009CEE5
		protected void Awake()
		{
			this.m_ToggleGroup = base.gameObject.GetComponent<ToggleGroup>();
		}

		// Token: 0x06001F99 RID: 8089 RVA: 0x0009ECF8 File Offset: 0x0009CEF8
		protected void Start()
		{
			if (this.m_CharactersContainer != null)
			{
				foreach (object obj in this.m_CharactersContainer)
				{
					UnityEngine.Object.Destroy(((Transform)obj).gameObject);
				}
			}
			if (this.m_IsDemo && this.m_CharacterPrefab)
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
					}, i == 0);
				}
			}
		}

		// Token: 0x06001F9A RID: 8090 RVA: 0x0009EEA4 File Offset: 0x0009D0A4
		public void AddCharacter(Demo_CharacterInfo info, bool selected)
		{
			if (this.m_CharacterPrefab == null || this.m_CharactersContainer == null)
			{
				return;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_CharacterPrefab);
			gameObject.layer = this.m_CharactersContainer.gameObject.layer;
			gameObject.transform.SetParent(this.m_CharactersContainer, false);
			gameObject.transform.localScale = this.m_CharacterPrefab.transform.localScale;
			gameObject.transform.localPosition = this.m_CharacterPrefab.transform.localPosition;
			gameObject.transform.localRotation = this.m_CharacterPrefab.transform.localRotation;
			Demo_CharacterSelectList_Character component = gameObject.GetComponent<Demo_CharacterSelectList_Character>();
			if (component != null)
			{
				component.SetCharacterInfo(info);
				component.SetToggleGroup(this.m_ToggleGroup);
				component.SetSelected(selected);
				component.AddOnSelectListener(new UnityAction<Demo_CharacterSelectList_Character>(this.OnCharacterSelected));
				component.AddOnDeleteListener(new UnityAction<Demo_CharacterSelectList_Character>(this.OnCharacterDeleteRequested));
			}
		}

		// Token: 0x06001F9B RID: 8091 RVA: 0x0009EF9F File Offset: 0x0009D19F
		private void OnCharacterSelected(Demo_CharacterSelectList_Character character)
		{
			if (this.m_OnCharacterSelected != null)
			{
				this.m_OnCharacterSelected.Invoke(character.characterInfo);
			}
		}

		// Token: 0x06001F9C RID: 8092 RVA: 0x0009EFBC File Offset: 0x0009D1BC
		private void OnCharacterDeleteRequested(Demo_CharacterSelectList_Character character)
		{
			this.m_DeletingCharacter = character;
			UIModalBox uimodalBox = UIModalBoxManager.Instance.Create(base.gameObject);
			if (uimodalBox != null)
			{
				uimodalBox.SetText1("Do you really want to delete this character?");
				uimodalBox.SetText2("You wont be able to reverse this operation and your\u0003charcater will be permamently removed.");
				uimodalBox.SetConfirmButtonText("delete");
				uimodalBox.onConfirm.AddListener(new UnityAction(this.OnCharacterDeleteConfirm));
				uimodalBox.onCancel.AddListener(new UnityAction(this.OnCharacterDeleteCancel));
				uimodalBox.Show();
			}
		}

		// Token: 0x06001F9D RID: 8093 RVA: 0x0009F040 File Offset: 0x0009D240
		private void OnCharacterDeleteConfirm()
		{
			if (this.m_DeletingCharacter == null)
			{
				return;
			}
			if (this.m_DeletingCharacter.isSelected && this.m_CharactersContainer != null)
			{
				foreach (object obj in this.m_CharactersContainer)
				{
					Demo_CharacterSelectList_Character component = ((Transform)obj).gameObject.GetComponent<Demo_CharacterSelectList_Character>();
					if (!component.Equals(this.m_DeletingCharacter))
					{
						component.SetSelected(true);
						break;
					}
				}
			}
			if (this.m_OnCharacterDelete != null)
			{
				this.m_OnCharacterDelete.Invoke(this.m_DeletingCharacter.characterInfo);
			}
			UnityEngine.Object.Destroy(this.m_DeletingCharacter.gameObject);
		}

		// Token: 0x06001F9E RID: 8094 RVA: 0x0009F10C File Offset: 0x0009D30C
		private void OnCharacterDeleteCancel()
		{
			this.m_DeletingCharacter = null;
		}

		// Token: 0x04001980 RID: 6528
		[SerializeField]
		private GameObject m_CharacterPrefab;

		// Token: 0x04001981 RID: 6529
		[SerializeField]
		private Transform m_CharactersContainer;

		// Token: 0x04001982 RID: 6530
		[Header("Demo Properties")]
		[SerializeField]
		private bool m_IsDemo;

		// Token: 0x04001983 RID: 6531
		[SerializeField]
		private int m_AddCharacters = 5;

		// Token: 0x04001984 RID: 6532
		[Header("Events")]
		[SerializeField]
		private Demo_CharacterSelectList.OnCharacterSelectedEvent m_OnCharacterSelected = new Demo_CharacterSelectList.OnCharacterSelectedEvent();

		// Token: 0x04001985 RID: 6533
		[SerializeField]
		private Demo_CharacterSelectList.OnCharacterDeleteEvent m_OnCharacterDelete = new Demo_CharacterSelectList.OnCharacterDeleteEvent();

		// Token: 0x04001986 RID: 6534
		private ToggleGroup m_ToggleGroup;

		// Token: 0x04001987 RID: 6535
		private Demo_CharacterSelectList_Character m_DeletingCharacter;

		// Token: 0x02000594 RID: 1428
		[Serializable]
		public class OnCharacterSelectedEvent : UnityEvent<Demo_CharacterInfo>
		{
		}

		// Token: 0x02000595 RID: 1429
		[Serializable]
		public class OnCharacterDeleteEvent : UnityEvent<Demo_CharacterInfo>
		{
		}
	}
}
