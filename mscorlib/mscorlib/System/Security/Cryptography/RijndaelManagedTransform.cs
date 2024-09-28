﻿using System;
using System.Runtime.InteropServices;
using Unity;

namespace System.Security.Cryptography
{
	/// <summary>Performs a cryptographic transformation of data using the Rijndael algorithm. This class cannot be inherited.</summary>
	// Token: 0x020004A6 RID: 1190
	[ComVisible(true)]
	public sealed class RijndaelManagedTransform : ICryptoTransform, IDisposable
	{
		// Token: 0x06002F8A RID: 12170 RVA: 0x000A9250 File Offset: 0x000A7450
		internal RijndaelManagedTransform(byte[] rgbKey, CipherMode mode, byte[] rgbIV, int blockSize, int feedbackSize, PaddingMode PaddingValue, RijndaelManagedTransformMode transformMode)
		{
			if (rgbKey == null)
			{
				throw new ArgumentNullException("rgbKey");
			}
			this.m_blockSizeBits = blockSize;
			this.m_blockSizeBytes = blockSize / 8;
			this.m_cipherMode = mode;
			this.m_paddingValue = PaddingValue;
			this.m_transformMode = transformMode;
			this.m_Nb = blockSize / 32;
			this.m_Nk = rgbKey.Length / 4;
			int num = (this.m_Nb > 6) ? 3 : 2;
			int num2 = (this.m_Nb > 6) ? 4 : 3;
			int[] array = new int[this.m_Nb];
			int[] array2 = new int[this.m_Nb];
			int[] array3 = new int[this.m_Nb];
			int[] array4 = new int[this.m_Nb];
			int[] array5 = new int[this.m_Nb];
			int[] array6 = new int[this.m_Nb];
			for (int i = 0; i < this.m_Nb; i++)
			{
				array[i] = (i + 1) % this.m_Nb;
				array2[i] = (i + num) % this.m_Nb;
				array3[i] = (i + num2) % this.m_Nb;
				array4[i] = (i - 1 + this.m_Nb) % this.m_Nb;
				array5[i] = (i - num + this.m_Nb) % this.m_Nb;
				array6[i] = (i - num2 + this.m_Nb) % this.m_Nb;
			}
			this.m_encryptindex = new int[this.m_Nb * 3];
			Array.Copy(array, 0, this.m_encryptindex, 0, this.m_Nb);
			Array.Copy(array2, 0, this.m_encryptindex, this.m_Nb, this.m_Nb);
			Array.Copy(array3, 0, this.m_encryptindex, this.m_Nb * 2, this.m_Nb);
			this.m_decryptindex = new int[this.m_Nb * 3];
			Array.Copy(array4, 0, this.m_decryptindex, 0, this.m_Nb);
			Array.Copy(array5, 0, this.m_decryptindex, this.m_Nb, this.m_Nb);
			Array.Copy(array6, 0, this.m_decryptindex, this.m_Nb * 2, this.m_Nb);
			CipherMode cipherMode = this.m_cipherMode;
			if (cipherMode - CipherMode.CBC > 1)
			{
				if (cipherMode != CipherMode.CFB)
				{
					throw new CryptographicException(Environment.GetResourceString("Specified cipher mode is not valid for this algorithm."));
				}
				this.m_inputBlockSize = feedbackSize / 8;
				this.m_outputBlockSize = feedbackSize / 8;
			}
			else
			{
				this.m_inputBlockSize = this.m_blockSizeBytes;
				this.m_outputBlockSize = this.m_blockSizeBytes;
			}
			if (mode == CipherMode.CBC || mode == CipherMode.CFB)
			{
				if (rgbIV == null)
				{
					throw new ArgumentNullException("rgbIV");
				}
				if (rgbIV.Length / 4 != this.m_Nb)
				{
					throw new CryptographicException(Environment.GetResourceString("Specified initialization vector (IV) does not match the block size for this algorithm."));
				}
				this.m_IV = new int[this.m_Nb];
				int num3 = 0;
				for (int j = 0; j < this.m_Nb; j++)
				{
					int num4 = (int)rgbIV[num3++];
					int num5 = (int)rgbIV[num3++];
					int num6 = (int)rgbIV[num3++];
					int num7 = (int)rgbIV[num3++];
					this.m_IV[j] = (num7 << 24 | num6 << 16 | num5 << 8 | num4);
				}
			}
			this.GenerateKeyExpansion(rgbKey);
			if (this.m_cipherMode == CipherMode.CBC)
			{
				this.m_lastBlockBuffer = new int[this.m_Nb];
				Buffer.InternalBlockCopy(this.m_IV, 0, this.m_lastBlockBuffer, 0, this.m_blockSizeBytes);
			}
			if (this.m_cipherMode == CipherMode.CFB)
			{
				this.m_shiftRegister = new byte[4 * this.m_Nb];
				Buffer.InternalBlockCopy(this.m_IV, 0, this.m_shiftRegister, 0, 4 * this.m_Nb);
			}
		}

		/// <summary>Releases all resources used by the current instance of the <see cref="T:System.Security.Cryptography.RijndaelManagedTransform" /> class.</summary>
		// Token: 0x06002F8B RID: 12171 RVA: 0x000A95CD File Offset: 0x000A77CD
		public void Dispose()
		{
			this.Dispose(true);
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Security.Cryptography.RijndaelManagedTransform" /> class.</summary>
		// Token: 0x06002F8C RID: 12172 RVA: 0x000A95CD File Offset: 0x000A77CD
		public void Clear()
		{
			this.Dispose(true);
		}

		// Token: 0x06002F8D RID: 12173 RVA: 0x000A95D8 File Offset: 0x000A77D8
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.m_IV != null)
				{
					Array.Clear(this.m_IV, 0, this.m_IV.Length);
					this.m_IV = null;
				}
				if (this.m_lastBlockBuffer != null)
				{
					Array.Clear(this.m_lastBlockBuffer, 0, this.m_lastBlockBuffer.Length);
					this.m_lastBlockBuffer = null;
				}
				if (this.m_encryptKeyExpansion != null)
				{
					Array.Clear(this.m_encryptKeyExpansion, 0, this.m_encryptKeyExpansion.Length);
					this.m_encryptKeyExpansion = null;
				}
				if (this.m_decryptKeyExpansion != null)
				{
					Array.Clear(this.m_decryptKeyExpansion, 0, this.m_decryptKeyExpansion.Length);
					this.m_decryptKeyExpansion = null;
				}
				if (this.m_depadBuffer != null)
				{
					Array.Clear(this.m_depadBuffer, 0, this.m_depadBuffer.Length);
					this.m_depadBuffer = null;
				}
				if (this.m_shiftRegister != null)
				{
					Array.Clear(this.m_shiftRegister, 0, this.m_shiftRegister.Length);
					this.m_shiftRegister = null;
				}
			}
		}

		/// <summary>Gets the block size.</summary>
		/// <returns>The size of the data blocks in bytes.</returns>
		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x06002F8E RID: 12174 RVA: 0x000A96BD File Offset: 0x000A78BD
		public int BlockSizeValue
		{
			get
			{
				return this.m_blockSizeBits;
			}
		}

		/// <summary>Gets the input block size.</summary>
		/// <returns>The size of the input data blocks in bytes.</returns>
		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06002F8F RID: 12175 RVA: 0x000A96C5 File Offset: 0x000A78C5
		public int InputBlockSize
		{
			get
			{
				return this.m_inputBlockSize;
			}
		}

		/// <summary>Gets the output block size.</summary>
		/// <returns>The size of the output data blocks in bytes.</returns>
		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06002F90 RID: 12176 RVA: 0x000A96CD File Offset: 0x000A78CD
		public int OutputBlockSize
		{
			get
			{
				return this.m_outputBlockSize;
			}
		}

		/// <summary>Gets a value indicating whether multiple blocks can be transformed.</summary>
		/// <returns>
		///   <see langword="true" /> if multiple blocks can be transformed; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06002F91 RID: 12177 RVA: 0x000040F7 File Offset: 0x000022F7
		public bool CanTransformMultipleBlocks
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a value indicating whether the current transform can be reused.</summary>
		/// <returns>Always <see langword="true" />.</returns>
		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06002F92 RID: 12178 RVA: 0x000040F7 File Offset: 0x000022F7
		public bool CanReuseTransform
		{
			get
			{
				return true;
			}
		}

		/// <summary>Computes the transformation for the specified region of the input byte array and copies the resulting transformation to the specified region of the output byte array.</summary>
		/// <param name="inputBuffer">The input to perform the operation on.</param>
		/// <param name="inputOffset">The offset into the input byte array to begin using data from.</param>
		/// <param name="inputCount">The number of bytes in the input byte array to use as data.</param>
		/// <param name="outputBuffer">The output to write the data to.</param>
		/// <param name="outputOffset">The offset into the output byte array to begin writing data from.</param>
		/// <returns>The number of bytes written.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="inputBuffer" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="outputBuffer" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The length of the input buffer is less than the sum of the input offset and the input count.  
		///  -or-  
		///  The value of the <paramref name="inputCount" /> parameter is less than or equal to 0.  
		///  -or-  
		///  The value of the <paramref name="inputCount" /> parameter is greater than the length of the <paramref name="inputBuffer" /> parameter.  
		///  -or-  
		///  The length of the <paramref name="inputCount" /> parameter is not evenly devisable by input block size.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <paramref name="inputOffset" /> parameter is negative.</exception>
		// Token: 0x06002F93 RID: 12179 RVA: 0x000A96D8 File Offset: 0x000A78D8
		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			if (inputBuffer == null)
			{
				throw new ArgumentNullException("inputBuffer");
			}
			if (outputBuffer == null)
			{
				throw new ArgumentNullException("outputBuffer");
			}
			if (inputOffset < 0)
			{
				throw new ArgumentOutOfRangeException("inputOffset", Environment.GetResourceString("Non-negative number required."));
			}
			if (inputCount <= 0 || inputCount % this.InputBlockSize != 0 || inputCount > inputBuffer.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Value was invalid."));
			}
			if (inputBuffer.Length - inputCount < inputOffset)
			{
				throw new ArgumentException(Environment.GetResourceString("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection."));
			}
			if (this.m_transformMode == RijndaelManagedTransformMode.Encrypt)
			{
				return this.EncryptData(inputBuffer, inputOffset, inputCount, ref outputBuffer, outputOffset, this.m_paddingValue, false);
			}
			if (this.m_paddingValue == PaddingMode.Zeros || this.m_paddingValue == PaddingMode.None)
			{
				return this.DecryptData(inputBuffer, inputOffset, inputCount, ref outputBuffer, outputOffset, this.m_paddingValue, false);
			}
			if (this.m_depadBuffer == null)
			{
				this.m_depadBuffer = new byte[this.InputBlockSize];
				int num = inputCount - this.InputBlockSize;
				Buffer.InternalBlockCopy(inputBuffer, inputOffset + num, this.m_depadBuffer, 0, this.InputBlockSize);
				return this.DecryptData(inputBuffer, inputOffset, num, ref outputBuffer, outputOffset, this.m_paddingValue, false);
			}
			int num2 = this.DecryptData(this.m_depadBuffer, 0, this.m_depadBuffer.Length, ref outputBuffer, outputOffset, this.m_paddingValue, false);
			outputOffset += this.OutputBlockSize;
			int num3 = inputCount - this.InputBlockSize;
			Buffer.InternalBlockCopy(inputBuffer, inputOffset + num3, this.m_depadBuffer, 0, this.InputBlockSize);
			num2 = this.DecryptData(inputBuffer, inputOffset, num3, ref outputBuffer, outputOffset, this.m_paddingValue, false);
			return this.OutputBlockSize + num2;
		}

		/// <summary>Computes the transformation for the specified region of the specified byte array.</summary>
		/// <param name="inputBuffer">The input to perform the operation on.</param>
		/// <param name="inputOffset">The offset into the byte array to begin using data from.</param>
		/// <param name="inputCount">The number of bytes in the byte array to use as data.</param>
		/// <returns>The computed transformation.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="inputBuffer" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The value of the <paramref name="inputCount" /> parameter is less than 0.  
		///  -or-  
		///  The value of the <paramref name="inputCount" /> parameter is grater than the length of <paramref name="inputBuffer" /> parameter.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <paramref name="inputOffset" /> parameter is negative.</exception>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The length of the <paramref name="inputCount" /> parameter is not evenly devisable by input block size.</exception>
		// Token: 0x06002F94 RID: 12180 RVA: 0x000A9854 File Offset: 0x000A7A54
		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			if (inputBuffer == null)
			{
				throw new ArgumentNullException("inputBuffer");
			}
			if (inputOffset < 0)
			{
				throw new ArgumentOutOfRangeException("inputOffset", Environment.GetResourceString("Non-negative number required."));
			}
			if (inputCount < 0 || inputCount > inputBuffer.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Value was invalid."));
			}
			if (inputBuffer.Length - inputCount < inputOffset)
			{
				throw new ArgumentException(Environment.GetResourceString("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection."));
			}
			if (this.m_transformMode == RijndaelManagedTransformMode.Encrypt)
			{
				byte[] result = null;
				this.EncryptData(inputBuffer, inputOffset, inputCount, ref result, 0, this.m_paddingValue, true);
				this.Reset();
				return result;
			}
			if (inputCount % this.InputBlockSize != 0)
			{
				throw new CryptographicException(Environment.GetResourceString("Length of the data to decrypt is invalid."));
			}
			if (this.m_depadBuffer == null)
			{
				byte[] result2 = null;
				this.DecryptData(inputBuffer, inputOffset, inputCount, ref result2, 0, this.m_paddingValue, true);
				this.Reset();
				return result2;
			}
			byte[] array = new byte[this.m_depadBuffer.Length + inputCount];
			Buffer.InternalBlockCopy(this.m_depadBuffer, 0, array, 0, this.m_depadBuffer.Length);
			Buffer.InternalBlockCopy(inputBuffer, inputOffset, array, this.m_depadBuffer.Length, inputCount);
			byte[] result3 = null;
			this.DecryptData(array, 0, array.Length, ref result3, 0, this.m_paddingValue, true);
			this.Reset();
			return result3;
		}

		/// <summary>Resets the internal state of <see cref="T:System.Security.Cryptography.RijndaelManagedTransform" /> so it can be used again to do a different encryption or decryption.</summary>
		// Token: 0x06002F95 RID: 12181 RVA: 0x000A9978 File Offset: 0x000A7B78
		public void Reset()
		{
			this.m_depadBuffer = null;
			if (this.m_cipherMode == CipherMode.CBC)
			{
				Buffer.InternalBlockCopy(this.m_IV, 0, this.m_lastBlockBuffer, 0, this.m_blockSizeBytes);
			}
			if (this.m_cipherMode == CipherMode.CFB)
			{
				Buffer.InternalBlockCopy(this.m_IV, 0, this.m_shiftRegister, 0, 4 * this.m_Nb);
			}
		}

		// Token: 0x06002F96 RID: 12182 RVA: 0x000A99D4 File Offset: 0x000A7BD4
		[SecuritySafeCritical]
		private unsafe int EncryptData(byte[] inputBuffer, int inputOffset, int inputCount, ref byte[] outputBuffer, int outputOffset, PaddingMode paddingMode, bool fLast)
		{
			if (inputBuffer.Length < inputOffset + inputCount)
			{
				throw new CryptographicException(Environment.GetResourceString("Input buffer contains insufficient data."));
			}
			int num = 0;
			int num2 = inputCount % this.m_inputBlockSize;
			byte[] array = null;
			int num3 = inputOffset;
			if (fLast)
			{
				switch (paddingMode)
				{
				case PaddingMode.None:
					if (num2 != 0)
					{
						throw new CryptographicException(Environment.GetResourceString("Length of the data to encrypt is invalid."));
					}
					break;
				case PaddingMode.PKCS7:
					num = this.m_inputBlockSize - num2;
					break;
				case PaddingMode.Zeros:
					if (num2 != 0)
					{
						num = this.m_inputBlockSize - num2;
					}
					break;
				case PaddingMode.ANSIX923:
					num = this.m_inputBlockSize - num2;
					break;
				case PaddingMode.ISO10126:
					num = this.m_inputBlockSize - num2;
					break;
				}
				if (num != 0)
				{
					array = new byte[num];
					switch (paddingMode)
					{
					case PaddingMode.PKCS7:
						for (int i = 0; i < num; i++)
						{
							array[i] = (byte)num;
						}
						break;
					case PaddingMode.ANSIX923:
						array[num - 1] = (byte)num;
						break;
					case PaddingMode.ISO10126:
						Utils.StaticRandomNumberGenerator.GetBytes(array);
						array[num - 1] = (byte)num;
						break;
					}
				}
			}
			if (outputBuffer == null)
			{
				outputBuffer = new byte[inputCount + num];
				outputOffset = 0;
			}
			else if (outputBuffer.Length - outputOffset < inputCount + num)
			{
				throw new CryptographicException(Environment.GetResourceString("Input buffer contains insufficient data."));
			}
			int[] array2;
			int* encryptindex;
			if ((array2 = this.m_encryptindex) == null || array2.Length == 0)
			{
				encryptindex = null;
			}
			else
			{
				encryptindex = &array2[0];
			}
			int[] array3;
			int* encryptKeyExpansion;
			if ((array3 = this.m_encryptKeyExpansion) == null || array3.Length == 0)
			{
				encryptKeyExpansion = null;
			}
			else
			{
				encryptKeyExpansion = &array3[0];
			}
			int[] array4;
			int* t;
			if ((array4 = RijndaelManagedTransform.s_T) == null || array4.Length == 0)
			{
				t = null;
			}
			else
			{
				t = &array4[0];
			}
			int[] array5;
			int* tf;
			if ((array5 = RijndaelManagedTransform.s_TF) == null || array5.Length == 0)
			{
				tf = null;
			}
			else
			{
				tf = &array5[0];
			}
			int* ptr;
			int* ptr2;
			checked
			{
				ptr = stackalloc int[unchecked((UIntPtr)this.m_Nb) * 4];
				ptr2 = stackalloc int[unchecked((UIntPtr)this.m_Nb) * 4];
			}
			int num4 = (inputCount + num) / this.m_inputBlockSize;
			int num5 = outputOffset;
			for (int j = 0; j < num4; j++)
			{
				if (this.m_cipherMode == CipherMode.CFB)
				{
					Buffer.Memcpy((byte*)ptr, 0, this.m_shiftRegister, 0, this.m_blockSizeBytes);
				}
				else if (j != num4 - 1 || num == 0)
				{
					Buffer.Memcpy((byte*)ptr, 0, inputBuffer, num3, this.m_blockSizeBytes);
				}
				else
				{
					int num6 = 0;
					int i = num3;
					for (int k = 0; k < this.m_Nb; k++)
					{
						int num7 = (int)((i >= num3 + num2) ? array[num6++] : inputBuffer[i++]);
						int num8 = (int)((i >= num3 + num2) ? array[num6++] : inputBuffer[i++]);
						int num9 = (int)((i >= num3 + num2) ? array[num6++] : inputBuffer[i++]);
						int num10 = (int)((i >= num3 + num2) ? array[num6++] : inputBuffer[i++]);
						ptr[k] = (num10 << 24 | num9 << 16 | num8 << 8 | num7);
					}
				}
				if (this.m_cipherMode == CipherMode.CBC)
				{
					for (int l = 0; l < this.m_Nb; l++)
					{
						ptr[l] ^= this.m_lastBlockBuffer[l];
					}
				}
				this.Enc(encryptindex, encryptKeyExpansion, t, tf, ptr, ptr2);
				if (this.m_cipherMode == CipherMode.CFB)
				{
					int i = num3;
					if (j != num4 - 1 || num == 0)
					{
						for (int m = 0; m < this.m_Nb; m++)
						{
							if (i >= num3 + this.m_inputBlockSize)
							{
								break;
							}
							outputBuffer[num5++] = (byte)((ptr2[m] & 255) ^ (int)inputBuffer[i++]);
							if (i >= num3 + this.m_inputBlockSize)
							{
								break;
							}
							outputBuffer[num5++] = (byte)((ptr2[m] >> 8 & 255) ^ (int)inputBuffer[i++]);
							if (i >= num3 + this.m_inputBlockSize)
							{
								break;
							}
							outputBuffer[num5++] = (byte)((ptr2[m] >> 16 & 255) ^ (int)inputBuffer[i++]);
							if (i >= num3 + this.m_inputBlockSize)
							{
								break;
							}
							outputBuffer[num5++] = (byte)((ptr2[m] >> 24 & 255) ^ (int)inputBuffer[i++]);
						}
					}
					else
					{
						byte[] array6 = new byte[this.m_inputBlockSize];
						Buffer.InternalBlockCopy(inputBuffer, num3, array6, 0, num2);
						Buffer.InternalBlockCopy(array, 0, array6, num2, num);
						i = 0;
						int num11 = 0;
						while (num11 < this.m_Nb && i < this.m_inputBlockSize)
						{
							outputBuffer[num5++] = (byte)((ptr2[num11] & 255) ^ (int)array6[i++]);
							if (i >= this.m_inputBlockSize)
							{
								break;
							}
							outputBuffer[num5++] = (byte)((ptr2[num11] >> 8 & 255) ^ (int)array6[i++]);
							if (i >= this.m_inputBlockSize)
							{
								break;
							}
							outputBuffer[num5++] = (byte)((ptr2[num11] >> 16 & 255) ^ (int)array6[i++]);
							if (i >= this.m_inputBlockSize)
							{
								break;
							}
							outputBuffer[num5++] = (byte)((ptr2[num11] >> 24 & 255) ^ (int)array6[i++]);
							num11++;
						}
					}
					for (i = 0; i < this.m_blockSizeBytes - this.m_inputBlockSize; i++)
					{
						this.m_shiftRegister[i] = this.m_shiftRegister[i + this.m_inputBlockSize];
					}
					Buffer.InternalBlockCopy(outputBuffer, j * this.m_inputBlockSize, this.m_shiftRegister, this.m_blockSizeBytes - this.m_inputBlockSize, this.m_inputBlockSize);
				}
				else
				{
					for (int n = 0; n < this.m_Nb; n++)
					{
						outputBuffer[num5++] = (byte)(ptr2[n] & 255);
						outputBuffer[num5++] = (byte)(ptr2[n] >> 8 & 255);
						outputBuffer[num5++] = (byte)(ptr2[n] >> 16 & 255);
						outputBuffer[num5++] = (byte)(ptr2[n] >> 24 & 255);
					}
					if (this.m_cipherMode == CipherMode.CBC)
					{
						int[] array7;
						int* dest;
						if ((array7 = this.m_lastBlockBuffer) == null || array7.Length == 0)
						{
							dest = null;
						}
						else
						{
							dest = &array7[0];
						}
						Buffer.Memcpy((byte*)dest, (byte*)ptr2, this.m_blockSizeBytes);
						array7 = null;
					}
				}
				num3 += this.m_inputBlockSize;
			}
			array5 = null;
			array4 = null;
			array3 = null;
			array2 = null;
			return inputCount + num;
		}

		// Token: 0x06002F97 RID: 12183 RVA: 0x000AA06C File Offset: 0x000A826C
		[SecuritySafeCritical]
		private unsafe int DecryptData(byte[] inputBuffer, int inputOffset, int inputCount, ref byte[] outputBuffer, int outputOffset, PaddingMode paddingMode, bool fLast)
		{
			if (inputBuffer.Length < inputOffset + inputCount)
			{
				throw new CryptographicException(Environment.GetResourceString("Input buffer contains insufficient data."));
			}
			if (outputBuffer == null)
			{
				outputBuffer = new byte[inputCount];
				outputOffset = 0;
			}
			else if (outputBuffer.Length - outputOffset < inputCount)
			{
				throw new CryptographicException(Environment.GetResourceString("Input buffer contains insufficient data."));
			}
			int[] encryptindex;
			int* encryptindex2;
			if ((encryptindex = this.m_encryptindex) == null || encryptindex.Length == 0)
			{
				encryptindex2 = null;
			}
			else
			{
				encryptindex2 = &encryptindex[0];
			}
			int[] encryptKeyExpansion;
			int* encryptKeyExpansion2;
			if ((encryptKeyExpansion = this.m_encryptKeyExpansion) == null || encryptKeyExpansion.Length == 0)
			{
				encryptKeyExpansion2 = null;
			}
			else
			{
				encryptKeyExpansion2 = &encryptKeyExpansion[0];
			}
			int[] decryptindex;
			int* decryptindex2;
			if ((decryptindex = this.m_decryptindex) == null || decryptindex.Length == 0)
			{
				decryptindex2 = null;
			}
			else
			{
				decryptindex2 = &decryptindex[0];
			}
			int[] decryptKeyExpansion;
			int* decryptKeyExpansion2;
			if ((decryptKeyExpansion = this.m_decryptKeyExpansion) == null || decryptKeyExpansion.Length == 0)
			{
				decryptKeyExpansion2 = null;
			}
			else
			{
				decryptKeyExpansion2 = &decryptKeyExpansion[0];
			}
			int[] array;
			int* t;
			if ((array = RijndaelManagedTransform.s_T) == null || array.Length == 0)
			{
				t = null;
			}
			else
			{
				t = &array[0];
			}
			int[] array2;
			int* tf;
			if ((array2 = RijndaelManagedTransform.s_TF) == null || array2.Length == 0)
			{
				tf = null;
			}
			else
			{
				tf = &array2[0];
			}
			int[] array3;
			int* iT;
			if ((array3 = RijndaelManagedTransform.s_iT) == null || array3.Length == 0)
			{
				iT = null;
			}
			else
			{
				iT = &array3[0];
			}
			int[] array4;
			int* iTF;
			if ((array4 = RijndaelManagedTransform.s_iTF) == null || array4.Length == 0)
			{
				iTF = null;
			}
			else
			{
				iTF = &array4[0];
			}
			int* ptr;
			int* ptr2;
			int num;
			int num2;
			int num3;
			checked
			{
				ptr = stackalloc int[unchecked((UIntPtr)this.m_Nb) * 4];
				ptr2 = stackalloc int[unchecked((UIntPtr)this.m_Nb) * 4];
				num = inputCount / this.m_inputBlockSize;
				num2 = inputOffset;
				num3 = outputOffset;
			}
			for (int i = 0; i < num; i++)
			{
				if (this.m_cipherMode == CipherMode.CFB)
				{
					int j = 0;
					for (int k = 0; k < this.m_Nb; k++)
					{
						int num4 = (int)this.m_shiftRegister[j++];
						int num5 = (int)this.m_shiftRegister[j++];
						int num6 = (int)this.m_shiftRegister[j++];
						int num7 = (int)this.m_shiftRegister[j++];
						ptr[k] = (num7 << 24 | num6 << 16 | num5 << 8 | num4);
					}
				}
				else
				{
					int j = num2;
					for (int l = 0; l < this.m_Nb; l++)
					{
						int num8 = (int)inputBuffer[j++];
						int num9 = (int)inputBuffer[j++];
						int num10 = (int)inputBuffer[j++];
						int num11 = (int)inputBuffer[j++];
						ptr[l] = (num11 << 24 | num10 << 16 | num9 << 8 | num8);
					}
				}
				if (this.m_cipherMode == CipherMode.CFB)
				{
					this.Enc(encryptindex2, encryptKeyExpansion2, t, tf, ptr, ptr2);
					int j = num2;
					int num12 = 0;
					while (num12 < this.m_Nb && j < num2 + this.m_inputBlockSize)
					{
						outputBuffer[num3++] = (byte)((ptr2[num12] & 255) ^ (int)inputBuffer[j++]);
						if (j >= num2 + this.m_inputBlockSize)
						{
							break;
						}
						outputBuffer[num3++] = (byte)((ptr2[num12] >> 8 & 255) ^ (int)inputBuffer[j++]);
						if (j >= num2 + this.m_inputBlockSize)
						{
							break;
						}
						outputBuffer[num3++] = (byte)((ptr2[num12] >> 16 & 255) ^ (int)inputBuffer[j++]);
						if (j >= num2 + this.m_inputBlockSize)
						{
							break;
						}
						outputBuffer[num3++] = (byte)((ptr2[num12] >> 24 & 255) ^ (int)inputBuffer[j++]);
						num12++;
					}
					for (j = 0; j < this.m_blockSizeBytes - this.m_inputBlockSize; j++)
					{
						this.m_shiftRegister[j] = this.m_shiftRegister[j + this.m_inputBlockSize];
					}
					Buffer.InternalBlockCopy(inputBuffer, num2, this.m_shiftRegister, this.m_blockSizeBytes - this.m_inputBlockSize, this.m_inputBlockSize);
				}
				else
				{
					this.Dec(decryptindex2, decryptKeyExpansion2, iT, iTF, ptr, ptr2);
					if (this.m_cipherMode == CipherMode.CBC)
					{
						int j = num2;
						for (int m = 0; m < this.m_Nb; m++)
						{
							ptr2[m] ^= this.m_lastBlockBuffer[m];
							int num13 = (int)inputBuffer[j++];
							int num14 = (int)inputBuffer[j++];
							int num15 = (int)inputBuffer[j++];
							int num16 = (int)inputBuffer[j++];
							this.m_lastBlockBuffer[m] = (num16 << 24 | num15 << 16 | num14 << 8 | num13);
						}
					}
					for (int n = 0; n < this.m_Nb; n++)
					{
						outputBuffer[num3++] = (byte)(ptr2[n] & 255);
						outputBuffer[num3++] = (byte)(ptr2[n] >> 8 & 255);
						outputBuffer[num3++] = (byte)(ptr2[n] >> 16 & 255);
						outputBuffer[num3++] = (byte)(ptr2[n] >> 24 & 255);
					}
				}
				num2 += this.m_inputBlockSize;
			}
			if (!fLast)
			{
				return inputCount;
			}
			byte[] array5 = outputBuffer;
			switch (paddingMode)
			{
			case PaddingMode.PKCS7:
			{
				if (inputCount == 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Padding is invalid and cannot be removed."));
				}
				int num17 = (int)outputBuffer[inputCount - 1];
				if (num17 > outputBuffer.Length || num17 > this.InputBlockSize || num17 <= 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Padding is invalid and cannot be removed."));
				}
				for (int j = 1; j <= num17; j++)
				{
					if ((int)outputBuffer[inputCount - j] != num17)
					{
						throw new CryptographicException(Environment.GetResourceString("Padding is invalid and cannot be removed."));
					}
				}
				array5 = new byte[outputBuffer.Length - num17];
				Buffer.InternalBlockCopy(outputBuffer, 0, array5, 0, outputBuffer.Length - num17);
				break;
			}
			case PaddingMode.ANSIX923:
			{
				if (inputCount == 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Padding is invalid and cannot be removed."));
				}
				int num17 = (int)outputBuffer[inputCount - 1];
				if (num17 > outputBuffer.Length || num17 > this.InputBlockSize || num17 <= 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Padding is invalid and cannot be removed."));
				}
				for (int j = 2; j <= num17; j++)
				{
					if (outputBuffer[inputCount - j] != 0)
					{
						throw new CryptographicException(Environment.GetResourceString("Padding is invalid and cannot be removed."));
					}
				}
				array5 = new byte[outputBuffer.Length - num17];
				Buffer.InternalBlockCopy(outputBuffer, 0, array5, 0, outputBuffer.Length - num17);
				break;
			}
			case PaddingMode.ISO10126:
			{
				if (inputCount == 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Padding is invalid and cannot be removed."));
				}
				int num17 = (int)outputBuffer[inputCount - 1];
				if (num17 > outputBuffer.Length || num17 > this.InputBlockSize || num17 <= 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Padding is invalid and cannot be removed."));
				}
				array5 = new byte[outputBuffer.Length - num17];
				Buffer.InternalBlockCopy(outputBuffer, 0, array5, 0, outputBuffer.Length - num17);
				break;
			}
			}
			outputBuffer = array5;
			return array5.Length;
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x000AA74C File Offset: 0x000A894C
		[SecurityCritical]
		private unsafe void Enc(int* encryptindex, int* encryptKeyExpansion, int* T, int* TF, int* work, int* temp)
		{
			for (int i = 0; i < this.m_Nb; i++)
			{
				work[i] ^= encryptKeyExpansion[i];
			}
			int* ptr = encryptKeyExpansion + this.m_Nb;
			int* ptr2;
			for (int j = 1; j < this.m_Nr; j++)
			{
				ptr2 = encryptindex;
				for (int k = 0; k < this.m_Nb; k++)
				{
					temp[k] = (T[work[k] & 255] ^ T[256 + (work[*ptr2] >> 8 & 255)] ^ T[512 + (work[ptr2[this.m_Nb]] >> 16 & 255)] ^ T[768 + (work[ptr2[this.m_Nb * 2]] >> 24 & 255)] ^ *ptr);
					ptr2++;
					ptr++;
				}
				for (int l = 0; l < this.m_Nb; l++)
				{
					work[l] = temp[l];
				}
			}
			ptr2 = encryptindex;
			for (int m = 0; m < this.m_Nb; m++)
			{
				temp[m] = (TF[work[m] & 255] ^ TF[256 + (work[*ptr2] >> 8 & 255)] ^ TF[512 + (work[ptr2[this.m_Nb]] >> 16 & 255)] ^ TF[768 + (work[ptr2[this.m_Nb * 2]] >> 24 & 255)] ^ *ptr);
				ptr2++;
				ptr++;
			}
		}

		// Token: 0x06002F99 RID: 12185 RVA: 0x000AA93C File Offset: 0x000A8B3C
		[SecurityCritical]
		private unsafe void Dec(int* decryptindex, int* decryptKeyExpansion, int* iT, int* iTF, int* work, int* temp)
		{
			int num = this.m_Nb * this.m_Nr;
			for (int i = 0; i < this.m_Nb; i++)
			{
				work[i] ^= decryptKeyExpansion[num];
				num++;
			}
			int* ptr;
			int* ptr2;
			for (int j = 1; j < this.m_Nr; j++)
			{
				num -= 2 * this.m_Nb;
				ptr = decryptindex;
				ptr2 = decryptKeyExpansion + num;
				for (int k = 0; k < this.m_Nb; k++)
				{
					temp[k] = (iT[work[k] & 255] ^ iT[256 + (work[*ptr] >> 8 & 255)] ^ iT[512 + (work[ptr[this.m_Nb]] >> 16 & 255)] ^ iT[768 + (work[ptr[this.m_Nb * 2]] >> 24 & 255)] ^ *ptr2);
					num++;
					ptr++;
					ptr2++;
				}
				for (int l = 0; l < this.m_Nb; l++)
				{
					work[l] = temp[l];
				}
			}
			num = 0;
			ptr = decryptindex;
			ptr2 = decryptKeyExpansion + num;
			for (int m = 0; m < this.m_Nb; m++)
			{
				temp[m] = (iTF[work[m] & 255] ^ iTF[256 + (work[*ptr] >> 8 & 255)] ^ iTF[512 + (work[ptr[this.m_Nb]] >> 16 & 255)] ^ iTF[768 + (work[ptr[this.m_Nb * 2]] >> 24 & 255)] ^ *ptr2);
				ptr++;
				ptr2++;
			}
		}

		// Token: 0x06002F9A RID: 12186 RVA: 0x000AAB58 File Offset: 0x000A8D58
		private void GenerateKeyExpansion(byte[] rgbKey)
		{
			int num = (this.m_blockSizeBits > rgbKey.Length * 8) ? this.m_blockSizeBits : (rgbKey.Length * 8);
			if (num != 128)
			{
				if (num != 192)
				{
					if (num != 256)
					{
						throw new CryptographicException(Environment.GetResourceString("Specified key is not a valid size for this algorithm."));
					}
					this.m_Nr = 14;
				}
				else
				{
					this.m_Nr = 12;
				}
			}
			else
			{
				this.m_Nr = 10;
			}
			this.m_encryptKeyExpansion = new int[this.m_Nb * (this.m_Nr + 1)];
			this.m_decryptKeyExpansion = new int[this.m_Nb * (this.m_Nr + 1)];
			int num2 = 0;
			for (int i = 0; i < this.m_Nk; i++)
			{
				int num3 = (int)rgbKey[num2++];
				int num4 = (int)rgbKey[num2++];
				int num5 = (int)rgbKey[num2++];
				int num6 = (int)rgbKey[num2++];
				this.m_encryptKeyExpansion[i] = (num6 << 24 | num5 << 16 | num4 << 8 | num3);
			}
			if (this.m_Nk <= 6)
			{
				for (int j = this.m_Nk; j < this.m_Nb * (this.m_Nr + 1); j++)
				{
					int num7 = this.m_encryptKeyExpansion[j - 1];
					if (j % this.m_Nk == 0)
					{
						num7 = RijndaelManagedTransform.SubWord(RijndaelManagedTransform.rot3(num7));
						num7 ^= RijndaelManagedTransform.s_Rcon[j / this.m_Nk - 1];
					}
					this.m_encryptKeyExpansion[j] = (this.m_encryptKeyExpansion[j - this.m_Nk] ^ num7);
				}
			}
			else
			{
				for (int k = this.m_Nk; k < this.m_Nb * (this.m_Nr + 1); k++)
				{
					int num7 = this.m_encryptKeyExpansion[k - 1];
					if (k % this.m_Nk == 0)
					{
						num7 = RijndaelManagedTransform.SubWord(RijndaelManagedTransform.rot3(num7));
						num7 ^= RijndaelManagedTransform.s_Rcon[k / this.m_Nk - 1];
					}
					else if (k % this.m_Nk == 4)
					{
						num7 = RijndaelManagedTransform.SubWord(num7);
					}
					this.m_encryptKeyExpansion[k] = (this.m_encryptKeyExpansion[k - this.m_Nk] ^ num7);
				}
			}
			for (int l = 0; l < this.m_Nb; l++)
			{
				this.m_decryptKeyExpansion[l] = this.m_encryptKeyExpansion[l];
				this.m_decryptKeyExpansion[this.m_Nb * this.m_Nr + l] = this.m_encryptKeyExpansion[this.m_Nb * this.m_Nr + l];
			}
			for (int m = this.m_Nb; m < this.m_Nb * this.m_Nr; m++)
			{
				int num8 = this.m_encryptKeyExpansion[m];
				int num9 = RijndaelManagedTransform.MulX(num8);
				int num10 = RijndaelManagedTransform.MulX(num9);
				int num11 = RijndaelManagedTransform.MulX(num10);
				int num12 = num8 ^ num11;
				this.m_decryptKeyExpansion[m] = (num9 ^ num10 ^ num11 ^ RijndaelManagedTransform.rot3(num9 ^ num12) ^ RijndaelManagedTransform.rot2(num10 ^ num12) ^ RijndaelManagedTransform.rot1(num12));
			}
		}

		// Token: 0x06002F9B RID: 12187 RVA: 0x000AAE28 File Offset: 0x000A9028
		private static int rot1(int val)
		{
			return (val << 8 & -256) | (val >> 24 & 255);
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x000AAE3E File Offset: 0x000A903E
		private static int rot2(int val)
		{
			return (val << 16 & -65536) | (val >> 16 & 65535);
		}

		// Token: 0x06002F9D RID: 12189 RVA: 0x000AAE55 File Offset: 0x000A9055
		private static int rot3(int val)
		{
			return (val << 24 & -16777216) | (val >> 8 & 16777215);
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x000AAE6C File Offset: 0x000A906C
		private static int SubWord(int a)
		{
			return (int)RijndaelManagedTransform.s_Sbox[a & 255] | (int)RijndaelManagedTransform.s_Sbox[a >> 8 & 255] << 8 | (int)RijndaelManagedTransform.s_Sbox[a >> 16 & 255] << 16 | (int)RijndaelManagedTransform.s_Sbox[a >> 24 & 255] << 24;
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x000AAEC0 File Offset: 0x000A90C0
		private static int MulX(int x)
		{
			int num = x & -2139062144;
			return (x & 2139062143) << 1 ^ (num - (num >> 7 & 33554431) & 454761243);
		}

		// Token: 0x06002FA1 RID: 12193 RVA: 0x000173AD File Offset: 0x000155AD
		internal RijndaelManagedTransform()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x0400219C RID: 8604
		private CipherMode m_cipherMode;

		// Token: 0x0400219D RID: 8605
		private PaddingMode m_paddingValue;

		// Token: 0x0400219E RID: 8606
		private RijndaelManagedTransformMode m_transformMode;

		// Token: 0x0400219F RID: 8607
		private int m_blockSizeBits;

		// Token: 0x040021A0 RID: 8608
		private int m_blockSizeBytes;

		// Token: 0x040021A1 RID: 8609
		private int m_inputBlockSize;

		// Token: 0x040021A2 RID: 8610
		private int m_outputBlockSize;

		// Token: 0x040021A3 RID: 8611
		private int[] m_encryptKeyExpansion;

		// Token: 0x040021A4 RID: 8612
		private int[] m_decryptKeyExpansion;

		// Token: 0x040021A5 RID: 8613
		private int m_Nr;

		// Token: 0x040021A6 RID: 8614
		private int m_Nb;

		// Token: 0x040021A7 RID: 8615
		private int m_Nk;

		// Token: 0x040021A8 RID: 8616
		private int[] m_encryptindex;

		// Token: 0x040021A9 RID: 8617
		private int[] m_decryptindex;

		// Token: 0x040021AA RID: 8618
		private int[] m_IV;

		// Token: 0x040021AB RID: 8619
		private int[] m_lastBlockBuffer;

		// Token: 0x040021AC RID: 8620
		private byte[] m_depadBuffer;

		// Token: 0x040021AD RID: 8621
		private byte[] m_shiftRegister;

		// Token: 0x040021AE RID: 8622
		private static readonly byte[] s_Sbox = new byte[]
		{
			99,
			124,
			119,
			123,
			242,
			107,
			111,
			197,
			48,
			1,
			103,
			43,
			254,
			215,
			171,
			118,
			202,
			130,
			201,
			125,
			250,
			89,
			71,
			240,
			173,
			212,
			162,
			175,
			156,
			164,
			114,
			192,
			183,
			253,
			147,
			38,
			54,
			63,
			247,
			204,
			52,
			165,
			229,
			241,
			113,
			216,
			49,
			21,
			4,
			199,
			35,
			195,
			24,
			150,
			5,
			154,
			7,
			18,
			128,
			226,
			235,
			39,
			178,
			117,
			9,
			131,
			44,
			26,
			27,
			110,
			90,
			160,
			82,
			59,
			214,
			179,
			41,
			227,
			47,
			132,
			83,
			209,
			0,
			237,
			32,
			252,
			177,
			91,
			106,
			203,
			190,
			57,
			74,
			76,
			88,
			207,
			208,
			239,
			170,
			251,
			67,
			77,
			51,
			133,
			69,
			249,
			2,
			127,
			80,
			60,
			159,
			168,
			81,
			163,
			64,
			143,
			146,
			157,
			56,
			245,
			188,
			182,
			218,
			33,
			16,
			byte.MaxValue,
			243,
			210,
			205,
			12,
			19,
			236,
			95,
			151,
			68,
			23,
			196,
			167,
			126,
			61,
			100,
			93,
			25,
			115,
			96,
			129,
			79,
			220,
			34,
			42,
			144,
			136,
			70,
			238,
			184,
			20,
			222,
			94,
			11,
			219,
			224,
			50,
			58,
			10,
			73,
			6,
			36,
			92,
			194,
			211,
			172,
			98,
			145,
			149,
			228,
			121,
			231,
			200,
			55,
			109,
			141,
			213,
			78,
			169,
			108,
			86,
			244,
			234,
			101,
			122,
			174,
			8,
			186,
			120,
			37,
			46,
			28,
			166,
			180,
			198,
			232,
			221,
			116,
			31,
			75,
			189,
			139,
			138,
			112,
			62,
			181,
			102,
			72,
			3,
			246,
			14,
			97,
			53,
			87,
			185,
			134,
			193,
			29,
			158,
			225,
			248,
			152,
			17,
			105,
			217,
			142,
			148,
			155,
			30,
			135,
			233,
			206,
			85,
			40,
			223,
			140,
			161,
			137,
			13,
			191,
			230,
			66,
			104,
			65,
			153,
			45,
			15,
			176,
			84,
			187,
			22
		};

		// Token: 0x040021AF RID: 8623
		private static readonly int[] s_Rcon = new int[]
		{
			1,
			2,
			4,
			8,
			16,
			32,
			64,
			128,
			27,
			54,
			108,
			216,
			171,
			77,
			154,
			47,
			94,
			188,
			99,
			198,
			151,
			53,
			106,
			212,
			179,
			125,
			250,
			239,
			197,
			145
		};

		// Token: 0x040021B0 RID: 8624
		private static readonly int[] s_T = new int[]
		{
			-1520213050,
			-2072216328,
			-1720223762,
			-1921287178,
			234025727,
			-1117033514,
			-1318096930,
			1422247313,
			1345335392,
			50397442,
			-1452841010,
			2099981142,
			436141799,
			1658312629,
			-424957107,
			-1703512340,
			1170918031,
			-1652391393,
			1086966153,
			-2021818886,
			368769775,
			-346465870,
			-918075506,
			200339707,
			-324162239,
			1742001331,
			-39673249,
			-357585083,
			-1080255453,
			-140204973,
			-1770884380,
			1539358875,
			-1028147339,
			486407649,
			-1366060227,
			1780885068,
			1513502316,
			1094664062,
			49805301,
			1338821763,
			1546925160,
			-190470831,
			887481809,
			150073849,
			-1821281822,
			1943591083,
			1395732834,
			1058346282,
			201589768,
			1388824469,
			1696801606,
			1589887901,
			672667696,
			-1583966665,
			251987210,
			-1248159185,
			151455502,
			907153956,
			-1686077413,
			1038279391,
			652995533,
			1764173646,
			-843926913,
			-1619692054,
			453576978,
			-1635548387,
			1949051992,
			773462580,
			756751158,
			-1301385508,
			-296068428,
			-73359269,
			-162377052,
			1295727478,
			1641469623,
			-827083907,
			2066295122,
			1055122397,
			1898917726,
			-1752923117,
			-179088474,
			1758581177,
			0,
			753790401,
			1612718144,
			536673507,
			-927878791,
			-312779850,
			-1100322092,
			1187761037,
			-641810841,
			1262041458,
			-565556588,
			-733197160,
			-396863312,
			1255133061,
			1808847035,
			720367557,
			-441800113,
			385612781,
			-985447546,
			-682799718,
			1429418854,
			-1803188975,
			-817543798,
			284817897,
			100794884,
			-2122350594,
			-263171936,
			1144798328,
			-1163944155,
			-475486133,
			-212774494,
			-22830243,
			-1069531008,
			-1970303227,
			-1382903233,
			-1130521311,
			1211644016,
			83228145,
			-541279133,
			-1044990345,
			1977277103,
			1663115586,
			806359072,
			452984805,
			250868733,
			1842533055,
			1288555905,
			336333848,
			890442534,
			804056259,
			-513843266,
			-1567123659,
			-867941240,
			957814574,
			1472513171,
			-223893675,
			-2105639172,
			1195195770,
			-1402706744,
			-413311558,
			723065138,
			-1787595802,
			-1604296512,
			-1736343271,
			-783331426,
			2145180835,
			1713513028,
			2116692564,
			-1416589253,
			-2088204277,
			-901364084,
			703524551,
			-742868885,
			1007948840,
			2044649127,
			-497131844,
			487262998,
			1994120109,
			1004593371,
			1446130276,
			1312438900,
			503974420,
			-615954030,
			168166924,
			1814307912,
			-463709000,
			1573044895,
			1859376061,
			-273896381,
			-1503501628,
			-1466855111,
			-1533700815,
			937747667,
			-1954973198,
			854058965,
			1137232011,
			1496790894,
			-1217565222,
			-1936880383,
			1691735473,
			-766620004,
			-525751991,
			-1267962664,
			-95005012,
			133494003,
			636152527,
			-1352309302,
			-1904575756,
			-374428089,
			403179536,
			-709182865,
			-2005370640,
			1864705354,
			1915629148,
			605822008,
			-240736681,
			-944458637,
			1371981463,
			602466507,
			2094914977,
			-1670089496,
			555687742,
			-582268010,
			-591544991,
			-2037675251,
			-2054518257,
			-1871679264,
			1111375484,
			-994724495,
			-1436129588,
			-666351472,
			84083462,
			32962295,
			302911004,
			-1553899070,
			1597322602,
			-111716434,
			-793134743,
			-1853454825,
			1489093017,
			656219450,
			-1180787161,
			954327513,
			335083755,
			-1281845205,
			856756514,
			-1150719534,
			1893325225,
			-1987146233,
			-1483434957,
			-1231316179,
			572399164,
			-1836611819,
			552200649,
			1238290055,
			-11184726,
			2015897680,
			2061492133,
			-1886614525,
			-123625127,
			-2138470135,
			386731290,
			-624967835,
			837215959,
			-968736124,
			-1201116976,
			-1019133566,
			-1332111063,
			1999449434,
			286199582,
			-877612933,
			-61582168,
			-692339859,
			974525996,
			1667483301,
			2088564868,
			2004348569,
			2071721613,
			-218956019,
			1802229437,
			1869602481,
			-976907948,
			808476752,
			16843267,
			1734856361,
			724260477,
			-16849127,
			-673729182,
			-1414836762,
			1987505306,
			-892694715,
			-2105401443,
			-909539008,
			2105408135,
			-84218091,
			1499050731,
			1195871945,
			-252642549,
			-1381154324,
			-724257945,
			-1566416899,
			-1347467798,
			-1667488833,
			-1532734473,
			1920132246,
			-1061119141,
			-1212713534,
			-33693412,
			-1819066962,
			640044138,
			909536346,
			1061125697,
			-134744830,
			-859012273,
			875849820,
			-1515892236,
			-437923532,
			-235800312,
			1903288979,
			-656888973,
			825320019,
			353708607,
			67373068,
			-943221422,
			589514341,
			-1010590370,
			404238376,
			-1768540255,
			84216335,
			-1701171275,
			117902857,
			303178806,
			-2139087973,
			-488448195,
			-336868058,
			656887401,
			-1296924723,
			1970662047,
			151589403,
			-2088559202,
			741103732,
			437924910,
			454768173,
			1852759218,
			1515893998,
			-1600103429,
			1381147894,
			993752653,
			-690571423,
			-1280082482,
			690573947,
			-471605954,
			791633521,
			-2071719017,
			1397991157,
			-774784664,
			0,
			-303185620,
			538984544,
			-50535649,
			-1313769016,
			1532737261,
			1785386174,
			-875852474,
			-1094817831,
			960066123,
			1246401758,
			1280088276,
			1482207464,
			-808483510,
			-791626901,
			-269499094,
			-1431679003,
			-67375850,
			1128498885,
			1296931543,
			859006549,
			-2054876780,
			1162185423,
			-101062384,
			33686534,
			2139094657,
			1347461360,
			1010595908,
			-1616960070,
			-1465365533,
			1364304627,
			-1549574658,
			1077969088,
			-1886452342,
			-1835909203,
			-1650646596,
			943222856,
			-168431356,
			-1128504353,
			-1229555775,
			-623202443,
			555827811,
			269492272,
			-6886,
			-202113778,
			-757940371,
			-842170036,
			202119188,
			320022069,
			-320027857,
			1600110305,
			-1751698014,
			1145342156,
			387395129,
			-993750185,
			-1482205710,
			2122251394,
			1027439175,
			1684326572,
			1566423783,
			421081643,
			1936975509,
			1616953504,
			-2122245736,
			1330618065,
			-589520001,
			572671078,
			707417214,
			-1869595733,
			-2004350077,
			1179028682,
			-286341335,
			-1195873325,
			336865340,
			-555833479,
			1583267042,
			185275933,
			-606360202,
			-522134725,
			842163286,
			976909390,
			168432670,
			1229558491,
			101059594,
			606357612,
			1549580516,
			-1027432611,
			-741098130,
			-1397996561,
			1650640038,
			-1852753496,
			-1785384540,
			-454765769,
			2038035083,
			-404237006,
			-926381245,
			926379609,
			1835915959,
			-1920138868,
			-707415708,
			1313774802,
			-1448523296,
			1819072692,
			1448520954,
			-185273593,
			-353710299,
			1701169839,
			2054878350,
			-1364310039,
			134746136,
			-1162186795,
			2021191816,
			623200879,
			774790258,
			471611428,
			-1499047951,
			-1263242297,
			-960063663,
			-387396829,
			-572677764,
			1953818780,
			522141217,
			1263245021,
			-1111662116,
			-1953821306,
			-1970663547,
			1886445712,
			1044282434,
			-1246400060,
			1718013098,
			1212715224,
			50529797,
			-151587071,
			235805714,
			1633796771,
			892693087,
			1465364217,
			-1179031088,
			-2038032495,
			-1044276904,
			488454695,
			-1633802311,
			-505292488,
			-117904621,
			-1734857805,
			286335539,
			1768542907,
			-640046736,
			-1903294583,
			-1802226777,
			-1684329034,
			505297954,
			-2021190254,
			-370554592,
			-825325751,
			1431677695,
			673730680,
			-538991238,
			-1936981105,
			-1583261192,
			-1987507840,
			218962455,
			-1077975590,
			-421079247,
			1111655622,
			1751699640,
			1094812355,
			-1718015568,
			757946999,
			252648977,
			-1330611253,
			1414834428,
			-1145344554,
			370551866,
			1673962851,
			2096661628,
			2012125559,
			2079755643,
			-218165774,
			1809235307,
			1876865391,
			-980331323,
			811618352,
			16909057,
			1741597031,
			727088427,
			-18408962,
			-675978537,
			-1420958037,
			1995217526,
			-896580150,
			-2111857278,
			-913751863,
			2113570685,
			-84994566,
			1504897881,
			1200539975,
			-251982864,
			-1388188499,
			-726439980,
			-1570767454,
			-1354372433,
			-1675378788,
			-1538000988,
			1927583346,
			-1063560256,
			-1217019209,
			-35578627,
			-1824674157,
			642542118,
			913070646,
			1065238847,
			-134937865,
			-863809588,
			879254580,
			-1521355611,
			-439274267,
			-235337487,
			1910674289,
			-659852328,
			828527409,
			355090197,
			67636228,
			-946515257,
			591815971,
			-1013096765,
			405809176,
			-1774739050,
			84545285,
			-1708149350,
			118360327,
			304363026,
			-2145674368,
			-488686110,
			-338876693,
			659450151,
			-1300247118,
			1978310517,
			152181513,
			-2095210877,
			743994412,
			439627290,
			456535323,
			1859957358,
			1521806938,
			-1604584544,
			1386542674,
			997608763,
			-692624938,
			-1283600717,
			693271337,
			-472039709,
			794718511,
			-2079090812,
			1403450707,
			-776378159,
			0,
			-306107155,
			541089824,
			-52224004,
			-1317418831,
			1538714971,
			1792327274,
			-879933749,
			-1100490306,
			963791673,
			1251270218,
			1285084236,
			1487988824,
			-813348145,
			-793023536,
			-272291089,
			-1437604438,
			-68348165,
			1132905795,
			1301993293,
			862344499,
			-2062445435,
			1166724933,
			-102166279,
			33818114,
			2147385727,
			1352724560,
			1014514748,
			-1624917345,
			-1471421528,
			1369633617,
			-1554121053,
			1082179648,
			-1895462257,
			-1841320558,
			-1658733411,
			946882616,
			-168753931,
			-1134305348,
			-1233665610,
			-626035238,
			557998881,
			270544912,
			-1762561,
			-201519373,
			-759206446,
			-847164211,
			202904588,
			321271059,
			-322752532,
			1606345055,
			-1758092649,
			1149815876,
			388905239,
			-996976700,
			-1487539545,
			2130477694,
			1031423805,
			1690872932,
			1572530013,
			422718233,
			1944491379,
			1623236704,
			-2129028991,
			1335808335,
			-593264676,
			574907938,
			710180394,
			-1875137648,
			-2012511352,
			1183631942,
			-288937490,
			-1200893000,
			338181140,
			-559449634,
			1589437022,
			185998603,
			-609388837,
			-522503200,
			845436466,
			980700730,
			169090570,
			1234361161,
			101452294,
			608726052,
			1555620956,
			-1029743166,
			-742560045,
			-1404833876,
			1657054818,
			-1858492271,
			-1791908715,
			-455919644,
			2045938553,
			-405458201,
			-930397240,
			929978679,
			1843050349,
			-1929278323,
			-709794603,
			1318900302,
			-1454776151,
			1826141292,
			1454176854,
			-185399308,
			-355523094,
			1707781989,
			2062847610,
			-1371018834,
			135272456,
			-1167075910,
			2029029496,
			625635109,
			777810478,
			473441308,
			-1504185946,
			-1267480652,
			-963161658,
			-389340184,
			-576619299,
			1961401460,
			524165407,
			1268178251,
			-1117659971,
			-1962047861,
			-1978694262,
			1893765232,
			1048330814,
			-1250835275,
			1724688998,
			1217452104,
			50726147,
			-151584266,
			236720654,
			1640145761,
			896163637,
			1471084887,
			-1184247623,
			-2045275770,
			-1046914879,
			490350365,
			-1641563746,
			-505857823,
			-118811656,
			-1741966440,
			287453969,
			1775418217,
			-643206951,
			-1912108658,
			-1808554092,
			-1691502949,
			507257374,
			-2028629369,
			-372694807,
			-829994546,
			1437269845,
			676362280,
			-542803233,
			-1945923700,
			-1587939167,
			-1995865975,
			219813645,
			-1083843905,
			-422104602,
			1115997762,
			1758509160,
			1099088705,
			-1725321063,
			760903469,
			253628687,
			-1334064208,
			1420360788,
			-1150429509,
			371997206,
			-962239645,
			-125535108,
			-291932297,
			-158499973,
			-15863054,
			-692229269,
			-558796945,
			-1856715323,
			1615867952,
			33751297,
			-827758745,
			1451043627,
			-417726722,
			-1251813417,
			1306962859,
			-325421450,
			-1891251510,
			530416258,
			-1992242743,
			-91783811,
			-283772166,
			-1293199015,
			-1899411641,
			-83103504,
			1106029997,
			-1285040940,
			1610457762,
			1173008303,
			599760028,
			1408738468,
			-459902350,
			-1688485696,
			1975695287,
			-518193667,
			1034851219,
			1282024998,
			1817851446,
			2118205247,
			-184354825,
			-2091922228,
			1750873140,
			1374987685,
			-785062427,
			-116854287,
			-493653647,
			-1418471208,
			1649619249,
			708777237,
			135005188,
			-1789737017,
			1181033251,
			-1654733885,
			807933976,
			933336726,
			168756485,
			800430746,
			235472647,
			607523346,
			463175808,
			-549592350,
			-853087253,
			1315514151,
			2144187058,
			-358648459,
			303761673,
			496927619,
			1484008492,
			875436570,
			908925723,
			-592286098,
			-1259447718,
			1543217312,
			-1527360942,
			1984772923,
			-1218324778,
			2110698419,
			1383803177,
			-583080989,
			1584475951,
			328696964,
			-1493871789,
			-1184312879,
			0,
			-1054020115,
			1080041504,
			-484442884,
			2043195825,
			-1225958565,
			-725718422,
			-1924740149,
			1742323390,
			1917532473,
			-1797371318,
			-1730917300,
			-1326950312,
			-2058694705,
			-1150562096,
			-987041809,
			1340451498,
			-317260805,
			-2033892541,
			-1697166003,
			1716859699,
			294946181,
			-1966127803,
			-384763399,
			67502594,
			-25067649,
			-1594863536,
			2017737788,
			632987551,
			1273211048,
			-1561112239,
			1576969123,
			-2134884288,
			92966799,
			1068339858,
			566009245,
			1883781176,
			-251333131,
			1675607228,
			2009183926,
			-1351230758,
			1113792801,
			540020752,
			-451215361,
			-49351693,
			-1083321646,
			-2125673011,
			403966988,
			641012499,
			-1020269332,
			-1092526241,
			899848087,
			-1999879100,
			775493399,
			-1822964540,
			1441965991,
			-58556802,
			2051489085,
			-928226204,
			-1159242403,
			841685273,
			-426413197,
			-1063231392,
			429425025,
			-1630449841,
			-1551901476,
			1147544098,
			1417554474,
			1001099408,
			193169544,
			-1932900794,
			-953553170,
			1809037496,
			675025940,
			-1485185314,
			-1126015394,
			371002123,
			-1384719397,
			-616832800,
			1683370546,
			1951283770,
			337512970,
			-1831122615,
			201983494,
			1215046692,
			-1192993700,
			-1621245246,
			-1116810285,
			1139780780,
			-995728798,
			967348625,
			832869781,
			-751311644,
			-225740423,
			-718084121,
			-1958491960,
			1851340599,
			-625513107,
			25988493,
			-1318791723,
			-1663938994,
			1239460265,
			-659264404,
			-1392880042,
			-217582348,
			-819598614,
			-894474907,
			-191989126,
			1206496942,
			270010376,
			1876277946,
			-259491720,
			1248797989,
			1550986798,
			941890588,
			1475454630,
			1942467764,
			-1756248378,
			-886839064,
			-1585652259,
			-392399756,
			1042358047,
			-1763882165,
			1641856445,
			226921355,
			260409994,
			-527404944,
			2084716094,
			1908716981,
			-861247898,
			-1864873912,
			100991747,
			-150866186,
			470945294,
			-1029480095,
			1784624437,
			-1359390889,
			1775286713,
			395413126,
			-1722236479,
			975641885,
			666476190,
			-650583583,
			-351012616,
			733190296,
			573772049,
			-759469719,
			-1452221991,
			126455438,
			866620564,
			766942107,
			1008868894,
			361924487,
			-920589847,
			-2025206066,
			-1426107051,
			1350051880,
			-1518673953,
			59739276,
			1509466529,
			159418761,
			437718285,
			1708834751,
			-684595482,
			-2067381694,
			-793221016,
			-2101132991,
			699439513,
			1517759789,
			504434447,
			2076946608,
			-1459858348,
			1842789307,
			742004246
		};

		// Token: 0x040021B1 RID: 8625
		private static readonly int[] s_TF = new int[]
		{
			99,
			124,
			119,
			123,
			242,
			107,
			111,
			197,
			48,
			1,
			103,
			43,
			254,
			215,
			171,
			118,
			202,
			130,
			201,
			125,
			250,
			89,
			71,
			240,
			173,
			212,
			162,
			175,
			156,
			164,
			114,
			192,
			183,
			253,
			147,
			38,
			54,
			63,
			247,
			204,
			52,
			165,
			229,
			241,
			113,
			216,
			49,
			21,
			4,
			199,
			35,
			195,
			24,
			150,
			5,
			154,
			7,
			18,
			128,
			226,
			235,
			39,
			178,
			117,
			9,
			131,
			44,
			26,
			27,
			110,
			90,
			160,
			82,
			59,
			214,
			179,
			41,
			227,
			47,
			132,
			83,
			209,
			0,
			237,
			32,
			252,
			177,
			91,
			106,
			203,
			190,
			57,
			74,
			76,
			88,
			207,
			208,
			239,
			170,
			251,
			67,
			77,
			51,
			133,
			69,
			249,
			2,
			127,
			80,
			60,
			159,
			168,
			81,
			163,
			64,
			143,
			146,
			157,
			56,
			245,
			188,
			182,
			218,
			33,
			16,
			255,
			243,
			210,
			205,
			12,
			19,
			236,
			95,
			151,
			68,
			23,
			196,
			167,
			126,
			61,
			100,
			93,
			25,
			115,
			96,
			129,
			79,
			220,
			34,
			42,
			144,
			136,
			70,
			238,
			184,
			20,
			222,
			94,
			11,
			219,
			224,
			50,
			58,
			10,
			73,
			6,
			36,
			92,
			194,
			211,
			172,
			98,
			145,
			149,
			228,
			121,
			231,
			200,
			55,
			109,
			141,
			213,
			78,
			169,
			108,
			86,
			244,
			234,
			101,
			122,
			174,
			8,
			186,
			120,
			37,
			46,
			28,
			166,
			180,
			198,
			232,
			221,
			116,
			31,
			75,
			189,
			139,
			138,
			112,
			62,
			181,
			102,
			72,
			3,
			246,
			14,
			97,
			53,
			87,
			185,
			134,
			193,
			29,
			158,
			225,
			248,
			152,
			17,
			105,
			217,
			142,
			148,
			155,
			30,
			135,
			233,
			206,
			85,
			40,
			223,
			140,
			161,
			137,
			13,
			191,
			230,
			66,
			104,
			65,
			153,
			45,
			15,
			176,
			84,
			187,
			22,
			25344,
			31744,
			30464,
			31488,
			61952,
			27392,
			28416,
			50432,
			12288,
			256,
			26368,
			11008,
			65024,
			55040,
			43776,
			30208,
			51712,
			33280,
			51456,
			32000,
			64000,
			22784,
			18176,
			61440,
			44288,
			54272,
			41472,
			44800,
			39936,
			41984,
			29184,
			49152,
			46848,
			64768,
			37632,
			9728,
			13824,
			16128,
			63232,
			52224,
			13312,
			42240,
			58624,
			61696,
			28928,
			55296,
			12544,
			5376,
			1024,
			50944,
			8960,
			49920,
			6144,
			38400,
			1280,
			39424,
			1792,
			4608,
			32768,
			57856,
			60160,
			9984,
			45568,
			29952,
			2304,
			33536,
			11264,
			6656,
			6912,
			28160,
			23040,
			40960,
			20992,
			15104,
			54784,
			45824,
			10496,
			58112,
			12032,
			33792,
			21248,
			53504,
			0,
			60672,
			8192,
			64512,
			45312,
			23296,
			27136,
			51968,
			48640,
			14592,
			18944,
			19456,
			22528,
			52992,
			53248,
			61184,
			43520,
			64256,
			17152,
			19712,
			13056,
			34048,
			17664,
			63744,
			512,
			32512,
			20480,
			15360,
			40704,
			43008,
			20736,
			41728,
			16384,
			36608,
			37376,
			40192,
			14336,
			62720,
			48128,
			46592,
			55808,
			8448,
			4096,
			65280,
			62208,
			53760,
			52480,
			3072,
			4864,
			60416,
			24320,
			38656,
			17408,
			5888,
			50176,
			42752,
			32256,
			15616,
			25600,
			23808,
			6400,
			29440,
			24576,
			33024,
			20224,
			56320,
			8704,
			10752,
			36864,
			34816,
			17920,
			60928,
			47104,
			5120,
			56832,
			24064,
			2816,
			56064,
			57344,
			12800,
			14848,
			2560,
			18688,
			1536,
			9216,
			23552,
			49664,
			54016,
			44032,
			25088,
			37120,
			38144,
			58368,
			30976,
			59136,
			51200,
			14080,
			27904,
			36096,
			54528,
			19968,
			43264,
			27648,
			22016,
			62464,
			59904,
			25856,
			31232,
			44544,
			2048,
			47616,
			30720,
			9472,
			11776,
			7168,
			42496,
			46080,
			50688,
			59392,
			56576,
			29696,
			7936,
			19200,
			48384,
			35584,
			35328,
			28672,
			15872,
			46336,
			26112,
			18432,
			768,
			62976,
			3584,
			24832,
			13568,
			22272,
			47360,
			34304,
			49408,
			7424,
			40448,
			57600,
			63488,
			38912,
			4352,
			26880,
			55552,
			36352,
			37888,
			39680,
			7680,
			34560,
			59648,
			52736,
			21760,
			10240,
			57088,
			35840,
			41216,
			35072,
			3328,
			48896,
			58880,
			16896,
			26624,
			16640,
			39168,
			11520,
			3840,
			45056,
			21504,
			47872,
			5632,
			6488064,
			8126464,
			7798784,
			8060928,
			15859712,
			7012352,
			7274496,
			12910592,
			3145728,
			65536,
			6750208,
			2818048,
			16646144,
			14090240,
			11206656,
			7733248,
			13238272,
			8519680,
			13172736,
			8192000,
			16384000,
			5832704,
			4653056,
			15728640,
			11337728,
			13893632,
			10616832,
			11468800,
			10223616,
			10747904,
			7471104,
			12582912,
			11993088,
			16580608,
			9633792,
			2490368,
			3538944,
			4128768,
			16187392,
			13369344,
			3407872,
			10813440,
			15007744,
			15794176,
			7405568,
			14155776,
			3211264,
			1376256,
			262144,
			13041664,
			2293760,
			12779520,
			1572864,
			9830400,
			327680,
			10092544,
			458752,
			1179648,
			8388608,
			14811136,
			15400960,
			2555904,
			11665408,
			7667712,
			589824,
			8585216,
			2883584,
			1703936,
			1769472,
			7208960,
			5898240,
			10485760,
			5373952,
			3866624,
			14024704,
			11730944,
			2686976,
			14876672,
			3080192,
			8650752,
			5439488,
			13697024,
			0,
			15532032,
			2097152,
			16515072,
			11599872,
			5963776,
			6946816,
			13303808,
			12451840,
			3735552,
			4849664,
			4980736,
			5767168,
			13565952,
			13631488,
			15663104,
			11141120,
			16449536,
			4390912,
			5046272,
			3342336,
			8716288,
			4521984,
			16318464,
			131072,
			8323072,
			5242880,
			3932160,
			10420224,
			11010048,
			5308416,
			10682368,
			4194304,
			9371648,
			9568256,
			10289152,
			3670016,
			16056320,
			12320768,
			11927552,
			14286848,
			2162688,
			1048576,
			16711680,
			15925248,
			13762560,
			13434880,
			786432,
			1245184,
			15466496,
			6225920,
			9895936,
			4456448,
			1507328,
			12845056,
			10944512,
			8257536,
			3997696,
			6553600,
			6094848,
			1638400,
			7536640,
			6291456,
			8454144,
			5177344,
			14417920,
			2228224,
			2752512,
			9437184,
			8912896,
			4587520,
			15597568,
			12058624,
			1310720,
			14548992,
			6160384,
			720896,
			14352384,
			14680064,
			3276800,
			3801088,
			655360,
			4784128,
			393216,
			2359296,
			6029312,
			12713984,
			13828096,
			11272192,
			6422528,
			9502720,
			9764864,
			14942208,
			7929856,
			15138816,
			13107200,
			3604480,
			7143424,
			9240576,
			13959168,
			5111808,
			11075584,
			7077888,
			5636096,
			15990784,
			15335424,
			6619136,
			7995392,
			11403264,
			524288,
			12189696,
			7864320,
			2424832,
			3014656,
			1835008,
			10878976,
			11796480,
			12976128,
			15204352,
			14483456,
			7602176,
			2031616,
			4915200,
			12386304,
			9109504,
			9043968,
			7340032,
			4063232,
			11862016,
			6684672,
			4718592,
			196608,
			16121856,
			917504,
			6356992,
			3473408,
			5701632,
			12124160,
			8781824,
			12648448,
			1900544,
			10354688,
			14745600,
			16252928,
			9961472,
			1114112,
			6881280,
			14221312,
			9306112,
			9699328,
			10158080,
			1966080,
			8847360,
			15269888,
			13500416,
			5570560,
			2621440,
			14614528,
			9175040,
			10551296,
			8978432,
			851968,
			12517376,
			15073280,
			4325376,
			6815744,
			4259840,
			10027008,
			2949120,
			983040,
			11534336,
			5505024,
			12255232,
			1441792,
			1660944384,
			2080374784,
			1996488704,
			2063597568,
			-234881024,
			1795162112,
			1862270976,
			-989855744,
			805306368,
			16777216,
			1728053248,
			721420288,
			-33554432,
			-687865856,
			-1426063360,
			1979711488,
			-905969664,
			-2113929216,
			-922746880,
			2097152000,
			-100663296,
			1493172224,
			1191182336,
			-268435456,
			-1392508928,
			-738197504,
			-1577058304,
			-1358954496,
			-1677721600,
			-1543503872,
			1912602624,
			-1073741824,
			-1224736768,
			-50331648,
			-1828716544,
			637534208,
			905969664,
			1056964608,
			-150994944,
			-872415232,
			872415232,
			-1526726656,
			-452984832,
			-251658240,
			1895825408,
			-671088640,
			822083584,
			352321536,
			67108864,
			-956301312,
			587202560,
			-1023410176,
			402653184,
			-1778384896,
			83886080,
			-1711276032,
			117440512,
			301989888,
			int.MinValue,
			-503316480,
			-352321536,
			654311424,
			-1308622848,
			1962934272,
			150994944,
			-2097152000,
			738197504,
			436207616,
			452984832,
			1845493760,
			1509949440,
			-1610612736,
			1375731712,
			989855744,
			-704643072,
			-1291845632,
			687865856,
			-486539264,
			788529152,
			-2080374784,
			1392508928,
			-788529152,
			0,
			-318767104,
			536870912,
			-67108864,
			-1325400064,
			1526726656,
			1778384896,
			-889192448,
			-1107296256,
			956301312,
			1241513984,
			1275068416,
			1476395008,
			-822083584,
			-805306368,
			-285212672,
			-1442840576,
			-83886080,
			1124073472,
			1291845632,
			855638016,
			-2063597568,
			1157627904,
			-117440512,
			33554432,
			2130706432,
			1342177280,
			1006632960,
			-1627389952,
			-1476395008,
			1358954496,
			-1560281088,
			1073741824,
			-1895825408,
			-1845493760,
			-1660944384,
			939524096,
			-184549376,
			-1140850688,
			-1241513984,
			-637534208,
			553648128,
			268435456,
			-16777216,
			-218103808,
			-771751936,
			-855638016,
			201326592,
			318767104,
			-335544320,
			1593835520,
			-1761607680,
			1140850688,
			385875968,
			-1006632960,
			-1493172224,
			2113929216,
			1023410176,
			1677721600,
			1560281088,
			419430400,
			1929379840,
			1610612736,
			-2130706432,
			1325400064,
			-603979776,
			570425344,
			704643072,
			-1879048192,
			-2013265920,
			1174405120,
			-301989888,
			-1207959552,
			335544320,
			-570425344,
			1577058304,
			184549376,
			-620756992,
			-536870912,
			838860800,
			973078528,
			167772160,
			1224736768,
			100663296,
			603979776,
			1543503872,
			-1040187392,
			-754974720,
			-1409286144,
			1644167168,
			-1862270976,
			-1795162112,
			-469762048,
			2030043136,
			-419430400,
			-939524096,
			922746880,
			1828716544,
			-1929379840,
			-721420288,
			1308622848,
			-1459617792,
			1811939328,
			1442840576,
			-201326592,
			-369098752,
			1694498816,
			2046820352,
			-1375731712,
			134217728,
			-1174405120,
			2013265920,
			620756992,
			771751936,
			469762048,
			-1509949440,
			-1275068416,
			-973078528,
			-402653184,
			-587202560,
			1946157056,
			520093696,
			1258291200,
			-1124073472,
			-1962934272,
			-1979711488,
			1879048192,
			1040187392,
			-1258291200,
			1711276032,
			1207959552,
			50331648,
			-167772160,
			234881024,
			1627389952,
			889192448,
			1459617792,
			-1191182336,
			-2046820352,
			-1056964608,
			486539264,
			-1644167168,
			-520093696,
			-134217728,
			-1744830464,
			285212672,
			1761607680,
			-654311424,
			-1912602624,
			-1811939328,
			-1694498816,
			503316480,
			-2030043136,
			-385875968,
			-838860800,
			1426063360,
			671088640,
			-553648128,
			-1946157056,
			-1593835520,
			-1996488704,
			218103808,
			-1090519040,
			-436207616,
			1107296256,
			1744830464,
			1090519040,
			-1728053248,
			754974720,
			251658240,
			-1342177280,
			1409286144,
			-1157627904,
			369098752
		};

		// Token: 0x040021B2 RID: 8626
		private static readonly int[] s_iT = new int[]
		{
			1353184337,
			1399144830,
			-1012656358,
			-1772214470,
			-882136261,
			-247096033,
			-1420232020,
			-1828461749,
			1442459680,
			-160598355,
			-1854485368,
			625738485,
			-52959921,
			-674551099,
			-2143013594,
			-1885117771,
			1230680542,
			1729870373,
			-1743852987,
			-507445667,
			41234371,
			317738113,
			-1550367091,
			-956705941,
			-413167869,
			-1784901099,
			-344298049,
			-631680363,
			763608788,
			-752782248,
			694804553,
			1154009486,
			1787413109,
			2021232372,
			1799248025,
			-579749593,
			-1236278850,
			397248752,
			1722556617,
			-1271214467,
			407560035,
			-2110711067,
			1613975959,
			1165972322,
			-529046351,
			-2068943941,
			480281086,
			-1809118983,
			1483229296,
			436028815,
			-2022908268,
			-1208452270,
			601060267,
			-503166094,
			1468997603,
			715871590,
			120122290,
			63092015,
			-1703164538,
			-1526188077,
			-226023376,
			-1297760477,
			-1167457534,
			1552029421,
			723308426,
			-1833666137,
			-252573709,
			-1578997426,
			-839591323,
			-708967162,
			526529745,
			-1963022652,
			-1655493068,
			-1604979806,
			853641733,
			1978398372,
			971801355,
			-1427152832,
			111112542,
			1360031421,
			-108388034,
			1023860118,
			-1375387939,
			1186850381,
			-1249028975,
			90031217,
			1876166148,
			-15380384,
			620468249,
			-1746289194,
			-868007799,
			2006899047,
			-1119688528,
			-2004121337,
			945494503,
			-605108103,
			1191869601,
			-384875908,
			-920746760,
			0,
			-2088337399,
			1223502642,
			-1401941730,
			1316117100,
			-67170563,
			1446544655,
			517320253,
			658058550,
			1691946762,
			564550760,
			-783000677,
			976107044,
			-1318647284,
			266819475,
			-761860428,
			-1634624741,
			1338359936,
			-1574904735,
			1766553434,
			370807324,
			179999714,
			-450191168,
			1138762300,
			488053522,
			185403662,
			-1379431438,
			-1180125651,
			-928440812,
			-2061897385,
			1275557295,
			-1143105042,
			-44007517,
			-1624899081,
			-1124765092,
			-985962940,
			880737115,
			1982415755,
			-590994485,
			1761406390,
			1676797112,
			-891538985,
			277177154,
			1076008723,
			538035844,
			2099530373,
			-130171950,
			288553390,
			1839278535,
			1261411869,
			-214912292,
			-330136051,
			-790380169,
			1813426987,
			-1715900247,
			-95906799,
			577038663,
			-997393240,
			440397984,
			-668172970,
			-275762398,
			-951170681,
			-1043253031,
			-22885748,
			906744984,
			-813566554,
			685669029,
			646887386,
			-1530942145,
			-459458004,
			227702864,
			-1681105046,
			1648787028,
			-1038905866,
			-390539120,
			1593260334,
			-173030526,
			-1098883681,
			2090061929,
			-1456614033,
			-1290656305,
			999926984,
			-1484974064,
			1852021992,
			2075868123,
			158869197,
			-199730834,
			28809964,
			-1466282109,
			1701746150,
			2129067946,
			147831841,
			-420997649,
			-644094022,
			-835293366,
			-737566742,
			-696471511,
			-1347247055,
			824393514,
			815048134,
			-1067015627,
			935087732,
			-1496677636,
			-1328508704,
			366520115,
			1251476721,
			-136647615,
			240176511,
			804688151,
			-1915335306,
			1303441219,
			1414376140,
			-553347356,
			-474623586,
			461924940,
			-1205916479,
			2136040774,
			82468509,
			1563790337,
			1937016826,
			776014843,
			1511876531,
			1389550482,
			861278441,
			323475053,
			-1939744870,
			2047648055,
			-1911228327,
			-1992551445,
			-299390514,
			902390199,
			-303751967,
			1018251130,
			1507840668,
			1064563285,
			2043548696,
			-1086863501,
			-355600557,
			1537932639,
			342834655,
			-2032450440,
			-2114736182,
			1053059257,
			741614648,
			1598071746,
			1925389590,
			203809468,
			-1958134744,
			1100287487,
			1895934009,
			-558691320,
			-1662733096,
			-1866377628,
			1636092795,
			1890988757,
			1952214088,
			1113045200,
			-1477160624,
			1698790995,
			-1541989693,
			1579629206,
			1806384075,
			1167925233,
			1492823211,
			65227667,
			-97509291,
			1836494326,
			1993115793,
			1275262245,
			-672837636,
			-886389289,
			1144333952,
			-1553812081,
			1521606217,
			465184103,
			250234264,
			-1057071647,
			1966064386,
			-263421678,
			-1756983901,
			-103584826,
			1603208167,
			-1668147819,
			2054012907,
			1498584538,
			-2084645843,
			561273043,
			1776306473,
			-926314940,
			-1983744662,
			2039411832,
			1045993835,
			1907959773,
			1340194486,
			-1383534569,
			-1407137434,
			986611124,
			1256153880,
			823846274,
			860985184,
			2136171077,
			2003087840,
			-1368671356,
			-1602093540,
			722008468,
			1749577816,
			-45773031,
			1826526343,
			-126135625,
			-747394269,
			38499042,
			-1893735593,
			-1420466646,
			686535175,
			-1028313341,
			2076542618,
			137876389,
			-2027409166,
			-1514200142,
			1778582202,
			-2112426660,
			483363371,
			-1267095662,
			-234359824,
			-496415071,
			-187013683,
			-1106966827,
			1647628575,
			-22625142,
			1395537053,
			1442030240,
			-511048398,
			-336157579,
			-326956231,
			-278904662,
			-1619960314,
			275692881,
			-1977532679,
			115185213,
			88006062,
			-1108980410,
			-1923837515,
			1573155077,
			-737803153,
			357589247,
			-73918172,
			-373434729,
			1128303052,
			-1629919369,
			1122545853,
			-1953953912,
			1528424248,
			-288851493,
			175939911,
			256015593,
			512030921,
			0,
			-2038429309,
			-315936184,
			1880170156,
			1918528590,
			-15794693,
			948244310,
			-710001378,
			959264295,
			-653325724,
			-1503893471,
			1415289809,
			775300154,
			1728711857,
			-413691121,
			-1762741038,
			-1852105826,
			-977239985,
			551313826,
			1266113129,
			437394454,
			-1164713462,
			715178213,
			-534627261,
			387650077,
			218697227,
			-947129683,
			-1464455751,
			-1457646392,
			435246981,
			125153100,
			-577114437,
			1618977789,
			637663135,
			-177054532,
			996558021,
			2130402100,
			692292470,
			-970732580,
			-51530136,
			-236668829,
			-600713270,
			-2057092592,
			580326208,
			298222624,
			608863613,
			1035719416,
			855223825,
			-1591097491,
			798891339,
			817028339,
			1384517100,
			-473860144,
			380840812,
			-1183798887,
			1217663482,
			1693009698,
			-1929598780,
			1072734234,
			746411736,
			-1875696913,
			1313441735,
			-784803391,
			-1563783938,
			198481974,
			-2114607409,
			-562387672,
			-1900553690,
			-1079165020,
			-1657131804,
			-1837608947,
			-866162021,
			1182684258,
			328070850,
			-1193766680,
			-147247522,
			-1346141451,
			-2141347906,
			-1815058052,
			768962473,
			304467891,
			-1716729797,
			2098729127,
			1671227502,
			-1153705093,
			2015808777,
			408514292,
			-1214583807,
			-1706064984,
			1855317605,
			-419452290,
			-809754360,
			-401215514,
			-1679312167,
			913263310,
			161475284,
			2091919830,
			-1297862225,
			591342129,
			-1801075152,
			1721906624,
			-1135709129,
			-897385306,
			-795811664,
			-660131051,
			-1744506550,
			-622050825,
			1355644686,
			-158263505,
			-699566451,
			-1326496947,
			1303039060,
			76997855,
			-1244553501,
			-2006299621,
			523026872,
			1365591679,
			-362898172,
			898367837,
			1955068531,
			1091304238,
			493335386,
			-757362094,
			1443948851,
			1205234963,
			1641519756,
			211892090,
			351820174,
			1007938441,
			665439982,
			-916342987,
			-451091987,
			-1320715716,
			-539845543,
			1945261375,
			-837543815,
			935818175,
			-839429142,
			-1426235557,
			1866325780,
			-616269690,
			-206583167,
			-999769794,
			874788908,
			1084473951,
			-1021503886,
			635616268,
			1228679307,
			-1794244799,
			27801969,
			-1291056930,
			-457910116,
			-1051302768,
			-2067039391,
			-1238182544,
			1550600308,
			1471729730,
			-195997529,
			1098797925,
			387629988,
			658151006,
			-1422144661,
			-1658851003,
			-89347240,
			-481586429,
			807425530,
			1991112301,
			-863465098,
			49620300,
			-447742761,
			717608907,
			891715652,
			1656065955,
			-1310832294,
			-1171953893,
			-364537842,
			-27401792,
			801309301,
			1283527408,
			1183687575,
			-747911431,
			-1895569569,
			-1844079204,
			1841294202,
			1385552473,
			-1093390973,
			1951978273,
			-532076183,
			-913423160,
			-1032492407,
			-1896580999,
			1486449470,
			-1188569743,
			-507595185,
			-1997531219,
			550069932,
			-830622662,
			-547153846,
			451248689,
			1368875059,
			1398949247,
			1689378935,
			1807451310,
			-2114052960,
			150574123,
			1215322216,
			1167006205,
			-560691348,
			2069018616,
			1940595667,
			1265820162,
			534992783,
			1432758955,
			-340654296,
			-1255210046,
			-981034373,
			936617224,
			674296455,
			-1088179547,
			50510442,
			384654466,
			-813028580,
			2041025204,
			133427442,
			1766760930,
			-630862348,
			84334014,
			886120290,
			-1497068802,
			775200083,
			-207445931,
			-1979370783,
			-156994069,
			-2096416276,
			1614850799,
			1901987487,
			1857900816,
			557775242,
			-577356538,
			1054715397,
			-431143235,
			1418835341,
			-999226019,
			100954068,
			1348534037,
			-1743182597,
			-1110009879,
			1082772547,
			-647530594,
			-391070398,
			-1995994997,
			434583643,
			-931537938,
			2090944266,
			1115482383,
			-2064070370,
			0,
			-2146860154,
			724715757,
			287222896,
			1517047410,
			251526143,
			-2062592456,
			-1371726123,
			758523705,
			252339417,
			1550328230,
			1536938324,
			908343854,
			168604007,
			1469255655,
			-290139498,
			-1692688751,
			-1065332795,
			-597581280,
			2002413899,
			303830554,
			-1813902662,
			-1597971158,
			574374880,
			454171927,
			151915277,
			-1947030073,
			-1238517336,
			504678569,
			-245922535,
			1974422535,
			-1712407587,
			2141453664,
			33005350,
			1918680309,
			1715782971,
			-77908866,
			1133213225,
			600562886,
			-306812676,
			-457677839,
			836225756,
			1665273989,
			-1760346078,
			-964419567,
			1250262308,
			-1143801795,
			-106032846,
			700935585,
			-1642247377,
			-1294142672,
			-2045907886,
			-1049112349,
			-1288999914,
			1890163129,
			-1810761144,
			-381214108,
			-56048500,
			-257942977,
			2102843436,
			857927568,
			1233635150,
			953795025,
			-896729438,
			-728222197,
			-173617279,
			2057644254,
			-1210440050,
			-1388337985,
			976020637,
			2018512274,
			1600822220,
			2119459398,
			-1913208301,
			-661591880,
			959340279,
			-1014827601,
			1570750080,
			-798393197,
			-714102483,
			634368786,
			-1396163687,
			403744637,
			-1662488989,
			1004239803,
			650971512,
			1500443672,
			-1695809097,
			1334028442,
			-1780062866,
			-5603610,
			-1138685745,
			368043752,
			-407184997,
			1867173430,
			-1612000247,
			-1339435396,
			-1540247630,
			1059729699,
			-1513738092,
			-1573535642,
			1316239292,
			-2097371446,
			-1864322864,
			-1489824296,
			82922136,
			-331221030,
			-847311280,
			-1860751370,
			1299615190,
			-280801872,
			-1429449651,
			-1763385596,
			-778116171,
			1783372680,
			750893087,
			1699118929,
			1587348714,
			-1946067659,
			-2013629580,
			201010753,
			1739807261,
			-611167534,
			283718486,
			-697494713,
			-677737375,
			-1590199796,
			-128348652,
			334203196,
			-1446056409,
			1639396809,
			484568549,
			1199193265,
			-761505313,
			-229294221,
			337148366,
			-948715721,
			-145495347,
			-44082262,
			1038029935,
			1148749531,
			-1345682957,
			1756970692,
			607661108,
			-1547542720,
			488010435,
			-490992603,
			1009290057,
			234832277,
			-1472630527,
			201907891,
			-1260872476,
			1449431233,
			-881106556,
			852848822,
			1816687708,
			-1194311081,
			1364240372,
			2119394625,
			449029143,
			982933031,
			1003187115,
			535905693,
			-1398056710,
			1267925987,
			542505520,
			-1376359050,
			-2003732788,
			-182105086,
			1341970405,
			-975713494,
			645940277,
			-1248877726,
			-565617999,
			627514298,
			1167593194,
			1575076094,
			-1023249105,
			-2129465268,
			-1918658746,
			1808202195,
			65494927,
			362126482,
			-1075086739,
			-1780852398,
			-735214658,
			1490231668,
			1227450848,
			-1908094775,
			1969916354,
			-193431154,
			-1721024936,
			668823993,
			-1095348255,
			-266883704,
			-916018144,
			2108963534,
			1662536415,
			-444452582,
			-1755303087,
			1648721747,
			-1310689436,
			-1148932501,
			-31678335,
			-107730168,
			1884842056,
			-1894122171,
			-1803064098,
			1387788411,
			-1423715469,
			1927414347,
			-480800993,
			1714072405,
			-1308153621,
			788775605,
			-2036696123,
			-744159177,
			821200680,
			598910399,
			45771267,
			-312704490,
			-1976886065,
			-1483557767,
			-202313209,
			1319232105,
			1707996378,
			114671109,
			-786472396,
			-997523802,
			882725678,
			-1566550541,
			87220618,
			-1535775754,
			188345475,
			1084944224,
			1577492337,
			-1118760850,
			1056541217,
			-1774385443,
			-575797954,
			1296481766,
			-1850372780,
			1896177092,
			74437638,
			1627329872,
			421854104,
			-694687299,
			-1983102144,
			1735892697,
			-1329773848,
			126389129,
			-415737063,
			2044456648,
			-1589179780,
			2095648578,
			-121037180,
			0,
			159614592,
			843640107,
			514617361,
			1817080410,
			-33816818,
			257308805,
			1025430958,
			908540205,
			174381327,
			1747035740,
			-1680780197,
			607792694,
			212952842,
			-1827674281,
			-1261267218,
			463376795,
			-2142255680,
			1638015196,
			1516850039,
			471210514,
			-502613357,
			-1058723168,
			1011081250,
			303896347,
			235605257,
			-223492213,
			767142070,
			348694814,
			1468340721,
			-1353971851,
			-289677927,
			-1543675777,
			-140564991,
			1555887474,
			1153776486,
			1530167035,
			-1955190461,
			-874723805,
			-1234633491,
			-1201409564,
			-674571215,
			1108378979,
			322970263,
			-2078273082,
			-2055396278,
			-755483205,
			-1374604551,
			-949116631,
			491466654,
			-588042062,
			233591430,
			2010178497,
			728503987,
			-1449543312,
			301615252,
			1193436393,
			-1463513860,
			-1608892432,
			1457007741,
			586125363,
			-2016981431,
			-641609416,
			-1929469238,
			-1741288492,
			-1496350219,
			-1524048262,
			-635007305,
			1067761581,
			753179962,
			1343066744,
			1788595295,
			1415726718,
			-155053171,
			-1863796520,
			777975609,
			-2097827901,
			-1614905251,
			1769771984,
			1873358293,
			-810347995,
			-935618132,
			279411992,
			-395418724,
			-612648133,
			-855017434,
			1861490777,
			-335431782,
			-2086102449,
			-429560171,
			-1434523905,
			554225596,
			-270079979,
			-1160143897,
			1255028335,
			-355202657,
			701922480,
			833598116,
			707863359,
			-969894747,
			901801634,
			1949809742,
			-56178046,
			-525283184,
			857069735,
			-246769660,
			1106762476,
			2131644621,
			389019281,
			1989006925,
			1129165039,
			-866890326,
			-455146346,
			-1629243951,
			1276872810,
			-1044898004,
			1182749029,
			-1660622242,
			22885772,
			-93096825,
			-80854773,
			-1285939865,
			-1840065829,
			-382511600,
			1829980118,
			-1702075945,
			930745505,
			1502483704,
			-343327725,
			-823253079,
			-1221211807,
			-504503012,
			2050797895,
			-1671831598,
			1430221810,
			410635796,
			1941911495,
			1407897079,
			1599843069,
			-552308931,
			2022103876,
			-897453137,
			-1187068824,
			942421028,
			-1033944925,
			376619805,
			-1140054558,
			680216892,
			-12479219,
			963707304,
			148812556,
			-660806476,
			1687208278,
			2069988555,
			-714033614,
			1215585388,
			-800958536
		};

		// Token: 0x040021B3 RID: 8627
		private static readonly int[] s_iTF = new int[]
		{
			82,
			9,
			106,
			213,
			48,
			54,
			165,
			56,
			191,
			64,
			163,
			158,
			129,
			243,
			215,
			251,
			124,
			227,
			57,
			130,
			155,
			47,
			255,
			135,
			52,
			142,
			67,
			68,
			196,
			222,
			233,
			203,
			84,
			123,
			148,
			50,
			166,
			194,
			35,
			61,
			238,
			76,
			149,
			11,
			66,
			250,
			195,
			78,
			8,
			46,
			161,
			102,
			40,
			217,
			36,
			178,
			118,
			91,
			162,
			73,
			109,
			139,
			209,
			37,
			114,
			248,
			246,
			100,
			134,
			104,
			152,
			22,
			212,
			164,
			92,
			204,
			93,
			101,
			182,
			146,
			108,
			112,
			72,
			80,
			253,
			237,
			185,
			218,
			94,
			21,
			70,
			87,
			167,
			141,
			157,
			132,
			144,
			216,
			171,
			0,
			140,
			188,
			211,
			10,
			247,
			228,
			88,
			5,
			184,
			179,
			69,
			6,
			208,
			44,
			30,
			143,
			202,
			63,
			15,
			2,
			193,
			175,
			189,
			3,
			1,
			19,
			138,
			107,
			58,
			145,
			17,
			65,
			79,
			103,
			220,
			234,
			151,
			242,
			207,
			206,
			240,
			180,
			230,
			115,
			150,
			172,
			116,
			34,
			231,
			173,
			53,
			133,
			226,
			249,
			55,
			232,
			28,
			117,
			223,
			110,
			71,
			241,
			26,
			113,
			29,
			41,
			197,
			137,
			111,
			183,
			98,
			14,
			170,
			24,
			190,
			27,
			252,
			86,
			62,
			75,
			198,
			210,
			121,
			32,
			154,
			219,
			192,
			254,
			120,
			205,
			90,
			244,
			31,
			221,
			168,
			51,
			136,
			7,
			199,
			49,
			177,
			18,
			16,
			89,
			39,
			128,
			236,
			95,
			96,
			81,
			127,
			169,
			25,
			181,
			74,
			13,
			45,
			229,
			122,
			159,
			147,
			201,
			156,
			239,
			160,
			224,
			59,
			77,
			174,
			42,
			245,
			176,
			200,
			235,
			187,
			60,
			131,
			83,
			153,
			97,
			23,
			43,
			4,
			126,
			186,
			119,
			214,
			38,
			225,
			105,
			20,
			99,
			85,
			33,
			12,
			125,
			20992,
			2304,
			27136,
			54528,
			12288,
			13824,
			42240,
			14336,
			48896,
			16384,
			41728,
			40448,
			33024,
			62208,
			55040,
			64256,
			31744,
			58112,
			14592,
			33280,
			39680,
			12032,
			65280,
			34560,
			13312,
			36352,
			17152,
			17408,
			50176,
			56832,
			59648,
			51968,
			21504,
			31488,
			37888,
			12800,
			42496,
			49664,
			8960,
			15616,
			60928,
			19456,
			38144,
			2816,
			16896,
			64000,
			49920,
			19968,
			2048,
			11776,
			41216,
			26112,
			10240,
			55552,
			9216,
			45568,
			30208,
			23296,
			41472,
			18688,
			27904,
			35584,
			53504,
			9472,
			29184,
			63488,
			62976,
			25600,
			34304,
			26624,
			38912,
			5632,
			54272,
			41984,
			23552,
			52224,
			23808,
			25856,
			46592,
			37376,
			27648,
			28672,
			18432,
			20480,
			64768,
			60672,
			47360,
			55808,
			24064,
			5376,
			17920,
			22272,
			42752,
			36096,
			40192,
			33792,
			36864,
			55296,
			43776,
			0,
			35840,
			48128,
			54016,
			2560,
			63232,
			58368,
			22528,
			1280,
			47104,
			45824,
			17664,
			1536,
			53248,
			11264,
			7680,
			36608,
			51712,
			16128,
			3840,
			512,
			49408,
			44800,
			48384,
			768,
			256,
			4864,
			35328,
			27392,
			14848,
			37120,
			4352,
			16640,
			20224,
			26368,
			56320,
			59904,
			38656,
			61952,
			52992,
			52736,
			61440,
			46080,
			58880,
			29440,
			38400,
			44032,
			29696,
			8704,
			59136,
			44288,
			13568,
			34048,
			57856,
			63744,
			14080,
			59392,
			7168,
			29952,
			57088,
			28160,
			18176,
			61696,
			6656,
			28928,
			7424,
			10496,
			50432,
			35072,
			28416,
			46848,
			25088,
			3584,
			43520,
			6144,
			48640,
			6912,
			64512,
			22016,
			15872,
			19200,
			50688,
			53760,
			30976,
			8192,
			39424,
			56064,
			49152,
			65024,
			30720,
			52480,
			23040,
			62464,
			7936,
			56576,
			43008,
			13056,
			34816,
			1792,
			50944,
			12544,
			45312,
			4608,
			4096,
			22784,
			9984,
			32768,
			60416,
			24320,
			24576,
			20736,
			32512,
			43264,
			6400,
			46336,
			18944,
			3328,
			11520,
			58624,
			31232,
			40704,
			37632,
			51456,
			39936,
			61184,
			40960,
			57344,
			15104,
			19712,
			44544,
			10752,
			62720,
			45056,
			51200,
			60160,
			47872,
			15360,
			33536,
			21248,
			39168,
			24832,
			5888,
			11008,
			1024,
			32256,
			47616,
			30464,
			54784,
			9728,
			57600,
			26880,
			5120,
			25344,
			21760,
			8448,
			3072,
			32000,
			5373952,
			589824,
			6946816,
			13959168,
			3145728,
			3538944,
			10813440,
			3670016,
			12517376,
			4194304,
			10682368,
			10354688,
			8454144,
			15925248,
			14090240,
			16449536,
			8126464,
			14876672,
			3735552,
			8519680,
			10158080,
			3080192,
			16711680,
			8847360,
			3407872,
			9306112,
			4390912,
			4456448,
			12845056,
			14548992,
			15269888,
			13303808,
			5505024,
			8060928,
			9699328,
			3276800,
			10878976,
			12713984,
			2293760,
			3997696,
			15597568,
			4980736,
			9764864,
			720896,
			4325376,
			16384000,
			12779520,
			5111808,
			524288,
			3014656,
			10551296,
			6684672,
			2621440,
			14221312,
			2359296,
			11665408,
			7733248,
			5963776,
			10616832,
			4784128,
			7143424,
			9109504,
			13697024,
			2424832,
			7471104,
			16252928,
			16121856,
			6553600,
			8781824,
			6815744,
			9961472,
			1441792,
			13893632,
			10747904,
			6029312,
			13369344,
			6094848,
			6619136,
			11927552,
			9568256,
			7077888,
			7340032,
			4718592,
			5242880,
			16580608,
			15532032,
			12124160,
			14286848,
			6160384,
			1376256,
			4587520,
			5701632,
			10944512,
			9240576,
			10289152,
			8650752,
			9437184,
			14155776,
			11206656,
			0,
			9175040,
			12320768,
			13828096,
			655360,
			16187392,
			14942208,
			5767168,
			327680,
			12058624,
			11730944,
			4521984,
			393216,
			13631488,
			2883584,
			1966080,
			9371648,
			13238272,
			4128768,
			983040,
			131072,
			12648448,
			11468800,
			12386304,
			196608,
			65536,
			1245184,
			9043968,
			7012352,
			3801088,
			9502720,
			1114112,
			4259840,
			5177344,
			6750208,
			14417920,
			15335424,
			9895936,
			15859712,
			13565952,
			13500416,
			15728640,
			11796480,
			15073280,
			7536640,
			9830400,
			11272192,
			7602176,
			2228224,
			15138816,
			11337728,
			3473408,
			8716288,
			14811136,
			16318464,
			3604480,
			15204352,
			1835008,
			7667712,
			14614528,
			7208960,
			4653056,
			15794176,
			1703936,
			7405568,
			1900544,
			2686976,
			12910592,
			8978432,
			7274496,
			11993088,
			6422528,
			917504,
			11141120,
			1572864,
			12451840,
			1769472,
			16515072,
			5636096,
			4063232,
			4915200,
			12976128,
			13762560,
			7929856,
			2097152,
			10092544,
			14352384,
			12582912,
			16646144,
			7864320,
			13434880,
			5898240,
			15990784,
			2031616,
			14483456,
			11010048,
			3342336,
			8912896,
			458752,
			13041664,
			3211264,
			11599872,
			1179648,
			1048576,
			5832704,
			2555904,
			8388608,
			15466496,
			6225920,
			6291456,
			5308416,
			8323072,
			11075584,
			1638400,
			11862016,
			4849664,
			851968,
			2949120,
			15007744,
			7995392,
			10420224,
			9633792,
			13172736,
			10223616,
			15663104,
			10485760,
			14680064,
			3866624,
			5046272,
			11403264,
			2752512,
			16056320,
			11534336,
			13107200,
			15400960,
			12255232,
			3932160,
			8585216,
			5439488,
			10027008,
			6356992,
			1507328,
			2818048,
			262144,
			8257536,
			12189696,
			7798784,
			14024704,
			2490368,
			14745600,
			6881280,
			1310720,
			6488064,
			5570560,
			2162688,
			786432,
			8192000,
			1375731712,
			150994944,
			1778384896,
			-721420288,
			805306368,
			905969664,
			-1526726656,
			939524096,
			-1090519040,
			1073741824,
			-1560281088,
			-1644167168,
			-2130706432,
			-218103808,
			-687865856,
			-83886080,
			2080374784,
			-486539264,
			956301312,
			-2113929216,
			-1694498816,
			788529152,
			-16777216,
			-2030043136,
			872415232,
			-1912602624,
			1124073472,
			1140850688,
			-1006632960,
			-570425344,
			-385875968,
			-889192448,
			1409286144,
			2063597568,
			-1811939328,
			838860800,
			-1509949440,
			-1040187392,
			587202560,
			1023410176,
			-301989888,
			1275068416,
			-1795162112,
			184549376,
			1107296256,
			-100663296,
			-1023410176,
			1308622848,
			134217728,
			771751936,
			-1593835520,
			1711276032,
			671088640,
			-654311424,
			603979776,
			-1308622848,
			1979711488,
			1526726656,
			-1577058304,
			1224736768,
			1828716544,
			-1962934272,
			-788529152,
			620756992,
			1912602624,
			-134217728,
			-167772160,
			1677721600,
			-2046820352,
			1744830464,
			-1744830464,
			369098752,
			-738197504,
			-1543503872,
			1543503872,
			-872415232,
			1560281088,
			1694498816,
			-1241513984,
			-1845493760,
			1811939328,
			1879048192,
			1207959552,
			1342177280,
			-50331648,
			-318767104,
			-1191182336,
			-637534208,
			1577058304,
			352321536,
			1174405120,
			1459617792,
			-1493172224,
			-1929379840,
			-1660944384,
			-2080374784,
			-1879048192,
			-671088640,
			-1426063360,
			0,
			-1946157056,
			-1140850688,
			-754974720,
			167772160,
			-150994944,
			-469762048,
			1476395008,
			83886080,
			-1207959552,
			-1291845632,
			1157627904,
			100663296,
			-805306368,
			738197504,
			503316480,
			-1895825408,
			-905969664,
			1056964608,
			251658240,
			33554432,
			-1056964608,
			-1358954496,
			-1124073472,
			50331648,
			16777216,
			318767104,
			-1979711488,
			1795162112,
			973078528,
			-1862270976,
			285212672,
			1090519040,
			1325400064,
			1728053248,
			-603979776,
			-369098752,
			-1761607680,
			-234881024,
			-822083584,
			-838860800,
			-268435456,
			-1275068416,
			-436207616,
			1929379840,
			-1778384896,
			-1409286144,
			1946157056,
			570425344,
			-419430400,
			-1392508928,
			889192448,
			-2063597568,
			-503316480,
			-117440512,
			922746880,
			-402653184,
			469762048,
			1962934272,
			-553648128,
			1845493760,
			1191182336,
			-251658240,
			436207616,
			1895825408,
			486539264,
			687865856,
			-989855744,
			-1996488704,
			1862270976,
			-1224736768,
			1644167168,
			234881024,
			-1442840576,
			402653184,
			-1107296256,
			452984832,
			-67108864,
			1442840576,
			1040187392,
			1258291200,
			-973078528,
			-771751936,
			2030043136,
			536870912,
			-1711276032,
			-620756992,
			-1073741824,
			-33554432,
			2013265920,
			-855638016,
			1509949440,
			-201326592,
			520093696,
			-587202560,
			-1476395008,
			855638016,
			-2013265920,
			117440512,
			-956301312,
			822083584,
			-1325400064,
			301989888,
			268435456,
			1493172224,
			654311424,
			int.MinValue,
			-335544320,
			1593835520,
			1610612736,
			1358954496,
			2130706432,
			-1459617792,
			419430400,
			-1258291200,
			1241513984,
			218103808,
			754974720,
			-452984832,
			2046820352,
			-1627389952,
			-1828716544,
			-922746880,
			-1677721600,
			-285212672,
			-1610612736,
			-536870912,
			989855744,
			1291845632,
			-1375731712,
			704643072,
			-184549376,
			-1342177280,
			-939524096,
			-352321536,
			-1157627904,
			1006632960,
			-2097152000,
			1392508928,
			-1728053248,
			1627389952,
			385875968,
			721420288,
			67108864,
			2113929216,
			-1174405120,
			1996488704,
			-704643072,
			637534208,
			-520093696,
			1761607680,
			335544320,
			1660944384,
			1426063360,
			553648128,
			201326592,
			2097152000
		};
	}
}
