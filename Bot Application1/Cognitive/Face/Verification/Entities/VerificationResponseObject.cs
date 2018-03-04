using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ChatBot.Cognitive.Verification.Entities
{
    [DataContract]
    public class VerificationResponseRootObject
    {
        [DataMember]
        public bool isIdentical { get; set; }

        [DataMember]
        public double confidence { get; set; }
    }
}