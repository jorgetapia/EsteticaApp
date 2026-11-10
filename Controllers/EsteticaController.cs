using Microsoft.AspNetCore.Mvc;
using EsteticaApp.Models;
using EsteticaApp.Services;

namespace EsteticaApp.Web.Controllers
{
    public class EsteticaController : Controller
    {
        private static readonly EsteticaManager _manager = new EsteticaManager();

        private static bool _datosCargados = false;

        public EsteticaController()
        {
            // Cargar datos dummy una sola vez al iniciar la ejecución
            if (!_datosCargados)
            {
                InicializadorDatosDummy.CargarDatosIniciales(_manager);
                _datosCargados = true;
            }
        }
        public IActionResult Index()
        {
            ViewBag.TotalEnEspera = _manager.ObtenerClientesEnEsperaCount();
            ViewBag.IngresosDia = _manager.ObtenerIngresosTotalesDia();
            return View(_manager.ObtenerFilaEspera());
        }

        [HttpPost]
        public IActionResult RegistrarLlegada(int idCita)
        {
            _manager.RegistrarLlegadaACola(idCita);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult AtenderSiguiente()
        {
            var atendido = _manager.AtenderSiguienteCliente();
            if (atendido != null)
                TempData["Mensaje"] = $"Atendiendo a: {atendido.Cliente.Nombre}";
            else
                TempData["Error"] = "La cola está vacía.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Cobrar(int idCita, string metodoPago)
        {
            var t = _manager.CobrarServicio(idCita, metodoPago);
            TempData["Mensaje"] = $"Cobrado: ${t.MontoTotal:N2} (Comisión: ${t.ComisionEstilista:N2})";
            return RedirectToAction(nameof(Index));
        }


        // GET: /Estetica/Agenda
        public IActionResult Agenda()
        {
            // Obtener todas las citas registradas en el gestor
            var citas = _manager.ObtenerTodasLasCitas();
            return View(citas);
        }

        // POST: /Estetica/AgendarCita
        [HttpPost]
        public IActionResult AgendarCita(string nombreCliente, int idEstilista, int idServicio, DateTime fechaHora)
        {
            try
            {
                // Lógica de armado de cita y guardado
                var cliente = new Cliente { IdCliente = new Random().Next(100, 999), Nombre = nombreCliente };

                // Simulación de búsqueda de entidades dummy
                Estilista estilista = idEstilista == 1
                    ? new Estilista { IdEstilista = 1, Nombre = "Sofía Ramírez", PorcentajeComision = 0.40m }
                    : new Estilista { IdEstilista = 2, Nombre = "Carlos Mendoza", PorcentajeComision = 0.35m };

                Servicio servicio = idServicio == 1
                    ? new ServicioCorte { IdServicio = 1, Nombre = "Corte Estilizado", Precio = 250.00m }
                    : new ServicioTinte { IdServicio = 2, Nombre = "Tinte Completo", Precio = 850.00m };

                var nuevaCita = new Cita
                {
                    IdCita = new Random().Next(1000, 9999),
                    Cliente = cliente,
                    EstilistaAsignado = estilista,
                    Servicio = servicio,
                    FechaHora = fechaHora,
                    Estado = "Programada"
                };

                _manager.AgendarCita(nuevaCita);
                TempData["Mensaje"] = "Cita agendada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Agenda));
        }

        // GET: /Estetica/CorteCaja
        public IActionResult CorteCaja()
        {
            ViewBag.IngresosDia = _manager.ObtenerIngresosTotalesDia();
            var comisiones = _manager.ObtenerComisionesPorEstilista();
            ViewBag.ComisionesPorEstilista = comisiones;
            ViewBag.TotalComisiones = comisiones.Values.Sum();

            return View();
        }
    }
}