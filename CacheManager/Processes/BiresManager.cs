using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace CacheManager.Processes
{
    public class BiresManager
    {
        public static MemoryStream GetStreamFromBires(string fileB2B, bool appendOrCreate = true)
        {
            List<string> fileToDonwload = new List<string>();
            string[] indiciDiMagazzino = new string[] {
            //"9","12","13","14","15","16","17","18","19","20"
           "14"
        };
            foreach (string indiceMag in indiciDiMagazzino)
            {
                fileToDonwload.Add(fileB2B + indiceMag + ".csv");
            }
            string urlB2B = "ftp://mercury.b2bires.com:6000/anag/";
            MemoryStream ms = new MemoryStream();
            foreach (string fileTmp in fileToDonwload)
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(urlB2B + fileTmp);
                request.UsePassive = true;
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential("ftpI00005226", "mf834hjc");
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                byte[] buffer = new byte[8 * 1024];
                int len;
                while ((len = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, len);
                    //output.Write(buffer, 0, len);
                }
            }
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
    }
}
