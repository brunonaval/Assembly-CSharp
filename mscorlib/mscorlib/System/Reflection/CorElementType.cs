﻿using System;

namespace System.Reflection
{
	// Token: 0x020008E1 RID: 2273
	[Serializable]
	internal enum CorElementType : byte
	{
		// Token: 0x04002F6F RID: 12143
		End,
		// Token: 0x04002F70 RID: 12144
		Void,
		// Token: 0x04002F71 RID: 12145
		Boolean,
		// Token: 0x04002F72 RID: 12146
		Char,
		// Token: 0x04002F73 RID: 12147
		I1,
		// Token: 0x04002F74 RID: 12148
		U1,
		// Token: 0x04002F75 RID: 12149
		I2,
		// Token: 0x04002F76 RID: 12150
		U2,
		// Token: 0x04002F77 RID: 12151
		I4,
		// Token: 0x04002F78 RID: 12152
		U4,
		// Token: 0x04002F79 RID: 12153
		I8,
		// Token: 0x04002F7A RID: 12154
		U8,
		// Token: 0x04002F7B RID: 12155
		R4,
		// Token: 0x04002F7C RID: 12156
		R8,
		// Token: 0x04002F7D RID: 12157
		String,
		// Token: 0x04002F7E RID: 12158
		Ptr,
		// Token: 0x04002F7F RID: 12159
		ByRef,
		// Token: 0x04002F80 RID: 12160
		ValueType,
		// Token: 0x04002F81 RID: 12161
		Class,
		// Token: 0x04002F82 RID: 12162
		Var,
		// Token: 0x04002F83 RID: 12163
		Array,
		// Token: 0x04002F84 RID: 12164
		GenericInst,
		// Token: 0x04002F85 RID: 12165
		TypedByRef,
		// Token: 0x04002F86 RID: 12166
		I = 24,
		// Token: 0x04002F87 RID: 12167
		U,
		// Token: 0x04002F88 RID: 12168
		FnPtr = 27,
		// Token: 0x04002F89 RID: 12169
		Object,
		// Token: 0x04002F8A RID: 12170
		SzArray,
		// Token: 0x04002F8B RID: 12171
		MVar,
		// Token: 0x04002F8C RID: 12172
		CModReqd,
		// Token: 0x04002F8D RID: 12173
		CModOpt,
		// Token: 0x04002F8E RID: 12174
		Internal,
		// Token: 0x04002F8F RID: 12175
		Max,
		// Token: 0x04002F90 RID: 12176
		Modifier = 64,
		// Token: 0x04002F91 RID: 12177
		Sentinel,
		// Token: 0x04002F92 RID: 12178
		Pinned = 69,
		// Token: 0x04002F93 RID: 12179
		ELEMENT_TYPE_END = 0,
		// Token: 0x04002F94 RID: 12180
		ELEMENT_TYPE_VOID,
		// Token: 0x04002F95 RID: 12181
		ELEMENT_TYPE_BOOLEAN,
		// Token: 0x04002F96 RID: 12182
		ELEMENT_TYPE_CHAR,
		// Token: 0x04002F97 RID: 12183
		ELEMENT_TYPE_I1,
		// Token: 0x04002F98 RID: 12184
		ELEMENT_TYPE_U1,
		// Token: 0x04002F99 RID: 12185
		ELEMENT_TYPE_I2,
		// Token: 0x04002F9A RID: 12186
		ELEMENT_TYPE_U2,
		// Token: 0x04002F9B RID: 12187
		ELEMENT_TYPE_I4,
		// Token: 0x04002F9C RID: 12188
		ELEMENT_TYPE_U4,
		// Token: 0x04002F9D RID: 12189
		ELEMENT_TYPE_I8,
		// Token: 0x04002F9E RID: 12190
		ELEMENT_TYPE_U8,
		// Token: 0x04002F9F RID: 12191
		ELEMENT_TYPE_R4,
		// Token: 0x04002FA0 RID: 12192
		ELEMENT_TYPE_R8,
		// Token: 0x04002FA1 RID: 12193
		ELEMENT_TYPE_STRING,
		// Token: 0x04002FA2 RID: 12194
		ELEMENT_TYPE_PTR,
		// Token: 0x04002FA3 RID: 12195
		ELEMENT_TYPE_BYREF,
		// Token: 0x04002FA4 RID: 12196
		ELEMENT_TYPE_VALUETYPE,
		// Token: 0x04002FA5 RID: 12197
		ELEMENT_TYPE_CLASS,
		// Token: 0x04002FA6 RID: 12198
		ELEMENT_TYPE_VAR,
		// Token: 0x04002FA7 RID: 12199
		ELEMENT_TYPE_ARRAY,
		// Token: 0x04002FA8 RID: 12200
		ELEMENT_TYPE_GENERICINST,
		// Token: 0x04002FA9 RID: 12201
		ELEMENT_TYPE_TYPEDBYREF,
		// Token: 0x04002FAA RID: 12202
		ELEMENT_TYPE_I = 24,
		// Token: 0x04002FAB RID: 12203
		ELEMENT_TYPE_U,
		// Token: 0x04002FAC RID: 12204
		ELEMENT_TYPE_FNPTR = 27,
		// Token: 0x04002FAD RID: 12205
		ELEMENT_TYPE_OBJECT,
		// Token: 0x04002FAE RID: 12206
		ELEMENT_TYPE_SZARRAY,
		// Token: 0x04002FAF RID: 12207
		ELEMENT_TYPE_MVAR,
		// Token: 0x04002FB0 RID: 12208
		ELEMENT_TYPE_CMOD_REQD,
		// Token: 0x04002FB1 RID: 12209
		ELEMENT_TYPE_CMOD_OPT,
		// Token: 0x04002FB2 RID: 12210
		ELEMENT_TYPE_INTERNAL,
		// Token: 0x04002FB3 RID: 12211
		ELEMENT_TYPE_MAX,
		// Token: 0x04002FB4 RID: 12212
		ELEMENT_TYPE_MODIFIER = 64,
		// Token: 0x04002FB5 RID: 12213
		ELEMENT_TYPE_SENTINEL,
		// Token: 0x04002FB6 RID: 12214
		ELEMENT_TYPE_PINNED = 69
	}
}