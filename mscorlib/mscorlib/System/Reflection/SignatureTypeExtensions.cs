﻿using System;

namespace System.Reflection
{
	// Token: 0x020008C9 RID: 2249
	internal static class SignatureTypeExtensions
	{
		// Token: 0x06004ADC RID: 19164 RVA: 0x000EF9BC File Offset: 0x000EDBBC
		public static bool MatchesParameterTypeExactly(this Type pattern, ParameterInfo parameter)
		{
			SignatureType signatureType = pattern as SignatureType;
			if (signatureType != null)
			{
				return signatureType.MatchesExactly(parameter.ParameterType);
			}
			return pattern == parameter.ParameterType;
		}

		// Token: 0x06004ADD RID: 19165 RVA: 0x000EF9EC File Offset: 0x000EDBEC
		internal static bool MatchesExactly(this SignatureType pattern, Type actual)
		{
			if (pattern.IsSZArray)
			{
				return actual.IsSZArray && pattern.ElementType.MatchesExactly(actual.GetElementType());
			}
			if (pattern.IsVariableBoundArray)
			{
				return actual.IsVariableBoundArray && pattern.GetArrayRank() == actual.GetArrayRank() && pattern.ElementType.MatchesExactly(actual.GetElementType());
			}
			if (pattern.IsByRef)
			{
				return actual.IsByRef && pattern.ElementType.MatchesExactly(actual.GetElementType());
			}
			if (pattern.IsPointer)
			{
				return actual.IsPointer && pattern.ElementType.MatchesExactly(actual.GetElementType());
			}
			if (!pattern.IsConstructedGenericType)
			{
				return pattern.IsGenericMethodParameter && actual.IsGenericMethodParameter && pattern.GenericParameterPosition == actual.GenericParameterPosition;
			}
			if (!actual.IsConstructedGenericType)
			{
				return false;
			}
			if (!(pattern.GetGenericTypeDefinition() == actual.GetGenericTypeDefinition()))
			{
				return false;
			}
			Type[] genericTypeArguments = pattern.GenericTypeArguments;
			Type[] genericTypeArguments2 = actual.GenericTypeArguments;
			int num = genericTypeArguments.Length;
			if (num != genericTypeArguments2.Length)
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				Type type = genericTypeArguments[i];
				SignatureType signatureType = type as SignatureType;
				if (signatureType != null)
				{
					if (!signatureType.MatchesExactly(genericTypeArguments2[i]))
					{
						return false;
					}
				}
				else if (type != genericTypeArguments2[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004ADE RID: 19166 RVA: 0x000EFB39 File Offset: 0x000EDD39
		internal static Type TryResolveAgainstGenericMethod(this SignatureType signatureType, MethodInfo genericMethod)
		{
			return signatureType.TryResolve(genericMethod.GetGenericArguments());
		}

		// Token: 0x06004ADF RID: 19167 RVA: 0x000EFB48 File Offset: 0x000EDD48
		private static Type TryResolve(this SignatureType signatureType, Type[] genericMethodParameters)
		{
			if (signatureType.IsSZArray)
			{
				Type type = signatureType.ElementType.TryResolve(genericMethodParameters);
				if (type == null)
				{
					return null;
				}
				return type.TryMakeArrayType();
			}
			else if (signatureType.IsVariableBoundArray)
			{
				Type type2 = signatureType.ElementType.TryResolve(genericMethodParameters);
				if (type2 == null)
				{
					return null;
				}
				return type2.TryMakeArrayType(signatureType.GetArrayRank());
			}
			else if (signatureType.IsByRef)
			{
				Type type3 = signatureType.ElementType.TryResolve(genericMethodParameters);
				if (type3 == null)
				{
					return null;
				}
				return type3.TryMakeByRefType();
			}
			else if (signatureType.IsPointer)
			{
				Type type4 = signatureType.ElementType.TryResolve(genericMethodParameters);
				if (type4 == null)
				{
					return null;
				}
				return type4.TryMakePointerType();
			}
			else
			{
				if (signatureType.IsConstructedGenericType)
				{
					Type[] genericTypeArguments = signatureType.GenericTypeArguments;
					int num = genericTypeArguments.Length;
					Type[] array = new Type[num];
					for (int i = 0; i < num; i++)
					{
						Type type5 = genericTypeArguments[i];
						SignatureType signatureType2 = type5 as SignatureType;
						if (signatureType2 != null)
						{
							array[i] = signatureType2.TryResolve(genericMethodParameters);
							if (array[i] == null)
							{
								return null;
							}
						}
						else
						{
							array[i] = type5;
						}
					}
					return signatureType.GetGenericTypeDefinition().TryMakeGenericType(array);
				}
				if (!signatureType.IsGenericMethodParameter)
				{
					return null;
				}
				int genericParameterPosition = signatureType.GenericParameterPosition;
				if (genericParameterPosition >= genericMethodParameters.Length)
				{
					return null;
				}
				return genericMethodParameters[genericParameterPosition];
			}
		}

		// Token: 0x06004AE0 RID: 19168 RVA: 0x000EFC5C File Offset: 0x000EDE5C
		private static Type TryMakeArrayType(this Type type)
		{
			Type result;
			try
			{
				result = type.MakeArrayType();
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06004AE1 RID: 19169 RVA: 0x000EFC88 File Offset: 0x000EDE88
		private static Type TryMakeArrayType(this Type type, int rank)
		{
			Type result;
			try
			{
				result = type.MakeArrayType(rank);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06004AE2 RID: 19170 RVA: 0x000EFCB8 File Offset: 0x000EDEB8
		private static Type TryMakeByRefType(this Type type)
		{
			Type result;
			try
			{
				result = type.MakeByRefType();
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06004AE3 RID: 19171 RVA: 0x000EFCE4 File Offset: 0x000EDEE4
		private static Type TryMakePointerType(this Type type)
		{
			Type result;
			try
			{
				result = type.MakePointerType();
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06004AE4 RID: 19172 RVA: 0x000EFD10 File Offset: 0x000EDF10
		private static Type TryMakeGenericType(this Type type, Type[] instantiation)
		{
			Type result;
			try
			{
				result = type.MakeGenericType(instantiation);
			}
			catch
			{
				result = null;
			}
			return result;
		}
	}
}
