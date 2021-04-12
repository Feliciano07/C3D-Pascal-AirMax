using System;
using System.Collections.Generic;
using System.Text;
using Irony.Parsing;
using C3D_Pascal_AirMax.Utilidades;
using C3D_Pascal_AirMax.TipoDatos;
using C3D_Pascal_AirMax.Abstract;

namespace C3D_Pascal_AirMax.Analisis
{
    public static class Declaracion_Proc
    {

        /*
         * Metodo que captura la declaracion de un procedimiento
         */


        public static void Procedimiento(ParseTreeNode entrada)
        {
            string id = entrada.ChildNodes[1].Token.Text;
            int linea = entrada.ChildNodes[1].Span.Location.Line;
            int columna = entrada.ChildNodes[1].Span.Location.Column;

            //TODO: mandar a obtener los parametros (puede manejarse como atributos)
            LinkedList<Parametro> parametros = Obtener_parametros(entrada.ChildNodes[3]);
            
            if(entrada.ChildNodes.Count == 8)
            {
                LinkedList<Nodo> instrucciones = new LinkedList<Nodo>();
                Declaracion_Variables(entrada.ChildNodes[6], instrucciones);
                Instrucciones_Funcion(entrada.ChildNodes[7], instrucciones);

                //TODO: retornar el procedimiento

            }else if(entrada.ChildNodes.Count == 7)
            {
                LinkedList<Nodo> instrucciones = new LinkedList<Nodo>();
                Instrucciones_Funcion(entrada.ChildNodes[6], instrucciones);

                //TODO: retornar el procedimiento
            }

        }


        public static LinkedList<Parametro> Obtener_parametros(ParseTreeNode entrada)
        {
            LinkedList<Parametro> parametros = new LinkedList<Parametro>();
            foreach(ParseTreeNode node in entrada.ChildNodes)
            {
                string term = node.Term.Name;
                switch (term)
                {
                    case "parametros_valor":
                        break;
                    case "parametros_referencia":
                        break;
                }
            }
            return parametros;

        }

        public static void Parametros_Valor(ParseTreeNode entrada, LinkedList<Parametro> parametros)
        {
            int linea = entrada.Span.Location.Line;
            int columna = entrada.Span.Location.Column;

            Objeto tipo = new Objeto(Variable.getTipo(entrada.ChildNodes[2]), Variable.Nombre_Tipo(entrada.ChildNodes[2]));

            getId(entrada.ChildNodes[0], tipo, Parametro.Tipo_Parametro.VALOR, parametros);
        }

        public static void Parametros_Referencia(ParseTreeNode entrada, LinkedList<Parametro> parametros)
        {
            ParseTreeNode referencia = entrada.ChildNodes[1];

            int linea = referencia.Span.Location.Line;
            int columna = referencia.Span.Location.Column;

            Objeto tipo = new Objeto(Variable.getTipo(referencia.ChildNodes[2]), Variable.Nombre_Tipo(referencia.ChildNodes[2]));

            getId(referencia.ChildNodes[0], tipo, Parametro.Tipo_Parametro.REFERENCIA, parametros);
        }

        public static void getId(ParseTreeNode entrada, Objeto objeto, Parametro.Tipo_Parametro tipo_Parametro, LinkedList<Parametro> parametros)
        {
            for(int i = 0; i < entrada.ChildNodes.Count; i++)
            {
                int linea = entrada.ChildNodes[i].Span.Location.Line;
                int columna = entrada.ChildNodes[i].Span.Location.Column;
                string nombre = entrada.ChildNodes[i].Token.Text;

                parametros.AddLast(new Parametro(nombre, objeto, tipo_Parametro));
            }
        }


        public static void Declaracion_Variables(ParseTreeNode entrada, LinkedList<Nodo> instrucciones)
        {
            foreach(ParseTreeNode node in entrada.ChildNodes)
            {
                Declaraciones(node.ChildNodes[0], instrucciones);
            }
        }

        public static void Declaraciones(ParseTreeNode entrada, LinkedList<Nodo> instrucciones)
        {
            string toke = entrada.Term.Name;
            switch (toke)
            {
                case "variable":
                    lista_var(entrada.ChildNodes[1], instrucciones);
                    break;
                case "constante":
                    lista_constante(entrada.ChildNodes[1], instrucciones);
                    break;
            }
        }

        public static void lista_var(ParseTreeNode entrada, LinkedList<Nodo> instrucciones)
        {
            foreach(ParseTreeNode node in entrada.ChildNodes)
            {
                instrucciones.AddLast(Variable.Evaluar_Variable(node));
            }
        }

        public static void lista_constante(ParseTreeNode entrada, LinkedList<Nodo> instrucciones)
        {
            foreach(ParseTreeNode node in entrada.ChildNodes)
            {
                instrucciones.AddLast(Variable.Evaluar_Constante(node));
            }
        }


        public static void Instrucciones_Funcion(ParseTreeNode entrada, LinkedList<Nodo> lista)
        {
            foreach(ParseTreeNode node in entrada.ChildNodes[1].ChildNodes)
            {
                Instrucciones(node.ChildNodes[0], lista);
            }
        }

        public static void Instrucciones(ParseTreeNode actual, LinkedList<Nodo> lista)
        {
            String toke = actual.Term.Name;

            switch (toke)
            {
                case "write":
                    lista.AddLast(Main.Inst_Write(actual, false));
                    break;
                case "writeln":
                    lista.AddLast(Main.Inst_Write(actual, true));
                    break;
            }
        }

    }
}
