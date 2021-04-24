using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Optimizacion.Arbol
{
    public class Funcion : Nodo
    {

        public string id;
        public LinkedList<Nodo> instrucciones;

        public Funcion(int fila, string id) : base(fila)
        {
            this.id = id;
        }

        public override string getOriginal()
        {

            return "void " + this.id + "(){";
        }

        public override void Mirilla(Interprete interprete)
        {
            throw new NotImplementedException();
        }
    }
}
