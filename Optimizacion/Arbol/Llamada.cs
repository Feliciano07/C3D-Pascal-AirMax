using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Optimizacion.Arbol
{
    public class Llamada : Nodo
    {
        public string id;

        public Llamada(int fila, string id) : base(fila)
        {
            this.fila = fila;
            this.id = id;
        }

        public override string getOriginal()
        {
            return this.id + "();";
        }

        public override void Mirilla(Interprete interprete)
        {
            interprete.IP++;
        }
    }
}
