using System;
using System.Collections.Generic;
using EsteticaApp.Models;

namespace EsteticaApp.Services
{
    public static class InicializadorDatosDummy
    {
        public static void CargarDatosIniciales(EsteticaManager manager)
        {
            // 1. Crear Estilistas de prueba
            var estilista1 = new Estilista { IdEstilista = 1, Nombre = "Sofía Ramírez", PorcentajeComision = 0.40m };
            var estilista2 = new Estilista { IdEstilista = 2, Nombre = "Carlos Mendoza", PorcentajeComision = 0.35m };

            // 2. Crear Clientes de prueba (Usando la clase Cliente que seleccionaste)
            var cliente1 = new Cliente { IdCliente = 1, Nombre = "María López", Telefono = "4151234567", Correo = "maria@correo.com" };
            var cliente2 = new Cliente { IdCliente = 2, Nombre = "Juan Pérez", Telefono = "4159876543", Correo = "juan@correo.com" };
            var cliente3 = new Cliente { IdCliente = 3, Nombre = "Ana Torres", Telefono = "4155551234", Correo = "ana@correo.com" };

            // 3. Crear Servicios (Aplicando polimorfismo: Corte y Tinte con reglas distintas)
            Servicio corte = new ServicioCorte { IdServicio = 1, Nombre = "Corte Estilizado", Precio = 250.00m, DuracionMinutos = 30 };
            Servicio tinte = new ServicioTinte { IdServicio = 2, Nombre = "Tinte Completo", Precio = 850.00m, DuracionMinutos = 120 };

            // 4. Crear Citas de prueba y registrarlas en el manager
            var cita1 = new Cita
            {
                IdCita = 101,
                IdCliente = cliente1.IdCliente,
                Cliente = cliente1,
                IdServicio = corte.IdServicio,
                Servicio = corte,
                IdEstilista = estilista1.IdEstilista,
                EstilistaAsignado = estilista1,
                FechaHora = DateTime.Now.AddHours(1),
                Estado = "Programada"
            };

            var cita2 = new Cita
            {
                IdCita = 102,
                IdCliente = cliente2.IdCliente,
                Cliente = cliente2,
                IdServicio = tinte.IdServicio,
                Servicio = tinte,
                IdEstilista = estilista2.IdEstilista,
                EstilistaAsignado = estilista2,
                FechaHora = DateTime.Now.AddHours(2),
                Estado = "Programada"
            };

            // Agregamos las citas al gestor
            manager.AgendarCita(cita1);
            manager.AgendarCita(cita2);

            // Opcional: Mandar la primera cita directo a la sala de espera (Cola FIFO) para simular que el cliente ya llegó
            manager.RegistrarLlegadaACola(cita1.IdCita);
        }
    }
}
