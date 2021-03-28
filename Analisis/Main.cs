using C3D_Pascal_AirMax.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using Irony.Parsing;
using C3D_Pascal_AirMax.Instruccion.Funciones;
using C3D_Pascal_AirMax.Instruccion.Control;

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

        public static Nodo Instruccion_IfThen(ParseTreeNode entrada)
        {
            int linea = entrada.Span.Location.Line;
            int columna = entrada.Span.Location.Column;

            if(entrada.ChildNodes.Count == 4)
            {
                Nodo exp = Expresion.evaluar(entrada.ChildNodes[1]);
                LinkedList<Nodo> temporal = new LinkedList<Nodo>();
                temporal.AddLast(Main_Ifthen(entrada.ChildNodes[3].ChildNodes[0]));

                return new Ifthen(linea, columna, exp, temporal);

            }
            else if(entrada.ChildNodes.Count == 7)
            {
                Nodo exp = Expresion.evaluar(entrada.ChildNodes[1]);
                LinkedList<Nodo> tem = ListaMain_Ifthen(entrada.ChildNodes[4]);
                return new Ifthen(linea, columna, exp, tem);

            }else if(entrada.ChildNodes.Count == 6)
            {
                // pos no hace nada
            }
            return null;
        }
        /*
         *  INSTRUCCIONES QUE VAN DENTRO DEL IFTHE, SOLO ACEPTA
         */

        public static Nodo Main_Ifthen(ParseTreeNode actual)
        {
            string token = actual.Term.Name;

            switch (token)
            {
                case "writeln":
                    return Main.Inst_Write(actual, true);
                case "write":
                    return Main.Inst_Write(actual, false);
                case "ifthen":
                    return Main.Instruccion_IfThen(actual);
                case "ifelse":
                    return Main.Instruccion_Ifelse(actual);
                default:
                    break;
            }
            return null;
        }
        /*
         * MANEJA QUE IFTHEN ACEPTE VARIAS INSTRUCCIONES 
         */
        public static LinkedList<Nodo> ListaMain_Ifthen(ParseTreeNode actual)
        {
            LinkedList<Nodo> salida = new LinkedList<Nodo>();

            foreach(ParseTreeNode node in actual.ChildNodes)
            {
                salida.AddLast(Main_Ifthen(node.ChildNodes[0]));
            }
            return salida;
        }

        public static Nodo Instruccion_Ifelse(ParseTreeNode entrada)
        {
            int linea = entrada.Span.Location.Line;
            int columna = entrada.Span.Location.Column;

            if(entrada.ChildNodes.Count == 6)
            {
                Nodo exp = Expresion.evaluar(entrada.ChildNodes[1]);
                if(entrada.ChildNodes[3].ChildNodes.Count == 1)
                {
                    LinkedList<Nodo> tem_if = new LinkedList<Nodo>(); // instrucciones del if
                    tem_if.AddLast(Main_Ifthen(entrada.ChildNodes[3].ChildNodes[0]));
                    LinkedList<Nodo> tem_else = new LinkedList<Nodo>();
                    tem_else.AddLast(Main_Ifthen(entrada.ChildNodes[5].ChildNodes[0]));

                    return new IFelse(linea, columna, exp, tem_if, tem_else);
                }else if(entrada.ChildNodes[3].ChildNodes.Count == 3)
                {
                    LinkedList<Nodo> tem_if = ListaMain_Ifthen(entrada.ChildNodes[3].ChildNodes[1]);
                    LinkedList<Nodo> tem_else = new LinkedList<Nodo>();
                    tem_else.AddLast(Main_Ifthen(entrada.ChildNodes[5].ChildNodes[0]));

                    return new IFelse(linea, columna, exp, tem_if, tem_else);

                }

            }else if(entrada.ChildNodes.Count == 9)
            {
                Nodo exp = Expresion.evaluar(entrada.ChildNodes[1]);
                if(entrada.ChildNodes[3].ChildNodes.Count == 1)
                {
                    LinkedList<Nodo> tem_if = new LinkedList<Nodo>();
                    tem_if.AddLast(Main_Ifthen(entrada.ChildNodes[3].ChildNodes[0]));
                    LinkedList<Nodo> tem_else = ListaMain_Ifthen(entrada.ChildNodes[6]);

                    return new IFelse(linea, columna, exp, tem_if, tem_else);

                }
                else if(entrada.ChildNodes[3].ChildNodes.Count == 3)
                {
                    LinkedList<Nodo> tem_if = ListaMain_Ifthen(entrada.ChildNodes[3].ChildNodes[1]);
                    LinkedList<Nodo> tem_else = ListaMain_Ifthen(entrada.ChildNodes[6]);
                    return new IFelse(linea, columna, exp, tem_if, tem_else);
                }
            }
            return null;
        }
    }
}
