using System.Runtime.Serialization;

namespace ChatBot.Cognitive.Viso
{
    [DataContract]
    public class FaceRectangle
    {
        [DataMember]
        public int top { get; set; }

        [DataMember]
        public int left { get; set; }

        [DataMember]
        public int width { get; set; }

        [DataMember]
        public int height { get; set; }
    }

    [DataContract]
    public class FaceAttributes
    {
        [DataMember]
        public string gender { get; set; }

        [DataMember]
        public string age { get; set; }
    }

    [DataContract]
    public class VisoResponseRootObject
    {
        [DataMember]
        public string faceId { get; set; }

        [DataMember]
        public FaceRectangle faceRectangle { get; set; }

        [DataMember]
        public FaceAttributes faceAttributes { get; set; }
    }
}