using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace APIClient
{

    class Program
    {
        
        public static void Main()
        {
            string urlbase = ConfigurationManager.AppSettings.Get("urlbase");

            using (BDADMSURTIOFFICEEntities db = new BDADMSURTIOFFICEEntities())
            {
                //Listado de Clientes
                List<ADMCLIENTE> clientes = (from c in db.ADMCLIENTE
                                             where (c.EWEB == "N")
                                             select c).ToList();

                var usuariosw = new List<ClienteWeb>();

                foreach (var item in clientes)
                {
                    ClienteWeb cliw = new ClienteWeb();
                    cliw.idusuario = item.CODIGO.Trim();
                    cliw.activacion = "habilitar";
                    cliw.nombre = item.RAZONSOCIAL.Trim();
                    cliw.apellido = item.RAZONSOCIAL.Trim();
                    cliw.contrasenia = item.RUC.Trim();
                    cliw.correo = item.EMAIL.Trim();
                    cliw.identificacion = "RUC";
                    cliw.numero_identificacion = item.RUC.Trim();
                    if (item.DIRECCION.Trim().Length > 45)
                    {
                        cliw.direccion = item.DIRECCION.Trim().Substring(0, 45);
                    }
                    else
                    {
                        cliw.direccion = item.DIRECCION.Trim();
                    }

                    cliw.referencia = "No";
                    cliw.pais = "ECUADOR";
                    cliw.ciudad = item.PROVINCIA.Trim();
                    cliw.codigo_postal = "0000";
                    cliw.celular1 = item.TELEFONOS.Trim();
                    cliw.imagen = "No";
                    cliw.img_servicios = "No";
                    cliw.img_representante = "NO";
                    cliw.empresa = item.RAZONSOCIAL.Trim();
                    cliw.ruc = item.RUC.Trim();
                    cliw.idtipo = item.TIPO.Trim();
                    cliw.ingreso = DateTime.Now.Date.ToString("dd-MM-yyyy");

                    usuariosw.Add(cliw);
                }

                //Listado de Categorias
                List<ADMCATEGORIA> categorias = (from c in db.ADMCATEGORIA
                                                where (c.EWEB == "N" && c.ESTADO == "A")
                                                select c).ToList();

                List<CatagoriaWeb> listacate = new List<CatagoriaWeb>();
                foreach (var categoria in categorias)
                {
                    CatagoriaWeb catesingle = new CatagoriaWeb();
                    catesingle.estado = categoria.ESTADO;
                    catesingle.nombre = categoria.NOMBRE.Trim();
                    catesingle.idcategoria = categoria.CODIGO.Trim();
                    listacate.Add(catesingle);

                }

                Uri uClien = new Uri(urlbase+"api/usuarios");
                Uri uCate = new Uri(urlbase+"api/categoria");

                Task.Run(() => PostCategorias(listacate, uCate));
                Task.Run(() => PostClientes(usuariosw, uClien));
                GetPedidos();
                GetUsuarios("20-06-2019");
                
                
                Console.ReadKey();

            }


        }

        //Obtener Pedidos 
        async static void GetPedidos()
        {
            string urlbase = ConfigurationManager.AppSettings.Get("urlbase");
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(urlbase + "api/pedidos"))
                {
                    HttpContent content = response.Content;
                    string mycontent = await content.ReadAsStringAsync();
                    Console.WriteLine("-------");
                    Console.WriteLine(mycontent);

                }
            }

        }

        //Obtener usuarios por fecha
        async static void GetUsuarios(string date)
        {
            string urlbase = ConfigurationManager.AppSettings.Get("urlbase");
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(urlbase + "api/usuarios/" + date))
                {
                    HttpContent content = response.Content;
                    string mycontent = await content.ReadAsStringAsync();
                    Console.WriteLine("-------");
                    Console.WriteLine(mycontent);

                }
            }
        }
        class clientResponse
        {
            public string identify { get; set; }
            public string status { get; set; }
        }

        //Envio de Usuarios para POST
        private static async Task PostClientes(object content, Uri ul)
        {
            var data = new
            {
                usuarios = content
            };
            
            string myJson = JsonConvert.SerializeObject(data);
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(
                    ul,
                     new StringContent(myJson, Encoding.UTF8, "application/json"));

                HttpContent contentRes = response.Content;
                string mycontent = await contentRes.ReadAsStringAsync();
                var RootObjects = JsonConvert.DeserializeObject<List<clientResponse>>(mycontent);

                int noGuardado = (from c in RootObjects where (c.status == "NoSaved") select c).Count();
                int guardado = (from c in RootObjects where (c.status == "OK") select c).Count();
                Console.WriteLine("-------");
                Console.WriteLine("Envio de Usuarios Finalizado, total Guardado: "+guardado+", No Guardados: "+noGuardado);
          
            }

        }
        class categoryResponse
        {
            public string categoria { get; set; }
            public string status { get; set; }
        }
        //Envio de Categorias.
        private static async Task PostCategorias(object content, Uri ul)
        {
            var data = new
            {
                categorias = content
            };

            string myJson = JsonConvert.SerializeObject(data);

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(ul,new StringContent(myJson, Encoding.UTF8, "application/json"));

                HttpContent contentRes = response.Content;
                string mycontent = await contentRes.ReadAsStringAsync();
                var RootObjects = JsonConvert.DeserializeObject<List<categoryResponse>>(mycontent);
                int noGuardado = (from c in RootObjects where (c.status == "NoSaved") select c).Count();
                int guardado = (from c in RootObjects where (c.status == "OK") select c).Count();
                Console.WriteLine("-------");
                Console.WriteLine("Envio de Categorias Finalizado, total Guardado: " + guardado + ", No Guardados: " + noGuardado);

            }
        }
    }
}
