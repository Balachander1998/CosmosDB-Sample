using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_CosmosDB_Sample
{
    class Program
    {
        private static string endpointURL = "https://boldbicosmosdemo.documents.azure.com:443/";
        private static string primaryKey = "spBT3VwoX13LagGqbO7RqWdLf96RIJnAGGIa8ouGiSllBrLgG1F7CcmrIDLg6ZsOG28WD9vQGKzpZmEi1di7sg==";
        private static DocumentClient documentClient = new DocumentClient(new Uri(endpointURL), primaryKey);
       
       
        static void Main(string[] args)
        {
            getAllProducts();
            //getAllCollections();
            //getCollectionFields();
            Console.ReadKey();
        }
        private static void getAllProducts()
        {
           Uri uri= UriFactory.CreateDocumentCollectionUri("ToDoList", "productList");
            FeedOptions option = new FeedOptions();
            option.EnableCrossPartitionQuery = true;
            string sqlQuery = "Select * from productList";
            var response=documentClient.CreateDocumentQuery(uri, option, sqlQuery).ToList();
            JArray jArray = JArray.FromObject(response);  
            foreach(JObject jObject in jArray)
            {
                jObject.Remove("_rid");
                jObject.Remove("_self");
                jObject.Remove("_ts");
                jObject.Remove("_etag");
                foreach (JProperty prop in jObject.Properties())
                {
                   Console.WriteLine(prop.Name+" "+prop.Value.Type);                          
                }
            }
                
               
            
            
        }
        private static async Task getAllCollections()
        {
            Uri uri = UriFactory.CreateDatabaseUri("ToDoList");
            var collections=await documentClient.ReadDocumentCollectionFeedAsync(uri);
            int count = 0;
            foreach(DocumentCollection collection in collections)
            {
                Console.WriteLine(collection.Id);
                count = count + 1;
                Console.WriteLine(count);
            }
            
           
        }
    }
}
