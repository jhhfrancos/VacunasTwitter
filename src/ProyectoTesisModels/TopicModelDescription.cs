using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoTesisModels
{
    public class TopicModelDescription
    {
        public int TopicID { get; set; }
        public float Score { get; set; }
        public Dictionary<string, float> Tokens { get; set; }

    }
}
