using C3D_Pascal_AirMax.TipoDatos;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Utilidades
{
    public class Atributo
    {
        public string id;
        public Objeto objeto;
        public Atributo(string id, Objeto tipo)
        {
            this.id = id.ToLower();
            this.objeto = tipo;
            
        }

        public string getId()
        {
            return this.id;
        }

        public Objeto getObjeto()
        {
            return this.objeto;
        }
    }
}
