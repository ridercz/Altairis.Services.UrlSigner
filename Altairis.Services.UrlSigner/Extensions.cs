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

            var baseUrl = url.RemoveFragment(out var fragment);

            var lastSeparatorIndex = baseUrl.LastIndexOfAny("?&;".ToCharArray());
            if (lastSeparatorIndex < 1 || lastSeparatorIndex > url.Length - (nameLength + 3) || !url.Substring(lastSeparatorIndex + 1, nameLength + 1).Equals(paramName + "=", StringComparison.OrdinalIgnoreCase)) throw new FormatException("Invalid URL format");

            paramValue = baseUrl.Substring(lastSeparatorIndex + nameLength + 2);
            var shorterUrl = baseUrl.Substring(0, lastSeparatorIndex);

            if (string.IsNullOrEmpty(fragment)) {
                return shorterUrl;
            }
            else {
                return shorterUrl + fragment;
            }
        }

        internal static string AppendParameter(this string url, string paramName, string paramValue) {
            if (url == null) throw new ArgumentNullException(nameof(url));
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(url));
            if (paramName == null) throw new ArgumentNullException(nameof(paramName));
            if (string.IsNullOrWhiteSpace(paramName)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(paramName));

            var baseUrl = RemoveFragment(url, out var fragment);
            var separator = baseUrl.Contains("?") ? "&" : "?";
            return $"{baseUrl}{separator}{paramName}={paramValue}{fragment}";
        }

        internal static string RemoveFragment(this string url) => RemoveFragment(url, out _);

        internal static string RemoveFragment(this string url, out string fragment) {
            if (url == null) throw new ArgumentNullException(nameof(url));
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(url));

            var fragmentIndex = url.IndexOf('#');
            if (fragmentIndex < 0) {
                fragment = null;
                return url;
            }

            fragment = url.Substring(fragmentIndex);
            return url.Substring(0, fragmentIndex);
        }
    }
}
