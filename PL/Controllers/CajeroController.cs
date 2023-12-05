using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PL.Controllers
{
    public class CajeroController : Controller
    {
        public ActionResult Cajero()
        {
            ML.Result result = BL.SaldoCajero.GetSaldo();
            ML.SaldoCajero cajero = result.Object as ML.SaldoCajero;
            return View(cajero);
        }
    }
}