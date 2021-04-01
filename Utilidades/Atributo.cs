using C3D_Pascal_AirMax.TipoDatos;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Utilidades
{
    public class Atributo
    {
        private string id;
        private Objeto tipo;
        public Atributo(string id, Objeto tipo)
        {
            this.id = id;
            this.tipo = tipo;
            
        }

        public string getId()
        {
            return this.id;
        }
    }
}
