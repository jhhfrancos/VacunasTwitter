using Catalyst;
using Catalyst.Models;
using Mosaik.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Version = Mosaik.Core.Version;
using P = Catalyst.PatternUnitPrototype;
using System.Linq;

namespace ProyectoIA.NER
{
    public class MainClassNER
    {
        public MainClassNER()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }

        public async Task<IEnumerable<string>> Trainning(List<string> stringArray)
        {
            // For training an AveragePerceptronModel, check the source-code here: https://github.com/curiosity-ai/catalyst/blob/master/Catalyst.Training/src/TrainWikiNER.cs
            // This example uses the pre-trained WikiNER model, trained on the data provided by the paper "Learning multilingual named entity recognition from Wikipedia", Artificial Intelligence 194 (DOI: 10.1016/j.artint.2012.03.006)
            // The training data was sourced from the following repository: https://github.com/dice-group/FOX/tree/master/input/Wikiner

            //Download the Reuters corpus if necessary
            List<IDocument> training = new List<IDocument>();
            foreach (var item in stringArray)
            {
                //var newItem = Utils.Utils.DeleteStopWords(item);
                training.Add(new Document(item, Language.Spanish));
            }

            IDocument[] train = training.ToArray();


            //Configures the model storage to use the online repository backed by the local folder ./catalyst-models/
            Storage.Current = new OnlineRepositoryStorage(new DiskStorage("catalyst-models"));

            //Create a new pipeline for the english language, and add the WikiNER model to it
            Console.WriteLine("Loading models... This might take a bit longer the first time you run this sample, as the models have to be downloaded from the online repository");
            var nlp = await Pipeline.ForAsync(Language.Spanish);
            nlp.Add(await AveragePerceptronEntityRecognizer.FromStoreAsync(language: Language.Spanish, version: Version.Latest, tag: "WikiNER"));

            //Another available model for NER is the PatternSpotter, which is the conceptual equivalent of a RegEx on raw text, but operating on the tokenized form off the text.
            //Adds a custom pattern spotter for the pattern: single("is" / VERB) + multiple(NOUN/AUX/PROPN/AUX/DET/ADJ)
            var isApattern = new PatternSpotter(Language.Spanish, 0, tag: "is-a-pattern", captureTag: "IsA");
            isApattern.NewPattern(
                "Es+Noun",
                mp => mp.Add(
                    new PatternUnit(P.Single().WithToken("es").WithPOS(PartOfSpeech.VERB)),
                    new PatternUnit(P.Multiple().WithPOS(PartOfSpeech.NOUN, PartOfSpeech.PROPN, PartOfSpeech.AUX, PartOfSpeech.DET, PartOfSpeech.ADJ))
            ));
            nlp.Add(isApattern);

            //For processing a multiple documents in parallel (i.e. multithreading), you can call nlp.Process on an IEnumerable<IDocument> enumerable
            var docs = nlp.Process(training);

            //This will print all recognized entities. You can also see how the WikiNER model makes a mistake on recognizing Amazon as a location on Data.Sample_1

            List<string> result = new List<string>();
            foreach (var d in docs) {
                result.Add( PrintDocumentEntities(d));
            }

            return result;

            /*//For correcting Entity Recognition mistakes, you can use the Neuralyzer class. 
            //This class uses the Pattern Matching entity recognition class to perform "forget-entity" and "add-entity" 
            //passes on the document, after it has been processed by all other proceses in the NLP pipeline
            var neuralizer = new Neuralyzer(Language.Spanish, 0, "WikiNER-sample-fixes");

            //Teach the Neuralyzer class to forget the match for a single token "Amazon" with entity type "Location"
            neuralizer.TeachForgetPattern("Location", "Amazon", mp => mp.Add(new PatternUnit(P.Single().WithToken("Amazon").WithEntityType("Location"))));

            //Teach the Neuralyzer class to add the entity type Organization for a match for the single token "Amazon"
            neuralizer.TeachAddPattern("Organization", "Amazon", mp => mp.Add(new PatternUnit(P.Single().WithToken("Amazon"))));

            //Add the Neuralyzer to the pipeline
            nlp.UseNeuralyzer(neuralizer);

            //Now you can see that "Amazon" is correctly recognized as the entity type "Organization"
            var doc2 = new Document(Data.Sample_1, Language.Spanish);
            nlp.ProcessSingle(doc2);
            PrintDocumentEntities(doc2);*/
        }

        public async Task<IEnumerable<string>> Testing(string text)
        {
            //Configures the model storage to use the online repository backed by the local folder ./catalyst-models/
            Storage.Current = new OnlineRepositoryStorage(new DiskStorage("catalyst-models"));

            //Create a new pipeline for the english language, and add the WikiNER model to it
            Console.WriteLine("Loading models... This might take a bit longer the first time you run this sample, as the models have to be downloaded from the online repository");
            var nlp = await Pipeline.ForAsync(Language.Spanish);
            nlp.Add(await AveragePerceptronEntityRecognizer.FromStoreAsync(language: Language.Spanish, version: Version.Latest, tag: "WikiNER"));
            //For processing a multiple documents in parallel (i.e. multithreading), you can call nlp.Process on an IEnumerable<IDocument> enumerable
            Document doc = new Document(text,Language.Spanish);
            var docs = nlp.Process(new List<Document>() { doc });

            //This will print all recognized entities. You can also see how the WikiNER model makes a mistake on recognizing Amazon as a location on Data.Sample_1

            List<string> result = new List<string>();
            foreach (var d in docs)
            {
                result.Add(PrintDocumentEntities(d));
            }

            return result;
        }


            private string PrintDocumentEntities(IDocument doc)
        {
            var value = doc.Value;
            var tokens = doc.TokenizedValue(mergeEntities: true);
            var entities = string.Join("\n", doc.SelectMany(span => span.GetCapturedTokens()).Select(e => $"\t {e.Value} [{e.POS}] "));
            return $"{entities}";//$"Texto:\n\t'{value}'\n\nTokens:\n\t'{tokens}'\n\nEntidades: \n{entities}";
        }

    }
}
