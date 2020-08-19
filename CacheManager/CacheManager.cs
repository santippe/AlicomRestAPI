using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;
using System.Net;
using System.Text;
using System.Collections.Specialized;
using CacheManager.Processes;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CacheManager
{
    public class CacheManager
    {
        public NameValueCollection listaANAGtitle = new NameValueCollection();
        public NameValueCollection listaANAGimgs = new NameValueCollection();
        public NameValueCollection listaANAGdesc = new NameValueCollection();
        public NameValueCollection listaANAGlotto = new NameValueCollection();
        public NameValueCollection listaANAGprezz = new NameValueCollection();
        public NameValueCollection listaANAGdisp = new NameValueCollection();
        public NameValueCollection listaANAGean = new NameValueCollection();
        public NameValueCollection listaANAGbrand = new NameValueCollection();
        public NameValueCollection listaANAGCat = new NameValueCollection();
        public NameValueCollection listaEQUIPprezzo = new NameValueCollection();
        public NameValueCollection listaEQUIPBrand = new NameValueCollection();
        public NameValueCollection listaEQUIPCodeBrand = new NameValueCollection();
        public NameValueCollection listaEQUIPean = new NameValueCollection();
        public NameValueCollection capaldotitle = new NameValueCollection();
        public NameValueCollection capaldodisp = new NameValueCollection();
        public NameValueCollection capaldoimgs = new NameValueCollection();
        public NameValueCollection capaldoEAN = new NameValueCollection();
        public NameValueCollection capaldodesc = new NameValueCollection();
        public NameValueCollection capaldolotto = new NameValueCollection();
        public NameValueCollection capaldoprezzo = new NameValueCollection();
        public NameValueCollection capaldobrand = new NameValueCollection();
        public NameValueCollection capaldopeso = new NameValueCollection();
        public Dictionary<string, string> listaCediTitle = new Dictionary<string, string>();
        public Dictionary<string, float?> listaCediPrice = new Dictionary<string, float?>();
        public Dictionary<string, InventoryElement> inventoryList = new Dictionary<string, InventoryElement>();
        public IEnumerable<string> newCats;

        public IEnumerable<string> readLines(string filetoread)
        {
            //using (TextReader reader = File.OpenText(filetoread)) {
            object reader = null;
            try
            {
                reader = new StreamReader(filetoread, Encoding.Default);
            }
            catch { };
            if (reader != null)
            {
                string line;
                while ((line = ((StreamReader)reader).ReadLine()) != null)
                {
                    yield return line;
                }
                ((StreamReader)reader).Close();
            }
        }

        public CacheManager(ILogger<CacheManager> logger)
        {
            //TO DO read from bires
            Task.Run(() => {
                var cmds = new string[] { "ANAG_", "DISP_", "SCHEDE_" };
                while (true)
                {
                    foreach (var cmd in cmds)
                    {
                        BiresManager.GetStreamFromBires(cmd);
                    }
                    ReadFromFile();
                    Task.Delay(7200);
                }
            });
        }

        private void ReadFromFile()
        {
            var ASCII = System.Text.Encoding.ASCII;
            string pathToRead = AppDomain.CurrentDomain.BaseDirectory + "/stored/";
            var lines = File.ReadAllLines(pathToRead + "ANAG_9.csv", System.Text.Encoding.UTF8).Skip(1);
            var lines2 = File.ReadAllLines(pathToRead + "SCHEDE_9.csv", System.Text.Encoding.UTF8).Skip(1);
            var lines3 = File.ReadAllLines(pathToRead + "DISP_9.csv", System.Text.Encoding.UTF8).Skip(1);
            //var lineEquipe = readLines(pathToRead + "/art_05.txt");
            //var lineMarchiEquipe = readLines(pathToRead + "/MCH.txt");
            //var lineeEANquipe = readLines(pathToRead + "/ARA.txt");
            var lineeCapaldo = readLines(AppDomain.CurrentDomain.BaseDirectory + "/localuser/capaldo/exdess.csv");
            //var lineeCedi = readLines(pathToRead + "/CEDI.csv");
            foreach (string line in lines)
            {
                string[] formLine = line.Split('|');
                {
                    if (!formLine[1].ToLower().Contains("dvd") && !formLine[1].ToLower().Contains("cd") &&
                        !formLine[1].ToLower().Contains("brd") && formLine[0].Trim() != "CODICE")
                    {
                        string supplierCode = formLine[0];
                        float prezzo = 0;
                        InventoryElement elem = inventoryList.ContainsKey(supplierCode) ? inventoryList[supplierCode] : new InventoryElement();
                        float.TryParse(formLine[8], out prezzo);
                        listaANAGtitle[supplierCode] = formLine[1];
                        listaANAGlotto[supplierCode] = formLine[7];
                        listaANAGean[supplierCode] = formLine[2];
                        listaANAGCat[supplierCode] = formLine[4];
                        listaANAGbrand[supplierCode] = formLine[3];
                        listaANAGprezz[supplierCode] = (prezzo * 1.22 * 1.08).ToString("0.00");
                        elem.Supplier = "BIRES";
                        elem.Title = formLine[1];
                        elem.Price = prezzo * 1.22f * 1.08f;
                        elem.Ean = formLine[2];
                        elem.Brand = formLine[3];
                    }
                }
            }
            foreach (string line in lines2)
            {
                try
                {
                    string[] formLine = line.Split('|');
                    if (formLine[0].Trim() != ("CODICE ARTICOLO"))
                    {
                        string supplierCode = formLine[0];
                        InventoryElement elem = inventoryList.ContainsKey(supplierCode) ? inventoryList[supplierCode] : new InventoryElement();
                        listaANAGimgs[formLine[0]] = formLine[7];
                        listaANAGdesc[formLine[0]] = formLine[5];
                        elem.Supplier = "BIRES";
                        elem.Image = formLine[7];
                        elem.Description = formLine[5];
                    }
                }
                catch { }
            }
            foreach (string line in lines3)
            {
                try
                {
                    string[] formLine = line.Split('|');
                    if (formLine[0].Trim() != ("CODICE"))
                    {
                        string supplierCode = formLine[0];
                        InventoryElement elem = inventoryList.ContainsKey(supplierCode) ? inventoryList[supplierCode] : new InventoryElement();
                        listaANAGdisp[supplierCode] = formLine[1];
                        elem.Supplier = "BIRES";
                        int qty = 0;
                        int.TryParse(formLine[1], out qty);
                        elem.Qty = qty;
                    }
                }
                catch { }
            }
            foreach (string line in lineeCapaldo)
            {
                string[] formLine = line.Split(';');
                //try {
                float prezzo = 0;
                float promo = 0;
                float.TryParse(formLine[7].Replace(',', '.'), out prezzo);
                float.TryParse(formLine[10].Replace(',', '.'), out promo);
                float realPrice = (promo > 0 ? promo : prezzo);
                capaldotitle[formLine[0]] = formLine[1];
                float disp = 0;
                float.TryParse(formLine[5].Replace(',', '.'), out disp);
                capaldodisp[formLine[0]] = disp.ToString("0");
                capaldoimgs[formLine[0]] = formLine[15];
                capaldoprezzo[formLine[0]] = ((realPrice * 1.06) / (1 - .1803)).ToString("0.00");
                capaldoEAN[formLine[0]] = formLine[11];
                capaldobrand[formLine[0]] = formLine[19];
                capaldodesc[formLine[0]] = formLine[18];
                float peso = 0;
                float.TryParse(formLine[14].Replace(',', '.'), out peso);
                capaldopeso[formLine[0]] = peso.ToString("0.00");
                //} catch { };
            }
            //foreach(string line in lineeCedi) {
            //    string[] formLine = line.Split(';');
            //    listaCediTitle[formLine[2]] = formLine[3];
            //    listaCediPrice[formLine[2]] = formLine[18].AsFloat();
            //    //adding something new here
            //}
            newCats = listaANAGCat.AllKeys.Select(x => listaANAGCat[x]).Distinct().OrderBy(x => x);
        }
    }
}