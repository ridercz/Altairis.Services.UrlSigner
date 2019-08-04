using System;
using Xunit;

namespace Altairis.UrlSigner.Tests {
    public class NullUrlSignerTests {
        private static readonly Uri TestUri = new Uri("https://www.example.com/");
        private const string TestString = "https://www.example.com/";

        [Fact]
        public static void SignUri() {
            var signer = new NullUrlSigner();
            var signedUri = signer.Sign(TestUri);
            Assert.Equal(TestUri, signedUri);
        }

        [Fact]
        public static void SignString() {
            var signer = new NullUrlSigner();
            var signedString = signer.Sign(TestString);
            Assert.Equal(TestString, signedString);
        }

        [Fact]
        public static void VerifyUri() {
            var signer = new NullUrlSigner();
            Assert.True(signer.Verify(TestUri));
        }

        [Fact]
        public static void VerifyString() {
            var signer = new NullUrlSigner();
            Assert.True(signer.Verify(TestString));
        }

    }
}
