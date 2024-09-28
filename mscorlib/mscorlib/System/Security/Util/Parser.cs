﻿using System;
using System.IO;
using System.Text;

namespace System.Security.Util
{
	// Token: 0x020003EE RID: 1006
	internal sealed class Parser
	{
		// Token: 0x06002983 RID: 10627 RVA: 0x00096844 File Offset: 0x00094A44
		internal SecurityElement GetTopElement()
		{
			return this._doc.GetRootElement();
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x00096854 File Offset: 0x00094A54
		private void GetRequiredSizes(TokenizerStream stream, ref int index)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			int num = 1;
			SecurityElementType securityElementType = SecurityElementType.Regular;
			string text = null;
			bool flag5 = false;
			bool flag6 = false;
			int num2 = 0;
			for (;;)
			{
				short nextToken = stream.GetNextToken();
				while (nextToken != -1)
				{
					switch (nextToken & 255)
					{
					case 0:
						flag4 = true;
						flag6 = false;
						nextToken = stream.GetNextToken();
						if (nextToken == 2)
						{
							stream.TagLastToken(17408);
							for (;;)
							{
								nextToken = stream.GetNextToken();
								if (nextToken != 3)
								{
									break;
								}
								stream.ThrowAwayNextString();
								stream.TagLastToken(20480);
							}
							if (nextToken == -1)
							{
								goto Block_9;
							}
							if (nextToken != 1)
							{
								goto Block_10;
							}
							flag4 = false;
							index++;
							flag6 = false;
							num--;
							flag = true;
							goto IL_3B9;
						}
						else if (nextToken == 3)
						{
							flag3 = true;
							stream.TagLastToken(16640);
							index += SecurityDocument.EncodedStringSize(stream.GetNextString()) + 1;
							if (securityElementType != SecurityElementType.Regular)
							{
								goto Block_12;
							}
							flag = true;
							num++;
							goto IL_3B9;
						}
						else
						{
							if (nextToken == 6)
							{
								num2 = 1;
								do
								{
									nextToken = stream.GetNextToken();
									switch (nextToken)
									{
									case 0:
										num2++;
										break;
									case 1:
										num2--;
										break;
									case 3:
										stream.ThrowAwayNextString();
										stream.TagLastToken(20480);
										break;
									}
								}
								while (num2 > 0);
								flag4 = false;
								flag6 = false;
								flag = true;
								goto IL_3B9;
							}
							if (nextToken != 5)
							{
								goto IL_2B3;
							}
							nextToken = stream.GetNextToken();
							if (nextToken != 3)
							{
								goto Block_17;
							}
							flag3 = true;
							securityElementType = SecurityElementType.Format;
							stream.TagLastToken(16640);
							index += SecurityDocument.EncodedStringSize(stream.GetNextString()) + 1;
							num2 = 1;
							num++;
							flag = true;
							goto IL_3B9;
						}
						break;
					case 1:
						if (flag4)
						{
							flag4 = false;
							goto IL_3C4;
						}
						goto IL_2E0;
					case 2:
						nextToken = stream.GetNextToken();
						if (nextToken == 1)
						{
							stream.TagLastToken(17408);
							index++;
							num--;
							flag6 = false;
							flag = true;
							goto IL_3B9;
						}
						goto IL_329;
					case 3:
						if (flag4)
						{
							if (securityElementType == SecurityElementType.Comment)
							{
								stream.ThrowAwayNextString();
								stream.TagLastToken(20480);
								goto IL_3B9;
							}
							if (text == null)
							{
								text = stream.GetNextString();
								goto IL_3B9;
							}
							if (!flag5)
							{
								goto Block_5;
							}
							stream.TagLastToken(16896);
							index += SecurityDocument.EncodedStringSize(text) + SecurityDocument.EncodedStringSize(stream.GetNextString()) + 1;
							text = null;
							flag5 = false;
							goto IL_3B9;
						}
						else
						{
							if (flag6)
							{
								stream.TagLastToken(25344);
								index += SecurityDocument.EncodedStringSize(stream.GetNextString()) + SecurityDocument.EncodedStringSize(" ");
								goto IL_3B9;
							}
							stream.TagLastToken(17152);
							index += SecurityDocument.EncodedStringSize(stream.GetNextString()) + 1;
							flag6 = true;
							goto IL_3B9;
						}
						break;
					case 4:
						flag5 = true;
						goto IL_3B9;
					case 5:
						if (!flag4 || securityElementType != SecurityElementType.Format || num2 != 1)
						{
							goto IL_397;
						}
						nextToken = stream.GetNextToken();
						if (nextToken == 1)
						{
							stream.TagLastToken(17408);
							index++;
							num--;
							flag6 = false;
							flag = true;
							goto IL_3B9;
						}
						goto IL_37C;
					}
					goto Block_1;
					IL_3C4:
					nextToken = stream.GetNextToken();
					continue;
					IL_3B9:
					if (flag)
					{
						flag = false;
						flag2 = false;
						break;
					}
					flag2 = true;
					goto IL_3C4;
				}
				if (flag2)
				{
					index++;
					num--;
					flag6 = false;
				}
				else if (nextToken == -1 && (num != 1 || !flag3))
				{
					goto IL_3F5;
				}
				if (num <= 1)
				{
					return;
				}
			}
			Block_1:
			goto IL_3A8;
			Block_5:
			throw new XmlSyntaxException(this._t.LineNo);
			Block_9:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("Unexpected end of file."));
			Block_10:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("Expected > character."));
			Block_12:
			throw new XmlSyntaxException(this._t.LineNo);
			Block_17:
			throw new XmlSyntaxException(this._t.LineNo);
			IL_2B3:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("Expected / character or string."));
			IL_2E0:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("Unexpected > character."));
			IL_329:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("Expected > character."));
			IL_37C:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("Expected > character."));
			IL_397:
			throw new XmlSyntaxException(this._t.LineNo);
			IL_3A8:
			throw new XmlSyntaxException(this._t.LineNo);
			IL_3F5:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("Unexpected end of file."));
		}

		// Token: 0x06002985 RID: 10629 RVA: 0x00096C7C File Offset: 0x00094E7C
		private int DetermineFormat(TokenizerStream stream)
		{
			if (stream.GetNextToken() == 0 && stream.GetNextToken() == 5)
			{
				this._t.GetTokens(stream, -1, true);
				stream.GoToPosition(2);
				bool flag = false;
				bool flag2 = false;
				short nextToken = stream.GetNextToken();
				while (nextToken != -1 && nextToken != 1)
				{
					if (nextToken != 3)
					{
						if (nextToken != 4)
						{
							throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("Unexpected end of file."));
						}
						flag = true;
					}
					else
					{
						if (flag && flag2)
						{
							this._t.ChangeFormat(Encoding.GetEncoding(stream.GetNextString()));
							return 0;
						}
						if (!flag)
						{
							if (string.Compare(stream.GetNextString(), "encoding", StringComparison.Ordinal) == 0)
							{
								flag2 = true;
							}
						}
						else
						{
							flag = false;
							flag2 = false;
							stream.ThrowAwayNextString();
						}
					}
					nextToken = stream.GetNextToken();
				}
				return 0;
			}
			return 2;
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x00096D40 File Offset: 0x00094F40
		private void ParseContents()
		{
			TokenizerStream tokenizerStream = new TokenizerStream();
			this._t.GetTokens(tokenizerStream, 2, false);
			tokenizerStream.Reset();
			int position = this.DetermineFormat(tokenizerStream);
			tokenizerStream.GoToPosition(position);
			this._t.GetTokens(tokenizerStream, -1, false);
			tokenizerStream.Reset();
			int numData = 0;
			this.GetRequiredSizes(tokenizerStream, ref numData);
			this._doc = new SecurityDocument(numData);
			int num = 0;
			tokenizerStream.Reset();
			for (short nextFullToken = tokenizerStream.GetNextFullToken(); nextFullToken != -1; nextFullToken = tokenizerStream.GetNextFullToken())
			{
				if ((nextFullToken & 16384) == 16384)
				{
					short num2 = (short)((int)nextFullToken & 65280);
					if (num2 <= 17152)
					{
						if (num2 == 16640)
						{
							this._doc.AddToken(1, ref num);
							this._doc.AddString(tokenizerStream.GetNextString(), ref num);
							goto IL_19D;
						}
						if (num2 == 16896)
						{
							this._doc.AddToken(2, ref num);
							this._doc.AddString(tokenizerStream.GetNextString(), ref num);
							this._doc.AddString(tokenizerStream.GetNextString(), ref num);
							goto IL_19D;
						}
						if (num2 == 17152)
						{
							this._doc.AddToken(3, ref num);
							this._doc.AddString(tokenizerStream.GetNextString(), ref num);
							goto IL_19D;
						}
					}
					else
					{
						if (num2 == 17408)
						{
							this._doc.AddToken(4, ref num);
							goto IL_19D;
						}
						if (num2 == 20480)
						{
							tokenizerStream.ThrowAwayNextString();
							goto IL_19D;
						}
						if (num2 == 25344)
						{
							this._doc.AppendString(" ", ref num);
							this._doc.AppendString(tokenizerStream.GetNextString(), ref num);
							goto IL_19D;
						}
					}
					throw new XmlSyntaxException();
				}
				IL_19D:;
			}
		}

		// Token: 0x06002987 RID: 10631 RVA: 0x00096EF8 File Offset: 0x000950F8
		private Parser(Tokenizer t)
		{
			this._t = t;
			this._doc = null;
			try
			{
				this.ParseContents();
			}
			finally
			{
				this._t.Recycle();
			}
		}

		// Token: 0x06002988 RID: 10632 RVA: 0x00096F40 File Offset: 0x00095140
		internal Parser(string input) : this(new Tokenizer(input))
		{
		}

		// Token: 0x06002989 RID: 10633 RVA: 0x00096F4E File Offset: 0x0009514E
		internal Parser(string input, string[] searchStrings, string[] replaceStrings) : this(new Tokenizer(input, searchStrings, replaceStrings))
		{
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x00096F5E File Offset: 0x0009515E
		internal Parser(byte[] array, Tokenizer.ByteTokenEncoding encoding) : this(new Tokenizer(array, encoding, 0))
		{
		}

		// Token: 0x0600298B RID: 10635 RVA: 0x00096F6E File Offset: 0x0009516E
		internal Parser(byte[] array, Tokenizer.ByteTokenEncoding encoding, int startIndex) : this(new Tokenizer(array, encoding, startIndex))
		{
		}

		// Token: 0x0600298C RID: 10636 RVA: 0x00096F7E File Offset: 0x0009517E
		internal Parser(StreamReader input) : this(new Tokenizer(input))
		{
		}

		// Token: 0x0600298D RID: 10637 RVA: 0x00096F8C File Offset: 0x0009518C
		internal Parser(char[] array) : this(new Tokenizer(array))
		{
		}

		// Token: 0x04001EFB RID: 7931
		private SecurityDocument _doc;

		// Token: 0x04001EFC RID: 7932
		private Tokenizer _t;

		// Token: 0x04001EFD RID: 7933
		private const short c_flag = 16384;

		// Token: 0x04001EFE RID: 7934
		private const short c_elementtag = 16640;

		// Token: 0x04001EFF RID: 7935
		private const short c_attributetag = 16896;

		// Token: 0x04001F00 RID: 7936
		private const short c_texttag = 17152;

		// Token: 0x04001F01 RID: 7937
		private const short c_additionaltexttag = 25344;

		// Token: 0x04001F02 RID: 7938
		private const short c_childrentag = 17408;

		// Token: 0x04001F03 RID: 7939
		private const short c_wastedstringtag = 20480;
	}
}
