using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIClient
{
    class ProductoWeb
    {
        public string idproducto { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }
        public double precio { get; set; }
        public double precio_anterior { get; set; }
        public double precio2 { get; set; }
        public double precio3 { get; set; }
        public double precio4 { get; set; }
        public double precio5 { get; set; }
        public float total_con_iva { get; set; }
        public float total_con_iva2 { get; set; }
        public float total_oferta_iva { get; set; }
        public float total_oferta_iva2 { get; set; }
        public string imagen { get; set; }
        public float porcentaje_oferta { get; set; }
        public float iva { get; set; }
        public double oferta { get; set; }
        public string idcategoria { get; set; }
        public string idfamilia { get; set; }
        public string idmarca { get; set; }
        public decimal stock { get; set; }
        public float costo_envio { get; set; }
        public float envio_gratuito { get; set; }
        public string recomendado { get; set; }
        public string idcolor { get; set; }
        public string Graba_Iva { get; set; }
    }
}
