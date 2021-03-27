using C3D_Pascal_AirMax.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using Irony.Parsing;
using C3D_Pascal_AirMax.Instruccion.Funciones;

namespace C3D_Pascal_AirMax.Analisis
{
    public static class Main
    {
        public static LinkedList<Nodo> lista_expresion(ParseTreeNode entrada)
        {
            LinkedList<Nodo> parametros = new LinkedList<Nodo>();
            foreach(ParseTreeNode node in entrada.ChildNodes)
            {
                parametros.AddLast(Expresion.evaluar(node));
            }
            return parametros;
        }

        public static Nodo Inst_Write(ParseTreeNode entrada,bool tipo)
        {
            int linea = entrada.Span.Location.Line;
            int columna = entrada.Span.Location.Column;

            if (entrada.ChildNodes.Count == 4)
            {
                LinkedList<Nodo> tem;
                tem = lista_expresion(entrada.ChildNodes[2]);

                return new Write(linea, columna, tem, tipo);

            }
            else if (entrada.ChildNodes.Count == 3)
            {
                LinkedList<Nodo> tem = new LinkedList<Nodo>();
                return new Write(linea, columna, tem, tipo);
            }
            else
            {
                LinkedList<Nodo> tem = new LinkedList<Nodo>();
                return new Write(linea, columna, tem, tipo);
            }
 
        }
    }
}
