using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Tilemaps
{
	// Token: 0x0200057B RID: 1403
	[AddComponentMenu("Tilemap/Grid Information")]
	[Serializable]
	public class GridInformation : MonoBehaviour, ISerializationCallbackReceiver
	{
		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06001F27 RID: 7975 RVA: 0x0009CBEE File Offset: 0x0009ADEE
		internal Dictionary<GridInformation.GridInformationKey, GridInformation.GridInformationValue> PositionProperties
		{
			get
			{
				return this.m_PositionProperties;
			}
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x0009CBF8 File Offset: 0x0009ADF8
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (base.GetComponentInParent<Grid>() == null)
			{
				return;
			}
			this.m_PositionIntKeys.Clear();
			this.m_PositionIntValues.Clear();
			this.m_PositionStringKeys.Clear();
			this.m_PositionStringValues.Clear();
			this.m_PositionFloatKeys.Clear();
			this.m_PositionFloatValues.Clear();
			this.m_PositionDoubleKeys.Clear();
			this.m_PositionDoubleValues.Clear();
			this.m_PositionObjectKeys.Clear();
			this.m_PositionObjectValues.Clear();
			this.m_PositionColorKeys.Clear();
			this.m_PositionColorValues.Clear();
			foreach (KeyValuePair<GridInformation.GridInformationKey, GridInformation.GridInformationValue> keyValuePair in this.m_PositionProperties)
			{
				switch (keyValuePair.Value.type)
				{
				case GridInformationType.Integer:
					this.m_PositionIntKeys.Add(keyValuePair.Key);
					this.m_PositionIntValues.Add((int)keyValuePair.Value.data);
					continue;
				case GridInformationType.String:
					this.m_PositionStringKeys.Add(keyValuePair.Key);
					this.m_PositionStringValues.Add(keyValuePair.Value.data as string);
					continue;
				case GridInformationType.Float:
					this.m_PositionFloatKeys.Add(keyValuePair.Key);
					this.m_PositionFloatValues.Add((float)keyValuePair.Value.data);
					continue;
				case GridInformationType.Double:
					this.m_PositionDoubleKeys.Add(keyValuePair.Key);
					this.m_PositionDoubleValues.Add((double)keyValuePair.Value.data);
					continue;
				case GridInformationType.Color:
					this.m_PositionColorKeys.Add(keyValuePair.Key);
					this.m_PositionColorValues.Add((Color)keyValuePair.Value.data);
					continue;
				}
				this.m_PositionObjectKeys.Add(keyValuePair.Key);
				this.m_PositionObjectValues.Add(keyValuePair.Value.data as Object);
			}
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x0009CE40 File Offset: 0x0009B040
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this.m_PositionProperties.Clear();
			for (int num = 0; num != Math.Min(this.m_PositionIntKeys.Count, this.m_PositionIntValues.Count); num++)
			{
				GridInformation.GridInformationValue value;
				value.type = GridInformationType.Integer;
				value.data = this.m_PositionIntValues[num];
				this.m_PositionProperties.Add(this.m_PositionIntKeys[num], value);
			}
			for (int num2 = 0; num2 != Math.Min(this.m_PositionStringKeys.Count, this.m_PositionStringValues.Count); num2++)
			{
				GridInformation.GridInformationValue value2;
				value2.type = GridInformationType.String;
				value2.data = this.m_PositionStringValues[num2];
				this.m_PositionProperties.Add(this.m_PositionStringKeys[num2], value2);
			}
			for (int num3 = 0; num3 != Math.Min(this.m_PositionFloatKeys.Count, this.m_PositionFloatValues.Count); num3++)
			{
				GridInformation.GridInformationValue value3;
				value3.type = GridInformationType.Float;
				value3.data = this.m_PositionFloatValues[num3];
				this.m_PositionProperties.Add(this.m_PositionFloatKeys[num3], value3);
			}
			for (int num4 = 0; num4 != Math.Min(this.m_PositionDoubleKeys.Count, this.m_PositionDoubleValues.Count); num4++)
			{
				GridInformation.GridInformationValue value4;
				value4.type = GridInformationType.Double;
				value4.data = this.m_PositionDoubleValues[num4];
				this.m_PositionProperties.Add(this.m_PositionDoubleKeys[num4], value4);
			}
			for (int num5 = 0; num5 != Math.Min(this.m_PositionObjectKeys.Count, this.m_PositionObjectValues.Count); num5++)
			{
				GridInformation.GridInformationValue value5;
				value5.type = GridInformationType.UnityObject;
				value5.data = this.m_PositionObjectValues[num5];
				this.m_PositionProperties.Add(this.m_PositionObjectKeys[num5], value5);
			}
			for (int num6 = 0; num6 != Math.Min(this.m_PositionColorKeys.Count, this.m_PositionColorValues.Count); num6++)
			{
				GridInformation.GridInformationValue value6;
				value6.type = GridInformationType.Color;
				value6.data = this.m_PositionColorValues[num6];
				this.m_PositionProperties.Add(this.m_PositionColorKeys[num6], value6);
			}
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x0009D09E File Offset: 0x0009B29E
		public bool SetPositionProperty<T>(Vector3Int position, string name, T positionProperty)
		{
			throw new NotImplementedException("Storing this type is not accepted in GridInformation");
		}

		// Token: 0x06001F2B RID: 7979 RVA: 0x0009D0AA File Offset: 0x0009B2AA
		public bool SetPositionProperty(Vector3Int position, string name, int positionProperty)
		{
			return this.SetPositionProperty(position, name, GridInformationType.Integer, positionProperty);
		}

		// Token: 0x06001F2C RID: 7980 RVA: 0x0009D0BB File Offset: 0x0009B2BB
		public bool SetPositionProperty(Vector3Int position, string name, string positionProperty)
		{
			return this.SetPositionProperty(position, name, GridInformationType.String, positionProperty);
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x0009D0C7 File Offset: 0x0009B2C7
		public bool SetPositionProperty(Vector3Int position, string name, float positionProperty)
		{
			return this.SetPositionProperty(position, name, GridInformationType.Float, positionProperty);
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x0009D0D8 File Offset: 0x0009B2D8
		public bool SetPositionProperty(Vector3Int position, string name, double positionProperty)
		{
			return this.SetPositionProperty(position, name, GridInformationType.Double, positionProperty);
		}

		// Token: 0x06001F2F RID: 7983 RVA: 0x0009D0E9 File Offset: 0x0009B2E9
		public bool SetPositionProperty(Vector3Int position, string name, Object positionProperty)
		{
			return this.SetPositionProperty(position, name, GridInformationType.UnityObject, positionProperty);
		}

		// Token: 0x06001F30 RID: 7984 RVA: 0x0009D0F5 File Offset: 0x0009B2F5
		public bool SetPositionProperty(Vector3Int position, string name, Color positionProperty)
		{
			return this.SetPositionProperty(position, name, GridInformationType.Color, positionProperty);
		}

		// Token: 0x06001F31 RID: 7985 RVA: 0x0009D108 File Offset: 0x0009B308
		private bool SetPositionProperty(Vector3Int position, string name, GridInformationType dataType, object positionProperty)
		{
			if (base.GetComponentInParent<Grid>() != null && positionProperty != null)
			{
				GridInformation.GridInformationKey key;
				key.position = position;
				key.name = name;
				GridInformation.GridInformationValue value;
				value.type = dataType;
				value.data = positionProperty;
				this.m_PositionProperties[key] = value;
				return true;
			}
			return false;
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x0009D158 File Offset: 0x0009B358
		public T GetPositionProperty<T>(Vector3Int position, string name, T defaultValue) where T : Object
		{
			GridInformation.GridInformationKey key;
			key.position = position;
			key.name = name;
			GridInformation.GridInformationValue gridInformationValue;
			if (!this.m_PositionProperties.TryGetValue(key, out gridInformationValue))
			{
				return defaultValue;
			}
			if (gridInformationValue.type != GridInformationType.UnityObject)
			{
				throw new InvalidCastException("Value stored in GridInformation is not of the right type");
			}
			return gridInformationValue.data as T;
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x0009D1AC File Offset: 0x0009B3AC
		public int GetPositionProperty(Vector3Int position, string name, int defaultValue)
		{
			GridInformation.GridInformationKey key;
			key.position = position;
			key.name = name;
			GridInformation.GridInformationValue gridInformationValue;
			if (!this.m_PositionProperties.TryGetValue(key, out gridInformationValue))
			{
				return defaultValue;
			}
			if (gridInformationValue.type != GridInformationType.Integer)
			{
				throw new InvalidCastException("Value stored in GridInformation is not of the right type");
			}
			return (int)gridInformationValue.data;
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x0009D1FC File Offset: 0x0009B3FC
		public string GetPositionProperty(Vector3Int position, string name, string defaultValue)
		{
			GridInformation.GridInformationKey key;
			key.position = position;
			key.name = name;
			GridInformation.GridInformationValue gridInformationValue;
			if (!this.m_PositionProperties.TryGetValue(key, out gridInformationValue))
			{
				return defaultValue;
			}
			if (gridInformationValue.type != GridInformationType.String)
			{
				throw new InvalidCastException("Value stored in GridInformation is not of the right type");
			}
			return (string)gridInformationValue.data;
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x0009D24C File Offset: 0x0009B44C
		public float GetPositionProperty(Vector3Int position, string name, float defaultValue)
		{
			GridInformation.GridInformationKey key;
			key.position = position;
			key.name = name;
			GridInformation.GridInformationValue gridInformationValue;
			if (!this.m_PositionProperties.TryGetValue(key, out gridInformationValue))
			{
				return defaultValue;
			}
			if (gridInformationValue.type != GridInformationType.Float)
			{
				throw new InvalidCastException("Value stored in GridInformation is not of the right type");
			}
			return (float)gridInformationValue.data;
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x0009D29C File Offset: 0x0009B49C
		public double GetPositionProperty(Vector3Int position, string name, double defaultValue)
		{
			GridInformation.GridInformationKey key;
			key.position = position;
			key.name = name;
			GridInformation.GridInformationValue gridInformationValue;
			if (!this.m_PositionProperties.TryGetValue(key, out gridInformationValue))
			{
				return defaultValue;
			}
			if (gridInformationValue.type != GridInformationType.Double)
			{
				throw new InvalidCastException("Value stored in GridInformation is not of the right type");
			}
			return (double)gridInformationValue.data;
		}

		// Token: 0x06001F37 RID: 7991 RVA: 0x0009D2EC File Offset: 0x0009B4EC
		public Color GetPositionProperty(Vector3Int position, string name, Color defaultValue)
		{
			GridInformation.GridInformationKey key;
			key.position = position;
			key.name = name;
			GridInformation.GridInformationValue gridInformationValue;
			if (!this.m_PositionProperties.TryGetValue(key, out gridInformationValue))
			{
				return defaultValue;
			}
			if (gridInformationValue.type != GridInformationType.Color)
			{
				throw new InvalidCastException("Value stored in GridInformation is not of the right type");
			}
			return (Color)gridInformationValue.data;
		}

		// Token: 0x06001F38 RID: 7992 RVA: 0x0009D33C File Offset: 0x0009B53C
		public bool ErasePositionProperty(Vector3Int position, string name)
		{
			GridInformation.GridInformationKey key;
			key.position = position;
			key.name = name;
			return this.m_PositionProperties.Remove(key);
		}

		// Token: 0x06001F39 RID: 7993 RVA: 0x0009D365 File Offset: 0x0009B565
		public virtual void Reset()
		{
			this.m_PositionProperties.Clear();
		}

		// Token: 0x06001F3A RID: 7994 RVA: 0x0009D374 File Offset: 0x0009B574
		public Vector3Int[] GetAllPositions(string propertyName)
		{
			return (from x in this.m_PositionProperties.Keys.ToList<GridInformation.GridInformationKey>().FindAll((GridInformation.GridInformationKey x) => x.name == propertyName)
			select x.position).ToArray<Vector3Int>();
		}

		// Token: 0x0400192D RID: 6445
		private Dictionary<GridInformation.GridInformationKey, GridInformation.GridInformationValue> m_PositionProperties = new Dictionary<GridInformation.GridInformationKey, GridInformation.GridInformationValue>();

		// Token: 0x0400192E RID: 6446
		[SerializeField]
		[HideInInspector]
		private List<GridInformation.GridInformationKey> m_PositionIntKeys = new List<GridInformation.GridInformationKey>();

		// Token: 0x0400192F RID: 6447
		[SerializeField]
		[HideInInspector]
		private List<int> m_PositionIntValues = new List<int>();

		// Token: 0x04001930 RID: 6448
		[SerializeField]
		[HideInInspector]
		private List<GridInformation.GridInformationKey> m_PositionStringKeys = new List<GridInformation.GridInformationKey>();

		// Token: 0x04001931 RID: 6449
		[SerializeField]
		[HideInInspector]
		private List<string> m_PositionStringValues = new List<string>();

		// Token: 0x04001932 RID: 6450
		[SerializeField]
		[HideInInspector]
		private List<GridInformation.GridInformationKey> m_PositionFloatKeys = new List<GridInformation.GridInformationKey>();

		// Token: 0x04001933 RID: 6451
		[SerializeField]
		[HideInInspector]
		private List<float> m_PositionFloatValues = new List<float>();

		// Token: 0x04001934 RID: 6452
		[SerializeField]
		[HideInInspector]
		private List<GridInformation.GridInformationKey> m_PositionDoubleKeys = new List<GridInformation.GridInformationKey>();

		// Token: 0x04001935 RID: 6453
		[SerializeField]
		[HideInInspector]
		private List<double> m_PositionDoubleValues = new List<double>();

		// Token: 0x04001936 RID: 6454
		[SerializeField]
		[HideInInspector]
		private List<GridInformation.GridInformationKey> m_PositionObjectKeys = new List<GridInformation.GridInformationKey>();

		// Token: 0x04001937 RID: 6455
		[SerializeField]
		[HideInInspector]
		private List<Object> m_PositionObjectValues = new List<Object>();

		// Token: 0x04001938 RID: 6456
		[SerializeField]
		[HideInInspector]
		private List<GridInformation.GridInformationKey> m_PositionColorKeys = new List<GridInformation.GridInformationKey>();

		// Token: 0x04001939 RID: 6457
		[SerializeField]
		[HideInInspector]
		private List<Color> m_PositionColorValues = new List<Color>();

		// Token: 0x0200057C RID: 1404
		internal struct GridInformationValue
		{
			// Token: 0x0400193A RID: 6458
			public GridInformationType type;

			// Token: 0x0400193B RID: 6459
			public object data;
		}

		// Token: 0x0200057D RID: 1405
		[Serializable]
		internal struct GridInformationKey
		{
			// Token: 0x0400193C RID: 6460
			public Vector3Int position;

			// Token: 0x0400193D RID: 6461
			public string name;
		}
	}
}
