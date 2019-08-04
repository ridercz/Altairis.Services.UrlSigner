using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altairis.UrlSigner {
    public class NullUrlSigner : IUrlSigner {

        public string Sign(string url) => url;

        public Uri Sign(Uri url) => url;

        public bool Verify(string url) => true;

        public bool Verify(Uri url) => true;

    }
}
