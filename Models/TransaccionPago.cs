namespace EsteticaApp.Models
{
    public class TransaccionPago
    {
        public int IdTicket { get; set; }
        public int IdCita { get; set; }
        public Cita CitaAsociada { get; set; } = null!;
        public decimal MontoTotal { get; set; }
        public decimal ComisionEstilista { get; set; }
        public string MetodoPago { get; set; } = "Efectivo"; // Efectivo, Tarjeta
        public DateTime FechaPago { get; set; } = DateTime.Now;
    }
}
