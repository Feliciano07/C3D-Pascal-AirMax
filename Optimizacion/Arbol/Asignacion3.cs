using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Optimizacion.Arbol
{
    public class Asignacion3 : Nodo
    {
        private string direccion;
        private string valor;
        private string posicion;

        public Asignacion3(int fila, string direccion, string valor, string posicion) : base(fila)
        {
            this.direccion = direccion;
            this.valor = valor;
            this.posicion = posicion;
        }

        public override string getOriginal()
        {

            return this.direccion + " = " + this.valor + "[" + this.posicion + "];";
        }

        public override void Mirilla(Interprete interprete)
        {
            throw new NotImplementedException();
        }
    }
}
