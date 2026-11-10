namespace EsteticaApp.Models
{
    public abstract class Servicio
    {
        public int IdServicio { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int DuracionMinutos { get; set; }

        // Polimorfismo: método abstracto que cada tipo de servicio calcula de forma distinta
        public abstract decimal CalcularComision(decimal porcentajeEstilista);
    }

    public class ServicioTinte : Servicio
    {
        public override decimal CalcularComision(decimal porcentajeEstilista)
        {
            // Bono fijo de $50 por manejo de quimicos + porcentaje base
            return (Precio * porcentajeEstilista) + 50.00m;
        }
    }
}
