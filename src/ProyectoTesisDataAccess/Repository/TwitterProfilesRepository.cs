using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ProyectoTesisDataAccess.Context;
using ProyectoTesisModels.Modelos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProyectoTesisDataAccess.Repository
{
    public class TwitterProfilesRepository
    {
        private DataBaseContext dataContext;
        private IMongoCollection<BsonDocument> collectionProfiles;
        private IMongoCollection<BsonDocument> collectionProfilesClean;
        private IMongoCollection<BsonDocument> collectionBase;
        private IMongoCollection<BsonDocument> collectionBaseClean;
        private static string collectionProfilesName = "Tweets_Profiles";
        private static string collectionProfilesCleanName = "Tweets_Profiles_Clean";
        private static string collectionBaseName = "Tweets_Base";
        private static string collectionBaseCleanName = "Tweets_Base_Clean";
        public TwitterProfilesRepository()
        {
            dataContext = new DataBaseContext();
            collectionProfiles = dataContext.getCollection(collectionProfilesName);
            collectionProfilesClean = dataContext.getCollection(collectionProfilesCleanName);
            collectionBase = dataContext.getCollection(collectionBaseName);
            collectionBaseClean = dataContext.getCollection(collectionBaseCleanName);
        }

        public Tweet GetFirstTweet()
        {
            var firstDocument = collectionProfiles.Find(new BsonDocument()).FirstOrDefault();
            Tweet tweet = BsonSerializer.Deserialize<Tweet>(firstDocument);
            return tweet;
        }

        public List<Tweet> GetAllTweets(int limit, string name = "Tweets_Profiles")
        {
            var documents = (name == "Tweets_Profiles") ? collectionProfiles.Find(new BsonDocument()).Limit(limit).ToList() : collectionBase.Find(new BsonDocument()).Limit(limit).ToList();
            List<Tweet> tweets = new List<Tweet>();
            foreach (var item in documents)
            {
                Tweet tweet = BsonSerializer.Deserialize<Tweet>(item);
                tweets.Add(tweet);
            }
            return tweets;
        }

        public List<TweetClean> GetCleanTweets(int limit, string name = "Tweets_Profiles_Clean")
        {
            var documents = (name == "Tweets_Profiles_Clean") ? collectionProfilesClean.Find(new BsonDocument()).Limit(limit).ToList() : collectionBaseClean.Find(new BsonDocument()).Limit(limit).ToList();
            List<TweetClean> tweets = new List<TweetClean>();
            foreach (var item in documents)
            {
                TweetClean tweet = BsonSerializer.Deserialize<TweetClean>(item);
                tweets.Add(tweet);
            }
            return tweets;
        }

        public bool SaveTweet(string[] tweets, string name = "Tweets_Profiles")
        {
            foreach (var item in tweets)
            {
                try
                {
                    var jsonDoc = BsonDocument.Parse(item);
                    var filter = Builders<BsonDocument>.Filter.Eq("id", jsonDoc.GetValue("id"));
                    var document = (name == "Tweets_Profiles") ? collectionProfiles.Find(filter).FirstOrDefault(): collectionBase.Find(filter).FirstOrDefault();
                    if (document == null)
                        if (name == "Tweets_Profiles") collectionProfiles.InsertOne(jsonDoc);
                        else collectionBase.InsertOne(jsonDoc);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return true;
        }

        public bool DataCleansing()
        {
            if (dataContext.CollectionExists(collectionProfilesCleanName)) dataContext.DropCollection(collectionProfilesCleanName);
            if (dataContext.CollectionExists(collectionBaseCleanName)) dataContext.DropCollection(collectionBaseCleanName);
            string map = @"
                function() {
                    emit({id:this.id, user:this.user.id}, this.text);
                }";
            /*
             * //Remover URLs
             * //Transformar camel case a texto separado
             * //Tranforma a minusculas
             * //Reemplazar saltos de linea por espacios
             * //Remueve todo lo diferente a numeros y letras
             * //Remueve dosbles+ espacios en blanco
             * //Remueve StopWords
             *///
            //TODO: No recupera correctamente el user
            string reduce = @"
                function(id, tweet) {
                    var stopWords = ['actualmente','a','y', 'acuerdo', 'adelante', 'ademas', 'además', 'adrede', 'afirmó', 'agregó', 'ahi', 'ahora', 'ahí', 'al', 'algo', 'alguna', 'algunas', 'alguno', 'algunos', 'algún', 'alli', 'allí', 'alrededor', 'ambos', 'ampleamos', 'antano', 'antaño', 'ante', 'anterior', 'antes', 'apenas', 'aproximadamente', 'aquel', 'aquella', 'aquellas', 'aquello', 'aquellos', 'aqui', 'aquél', 'aquélla', 'aquéllas', 'aquéllos', 'aquí', 'arriba', 'arribaabajo', 'aseguró', 'asi', 'así', 'atras', 'aun', 'aunque', 'ayer', 'añadió', 'aún', 'bajo', 'bastante', 'bien', 'breve', 'buen', 'buena', 'buenas', 'bueno', 'buenos', 'cada', 'casi', 'cerca', 'cierta', 'ciertas', 'cierto', 'ciertos', 'cinco', 'claro', 'comentó', 'como', 'con', 'conmigo', 'conocer', 'conseguimos', 'conseguir', 'considera', 'consideró', 'consigo', 'consigue', 'consiguen', 'consigues', 'contigo', 'contra', 'cosas', 'creo', 'cual', 'cuales', 'cualquier', 'cuando', 'cuanta', 'cuantas', 'cuanto', 'cuantos', 'cuatro', 'cuenta', 'cuál', 'cuáles', 'cuándo', 'cuánta', 'cuántas', 'cuánto', 'cuántos', 'cómo', 'da', 'dado', 'dan', 'dar', 'de', 'debajo', 'debe', 'deben', 'debido', 'decir', 'dejó', 'del', 'delante', 'demasiado', 'demás', 'dentro', 'deprisa', 'desde', 'despacio', 'despues', 'después', 'detras', 'detrás', 'dia', 'dias', 'dice', 'dicen', 'dicho', 'dieron', 'diferente', 'diferentes', 'dijeron', 'dijo', 'dio', 'donde', 'dos', 'durante', 'día', 'días', 'dónde', 'ejemplo', 'el', 'ella', 'ellas', 'ello', 'ellos', 'embargo', 'empleais', 'emplean', 'emplear', 'empleas', 'empleo', 'en', 'encima', 'encuentra', 'enfrente', 'enseguida', 'entonces', 'entre', 'era', 'eramos', 'eran', 'eras', 'eres', 'es', 'esa', 'esas', 'ese', 'eso', 'esos', 'esta', 'estaba', 'estaban', 'estado', 'estados', 'estais', 'estamos', 'estan', 'estar', 'estará', 'estas', 'este', 'esto', 'estos', 'estoy', 'estuvo', 'está', 'están', 'ex', 'excepto', 'existe', 'existen', 'explicó', 'expresó', 'él', 'ésa', 'ésas', 'ése', 'ésos', 'ésta', 'éstas', 'éste', 'éstos', 'fin', 'final', 'fue', 'fuera', 'fueron', 'fui', 'fuimos', 'general', 'gran', 'grandes', 'gueno', 'ha', 'haber', 'habia', 'habla', 'hablan', 'habrá', 'había', 'habían', 'hace', 'haceis', 'hacemos', 'hacen', 'hacer', 'hacerlo', 'haces', 'hacia', 'haciendo', 'hago', 'han', 'hasta', 'hay', 'haya', 'he', 'hecho', 'hemos', 'hicieron', 'hizo', 'horas', 'hoy', 'hubo', 'igual', 'incluso', 'indicó', 'informo', 'informó', 'intenta', 'intentais', 'intentamos', 'intentan', 'intentar', 'intentas', 'intento', 'ir', 'junto', 'la', 'lado', 'largo', 'las', 'le', 'lejos', 'les', 'llegó', 'lleva', 'llevar', 'lo', 'los', 'luego', 'lugar', 'mal', 'manera', 'manifestó', 'mas', 'mayor', 'me', 'mediante', 'medio', 'mejor', 'mencionó', 'menos', 'menudo', 'mi', 'mia', 'mias', 'mientras', 'mio', 'mios', 'mis', 'misma', 'mismas', 'mismo', 'mismos', 'modo', 'momento', 'mucha', 'muchas', 'mucho', 'muchos', 'muy', 'más', 'mí', 'mía', 'mías', 'mío', 'míos', 'nada', 'nadie', 'ni', 'ninguna', 'ningunas', 'ninguno', 'ningunos', 'ningún', 'no', 'nos', 'nosotras', 'nosotros', 'nuestra', 'nuestras', 'nuestro', 'nuestros', 'nueva', 'nuevas', 'nuevo', 'nuevos', 'nunca', 'ocho', 'os', 'otra', 'otras', 'otro', 'otros', 'pais', 'para', 'parece', 'parte', 'partir', 'pasada', 'pasado', 'paìs', 'peor', 'pero', 'pesar', 'poca', 'pocas', 'poco', 'pocos', 'podeis', 'podemos', 'poder', 'podria', 'podriais', 'podriamos', 'podrian', 'podrias', 'podrá', 'podrán', 'podría', 'podrían', 'poner', 'por', 'porque', 'posible', 'primer', 'primera', 'primero', 'primeros', 'principalmente', 'pronto', 'propia', 'propias', 'propio', 'propios', 'proximo', 'próximo', 'próximos', 'pudo', 'pueda', 'puede', 'pueden', 'puedo', 'pues', 'qeu', 'que', 'quedó', 'queremos', 'quien', 'quienes', 'quiere', 'quiza', 'quizas', 'quizá', 'quizás', 'quién', 'quiénes', 'qué', 'raras', 'realizado', 'realizar', 'realizó', 'repente', 'respecto', 'sabe', 'sabeis', 'sabemos', 'saben', 'saber', 'sabes', 'salvo', 'se', 'sea', 'sean', 'segun', 'segunda', 'segundo', 'según', 'seis', 'ser', 'sera', 'será', 'serán', 'sería', 'señaló', 'si', 'sido', 'siempre', 'siendo', 'siete', 'sigue', 'siguiente', 'sin', 'sino', 'sobre', 'sois', 'sola', 'solamente', 'solas', 'solo', 'solos', 'somos', 'son', 'soy', 'soyos', 'su', 'supuesto', 'sus', 'suya', 'suyas', 'suyo', 'sé', 'sí', 'sólo', 'tal', 'tambien', 'también', 'tampoco', 'tan', 'tanto', 'tarde', 'te', 'temprano', 'tendrá', 'tendrán', 'teneis', 'tenemos', 'tener', 'tenga', 'tengo', 'tenido', 'tenía', 'tercera', 'ti', 'tiempo', 'tiene', 'tienen', 'toda', 'todas', 'todavia', 'todavía', 'todo', 'todos', 'total', 'trabaja', 'trabajais', 'trabajamos', 'trabajan', 'trabajar', 'trabajas', 'trabajo', 'tras', 'trata', 'través', 'tres', 'tu', 'tus', 'tuvo', 'tuya', 'tuyas', 'tuyo', 'tuyos', 'tú', 'ultimo', 'un', 'una', 'unas', 'uno', 'unos', 'usa', 'usais', 'usamos', 'usan', 'usar', 'usas', 'uso', 'usted', 'ustedes', 'última', 'últimas', 'último', 'últimos', 'va', 'vais', 'valor', 'vamos', 'van', 'varias', 'varios', 'vaya', 'veces', 'ver', 'verdad', 'verdadera', 'verdadero', 'vez', 'vosotras', 'vosotros', 'voy', 'vuestra', 'vuestras', 'vuestro', 'vuestros', 'ya', 'yo'];
                    var texto = tweet[0];
                    var textoStop = '';
                    if(Object.prototype.toString.call(texto) != '[object String]') texto = texto.texto;
                    if(texto){
                        texto = texto.replace(/(?:www|http?)[^\s]+/gi,'');
                        texto = texto.replace(/([a-z0-9])([A-Z])/g, '$1 $2');
                        texto = texto.toLowerCase();
                        texto = texto.replace(/[\r\n]/g, ' ');
                        texto = texto.replace(/[^a-zA-Z0-9áéíóú ]/g, '');
                        texto = texto.replace(/\s\s+/g, ' ');
                        textoStop = texto.split(' ').filter(w => w && !stopWords.includes(w)).join(' ');
                    }
                    return {texto:texto, textoStop: textoStop};
                }";
            BsonJavaScript mapScript = new BsonJavaScript(map);
            BsonJavaScript reduceScript = new BsonJavaScript(reduce);

            FilterDefinitionBuilder<BsonDocument> filterBuilder = new FilterDefinitionBuilder<BsonDocument>();
            FilterDefinition<BsonDocument> filter = filterBuilder.Empty;
            MapReduceOptions<BsonDocument, BsonDocument> options = new MapReduceOptions<BsonDocument, BsonDocument>
            {
                Filter = filter,
                OutputOptions = MapReduceOutputOptions.Reduce(collectionProfilesCleanName, "Twitter"),
                Verbose = true
            };
            MapReduceOptions<BsonDocument, BsonDocument> optionsBase = new MapReduceOptions<BsonDocument, BsonDocument>
            {
                Filter = filter,
                OutputOptions = MapReduceOutputOptions.Reduce(collectionBaseCleanName, "Twitter"),
                Verbose = true
            };
            try
            {
                collectionBase.MapReduce(mapScript, reduceScript, optionsBase).ToList();
                collectionProfiles.MapReduce(mapScript, reduceScript, options).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred {ex.Message}");
            }
            return true;
        }
    }
}
