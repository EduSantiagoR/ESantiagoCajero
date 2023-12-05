using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Retiro
    {
        public int IdRetiro { get; set; }
        public DateTime FechaRetiro { get; set; }
        public int Billetes1000 { get; set; }
        public int Billetes500 { get; set; }
        public int Billetes200 { get; set; }
        public int Billetes100 { get; set; }
        public int Billetes50 { get; set; }
        public int Billetes20 { get; set; }
        public int TotalRetiro { get; set; }
        public List<object> Retiros { get; set; }
    }
}
