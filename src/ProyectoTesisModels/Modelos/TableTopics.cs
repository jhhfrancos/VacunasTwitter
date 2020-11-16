using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoTesisModels.Modelos
{
    public class TableTopics
    {
        public string text { get; set; }
        public string topics { get; set; }
        public string[] tokens { get; set; }

        public TableTopics(string text, string topics, string[] tokens)
        {
            this.text = text;
            this.topics = topics;
            this.tokens = tokens;
        }
    }
}
