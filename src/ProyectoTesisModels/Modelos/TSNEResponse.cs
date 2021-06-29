using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoTesisModels.Modelos
{
    public class TSNEResponse
    {
        public TSNEResponse(double[][] _mapXY, int[] _target)
        {
            mapXY = _mapXY;
            targets = _target;
        }
        public double[][] mapXY { get; set; }
        public int[] targets { get; set; }
        public List<TopicModel> documents { get; set; }
    }
}
