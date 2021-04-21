using C3D_Pascal_AirMax.Optimizacion.Arbol;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Optimizacion
{
    public class Interprete
    {
        public LinkedList<Nodo> instrucciones;
        public string header;
        public int IP;// saber que parte del codigo se va optimizando

        public Interprete(string header, LinkedList<Nodo> instrucciones)
        {
            this.instrucciones = instrucciones;
            this.header = header;
            this.IP = 0;
        }


    }
}
