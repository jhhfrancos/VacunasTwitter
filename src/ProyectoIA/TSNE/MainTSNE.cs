using Accord.MachineLearning.Clustering;
using ProyectoTesisModels.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoIA.TSNEproject
{
    public class MainTSNE
    {
        public MainTSNE()
        {

        }
        public TSNEResponse CreateTSNEModel(double[][] inputs, int[] inputsTarget, int perplexity)
        {
            // Create a new t-SNE algorithm 
            TSNE tSNE = new TSNE()
            {
                NumberOfOutputs = 2,
                Perplexity = perplexity
            };

            //Transform to a reduced dimensionality space
            double[][] output = tSNE.Transform(inputs);
            return new TSNEResponse(output, inputsTarget);
        }
    }
}
