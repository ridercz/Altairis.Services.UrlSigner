using System;
using System.Security.Cryptography;

namespace Altairis.Services.UrlSigner {
    public class HmacUrlSigner<TAlg> : UrlSigner where TAlg : KeyedHashAlgorithm, new() {
        readonly TAlg hmac;

        public HmacUrlSigner(byte[] key) {
            this.hmac = new TAlg { Key = key };
        }

        protected override byte[] GetSignature(byte[] data) => this.hmac.ComputeHash(data);

        protected override bool VerifySignature(byte[] data, byte[] sig) {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (sig == null) throw new ArgumentNullException(nameof(sig));

            // Compute correct signature
            var correctSig = this.GetSignature(data);
            if (correctSig.Length != sig.Length) return false;

            // Constant time compare
            var result = 0;
            for (var i = 0; i < correctSig.Length; i++) {
                result |= sig[i] ^ correctSig[i];
            }
            return result == 0;
        }

    }
}
