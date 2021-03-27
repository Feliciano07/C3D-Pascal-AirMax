using C3D_Pascal_AirMax.TipoDatos;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Enviroment
{
    public class SimboloFuncion
    {
        private Objeto.TipoObjeto tipo;
        private string nombre;
        private int size;
        private LinkedList<Object> parametros;

        public SimboloFuncion(Objeto.TipoObjeto tipo, string nombre, int size, LinkedList<Object> parametros)
        {
            this.tipo = tipo;
            this.nombre = nombre;
            this.size = size;
            this.parametros = parametros;
        }

    }
}
