using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace APIClient
{

    class Program
    {
        public static String TOKEN = "";

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
                    cliw.contrasenia = item.CLAVECARRO;
                    cliw.correo = item.CORREOCARRO;
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

                //listado de Marcas 
                List<ADMMARCA> marcas = (from c in db.ADMMARCA
                                         where (c.ESTADO == "A" && c.EWEB == "N")
                                         select c).ToList();

                List<MarcaWeb> marcalist = new List<MarcaWeb>();
                foreach (var mark in marcas)
                {
                    MarcaWeb marcasin = new MarcaWeb();
                    marcasin.estado = mark.ESTADO;
                    marcasin.nombre = mark.NOMBRE.Trim();
                    marcasin.idmarca = mark.CODIGO.Trim();
                    marcalist.Add(marcasin);

                }

                //Listado de Provincias
                List<ADMPROVINCIA> provincias = (from c in db.ADMPROVINCIA 
                                                 where (c.estado == "A") 
                                                 select c).ToList();

                //Listado de Cantones
                List<ADMCANTON> cantones = (from c in db.ADMCANTON
                                                 where (c.estado == "A")
                                                 select c).ToList();


                //listado de Productos
                List<ADMITEM> productos = (from c in db.ADMITEM
                                           where (c.CARRO == "S" && c.ESTAENCARRO == "N"
                                           && c.ESTADO == "A")
                                           select c).ToList();

                List<ProductoWeb> listaPro = new List<ProductoWeb>();
                foreach (var item in productos)
                {
                    ProductoWeb Prosingle = new ProductoWeb();
                    Prosingle.idproducto = item.ITEM.Trim();
                    Prosingle.descripcion = item.NOMBRE.Trim();
                    Prosingle.estado = item.ESTADO;
                    Prosingle.precio = item.PRECIO1.Value;
                    Prosingle.precio_anterior = item.PRECIO1.Value;
                    Prosingle.precio2 = item.PRECIO2.Value;
                    Prosingle.precio3 = item.PRECIO3.Value;
                    Prosingle.precio4 = item.PRECIO4.Value;
                    Prosingle.precio5 = item.PRECIO5.Value;
                    Prosingle.total_con_iva = 0;
                    Prosingle.total_con_iva2 = 0;
                    Prosingle.total_oferta_iva = 0;
                    Prosingle.total_oferta_iva2 = 0;
                    Prosingle.imagen = item.IMAGEN.Trim();
                    Prosingle.porcentaje_oferta = 0;
                    Prosingle.iva = 0;
                    Prosingle.oferta = item.POFERTA;
                    Prosingle.idcategoria = item.CATEGORIA.Trim();
                    Prosingle.idfamilia = item.FAMILIA.Trim();
                    Prosingle.idmarca = item.MARCA.Trim();
                    Prosingle.stock = item.STOCK.Value;
                    Prosingle.costo_envio = 0;
                    Prosingle.envio_gratuito = 0;
                    Prosingle.recomendado = item.NOVEDAD.Trim();
                    Prosingle.idcolor = "ninguno";
                    Prosingle.Graba_Iva = item.IVA.Trim();

                    listaPro.Add(Prosingle);

                }
                //ConsoleColor background = Console.BackgroundColor;
                //ConsoleColor foreground = Console.ForegroundColor;
                //Console.ForegroundColor = ConsoleColor.Red;
                //Console.BackgroundColor = ConsoleColor.White;
                //Console.Clear();

                //List<ADMCLIPRECIO> clientePrecios = new List<ADMCLIPRECIO>();


                var clientePrecios = db.Database.SqlQuery<ADMCLIPRECIO>("SELECT * FROM ADMCLIPRECIO").ToList();
              
                using (StreamWriter file = new StreamWriter(@"log.txt", true))
                {
                    file.WriteLine("\n0-0-0-0-0-0--0-0-0-0-0-0-0-0-0-0-0-0-0-0--0-0-0-0-0-0-0-0-0");
                    file.WriteLine("Inicio del Proceso: " + DateTime.Now.ToString("dd-MM-yyyy, hh':'mm tt"));
                    file.Close();
                }

                //direcciones de  Url's
                Uri uClien = new Uri(urlbase + "api/usuarios");
                Uri uCate = new Uri(urlbase + "api/categoria");
                Uri uMarca = new Uri(urlbase + "api/marca");
                Uri uProducto = new Uri(urlbase + "api/productos");
                Uri uTipoCliente = new Uri(urlbase + "api/tipocliente");
                Uri postCantones = new Uri(urlbase + "api/cantones");
                Uri postProvincias = new Uri(urlbase + "api/provincias");


                //Chequear que el servidor de API web esta Up.
                HttpClient client4 = new HttpClient();
                client4.Timeout = TimeSpan.FromSeconds(10);

                // Issue a request
                client4.GetAsync(urlbase + "/api/pedidos").ContinueWith(
                    getTask =>
                    {
                        if (getTask.IsCanceled)
                        {
                            using (StreamWriter file = new StreamWriter(@"log.txt", true))
                            {
                                file.WriteLine("Peticion Cancelada");
                                file.Close();
                            }
                        }
                        else if (getTask.IsFaulted)
                        {
                            using (StreamWriter file = new StreamWriter(@"log.txt", true))
                            {
                                file.WriteLine("No se puede comunicar con el Servidor API");
                                file.Close();
                            }
                        }
                        else
                        {
                            
                            string tokenApi = ConfigurationManager.AppSettings.Get("token");
                            string userEmail = ConfigurationManager.AppSettings.Get("userEmail");
                            string userPassword = ConfigurationManager.AppSettings.Get("userPassword");
                            Uri lin = new Uri(urlbase + "api/login");

                           

                            if (TOKEN == "")
                            {
                               
                                //una tarea se ejecuta al finalizar la predecesora.
                                Task<string> tokenValid = Task.Run(() => LoginApi(lin, userEmail, userPassword));
                                Task a = tokenValid.ContinueWith(x => PostCategorias(listacate, uCate));
                                Task c = a.ContinueWith(x => PostMarcas(marcalist, uMarca));
                                Task e = c.ContinueWith(x => PostClientePrecio(clientePrecios, uTipoCliente));
                                Task b = e.ContinueWith(x => PostClientes(usuariosw, uClien));
                                Task usuarios = e.ContinueWith(x => GetUsuarios(DateTime.Now.Date.ToString("dd-MM-yyyy")));
                                Task pedidos = usuarios.ContinueWith(x => GetPedidos());
                                Task d = b.ContinueWith(x => PostProducto(listaPro, uProducto));
                                Task f = d.ContinueWith(x => PostProvincias(provincias, postProvincias));
                                Task g = f.ContinueWith(x => PostCantones(cantones, postCantones));

                            }

                            HttpResponseMessage response = getTask.Result;
                        }
                    });

                //GetUsuarios("12-07-2019");
                //Console.ReadKey();

                Thread.Sleep(22000);

            }
        }

        class JWTLogin
        {
            public string token { get; set; }
            public string type { get; set; }
            public string expires { get; set; }
        }

        //Login - Obtener Json Web Token
        async static Task<string> LoginApi(Uri ul, string email2, string password2)
        {
            var data = new
            {
                email = email2,
                password = password2,
            };
                        
            using (HttpClient client = new HttpClient())
            {
                string myJson = JsonConvert.SerializeObject(data);
                var response = await client.PostAsync(ul,new StringContent(myJson, Encoding.UTF8, "application/json"));
                HttpContent contentRes = response.Content;
                string mycontent = await contentRes.ReadAsStringAsync();
                var RootObjects = JsonConvert.DeserializeObject<JWTLogin>(mycontent);

                TOKEN = RootObjects.token;
                using (StreamWriter file = new StreamWriter(@"log.txt", true))
                {
                    Console.WriteLine(TOKEN);
                    file.WriteLine("Token Usado: " + TOKEN);
                    file.Close();
                }
                //Console.WriteLine(TOKEN);
                return TOKEN;
            }
        }


        //Obtener Pedidos 
        async static void GetPedidos()
        {
            string urlbase = ConfigurationManager.AppSettings.Get("urlbase");
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);
                using (HttpResponseMessage response = await client.GetAsync(urlbase + "api/pedidos"))
                {
                    HttpContent content = response.Content;
                    string mycontent = await content.ReadAsStringAsync();
                    var RootObjects = JsonConvert.DeserializeObject<PedidoResponse>(mycontent);
                    int contadorCabeceras = 0;

                    using (BDADMSURTIOFFICEEntities db = new BDADMSURTIOFFICEEntities())
                    {
                        var parametroV = db.ADMPARAMETROV.First();
                        var parametroC = db.ADMPARAMETROC.First();
                        var bodega = (from c in db.ADMBODEGA where (c.CODIGO == parametroV.BODFAC.Value) select c).FirstOrDefault();

                        decimal Numfactura = bodega.NOFACTURA.Value;
                        decimal secuencial = parametroV.SECUENCIAL.Value;

                        //cabeceras que se regresan con el estado "P" a la WEB.
                        List<CabeceraWeb> CabecerasProcesados = new List<CabeceraWeb>();
                        IFormatProvider culture = new CultureInfo("en-US", true);
                        

                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                foreach (var pedido in RootObjects.pedidos)
                                {
                                    var Cliente = (from c in db.ADMCLIENTE where (c.RUC == pedido.ruc) select c).FirstOrDefault();
                                    if (Cliente != null)
                                    {
                                        ADMCABPEDIDO ped = new ADMCABPEDIDO();
                                        ped.TIPO = "PED";
                                        ped.BODEGA = parametroV.BODFAC.Value;
                                        ped.NUMERO = Numfactura + 1;
                                        ped.SECUENCIAL = secuencial + 1;
                                        ped.CLIENTE = Cliente.CODIGO;
                                        ped.VENDEDOR = parametroC.CODVENPOS;
                                        ped.FECHA = DateTime.ParseExact(pedido.fecha, "dd/MM/yyyy", culture);
                                        ped.ESTADO = "WEB";
                                        ped.SUBTOTAL = pedido.subtotal;
                                        ped.DESCUENTO = 0;
                                        ped.IVA = pedido.iva;
                                        ped.NETO = pedido.total;
                                        ped.PESO = 0;
                                        ped.VOLUMEN = 0;
                                        ped.OPERADOR = "ADM";
                                        ped.COMENTARIO = "Gracias-web";
                                        ped.OBSERVACION = "Pedido-Web";
                                        ped.FACTURA = null;
                                        ped.GUIA = null;
                                        ped.DOCFAC = "FAC";
                                        ped.DIASCRED = "99";
                                        ped.GRAVAIVA = pedido.Graba_iva;
                                        ped.CREDITO = "N";
                                        ped.NUMCUOTAS = null;
                                        ped.FECHALIBERACION = null;
                                        ped.HORALIBERACION = null;
                                        ped.OPERLIBERACION = null;
                                        ped.TRANSPORTE = 0;
                                        ped.RECARGO = 0;
                                        ped.TIPOCLIENTE = Cliente.TIPO;
                                        ped.SUCURSAL = "";
                                        ped.ESMOVIL = "N";
                                        ped.SECAUTOPEDMOVIL = null;
                                        ped.SERIEPOLOCLUB = "000000";
                                        ped.CODIGORETAILPRO = null;
                                        //ped.NUMEROPLANTILLA = 0;
                                        ped.CODIGOPEDIDO = 0;

                                        db.ADMCABPEDIDO.Add(ped);
                                        contadorCabeceras++;
                                     

                                        pedido.estado = "P";
                                        //pedido.idusuario = pedido.idusuario;
                                        CabecerasProcesados.Add(pedido);

                                        //Lista Detalles de la Actual Cabecera para almacenar en ADM.
                                        List<DetalleWeb> detalles = (from c in RootObjects.detalles where (c.idventa == pedido.idventas) select c).ToList();
                                        int linea = 0;
                                        foreach (var detaw in detalles)
                                        {
                                            string idPro = Convert.ToString(detaw.idproducto);
                                            ADMITEM producto = (from c in db.ADMITEM where (c.ITEM == idPro) select c).First();
                                            decimal factor = producto.FACTOR.Value;
                                            decimal res = detaw.cantidad / factor;

                                            ADMDETPEDIDO detal = new ADMDETPEDIDO();
                                            detal.LINEA = linea + 1;
                                            detal.SECUENCIAL = ped.SECUENCIAL;
                                            detal.ITEM = Convert.ToString(detaw.idproducto);

                                            if (res >= 1)
                                            {
                                                detal.CANTIC = Convert.ToInt32(Math.Truncate(res));
                                            }
                                            else
                                            {
                                                detal.CANTIC = 0;
                                            }
                                            detal.CANTIU = Convert.ToInt32(Math.Truncate(detaw.cantidad % factor));
                                            detal.CANTFUN = detaw.cantidad;
                                            detal.COSTOP = producto.COSTOP;
                                            detal.COSTOU = producto.COSTOU;
                                            detal.PORDES = 0;
                                            detal.PORDES2 = 0;
                                            detal.DESCUENTO = 0;
                                            detal.FORMAVTA = "V";
                                            detal.PRECIO = detaw.precio;
                                            detal.SUBTOTAL = detaw.subtotal;
                                            detal.IVA = detaw.iva;
                                            detal.NETO = detaw.valor_neto;
                                            detal.ESTADO = null;
                                            detal.FACTURADO = null;
                                            detal.GRAVAIVA = detaw.graba_iva;
                                            detal.TIPOPEDIDO = null;
                                            detal.TIPOITEM = "B";
                                            detal.LINGENCONDICION = 0;
                                            detal.DETALLE = "Pedido Web";
                                            detal.lote = "";
                                            db.ADMDETPEDIDO.Add(detal);
                                            linea++;
                                        }
                                        detalles.Clear();

                                        //db.CARROPRO("Secuencial", secuencial);
                                        //db.CARROPRO("NumeroFactura", Numfactura);
                                        Numfactura++;
                                        secuencial++;
                                        db.Database.ExecuteSqlCommand("UPDATE ADMPARAMETROV set SECUENCIAL = @num", new SqlParameter("@num", secuencial));
                                        db.Database.ExecuteSqlCommand("UPDATE ADMBODEGA set NOFACTURA = @num", new SqlParameter("@num", Numfactura));
                                        db.SaveChanges();
                                        

                                    }
                                    
                                }
                                trans.Commit();

                                //Envio de Cabeceras para actualizar en WEB.
                                Uri uPutProductos = new Uri(urlbase + "api/actualizarcab");
                                await Task.Run(() => PutCabeceras(CabecerasProcesados, uPutProductos));

                                using (StreamWriter file = new StreamWriter(@"log.txt", true))
                                {
                                    Console.WriteLine("Se obtuvieron: " + RootObjects.pedidos.Count() + " Pedidos, Guardados : " + contadorCabeceras);
                                    file.WriteLine("Se obtuvieron: " + RootObjects.pedidos.Count() + " Pedidos, Guardados : " + contadorCabeceras);
                                    file.Close();
                                }

                                //Console.WriteLine("-------");
                                //Console.WriteLine("Se obtuvieron: " + RootObjects.pedidos.Count() + " Pedidos, Guardados : " + contadorCabeceras);

                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error en almancenar pedidos:" + e);
                                Console.WriteLine(e);
                                //trans.Rollback();
                                using (StreamWriter file = new StreamWriter(@"log.txt", true))
                                {
                                 
                                    file.WriteLine("Error en almancenar pedidos:" + e);
                                    file.Close();
                                }
                                //Console.WriteLine("Error en almancenar pedidos");
                                
                            }
                        }
                    }
                }
            }
        }

        //actualizar las cabeceras de pedidos en la WEB.
        public async static Task PutCabeceras(object content, Uri ul)
        {
            var data = new
            {
                pedidos = content
            };

            string myJson = JsonConvert.SerializeObject(data);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);
                var response = await client.PostAsync(ul, new StringContent(myJson, Encoding.UTF8, "application/json"));

                HttpContent contentRes = response.Content;
                string mycontent = await contentRes.ReadAsStringAsync();
               
                //Console.WriteLine(mycontent);
                var RootObjects = JsonConvert.DeserializeObject<List<clientResponse>>(mycontent);

                int actualizados = (from c in RootObjects where (c.status == "Update") select c).Count();
                int noActualizado = (from c in RootObjects where (c.status == "NoFinded") select c).Count();

                using (StreamWriter file = new StreamWriter(@"log.txt", true))
                {
                    Console.WriteLine("Actualizacion de pedido en web Finalizado, total: " + actualizados + ", No Actualizados: " + noActualizado);
                    file.WriteLine("Actualizacion de pedido en web Finalizado, total: " + actualizados + ", No Actualizados: " + noActualizado);
                    file.Close();
                }
                //Console.WriteLine("-------");
                //Console.WriteLine("Actualizacion de pedido en web Finalizado, total: " + actualizados + ", No Actualizados: " + noActualizado);

            }

        }


        //Obtener usuarios por fecha
        async static void GetUsuarios(string date)
        {
            string urlbase = ConfigurationManager.AppSettings.Get("urlbase");

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);
                using (HttpResponseMessage response = await client.GetAsync(urlbase + "api/usuarios/" + date))
                {
                    HttpContent content = response.Content;
                    string mycontent = await content.ReadAsStringAsync();
                    var RootObjects = JsonConvert.DeserializeObject<ClienteRes>(mycontent);
                    string cliBase = ConfigurationManager.AppSettings.Get("ClienteBase");

                    using (BDADMSURTIOFFICEEntities db = new BDADMSURTIOFFICEEntities())
                    {
                        ADMPARAMETROC parametroC = db.ADMPARAMETROC.First();
                        string clienteLetra = parametroC.LETRAINI;
                        var numCliente = parametroC.NUMCLIENTE.Value;
                        int totalUsersGet = RootObjects.usuariosw.Count();
                        int savedUsers = 0;
                        int repeatRuc = 0;

                        ADMCLIENTE baseCli = db.ADMCLIENTE.Find(parametroC.CLIENTEMODELOCARRO);


                        // db.Database.Log = Console.Write;
                        using (DbContextTransaction trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                foreach (var usuario in RootObjects.usuariosw)
                                {
                                    int ruc = (from c in db.ADMCLIENTE where (c.RUC == usuario.numero_identificacion) select c).Count();

                                    if (ruc == 0)
                                    {
                                        string numeroTemp = "";

                                        ADMCLIENTE nUser = new ADMCLIENTE();
                                        if (numCliente.ToString().Length == 6)
                                        {
                                            numeroTemp = Convert.ToString(numCliente + 1);
                                        }
                                        else
                                        {
                                            numeroTemp = Convert.ToInt32(numCliente + 1).ToString("D6");
                                        }

                                        nUser.CODIGO = clienteLetra + numeroTemp;
                                        nUser.RAZONSOCIAL = usuario.nombre + " " + usuario.apellido;
                                        nUser.CLIENTEWEB = baseCli.CLIENTEWEB;
                                        nUser.CLIENTEDOMI = baseCli.CLIENTEDOMI.Trim();
                                        nUser.NEGOCIO = baseCli.NEGOCIO.Trim();
                                        nUser.REPRESENTA = usuario.nombre + " " + usuario.apellido;
                                        nUser.RUC = usuario.numero_identificacion.Trim();
                                        nUser.DIRECCION = usuario.direccion.Trim();
                                        nUser.TELEFONOS = usuario.celular1.Trim();
                                        nUser.EMAIL = usuario.correo.Trim();
                                        nUser.TIPO = usuario.idtipo.Trim();
                                        nUser.PROVINCIA = usuario.ciudad;
                                        nUser.CANTON = usuario.canton;
                                        nUser.PARROQUIA = baseCli.PARROQUIA.Trim();
                                        nUser.SECTOR = baseCli.SECTOR.Trim();
                                        nUser.TIPONEGO = baseCli.TIPONEGO.Trim();
                                        nUser.RUTA = baseCli.RUTA.Trim();
                                        nUser.FECHAING = Convert.ToDateTime(usuario.ingreso);
                                        nUser.FECNAC = Convert.ToDateTime(usuario.ingreso);
                                        nUser.ESTADO = baseCli.ESTADO.Trim();
                                        nUser.VENDEDOR = parametroC.CODVENPOS.Trim();
                                        nUser.FORMAPAG = baseCli.FORMAPAG.Trim();
                                        nUser.IVA = baseCli.IVA.Trim();
                                        nUser.BACKORDER = baseCli.BACKORDER.Trim();
                                        nUser.RETENPED = baseCli.RETENPED.Trim();
                                        nUser.RETIENEFUENTE = baseCli.RETIENEFUENTE.Trim();
                                        nUser.RETIENEIVA = baseCli.RETIENEIVA.Trim();
                                        nUser.PORDESSUGERIDO = baseCli.PORDESSUGERIDO;
                                        nUser.CONFINAL = baseCli.CONFINAL.Trim();
                                        nUser.CLASE = baseCli.CLASE.Trim();
                                        nUser.OBSERVACION = baseCli.OBSERVACION.Trim();
                                        nUser.FACTURAELECTRONICA = baseCli.FACTURAELECTRONICA;
                                        nUser.CLAVEFE = baseCli.CLAVEFE.Trim();
                                        nUser.SUBIRWEB = baseCli.SUBIRWEB.Trim();
                                        nUser.TDCREDITO = baseCli.TDCREDITO.Trim();
                                        nUser.DIASCREDIT = baseCli.DIASCREDIT;
                                        nUser.TIPOCUENTA = baseCli.TIPOCUENTA.Trim();
                                        nUser.TIPOPERSONA = baseCli.TIPOPERSONA.Trim();
                                        nUser.ZONA = baseCli.ZONA.Trim();
                                        nUser.TIPOPERSONAADICIONAL = baseCli.TIPOPERSONAADICIONAL.Trim();
                                        nUser.PAGOCUOTAS = baseCli.PAGOCUOTAS.Trim();
                                        nUser.SEXO = baseCli.SEXO.Trim();
                                        nUser.ESTADOPARAWEB = baseCli.ESTADOPARAWEB.Trim();
                                        nUser.CLIRELACIONADO = baseCli.CLIRELACIONADO.Trim();
                                        nUser.VENDEDORAUX = baseCli.VENDEDORAUX.Trim();
                                        nUser.TIPODOC = usuario.identificacion.Substring(0, 1).ToUpper();
                                        nUser.TIPOCONTRIBUYENTE = baseCli.TIPOCONTRIBUYENTE.Trim();
                                        nUser.EWEB = "S"; //baseCli.EWEB.Trim();
                                        nUser.CUPO = baseCli.CUPO;
                                        nUser.GRUPO = baseCli.GRUPO.Trim();
                                        nUser.ORDEN = baseCli.ORDEN;
                                        nUser.CODFRE = baseCli.CODFRE.Trim();
                                        nUser.CREDITO = baseCli.CREDITO.Trim();
                                        nUser.FAX = baseCli.FAX.Trim();
                                        nUser.DIA = baseCli.DIA;
                                        nUser.FECMOD = Convert.ToDateTime(usuario.ingreso);
                                        nUser.FECDESDE = Convert.ToDateTime(usuario.ingreso);
                                        nUser.CTACLIENTE = baseCli.CTACLIENTE.Trim();
                                        nUser.grupocliente = baseCli.grupocliente.Trim();
                                        nUser.grupocredito = baseCli.grupocredito.Trim();
                                        nUser.CORREOCARRO = usuario.correo.Trim();
                                        nUser.CodShip = "Carro";

                                        if (usuario.contrasenia == null)
                                        {
                                            nUser.CLAVECARRO = "";
                                        }
                                        else
                                        {
                                            nUser.CLAVECARRO = usuario.contrasenia;
                                        }



                                        db.ADMCLIENTE.Add(nUser);
                                        db.SaveChanges();
                                        numCliente++;
                                        savedUsers++;
                                    }
                                    else
                                    {
                                        //actualizar correo y contraseña de clientes desde la web -> AMD.
                                        ADMCLIENTE cliente = (from c in db.ADMCLIENTE
                                                              where (c.RUC == usuario.numero_identificacion)
                                                              select c).First();

                                        //Verificar si tiene establecida la fecha de modificacion
                                        if (cliente.FECMOD != null)
                                        {
                                            DateTime f2 = DateTime.Now.AddHours(1);
                                            var lapso = (f2 - DateTime.Now).TotalHours;
                                            //si ha transcurrido 1 hora desde la ultima actualizacion
                                            if (lapso >= 1)
                                            {
                                                cliente.CLAVECARRO = usuario.contrasenia;
                                                cliente.CORREOCARRO = usuario.correo;
                                            }
                                        }
                                        else
                                        {
                                            cliente.CLAVECARRO = usuario.contrasenia;
                                            cliente.CORREOCARRO = usuario.correo;
                                            cliente.FECMOD = DateTime.Now;
                                        }
                                        db.SaveChanges();
                                        repeatRuc++;
                                    }

                                }
                                //db.CARROPRO("NumeroCliente", numCliente);
                                db.Database.ExecuteSqlCommand("UPDATE ADMPARAMETROC set NUMCLIENTE = @num", new SqlParameter("@num", numCliente));
                                db.SaveChanges();
                                trans.Commit();

                            }
                            catch (Exception e)
                            {

                                trans.Rollback();
                                //Console.WriteLine(e);
                                using (StreamWriter file = new StreamWriter(@"log.txt", true))
                                {
                                    Console.WriteLine("Error GetUsuarios: " + e);
                                    file.WriteLine("Error GetUsuarios: " + e);
                                    file.Close();
                                }
                            }
                            using (StreamWriter file = new StreamWriter(@"log.txt", true))
                            {
                                Console.WriteLine("Se obtuvieron " + totalUsersGet + " Usuarios de la Web, " + savedUsers + " nuevos fueron almacenados en ADM; " + repeatRuc + " RUC repetidos/Actualizados.");
                                file.WriteLine("Se obtuvieron " + totalUsersGet + " Usuarios de la Web, " + savedUsers + " nuevos fueron almacenados en ADM; " + repeatRuc + " RUC repetidos/Actualizados.");
                                file.Close();
                            }
                            //Console.WriteLine("-------");
                            //Console.WriteLine("Se obtuvieron " + totalUsersGet + " Usuarios de la Web, " + savedUsers + " fueron almacenados en ADM; " + repeatRuc + " RUC repetidos.");
                        }

                    }

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
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);
                var response = await client.PostAsync(
                    ul,
                     new StringContent(myJson, Encoding.UTF8, "application/json"));

                HttpContent contentRes = response.Content;
                string mycontent = await contentRes.ReadAsStringAsync();
                var RootObjects = JsonConvert.DeserializeObject<List<clientResponse>>(mycontent);

                using (BDADMSURTIOFFICEEntities db = new BDADMSURTIOFFICEEntities())
                {
                    using (DbContextTransaction trans = db.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var userRes in RootObjects)
                            {
                                if (userRes.status == "OK")
                                {
                                    ADMCLIENTE result = (from c in db.ADMCLIENTE
                                                         where (c.RUC == userRes.identify)
                                                         select c).First();
                                    result.EWEB = "S";
                                    //db.ADMCLIENTE.Add(result);
                                    db.SaveChanges();
                                }
                            }
                            trans.Commit();
                        }
                        catch (Exception e)
                        {

                            trans.Rollback();
                            using (StreamWriter file = new StreamWriter(@"log.txt", true))
                            {
                                file.WriteLine("Error al PostClientes: " + e);
                                file.Close();
                            }
                            //Console.WriteLine(e);
                            throw e;
                        }

                    }

                }

                int noGuardado = (from c in RootObjects where (c.status == "NoSaved") select c).Count();
                int guardado = (from c in RootObjects where (c.status == "OK") select c).Count();

                using (StreamWriter file = new StreamWriter(@"log.txt", true))
                {
                    file.WriteLine("Envio de Usuarios Finalizado, total Guardado: " + guardado + ", No Guardados: " + noGuardado);
                    file.Close();
                }

                Console.WriteLine("-------");
                Console.WriteLine("Envio de Usuarios Finalizado, total Guardado: " + guardado + ", No Guardados: " + noGuardado);

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
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);
                var response = await client.PostAsync(ul, new StringContent(myJson, Encoding.UTF8, "application/json"));

                HttpContent contentRes = response.Content;
                string mycontent = await contentRes.ReadAsStringAsync();
                var RootObjects = JsonConvert.DeserializeObject<List<categoryResponse>>(mycontent);

                using (BDADMSURTIOFFICEEntities db = new BDADMSURTIOFFICEEntities())
                {
                    foreach (var cateRes in RootObjects)
                    {
                        ADMCATEGORIA result = db.ADMCATEGORIA.Find(cateRes.categoria);
                        result.EWEB = "S";
                        db.SaveChanges();
                    }


                    int noGuardado = (from c in RootObjects where (c.status == "NoSaved") select c).Count();
                    int guardado = (from c in RootObjects where (c.status == "OK") select c).Count();

                    using (StreamWriter file = new StreamWriter(@"log.txt", true))
                    {
                        file.WriteLine("Envio de Categorias Finalizado, total Guardado: " + guardado + ", No Guardados: " + noGuardado);
                        file.Close();
                    }
                    Console.WriteLine("-------");
                    Console.WriteLine("Envio de Categorias Finalizado, total Guardado: " + guardado + ", No Guardados: " + noGuardado);
                    string urlbase = ConfigurationManager.AppSettings.Get("urlbase");

                    //Listado de Categorias
                    List<ADMFAMILIA> familias = (from c in db.ADMFAMILIA
                                                 where (c.EWEB == "N" && c.ESTADO == "A")
                                                 select c).ToList();

                    List<FamiliWeb> listfami = new List<FamiliWeb>();
                    foreach (var familia in familias)
                    {
                        FamiliWeb famiSingle = new FamiliWeb();
                        famiSingle.idfamilia = familia.CODIGO.Trim();
                        famiSingle.nombre_familia = familia.NOMBRE.Trim();
                        famiSingle.idcategoria = familia.CATEGORIA.Trim();
                        famiSingle.estado = familia.ESTADO;
                        listfami.Add(famiSingle);

                    }
                    Uri uFami = new Uri(urlbase + "api/familia");
                    await Task.Run(() => PostFamilias(listfami, uFami));

                }
            }
        }

        class familyResponse
        {
            public string familia { get; set; }
            public string status { get; set; }
        }
        //Envio de Familia.
        private static async Task PostFamilias(object content, Uri ul)
        {
            var data = new
            {
                familias = content
            };

            string myJson = JsonConvert.SerializeObject(data);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);
                var response = await client.PostAsync(ul, new StringContent(myJson, Encoding.UTF8, "application/json"));
                HttpContent contentRes = response.Content;
                string mycontent = await contentRes.ReadAsStringAsync();
                Console.WriteLine(mycontent);

                var RootObjects = JsonConvert.DeserializeObject<List<familyResponse>>(mycontent);

                using (BDADMSURTIOFFICEEntities db2 = new BDADMSURTIOFFICEEntities())
                {
                    foreach (var famiRes in RootObjects)
                    {
                        ADMFAMILIA resultf = (from c in db2.ADMFAMILIA where (c.CODIGO == famiRes.familia) select c).FirstOrDefault();
                        resultf.EWEB = "S";
                        db2.SaveChanges();
                    }
                }

                int noGuardado = (from c in RootObjects where (c.status == "NoSaved") select c).Count();
                int guardado = (from c in RootObjects where (c.status == "OK") select c).Count();

                using (StreamWriter file = new StreamWriter(@"log.txt", true))
                {
                    file.WriteLine("Envio de Familias Finalizado, total Guardado: " + guardado + ", No Guardados: " + noGuardado);
                    file.Close();
                }
                Console.WriteLine("-------");
                Console.WriteLine("Envio de Familias Finalizado, total Guardado: " + guardado + ", No Guardados: " + noGuardado);

            }
        }


        //Envio de Provincias
        private static async Task PostProvincias(object content, Uri ul)
        {
            var data = new
            {
                provincias = content
            };

            string myJson = JsonConvert.SerializeObject(data);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);
                var response = await client.PostAsync(ul, new StringContent(myJson, Encoding.UTF8, "application/json"));
                HttpContent contentRes = response.Content;
                string mycontent = await contentRes.ReadAsStringAsync();
                //Console.WriteLine(mycontent);

                var RootObjects = JsonConvert.DeserializeObject<List<familyResponse>>(mycontent);

                //using (BDADMSURTIOFFICEEntities db2 = new BDADMSURTIOFFICEEntities())
                //{
                //    foreach (var proviRes in RootObjects)
                //    {
                //        ADMFAMILIA resultf = (from c in db2.ADMFAMILIA where (c.CODIGO == famiRes.familia) select c).FirstOrDefault();
                //        resultf.EWEB = "S";
                //        db2.SaveChanges();
                //    }
                //}

                //int noGuardado = (from c in RootObjects where (c.status == "NoSaved") select c).Count();
                //int guardado = (from c in RootObjects where (c.status == "OK") select c).Count();

                using (StreamWriter file = new StreamWriter(@"log.txt", true))
                {
                    file.WriteLine("Envio de Provincas Finalizado");
                    file.Close();
                }
                Console.WriteLine("-------");
                Console.WriteLine("Envio de Provincias Finalizado");

            }
        }

        //envio de Cantones

        private static async Task PostCantones(object content, Uri ul)
        {
            var data = new
            {
                cantones = content
            };

            string myJson = JsonConvert.SerializeObject(data);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);
                var response = await client.PostAsync(ul, new StringContent(myJson, Encoding.UTF8, "application/json"));
                HttpContent contentRes = response.Content;
                string mycontent = await contentRes.ReadAsStringAsync();
                Console.WriteLine(mycontent);

                var RootObjects = JsonConvert.DeserializeObject<List<familyResponse>>(mycontent);

                //using (BDADMSURTIOFFICEEntities db2 = new BDADMSURTIOFFICEEntities())
                //{
                //    foreach (var proviRes in RootObjects)
                //    {
                //        ADMFAMILIA resultf = (from c in db2.ADMFAMILIA where (c.CODIGO == famiRes.familia) select c).FirstOrDefault();
                //        resultf.EWEB = "S";
                //        db2.SaveChanges();
                //    }
                //}

                //int noGuardado = (from c in RootObjects where (c.status == "NoSaved") select c).Count();
                //int guardado = (from c in RootObjects where (c.status == "OK") select c).Count();

                using (StreamWriter file = new StreamWriter(@"log.txt", true))
                {
                    file.WriteLine("Envio de Cantones Finalizado");
                    file.Close();
                }
                Console.WriteLine("-------");
                Console.WriteLine("Envio de Cantones Finalizado");

            }
        }

        class MarcaResponse
        {
            public string marca { get; set; }
            public string status { get; set; }
        }
        //Envio de Marcas
        private static async Task PostMarcas(object content, Uri ul)
        {
            var data = new
            {
                marcas = content
            };

            string myJson = JsonConvert.SerializeObject(data);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);
                var response = await client.PostAsync(ul, new StringContent(myJson, Encoding.UTF8, "application/json"));

                HttpContent contentRes = response.Content;
                string mycontent = await contentRes.ReadAsStringAsync();

                var RootObjects = JsonConvert.DeserializeObject<List<MarcaResponse>>(mycontent);

                using (BDADMSURTIOFFICEEntities db = new BDADMSURTIOFFICEEntities())
                {
                    foreach (var marcaRes in RootObjects)
                    {
                        ADMMARCA result = db.ADMMARCA.Find(marcaRes.marca);
                        result.EWEB = "S";
                        db.SaveChanges();
                    }
                }

                int noGuardado = (from c in RootObjects where (c.status == "NoSaved") select c).Count();
                int guardado = (from c in RootObjects where (c.status == "OK") select c).Count();
                using (StreamWriter file = new StreamWriter(@"log.txt", true))
                {
                    file.WriteLine("Envio de Marcas Finalizado, total Guardado: " + guardado + ", No Guardados: " + noGuardado);
                    file.Close();
                }
                Console.WriteLine("-------");
                Console.WriteLine("Envio de Marcas Finalizado, total Guardado: " + guardado + ", No Guardados: " + noGuardado);

            }
        }

        class productResponse
        {
            public string product { get; set; }
            public string status { get; set; }
        }

        //Envio de productos
        private static async Task PostProducto(object content, Uri ul)
        {
            var data = new
            {
                productos = content
            };

            string myJson = JsonConvert.SerializeObject(data);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);
                var response = await client.PostAsync(ul, new StringContent(myJson, Encoding.UTF8, "application/json"));

                HttpContent contentRes = response.Content;
                string mycontent = await contentRes.ReadAsStringAsync();
                Console.WriteLine(mycontent);

                var RootObjects = JsonConvert.DeserializeObject<List<productResponse>>(mycontent);

                using (BDADMSURTIOFFICEEntities db = new BDADMSURTIOFFICEEntities())
                {
                    foreach (var productRes in RootObjects)
                    {
                        ADMITEM result = db.ADMITEM.Find(productRes.product);
                        result.ESTAENCARRO = "S";
                        db.SaveChanges();
                    }
                }

                int noGuardado = (from c in RootObjects where (c.status == "NoSaved") select c).Count();
                int guardado = (from c in RootObjects where (c.status == "OK") select c).Count();

                using (StreamWriter file = new StreamWriter(@"log.txt", true))
                {
                    file.WriteLine("Envio de Productos Finalizado, total Guardado: " + guardado + ", No Guardados: " + noGuardado);
                    file.Close();
                }

                Console.WriteLine("-------");
                Console.WriteLine("Envio de Productos Finalizado, total Guardado: " + guardado + ", No Guardados: " + noGuardado);
            }
        }


        class tipoclientResponse
        {
            public string tiposClientes { get; set; }
            public string status { get; set; }
        }
        //Envio de tipos de Clientes-Precio
        private static async Task PostClientePrecio(object content, Uri ul)
        {
            var data = new
            {
                clientePrecio = content
            };

            string myJson = JsonConvert.SerializeObject(data);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);
                var response = await client.PostAsync(ul, new StringContent(myJson, Encoding.UTF8, "application/json"));

                HttpContent contentRes = response.Content;
                string mycontent = await contentRes.ReadAsStringAsync();
                               
                var RootObjects = JsonConvert.DeserializeObject<List<tipoclientResponse>>(mycontent);
               
                int actualizado = (from c in RootObjects where (c.status == "Updated") select c).Count();
                int guardado = (from c in RootObjects where (c.status == "Created") select c).Count();
              
                using (StreamWriter file = new StreamWriter(@"log.txt", true))
                {
                    //Console.WriteLine(actualizado);
                    file.WriteLine("Envio de TiposClientes Finalizado, total Guardado: " + guardado + ", Actualizados: " + actualizado);
                    file.Close();
                }

            }

        }
    }
}