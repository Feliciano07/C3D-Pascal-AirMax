using C3D_Pascal_AirMax.Enviroment;
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
            NULO = 10,
            TYPES = 11,
            CONST = 12
        }

        private TipoObjeto tipo;
        private string objetoId;
        public SimboloObjeto symObj;

        public Objeto(TipoObjeto tipo, string objetoId)
        {
            this.tipo = tipo;
            this.objetoId = objetoId;
        }
        public TipoObjeto getTipo()
        {
            return this.tipo;
        }

        public string getObjetoId()
        {
            return this.objetoId.ToLower();
        }


    }
}
