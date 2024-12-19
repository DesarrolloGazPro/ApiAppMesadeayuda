using ApiAppMesaDeAyuda.Modelos;
using ApiAppMesaDeAyuda.Repositorio.Irepositorio;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ApiAppMesaDeAyuda.Repositorio
{
    public class TicketsRepositorio : ITicketsRepositorio
    {
        private readonly string connectionMensaAyuda;
        private readonly string connectionBIOTIME2;
        public TicketsRepositorio(IConfiguration configuration)
        {
            connectionMensaAyuda = configuration.GetConnectionString("ConexionmesaAyuda");
            connectionBIOTIME2 = configuration.GetConnectionString("ConexionBIOTIME2");


        }

        public async Task<bool> updateTicket(SolicitudTicket ticket)
        {

            bool update = false;
            try
            {


                string fechaDetencion = string.Empty;
                string horaDeAtencion = string.Empty;
                string fechahoraStr = string.Empty;
                DateTime fechaHora = DateTime.Now;
                DateTime fechaA = DateTime.Now;
                TimeSpan horas = TimeSpan.Zero;
                double totalHoras = 0;
                string totalHorasDosDigitos = string.Empty;
                DateTime fecha_cerrado = DateTime.Now;
                string horasTiempoRespuesta = string.Empty;
                int indiceEspacio = -1;
                string numeroStr = string.Empty;
                string condicionEstatus = string.Empty;

                try
                {
                    fechaDetencion = Convert.ToDateTime(ticket.fecha).ToShortDateString();
                    horaDeAtencion = ticket.hora;
                    fechahoraStr = fechaDetencion + " " + horaDeAtencion;
                    fechaHora = Convert.ToDateTime(fechahoraStr);
                    fechaA = DateTime.Now;
                    DateTime  fechaCreado = Convert.ToDateTime(ticket.fechacreado);
                    horas = fechaA - fechaCreado;
                    totalHoras = horas.TotalHours;
                    totalHorasDosDigitos = totalHoras.ToString("F2");
                    fecha_cerrado = DateTime.Now;

                    horasTiempoRespuesta = ticket.tiemporespuesta;
                    indiceEspacio = horasTiempoRespuesta.IndexOf(' ');
                    numeroStr = horasTiempoRespuesta.Substring(0, indiceEspacio);
                    condicionEstatus = regresarCondicionTicket(ticket.estatus, Convert.ToDateTime(ticket.fechacreado), fechaA, numeroStr, ticket.id, fechaA);

                }
                catch (Exception ex)
                {
                    // Manejo de errores aquí
                }


                using (var connection = new SqlConnection(connectionMensaAyuda))
                {
                    await connection.OpenAsync();
                    


                    string query = string.Empty;
                    object parameters = null;

                    if (ticket.reasignarticket == "No")
                    {
                        if (ticket.estatus == "cerrado")
                        {
                            if (ticket.solicitudreabrir == "SI")
                            {
                                query = "UPDATE TICKETS SET estatus = @estatus, solicitud_reabrir='NO' WHERE id = @id ";

                                parameters = new
                                {
                                    estatus = ticket.estatus,
                                    id = ticket.id
                                };

                            }
                            else
                            {
                                query = "UPDATE TICKETS SET estatus = @estatus,fecha_atendido = @fecha_atendido, " +
                                        " tiempo_atencion  =@tiempo_atencion, fecha_cerrado = @fecha_cerrado, " +
                                         "Atendio = @Atendio, condicion = @condicion WHERE id = @id ";

                                parameters = new
                                {
                                    estatus = ticket.estatus,
                                    fecha_atendido = fechaHora,
                                    tiempo_atencion = totalHorasDosDigitos,
                                    fecha_cerrado = fecha_cerrado,
                                    Atendio = ticket.atendio,
                                    condicion = condicionEstatus,
                                    id = ticket.id
                                };

                            }
                        }
                        else if (ticket.estatus == "respondido")
                        {
                            if (ticket.fecha == "")
                            {
                                query = "UPDATE TICKETS SET estatus = @estatus WHERE id = @id ";

                                parameters = new
                                {
                                    estatus = ticket.estatus,
                                    id = ticket.id
                                };
                            }
                            else
                            {
                                query = "UPDATE TICKETS SET estatus = @estatus,fecha_atendido = @fecha_atendido, " +
                                       " tiempo_atencion  = @tiempo_atencion, fecha_cerrado = @fecha_cerrado " +
                                        " WHERE id = @id ";
                                parameters = new
                                {
                                    estatus = ticket.estatus,
                                    fecha_atendido = fechaHora,
                                    tiempo_atencion = totalHorasDosDigitos,
                                    fecha_cerrado = fecha_cerrado,
                                    id = ticket.id
                                };
                            }
                        }

                    }
                    else {

                        query = "UPDATE TICKETS SET estatus = 'abierto', servicio = @servicio, " +
                                       " falla  = @falla, prioridad = @prioridad, usuario_asignado = @usuario_asignado, " +
                                       " fecha_creado=getdate() , tiempo_respuesta = @tiempo_respuesta, reabierto='NO', Condicion = 'EN TIEMPO', Nivel = 1" +
                                        " WHERE id = @id ";
                        parameters = new
                        {
                            servicio = ticket.servicio,
                            falla = ticket.falla,
                            prioridad = ticket.prioridad,
                            usuario_asignado = ticket.servicio,
                            tiempo_respuesta = ticket.tiemporespuesta,
                            id = ticket.id
                        };

                    }


                    int actualizado = await connection.ExecuteAsync(query, parameters);

                    if(actualizado == 1)                    
                        update = true;                 

                }

                return update;                
            }
            catch (Exception ex)
            {
                return update;
            }
        }
        public string regresarCondicionTicket(string estatusTicket, DateTime fechaCreado, DateTime fechaActual, string numeroStr, string idTicket, DateTime fechaCerrado)
        {
            int TiempoRespuesta = Int32.Parse(numeroStr);

            TimeSpan diferenciaCerrado = fechaCerrado - fechaCreado;
            double horasDiferenciaCerrado = diferenciaCerrado.TotalHours;

            if (horasDiferenciaCerrado <= TiempoRespuesta)
            {
                return "EN TIEMPO";
            }
            else
            {
                return "VENCIDO";
            }
        }

        public async Task<List<Prioridades>> consultaPrioridades()
        {
            List<Prioridades> prioridad = new List<Prioridades>();
            try
            {
                using (var connection = new SqlConnection(connectionMensaAyuda))
                {
                    await connection.OpenAsync();
                    prioridad = (await connection.QueryAsync<Prioridades>(
                         "select * from CAT_PRIORIDADES"
                        
                        )).ToList();

                }
                return prioridad;
            }
            catch (Exception ex)
            {
                return new List<Prioridades>();
            }
        }


        public async Task<List<Fallas>> consultaFallas(int clasificacionID)
        {
            List<Fallas> fallas = new List<Fallas>();
            try
            {
                using (var connection = new SqlConnection(connectionMensaAyuda))
                {
                    await connection.OpenAsync();
                    fallas = (await connection.QueryAsync<Fallas>(
                         "select '0' as id, 'Selecciona' as falla, '0' as Prioridad ,'0' as Tiempo,'0' as DepartamentoID,'0' as CategoriaID ,'0' as clasificacionID " +
                         " union select id,Falla,Prioridad,Tiempo,DepartamentoID,CategoriaID,clasificacionID from CAT_FALLASYSERV " +
                         " where clasificacionID=@clasificacionID order by id asc",
                         new { clasificacionID = clasificacionID }
                        )).ToList();

                }
                return fallas;
            }
            catch (Exception ex)
            {
                return new List<Fallas>();
            }
        }
        public async Task<string> consultaareAignada(string clave)
        {
            string area = "";
            try
            {
                using (var connection = new SqlConnection(connectionMensaAyuda))
                {
                    await connection.OpenAsync();

                    area = await connection.QuerySingleAsync<string>(
                         " SELECT top 1 (d.clave)   FROM CAT_FALLASYSERV f join CAT_DEPARTAMENTOS d " +
                         " on d.id = f.DepartamentoID join CAT_SERVICIOS s on s.id = f.clasificacionID where s.clave=@clave order by d.clave",
                         new { clave = clave }
                        );

                }
                return area;
            }
            catch (Exception ex)
            {
                return area;
            }
        }

        public async Task<List<Servicios>> consultaServicios()
        {
            List<Servicios> tickets = new List<Servicios>();
            try
            {
                using (var connection = new SqlConnection(connectionMensaAyuda))
                {
                    await connection.OpenAsync();
                    tickets = (await connection.QueryAsync<Servicios>(
                         "select '0' as id, 'Selecciona' as clave union select id, clave from CAT_SERVICIOS"
                        )).ToList();

                }

                return tickets;
            }
            catch (Exception ex)
            {
                return new List<Servicios>();
            }
        }

        public async Task<List<Personal>> consultaPersonal(string departamento)
        {
            List<Personal> tickets = new List<Personal>();
            try
            {
                using (var connection = new SqlConnection(connectionBIOTIME2))
                {
                    await connection.OpenAsync();
                    tickets = (await connection.QueryAsync<Personal>(
                         "select 'Selecciona' as Nombre,'1' as departamento union select distinct First_name + ' ' +" +
                         " last_name as Nombre, d.departamento from personnel_employee emp join departamentos d on d.id_biotime = department_id " +
                         "where enable_att = 1 and departamento=@departamento order by departamento asc",
                         new { departamento = departamento }
                        )).ToList();

                }

                return tickets;
            }
            catch (Exception ex)
            {
                return new List<Personal>();
            }
        }

        public async Task<TicketMensajeArchivo> consultaTicket(string ticket_id)
        {
            var ticketMensajeArchivo = new TicketMensajeArchivo();

            try
            {
                using (var connection = new SqlConnection(connectionMensaAyuda))
                {
                    await connection.OpenAsync();

                    var query = @"
                SELECT * FROM TICKETS WHERE id = @ticket_id;
                SELECT * FROM TICKETS_MENSAJES WHERE ticket_id = @ticket_id;
                SELECT id,archivo_nombre, ticket_id, ticket_mensaje_id FROM TICKETS_ARCHIVOS WHERE ticket_id = @ticket_id;
            ";

                    using (var multi = await connection.QueryMultipleAsync(query, new { ticket_id }))
                    {
                        ticketMensajeArchivo.tickets = (await multi.ReadAsync<Tickets>()).ToList();
                        ticketMensajeArchivo.ticketsMensajes = (await multi.ReadAsync<TicketsMensajes>()).ToList();
                        ticketMensajeArchivo.archivosTickets = (await multi.ReadAsync<ArchivosTickets>()).ToList();
                    }
                }

                return ticketMensajeArchivo;
            }
            catch (Exception)
            {
                return new TicketMensajeArchivo();
            }
        }

        public async Task<List<ArchivosTickets>> consultaArchivo(string idMensaje)
        {
            List<ArchivosTickets> archivosTickets = new List<ArchivosTickets>();

           
            try
            {
                using (var connection = new SqlConnection(connectionMensaAyuda))
                {
                    await connection.OpenAsync();
                    archivosTickets = (await connection.QueryAsync<ArchivosTickets>(
                         "select * from TICKETS_ARCHIVOS where ticket_mensaje_id=@ticket_mensaje_id",
                         new { ticket_mensaje_id = idMensaje }
                        )).ToList();

                }
                return archivosTickets;
            }
            catch (Exception ex)
            {
                return new List<ArchivosTickets>();
            }
        }



        public async Task<List<Tickets>> consultaTickets(string usuario_asignado, string usuarioClavePerfil, string usuarioId)
        {

            string condicionQuery = "";
            string idEstatus = "abierto','reabierto','respondido";


            List<Tickets> tickets = new List<Tickets>();
            try
            {
                

                using (var connection = new SqlConnection(connectionMensaAyuda))
                {
                    await connection.OpenAsync();

                    if (usuarioClavePerfil == "masterAdmin")
                    {
                        //tickets = (await connection.QueryAsync<Tickets>(
                        //                    "select * from TICKETS where id='1609' "
                        //                   )).ToList();

                        tickets = (await connection.QueryAsync<Tickets>(
                                                "select * from TICKETS where estatus in ('abierto','reabierto','respondido') order by fecha_creado desc"
                                               )).ToList();

                    }
                    else if (usuarioClavePerfil == "gerenteArea")
                    {
                        var departamenots = (await connection.QueryAsync<string>(
                                                "select clave from CAT_DEPARTAMENTOS where GerenteDirectorID=@GerenteDirectorID",
                                                new { GerenteDirectorID = usuarioId }
                                               )).ToList();

                        tickets = (await connection.QueryAsync<Tickets>(
                                               "select * from TICKETS where estatus in ('abierto','reabierto','respondido') and usuario_asignado in @usuario_asignado order by fecha_creado desc",
                                               new { usuario_asignado = departamenots }
                                              )).ToList();


                    }
                    else
                    {
                        tickets = (await connection.QueryAsync<Tickets>(
                                                "select * from TICKETS where estatus in ('abierto','reabierto','respondido') and usuario_asignado=@usuario_asignado order by fecha_creado desc",
                                                new { usuario_asignado = usuario_asignado }
                                               )).ToList();
                    }

                   

                }

                return tickets;
            }
            catch (Exception ex)
            {
                return new List<Tickets>();
            }

        }

        public async Task<bool> crearMensaje(MensajeTicket mensaje)
        {           
            
            try
            {
                string sql = @"
                            INSERT INTO TICKETS_MENSAJES (Mensaje, Fecha_Creado, Ticket_Id, EsMensajeSoporte, Usuario, Usuario_Id, Usuario_Nombre)
                            VALUES (@Mensaje, @Fecha_Creado, @Ticket_Id, @EsMensajeSoporte, @Usuario, @Usuario_Id, @Usuario_Nombre);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (var connection = new SqlConnection(connectionMensaAyuda))
                {
                    await connection.OpenAsync();                 
                    int idmensaje = await connection.QuerySingleAsync<int>(sql, new
                    {
                        Mensaje = mensaje.Mensaje,
                        Fecha_Creado = Convert.ToDateTime(mensaje.fecha_creado),
                        Ticket_Id = mensaje.ticket_id,
                        EsMensajeSoporte= mensaje.esMensajeSoporte,
                        Usuario= mensaje.usuario,
                        Usuario_Id = mensaje.usuario_id,
                        Usuario_Nombre = mensaje.usuario_nombre,

                    });

                    if (idmensaje != 0)
                    {
                        if(mensaje.archivo != "" && mensaje.archivo != null)
                        {
                            mensaje.archivo_nombre = idmensaje.ToString() + ".jpg"; 
                            sql = @"
                            INSERT INTO TICKETS_ARCHIVOS (archivo, archivo_nombre, ticket_id, ticket_mensaje_id)
                            VALUES (@archivo, @archivo_nombre, @ticket_id, @ticket_mensaje_id);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                            await connection.QuerySingleAsync<int>(sql, new
                            {
                                archivo = Convert.FromBase64String(mensaje.archivo),
                                archivo_nombre = mensaje.archivo_nombre,
                                Ticket_Id = mensaje.ticket_id,
                                ticket_mensaje_id = idmensaje,                              

                            });
                        }
                        return true;
                    }                    
                    else 
                        return false;
                }               
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        
    }
}
