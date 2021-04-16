using System;
using System.Collections.Generic;
using System.Text;
using Irony.Parsing;
using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Instruccion.Transferencia;

namespace C3D_Pascal_AirMax.Analisis
{
    public static class Sentencias
    {
        public static Nodo Sentencia_Continue(ParseTreeNode entrada)
        {
            int linea = entrada.Span.Location.Line;
            int columna = entrada.Span.Location.Column;
            return new Continue(linea, columna);
        }

        public static Nodo Sentencia_break(ParseTreeNode entrada)
        {
            int linea = entrada.Span.Location.Line;
            int columna = entrada.Span.Location.Column;
            return new Break(linea, columna);
        }

        public static Nodo Sentencia_Exit(ParseTreeNode entrada)
        {
            int linea = entrada.ChildNodes[0].Span.Location.Line;
            int columna = entrada.ChildNodes[0].Span.Location.Column;

            if(entrada.ChildNodes.Count == 4)
            {
                Nodo expresion = Expresion.evaluar(entrada.ChildNodes[2]);
                return new Exit(linea, columna, expresion);
            }
            else
            {
                return new Exit(linea, columna,null);
            }
        }

    }
}
