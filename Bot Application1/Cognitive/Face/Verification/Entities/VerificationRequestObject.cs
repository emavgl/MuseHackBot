using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ChatBot.Cognitive.Verification.Entities
{
    [DataContract]
    public class VerificationRequestRootObject
    {
        [DataMember]
        public string faceId1 { get; set; }

        [DataMember]
        public string faceId2 { get; set; }
    }
}