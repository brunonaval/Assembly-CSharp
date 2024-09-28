using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// Token: 0x02000348 RID: 840
public static class GameInputModule
{
	// Token: 0x0600106B RID: 4203 RVA: 0x0004D410 File Offset: 0x0004B610
	static GameInputModule()
	{
		GameInputModule.InitializeDefaultDictionary();
	}

	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x0600106C RID: 4204 RVA: 0x0004DCE5 File Offset: 0x0004BEE5
	public static Dictionary<string, KeyMap> KeyMapping
	{
		get
		{
			return GameInputModule.keyMapping;
		}
	}

	// Token: 0x0600106D RID: 4205 RVA: 0x0004DCEC File Offset: 0x0004BEEC
	public static void InitializeDictionary(Dictionary<string, KeyMap> keyMapping)
	{
		GameInputModule.keyMapping = keyMapping;
	}

	// Token: 0x0600106E RID: 4206 RVA: 0x0004DCF4 File Offset: 0x0004BEF4
	public static void InitializeDefaultDictionary()
	{
		GameInputModule.keyMapping = new Dictionary<string, KeyMap>();
		for (int i = 0; i < GameInputModule.keyMaps.Length; i++)
		{
			GameInputModule.keyMapping.Add(GameInputModule.keyMaps[i], GameInputModule.defaults[i]);
		}
	}

	// Token: 0x0600106F RID: 4207 RVA: 0x0004DD3C File Offset: 0x0004BF3C
	public static void InitializeEmptyDictionary()
	{
		GameInputModule.keyMapping = new Dictionary<string, KeyMap>();
		for (int i = 0; i < GameInputModule.keyMaps.Length; i++)
		{
			GameInputModule.keyMapping.Add(GameInputModule.keyMaps[i], new KeyMap
			{
				KeyCode = KeyCode.None,
				AltKeyCode = KeyCode.None
			});
		}
	}

	// Token: 0x06001070 RID: 4208 RVA: 0x0004DD90 File Offset: 0x0004BF90
	public static void SetKeyMap(string mapName, KeyMap key)
	{
		KeyValuePair<string, KeyMap> keyValuePair = GameInputModule.keyMapping.FirstOrDefault((KeyValuePair<string, KeyMap> km) => km.Key != mapName & ((km.Value.KeyCode == key.KeyCode & key.KeyCode > KeyCode.None) | (km.Value.AltKeyCode == key.KeyCode & key.KeyCode > KeyCode.None)));
		KeyValuePair<string, KeyMap> keyValuePair2 = GameInputModule.keyMapping.FirstOrDefault((KeyValuePair<string, KeyMap> km) => km.Key != mapName & ((km.Value.KeyCode == key.AltKeyCode & key.AltKeyCode > KeyCode.None) | (km.Value.AltKeyCode == key.AltKeyCode & key.AltKeyCode > KeyCode.None)));
		if (!string.IsNullOrEmpty(keyValuePair.Key))
		{
			throw new ArgumentException("keycode_already_bound_message")
			{
				Data = 
				{
					{
						"keymap",
						keyValuePair.Key
					}
				}
			};
		}
		if (!string.IsNullOrEmpty(keyValuePair2.Key))
		{
			throw new ArgumentException("keycode_already_bound_message")
			{
				Data = 
				{
					{
						"keymap",
						keyValuePair2.Key
					}
				}
			};
		}
		if (!GameInputModule.keyMapping.ContainsKey(mapName))
		{
			throw new ArgumentException("invalid_keymap_name_message")
			{
				Data = 
				{
					{
						"mapname",
						mapName
					}
				}
			};
		}
		GameInputModule.keyMapping[mapName] = key;
	}

	// Token: 0x06001071 RID: 4209 RVA: 0x0004DE88 File Offset: 0x0004C088
	public static bool GetKeyDown(string mapName)
	{
		return GameInputModule.keyMapping.ContainsKey(mapName) && (Input.GetKeyDown(GameInputModule.keyMapping[mapName].KeyCode) || Input.GetKeyDown(GameInputModule.keyMapping[mapName].AltKeyCode));
	}

	// Token: 0x06001072 RID: 4210 RVA: 0x0004DEC7 File Offset: 0x0004C0C7
	public static bool GetKey(string mapName)
	{
		return Input.GetKey(GameInputModule.keyMapping[mapName].KeyCode) || Input.GetKey(GameInputModule.keyMapping[mapName].AltKeyCode);
	}

	// Token: 0x06001073 RID: 4211 RVA: 0x0004DEF8 File Offset: 0x0004C0F8
	public static Vector2 GetAxis()
	{
		float y = 0f;
		float x = 0f;
		if (GameInputModule.GetKey("Walk Down"))
		{
			y = -1f;
		}
		if (GameInputModule.GetKey("Walk Up"))
		{
			y = 1f;
		}
		if (GameInputModule.GetKey("Walk Left"))
		{
			x = -1f;
		}
		if (GameInputModule.GetKey("Walk Right"))
		{
			x = 1f;
		}
		return new Vector2(x, y);
	}

	// Token: 0x06001074 RID: 4212 RVA: 0x0004DF60 File Offset: 0x0004C160
	public static KeyMap GetKeyMap(string mapName)
	{
		if (!GameInputModule.keyMapping.ContainsKey(mapName))
		{
			Debug.LogError("KeyMap not found: " + mapName);
			return default(KeyMap);
		}
		return GameInputModule.keyMapping[mapName];
	}

	// Token: 0x06001075 RID: 4213 RVA: 0x0004DFA0 File Offset: 0x0004C1A0
	public static KeyCode DetectKeyPressed()
	{
		foreach (object obj in Enum.GetValues(typeof(KeyCode)))
		{
			KeyCode keyCode = (KeyCode)obj;
			if (Input.GetKey(keyCode) & !Input.GetMouseButton(0))
			{
				return keyCode;
			}
		}
		return KeyCode.None;
	}

	// Token: 0x06001076 RID: 4214 RVA: 0x0004E014 File Offset: 0x0004C214
	public static void RestoreDefault()
	{
		if (PlayerPrefs.HasKey("KeyMapping"))
		{
			PlayerPrefs.DeleteKey("KeyMapping");
		}
		GameInputModule.InitializeDefaultDictionary();
	}

	// Token: 0x06001077 RID: 4215 RVA: 0x0004E034 File Offset: 0x0004C234
	public static void SaveMapping()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<string, KeyMap> keyValuePair in GameInputModule.keyMapping)
		{
			stringBuilder.AppendLine(string.Format("{0};{1};{2}", keyValuePair.Key, keyValuePair.Value.KeyCode, keyValuePair.Value.AltKeyCode));
		}
		PlayerPrefs.SetString("KeyMapping", stringBuilder.ToString());
	}

	// Token: 0x06001078 RID: 4216 RVA: 0x0004E0D0 File Offset: 0x0004C2D0
	public static void LoadMapping()
	{
		if (!PlayerPrefs.HasKey("KeyMapping"))
		{
			return;
		}
		if (GameInputModule.keyMappingLoaded)
		{
			return;
		}
		try
		{
			GameInputModule.InitializeEmptyDictionary();
			string[] array = PlayerPrefs.GetString("KeyMapping").Split(new string[]
			{
				Environment.NewLine
			}, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new string[]
				{
					";"
				}, StringSplitOptions.RemoveEmptyEntries);
				string text = array2[0];
				KeyCode keyCode;
				Enum.TryParse<KeyCode>(array2[1], out keyCode);
				KeyCode keyCode2;
				Enum.TryParse<KeyCode>(array2[2], out keyCode2);
				Debug.Log(string.Format("Processing: {0}/{1}/{2}", text, keyCode, keyCode2));
				GameInputModule.SetKeyMap(text, new KeyMap
				{
					KeyCode = keyCode,
					AltKeyCode = keyCode2
				});
			}
			GameInputModule.keyMappingLoaded = true;
		}
		catch (Exception ex)
		{
			foreach (object obj in ex.Data.Values)
			{
				Debug.Log(string.Format(LanguageManager.Instance.GetText(ex.Message), GlobalUtils.KeyMapNameToString(obj.ToString())));
			}
			GameInputModule.InitializeDefaultDictionary();
		}
	}

	// Token: 0x04000FEE RID: 4078
	private static bool keyMappingLoaded;

	// Token: 0x04000FEF RID: 4079
	private const int totalKeys = 50;

	// Token: 0x04000FF0 RID: 4080
	private static Dictionary<string, KeyMap> keyMapping;

	// Token: 0x04000FF1 RID: 4081
	private static readonly string[] keyMaps = new string[]
	{
		"Dash",
		"Action",
		"Submit",
		"Collect",
		"Map Window",
		"Change Target",
		"Change SkillBars",
		"Cancel",
		"Player Window",
		"Skills Window",
		"Attributes Window",
		"Inventory Window",
		"Quest Window",
		"FriendList Window",
		"Help Window",
		"Storage Window",
		"Item1",
		"Item2",
		"Item3",
		"Item4",
		"Item5",
		"Item6",
		"Item7",
		"Item8",
		"Item9",
		"Item10",
		"Skill1",
		"Skill2",
		"Skill3",
		"Skill4",
		"Skill5",
		"Skill6",
		"Skill7",
		"Skill8",
		"Skill9",
		"Skill0",
		"Second Skill1",
		"Second Skill2",
		"Second Skill3",
		"Second Skill4",
		"Second Skill5",
		"Second Skill6",
		"Second Skill7",
		"Second Skill8",
		"Second Skill9",
		"Second Skill0",
		"Walk Up",
		"Walk Left",
		"Walk Down",
		"Walk Right"
	};

	// Token: 0x04000FF2 RID: 4082
	private static readonly KeyMap[] defaults = new KeyMap[]
	{
		new KeyMap
		{
			KeyCode = KeyCode.V,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.F,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.Return,
			AltKeyCode = KeyCode.KeypadEnter
		},
		new KeyMap
		{
			KeyCode = KeyCode.Space,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.M,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.Tab,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.Quote,
			AltKeyCode = KeyCode.BackQuote
		},
		new KeyMap
		{
			KeyCode = KeyCode.Escape,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.P,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.K,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.T,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.I,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.J,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.Y,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.H,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.O,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.F1,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.F2,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.F3,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.F4,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.F5,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.F6,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.F7,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.F8,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.F9,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.F10,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.Alpha1,
			AltKeyCode = KeyCode.Keypad1
		},
		new KeyMap
		{
			KeyCode = KeyCode.Alpha2,
			AltKeyCode = KeyCode.Keypad2
		},
		new KeyMap
		{
			KeyCode = KeyCode.Alpha3,
			AltKeyCode = KeyCode.Keypad3
		},
		new KeyMap
		{
			KeyCode = KeyCode.Alpha4,
			AltKeyCode = KeyCode.Keypad4
		},
		new KeyMap
		{
			KeyCode = KeyCode.Alpha5,
			AltKeyCode = KeyCode.Keypad5
		},
		new KeyMap
		{
			KeyCode = KeyCode.Alpha6,
			AltKeyCode = KeyCode.Keypad6
		},
		new KeyMap
		{
			KeyCode = KeyCode.Alpha7,
			AltKeyCode = KeyCode.Keypad7
		},
		new KeyMap
		{
			KeyCode = KeyCode.Alpha8,
			AltKeyCode = KeyCode.Keypad8
		},
		new KeyMap
		{
			KeyCode = KeyCode.Alpha9,
			AltKeyCode = KeyCode.Keypad9
		},
		new KeyMap
		{
			KeyCode = KeyCode.Alpha0,
			AltKeyCode = KeyCode.Keypad0
		},
		new KeyMap
		{
			KeyCode = KeyCode.Q,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.E,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.R,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.Z,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.X,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.C,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.None,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.None,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.None,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.None,
			AltKeyCode = KeyCode.None
		},
		new KeyMap
		{
			KeyCode = KeyCode.W,
			AltKeyCode = KeyCode.UpArrow
		},
		new KeyMap
		{
			KeyCode = KeyCode.A,
			AltKeyCode = KeyCode.LeftArrow
		},
		new KeyMap
		{
			KeyCode = KeyCode.S,
			AltKeyCode = KeyCode.DownArrow
		},
		new KeyMap
		{
			KeyCode = KeyCode.D,
			AltKeyCode = KeyCode.RightArrow
		}
	};
}
