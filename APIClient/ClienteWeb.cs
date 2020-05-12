using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIClient
{
    class ClienteWeb
    {
        public string idusuario { get; set; }
        public string activacion { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string correo { get; set; }
        public string contrasenia { get; set; }
        public string identificacion { get; set; }
        public string numero_identificacion { get; set; }
        public string direccion { get; set; }
        public string referencia { get; set; }
        public string pais { get; set; }
        public string ciudad { get; set; }
        public string codigo_postal { get; set; }
        public string celular1 { get; set; }
        public string celular2 { get; set; }
        public string imagen { get; set; }
        public string img_servicios { get; set; }
        public string img_representante { get; set; }
        public string img_cedula { get; set; }
        public string empresa { get; set; }
        public string ruc { get; set; }
        public string idtipo { get; set; }
        public string ingreso { get; set; }
        public string canton { get; set; }
    }

    class ClienteRes
    {
        public string status { get; set; }
        public string fecha { get; set; }
        public List<ClienteWeb> usuariosw { get; set; }
    }
}
