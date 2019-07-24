using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIClient
{
    class ADMCLIPRECIO
    {
        public Decimal? PRECIO { get; set; }
        public string TIPO { get; set; }
        public DateTime? FECHA { get; set; }
        public string OPERADOR { get; set; }
        public int? ORDEN { get; set; }
    }
}
