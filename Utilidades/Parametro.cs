using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.TipoDatos;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Utilidades
{
    public class Parametro
    {
        public enum Param
        {
            VALOR,
            REFERENCIA
        }

        private string id;
        private Objeto tipo;
        private Param param;

        public Parametro(string id, Objeto tipo, Param param)
        {
            this.id = id;
            this.tipo = tipo;
            this.param = param;
        }



    }
}
