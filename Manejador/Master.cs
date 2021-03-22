using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Manejador
{
    public sealed class Master
    {
        private static readonly Master instancia = new Master();


        public static Master getInstancia
        {
            get
            {
                return instancia;
            }
        }
    }
}
