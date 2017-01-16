namespace LH.Forcas.Integration.Banks.Cze.Fio
{
    using System.IO;
    using Newtonsoft.Json;

    public class FioOutputParser
    {
        private readonly JsonSerializer serializer;
           
        public FioOutputParser()
        {
            this.serializer = new JsonSerializer();
        }

        public FioAccountStatement Parse(Stream stream)
        {
            using (var textReader = new StreamReader(stream))
            using(var jsonReader = new JsonTextReader(textReader))
            {
                var wrapper = this.serializer.Deserialize<FioAccountStatementJsonWrapper>(jsonReader);

                return wrapper?.AccountStatement;
            }
        }
    }
}