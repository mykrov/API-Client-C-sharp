using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIClient
{
    public class CabeceraWeb
    {
        public int idventas { get; set; }
        public int iddetalle_venta { get; set; }
        public  double subtotal { get; set; }
        public double iva { get; set; }
        public double costo_envio { get; set; }
        public double envio_gratuito { get; set; }
        public double  total { get; set; }
        public string fecha  { get; set; }
        public string  estado { get; set; }
        public string Graba_iva { get; set; }
        public string  token { get; set; }
        public string idusuario { get; set; }
        public string ruc { get; set; }
    }

    public class DetalleWeb
    {
        public int idcompra { get; set; }
        public int cantidad { get; set; }
        public double precio { get; set; }
        public double subtotal { get; set; }
        public double iva { get; set; }
        public double costo_envio { get; set; }
        public double envio_gratuio { get; set; }
        public int iddetalle_venta { get; set; }
        public int idproducto { get; set; }
        public double valor_neto { get; set; }
        public string graba_iva { get; set; }
        public string idusuario { get; set; }
        public string  estado { get; set; }
        public int idventa { get; set; }
      

    }


    public class PedidoResponse
    {
        public string status { get; set; }
        public List<CabeceraWeb> pedidos { get; set; }
        public List<DetalleWeb> detalles { get; set; }
    }
}
