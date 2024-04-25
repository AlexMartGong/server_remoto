using MySql.Data.MySqlClient;
using ObjetosRemotos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Server
namespace server_remoto
{
    public class PersonaServicio : MarshalByRefObject, Ipersona
    {
        private ConnectionDataBase connection;

        public PersonaServicio()
        {
            connection = new ConnectionDataBase();
        }

        public bool agregar(persona p)
        {
            persona personaExistente = buscar(p.telefono);
            if (personaExistente != null)
            {
                Console.WriteLine("Telefono existe, modificando persona...");
                using (var conn = connection.getConnection())
                {
                    using (var cmd = new MySqlCommand("spModifyPerson", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_phone", p.telefono);
                        cmd.Parameters.AddWithValue("p_name", p.nombre);
                        cmd.Parameters.AddWithValue("p_age", p.edad);

                        try
                        {
                            cmd.ExecuteNonQuery();
                            Console.WriteLine($"Modificando {p.nombre} {p.edad} {p.telefono}");
                            personaExistente.nombre = p.nombre;
                            personaExistente.edad = p.edad;
                            return true;
                        }
                        catch (MySqlException e)
                        {
                            Console.WriteLine("Error al modificar: " + e.Message);
                            return false;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Telefono no existe, agregando persona...");
                using (var conn = connection.getConnection())
                {
                    using (var cmd = new MySqlCommand("spAddPerson", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_phone", p.telefono);
                        cmd.Parameters.AddWithValue("p_name", p.nombre);
                        cmd.Parameters.AddWithValue("p_age", p.edad);

                        try
                        {
                            cmd.ExecuteNonQuery();
                            Console.WriteLine($"Agregando {p.nombre} {p.edad} {p.telefono}");
                            Program.personas.Add(p);
                            return true;
                        }
                        catch (MySqlException e)
                        {
                            Console.WriteLine("Error al agregar: " + e.Message);
                            return false;
                        }
                    }
                }
            }
        }



        public persona buscar(string telefono)
        {
            using (var conn = connection.getConnection())
            {
                using (var cmd = new MySqlCommand("spBuscarPerson", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_phone", telefono);

                    try
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                persona p = new persona()
                                {
                                    telefono = reader["phone"].ToString(),
                                    nombre = reader["name"].ToString(),
                                    edad = Convert.ToInt32(reader["age"])
                                };
                                return p;
                            }
                        }
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine("Error al buscar: " + e.Message);
                    }
                }
            }
            return null;
        }


        public List<persona> traerTodo()
        {
            List<persona> listaPersonas = new List<persona>();

            using (var conn = connection.getConnection())
            {
                using (var cmd = new MySqlCommand("spPeopleMostrar", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            persona p = new persona()
                            {
                                telefono = reader["phone"].ToString(),
                                nombre = reader["name"].ToString(),
                                edad = Convert.ToInt32(reader["age"])
                            };
                            listaPersonas.Add(p);
                        }
                    }
                }
            }

            Console.WriteLine("Consultado la lista");
            return listaPersonas;
        }

    }
}
