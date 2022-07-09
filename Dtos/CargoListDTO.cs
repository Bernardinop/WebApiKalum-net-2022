namespace WebApiKalum.Dtos
{
    public class CargoListDTO
    {
        public string CargoId { get; set; }
        public string Descripcion { get; set; }
        public string Prefijo { get; set; }
        public decimal Monto { get; set; }
        public bool GeneraMora { get; set; }
        public int PorcentajeMora { get; set; }
        public List<CuentaPorCobrarListDTO> CuentaPorCobrar { get; set; }
    }
}