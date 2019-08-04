using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xunit;

namespace Altairis.UrlSigner.Tests {
    public class TimedUrlSignerTest {
        private static readonly byte[] Key = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 };
        private static readonly TimeSpan TestTtl = TimeSpan.FromMilliseconds(100);
        private static readonly Uri TestUri = new Uri("https://www.example.com/");
        private const string TestString = "https://www.example.com/";

        [Fact]
        public static void ImmediateRoundtripUri() {
            var signer = new TimedUrlSigner(new HmacUrlSigner<HMACSHA512>(Key));
            var signedUri = signer.Sign(TestUri, TestTtl);
            Assert.True(signer.Verify(signedUri));
        }

        [Fact]
        public static void ImmediateRoundtripString() {
            var signer = new TimedUrlSigner(new HmacUrlSigner<HMACSHA512>(Key));
            var signedString = signer.Sign(TestString, TestTtl);
            Assert.True(signer.Verify(signedString));
        }

        [Fact]
        public static async Task ExpiredRoundtripUri() {
            var signer = new TimedUrlSigner(new HmacUrlSigner<HMACSHA512>(Key));
            var signedUri = signer.Sign(TestUri, TestTtl);
            await Task.Delay(TestTtl.Add(TestTtl));
            Assert.False(signer.Verify(signedUri));
        }

        [Fact]
        public static async Task ExpiredRoundtripString() {
            var signer = new TimedUrlSigner(new HmacUrlSigner<HMACSHA512>(Key));
            var signedString = signer.Sign(TestString, TestTtl);
            await Task.Delay(TestTtl.Add(TestTtl));
            Assert.False(signer.Verify(signedString));
        }

    }
}
