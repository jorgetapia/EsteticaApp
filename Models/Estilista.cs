namespace EsteticaApp.Models
{
    public class Estilista
    {
        public int IdEstilista { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal PorcentajeComision { get; set; } // Ej: 0.40 para 40%
    }
}
