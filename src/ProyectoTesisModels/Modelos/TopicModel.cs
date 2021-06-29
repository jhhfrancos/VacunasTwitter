using Catalyst;
using System;
using System.Collections.Generic;
using System.Text;
using static Catalyst.Models.LDA;

namespace ProyectoTesisModels.Modelos
{
    public class TopicModel
    {
        public string Document { get; set; }
        public List<TopicModelDescription> TopicDescriptions { get; set; }
    }
}
