using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Retiro
    {
        public static ML.Result GetRetiros()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.ESantiagoCajeroEntities context = new DL.ESantiagoCajeroEntities())
                {
                    var query = context.RetiroGetAll().ToList();
                    if(query != null)
                    {
                        result.Objects = new List<object>();
                        foreach(var item in query)
                        {
                            ML.Retiro retiro = new ML.Retiro();
                            retiro.IdRetiro = item.IdRetiro;
                            retiro.FechaRetiro = item.FechaRetiro;
                            retiro.Billetes1000 = item.Billetes1000.Value;
                            retiro.Billetes500 = item.Billetes500.Value;
                            retiro.Billetes200 = item.Billetes200.Value;
                            retiro.Billetes100 = item.Billetes100.Value;
                            retiro.Billetes50 = item.Billetes50.Value;
                            retiro.Billetes20 = item.Billetes20.Value;
                            retiro.TotalRetiro = item.TotalRetiro.Value;
                            result.Objects.Add(retiro);
                        }
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
        public static ML.Result Retirar(ML.Retiro retiro)
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.ESantiagoCajeroEntities context = new DL.ESantiagoCajeroEntities())
                {
                    int rowsAffected = context.RealizarRetiro(retiro.Billetes1000, retiro.Billetes500, retiro.Billetes200, retiro.Billetes100, retiro.Billetes50, retiro.Billetes20);
                    if(rowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se ha podido realizar el retiro.";
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
