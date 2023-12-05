using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace PL.Controllers
{
    public class RetiroController : Controller
    {
        // GET: Retiro
        public ActionResult Retiro()
        {
            ML.Result result = BL.Retiro.GetRetiros();
            ML.Retiro retiro = new ML.Retiro();
            retiro.Retiros = result.Objects;
            return View(retiro);
        }

        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Form(int cantidad)
        {
            if(cantidad == 30)
            {
                ViewBag.Mensaje = "No hay denominaciones para cubrir esa cantidad.\n Por favor, ingresa una cantidad diferente.";
                ViewBag.Correct = false;
            }
            else
            {

                ML.Result result = BL.SaldoCajero.GetSaldo();
                ML.SaldoCajero billetesDisponibles = result.Object as ML.SaldoCajero;
                ML.Retiro retiro = ObtenerBilletes(cantidad, billetesDisponibles);

                bool cumple = Verificar(cantidad, retiro);
                if (cumple)
                {
                    ML.Result resultadoOpreacion = BL.Retiro.Retirar(retiro);
                    if (resultadoOpreacion.Correct)
                    {
                        ViewBag.Mensaje = $"Retiro éxitoso. Fecha y hora: {DateTime.Now}. Cantidad: $ {cantidad}";
                        ViewBag.Correct = true;
                    }
                    else
                    {
                        ViewBag.Mensaje = resultadoOpreacion.ErrorMessage;
                        ViewBag.Correct = false;
                    }
                    result = BL.SaldoCajero.GetSaldo();
                    billetesDisponibles = result.Object as ML.SaldoCajero;
                    ValidarExistencias(billetesDisponibles);
                }
                else
                {
                    ViewBag.Mensaje = "No hay demoninaciones disponibles para cubrir esa cantidad. No es posible realizar el retiro.";
                    ViewBag.Correct = false;
                }
            }
            return PartialView("Modal");
        }

        //Método para obtener la cantidad de billetes a retirar
        private ML.Retiro ObtenerBilletes(int cantidad, ML.SaldoCajero saldoCajero)
        {
            ML.Retiro retiro = new ML.Retiro();
            Dictionary<int,int> billetesDisponibles = new Dictionary<int, int>
            {
                { 1000,saldoCajero.Billetes1000 },
                { 500, saldoCajero.Billetes500 },
                { 200, saldoCajero.Billetes200 },
                { 100, saldoCajero.Billetes100 },
                { 50, saldoCajero.Billetes50 },
                { 20, saldoCajero.Billetes20 }
            };
            List<int> billetes = new List<int>(billetesDisponibles.Keys);
            Dictionary<int, int> resultado = new Dictionary<int, int>()
            {
                {1000,0},
                {500,0},
                {200,0},
                {100,0},
                {50,0},
                {20,0},
            };
            foreach(var denominacion in billetes)
            {
                int cantidadDisponible = billetesDisponibles[denominacion];
                int cantidadDeBilletes = Math.Min(cantidad / denominacion, cantidadDisponible);
                if(cantidadDeBilletes > 0)
                {
                    resultado[denominacion] += cantidadDeBilletes;
                    cantidad -= cantidadDeBilletes * denominacion;
                    billetesDisponibles[denominacion] -= cantidadDeBilletes;
                    if(cantidad == 0)
                    {
                        break;
                    }
                }
            }
            retiro.Billetes1000 = resultado[1000];
            retiro.Billetes500 = resultado[500];
            retiro.Billetes200 = resultado[200];
            retiro.Billetes100 = resultado[100];
            retiro.Billetes50 = resultado[50];
            retiro.Billetes20 = resultado[20];
            return retiro;
        }
        //Método para verificar que la cantidad de billetes cubre el total del retiro solicitado.
        private bool Verificar(int cantidad, ML.Retiro retiro)
        {
            bool correct = false;
            int resultado = retiro.Billetes1000 * 1000 + retiro.Billetes500 * 500 + retiro.Billetes200 * 200 + retiro.Billetes100 * 100 + retiro.Billetes50 * 50 + retiro.Billetes20 * 20;
            if(cantidad == resultado) 
            {
                correct = true;
            }
            return correct;
        }
        private ML.SaldoCajero ValidarExistencias(ML.SaldoCajero saldoCajero)
        {
            if(saldoCajero.Billetes1000 == 0)
            {
                ViewBag.Mensaje += " Se han terminado los billetes de 1,000. ";
            }
            if(saldoCajero.Billetes500 == 0)
            {
                ViewBag.Mensaje += " Se han terminado los billetes de 500. ";
            }
            if(saldoCajero.Billetes200 == 0)
            {
                ViewBag.Mensaje += " Se han terminado los billetes de 200. ";
            }
            if(saldoCajero.Billetes100 == 0)
            {
                ViewBag.Mensaje += " Se han terminado los billetes de 100. ";
            }
            if(saldoCajero.Billetes50 == 0)
            {
                ViewBag.Mensaje += " Se han terminado los billetes de 50. ";
            }
            if(saldoCajero.Billetes20  == 0)
            {
                ViewBag.Mensaje += " Se han terminado los billetes de 20. ";
            }
            return saldoCajero;
        }
    }
}