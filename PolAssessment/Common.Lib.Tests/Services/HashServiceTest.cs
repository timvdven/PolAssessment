using PolAssessment.Common.Lib.Services;

namespace Common.Lib.Tests.Services;

public class HashServiceTest
{
#pragma warning disable CA1859 // Use concrete types when possible for improved performance
// The whole idea is to test the interface implementation, so it's better to use the interface type.
    private IHashService _hashService;
#pragma warning restore CA1859

    [SetUp]
    public void Setup()
    {
        _hashService = new HashService();
    }

    [TestCase("Hello World", "a591a6d40bf420404a011733cfb7b190d62c65bf0bcda32b57b277d9ad9f146e")]
    [TestCase("Some Other String", "ce7f5b9c1758c6de6db765ae65a7a81c42ed7bee284946d2f71928f7345e824d")]
    [TestCase("", "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855")]
    public void GetHash_StringInput_ReturnsValidHash(string input, string expected)
    {
        var actual = _hashService.GetHash(input);
        
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase(123, "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3")]
    [TestCase(-1234.442, "d6016a306ec70958cd5a449aef4994b2462645fe1c556bc9eade791c963be116")]
    [TestCase("", "12ae32cb1ec02d01eda3581b127c1fee3b0dc53572ed6baf239721a03d82e126")]
    public void GetHash_ObjectInput_ReturnsValidHash(object input, string expected)
    {
        var actual = _hashService.GetHash(input);
        
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void GetHash_RandomStringInput_ReturnsValidHashFormat()
    {
        foreach (var input in GetRandomStrings(50))
        {
            var actual = _hashService.GetHash(input);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual?.Length, Is.EqualTo(64));
        }
    }

    private static IEnumerable<string> GetRandomStrings(int count)
    {
        for (var i = 0; i < count; i++)
        {
            yield return GetRandomString();
        }
    }

    private static string GetRandomString()
    {
        var random = new Random();
        var length = random.Next(1, 100);
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}