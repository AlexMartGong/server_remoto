using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjetosRemotos
{
    public interface Ipersona
    {
        persona buscar(string telefono);
        List<persona> traerTodo();
        bool agregar(persona p);
    }
}
