using Newtonsoft.Json;
using SharpTestsEx;
using SmartParkAPI.Shared.Helpers;
using Xunit;

namespace SmartParkAPI.Shared.Tests
{
    public class EncryptHelperTests
    {
        private class SerializableClass
        {
            public int Prop1 { get; set; }
            public string Prop2 { get; set; }
            public double[] Props3 { get; set; }
        }

        private readonly SerializableClass _sut;

        public EncryptHelperTests()
        {
            _sut = new SerializableClass
            {
                Prop1 = 5,
                Prop2 = "prop2",
                Props3 = new[]
                {
                    2.2, 3.4, 5.133
                }
            };
        }

        [Fact]
        public void WhenSerializeObject_AndEncrypted_ThenDecryptededObjectEquals()
        {
            //Act
            var serializedObject = JsonConvert.SerializeObject(_sut);
            var encryptedObject = EncryptHelper.Encrypt(serializedObject);

            var decryptedObject = EncryptHelper.Decrypt(System.Net.WebUtility.UrlDecode(encryptedObject));
            var deserializedObject = JsonConvert.DeserializeObject<SerializableClass>(decryptedObject);

            //Then
            deserializedObject.Prop1.Should().Be.EqualTo(_sut.Prop1);
            deserializedObject.Prop2.Should().Be.EqualTo(_sut.Prop2);
            deserializedObject.Props3.Should().Have.Count.EqualTo(_sut.Props3.Length);

            for (int i = 0; i < deserializedObject.Props3.Length; i++)
            {
                deserializedObject.Props3[i].Should().Be.EqualTo(_sut.Props3[i]);
            }

            decryptedObject.Should().Be.EqualTo(serializedObject);
        }
    }
}
