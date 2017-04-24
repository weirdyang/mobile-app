using System;
using System.IO;
using LH.Forcas.Banking.Exceptions;
using Newtonsoft.Json;

namespace LH.Forcas.Banking.Providers.Cz.Fio
{
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
            using (var jsonReader = new JsonTextReader(textReader))
            {
                FioAccountStatementJsonWrapper wrapper;

                try
                {
                    wrapper = this.serializer.Deserialize<FioAccountStatementJsonWrapper>(jsonReader);
                }
                catch (Exception ex)
                {
                    throw new BankPayloadFormatException("Parsing payload from FIO failed.", ex);
                }

                if (wrapper?.AccountStatement?.Info == null || wrapper.AccountStatement.TransactionList == null)
                {
                    throw new BankPayloadFormatException("Parsing payload from FIO failed. The deserialized object does not contain all data.");
                }

                return wrapper?.AccountStatement;
            }
        }
    }
}