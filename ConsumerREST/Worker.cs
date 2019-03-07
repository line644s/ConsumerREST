using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Modellib.Model;
using Newtonsoft.Json;

namespace ConsumerREST
{
    internal class Worker
    {
        private const string URI = "http://localhost:56413/api/Facilitets";

        public Worker()
        {

        }

        public void Start()
        {
            List<Facilitet> facilitets = GET_ALL();

            foreach (Facilitet facilitet in facilitets)
            {
                Console.WriteLine(facilitet);
            }

            Console.WriteLine("Find nr 5");
            Console.WriteLine(GET_ONE(5));

            Console.WriteLine("indsæt nr. 10");
            Console.WriteLine("result = " + Post(new Facilitet(10, "bad")));

            Console.WriteLine("update nr. 10");
            Console.WriteLine("result = " + PUT(10, new Facilitet(10, "vaskerum")));

            Console.WriteLine("Slet nr. 10");
            Console.WriteLine("result = " + DELETE(10));
        }

        private List<Facilitet> GET_ALL()
        {
            List<Facilitet> liste = new List<Facilitet>();

            using (HttpClient client = new HttpClient())
            {
                Task<string> resTask = client.GetStringAsync(URI);
                string jsonStr = resTask.Result;

                liste = JsonConvert.DeserializeObject<List<Facilitet>>(jsonStr);
            }

            return liste;
        }

        private Facilitet GET_ONE(int id)
        {
            Facilitet facilitet = new Facilitet();

            using (HttpClient client = new HttpClient())
            {
                Task<string> resTask = client.GetStringAsync(URI + "/" + id);
                string jsonStr = resTask.Result;

                facilitet = JsonConvert.DeserializeObject<Facilitet>(jsonStr);
            }

            return facilitet;
        }

        private bool Post(Facilitet facilitet)
        {
            bool ok = true;

            using (HttpClient client = new HttpClient())
            {
                string serializeObject = JsonConvert.SerializeObject(facilitet);
                StringContent content = new StringContent(serializeObject, Encoding.UTF8, "application/json");

                Task<HttpResponseMessage> postAsync = client.PostAsync(URI, content);

                HttpResponseMessage resps = postAsync.Result;

                if (resps.IsSuccessStatusCode)
                {
                    string jsonStr = resps.Content.ReadAsStringAsync().Result;
                    ok = JsonConvert.DeserializeObject<bool>(jsonStr);
                }
                else
                {
                    ok = false;
                }
            }

            return ok;
        }

        private bool PUT(int id, Facilitet facilitet)
        {
            bool ok = true;

            using (HttpClient client = new HttpClient())
            {
                string serializeObject = JsonConvert.SerializeObject(facilitet);
                StringContent content = new StringContent(serializeObject, Encoding.UTF8, "application/json");

                Task<HttpResponseMessage> putAsync = client.PutAsync(URI + "/" + id, content);

                HttpResponseMessage resps = putAsync.Result;

                if (resps.IsSuccessStatusCode)
                {
                    string jsonStr = resps.Content.ReadAsStringAsync().Result;
                    ok = JsonConvert.DeserializeObject<bool>(jsonStr);
                }
                else
                {
                    ok = false;
                }
            }

            return ok;
        }

        private bool DELETE(int id)
        {
            bool ok = true;

            using (HttpClient client = new HttpClient())
            {
                Task<HttpResponseMessage> deleteAsync = client.DeleteAsync(URI + "/" + id);
                HttpResponseMessage resps = deleteAsync.Result;

                if (resps.IsSuccessStatusCode)
                {
                    string jsonStr = resps.Content.ReadAsStringAsync().Result;
                    ok = JsonConvert.DeserializeObject<bool>(jsonStr);
                }
                else
                {
                    ok = false;
                }
            }

            return ok;
        }
    }
}

