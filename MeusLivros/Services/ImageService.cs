using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MeusLivros.Services
{
    public class ImageService
    {
        public static void PostToImgur(string imagFilePath, string apiKey)
        {
            byte[] imageData;

            FileStream fileStream = File.OpenRead(imagFilePath);
            imageData = new byte[fileStream.Length];
            fileStream.Read(imageData, 0, imageData.Length);
            fileStream.Close();

            string uploadRequestString = "image=" + Uri.EscapeDataString(System.Convert.ToBase64String(imageData)) + "&key=" + apiKey;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://api.imgur.com/2/upload");
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ServicePoint.Expect100Continue = false;

            StreamWriter streamWriter = new StreamWriter(webRequest.GetRequestStream());
            streamWriter.Write(uploadRequestString);
            streamWriter.Close();

            WebResponse response = webRequest.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader responseReader = new StreamReader(responseStream);

            string responseString = responseReader.ReadToEnd();
        }
    }
}
