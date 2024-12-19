using ApiAppMesaDeAyuda.Modelos;
using ApiAppMesaDeAyuda.Repositorio.Irepositorio;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ApiAppMesaDeAyuda.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly string connectionMensaAyuda;
        private readonly string claveSecreta;

        public UsuarioRepositorio(IConfiguration configuration)
        {
            connectionMensaAyuda = configuration.GetConnectionString("ConexionmesaAyuda");
            claveSecreta = configuration.GetValue<string>("ApiSettings:Secreta");
        }

        public async Task<RespuestaUsuario> Login(string usuario, string password)
        {
            int idempleado = Convert.ToInt32(usuario);
            var passwordEncriptado = EncriptarNew(password);
            Usuarios usuarioEncontrado = new Usuarios();
            try
            {
                using (var connection = new SqlConnection(connectionMensaAyuda))
                {
                    await connection.OpenAsync();

                    usuarioEncontrado = await connection.QuerySingleAsync<Usuarios>(
                        "SELECT Id,EmpleadoID,Usuario,Nombre,Correo,Estatus,departamento_clave,perfil_clave,Depto_biotime FROM Usuarios WHERE empleadoID = @idempleado AND contrasena = @contrasena",
                        new { idempleado = idempleado, contrasena = passwordEncriptado }
                    );
                }
            }
            catch (Exception ex)
            {
                return new RespuestaUsuario();
            }

            
            if (usuarioEncontrado == null)
            {
                return new RespuestaUsuario()
                {
                    Token = "",
                    usuarios = new Usuarios()
                };

            }

            //Aquí existe el usuario entonces podemos procesar el login      
            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, idempleado.ToString()),
                    
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadorToken.CreateToken(tokenDescriptor);
            RespuestaUsuario usuarioLoginRespuestaDto = new RespuestaUsuario()
            {
                Token = manejadorToken.WriteToken(token),
                usuarios = usuarioEncontrado
            };


            return usuarioLoginRespuestaDto;


        }
        public static string DesEncriptarNew(string cadena)
        {
            string clave = "SIA140922LP9";
            byte[] llave;
            byte[] arreglo = Convert.FromBase64String(cadena); // Arreglo donde guardaremos la cadena descovertida.

            // Ciframos utilizando el Algoritmo MD5.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            llave = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(clave));
            md5.Clear();

            //Ciframos utilizando el Algoritmo 3DES.
            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = llave;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = tripledes.CreateDecryptor();
            byte[] resultado = convertir.TransformFinalBlock(arreglo, 0, arreglo.Length);
            tripledes.Clear();

            string cadena_descifrada = UTF8Encoding.UTF8.GetString(resultado); // Obtenemos la cadena
            return cadena_descifrada; // Devolvemos la cadena
        }


        public static string EncriptarNew(string cadena)
        {
            string clave = "SIA140922LP9";
            byte[] llave; //Arreglo donde guardaremos la llave para el cifrado 3DES.

            byte[] arreglo = UTF8Encoding.UTF8.GetBytes(cadena); //Arreglo donde guardaremos la cadena descifrada.

            // Ciframos utilizando el Algoritmo MD5.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            llave = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(clave));
            md5.Clear();

            //Ciframos utilizando el Algoritmo 3DES.
            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = llave;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = tripledes.CreateEncryptor(); // Iniciamos la conversión de la cadena
            byte[] resultado = convertir.TransformFinalBlock(arreglo, 0, arreglo.Length); //Arreglo de bytes donde guardaremos la cadena cifrada.
            tripledes.Clear();

            return Convert.ToBase64String(resultado, 0, resultado.Length); // Convertimos la cadena y la regresamos.
        }

        public async Task<bool> CreateUser(DatosUsuario dataUser)
        {
            bool isCreated=false;
            try
            {
                var passwordEncriptado = EncriptarNew(dataUser.Password);

                var sql = @" INSERT INTO users (
                                email, name, lastname, phone, image, password, 
                                is_available, session_token, created_at, updated_at
                            ) VALUES (
                                @Email, @Name, @Lastname, @Phone, @Image, @Password, 
                                @IsAvailable, @SessionToken, @CreatedAt, @UpdatedAt
                            )";
                using (var connection = new SqlConnection(connectionMensaAyuda))
                {
                    await connection.OpenAsync();

                    var result = await connection.ExecuteAsync(sql, new
                    {
                        Email = dataUser.Email,
                        Name = dataUser.Name,
                        Lastname = dataUser.Lastname,
                        Phone = dataUser.Phone,
                        Image = dataUser.Image,
                        Password = passwordEncriptado,
                        IsAvailable = dataUser.IsAvailable,
                        SessionToken = dataUser.SessionToken,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });

                    if (result == 1)
                    {
                        isCreated = true;

                    }
                }



            }
            catch (Exception ex) {

                isCreated=false;
            }
            return isCreated;
        }
    }
}
