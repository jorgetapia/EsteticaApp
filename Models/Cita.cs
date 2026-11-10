namespace EsteticaApp.Models
{
    public class Cita
    {
        public int IdCita { get; set; }
        public int IdCliente { get; set; }
        public Cliente Cliente { get; set; } = null!;
        public int IdServicio { get; set; }
        public Servicio Servicio { get; set; } = null!;
        public int IdEstilista { get; set; }
        public Estilista EstilistaAsignado { get; set; } = null!;
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; } = "Programada"; // Programada, EnEspera, EnAtencion, Finalizada, Cancelada
    }
}
