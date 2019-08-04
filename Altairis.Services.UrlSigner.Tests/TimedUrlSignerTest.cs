using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xunit;

namespace Altairis.Services.UrlSigner.Tests {
    public class TimedUrlSignerTest {
        private static readonly byte[] Key = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 };
        private static readonly TimeSpan TestTtl = TimeSpan.FromSeconds(1);
        private static readonly Uri TestUri = new Uri("https://www.example.com/");
        private const string TestString = "https://www.example.com/";

        [Fact]
        public static async Task ImmediateRoundtripUri() {
            var signer = new TimedUrlSigner(new HmacUrlSigner<HMACSHA512>(Key));
            var signedUri = signer.Sign(TestUri, TestTtl);
            await Task.Delay((int)TestTtl.TotalMilliseconds / 2);
            Assert.True(signer.Verify(signedUri));
        }

        [Fact]
        public static async Task ImmediateRoundtripString() {
            var signer = new TimedUrlSigner(new HmacUrlSigner<HMACSHA512>(Key));
            var signedString = signer.Sign(TestString, TestTtl);
            await Task.Delay((int)TestTtl.TotalMilliseconds / 2);
            Assert.True(signer.Verify(signedString));
        }

        [Fact]
        public static async Task ImmediateRoundtripStringWithFragment() {
            const string origUrl = "https://www.example.com#myFragment";

            var signer = new TimedUrlSigner(new HmacUrlSigner<HMACSHA512>(Key));
            var signedString = signer.Sign(origUrl, TestTtl);
            await Task.Delay((int)TestTtl.TotalMilliseconds / 2);
            Assert.True(signer.Verify(signedString));
            Assert.EndsWith("#myFragment", signedString); // we want preserve fragment component

            var signedStringWithoutFragment = signedString.Replace("#myFragment", "");
            Assert.True(signer.Verify(signedStringWithoutFragment));
        }

        [Fact]
        public static async Task ExpiredRoundtripUri() {
            var signer = new TimedUrlSigner(new HmacUrlSigner<HMACSHA512>(Key));
            var signedUri = signer.Sign(TestUri, TestTtl);
            await Task.Delay((int)TestTtl.TotalMilliseconds * 2);
            Assert.False(signer.Verify(signedUri));
        }

        [Fact]
        public static async Task ExpiredRoundtripString() {
            var signer = new TimedUrlSigner(new HmacUrlSigner<HMACSHA512>(Key));
            var signedString = signer.Sign(TestString, TestTtl);
            await Task.Delay((int)TestTtl.TotalMilliseconds * 2);
            Assert.False(signer.Verify(signedString));
        }

    }
}
