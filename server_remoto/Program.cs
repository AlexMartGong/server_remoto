using ObjetosRemotos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;

namespace server_remoto
{
    public class Program
    {
        public static List<persona> personas = new List<persona>();
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando aplicacion");

            try
            {
                TcpServerChannel canal = new TcpServerChannel(5000);
                ChannelServices.RegisterChannel(canal, false);
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(PersonaServicio), 
                    "Ipersona", WellKnownObjectMode.SingleCall);
                Console.WriteLine("Mi servidor esta esperado una conexion");
            }
            catch (Exception)
            {

                Console.WriteLine("Error");
            }

            Console.Read();
        }
    }
}
