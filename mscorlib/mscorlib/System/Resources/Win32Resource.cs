using System;
using System.IO;

namespace System.Resources
{
	// Token: 0x02000874 RID: 2164
	internal abstract class Win32Resource
	{
		// Token: 0x06004813 RID: 18451 RVA: 0x000ED02E File Offset: 0x000EB22E
		internal Win32Resource(NameOrId type, NameOrId name, int language)
		{
			this.type = type;
			this.name = name;
			this.language = language;
		}

		// Token: 0x06004814 RID: 18452 RVA: 0x000ED04B File Offset: 0x000EB24B
		internal Win32Resource(Win32ResourceType type, int name, int language)
		{
			this.type = new NameOrId((int)type);
			this.name = new NameOrId(name);
			this.language = language;
		}

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x06004815 RID: 18453 RVA: 0x000ED072 File Offset: 0x000EB272
		public Win32ResourceType ResourceType
		{
			get
			{
				if (this.type.IsName)
				{
					return (Win32ResourceType)(-1);
				}
				return (Win32ResourceType)this.type.Id;
			}
		}

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x06004816 RID: 18454 RVA: 0x000ED08E File Offset: 0x000EB28E
		public NameOrId Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x06004817 RID: 18455 RVA: 0x000ED096 File Offset: 0x000EB296
		public NameOrId Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x06004818 RID: 18456 RVA: 0x000ED09E File Offset: 0x000EB29E
		public int Language
		{
			get
			{
				return this.language;
			}
		}

		// Token: 0x06004819 RID: 18457
		public abstract void WriteTo(Stream s);

		// Token: 0x0600481A RID: 18458 RVA: 0x000ED0A8 File Offset: 0x000EB2A8
		public override string ToString()
		{
			string[] array = new string[5];
			array[0] = "Win32Resource (Kind=";
			array[1] = this.ResourceType.ToString();
			array[2] = ", Name=";
			int num = 3;
			NameOrId nameOrId = this.name;
			array[num] = ((nameOrId != null) ? nameOrId.ToString() : null);
			array[4] = ")";
			return string.Concat(array);
		}

		// Token: 0x04002E2A RID: 11818
		private NameOrId type;

		// Token: 0x04002E2B RID: 11819
		private NameOrId name;

		// Token: 0x04002E2C RID: 11820
		private int language;
	}
}
