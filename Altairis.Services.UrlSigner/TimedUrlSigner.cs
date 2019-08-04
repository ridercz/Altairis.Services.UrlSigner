using System;

namespace Altairis.Services.UrlSigner {
    public class TimedUrlSigner {
        private static readonly DateTime ZeroTime = new DateTime(2019, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public IUrlSigner Signer { get; }

        public TimedUrlSigner(IUrlSigner signer) {
            this.Signer = signer;
        }

        // Helper methods for working with URIs

        public Uri Sign(Uri url, TimeSpan ttl) => new Uri(this.Sign(url.ToString(), ttl));

        public bool Verify(Uri url) => this.Verify(url.ToString());

        // TTL-aware signing and verification

        public string Sign(string url, TimeSpan ttl) {
            if (url == null) throw new ArgumentNullException(nameof(url));
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(url));

            // Append expiration timestamp
            var expTimeStamp = DateTime.UtcNow.Add(ttl).Subtract(ZeroTime).TotalSeconds;
            url = url.AppendParameter("exp", expTimeStamp.ToString());

            // Sign value with configured signer
            return this.Signer.Sign(url);
        }

        public bool Verify(string url) {
            if (url == null) throw new ArgumentNullException(nameof(url));
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(url));

            // Perform signature verification
            var result = this.Signer.Verify(url);
            if (!result) return false;

            // Get expiration timestamp
            var unsignedUrl = url.RemoveLastParameter("sig", out var _);
            unsignedUrl.RemoveLastParameter("exp", out var expString);

            // Compare with current time
            var expTime = ZeroTime.AddSeconds(double.Parse(expString));
            return expTime >= DateTime.UtcNow;
        }

    }
}
