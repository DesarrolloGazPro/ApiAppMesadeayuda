namespace ApiAppMesaDeAyuda.Modelos
{
    public class Fallas
    {
        public int id { get; set; }
        public string Falla { get; set; }
        public int Prioridad { get; set; }
        public int Tiempo { get; set; }
        public int DepartamentoID { get; set; }
        public int CategoriaID { get; set; }
        public int clasificacionID { get; set; }
        
    }
}
