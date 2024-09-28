﻿using System;
using System.Configuration.Assemblies;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Security;
using System.Security.Policy;
using System.Threading;
using Unity;

namespace System
{
	/// <summary>Contains methods to create types of objects locally or remotely, or obtain references to existing remote objects. This class cannot be inherited.</summary>
	// Token: 0x020001EA RID: 490
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_Activator))]
	public sealed class Activator : _Activator
	{
		// Token: 0x060014E5 RID: 5349 RVA: 0x0000259F File Offset: 0x0000079F
		private Activator()
		{
		}

		/// <summary>Creates an instance of the specified type using the constructor that best matches the specified parameters.</summary>
		/// <param name="type">The type of object to create.</param>
		/// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="type" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted.</param>
		/// <param name="binder">An object that uses <paramref name="bindingAttr" /> and <paramref name="args" /> to seek and identify the <paramref name="type" /> constructor. If <paramref name="binder" /> is <see langword="null" />, the default binder is used.</param>
		/// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or <see langword="null" />, the constructor that takes no parameters (the default constructor) is invoked.</param>
		/// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="type" /> constructor. If <paramref name="culture" /> is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.</param>
		/// <returns>A reference to the newly created object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> is not a <see langword="RuntimeType" />.  
		/// -or-  
		/// <paramref name="type" /> is an open generic type (that is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns <see langword="true" />).</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="type" /> cannot be a <see cref="T:System.Reflection.Emit.TypeBuilder" />.  
		/// -or-  
		/// Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.  
		/// -or-  
		/// The assembly that contains <paramref name="type" /> is a dynamic assembly that was created with <see cref="F:System.Reflection.Emit.AssemblyBuilderAccess.Save" />.  
		/// -or-  
		/// The constructor that best matches <paramref name="args" /> has <see langword="varargs" /> arguments.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor being called throws an exception.</exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />.</exception>
		/// <exception cref="T:System.MissingMethodException">No matching constructor was found.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">
		///   <paramref name="type" /> is a COM object but the class identifier used to obtain the type is invalid, or the identified class is not registered.</exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="type" /> is not a valid type.</exception>
		// Token: 0x060014E6 RID: 5350 RVA: 0x000523EF File Offset: 0x000505EF
		public static object CreateInstance(Type type, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture)
		{
			return Activator.CreateInstance(type, bindingAttr, binder, args, culture, null);
		}

		/// <summary>Creates an instance of the specified type using the constructor that best matches the specified parameters.</summary>
		/// <param name="type">The type of object to create.</param>
		/// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="type" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted.</param>
		/// <param name="binder">An object that uses <paramref name="bindingAttr" /> and <paramref name="args" /> to seek and identify the <paramref name="type" /> constructor. If <paramref name="binder" /> is <see langword="null" />, the default binder is used.</param>
		/// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or <see langword="null" />, the constructor that takes no parameters (the default constructor) is invoked.</param>
		/// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="type" /> constructor. If <paramref name="culture" /> is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.</param>
		/// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.  
		///  This parameter is related to client-activated objects. Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
		/// <returns>A reference to the newly created object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> is not a <see langword="RuntimeType" />.  
		/// -or-  
		/// <paramref name="type" /> is an open generic type (that is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns <see langword="true" />).</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="type" /> cannot be a <see cref="T:System.Reflection.Emit.TypeBuilder" />.  
		/// -or-  
		/// Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.  
		/// -or-  
		/// <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />.  
		/// -or-  
		/// The assembly that contains <paramref name="type" /> is a dynamic assembly that was created with <see cref="F:System.Reflection.Emit.AssemblyBuilderAccess.Save" />.  
		/// -or-  
		/// The constructor that best matches <paramref name="args" /> has <see langword="varargs" /> arguments.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor being called throws an exception.</exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />.</exception>
		/// <exception cref="T:System.MissingMethodException">No matching constructor was found.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">
		///   <paramref name="type" /> is a COM object but the class identifier used to obtain the type is invalid, or the identified class is not registered.</exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="type" /> is not a valid type.</exception>
		// Token: 0x060014E7 RID: 5351 RVA: 0x00052400 File Offset: 0x00050600
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static object CreateInstance(Type type, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (RuntimeFeature.IsDynamicCodeSupported && type is TypeBuilder)
			{
				throw new NotSupportedException(Environment.GetResourceString("CreateInstance cannot be used with an object of type TypeBuilder."));
			}
			if ((bindingAttr & (BindingFlags)255) == BindingFlags.Default)
			{
				bindingAttr |= (BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance);
			}
			if (activationAttributes != null && activationAttributes.Length != 0)
			{
				if (!type.IsMarshalByRef)
				{
					throw new NotSupportedException(Environment.GetResourceString("Activation Attributes are not supported for types not deriving from MarshalByRefObject."));
				}
				if (!type.IsContextful && (activationAttributes.Length > 1 || !(activationAttributes[0] is UrlAttribute)))
				{
					throw new NotSupportedException(Environment.GetResourceString("UrlAttribute is the only attribute supported for MarshalByRefObject."));
				}
			}
			RuntimeType runtimeType = type.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Type must be a type provided by the runtime."), "type");
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return runtimeType.CreateInstanceImpl(bindingAttr, binder, args, culture, activationAttributes, ref stackCrawlMark);
		}

		/// <summary>Creates an instance of the specified type using the constructor that best matches the specified parameters.</summary>
		/// <param name="type">The type of object to create.</param>
		/// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or <see langword="null" />, the constructor that takes no parameters (the default constructor) is invoked.</param>
		/// <returns>A reference to the newly created object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> is not a <see langword="RuntimeType" />.  
		/// -or-  
		/// <paramref name="type" /> is an open generic type (that is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns <see langword="true" />).</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="type" /> cannot be a <see cref="T:System.Reflection.Emit.TypeBuilder" />.  
		/// -or-  
		/// Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.  
		/// -or-  
		/// The assembly that contains <paramref name="type" /> is a dynamic assembly that was created with <see cref="F:System.Reflection.Emit.AssemblyBuilderAccess.Save" />.  
		/// -or-  
		/// The constructor that best matches <paramref name="args" /> has <see langword="varargs" /> arguments.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor being called throws an exception.</exception>
		/// <exception cref="T:System.MethodAccessException">In the .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.MemberAccessException" />, instead.  
		///
		///
		///
		///
		///  The caller does not have permission to call this constructor.</exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />.</exception>
		/// <exception cref="T:System.MissingMethodException">In the .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.MissingMemberException" />, instead.  
		///
		///
		///
		///
		///  No matching public constructor was found.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">
		///   <paramref name="type" /> is a COM object but the class identifier used to obtain the type is invalid, or the identified class is not registered.</exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="type" /> is not a valid type.</exception>
		// Token: 0x060014E8 RID: 5352 RVA: 0x000524D0 File Offset: 0x000506D0
		public static object CreateInstance(Type type, params object[] args)
		{
			return Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, args, null, null);
		}

		/// <summary>Creates an instance of the specified type using the constructor that best matches the specified parameters.</summary>
		/// <param name="type">The type of object to create.</param>
		/// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or <see langword="null" />, the constructor that takes no parameters (the default constructor) is invoked.</param>
		/// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.  
		///  This parameter is related to client-activated objects. Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
		/// <returns>A reference to the newly created object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> is not a <see langword="RuntimeType" />.  
		/// -or-  
		/// <paramref name="type" /> is an open generic type (that is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns <see langword="true" />).</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="type" /> cannot be a <see cref="T:System.Reflection.Emit.TypeBuilder" />.  
		/// -or-  
		/// Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.  
		/// -or-  
		/// <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />.  
		/// -or-  
		/// The assembly that contains <paramref name="type" /> is a dynamic assembly that was created with <see cref="F:System.Reflection.Emit.AssemblyBuilderAccess.Save" />.  
		/// -or-  
		/// The constructor that best matches <paramref name="args" /> has <see langword="varargs" /> arguments.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor being called throws an exception.</exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />.</exception>
		/// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">
		///   <paramref name="type" /> is a COM object but the class identifier used to obtain the type is invalid, or the identified class is not registered.</exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="type" /> is not a valid type.</exception>
		// Token: 0x060014E9 RID: 5353 RVA: 0x000524E1 File Offset: 0x000506E1
		public static object CreateInstance(Type type, object[] args, object[] activationAttributes)
		{
			return Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, args, null, activationAttributes);
		}

		/// <summary>Creates an instance of the specified type using that type's default constructor.</summary>
		/// <param name="type">The type of object to create.</param>
		/// <returns>A reference to the newly created object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> is not a <see langword="RuntimeType" />.  
		/// -or-  
		/// <paramref name="type" /> is an open generic type (that is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns <see langword="true" />).</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="type" /> cannot be a <see cref="T:System.Reflection.Emit.TypeBuilder" />.  
		/// -or-  
		/// Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.  
		/// -or-  
		/// The assembly that contains <paramref name="type" /> is a dynamic assembly that was created with <see cref="F:System.Reflection.Emit.AssemblyBuilderAccess.Save" />.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor being called throws an exception.</exception>
		/// <exception cref="T:System.MethodAccessException">In the .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.MemberAccessException" />, instead.  
		///
		///
		///
		///
		///  The caller does not have permission to call this constructor.</exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />.</exception>
		/// <exception cref="T:System.MissingMethodException">In the .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.MissingMemberException" />, instead.  
		///
		///
		///
		///
		///  No matching public constructor was found.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">
		///   <paramref name="type" /> is a COM object but the class identifier used to obtain the type is invalid, or the identified class is not registered.</exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="type" /> is not a valid type.</exception>
		// Token: 0x060014EA RID: 5354 RVA: 0x000524F2 File Offset: 0x000506F2
		public static object CreateInstance(Type type)
		{
			return Activator.CreateInstance(type, false);
		}

		/// <summary>Creates an instance of the type whose name is specified, using the named assembly and default constructor.</summary>
		/// <param name="assemblyName">The name of the assembly where the type named <paramref name="typeName" /> is sought. If <paramref name="assemblyName" /> is <see langword="null" />, the executing assembly is searched.</param>
		/// <param name="typeName">The fully qualified name of the preferred type.</param>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyName" />.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyName" /> was not found.</exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
		/// <exception cref="T:System.MemberAccessException">You cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />.</exception>
		/// <exception cref="T:System.NotSupportedException">Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.</exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyName" /> is not a valid assembly.  
		/// -or-  
		/// The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		/// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.  
		///  -or-  
		///  The assembly name or code base is invalid.</exception>
		// Token: 0x060014EB RID: 5355 RVA: 0x000524FC File Offset: 0x000506FC
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ObjectHandle CreateInstance(string assemblyName, string typeName)
		{
			if (assemblyName == null)
			{
				assemblyName = Assembly.GetCallingAssembly().GetName().Name;
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Activator.CreateInstance(assemblyName, typeName, false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null, null, null, ref stackCrawlMark);
		}

		/// <summary>Creates an instance of the type whose name is specified, using the named assembly and default constructor.</summary>
		/// <param name="assemblyName">The name of the assembly where the type named <paramref name="typeName" /> is sought. If <paramref name="assemblyName" /> is <see langword="null" />, the executing assembly is searched.</param>
		/// <param name="typeName">The fully qualified name of the preferred type.</param>
		/// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.  
		///  This parameter is related to client-activated objects. Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyName" />.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyName" /> was not found.</exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />.</exception>
		/// <exception cref="T:System.NotSupportedException">Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.  
		///  -or-  
		///  <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />.  
		///  -or-  
		///  <paramref name="activationAttributes" /> is not a <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" />  
		///  array.</exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyName" /> is not a valid assembly.  
		/// -or-  
		/// The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		/// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.  
		///  -or-  
		///  The assembly name or code base is invalid.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">An error occurred when attempting remote activation in a target specified in <paramref name="activationAttributes" />.</exception>
		// Token: 0x060014EC RID: 5356 RVA: 0x00052534 File Offset: 0x00050734
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ObjectHandle CreateInstance(string assemblyName, string typeName, object[] activationAttributes)
		{
			if (assemblyName == null)
			{
				assemblyName = Assembly.GetCallingAssembly().GetName().Name;
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Activator.CreateInstance(assemblyName, typeName, false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null, activationAttributes, null, ref stackCrawlMark);
		}

		/// <summary>Creates an instance of the specified type using that type's default constructor.</summary>
		/// <param name="type">The type of object to create.</param>
		/// <param name="nonPublic">
		///   <see langword="true" /> if a public or nonpublic default constructor can match; <see langword="false" /> if only a public default constructor can match.</param>
		/// <returns>A reference to the newly created object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> is not a <see langword="RuntimeType" />.  
		/// -or-  
		/// <paramref name="type" /> is an open generic type (that is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns <see langword="true" />).</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="type" /> cannot be a <see cref="T:System.Reflection.Emit.TypeBuilder" />.  
		/// -or-  
		/// Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.  
		/// -or-  
		/// The assembly that contains <paramref name="type" /> is a dynamic assembly that was created with <see cref="F:System.Reflection.Emit.AssemblyBuilderAccess.Save" />.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor being called throws an exception.</exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />.</exception>
		/// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.COMException">
		///   <paramref name="type" /> is a COM object but the class identifier used to obtain the type is invalid, or the identified class is not registered.</exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="type" /> is not a valid type.</exception>
		// Token: 0x060014ED RID: 5357 RVA: 0x0005256B File Offset: 0x0005076B
		public static object CreateInstance(Type type, bool nonPublic)
		{
			return Activator.CreateInstance(type, nonPublic, true);
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x00052578 File Offset: 0x00050778
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static object CreateInstance(Type type, bool nonPublic, bool wrapExceptions)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			RuntimeType runtimeType = type.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Type must be a type provided by the runtime."), "type");
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return runtimeType.CreateInstanceDefaultCtor(!nonPublic, false, true, wrapExceptions, ref stackCrawlMark);
		}

		/// <summary>Creates an instance of the type designated by the specified generic type parameter, using the parameterless constructor.</summary>
		/// <typeparam name="T">The type to create.</typeparam>
		/// <returns>A reference to the newly created object.</returns>
		/// <exception cref="T:System.MissingMethodException">In the .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.MissingMemberException" />, instead.  
		///
		///
		///
		///
		///  The type that is specified for <paramref name="T" /> does not have a parameterless constructor.</exception>
		// Token: 0x060014EF RID: 5359 RVA: 0x000525CC File Offset: 0x000507CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static T CreateInstance<T>()
		{
			RuntimeType runtimeType = typeof(T) as RuntimeType;
			if (runtimeType.HasElementType)
			{
				throw new MissingMethodException(Environment.GetResourceString("No parameterless constructor defined for this object."));
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return (T)((object)runtimeType.CreateInstanceDefaultCtor(true, true, true, true, ref stackCrawlMark));
		}

		/// <summary>Creates an instance of the type whose name is specified, using the named assembly file and default constructor.</summary>
		/// <param name="assemblyFile">The name of a file that contains an assembly where the type named <paramref name="typeName" /> is sought.</param>
		/// <param name="typeName">The name of the preferred type.</param>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyFile" />.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyFile" /> was not found.</exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does have the required <see cref="T:System.Security.Permissions.FileIOPermission" />.</exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyFile" /> is not a valid assembly.  
		/// -or-  
		/// The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		// Token: 0x060014F0 RID: 5360 RVA: 0x00052612 File Offset: 0x00050812
		public static ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName)
		{
			return Activator.CreateInstanceFrom(assemblyFile, typeName, null);
		}

		/// <summary>Creates an instance of the type whose name is specified, using the named assembly file and default constructor.</summary>
		/// <param name="assemblyFile">The name of a file that contains an assembly where the type named <paramref name="typeName" /> is sought.</param>
		/// <param name="typeName">The name of the preferred type.</param>
		/// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.  
		///  This parameter is related to client-activated objects. Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyFile" />.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyFile" /> was not found.</exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does have the required <see cref="T:System.Security.Permissions.FileIOPermission" />.</exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyFile" /> is not a valid assembly.  
		/// -or-  
		/// The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		// Token: 0x060014F1 RID: 5361 RVA: 0x0005261C File Offset: 0x0005081C
		public static ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, object[] activationAttributes)
		{
			return Activator.CreateInstanceFrom(assemblyFile, typeName, false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null, activationAttributes);
		}

		/// <summary>Creates an instance of the type whose name is specified, using the named assembly and the constructor that best matches the specified parameters.</summary>
		/// <param name="assemblyName">The name of the assembly where the type named <paramref name="typeName" /> is sought. If <paramref name="assemblyName" /> is <see langword="null" />, the executing assembly is searched.</param>
		/// <param name="typeName">The fully qualified name of the preferred type.</param>
		/// <param name="ignoreCase">
		///   <see langword="true" /> to specify that the search for <paramref name="typeName" /> is not case-sensitive; <see langword="false" /> to specify that the search is case-sensitive.</param>
		/// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="typeName" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted.</param>
		/// <param name="binder">An object that uses <paramref name="bindingAttr" /> and <paramref name="args" /> to seek and identify the <paramref name="typeName" /> constructor. If <paramref name="binder" /> is <see langword="null" />, the default binder is used.</param>
		/// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or <see langword="null" />, the constructor that takes no parameters (the default constructor) is invoked.</param>
		/// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="typeName" /> constructor. If <paramref name="culture" /> is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.</param>
		/// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.  
		///  This parameter is related to client-activated objects. Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
		/// <param name="securityInfo">Information used to make security policy decisions and grant code permissions.</param>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.MissingMethodException">No matching constructor was found.</exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyName" />.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyName" /> was not found.</exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />.</exception>
		/// <exception cref="T:System.NotSupportedException">Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.  
		///  -or-  
		///  <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />.  
		///  -or-  
		///  The constructor that best matches <paramref name="args" /> has <see langword="varargs" /> arguments.</exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyName" /> is not a valid assembly.  
		/// -or-  
		/// The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		/// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.  
		///  -or-  
		///  The assembly name or code base is invalid.</exception>
		// Token: 0x060014F2 RID: 5362 RVA: 0x00052630 File Offset: 0x00050830
		[Obsolete("Methods which use evidence to sandbox are obsolete and will be removed in a future release of the .NET Framework. Please use an overload of CreateInstance which does not take an Evidence parameter. See http://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ObjectHandle CreateInstance(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityInfo)
		{
			if (assemblyName == null)
			{
				assemblyName = Assembly.GetCallingAssembly().GetName().Name;
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Activator.CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityInfo, ref stackCrawlMark);
		}

		/// <summary>Creates an instance of the type whose name is specified, using the named assembly and the constructor that best matches the specified parameters.</summary>
		/// <param name="assemblyName">The name of the assembly where the type named <paramref name="typeName" /> is sought. If <paramref name="assemblyName" /> is <see langword="null" />, the executing assembly is searched.</param>
		/// <param name="typeName">The fully qualified name of the preferred type.</param>
		/// <param name="ignoreCase">
		///   <see langword="true" /> to specify that the search for <paramref name="typeName" /> is not case-sensitive; <see langword="false" /> to specify that the search is case-sensitive.</param>
		/// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="typeName" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted.</param>
		/// <param name="binder">An object that uses <paramref name="bindingAttr" /> and <paramref name="args" /> to seek and identify the <paramref name="typeName" /> constructor. If <paramref name="binder" /> is <see langword="null" />, the default binder is used.</param>
		/// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or <see langword="null" />, the constructor that takes no parameters (the default constructor) is invoked.</param>
		/// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="typeName" /> constructor. If <paramref name="culture" /> is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.</param>
		/// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.  
		///  This parameter is related to client-activated objects. Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.MissingMethodException">No matching constructor was found.</exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyName" />.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyName" /> was not found.</exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />.</exception>
		/// <exception cref="T:System.NotSupportedException">Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.  
		///  -or-  
		///  <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />.  
		///  -or-  
		///  The constructor that best matches <paramref name="args" /> has <see langword="varargs" /> arguments.</exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyName" /> is not a valid assembly.  
		/// -or-  
		/// The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		/// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.  
		///  -or-  
		///  The assembly name or code base is invalid.</exception>
		// Token: 0x060014F3 RID: 5363 RVA: 0x00052668 File Offset: 0x00050868
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ObjectHandle CreateInstance(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
		{
			if (assemblyName == null)
			{
				assemblyName = Assembly.GetCallingAssembly().GetName().Name;
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Activator.CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, null, ref stackCrawlMark);
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x000526A0 File Offset: 0x000508A0
		[SecurityCritical]
		internal static ObjectHandle CreateInstance(string assemblyString, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityInfo, ref StackCrawlMark stackMark)
		{
			Type type = null;
			Assembly assembly = null;
			if (assemblyString == null)
			{
				assembly = RuntimeAssembly.GetExecutingAssembly(ref stackMark);
			}
			else
			{
				RuntimeAssembly runtimeAssembly;
				AssemblyName assemblyName = RuntimeAssembly.CreateAssemblyName(assemblyString, false, out runtimeAssembly);
				if (runtimeAssembly != null)
				{
					assembly = runtimeAssembly;
				}
				else if (assemblyName.ContentType == AssemblyContentType.WindowsRuntime)
				{
					type = Type.GetType(typeName + ", " + assemblyString, true, ignoreCase);
				}
				else
				{
					assembly = RuntimeAssembly.InternalLoadAssemblyName(assemblyName, securityInfo, null, ref stackMark, true, false, false);
				}
			}
			if (type == null)
			{
				if (assembly == null)
				{
					return null;
				}
				type = assembly.GetType(typeName, true, ignoreCase);
			}
			object obj = Activator.CreateInstance(type, bindingAttr, binder, args, culture, activationAttributes);
			if (obj == null)
			{
				return null;
			}
			return new ObjectHandle(obj);
		}

		/// <summary>Creates an instance of the type whose name is specified, using the named assembly file and the constructor that best matches the specified parameters.</summary>
		/// <param name="assemblyFile">The name of a file that contains an assembly where the type named <paramref name="typeName" /> is sought.</param>
		/// <param name="typeName">The name of the preferred type.</param>
		/// <param name="ignoreCase">
		///   <see langword="true" /> to specify that the search for <paramref name="typeName" /> is not case-sensitive; <see langword="false" /> to specify that the search is case-sensitive.</param>
		/// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="typeName" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted.</param>
		/// <param name="binder">An object that uses <paramref name="bindingAttr" /> and <paramref name="args" /> to seek and identify the <paramref name="typeName" /> constructor. If <paramref name="binder" /> is <see langword="null" />, the default binder is used.</param>
		/// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or <see langword="null" />, the constructor that takes no parameters (the default constructor) is invoked.</param>
		/// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="typeName" /> constructor. If <paramref name="culture" /> is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.</param>
		/// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.  
		///  This parameter is related to client-activated objects. Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
		/// <param name="securityInfo">Information used to make security policy decisions and grant code permissions.</param>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.MissingMethodException">No matching constructor was found.</exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyFile" />.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyFile" /> was not found.</exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required <see cref="T:System.Security.Permissions.FileIOPermission" />.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />.</exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyFile" /> is not a valid assembly.  
		/// -or-  
		/// The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		// Token: 0x060014F5 RID: 5365 RVA: 0x00052740 File Offset: 0x00050940
		[Obsolete("Methods which use evidence to sandbox are obsolete and will be removed in a future release of the .NET Framework. Please use an overload of CreateInstanceFrom which does not take an Evidence parameter. See http://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
		public static ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityInfo)
		{
			return Activator.CreateInstanceFromInternal(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityInfo);
		}

		/// <summary>Creates an instance of the type whose name is specified, using the named assembly file and the constructor that best matches the specified parameters.</summary>
		/// <param name="assemblyFile">The name of a file that contains an assembly where the type named <paramref name="typeName" /> is sought.</param>
		/// <param name="typeName">The name of the preferred type.</param>
		/// <param name="ignoreCase">
		///   <see langword="true" /> to specify that the search for <paramref name="typeName" /> is not case-sensitive; <see langword="false" /> to specify that the search is case-sensitive.</param>
		/// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="typeName" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted.</param>
		/// <param name="binder">An object that uses <paramref name="bindingAttr" /> and <paramref name="args" /> to seek and identify the <paramref name="typeName" /> constructor. If <paramref name="binder" /> is <see langword="null" />, the default binder is used.</param>
		/// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or <see langword="null" />, the constructor that takes no parameters (the default constructor) is invoked.</param>
		/// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="typeName" /> constructor. If <paramref name="culture" /> is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.</param>
		/// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.  
		///  This parameter is related to client-activated objects. Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.MissingMethodException">No matching constructor was found.</exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyFile" />.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyFile" /> was not found.</exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required <see cref="T:System.Security.Permissions.FileIOPermission" />.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />.</exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyFile" /> is not a valid assembly.  
		/// -or-  
		/// The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		// Token: 0x060014F6 RID: 5366 RVA: 0x00052760 File Offset: 0x00050960
		public static ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
		{
			return Activator.CreateInstanceFromInternal(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, null);
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x00052780 File Offset: 0x00050980
		private static ObjectHandle CreateInstanceFromInternal(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityInfo)
		{
			object obj = Activator.CreateInstance(Assembly.LoadFrom(assemblyFile, securityInfo).GetType(typeName, true, ignoreCase), bindingAttr, binder, args, culture, activationAttributes);
			if (obj == null)
			{
				return null;
			}
			return new ObjectHandle(obj);
		}

		/// <summary>Creates an instance of the type whose name is specified in the specified remote domain, using the named assembly and default constructor.</summary>
		/// <param name="domain">The remote domain where the type named <paramref name="typeName" /> is created.</param>
		/// <param name="assemblyName">The name of the assembly where the type named <paramref name="typeName" /> is sought. If <paramref name="assemblyName" /> is <see langword="null" />, the executing assembly is searched.</param>
		/// <param name="typeName">The fully qualified name of the preferred type.</param>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeName" /> or <paramref name="domain" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyName" />.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyName" /> was not found.</exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract type.  
		///  -or-  
		///  This member was invoked with a late-binding mechanism.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />.</exception>
		/// <exception cref="T:System.NotSupportedException">Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.</exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyName" /> is not a valid assembly.  
		/// -or-  
		/// The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		/// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.  
		///  -or-  
		///  The assembly name or code base is invalid.</exception>
		// Token: 0x060014F8 RID: 5368 RVA: 0x000527B7 File Offset: 0x000509B7
		[SecurityCritical]
		public static ObjectHandle CreateInstance(AppDomain domain, string assemblyName, string typeName)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			return domain.InternalCreateInstanceWithNoSecurity(assemblyName, typeName);
		}

		/// <summary>Creates an instance of the type whose name is specified in the specified remote domain, using the named assembly and the constructor that best matches the specified parameters.</summary>
		/// <param name="domain">The domain where the type named <paramref name="typeName" /> is created.</param>
		/// <param name="assemblyName">The name of the assembly where the type named <paramref name="typeName" /> is sought. If <paramref name="assemblyName" /> is <see langword="null" />, the executing assembly is searched.</param>
		/// <param name="typeName">The fully qualified name of the preferred type.</param>
		/// <param name="ignoreCase">
		///   <see langword="true" /> to specify that the search for <paramref name="typeName" /> is not case-sensitive; <see langword="false" /> to specify that the search is case-sensitive.</param>
		/// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="typeName" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted.</param>
		/// <param name="binder">An object that uses <paramref name="bindingAttr" /> and <paramref name="args" /> to seek and identify the <paramref name="typeName" /> constructor. If <paramref name="binder" /> is <see langword="null" />, the default binder is used.</param>
		/// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or <see langword="null" />, the constructor that takes no parameters (the default constructor) is invoked.</param>
		/// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="typeName" /> constructor. If <paramref name="culture" /> is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.</param>
		/// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object. The <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> specifies the URL that is required to activate a remote object.</param>
		/// <param name="securityAttributes">Information used to make security policy decisions and grant code permissions.</param>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="domain" /> or <paramref name="typeName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.MissingMethodException">No matching constructor was found.</exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyName" />.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyName" /> was not found.</exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />.</exception>
		/// <exception cref="T:System.NotSupportedException">Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.  
		///  -or-  
		///  <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />.  
		///  -or-  
		///  The constructor that best matches <paramref name="args" /> has <see langword="varargs" /> arguments.</exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyName" /> is not a valid assembly.  
		/// -or-  
		/// The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		/// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.  
		///  -or-  
		///  The assembly name or code base is invalid.</exception>
		// Token: 0x060014F9 RID: 5369 RVA: 0x000527D0 File Offset: 0x000509D0
		[SecurityCritical]
		[Obsolete("Methods which use evidence to sandbox are obsolete and will be removed in a future release of the .NET Framework. Please use an overload of CreateInstance which does not take an Evidence parameter. See http://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
		public static ObjectHandle CreateInstance(AppDomain domain, string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			return domain.InternalCreateInstanceWithNoSecurity(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
		}

		/// <summary>Creates an instance of the type whose name is specified in the specified remote domain, using the named assembly and the constructor that best matches the specified parameters.</summary>
		/// <param name="domain">The domain where the type named <paramref name="typeName" /> is created.</param>
		/// <param name="assemblyName">The name of the assembly where the type named <paramref name="typeName" /> is sought. If <paramref name="assemblyName" /> is <see langword="null" />, the executing assembly is searched.</param>
		/// <param name="typeName">The fully qualified name of the preferred type.</param>
		/// <param name="ignoreCase">
		///   <see langword="true" /> to specify that the search for <paramref name="typeName" /> is not case-sensitive; <see langword="false" /> to specify that the search is case-sensitive.</param>
		/// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="typeName" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted.</param>
		/// <param name="binder">An object that uses <paramref name="bindingAttr" /> and <paramref name="args" /> to seek and identify the <paramref name="typeName" /> constructor. If <paramref name="binder" /> is <see langword="null" />, the default binder is used.</param>
		/// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or <see langword="null" />, the constructor that takes no parameters (the default constructor) is invoked.</param>
		/// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="typeName" /> constructor. If <paramref name="culture" /> is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.</param>
		/// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.  
		///  This parameter is related to client-activated objects. Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="domain" /> or <paramref name="typeName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.MissingMethodException">No matching constructor was found.</exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyName" />.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyName" /> was not found.</exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />.</exception>
		/// <exception cref="T:System.NotSupportedException">Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.  
		///  -or-  
		///  <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />.  
		///  -or-  
		///  The constructor that best matches <paramref name="args" /> has <see langword="varargs" /> arguments.</exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyName" /> is not a valid assembly.  
		/// -or-  
		/// The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		/// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.  
		///  -or-  
		///  The assembly name or code base is invalid.</exception>
		// Token: 0x060014FA RID: 5370 RVA: 0x00052800 File Offset: 0x00050A00
		[SecurityCritical]
		public static ObjectHandle CreateInstance(AppDomain domain, string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			return domain.InternalCreateInstanceWithNoSecurity(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, null);
		}

		/// <summary>Creates an instance of the type whose name is specified in the specified remote domain, using the named assembly file and default constructor.</summary>
		/// <param name="domain">The remote domain where the type named <paramref name="typeName" /> is created.</param>
		/// <param name="assemblyFile">The name of a file that contains an assembly where the type named <paramref name="typeName" /> is sought.</param>
		/// <param name="typeName">The name of the preferred type.</param>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="domain" /> or <paramref name="typeName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyFile" />.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyFile" /> was not found.</exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does have the required <see cref="T:System.Security.Permissions.FileIOPermission" />.</exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyFile" /> is not a valid assembly.  
		/// -or-  
		/// The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		// Token: 0x060014FB RID: 5371 RVA: 0x0005282F File Offset: 0x00050A2F
		[SecurityCritical]
		public static ObjectHandle CreateInstanceFrom(AppDomain domain, string assemblyFile, string typeName)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			return domain.InternalCreateInstanceFromWithNoSecurity(assemblyFile, typeName);
		}

		/// <summary>Creates an instance of the type whose name is specified in the specified remote domain, using the named assembly file and the constructor that best matches the specified parameters.</summary>
		/// <param name="domain">The remote domain where the type named <paramref name="typeName" /> is created.</param>
		/// <param name="assemblyFile">The name of a file that contains an assembly where the type named <paramref name="typeName" /> is sought.</param>
		/// <param name="typeName">The name of the preferred type.</param>
		/// <param name="ignoreCase">
		///   <see langword="true" /> to specify that the search for <paramref name="typeName" /> is not case-sensitive; <see langword="false" /> to specify that the search is case-sensitive.</param>
		/// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="typeName" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted.</param>
		/// <param name="binder">An object that uses <paramref name="bindingAttr" /> and <paramref name="args" /> to seek and identify the <paramref name="typeName" /> constructor. If <paramref name="binder" /> is <see langword="null" />, the default binder is used.</param>
		/// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or <see langword="null" />, the constructor that takes no parameters (the default constructor) is invoked.</param>
		/// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="typeName" /> constructor. If <paramref name="culture" /> is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.</param>
		/// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.  
		///  This parameter is related to client-activated objects. Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
		/// <param name="securityAttributes">Information used to make security policy decisions and grant code permissions.</param>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="domain" /> or <paramref name="typeName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.MissingMethodException">No matching constructor was found.</exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyFile" />.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyFile" /> was not found.</exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does have the required <see cref="T:System.Security.Permissions.FileIOPermission" />.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />.</exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyFile" /> is not a valid assembly.  
		/// -or-  
		/// The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
		// Token: 0x060014FC RID: 5372 RVA: 0x00052848 File Offset: 0x00050A48
		[Obsolete("Methods which use Evidence to sandbox are obsolete and will be removed in a future release of the .NET Framework. Please use an overload of CreateInstanceFrom which does not take an Evidence parameter. See http://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
		[SecurityCritical]
		public static ObjectHandle CreateInstanceFrom(AppDomain domain, string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			return domain.InternalCreateInstanceFromWithNoSecurity(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
		}

		/// <summary>Creates an instance of the type whose name is specified in the specified remote domain, using the named assembly file and the constructor that best matches the specified parameters.</summary>
		/// <param name="domain">The remote domain where the type named <paramref name="typeName" /> is created.</param>
		/// <param name="assemblyFile">The name of a file that contains an assembly where the type named <paramref name="typeName" /> is sought.</param>
		/// <param name="typeName">The name of the preferred type.</param>
		/// <param name="ignoreCase">
		///   <see langword="true" /> to specify that the search for <paramref name="typeName" /> is not case-sensitive; <see langword="false" /> to specify that the search is case-sensitive.</param>
		/// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="typeName" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted.</param>
		/// <param name="binder">An object that uses <paramref name="bindingAttr" /> and <paramref name="args" /> to seek and identify the <paramref name="typeName" /> constructor. If <paramref name="binder" /> is <see langword="null" />, the default binder is used.</param>
		/// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or <see langword="null" />, the constructor that takes no parameters (the default constructor) is invoked.</param>
		/// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="typeName" /> constructor. If <paramref name="culture" /> is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.</param>
		/// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.  
		///  This parameter is related to client-activated objects. Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="domain" /> or <paramref name="typeName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.MissingMethodException">No matching constructor was found.</exception>
		/// <exception cref="T:System.TypeLoadException">
		///   <paramref name="typename" /> was not found in <paramref name="assemblyFile" />.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyFile" /> was not found.</exception>
		/// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does have the required <see cref="T:System.Security.Permissions.FileIOPermission" />.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />.</exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyFile" /> is not a valid assembly.  
		/// -or-  
		/// <paramref name="assemblyName" /> was compiled for a version of the common language runtime that is later than the version that is currently loaded.</exception>
		// Token: 0x060014FD RID: 5373 RVA: 0x00052878 File Offset: 0x00050A78
		[SecurityCritical]
		public static ObjectHandle CreateInstanceFrom(AppDomain domain, string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			return domain.InternalCreateInstanceFromWithNoSecurity(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, null);
		}

		/// <summary>Creates an instance of the COM object whose name is specified, using the named assembly file and the default constructor.</summary>
		/// <param name="assemblyName">The name of a file that contains an assembly where the type named <paramref name="typeName" /> is sought.</param>
		/// <param name="typeName">The name of the preferred type.</param>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeName" /> or <paramref name="assemblyName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.TypeLoadException">An instance cannot be created through COM.  
		///  -or-  
		///  <paramref name="typename" /> was not found in <paramref name="assemblyName" />.</exception>
		/// <exception cref="T:System.MissingMethodException">No matching constructor was found.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyName" /> is not found, or the module you are trying to load does not specify a file name extension.</exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class.  
		///  -or-  
		///  This member was invoked with a late-binding mechanism.</exception>
		/// <exception cref="T:System.NotSupportedException">The caller cannot provide activation attributes for an object that does not inherit from <see cref="T:System.MarshalByRefObject" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="assemblyName" /> is the empty string ("").</exception>
		// Token: 0x060014FE RID: 5374 RVA: 0x000528A7 File Offset: 0x00050AA7
		public static ObjectHandle CreateComInstanceFrom(string assemblyName, string typeName)
		{
			return Activator.CreateComInstanceFrom(assemblyName, typeName, null, AssemblyHashAlgorithm.None);
		}

		/// <summary>Creates an instance of the COM object whose name is specified, using the named assembly file and the default constructor.</summary>
		/// <param name="assemblyName">The name of a file that contains an assembly where the type named <paramref name="typeName" /> is sought.</param>
		/// <param name="typeName">The name of the preferred type.</param>
		/// <param name="hashValue">The value of the computed hash code.</param>
		/// <param name="hashAlgorithm">The hash algorithm used for hashing files and generating the strong name.</param>
		/// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeName" /> or <paramref name="assemblyName" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="assemblyName" /> is the empty string ("").</exception>
		/// <exception cref="T:System.IO.PathTooLongException">An assembly or module was loaded twice with two different evidences.
		///  -or- 
		///  <paramref name="assemblyName" /> is longer than the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///   <paramref name="assemblyName" /> is not found, or the module you are trying to load does not specify a file name extension.</exception>
		/// <exception cref="T:System.IO.FileLoadException">
		///   <paramref name="assemblyName" /> is found but cannot be loaded.</exception>
		/// <exception cref="T:System.BadImageFormatException">
		///   <paramref name="assemblyName" /> is not a valid assembly.</exception>
		/// <exception cref="T:System.Security.SecurityException">A code base that does not start with "file://" was specified without the required <see langword="WebPermission" />.</exception>
		/// <exception cref="T:System.TypeLoadException">An instance cannot be created through COM.  
		///  -or-  
		///  <paramref name="typename" /> was not found in <paramref name="assemblyName" />.</exception>
		/// <exception cref="T:System.MissingMethodException">No matching constructor was found.</exception>
		/// <exception cref="T:System.MemberAccessException">An instance of an abstract class cannot be created.  
		///  -or-  
		///  This member was invoked with a late-binding mechanism.</exception>
		/// <exception cref="T:System.NotSupportedException">The caller cannot provide activation attributes for an object that does not inherit from <see cref="T:System.MarshalByRefObject" />.</exception>
		// Token: 0x060014FF RID: 5375 RVA: 0x000528B4 File Offset: 0x00050AB4
		public static ObjectHandle CreateComInstanceFrom(string assemblyName, string typeName, byte[] hashValue, AssemblyHashAlgorithm hashAlgorithm)
		{
			Assembly assembly = Assembly.LoadFrom(assemblyName, hashValue, hashAlgorithm);
			Type type = assembly.GetType(typeName, true, false);
			object[] customAttributes = type.GetCustomAttributes(typeof(ComVisibleAttribute), false);
			if (customAttributes.Length != 0 && !((ComVisibleAttribute)customAttributes[0]).Value)
			{
				throw new TypeLoadException(Environment.GetResourceString("The specified type must be visible from COM."));
			}
			if (assembly == null)
			{
				return null;
			}
			object obj = Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null, null);
			if (obj == null)
			{
				return null;
			}
			return new ObjectHandle(obj);
		}

		/// <summary>Creates a proxy for the well-known object indicated by the specified type and URL.</summary>
		/// <param name="type">The type of the well-known object to which you want to connect.</param>
		/// <param name="url">The URL of the well-known object.</param>
		/// <returns>A proxy that points to an endpoint served by the requested well-known object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> or <paramref name="url" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">
		///   <paramref name="type" /> is not marshaled by reference and is not an interface.</exception>
		/// <exception cref="T:System.MemberAccessException">This member was invoked with a late-binding mechanism.</exception>
		// Token: 0x06001500 RID: 5376 RVA: 0x0005292D File Offset: 0x00050B2D
		[SecurityCritical]
		public static object GetObject(Type type, string url)
		{
			return Activator.GetObject(type, url, null);
		}

		/// <summary>Creates a proxy for the well-known object indicated by the specified type, URL, and channel data.</summary>
		/// <param name="type">The type of the well-known object to which you want to connect.</param>
		/// <param name="url">The URL of the well-known object.</param>
		/// <param name="state">Channel-specific data or <see langword="null" />.</param>
		/// <returns>A proxy that points to an endpoint served by the requested well-known object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> or <paramref name="url" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">
		///   <paramref name="type" /> is not marshaled by reference and is not an interface.</exception>
		/// <exception cref="T:System.MemberAccessException">This member was invoked with a late-binding mechanism.</exception>
		// Token: 0x06001501 RID: 5377 RVA: 0x00052937 File Offset: 0x00050B37
		[SecurityCritical]
		public static object GetObject(Type type, string url, object state)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return RemotingServices.Connect(type, url, state);
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x00052955 File Offset: 0x00050B55
		[Conditional("_DEBUG")]
		private static void Log(bool test, string title, string success, string failure)
		{
		}

		/// <summary>Retrieves the number of type information interfaces that an object provides (either 0 or 1).</summary>
		/// <param name="pcTInfo">When this method returns, contains a pointer to a location that receives the number of type information interfaces provided by the object. This parameter is passed uninitialized.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		// Token: 0x06001503 RID: 5379 RVA: 0x000479FC File Offset: 0x00045BFC
		void _Activator.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the type information for an object, which can then be used to get the type information for an interface.</summary>
		/// <param name="iTInfo">The type information to return.</param>
		/// <param name="lcid">The locale identifier for the type information.</param>
		/// <param name="ppTInfo">An object that receives a pointer to the requested type information object.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		// Token: 0x06001504 RID: 5380 RVA: 0x000479FC File Offset: 0x00045BFC
		void _Activator.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>Maps a set of names to a corresponding set of dispatch identifiers.</summary>
		/// <param name="riid">Reserved for future use. Must be IID_NULL.</param>
		/// <param name="rgszNames">The passed-in array of names to map.</param>
		/// <param name="cNames">The count of the names to map.</param>
		/// <param name="lcid">The locale context in which to interpret the names.</param>
		/// <param name="rgDispId">The caller-allocated array that receives the IDs corresponding to the names.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		// Token: 0x06001505 RID: 5381 RVA: 0x000479FC File Offset: 0x00045BFC
		void _Activator.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		/// <summary>Provides access to properties and methods exposed by an object.</summary>
		/// <param name="dispIdMember">A dispatch identifier that identifies the member.</param>
		/// <param name="riid">Reserved for future use. Must be IID_NULL.</param>
		/// <param name="lcid">The locale context in which to interpret arguments.</param>
		/// <param name="wFlags">Flags describing the context of the call.</param>
		/// <param name="pDispParams">A pointer to a structure that contains an array of arguments, an array of argument DISPIDs for named arguments, and counts for the number of elements in the arrays.</param>
		/// <param name="pVarResult">A pointer to the location where the result is to be stored.</param>
		/// <param name="pExcepInfo">A pointer to a structure that contains exception information.</param>
		/// <param name="puArgErr">The index of the first argument that has an error.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		// Token: 0x06001506 RID: 5382 RVA: 0x000479FC File Offset: 0x00045BFC
		void _Activator.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		/// <summary>Creates an instance of the type designated by the specified <see cref="T:System.ActivationContext" /> object.</summary>
		/// <param name="activationContext">An activation context object that specifies the object to create.</param>
		/// <returns>A handle that must be unwrapped to access the newly created object.</returns>
		// Token: 0x06001507 RID: 5383 RVA: 0x00052959 File Offset: 0x00050B59
		[SecuritySafeCritical]
		public static ObjectHandle CreateInstance(ActivationContext activationContext)
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}

		/// <summary>Creates an instance of the type that is designated by the specified <see cref="T:System.ActivationContext" /> object and activated with the specified custom activation data.</summary>
		/// <param name="activationContext">An activation context object that specifies the object to create.</param>
		/// <param name="activationCustomData">An array of Unicode strings that contain custom activation data.</param>
		/// <returns>A handle that must be unwrapped to access the newly created object.</returns>
		// Token: 0x06001508 RID: 5384 RVA: 0x00052959 File Offset: 0x00050B59
		[SecuritySafeCritical]
		public static ObjectHandle CreateInstance(ActivationContext activationContext, string[] activationCustomData)
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}

		// Token: 0x040014F4 RID: 5364
		internal const int LookupMask = 255;

		// Token: 0x040014F5 RID: 5365
		internal const BindingFlags ConLookup = BindingFlags.Instance | BindingFlags.Public;

		// Token: 0x040014F6 RID: 5366
		internal const BindingFlags ConstructorDefault = BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance;
	}
}
