using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Optimizacion.Arbol
{
    public class Asignacion2 : Nodo
    {
        private string direccion;
        private string posicion;
        private string valor;

        public Asignacion2(int fila, string direccion, string posicion, string valor) : base(fila)
        {
            this.direccion = direccion;
            this.posicion = posicion;
            this.valor = valor;
        }


        public override string getOriginal()
        {
            throw new NotImplementedException();
        }

        public override void Mirilla(Interprete interprete)
        {
            throw new NotImplementedException();
        }
    }
}
