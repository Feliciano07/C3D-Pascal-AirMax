using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.TipoDatos
{
    public class Objeto
    {
        public enum TipoObjeto : int
        {
            STRING = 0,
            INTEGER = 1,
            REAL = 2,
            BOOLEAN = 3,
            VOID = 4,
            ARRAY = 5,
            OBJECTS = 6,
            NULO = 10
        }

        private TipoObjeto tipo;

        public Objeto(TipoObjeto tipo)
        {
            this.tipo = tipo;
        }
        public TipoObjeto getTipo()
        {
            return this.tipo;
        }


    }
}
