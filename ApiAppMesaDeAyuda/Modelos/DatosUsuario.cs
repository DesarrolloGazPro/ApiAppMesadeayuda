namespace ApiAppMesaDeAyuda.Modelos
{
    public class DatosUsuario
    {
        public int Id { get; set; } // int
        public string Email { get; set; } // NVARCHAR(255)
        public string Name { get; set; } // NVARCHAR(255)
        public string Lastname { get; set; } // NVARCHAR(255)
        public string Phone { get; set; } // NVARCHAR(80)
        public string? Image { get; set; } // NVARCHAR(255), puede ser null
        public string Password { get; set; } // NVARCHAR(255)
        public bool? IsAvailable { get; set; } // BIT, puede ser null
        public string? SessionToken { get; set; } // NVARCHAR(255), puede ser null
        public DateTime CreatedAt { get; set; } // DATETIME
        public DateTime UpdatedAt { get; set; } // DATETIME
    }
}
