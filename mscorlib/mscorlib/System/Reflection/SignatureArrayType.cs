using System;

namespace System.Reflection
{
	// Token: 0x020008C1 RID: 2241
	internal sealed class SignatureArrayType : SignatureHasElementType
	{
		// Token: 0x06004A15 RID: 18965 RVA: 0x000EF66B File Offset: 0x000ED86B
		internal SignatureArrayType(SignatureType elementType, int rank, bool isMultiDim) : base(elementType)
		{
			this._rank = rank;
			this._isMultiDim = isMultiDim;
		}

		// Token: 0x06004A16 RID: 18966 RVA: 0x000040F7 File Offset: 0x000022F7
		protected sealed override bool IsArrayImpl()
		{
			return true;
		}

		// Token: 0x06004A17 RID: 18967 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected sealed override bool IsByRefImpl()
		{
			return false;
		}

		// Token: 0x06004A18 RID: 18968 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected sealed override bool IsPointerImpl()
		{
			return false;
		}

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x06004A19 RID: 18969 RVA: 0x000EF682 File Offset: 0x000ED882
		public sealed override bool IsSZArray
		{
			get
			{
				return !this._isMultiDim;
			}
		}

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x06004A1A RID: 18970 RVA: 0x000EF68D File Offset: 0x000ED88D
		public sealed override bool IsVariableBoundArray
		{
			get
			{
				return this._isMultiDim;
			}
		}

		// Token: 0x06004A1B RID: 18971 RVA: 0x000EF695 File Offset: 0x000ED895
		public sealed override int GetArrayRank()
		{
			return this._rank;
		}

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x06004A1C RID: 18972 RVA: 0x000EF69D File Offset: 0x000ED89D
		protected sealed override string Suffix
		{
			get
			{
				if (!this._isMultiDim)
				{
					return "[]";
				}
				if (this._rank == 1)
				{
					return "[*]";
				}
				return "[" + new string(',', this._rank - 1) + "]";
			}
		}

		// Token: 0x04002F2F RID: 12079
		private readonly int _rank;

		// Token: 0x04002F30 RID: 12080
		private readonly bool _isMultiDim;
	}
}
