using System;
using System.Collections.Generic;
using System.Text;
using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Instruccion.Structu;
using C3D_Pascal_AirMax.TipoDatos;
using C3D_Pascal_AirMax.Utilidades;
using Irony.Parsing;

namespace C3D_Pascal_AirMax.Analisis
{
    public static class TypeArray
    {
        public static void Declaracion_Arreglo(ParseTreeNode entrada)
        {
            foreach(ParseTreeNode node in entrada.ChildNodes)
            {
                Manejador.Master.getInstancia.addCompilar(definicion_arreglo(node));
            }
        }

        public static Nodo definicion_arreglo(ParseTreeNode entrada)
        {
            int linea = entrada.ChildNodes[0].Span.Location.Line;
            int columna = entrada.ChildNodes[0].Span.Location.Column;

            string id = entrada.ChildNodes[0].Token.Text;

            Objeto objeto = new Objeto(Variable.getTipo(entrada.ChildNodes[7]), Variable.Nombre_Tipo(entrada.ChildNodes[7]));

            LinkedList<Dimension> salida = get_dimensiones(entrada.ChildNodes[4]);


            return new EstructuraArreglo(linea, columna, id, salida, objeto);

        }

        public static LinkedList<Dimension> get_dimensiones(ParseTreeNode entrada)
        {
            LinkedList<Dimension> dimensions = new LinkedList<Dimension>();

            foreach(ParseTreeNode node in entrada.ChildNodes)
            {
                int inferior = get_dimension(node.ChildNodes[0]);
                int superior = get_dimension(node.ChildNodes[2]);
                dimensions.AddLast(new Dimension(inferior, superior));
            }
            return dimensions;
        }

        public static int get_dimension(ParseTreeNode entrada)
        {
            string valor = entrada.ChildNodes[0].Token.Text;
            return int.Parse(valor);
        }

    }
}
