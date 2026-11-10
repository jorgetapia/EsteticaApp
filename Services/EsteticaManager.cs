using System;
using System.Collections.Generic;
using System.Linq;
using EsteticaApp.Models;

namespace EsteticaApp.Services
{
    public class EsteticaManager
    {

        private List<Cita> _agendaCitas = new List<Cita>();

        // 2. Estructura de Datos: Cola (Queue) para la Sala de Espera en tiempo real (FIFO)
        private Queue<Cita> _salaEspera = new Queue<Cita>();

        // 3. Estructura de Datos para el Corte de Caja Diario (Historial de transacciones)
        private List<TransaccionPago> _cajaDiaria = new List<TransaccionPago>();


        public IEnumerable<Cita> ObtenerTodasLasCitas()
        {
            return _agendaCitas;
        }

        public void AgendarCita(Cita nuevaCita)
        {
          
            bool existeCruce = _agendaCitas.Any(c => c.EstilistaAsignado.IdEstilista == nuevaCita.EstilistaAsignado.IdEstilista
                                                 && c.FechaHora == nuevaCita.FechaHora
                                                 && c.Estado != "Cancelada");
            if (existeCruce)
            {
                throw new InvalidOperationException("El estilista ya tiene una cita asignada en ese horario.");
            }

            _agendaCitas.Add(nuevaCita);
        }

        public void RegistrarLlegadaACola(int idCita)
        {
            var cita = _agendaCitas.FirstOrDefault(c => c.IdCita == idCita);
            if (cita != null && cita.Estado == "Programada")
            {
                cita.Estado = "EnEspera";
                _salaEspera.Enqueue(cita); // Encolar (Enqueue)
            }
        }

        public Cita? AtenderSiguienteCliente()
        {
            if (_salaEspera.Count > 0)
            {
                var citaAtendida = _salaEspera.Dequeue(); // Desencolar (Dequeue)
                citaAtendida.Estado = "EnAtencion";
                return citaAtendida;
            }
            return null;
        }

        public int ObtenerClientesEnEsperaCount() => _salaEspera.Count;

        public IEnumerable<Cita> ObtenerFilaEspera() => _salaEspera;


        public TransaccionPago CobrarServicio(int idCita, string metodoPago)
        {
            var cita = _agendaCitas.FirstOrDefault(c => c.IdCita == idCita);
            if (cita == null) throw new ArgumentException("Cita no encontrada.");

            cita.Estado = "Finalizada";

            
            decimal comisionEstilista = cita.Servicio.CalcularComision(cita.EstilistaAsignado.PorcentajeComision);

            var transaccion = new TransaccionPago
            {
                IdCita = cita.IdCita,
                CitaAsociada = cita,
                MontoTotal = cita.Servicio.Precio,
                ComisionEstilista = comisionEstilista,
                MetodoPago = metodoPago,
                FechaPago = DateTime.Now
            };

            _cajaDiaria.Add(transaccion);
            return transaccion;
        }


        public decimal ObtenerIngresosTotalesDia()
        {
            return _cajaDiaria.Sum(t => t.MontoTotal);
        }

        public Dictionary<string, decimal> ObtenerComisionesPorEstilista()
        {
            return _cajaDiaria
                .GroupBy(t => t.CitaAsociada.EstilistaAsignado.Nombre)
                .ToDictionary(g => g.Key, g => g.Sum(t => t.ComisionEstilista));
        }
    }
}