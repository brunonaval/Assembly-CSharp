﻿using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	/// <summary>Provides support for alternative ways to generate the Microsoft intermediate language (MSIL) and metadata for a dynamic method, including methods for creating tokens and for inserting the code, exception handling, and local variable signature blobs.</summary>
	// Token: 0x0200091C RID: 2332
	[ComVisible(true)]
	public class DynamicILInfo
	{
		// Token: 0x06004F63 RID: 20323 RVA: 0x0000259F File Offset: 0x0000079F
		internal DynamicILInfo()
		{
		}

		// Token: 0x06004F64 RID: 20324 RVA: 0x000F9A3A File Offset: 0x000F7C3A
		internal DynamicILInfo(DynamicMethod method)
		{
			this.method = method;
		}

		/// <summary>Gets the dynamic method whose body is generated by the current instance.</summary>
		/// <returns>A <see cref="T:System.Reflection.Emit.DynamicMethod" /> object representing the dynamic method for which the current <see cref="T:System.Reflection.Emit.DynamicILInfo" /> object is generating code.</returns>
		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x06004F65 RID: 20325 RVA: 0x000F9A49 File Offset: 0x000F7C49
		public DynamicMethod DynamicMethod
		{
			get
			{
				return this.method;
			}
		}

		/// <summary>Gets a token, valid in the scope of the current <see cref="T:System.Reflection.Emit.DynamicILInfo" />, representing the signature for the associated dynamic method.</summary>
		/// <param name="signature">An array that contains the signature.</param>
		/// <returns>A token that can be embedded in the metadata and the MSIL stream for the associated dynamic method.</returns>
		// Token: 0x06004F66 RID: 20326 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public int GetTokenFor(byte[] signature)
		{
			throw new NotImplementedException();
		}

		/// <summary>Gets a token, valid in the scope of the current <see cref="T:System.Reflection.Emit.DynamicILInfo" />, representing a dynamic method to be called from the associated method.</summary>
		/// <param name="method">The dynamic method to call.</param>
		/// <returns>A token that can be embedded in the MSIL stream for the associated dynamic method, as the target of an MSIL instruction.</returns>
		// Token: 0x06004F67 RID: 20327 RVA: 0x000F9A51 File Offset: 0x000F7C51
		public int GetTokenFor(DynamicMethod method)
		{
			return this.method.GetILGenerator().TokenGenerator.GetToken(method, false);
		}

		/// <summary>Gets a token, valid in the scope of the current <see cref="T:System.Reflection.Emit.DynamicILInfo" />, representing a field to be accessed from the associated dynamic method.</summary>
		/// <param name="field">The field to be accessed.</param>
		/// <returns>A token that can be used as the operand of an MSIL instruction that accesses fields, in the scope of the current <see cref="T:System.Reflection.Emit.DynamicILInfo" /> object.</returns>
		// Token: 0x06004F68 RID: 20328 RVA: 0x000F9A6A File Offset: 0x000F7C6A
		public int GetTokenFor(RuntimeFieldHandle field)
		{
			return this.method.GetILGenerator().TokenGenerator.GetToken(FieldInfo.GetFieldFromHandle(field), false);
		}

		/// <summary>Gets a token, valid in the scope of the current <see cref="T:System.Reflection.Emit.DynamicILInfo" />, representing a method to be accessed from the associated dynamic method.</summary>
		/// <param name="method">The method to be accessed.</param>
		/// <returns>A token that can be used as the operand of an MSIL instruction that accesses methods, such as <see cref="F:System.Reflection.Emit.OpCodes.Call" /> or <see cref="F:System.Reflection.Emit.OpCodes.Ldtoken" />, in the scope of the current <see cref="T:System.Reflection.Emit.DynamicILInfo" /> object.</returns>
		// Token: 0x06004F69 RID: 20329 RVA: 0x000F9A88 File Offset: 0x000F7C88
		public int GetTokenFor(RuntimeMethodHandle method)
		{
			MethodBase methodFromHandle = MethodBase.GetMethodFromHandle(method);
			return this.method.GetILGenerator().TokenGenerator.GetToken(methodFromHandle, false);
		}

		/// <summary>Gets a token, valid in the scope of the current <see cref="T:System.Reflection.Emit.DynamicILInfo" />, representing a type to be used in the associated dynamic method.</summary>
		/// <param name="type">The type to be used.</param>
		/// <returns>A token that can be used as the operand of an MSIL instruction that requires a type, in the scope of the current <see cref="T:System.Reflection.Emit.DynamicILInfo" /> object.</returns>
		// Token: 0x06004F6A RID: 20330 RVA: 0x000F9AB4 File Offset: 0x000F7CB4
		public int GetTokenFor(RuntimeTypeHandle type)
		{
			Type typeFromHandle = Type.GetTypeFromHandle(type);
			return this.method.GetILGenerator().TokenGenerator.GetToken(typeFromHandle, false);
		}

		/// <summary>Gets a token, valid in the scope of the current <see cref="T:System.Reflection.Emit.DynamicILInfo" />, representing a string literal to be used in the associated dynamic method.</summary>
		/// <param name="literal">The string to be used.</param>
		/// <returns>A token that can be used as the operand of an MSIL instruction that requires a string, in the scope of the current <see cref="T:System.Reflection.Emit.DynamicILInfo" /> object.</returns>
		// Token: 0x06004F6B RID: 20331 RVA: 0x000F9ADF File Offset: 0x000F7CDF
		public int GetTokenFor(string literal)
		{
			return this.method.GetILGenerator().TokenGenerator.GetToken(literal);
		}

		/// <summary>Gets a token, valid in the scope of the current <see cref="T:System.Reflection.Emit.DynamicILInfo" />, representing a method on a generic type.</summary>
		/// <param name="method">The method.</param>
		/// <param name="contextType">The generic type the method belongs to.</param>
		/// <returns>A token that can be used as the operand of an MSIL instruction that accesses methods, such as <see cref="F:System.Reflection.Emit.OpCodes.Call" /> or <see cref="F:System.Reflection.Emit.OpCodes.Ldtoken" />, in the scope of the current <see cref="T:System.Reflection.Emit.DynamicILInfo" /> object.</returns>
		// Token: 0x06004F6C RID: 20332 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public int GetTokenFor(RuntimeMethodHandle method, RuntimeTypeHandle contextType)
		{
			throw new NotImplementedException();
		}

		/// <summary>Gets a token, valid in the scope of the current <see cref="T:System.Reflection.Emit.DynamicILInfo" />, representing a field to be accessed from the associated dynamic method; the field is on the specified generic type.</summary>
		/// <param name="field">The field to be accessed.</param>
		/// <param name="contextType">The generic type the field belongs to.</param>
		/// <returns>A token that can be used as the operand of an MSIL instruction that accesses fields in the scope of the current <see cref="T:System.Reflection.Emit.DynamicILInfo" /> object.</returns>
		// Token: 0x06004F6D RID: 20333 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public int GetTokenFor(RuntimeFieldHandle field, RuntimeTypeHandle contextType)
		{
			throw new NotImplementedException();
		}

		/// <summary>Sets the code body of the associated dynamic method.</summary>
		/// <param name="code">An array that contains the MSIL stream.</param>
		/// <param name="maxStackSize">The maximum number of items on the operand stack when the method is executing.</param>
		// Token: 0x06004F6E RID: 20334 RVA: 0x000F9AF7 File Offset: 0x000F7CF7
		public void SetCode(byte[] code, int maxStackSize)
		{
			if (code == null)
			{
				throw new ArgumentNullException("code");
			}
			this.method.GetILGenerator().SetCode(code, maxStackSize);
		}

		/// <summary>Sets the code body of the associated dynamic method.</summary>
		/// <param name="code">A pointer to a byte array containing the MSIL stream.</param>
		/// <param name="codeSize">The number of bytes in the MSIL stream.</param>
		/// <param name="maxStackSize">The maximum number of items on the operand stack when the method is executing.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="code" /> is <see langword="null" /> and <paramref name="codeSize" /> is greater than 0.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="codeSize" /> is less than 0.</exception>
		// Token: 0x06004F6F RID: 20335 RVA: 0x000F9B19 File Offset: 0x000F7D19
		[CLSCompliant(false)]
		public unsafe void SetCode(byte* code, int codeSize, int maxStackSize)
		{
			if (code == null)
			{
				throw new ArgumentNullException("code");
			}
			this.method.GetILGenerator().SetCode(code, codeSize, maxStackSize);
		}

		/// <summary>Sets the exception metadata for the associated dynamic method.</summary>
		/// <param name="exceptions">An array that contains the exception metadata.</param>
		// Token: 0x06004F70 RID: 20336 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public void SetExceptions(byte[] exceptions)
		{
			throw new NotImplementedException();
		}

		/// <summary>Sets the exception metadata for the associated dynamic method.</summary>
		/// <param name="exceptions">A pointer to a byte array containing the exception metadata.</param>
		/// <param name="exceptionsSize">The number of bytes of exception metadata.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="exceptions" /> is <see langword="null" /> and <paramref name="exceptionSize" /> is greater than 0.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="exceptionSize" /> is less than 0.</exception>
		// Token: 0x06004F71 RID: 20337 RVA: 0x000479FC File Offset: 0x00045BFC
		[CLSCompliant(false)]
		[MonoTODO]
		public unsafe void SetExceptions(byte* exceptions, int exceptionsSize)
		{
			throw new NotImplementedException();
		}

		/// <summary>Sets the local variable signature that describes the layout of local variables for the associated dynamic method.</summary>
		/// <param name="localSignature">An array that contains the layout of local variables for the associated <see cref="T:System.Reflection.Emit.DynamicMethod" />.</param>
		// Token: 0x06004F72 RID: 20338 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public void SetLocalSignature(byte[] localSignature)
		{
			throw new NotImplementedException();
		}

		/// <summary>Sets the local variable signature that describes the layout of local variables for the associated dynamic method.</summary>
		/// <param name="localSignature">An array that contains the layout of local variables for the associated <see cref="T:System.Reflection.Emit.DynamicMethod" />.</param>
		/// <param name="signatureSize">The number of bytes in the signature.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="localSignature" /> is <see langword="null" /> and <paramref name="signatureSize" /> is greater than 0.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="signatureSize" /> is less than 0.</exception>
		// Token: 0x06004F73 RID: 20339 RVA: 0x000F9B40 File Offset: 0x000F7D40
		[CLSCompliant(false)]
		public unsafe void SetLocalSignature(byte* localSignature, int signatureSize)
		{
			byte[] array = new byte[signatureSize];
			for (int i = 0; i < signatureSize; i++)
			{
				array[i] = localSignature[i];
			}
		}

		// Token: 0x04003135 RID: 12597
		private DynamicMethod method;
	}
}