using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class SaldoCajero
    {
        public static ML.Result GetSaldo()
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.ESantiagoCajeroEntities context = new DL.ESantiagoCajeroEntities())
                {
                    var query = context.CajeroGetSaldo().AsEnumerable().FirstOrDefault();
                    if(query != null)
                    {
                        result.Object = new object();
                        ML.SaldoCajero cajero = new ML.SaldoCajero();
                        cajero.IdCajero = query.IdCajero;
                        cajero.Billetes1000 = query.Billetes1000.Value;
                        cajero.Billetes500 = query.Billetes500.Value;
                        cajero.Billetes200 = query.Billetes200.Value;
                        cajero.Billetes100 = query.Billetes100.Value;
                        cajero.Billetes50 = query.Billetes50.Value;
                        cajero.Billetes20 = query.Billetes20.Value;
                        cajero.SaldoTotal = query.SaldoTotal.Value;
                        result.Object = cajero;
                        result.Correct = true;
                    }
                }
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
    }
}
