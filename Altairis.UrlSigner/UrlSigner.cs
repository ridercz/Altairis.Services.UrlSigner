using System;
using System.Text;

namespace Altairis.UrlSigner {
    public abstract class UrlSigner : IUrlSigner {

        // Abstract methods to be implemented in derived classes

        protected abstract byte[] GetSignature(byte[] data);

        protected abstract bool VerifySignature(byte[] data, byte[] sig);

        // Helper methods for working with URIs

        public Uri Sign(Uri url) => new Uri(this.Sign(url.ToString()));

        public bool Verify(Uri url) => this.Verify(url.ToString());


        // Public methods

        public virtual string Sign(string url) {
            if (url == null) throw new ArgumentNullException(nameof(url));
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(url));

            // Convert string to array of bytes
            var data = Encoding.UTF8.GetBytes(url);

            // Compute signature
            var sigData = this.GetSignature(data);

            // Convert it to URL-safe Base64
            var sigString = Convert.ToBase64String(sigData).Replace('+', '-').Replace('/', '_');

            // Append signature
            url += (url.Contains("?") ? "&" : "?") + "sig=" + sigString;
            return url;
        }

        public virtual bool Verify(string url) {
            if (url == null) throw new ArgumentNullException(nameof(url));
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(url));

            try {
                // Try to parse the signed URL
                var urlString = url.RemoveLastParameter("sig", out var sigString);
                sigString = sigString.Replace('-', '+').Replace('_', '/');

                // Convert to byte array
                var urlData = Encoding.UTF8.GetBytes(urlString);
                var sigData = Convert.FromBase64String(sigString);

                // Verify signature
                return this.VerifySignature(urlData, sigData);
            }
            catch (Exception) {
                return false;
            }
        }

    }
}
