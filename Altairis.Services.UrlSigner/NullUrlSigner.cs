using System;

namespace Altairis.Services.UrlSigner {
    public class NullUrlSigner : IUrlSigner {

        public string Sign(string url) => url;

        public Uri Sign(Uri url) => url;

        public bool Verify(string url) => true;

        public bool Verify(Uri url) => true;

    }
}
