using System;
using System.Collections.Generic;
using System.Resources;

namespace FxResources.System.Threading.Channels
{
	internal static class SR
	{
	}
}


namespace CaoNC.System
{
	internal static class SR
	{
		private static readonly bool s_usingResourceKeys = AppContext.TryGetSwitch("System.Resources.UseSystemResourceKeys", out var isEnabled) && isEnabled;

		private static ResourceManager s_resourceManager;

		internal static ResourceManager ResourceManager => s_resourceManager ?? (s_resourceManager = new ResourceManager(typeof(FxResources.System.Threading.Channels.SR)));

		internal static string ChannelClosedException_DefaultMessage => GetResourceString("ChannelClosedException_DefaultMessage");

		internal static string InvalidOperation_IncompleteAsyncOperation => GetResourceString("InvalidOperation_IncompleteAsyncOperation");

		internal static string InvalidOperation_MultipleContinuations => GetResourceString("InvalidOperation_MultipleContinuations");

		internal static string InvalidOperation_IncorrectToken => GetResourceString("InvalidOperation_IncorrectToken");

        internal static string NotSupported_CannotCallEqualsOnSpan => GetResourceString("NotSupported_CannotCallEqualsOnSpan", null);

        internal static string NotSupported_CannotCallGetHashCodeOnSpan => GetResourceString("NotSupported_CannotCallGetHashCodeOnSpan", null);

        internal static string Argument_InvalidTypeWithPointersNotSupported => GetResourceString("Argument_InvalidTypeWithPointersNotSupported", null);

        internal static string Argument_DestinationTooShort => GetResourceString("Argument_DestinationTooShort", null);

        internal static string Argument_OverlapAlignmentMismatch => GetResourceString("Argument_OverlapAlignmentMismatch", null);

        internal static string UnexpectedNumberOfNamedParameters => GetResourceString("UnexpectedNumberOfNamedParameters");

        internal static string CacheEntryHasEmptySize => GetResourceString("CacheEntryHasEmptySize");

        internal static string NonKeyedDescriptorMisuse => GetResourceString("NonKeyedDescriptorMisuse");

        internal static string TryAddIndistinguishableTypeToEnumerable => GetResourceString("TryAddIndistinguishableTypeToEnumerable");

        internal static string NoServiceRegistered => GetResourceString("NoServiceRegistered");

        internal static string Error_NoConfigurationServicesAndAction => GetResourceString("Error_NoConfigurationServicesAndAction");
		
		internal static string Error_NoConfigurationServices => GetResourceString("Error_NoConfigurationServices");

        internal static string Arg_ArgumentOutOfRangeException => GetResourceString("Arg_ArgumentOutOfRangeException", null);

        internal static string Arg_ElementsInSourceIsGreaterThanDestination => GetResourceString("Arg_ElementsInSourceIsGreaterThanDestination", null);

        internal static string Arg_NullArgumentNullRef => GetResourceString("Arg_NullArgumentNullRef", null);

        internal static string Arg_TypeNotSupported => GetResourceString("Arg_TypeNotSupported", null);

        internal static string Arg_InsufficientNumberOfElements => GetResourceString("Arg_InsufficientNumberOfElements", null);

        internal static string ArgumentException_BufferNotFromPool => GetResourceString("ArgumentException_BufferNotFromPool", null);

        internal static bool UsingResourceKeys()
		{
			return s_usingResourceKeys;
		}

		private static string GetResourceString(string resourceKey)
		{
			if (UsingResourceKeys())
			{
				return resourceKey;
			}
			string result = null;
			try
			{
				result = ResourceManager.GetString(resourceKey);
				return result;
			}
			catch (MissingManifestResourceException)
			{
				return result;
			}
		}

		private static string GetResourceString(string resourceKey, string defaultString)
		{
			string resourceString = GetResourceString(resourceKey);
			if (!(resourceKey == resourceString) && resourceString != null)
			{
				return resourceString;
			}
			return defaultString;
		}

		internal static string Format(string resourceFormat, object p1)
		{
			if (UsingResourceKeys())
			{
				return string.Join(", ", resourceFormat, p1);
			}
			return string.Format(resourceFormat, p1);
		}

		internal static string Format(string resourceFormat, object p1, object p2)
		{
			if (UsingResourceKeys())
			{
				return string.Join(", ", resourceFormat, p1, p2);
			}
			return string.Format(resourceFormat, p1, p2);
		}

		internal static string Format(string resourceFormat, object p1, object p2, object p3)
		{
			if (UsingResourceKeys())
			{
				return string.Join(", ", resourceFormat, p1, p2, p3);
			}
			return string.Format(resourceFormat, p1, p2, p3);
		}

		internal static string Format(string resourceFormat, params object[] args)
		{
			if (args != null)
			{
				if (UsingResourceKeys())
				{
					return resourceFormat + ", " + string.Join(", ", args);
				}
				return string.Format(resourceFormat, args);
			}
			return resourceFormat;
		}

		internal static string Format(IFormatProvider provider, string resourceFormat, object p1)
		{
			if (UsingResourceKeys())
			{
				return string.Join(", ", resourceFormat, p1);
			}
			return string.Format(provider, resourceFormat, p1);
		}

		internal static string Format(IFormatProvider provider, string resourceFormat, object p1, object p2)
		{
			if (UsingResourceKeys())
			{
				return string.Join(", ", resourceFormat, p1, p2);
			}
			return string.Format(provider, resourceFormat, p1, p2);
		}

		internal static string Format(IFormatProvider provider, string resourceFormat, object p1, object p2, object p3)
		{
			if (UsingResourceKeys())
			{
				return string.Join(", ", resourceFormat, p1, p2, p3);
			}
			return string.Format(provider, resourceFormat, p1, p2, p3);
		}

		internal static string Format(IFormatProvider provider, string resourceFormat, params object[] args)
		{
			if (args != null)
			{
				if (UsingResourceKeys())
				{
					return resourceFormat + ", " + string.Join(", ", args);
				}
				return string.Format(provider, resourceFormat, args);
			}
			return resourceFormat;
		}
    }
}
