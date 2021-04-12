using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.TipoDatos;
using System;
using System.Collections.Generic;
using System.Text; 

namespace C3D_Pascal_AirMax.Utilidades
{
    public class Parametro
    {
        public enum Tipo_Parametro : int
        {
            VALOR =1,
            REFERENCIA =2
        }

        public string id;
        public Objeto objeto;
        public Tipo_Parametro param;


        public Parametro(string id, Objeto tipo, Tipo_Parametro parametro)
        {
            this.id = id.ToLower();
            this.objeto = tipo;
            this.param = parametro;
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
