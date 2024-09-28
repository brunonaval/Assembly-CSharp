﻿using System;

namespace System.Security.Policy
{
	// Token: 0x02000417 RID: 1047
	internal sealed class MembershipConditionHelper
	{
		// Token: 0x06002AC2 RID: 10946 RVA: 0x0009A6C0 File Offset: 0x000988C0
		internal static int CheckSecurityElement(SecurityElement se, string parameterName, int minimumVersion, int maximumVersion)
		{
			if (se == null)
			{
				throw new ArgumentNullException(parameterName);
			}
			if (se.Tag != MembershipConditionHelper.XmlTag)
			{
				throw new ArgumentException(string.Format(Locale.GetText("Invalid tag {0}, expected {1}."), se.Tag, MembershipConditionHelper.XmlTag), parameterName);
			}
			int result = minimumVersion;
			string text = se.Attribute("version");
			if (text != null)
			{
				try
				{
					result = int.Parse(text);
				}
				catch (Exception innerException)
				{
					throw new ArgumentException(string.Format(Locale.GetText("Couldn't parse version from '{0}'."), text), parameterName, innerException);
				}
			}
			return result;
		}

		// Token: 0x06002AC3 RID: 10947 RVA: 0x0009A750 File Offset: 0x00098950
		internal static SecurityElement Element(Type type, int version)
		{
			SecurityElement securityElement = new SecurityElement(MembershipConditionHelper.XmlTag);
			securityElement.AddAttribute("class", type.FullName + ", " + type.Assembly.ToString().Replace('"', '\''));
			securityElement.AddAttribute("version", version.ToString());
			return securityElement;
		}

		// Token: 0x04001F9F RID: 8095
		private static readonly string XmlTag = "IMembershipCondition";
	}
}
