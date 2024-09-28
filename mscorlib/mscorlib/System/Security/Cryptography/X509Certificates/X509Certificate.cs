using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using Internal.Cryptography;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Provides methods that help you use X.509 v.3 certificates.</summary>
	// Token: 0x020004D9 RID: 1241
	[Serializable]
	public class X509Certificate : IDisposable, IDeserializationCallback, ISerializable
	{
		/// <summary>Resets the state of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> object.</summary>
		// Token: 0x0600316C RID: 12652 RVA: 0x000B6CB8 File Offset: 0x000B4EB8
		public virtual void Reset()
		{
			if (this.impl != null)
			{
				this.impl.Dispose();
				this.impl = null;
			}
			this.lazyCertHash = null;
			this.lazyIssuer = null;
			this.lazySubject = null;
			this.lazySerialNumber = null;
			this.lazyKeyAlgorithm = null;
			this.lazyKeyAlgorithmParameters = null;
			this.lazyPublicKey = null;
			this.lazyNotBefore = DateTime.MinValue;
			this.lazyNotAfter = DateTime.MinValue;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class.</summary>
		// Token: 0x0600316D RID: 12653 RVA: 0x000B6D34 File Offset: 0x000B4F34
		public X509Certificate()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class defined from a sequence of bytes representing an X.509v3 certificate.</summary>
		/// <param name="data">A byte array containing data from an X.509 certificate.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:  
		///
		/// The certificate file does not exist.  
		///
		/// The certificate is invalid.  
		///
		/// The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="rawData" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The length of the <paramref name="rawData" /> parameter is 0.</exception>
		// Token: 0x0600316E RID: 12654 RVA: 0x000B6D52 File Offset: 0x000B4F52
		public X509Certificate(byte[] data)
		{
			if (data != null && data.Length != 0)
			{
				this.impl = X509Helper.Import(data);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using a byte array and a password.</summary>
		/// <param name="rawData">A byte array containing data from an X.509 certificate.</param>
		/// <param name="password">The password required to access the X.509 certificate data.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:  
		///
		/// The certificate file does not exist.  
		///
		/// The certificate is invalid.  
		///
		/// The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="rawData" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The length of the <paramref name="rawData" /> parameter is 0.</exception>
		// Token: 0x0600316F RID: 12655 RVA: 0x000B6D83 File Offset: 0x000B4F83
		public X509Certificate(byte[] rawData, string password) : this(rawData, password, X509KeyStorageFlags.DefaultKeySet)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using a byte array and a password.</summary>
		/// <param name="rawData">A byte array that contains data from an X.509 certificate.</param>
		/// <param name="password">The password required to access the X.509 certificate data.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:  
		///
		/// The certificate file does not exist.  
		///
		/// The certificate is invalid.  
		///
		/// The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="rawData" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The length of the <paramref name="rawData" /> parameter is 0.</exception>
		// Token: 0x06003170 RID: 12656 RVA: 0x000B6D8E File Offset: 0x000B4F8E
		[CLSCompliant(false)]
		public X509Certificate(byte[] rawData, SecureString password) : this(rawData, password, X509KeyStorageFlags.DefaultKeySet)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using a byte array, a password, and a key storage flag.</summary>
		/// <param name="rawData">A byte array containing data from an X.509 certificate.</param>
		/// <param name="password">The password required to access the X.509 certificate data.</param>
		/// <param name="keyStorageFlags">A bitwise combination of the enumeration values that control where and how to import the certificate.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:  
		///
		/// The certificate file does not exist.  
		///
		/// The certificate is invalid.  
		///
		/// The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="rawData" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The length of the <paramref name="rawData" /> parameter is 0.</exception>
		// Token: 0x06003171 RID: 12657 RVA: 0x000B6D9C File Offset: 0x000B4F9C
		public X509Certificate(byte[] rawData, string password, X509KeyStorageFlags keyStorageFlags)
		{
			if (rawData == null || rawData.Length == 0)
			{
				throw new ArgumentException("Array cannot be empty or null.", "rawData");
			}
			X509Certificate.ValidateKeyStorageFlags(keyStorageFlags);
			using (SafePasswordHandle safePasswordHandle = new SafePasswordHandle(password))
			{
				this.impl = X509Helper.Import(rawData, safePasswordHandle, keyStorageFlags);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using a byte array, a password, and a key storage flag.</summary>
		/// <param name="rawData">A byte array that contains data from an X.509 certificate.</param>
		/// <param name="password">The password required to access the X.509 certificate data.</param>
		/// <param name="keyStorageFlags">A bitwise combination of the enumeration values that control where and how to import the certificate.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:  
		///
		/// The certificate file does not exist.  
		///
		/// The certificate is invalid.  
		///
		/// The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="rawData" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The length of the <paramref name="rawData" /> parameter is 0.</exception>
		// Token: 0x06003172 RID: 12658 RVA: 0x000B6E14 File Offset: 0x000B5014
		[CLSCompliant(false)]
		public X509Certificate(byte[] rawData, SecureString password, X509KeyStorageFlags keyStorageFlags)
		{
			if (rawData == null || rawData.Length == 0)
			{
				throw new ArgumentException("Array cannot be empty or null.", "rawData");
			}
			X509Certificate.ValidateKeyStorageFlags(keyStorageFlags);
			using (SafePasswordHandle safePasswordHandle = new SafePasswordHandle(password))
			{
				this.impl = X509Helper.Import(rawData, safePasswordHandle, keyStorageFlags);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using a handle to an unmanaged <see langword="PCCERT_CONTEXT" /> structure.</summary>
		/// <param name="handle">A handle to an unmanaged <see langword="PCCERT_CONTEXT" /> structure.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:  
		///
		/// The certificate file does not exist.  
		///
		/// The certificate is invalid.  
		///
		/// The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentException">The handle parameter does not represent a valid <see langword="PCCERT_CONTEXT" /> structure.</exception>
		// Token: 0x06003173 RID: 12659 RVA: 0x000B6E8C File Offset: 0x000B508C
		public X509Certificate(IntPtr handle)
		{
			throw new PlatformNotSupportedException("Initializing `X509Certificate` from native handle is not supported.");
		}

		// Token: 0x06003174 RID: 12660 RVA: 0x000B6EB4 File Offset: 0x000B50B4
		internal X509Certificate(X509CertificateImpl impl)
		{
			this.impl = X509Helper.InitFromCertificate(impl);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using the name of a PKCS7 signed file.</summary>
		/// <param name="fileName">The name of a PKCS7 signed file.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:  
		///
		/// The certificate file does not exist.  
		///
		/// The certificate is invalid.  
		///
		/// The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003175 RID: 12661 RVA: 0x000B6EDE File Offset: 0x000B50DE
		public X509Certificate(string fileName) : this(fileName, null, X509KeyStorageFlags.DefaultKeySet)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using the name of a PKCS7 signed file and a password to access the certificate.</summary>
		/// <param name="fileName">The name of a PKCS7 signed file.</param>
		/// <param name="password">The password required to access the X.509 certificate data.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:  
		///
		/// The certificate file does not exist.  
		///
		/// The certificate is invalid.  
		///
		/// The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003176 RID: 12662 RVA: 0x000B6EE9 File Offset: 0x000B50E9
		public X509Certificate(string fileName, string password) : this(fileName, password, X509KeyStorageFlags.DefaultKeySet)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using a certificate file name and a password.</summary>
		/// <param name="fileName">The name of a certificate file.</param>
		/// <param name="password">The password required to access the X.509 certificate data.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:  
		///
		/// The certificate file does not exist.  
		///
		/// The certificate is invalid.  
		///
		/// The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003177 RID: 12663 RVA: 0x000B6EF4 File Offset: 0x000B50F4
		[CLSCompliant(false)]
		public X509Certificate(string fileName, SecureString password) : this(fileName, password, X509KeyStorageFlags.DefaultKeySet)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using the name of a PKCS7 signed file, a password to access the certificate, and a key storage flag.</summary>
		/// <param name="fileName">The name of a PKCS7 signed file.</param>
		/// <param name="password">The password required to access the X.509 certificate data.</param>
		/// <param name="keyStorageFlags">A bitwise combination of the enumeration values that control where and how to import the certificate.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:  
		///
		/// The certificate file does not exist.  
		///
		/// The certificate is invalid.  
		///
		/// The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003178 RID: 12664 RVA: 0x000B6F00 File Offset: 0x000B5100
		public X509Certificate(string fileName, string password, X509KeyStorageFlags keyStorageFlags)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			X509Certificate.ValidateKeyStorageFlags(keyStorageFlags);
			byte[] rawData = File.ReadAllBytes(fileName);
			using (SafePasswordHandle safePasswordHandle = new SafePasswordHandle(password))
			{
				this.impl = X509Helper.Import(rawData, safePasswordHandle, keyStorageFlags);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using a certificate file name, a password, and a key storage flag.</summary>
		/// <param name="fileName">The name of a certificate file.</param>
		/// <param name="password">The password required to access the X.509 certificate data.</param>
		/// <param name="keyStorageFlags">A bitwise combination of the enumeration values that control where and how to import the certificate.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:  
		///
		/// The certificate file does not exist.  
		///
		/// The certificate is invalid.  
		///
		/// The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003179 RID: 12665 RVA: 0x000B6F78 File Offset: 0x000B5178
		[CLSCompliant(false)]
		public X509Certificate(string fileName, SecureString password, X509KeyStorageFlags keyStorageFlags) : this()
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			X509Certificate.ValidateKeyStorageFlags(keyStorageFlags);
			byte[] rawData = File.ReadAllBytes(fileName);
			using (SafePasswordHandle safePasswordHandle = new SafePasswordHandle(password))
			{
				this.impl = X509Helper.Import(rawData, safePasswordHandle, keyStorageFlags);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using another <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class.</summary>
		/// <param name="cert">A <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class from which to initialize this class.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:  
		///
		/// The certificate file does not exist.  
		///
		/// The certificate is invalid.  
		///
		/// The certificate's password is incorrect.</exception>
		/// <exception cref="T:System.ArgumentNullException">The value of the <paramref name="cert" /> parameter is <see langword="null" />.</exception>
		// Token: 0x0600317A RID: 12666 RVA: 0x000B6FD8 File Offset: 0x000B51D8
		public X509Certificate(X509Certificate cert)
		{
			if (cert == null)
			{
				throw new ArgumentNullException("cert");
			}
			this.impl = X509Helper.InitFromCertificate(cert);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class using a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object and a <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that describes serialization information.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure that describes how serialization should be performed.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:  
		///
		/// The certificate file does not exist.  
		///
		/// The certificate is invalid.  
		///
		/// The certificate's password is incorrect.</exception>
		// Token: 0x0600317B RID: 12667 RVA: 0x000B7010 File Offset: 0x000B5210
		public X509Certificate(SerializationInfo info, StreamingContext context) : this()
		{
			throw new PlatformNotSupportedException();
		}

		/// <summary>Creates an X.509v3 certificate from the specified PKCS7 signed file.</summary>
		/// <param name="filename">The path of the PKCS7 signed file from which to create the X.509 certificate.</param>
		/// <returns>The newly created X.509 certificate.</returns>
		/// <exception cref="T:System.ArgumentException">The <paramref name="filename" /> parameter is <see langword="null" />.</exception>
		// Token: 0x0600317C RID: 12668 RVA: 0x000B701D File Offset: 0x000B521D
		public static X509Certificate CreateFromCertFile(string filename)
		{
			return new X509Certificate(filename);
		}

		/// <summary>Creates an X.509v3 certificate from the specified signed file.</summary>
		/// <param name="filename">The path of the signed file from which to create the X.509 certificate.</param>
		/// <returns>The newly created X.509 certificate.</returns>
		// Token: 0x0600317D RID: 12669 RVA: 0x000B701D File Offset: 0x000B521D
		public static X509Certificate CreateFromSignedFile(string filename)
		{
			return new X509Certificate(filename);
		}

		/// <summary>Gets serialization information with all the data needed to recreate an instance of the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object.</summary>
		/// <param name="info">The object to populate with serialization information.</param>
		/// <param name="context">The destination context of the serialization.</param>
		// Token: 0x0600317E RID: 12670 RVA: 0x0001B98F File Offset: 0x00019B8F
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new PlatformNotSupportedException();
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and is called back by the deserialization event when deserialization is complete.</summary>
		/// <param name="sender">The source of the deserialization event.</param>
		// Token: 0x0600317F RID: 12671 RVA: 0x0001B98F File Offset: 0x00019B8F
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			throw new PlatformNotSupportedException();
		}

		/// <summary>Gets a handle to a Microsoft Cryptographic API certificate context described by an unmanaged <see langword="PCCERT_CONTEXT" /> structure.</summary>
		/// <returns>An <see cref="T:System.IntPtr" /> structure that represents an unmanaged <see langword="PCCERT_CONTEXT" /> structure.</returns>
		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06003180 RID: 12672 RVA: 0x000B7025 File Offset: 0x000B5225
		public IntPtr Handle
		{
			get
			{
				if (X509Helper.IsValid(this.impl))
				{
					return this.impl.Handle;
				}
				return IntPtr.Zero;
			}
		}

		/// <summary>Gets the name of the certificate authority that issued the X.509v3 certificate.</summary>
		/// <returns>The name of the certificate authority that issued the X.509v3 certificate.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate handle is invalid.</exception>
		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06003181 RID: 12673 RVA: 0x000B7048 File Offset: 0x000B5248
		public string Issuer
		{
			get
			{
				this.ThrowIfInvalid();
				string text = this.lazyIssuer;
				if (text == null)
				{
					text = (this.lazyIssuer = this.Impl.Issuer);
				}
				return text;
			}
		}

		/// <summary>Gets the subject distinguished name from the certificate.</summary>
		/// <returns>The subject distinguished name from the certificate.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate handle is invalid.</exception>
		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06003182 RID: 12674 RVA: 0x000B7080 File Offset: 0x000B5280
		public string Subject
		{
			get
			{
				this.ThrowIfInvalid();
				string text = this.lazySubject;
				if (text == null)
				{
					text = (this.lazySubject = this.Impl.Subject);
				}
				return text;
			}
		}

		/// <summary>Releases all resources used by the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object.</summary>
		// Token: 0x06003183 RID: 12675 RVA: 0x000B70B7 File Offset: 0x000B52B7
		public void Dispose()
		{
			this.Dispose(true);
		}

		/// <summary>Releases all of the unmanaged resources used by this <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x06003184 RID: 12676 RVA: 0x000B70C0 File Offset: 0x000B52C0
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Reset();
			}
		}

		/// <summary>Compares two <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> objects for equality.</summary>
		/// <param name="obj">An <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object to compare to the current object.</param>
		/// <returns>
		///   <see langword="true" /> if the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object is equal to the object specified by the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003185 RID: 12677 RVA: 0x000B70CC File Offset: 0x000B52CC
		public override bool Equals(object obj)
		{
			X509Certificate x509Certificate = obj as X509Certificate;
			return x509Certificate != null && this.Equals(x509Certificate);
		}

		/// <summary>Compares two <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> objects for equality.</summary>
		/// <param name="other">An <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object to compare to the current object.</param>
		/// <returns>
		///   <see langword="true" /> if the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object is equal to the object specified by the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003186 RID: 12678 RVA: 0x000B70EC File Offset: 0x000B52EC
		public virtual bool Equals(X509Certificate other)
		{
			if (other == null)
			{
				return false;
			}
			if (this.Impl == null)
			{
				return other.Impl == null;
			}
			if (!this.Issuer.Equals(other.Issuer))
			{
				return false;
			}
			byte[] rawSerialNumber = this.GetRawSerialNumber();
			byte[] rawSerialNumber2 = other.GetRawSerialNumber();
			if (rawSerialNumber.Length != rawSerialNumber2.Length)
			{
				return false;
			}
			for (int i = 0; i < rawSerialNumber.Length; i++)
			{
				if (rawSerialNumber[i] != rawSerialNumber2[i])
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Exports the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object to a byte array in a format described by one of the <see cref="T:System.Security.Cryptography.X509Certificates.X509ContentType" /> values.</summary>
		/// <param name="contentType">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509ContentType" /> values that describes how to format the output data.</param>
		/// <returns>An array of bytes that represents the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">A value other than <see cref="F:System.Security.Cryptography.X509Certificates.X509ContentType.Cert" />, <see cref="F:System.Security.Cryptography.X509Certificates.X509ContentType.SerializedCert" />, or <see cref="F:System.Security.Cryptography.X509Certificates.X509ContentType.Pkcs12" /> was passed to the <paramref name="contentType" /> parameter.  
		///  -or-  
		///  The certificate could not be exported.</exception>
		// Token: 0x06003187 RID: 12679 RVA: 0x000B7156 File Offset: 0x000B5356
		public virtual byte[] Export(X509ContentType contentType)
		{
			return this.Export(contentType, null);
		}

		/// <summary>Exports the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object to a byte array in a format described by one of the <see cref="T:System.Security.Cryptography.X509Certificates.X509ContentType" /> values, and using the specified password.</summary>
		/// <param name="contentType">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509ContentType" /> values that describes how to format the output data.</param>
		/// <param name="password">The password required to access the X.509 certificate data.</param>
		/// <returns>An array of bytes that represents the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">A value other than <see cref="F:System.Security.Cryptography.X509Certificates.X509ContentType.Cert" />, <see cref="F:System.Security.Cryptography.X509Certificates.X509ContentType.SerializedCert" />, or <see cref="F:System.Security.Cryptography.X509Certificates.X509ContentType.Pkcs12" /> was passed to the <paramref name="contentType" /> parameter.  
		///  -or-  
		///  The certificate could not be exported.</exception>
		// Token: 0x06003188 RID: 12680 RVA: 0x000B7160 File Offset: 0x000B5360
		public virtual byte[] Export(X509ContentType contentType, string password)
		{
			this.VerifyContentType(contentType);
			if (this.Impl == null)
			{
				throw new CryptographicException(-2147467261);
			}
			byte[] result;
			using (SafePasswordHandle safePasswordHandle = new SafePasswordHandle(password))
			{
				result = this.Impl.Export(contentType, safePasswordHandle);
			}
			return result;
		}

		/// <summary>Exports the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object to a byte array using the specified format and a password.</summary>
		/// <param name="contentType">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509ContentType" /> values that describes how to format the output data.</param>
		/// <param name="password">The password required to access the X.509 certificate data.</param>
		/// <returns>A byte array that represents the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">A value other than <see cref="F:System.Security.Cryptography.X509Certificates.X509ContentType.Cert" />, <see cref="F:System.Security.Cryptography.X509Certificates.X509ContentType.SerializedCert" />, or <see cref="F:System.Security.Cryptography.X509Certificates.X509ContentType.Pkcs12" /> was passed to the <paramref name="contentType" /> parameter.  
		///  -or-  
		///  The certificate could not be exported.</exception>
		// Token: 0x06003189 RID: 12681 RVA: 0x000B71BC File Offset: 0x000B53BC
		[CLSCompliant(false)]
		public virtual byte[] Export(X509ContentType contentType, SecureString password)
		{
			this.VerifyContentType(contentType);
			if (this.Impl == null)
			{
				throw new CryptographicException(-2147467261);
			}
			byte[] result;
			using (SafePasswordHandle safePasswordHandle = new SafePasswordHandle(password))
			{
				result = this.Impl.Export(contentType, safePasswordHandle);
			}
			return result;
		}

		/// <summary>Returns the raw data for the entire X.509v3 certificate as a hexadecimal string.</summary>
		/// <returns>The X.509 certificate data as a hexadecimal string.</returns>
		// Token: 0x0600318A RID: 12682 RVA: 0x000B7218 File Offset: 0x000B5418
		public virtual string GetRawCertDataString()
		{
			this.ThrowIfInvalid();
			return this.GetRawCertData().ToHexStringUpper();
		}

		/// <summary>Returns the hash value for the X.509v3 certificate as an array of bytes.</summary>
		/// <returns>The hash value for the X.509 certificate.</returns>
		// Token: 0x0600318B RID: 12683 RVA: 0x000B722B File Offset: 0x000B542B
		public virtual byte[] GetCertHash()
		{
			this.ThrowIfInvalid();
			return this.GetRawCertHash().CloneByteArray();
		}

		/// <summary>Returns the hash value for the X.509v3 certificate that is computed by using the specified cryptographic hash algorithm.</summary>
		/// <param name="hashAlgorithm">The name of the cryptographic hash algorithm to use.</param>
		/// <returns>A byte array that contains the hash value for the X.509 certificate.</returns>
		// Token: 0x0600318C RID: 12684 RVA: 0x0001B98F File Offset: 0x00019B8F
		public virtual byte[] GetCertHash(HashAlgorithmName hashAlgorithm)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x0001B98F File Offset: 0x00019B8F
		public virtual bool TryGetCertHash(HashAlgorithmName hashAlgorithm, Span<byte> destination, out int bytesWritten)
		{
			throw new PlatformNotSupportedException();
		}

		/// <summary>Returns the SHA1 hash value for the X.509v3 certificate as a hexadecimal string.</summary>
		/// <returns>The hexadecimal string representation of the X.509 certificate hash value.</returns>
		// Token: 0x0600318E RID: 12686 RVA: 0x000B723E File Offset: 0x000B543E
		public virtual string GetCertHashString()
		{
			this.ThrowIfInvalid();
			return this.GetRawCertHash().ToHexStringUpper();
		}

		/// <summary>Returns a hexadecimal string containing the hash value for the X.509v3 certificate computed using the specified cryptographic hash algorithm.</summary>
		/// <param name="hashAlgorithm">The name of the cryptographic hash algorithm to use.</param>
		/// <returns>The hexadecimal string representation of the X.509 certificate hash value.</returns>
		// Token: 0x0600318F RID: 12687 RVA: 0x000B7251 File Offset: 0x000B5451
		public virtual string GetCertHashString(HashAlgorithmName hashAlgorithm)
		{
			this.ThrowIfInvalid();
			return this.GetCertHash(hashAlgorithm).ToHexStringUpper();
		}

		// Token: 0x06003190 RID: 12688 RVA: 0x000B7268 File Offset: 0x000B5468
		private byte[] GetRawCertHash()
		{
			byte[] result;
			if ((result = this.lazyCertHash) == null)
			{
				result = (this.lazyCertHash = this.Impl.Thumbprint);
			}
			return result;
		}

		/// <summary>Returns the effective date of this X.509v3 certificate.</summary>
		/// <returns>The effective date for this X.509 certificate.</returns>
		// Token: 0x06003191 RID: 12689 RVA: 0x000B7298 File Offset: 0x000B5498
		public virtual string GetEffectiveDateString()
		{
			return this.GetNotBefore().ToString();
		}

		/// <summary>Returns the expiration date of this X.509v3 certificate.</summary>
		/// <returns>The expiration date for this X.509 certificate.</returns>
		// Token: 0x06003192 RID: 12690 RVA: 0x000B72B4 File Offset: 0x000B54B4
		public virtual string GetExpirationDateString()
		{
			return this.GetNotAfter().ToString();
		}

		/// <summary>Returns the name of the format of this X.509v3 certificate.</summary>
		/// <returns>The format of this X.509 certificate.</returns>
		// Token: 0x06003193 RID: 12691 RVA: 0x000B72CF File Offset: 0x000B54CF
		public virtual string GetFormat()
		{
			return "X509";
		}

		/// <summary>Returns the public key for the X.509v3 certificate as a hexadecimal string.</summary>
		/// <returns>The public key for the X.509 certificate as a hexadecimal string.</returns>
		// Token: 0x06003194 RID: 12692 RVA: 0x000B72D6 File Offset: 0x000B54D6
		public virtual string GetPublicKeyString()
		{
			return this.GetPublicKey().ToHexStringUpper();
		}

		/// <summary>Returns the raw data for the entire X.509v3 certificate as an array of bytes.</summary>
		/// <returns>A byte array containing the X.509 certificate data.</returns>
		// Token: 0x06003195 RID: 12693 RVA: 0x000B72E3 File Offset: 0x000B54E3
		public virtual byte[] GetRawCertData()
		{
			this.ThrowIfInvalid();
			return this.Impl.RawData.CloneByteArray();
		}

		/// <summary>Returns the hash code for the X.509v3 certificate as an integer.</summary>
		/// <returns>The hash code for the X.509 certificate as an integer.</returns>
		// Token: 0x06003196 RID: 12694 RVA: 0x000B72FC File Offset: 0x000B54FC
		public override int GetHashCode()
		{
			if (this.Impl == null)
			{
				return 0;
			}
			byte[] rawCertHash = this.GetRawCertHash();
			int num = 0;
			int num2 = 0;
			while (num2 < rawCertHash.Length && num2 < 4)
			{
				num = (num << 8 | (int)rawCertHash[num2]);
				num2++;
			}
			return num;
		}

		/// <summary>Returns the key algorithm information for this X.509v3 certificate as a string.</summary>
		/// <returns>The key algorithm information for this X.509 certificate as a string.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate context is invalid.</exception>
		// Token: 0x06003197 RID: 12695 RVA: 0x000B7338 File Offset: 0x000B5538
		public virtual string GetKeyAlgorithm()
		{
			this.ThrowIfInvalid();
			string text = this.lazyKeyAlgorithm;
			if (text == null)
			{
				text = (this.lazyKeyAlgorithm = this.Impl.KeyAlgorithm);
			}
			return text;
		}

		/// <summary>Returns the key algorithm parameters for the X.509v3 certificate as an array of bytes.</summary>
		/// <returns>The key algorithm parameters for the X.509 certificate as an array of bytes.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate context is invalid.</exception>
		// Token: 0x06003198 RID: 12696 RVA: 0x000B7370 File Offset: 0x000B5570
		public virtual byte[] GetKeyAlgorithmParameters()
		{
			this.ThrowIfInvalid();
			byte[] array = this.lazyKeyAlgorithmParameters;
			if (array == null)
			{
				array = (this.lazyKeyAlgorithmParameters = this.Impl.KeyAlgorithmParameters);
			}
			return array.CloneByteArray();
		}

		/// <summary>Returns the key algorithm parameters for the X.509v3 certificate as a hexadecimal string.</summary>
		/// <returns>The key algorithm parameters for the X.509 certificate as a hexadecimal string.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate context is invalid.</exception>
		// Token: 0x06003199 RID: 12697 RVA: 0x000B73AC File Offset: 0x000B55AC
		public virtual string GetKeyAlgorithmParametersString()
		{
			this.ThrowIfInvalid();
			return this.GetKeyAlgorithmParameters().ToHexStringUpper();
		}

		/// <summary>Returns the public key for the X.509v3 certificate as an array of bytes.</summary>
		/// <returns>The public key for the X.509 certificate as an array of bytes.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate context is invalid.</exception>
		// Token: 0x0600319A RID: 12698 RVA: 0x000B73C0 File Offset: 0x000B55C0
		public virtual byte[] GetPublicKey()
		{
			this.ThrowIfInvalid();
			byte[] array = this.lazyPublicKey;
			if (array == null)
			{
				array = (this.lazyPublicKey = this.Impl.PublicKeyValue);
			}
			return array.CloneByteArray();
		}

		/// <summary>Returns the serial number of the X.509v3 certificate as an array of bytes in little-endian order.</summary>
		/// <returns>The serial number of the X.509 certificate as an array of bytes in little-endian order.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate context is invalid.</exception>
		// Token: 0x0600319B RID: 12699 RVA: 0x000B73FC File Offset: 0x000B55FC
		public virtual byte[] GetSerialNumber()
		{
			this.ThrowIfInvalid();
			byte[] array = this.GetRawSerialNumber().CloneByteArray();
			Array.Reverse<byte>(array);
			return array;
		}

		/// <summary>Returns the serial number of the X.509v3 certificate as a little-endian hexadecimal string .</summary>
		/// <returns>The serial number of the X.509 certificate as a little-endian hexadecimal string.</returns>
		// Token: 0x0600319C RID: 12700 RVA: 0x000B7415 File Offset: 0x000B5615
		public virtual string GetSerialNumberString()
		{
			this.ThrowIfInvalid();
			return this.GetRawSerialNumber().ToHexStringUpper();
		}

		// Token: 0x0600319D RID: 12701 RVA: 0x000B7428 File Offset: 0x000B5628
		private byte[] GetRawSerialNumber()
		{
			byte[] result;
			if ((result = this.lazySerialNumber) == null)
			{
				result = (this.lazySerialNumber = this.Impl.SerialNumber);
			}
			return result;
		}

		/// <summary>Returns the name of the principal to which the certificate was issued.</summary>
		/// <returns>The name of the principal to which the certificate was issued.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate context is invalid.</exception>
		// Token: 0x0600319E RID: 12702 RVA: 0x000B7457 File Offset: 0x000B5657
		[Obsolete("This method has been deprecated.  Please use the Subject property instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual string GetName()
		{
			this.ThrowIfInvalid();
			return this.Impl.LegacySubject;
		}

		/// <summary>Returns the name of the certification authority that issued the X.509v3 certificate.</summary>
		/// <returns>The name of the certification authority that issued the X.509 certificate.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An error with the certificate occurs. For example:  
		///
		/// The certificate file does not exist.  
		///
		/// The certificate is invalid.  
		///
		/// The certificate's password is incorrect.</exception>
		// Token: 0x0600319F RID: 12703 RVA: 0x000B746A File Offset: 0x000B566A
		[Obsolete("This method has been deprecated.  Please use the Issuer property instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual string GetIssuerName()
		{
			this.ThrowIfInvalid();
			return this.Impl.LegacyIssuer;
		}

		/// <summary>Returns a string representation of the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object.</summary>
		/// <returns>A string representation of the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object.</returns>
		// Token: 0x060031A0 RID: 12704 RVA: 0x000B747D File Offset: 0x000B567D
		public override string ToString()
		{
			return this.ToString(false);
		}

		/// <summary>Returns a string representation of the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object, with extra information, if specified.</summary>
		/// <param name="fVerbose">
		///   <see langword="true" /> to produce the verbose form of the string representation; otherwise, <see langword="false" />.</param>
		/// <returns>A string representation of the current <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object.</returns>
		// Token: 0x060031A1 RID: 12705 RVA: 0x000B7488 File Offset: 0x000B5688
		public virtual string ToString(bool fVerbose)
		{
			if (!fVerbose || !X509Helper.IsValid(this.impl))
			{
				return base.ToString();
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("[Subject]");
			stringBuilder.Append("  ");
			stringBuilder.AppendLine(this.Subject);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("[Issuer]");
			stringBuilder.Append("  ");
			stringBuilder.AppendLine(this.Issuer);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("[Serial Number]");
			stringBuilder.Append("  ");
			byte[] serialNumber = this.GetSerialNumber();
			Array.Reverse<byte>(serialNumber);
			stringBuilder.Append(serialNumber.ToHexArrayUpper());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("[Not Before]");
			stringBuilder.Append("  ");
			stringBuilder.AppendLine(X509Certificate.FormatDate(this.GetNotBefore()));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("[Not After]");
			stringBuilder.Append("  ");
			stringBuilder.AppendLine(X509Certificate.FormatDate(this.GetNotAfter()));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("[Thumbprint]");
			stringBuilder.Append("  ");
			stringBuilder.Append(this.GetRawCertHash().ToHexArrayUpper());
			stringBuilder.AppendLine();
			return stringBuilder.ToString();
		}

		/// <summary>Populates the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object with data from a byte array.</summary>
		/// <param name="rawData">A byte array containing data from an X.509 certificate.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="rawData" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The length of the <paramref name="rawData" /> parameter is 0.</exception>
		// Token: 0x060031A2 RID: 12706 RVA: 0x000B75E1 File Offset: 0x000B57E1
		[ComVisible(false)]
		public virtual void Import(byte[] rawData)
		{
			throw new PlatformNotSupportedException("X509Certificate is immutable on this platform. Use the equivalent constructor instead.");
		}

		/// <summary>Populates the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object using data from a byte array, a password, and flags for determining how the private key is imported.</summary>
		/// <param name="rawData">A byte array containing data from an X.509 certificate.</param>
		/// <param name="password">The password required to access the X.509 certificate data.</param>
		/// <param name="keyStorageFlags">A bitwise combination of the enumeration values that control where and how to import the certificate.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="rawData" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The length of the <paramref name="rawData" /> parameter is 0.</exception>
		// Token: 0x060031A3 RID: 12707 RVA: 0x000B75E1 File Offset: 0x000B57E1
		[ComVisible(false)]
		public virtual void Import(byte[] rawData, string password, X509KeyStorageFlags keyStorageFlags)
		{
			throw new PlatformNotSupportedException("X509Certificate is immutable on this platform. Use the equivalent constructor instead.");
		}

		/// <summary>Populates an <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object using data from a byte array, a password, and a key storage flag.</summary>
		/// <param name="rawData">A byte array that contains data from an X.509 certificate.</param>
		/// <param name="password">The password required to access the X.509 certificate data.</param>
		/// <param name="keyStorageFlags">A bitwise combination of the enumeration values that control where and how to import the certificate.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="rawData" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The length of the <paramref name="rawData" /> parameter is 0.</exception>
		// Token: 0x060031A4 RID: 12708 RVA: 0x000B75E1 File Offset: 0x000B57E1
		public virtual void Import(byte[] rawData, SecureString password, X509KeyStorageFlags keyStorageFlags)
		{
			throw new PlatformNotSupportedException("X509Certificate is immutable on this platform. Use the equivalent constructor instead.");
		}

		/// <summary>Populates the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object with information from a certificate file.</summary>
		/// <param name="fileName">The name of a certificate file represented as a string.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		// Token: 0x060031A5 RID: 12709 RVA: 0x000B75E1 File Offset: 0x000B57E1
		[ComVisible(false)]
		public virtual void Import(string fileName)
		{
			throw new PlatformNotSupportedException("X509Certificate is immutable on this platform. Use the equivalent constructor instead.");
		}

		/// <summary>Populates the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object with information from a certificate file, a password, and a <see cref="T:System.Security.Cryptography.X509Certificates.X509KeyStorageFlags" /> value.</summary>
		/// <param name="fileName">The name of a certificate file represented as a string.</param>
		/// <param name="password">The password required to access the X.509 certificate data.</param>
		/// <param name="keyStorageFlags">A bitwise combination of the enumeration values that control where and how to import the certificate.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		// Token: 0x060031A6 RID: 12710 RVA: 0x000B75E1 File Offset: 0x000B57E1
		[ComVisible(false)]
		public virtual void Import(string fileName, string password, X509KeyStorageFlags keyStorageFlags)
		{
			throw new PlatformNotSupportedException("X509Certificate is immutable on this platform. Use the equivalent constructor instead.");
		}

		/// <summary>Populates an <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object with information from a certificate file, a password, and a key storage flag.</summary>
		/// <param name="fileName">The name of a certificate file.</param>
		/// <param name="password">The password required to access the X.509 certificate data.</param>
		/// <param name="keyStorageFlags">A bitwise combination of the enumeration values that control where and how to import the certificate.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		// Token: 0x060031A7 RID: 12711 RVA: 0x000B75E1 File Offset: 0x000B57E1
		public virtual void Import(string fileName, SecureString password, X509KeyStorageFlags keyStorageFlags)
		{
			throw new PlatformNotSupportedException("X509Certificate is immutable on this platform. Use the equivalent constructor instead.");
		}

		// Token: 0x060031A8 RID: 12712 RVA: 0x000B75F0 File Offset: 0x000B57F0
		internal DateTime GetNotAfter()
		{
			this.ThrowIfInvalid();
			DateTime dateTime = this.lazyNotAfter;
			if (dateTime == DateTime.MinValue)
			{
				dateTime = (this.lazyNotAfter = this.impl.NotAfter);
			}
			return dateTime;
		}

		// Token: 0x060031A9 RID: 12713 RVA: 0x000B7630 File Offset: 0x000B5830
		internal DateTime GetNotBefore()
		{
			this.ThrowIfInvalid();
			DateTime dateTime = this.lazyNotBefore;
			if (dateTime == DateTime.MinValue)
			{
				dateTime = (this.lazyNotBefore = this.impl.NotBefore);
			}
			return dateTime;
		}

		/// <summary>Converts the specified date and time to a string.</summary>
		/// <param name="date">The date and time to convert.</param>
		/// <returns>A string representation of the value of the <see cref="T:System.DateTime" /> object.</returns>
		// Token: 0x060031AA RID: 12714 RVA: 0x000B7670 File Offset: 0x000B5870
		protected static string FormatDate(DateTime date)
		{
			CultureInfo cultureInfo = CultureInfo.CurrentCulture;
			if (!cultureInfo.DateTimeFormat.Calendar.IsValidDay(date.Year, date.Month, date.Day, 0))
			{
				if (cultureInfo.DateTimeFormat.Calendar is UmAlQuraCalendar)
				{
					cultureInfo = (cultureInfo.Clone() as CultureInfo);
					cultureInfo.DateTimeFormat.Calendar = new HijriCalendar();
				}
				else
				{
					cultureInfo = CultureInfo.InvariantCulture;
				}
			}
			return date.ToString(cultureInfo);
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x000B76EC File Offset: 0x000B58EC
		internal static void ValidateKeyStorageFlags(X509KeyStorageFlags keyStorageFlags)
		{
			if ((keyStorageFlags & ~(X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.UserProtected | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.EphemeralKeySet)) != X509KeyStorageFlags.DefaultKeySet)
			{
				throw new ArgumentException("Value of flags is invalid.", "keyStorageFlags");
			}
			X509KeyStorageFlags x509KeyStorageFlags = keyStorageFlags & (X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.EphemeralKeySet);
			if (x509KeyStorageFlags == (X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.EphemeralKeySet))
			{
				throw new ArgumentException(SR.Format("The flags '{0}' may not be specified together.", x509KeyStorageFlags), "keyStorageFlags");
			}
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x000B7734 File Offset: 0x000B5934
		private void VerifyContentType(X509ContentType contentType)
		{
			if (contentType != X509ContentType.Cert && contentType != X509ContentType.SerializedCert && contentType != X509ContentType.Pfx)
			{
				throw new CryptographicException("Invalid content type.");
			}
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x000B774D File Offset: 0x000B594D
		internal void ImportHandle(X509CertificateImpl impl)
		{
			this.Reset();
			this.impl = impl;
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x060031AE RID: 12718 RVA: 0x000B775C File Offset: 0x000B595C
		internal X509CertificateImpl Impl
		{
			get
			{
				return this.impl;
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x060031AF RID: 12719 RVA: 0x000B7764 File Offset: 0x000B5964
		internal bool IsValid
		{
			get
			{
				return X509Helper.IsValid(this.impl);
			}
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x000B7771 File Offset: 0x000B5971
		internal void ThrowIfInvalid()
		{
			X509Helper.ThrowIfContextInvalid(this.impl);
		}

		// Token: 0x04002298 RID: 8856
		private X509CertificateImpl impl;

		// Token: 0x04002299 RID: 8857
		private volatile byte[] lazyCertHash;

		// Token: 0x0400229A RID: 8858
		private volatile byte[] lazySerialNumber;

		// Token: 0x0400229B RID: 8859
		private volatile string lazyIssuer;

		// Token: 0x0400229C RID: 8860
		private volatile string lazySubject;

		// Token: 0x0400229D RID: 8861
		private volatile string lazyKeyAlgorithm;

		// Token: 0x0400229E RID: 8862
		private volatile byte[] lazyKeyAlgorithmParameters;

		// Token: 0x0400229F RID: 8863
		private volatile byte[] lazyPublicKey;

		// Token: 0x040022A0 RID: 8864
		private DateTime lazyNotBefore = DateTime.MinValue;

		// Token: 0x040022A1 RID: 8865
		private DateTime lazyNotAfter = DateTime.MinValue;

		// Token: 0x040022A2 RID: 8866
		internal const X509KeyStorageFlags KeyStorageFlagsAll = X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.UserProtected | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.EphemeralKeySet;
	}
}
