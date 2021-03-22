using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Manejador;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Expresion
{
    public class Constante : Nodo
    {
        Objeto valor;

        public Constante(int linea, int columna, Objeto valor) : base(linea, columna)
        {
            this.valor = valor;
        }

        public override Retorno ejecutar()
        {
            throw new NotImplementedException();
        }
    }
}
