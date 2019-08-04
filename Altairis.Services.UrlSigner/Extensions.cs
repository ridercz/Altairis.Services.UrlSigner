using System;

namespace Altairis.Services.UrlSigner {
    public static class Extensions {

        public static TimedUrlSigner ToTimedSigner(this IUrlSigner signer) {
            if (signer == null) throw new ArgumentNullException(nameof(signer));
            return new TimedUrlSigner(signer);
        }

        internal static string RemoveLastParameter(this string url, string paramName, out string paramValue) {
            if (url == null) throw new ArgumentNullException(nameof(url));
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(url));
            if (paramName == null) throw new ArgumentNullException(nameof(paramName));
            if (string.IsNullOrWhiteSpace(paramName)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(paramName));

            var nameLength = paramName.Length;

            var lastSeparatorIndex = url.LastIndexOfAny("?&".ToCharArray());
            if (lastSeparatorIndex < 1 || lastSeparatorIndex > url.Length - (nameLength + 3) || !url.Substring(lastSeparatorIndex + 1, nameLength + 1).Equals(paramName + "=", StringComparison.OrdinalIgnoreCase)) throw new FormatException("Invalid URL format");

            paramValue = url.Substring(lastSeparatorIndex + nameLength + 2);
            return url.Substring(0, lastSeparatorIndex);
        }
    }
}
