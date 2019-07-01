using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIClient
{
    class Respuesta
    {
        public string status { get; set; }
        public string fecha { get; set; }
        public List<ClienteWeb> usuarios {get; set;}
    }
}
