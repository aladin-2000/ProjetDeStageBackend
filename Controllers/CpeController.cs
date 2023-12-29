
namespace backEnd.Controllers{

using System.Net.Http;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Xml;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.Json;

    [ApiController]
    [Route("/api/CPE")]
    public class CpeController:ControllerBase{

    private static readonly HttpClient httpClient=new HttpClient();

    [HttpGet]
    [Route("/cpeNames")]
    public async Task<ActionResult<List<string>>> GetCpeNames()
    {
        using (HttpClient client = new HttpClient())
        {
            List<string> cpeNames = new List<string>();

            for(int startIndex=0;startIndex<1;startIndex++)
            {
                string StringStartIndex=startIndex.ToString();

                HttpResponseMessage response = await client.GetAsync("https://services.nvd.nist.gov/rest/json/cpes/2.0/?startIndex="+StringStartIndex);
                Console.WriteLine(response);
                Console.WriteLine(response.GetType());


                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();

                    JObject jsonObject = JObject.Parse(jsonResult);


                    Console.WriteLine(jsonObject);
                    Console.WriteLine(response.Content);


                
                    JArray products = (JArray)jsonObject["products"];

         
                

                    foreach (JObject product in products)
                    {
            
                        string cpeName = (string)product["cpe"]["cpeName"];
                        cpeNames.Add(cpeName);
                    }
                
                }
                else
                {
                    return BadRequest($"Error: {response.StatusCode}"+StringStartIndex);
                }
        
            }
            return Ok(cpeNames);
            
            
        }
    }


    [HttpGet]
    [Route("/vendor")]
    public async Task<ActionResult<List<string>>> GetVendors()
    {
    var cpeNamesResult = await GetCpeNames();

    if (cpeNamesResult.Result is OkObjectResult okResult)
        {
            List<string> cpeNames = okResult.Value as List<string>;
            List<string>Vendors=new List<string>();
            foreach(string cpename in cpeNames)
            {
                string vendor;
                string [] L=cpename.Split(":");
                vendor=L[3];
                Vendors.Add(vendor);
            }
            List<string> distinctVendors=new List<string>();
            foreach(string vendor in Vendors){
                if(!distinctVendors.Contains(vendor)){
                    distinctVendors.Add(vendor);
                }
            }
            return Ok(distinctVendors);  
        }
    else
        {
            return BadRequest(cpeNamesResult.Result);
        }
    }


    [HttpGet]
    [Route("/vendor/avance")]
    public async Task<ActionResult<List<string>>> GetVendorsAvance(List<string>cpeNames)
    {
           
            List<string>Vendors=new List<string>();
            foreach(string cpename in cpeNames)
            {
                string vendor;
                string [] L=cpename.Split(":");
                vendor=L[3];
                Vendors.Add(vendor);
            }
            List<string> distinctVendors=new List<string>();
            foreach(string vendor in Vendors){
                if(!distinctVendors.Contains(vendor)){
                    distinctVendors.Add(vendor);
                }
            }
            return Ok(distinctVendors);  
    }



    [HttpGet]
    [Route("/version")]
    public async Task<ActionResult<List<string>>> GetVersions()
    {
    var cpeNamesResult = await GetCpeNames();

    if (cpeNamesResult.Result is OkObjectResult okResult)
        {
            List<string> cpeNames = okResult.Value as List<string>;
            List<string>Versions=new List<string>();
            foreach(string cpename in cpeNames)
            {
                string version;
                string [] L=cpename.Split(":");
                version=L[5];
                Versions.Add(version);
            }
            List<string>distinctVersions=new List<string>();
            foreach(string version in Versions){
                if(!distinctVersions.Contains(version)){
                    distinctVersions.Add(version);
                }
            }
            return Ok(distinctVersions);  
        }
    else
        {
            return BadRequest(cpeNamesResult.Result);
        }
    }




    [HttpGet]
    [Route("/product")]
    public async Task<ActionResult<List<string>>> GetProducts()
    {
    var cpeNamesResult = await GetCpeNames();

    if (cpeNamesResult.Result is OkObjectResult okResult)
        {
            List<string> cpeNames = okResult.Value as List<string>;
            List<string>Products=new List<string>();
            foreach(string cpename in cpeNames)
            {
                string product;
                string [] L=cpename.Split(":");
                product=L[4];
                Products.Add(product);
            }
            List<string> distinctProduct=new List<string>();
            foreach(string product in Products)
            {
                if(!distinctProduct.Contains(product)){
                    distinctProduct.Add(product);
                }
            }
            return Ok(distinctProduct);  
        }
    else
        {
            return BadRequest(cpeNamesResult.Result);
        }
    }



    [HttpGet]
    [Route("/vendor/Product/Version")]
    public async Task<ActionResult<List<string>>> GetVendorProductVersion()
    {
    var cpeNamesResult = await GetCpeNames();

    if (cpeNamesResult.Result is OkObjectResult okResult)
        {
            List<string> cpeNames = okResult.Value as List<string>;
            List<string>CpeVendorProductVersion=new List<string>();
            foreach(string cpename in cpeNames)
            {
                string VendorProductVersion;
                string [] L=cpename.Split(":");
                VendorProductVersion=L[3]+":"+L[4]+":"+L[5];
                CpeVendorProductVersion.Add(VendorProductVersion);
            }

            // Crée une nouvelle instance XmlDocument.
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("root");
            doc.AppendChild(root);

            // parcourir le tableau CpeVendorProductVersion
            foreach (var element in CpeVendorProductVersion)
            {
                XmlElement item = doc.CreateElement("item");
                item.InnerText = element;
                root.AppendChild(item);
            }

            doc.Save("PwnPatch.xml");

            return Ok(CpeVendorProductVersion);  
        }
    else
        {
            return BadRequest(cpeNamesResult.Result);
        }
    }
    
    

    [HttpGet]
    [Route("recuperer")]
    public ActionResult<List<string>> Recuperer()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load("PwnPatch.xml");

        XmlNodeList nodeList = doc.DocumentElement.SelectNodes("/root/item");

        List<string> maListe = new List<string>();  

        foreach (XmlNode node in nodeList)
        {
            maListe.Add(node.InnerText);
        }
        return maListe;  
    }


[HttpGet]
[Route("/vendor/Product/Version/2")]
public async Task<ActionResult<List<string>>> GetVendorProductVersion2()
{
    var cpeNamesResult = await GetCpeNames();

    if (cpeNamesResult.Result is OkObjectResult okResult)
    {
        List<string> cpeNames = okResult.Value as List<string>;
        List<string> CpeVendorProductVersion = new List<string>();
        foreach (string cpename in cpeNames)
        {
            string VendorProductVersion;
            string[] L = cpename.Split(":");
            VendorProductVersion = L[3] + ":" + L[4] + ":" + L[5];
            CpeVendorProductVersion.Add(VendorProductVersion);
        }

        // Connectez-vous à la base de données MongoDB.
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("myDatabase");
        var collection = database.GetCollection<BsonDocument>("CpeVendorProductVersion"); 

        // Créez un document BSON pour envelopper les valeurs de chaînes.
        var documentList = new List<BsonDocument>();
        foreach (var element in CpeVendorProductVersion)
        {
            var document = new BsonDocument
            {
                { "VendorProductVersion", element }
            };
            documentList.Add(document);
        }

        // Insérez les documents BSON dans MongoDB.
        await collection.InsertManyAsync(documentList);

        return Ok(CpeVendorProductVersion);
    }
    else
    {
        return BadRequest(cpeNamesResult.Result);
    }
}  


// Api that takes a vendor as a parametre then returns a list of cpes having that vendor 

[HttpGet]
[Route("/search/by/vendor/{vendor}")]
public async Task<ActionResult<List<string>>> GetCpesByVendor(string vendor){

    var cpeNamesResult = await GetCpeNames();
    if (cpeNamesResult.Result is OkObjectResult okResult)
    {
        List<string> cpeNames = okResult.Value as List<string>;
        List<string> CpeVendorProductVersion = new List<string>();
        foreach (string cpename in cpeNames)
        {
            string VendorProductVersion;
            string[] L = cpename.Split(":");
            VendorProductVersion = L[3] + ":" + L[4] + ":" + L[5];
            CpeVendorProductVersion.Add(VendorProductVersion);
        }
        List<string> L2= new List<string>();

        foreach(string VendorProductVersion in CpeVendorProductVersion){
            if(VendorProductVersion.StartsWith(vendor)==true){
                L2.Add(VendorProductVersion);
            }
        }
        return Ok(L2);
    }
   else
    {
        return BadRequest(cpeNamesResult.Result);
    }

}


// second Api that do the same task as the Api above

[HttpGet]
[Route("/searchByVendor/{vendor}")]
public async Task<ActionResult<List<string>>> GetCpeNamesByVendor(string vendor,List<string>cpeNames)
{

   
    List<string> CpeVendorProductVersion = new List<string>();
    foreach (string cpename in cpeNames)
    {
        string VendorProductVersion;
        string[] L = cpename.Split(":");
        VendorProductVersion = L[3] + ":" + L[4] + ":" + L[5];
        CpeVendorProductVersion.Add(VendorProductVersion);
    }
    List<string> L2= new List<string>();

    foreach(string VendorProductVersion in CpeVendorProductVersion){
        if(VendorProductVersion.StartsWith(vendor)==true){
            L2.Add(VendorProductVersion);
        }
    }
    return Ok(L2);
}

// Get full CpeNames By vendor

[HttpGet]
[Route("/search/Full/Cpenames/ByVendor/{vendor}")]
public async Task<ActionResult<List<string>>> GetFullCpeNamesByVendor(string vendor,List<string>cpeNames)
{
    List<string> L2= new List<string>();

    foreach(string cpeName in cpeNames){
        if(cpeName.Contains(vendor)==true){
            L2.Add(cpeName);
        }
    }
    return Ok(L2);
}
// get one cpename by vendor_Product_Version

[HttpGet]
[Route("/search/One/Cpename/ByVendor/{VendorProductVersion}")]
public async Task<ActionResult<string>> GetOneCpeNameByVendorProductVersion(string VendorProductVersion)
{
    string OneCpename = "";
    var dictResult = await RecuperercpenamesDictionaryXML();
    
        Console.WriteLine("392");

        Dictionary<string, string> dict = dictResult.Value as Dictionary<string, string>;
        List<string> L=new List<string>();
        L=dict[VendorProductVersion.Split(":")[0]].Split(",").ToList();
        foreach(string cpeName in L){
            if(cpeName.Contains(VendorProductVersion)==true){
            return Ok(cpeName);
            }
        }
        return "NOT FOUND";   
}



   // this Api Creates A mongoDB Data BASE 
[HttpGet]
[Route("/CreateDataBase")]
public async void CreateDataBase()
{
    var cpeNamesResult = await GetCpeNames();
    if(cpeNamesResult.Result is OkObjectResult okResult){
        List<string> cpeNames = okResult.Value as List<string>;
        var VendorsResult = await GetVendorsAvance(cpeNames);
        if (VendorsResult.Result is OkObjectResult okResult1)
        {
            List<string> Vendors = okResult1.Value as List<string>;
            // Connectez-vous à la base de données MongoDB.
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("myDatabase");
            var collection = database.GetCollection<BsonDocument>("newDataBase4"); 
            // fin du code qui permet la connection avec la base de donnée
            // Créez un document BSON pour envelopper les valeurs de chaînes.
            var documentList = new List<BsonDocument>();
            // fin du code qui permet la creation d'un doccument Bson pour envelopper les valeur de chaines
            foreach(string vendor in Vendors){
                var CpesResult=await GetCpeNamesByVendor(vendor,cpeNames);
                if(CpesResult.Result is OkObjectResult okResult2){
                    List<string> Cpes=okResult2.Value as List <string>;
                    string stringCpes = string.Join(",", Cpes);
                    Dictionary<string, string> D = new Dictionary<string, string>();
                    D.Add(vendor,stringCpes);
                    string jsonString = JsonSerializer.Serialize(D);
                    var document = new BsonDocument
                    {
                        { vendor, stringCpes }
                    };
                    documentList.Add(document);
                }
            }
            // Insérez les documents BSON dans MongoDB.
            await collection.InsertManyAsync(documentList);
        }
    }
}



[HttpGet]
[Route("/nouvelleJson")]
public async Task<ActionResult<Dictionary<string, string>>> NouvelleJson()
{
    var cpeNamesResult = await GetCpeNames();
    Dictionary<string, string> dict = new Dictionary<string, string>();
    if(cpeNamesResult.Result is OkObjectResult okResult){

        List<string> cpeNames = okResult.Value as List<string>;
        var VendorsResult = await GetVendorsAvance(cpeNames);
        if (VendorsResult.Result is OkObjectResult okResult1)
        {
            List<string> Vendors = okResult1.Value as List<string>;
            foreach(string vendor in Vendors){
                var CpesResult=await GetFullCpeNamesByVendor(vendor,cpeNames);
                if(CpesResult.Result is OkObjectResult okResult2){
                    List<string> Cpes=okResult2.Value as List <string>;
                    string stringCpes = string.Join(",", Cpes);
                    dict.Add(vendor,stringCpes);
                }
            }
            return dict;   
        }
    }          
    return dict;
}









[HttpGet]
[Route("/cpeNames/Dictionary")]
public async Task<ActionResult<Dictionary<string, string>>> cpenamesDictionary()
{
    var cpeNamesResult = await GetCpeNames();
    Dictionary<string, string> dict = new Dictionary<string, string>();
    if(cpeNamesResult.Result is OkObjectResult okResult){

        List<string> cpeNames = okResult.Value as List<string>;
        var VendorsResult = await GetVendorsAvance(cpeNames);
        if (VendorsResult.Result is OkObjectResult okResult1)
        {
            List<string> Vendors = okResult1.Value as List<string>;
            foreach(string vendor in Vendors){
                var CpesResult=await GetFullCpeNamesByVendor(vendor,cpeNames);
                if(CpesResult.Result is OkObjectResult okResult2){
                    List<string> Cpes=okResult2.Value as List <string>;
                    string stringCpes = string.Join(",", Cpes);
                    dict.Add(vendor,stringCpes);
                }
            }
            return dict;   
        }
    }          
    return dict;
}








[HttpGet]
[Route("/nouvellejson/formatedxml")]
    public async void créerUnFicherXML()
    {
        var dictResult = await NouvelleJson();
    
            Dictionary<string,string> dict = dictResult.Value as Dictionary<string,string>;
            // Chemin du fichier XML de sortie
            string xmlFilePath = "output2.xml";
            // Créez un Writer XML pour écrire les données dans le fichier
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true, // Indenter le fichier pour une meilleure lisibilité
                IndentChars = "\t" // Utiliser des tabulations pour l'indentation
            };

            using (XmlWriter writer = XmlWriter.Create(xmlFilePath, settings)){

                // Début du document XML
                writer.WriteStartDocument();

                // Élément racine
                writer.WriteStartElement("Dictionary");
                foreach (var element in dict)
                {
                    // Élément pour chaque paire clé-valeur dans le dictionnaire
                    writer.WriteStartElement("Entry");

                    // Élément pour la clé
                    Console.WriteLine("502 !!");
                    writer.WriteStartElement("Key");
                    writer.WriteString(element.Key);
                    writer.WriteEndElement();

                    // Élément pour la valeur
                    writer.WriteStartElement("Value");
                    writer.WriteString(element.Value);
                    writer.WriteEndElement();

                    // Fermer l'élément Entry
                    writer.WriteEndElement();
                }
                 // Fermer l'élément racine
                writer.WriteEndElement();

                // Fin du document XML
                writer.WriteEndDocument();

                // Fermer le Writer
                writer.Close();
            }
            Console.WriteLine("CRETAED !!");
    }


[HttpGet]
[Route("/cpenamesDictionary/formatedxml")]
    public async void créerUnFicherXMLAvecFullCpeNmes()
    {
        var dictResult = await cpenamesDictionary();
    
            Dictionary<string,string> dict = dictResult.Value as Dictionary<string,string>;
            // Chemin du fichier XML de sortie
            string xmlFilePath = "cpenamesDictionary.xml";
            // Créez un Writer XML pour écrire les données dans le fichier
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true, // Indenter le fichier pour une meilleure lisibilité
                IndentChars = "\t" // Utiliser des tabulations pour l'indentation
            };

            using (XmlWriter writer = XmlWriter.Create(xmlFilePath, settings)){

                // Début du document XML
                writer.WriteStartDocument();

                // Élément racine
                writer.WriteStartElement("Dictionary");
                foreach (var element in dict)
                {
                    // Élément pour chaque paire clé-valeur dans le dictionnaire
                    writer.WriteStartElement("Entry");

                    // Élément pour la clé
                    Console.WriteLine("502 !!");
                    writer.WriteStartElement("Key");
                    writer.WriteString(element.Key);
                    writer.WriteEndElement();

                    // Élément pour la valeur
                    writer.WriteStartElement("Value");
                    writer.WriteString(element.Value);
                    writer.WriteEndElement();

                    // Fermer l'élément Entry
                    writer.WriteEndElement();
                }
                 // Fermer l'élément racine
                writer.WriteEndElement();

                // Fin du document XML
                writer.WriteEndDocument();

                // Fermer le Writer
                writer.Close();
            }
            Console.WriteLine("CRETAED !!");
    }


    [HttpGet]
    [Route("/recuperer/ficherXML")]
    public async Task<ActionResult<Dictionary<string,string>>> RecupererFicherXML()
    {
        // Chemin du fichier XML à lire
        string xmlFilePath = "output.xml";
        // Dictionnaire pour stocker les données lues du fichier XML
        Dictionary<string, string> myDictionary = new Dictionary<string, string>();

        // Créez un Reader XML pour lire les données depuis le fichier
        using (XmlReader reader = XmlReader.Create(xmlFilePath))
        {
            string currentKey = null;
            string currentValue = null;

            while (reader.Read())
            {
                // Vérifier si l'élément courant est un élément de début
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "Key")
                    {
                        // Lire la clé et passer au nœud de texte suivant pour récupérer sa valeur
                        reader.Read();
                        currentKey = reader.Value;
                    }
                    else if (reader.Name == "Value")
                    {
                        // Lire la valeur et passer au nœud suivant pour continuer la lecture du dictionnaire
                        reader.Read();
                        currentValue = reader.Value;

                        // Ajouter la paire clé-valeur dans le dictionnaire
                        if (!string.IsNullOrEmpty(currentKey) && !string.IsNullOrEmpty(currentValue))
                        {
                            myDictionary.Add(currentKey, currentValue);
                        }
                    }
                }
            }
        }
        // Afficher le contenu du dictionnaire
        foreach (var kvp in myDictionary)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
        return myDictionary;

    }
    




    [HttpGet]
    [Route("/recuperer/cpenamesDictionaryXML")]
    public async Task<ActionResult<Dictionary<string,string>>> RecuperercpenamesDictionaryXML()
    {
        // Chemin du fichier XML à lire
        string xmlFilePath = "cpenamesDictionary.xml";
        // Dictionnaire pour stocker les données lues du fichier XML
        Dictionary<string, string> myDictionary = new Dictionary<string, string>();

        // Créez un Reader XML pour lire les données depuis le fichier
        using (XmlReader reader = XmlReader.Create(xmlFilePath))
        {
            string currentKey = null;
            string currentValue = null;

            while (reader.Read())
            {
                // Vérifier si l'élément courant est un élément de début
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "Key")
                    {
                        // Lire la clé et passer au nœud de texte suivant pour récupérer sa valeur
                        reader.Read();
                        currentKey = reader.Value;
                    }
                    else if (reader.Name == "Value")
                    {
                        // Lire la valeur et passer au nœud suivant pour continuer la lecture du dictionnaire
                        reader.Read();
                        currentValue = reader.Value;

                        // Ajouter la paire clé-valeur dans le dictionnaire
                        if (!string.IsNullOrEmpty(currentKey) && !string.IsNullOrEmpty(currentValue))
                        {
                            myDictionary.Add(currentKey, currentValue);
                        }
                    }
                }
            }
        }
        // Afficher le contenu du dictionnaire
        foreach (var kvp in myDictionary)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
        return myDictionary;

    }

    







        [HttpGet]
        [Route("/GetCves/{VendorProductVersion}")]
        public async Task<ActionResult<List<List<string>>>> GetCves(string VendorProductVersion)
        {

            var cpeResult=await GetOneCpeNameByVendorProductVersion(VendorProductVersion);
            if(cpeResult.Result is OkObjectResult okResult)
            {
                string cpe = okResult.Value as string;
                using (HttpClient client = new HttpClient())

                {    

                    List<List<string>> cves=new List<List<string>>();

                    HttpResponseMessage response = await client.GetAsync("https://services.nvd.nist.gov/rest/json/cves/2.0?cpeName="+cpe);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResult = await response.Content.ReadAsStringAsync();

                        JObject jsonObject = JObject.Parse(jsonResult);

                        JArray vulnerabilities = (JArray)jsonObject["vulnerabilities"];

                        foreach (JObject vulnerabilitie in vulnerabilities)
                        {
                
                            string id = (string) vulnerabilitie["cve"]["id"];
                            string sourceIdentifier = (string) vulnerabilitie["cve"]["sourceIdentifier"];
                            string published = (string) vulnerabilitie["cve"]["published"];
                            string lastModified = (string) vulnerabilitie["cve"]["lastModified"];
                            string vulnStatus = (string) vulnerabilitie["cve"]["vulnStatus"];
                            string cisaExploitAdd = (string) vulnerabilitie["cve"]["cisaExploitAdd"];
                            string cisaActionDue = (string) vulnerabilitie["cve"]["cisaActionDue"];
                            List<string> V= new List<string>();
                            V.Add(id);
                            V.Add(sourceIdentifier);
                            V.Add(published);
                            V.Add(lastModified);
                            V.Add(vulnStatus);
                            V.Add(cisaExploitAdd);
                            V.Add(cisaActionDue);
                            cves.Add(V);


                        }


                    }

                    else
                    {
                        return BadRequest($"Error: {response.StatusCode}");
                    }
                    return(Ok(cves));
                }
            }
            else
            {
                return BadRequest(cpeResult.Result);
            }

        }



    
    






    }
}  