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
                case "caseof":
                    return Main.Instruccion_case_of(actual.ChildNodes[0]);
                case "sentencia_while":
                    return Main.Instruccion_While_If(actual);
                case "opcion_else":
                    return Main.Opcion_else(actual);
                case "sentencia_repeat":
                    return Main.Repeat(actual);
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
        /*
         * MANEJA LA INTRUCCION DE TIPO if then else
         */

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

        public static Nodo Opcion_else(ParseTreeNode entrada)
        {
            int linea = entrada.Span.Location.Column;
            int columna = entrada.Span.Location.Column;
            Nodo exp = Expresion.evaluar(entrada.ChildNodes[1]);
            LinkedList<Nodo> tem_if = new LinkedList<Nodo>();
            // if 
            // if
            if (entrada.ChildNodes[3].ChildNodes.Count == 1)
            {

                tem_if.AddLast(Main_Ifthen(entrada.ChildNodes[3].ChildNodes[0]));

            }
            else if (entrada.ChildNodes[3].ChildNodes.Count == 3)
            {
                tem_if = ListaMain_Ifthen(entrada.ChildNodes[3].ChildNodes[1]);

            }

            LinkedList<Nodo> tem_else = new LinkedList<Nodo>();

            //else
            if (entrada.ChildNodes[5].ChildNodes.Count == 1)
            {

                tem_else.AddLast(Main_Ifthen(entrada.ChildNodes[5].ChildNodes[0]));

            }
            else if (entrada.ChildNodes[5].ChildNodes.Count == 3)
            {
                tem_else = ListaMain_Ifthen(entrada.ChildNodes[5].ChildNodes[1]);

            }
            return new IFelse(linea, columna, exp, tem_if, tem_else);
        }

        /*
         * MANEJA LA INSTRUCCION CASE OF
         */
        public static Nodo Instruccion_case_of(ParseTreeNode entrada)
        {
            int linea = entrada.Span.Location.Line;
            int columna = entrada.Span.Location.Column;
            if(entrada.ChildNodes.Count == 5)
            {
                Nodo exp = Expresion.evaluar(entrada.ChildNodes[1]);
                LinkedList<Case> casos = lista_casos(entrada.ChildNodes[3]);
                return new CaseOf(linea, columna, exp, casos);
                 
            }else if(entrada.ChildNodes.Count == 7)
            {
                Nodo exp = Expresion.evaluar(entrada.ChildNodes[1]);
                LinkedList<Case> casos = lista_casos(entrada.ChildNodes[3]);
                LinkedList<Nodo> temporal = new LinkedList<Nodo>();
                temporal.AddLast(Main_Ifthen(entrada.ChildNodes[5].ChildNodes[0]));

                return new CaseOf(linea, columna, exp, casos, temporal);

            }else if(entrada.ChildNodes.Count == 10)
            {
                Nodo exp = Expresion.evaluar(entrada.ChildNodes[1]);
                LinkedList<Case> casos = lista_casos(entrada.ChildNodes[3]);
                LinkedList<Nodo> tem = ListaMain_Ifthen(entrada.ChildNodes[6]);

                return new CaseOf(linea, columna, exp, casos, tem);
            }
            return null;
        }

        /*
         * MANEJA LOS CASOS QUE PUEDE TENER UNA SENTENCIAS CASE OF
         */
        public static LinkedList<Case> lista_casos(ParseTreeNode entrada)
        {
            LinkedList<Case> lista = new LinkedList<Case>();
            foreach(ParseTreeNode actual in entrada.ChildNodes)
            {
                lista.AddLast(Casos(actual));
            }
            return lista;
        }
        /*
         * RETORNA UN CASO EN ESPECIFICO 
         */

        public static Case Casos(ParseTreeNode entrada)
        {
            int linea = entrada.Span.Location.Line;
            int columna = entrada.Span.Location.Column;

            if(entrada.ChildNodes.Count == 3)
            {
                LinkedList<Nodo> tem;
                tem = lista_expresion(entrada.ChildNodes[0]);
                LinkedList<Nodo> temporal = new LinkedList<Nodo>();
                temporal.AddLast(Main_Ifthen(entrada.ChildNodes[2].ChildNodes[0]));
                return new Case(linea, columna, tem, temporal);
            }else if(entrada.ChildNodes.Count == 6)
            {
                LinkedList<Nodo> tem_exp;
                tem_exp = lista_expresion(entrada.ChildNodes[0]);
                LinkedList<Nodo> tem = ListaMain_Ifthen(entrada.ChildNodes[3]);
                return new Case(linea, columna, tem_exp, tem);
            }

            return null;
        }
        /*
         * MANEJA LA INSTRUCCION WHILE DO, QUE NO ESTA DENTRO DE UN IF
         */
        public static Nodo Instruccion_While(ParseTreeNode entrada)
        {
            int linea = entrada.Span.Location.Line;
            int columna = entrada.Span.Location.Column;
            if(entrada.ChildNodes.Count == 4)
            {
                Nodo condicion = Expresion.evaluar(entrada.ChildNodes[1]);
                LinkedList<Nodo> instrucciones = new LinkedList<Nodo>();
                instrucciones.AddLast(Main_Ifthen(entrada.ChildNodes[3].ChildNodes[0]));
                return new WhileDo(linea, columna, condicion, instrucciones);
            }else if(entrada.ChildNodes.Count == 7)
            {
                Nodo condicion = Expresion.evaluar(entrada.ChildNodes[1]);
                LinkedList<Nodo> instrucciones = ListaMain_Ifthen(entrada.ChildNodes[4]);
                return new WhileDo(linea, columna, condicion, instrucciones);
            }
            return null;
        }
        /*
         * MANEJA LA INSTRUCCION WHILE DO, QUE SI ESTA DENTRO DE UN IF
         */
        public static Nodo Instruccion_While_If(ParseTreeNode entrada)
        {
            int linea = entrada.Span.Location.Line;
            int columna = entrada.Span.Location.Column;
            Nodo condicion = Expresion.evaluar(entrada.ChildNodes[1]);
            if(entrada.ChildNodes[3].ChildNodes.Count == 1)
            {
                LinkedList<Nodo> tem_else = new LinkedList<Nodo>();
                tem_else.AddLast(Main_Ifthen(entrada.ChildNodes[3].ChildNodes[0]));
                return new WhileDo(linea, columna, condicion, tem_else);
            }else if(entrada.ChildNodes[3].ChildNodes.Count == 3)
            {
                LinkedList<Nodo> tem_if = ListaMain_Ifthen(entrada.ChildNodes[3].ChildNodes[1]);
                return new WhileDo(linea, columna, condicion, tem_if);
            }
            return null;
        }
        /*
         * Maneja lo que es la sentecia repeat y que no se encuentra dentro de un if
         */

        public static Nodo Repeat(ParseTreeNode entrada)
        {
            int linea = entrada.Span.Location.Line;
            int columna = entrada.Span.Location.Column;
            Nodo condicion = Expresion.evaluar(entrada.ChildNodes[3]);
            LinkedList<Nodo> tem = Lista_Repeat(entrada.ChildNodes[1]);
            return new Repeat(linea, columna, condicion, tem);
        }

        public static LinkedList<Nodo> Lista_Repeat(ParseTreeNode entrada)
        {
            LinkedList<Nodo> instrucciones = new LinkedList<Nodo>();
            foreach(ParseTreeNode node in entrada.ChildNodes)
            {
                string tipo = node.Term.Name;
                switch (tipo)
                {
                    case "main":
                        instrucciones.AddLast(Instruccion_Repeat(node.ChildNodes[0]));
                        break;
                    case "sentencias_main":
                        foreach(ParseTreeNode node2 in node.ChildNodes[1].ChildNodes)
                        {
                            instrucciones.AddLast(Instruccion_Repeat(node2.ChildNodes[0]));
                        }
                        break;
                }
            }
            return instrucciones;
        }

        public static Nodo Instruccion_Repeat(ParseTreeNode actual)
        {
            string toke = actual.Term.Name;
            switch (toke)
            {
                case "writeln":
                    return Main.Inst_Write(actual, true);
                case "write":
                    return Main.Inst_Write(actual, false);
                case "ifthen":
                    return Main.Instruccion_IfThen(actual);
                case "ifelse":
                    return Main.Instruccion_Ifelse(actual);
                case "caseof":
                    return Main.Instruccion_case_of(actual.ChildNodes[0]);
                case "whiledo":
                    return Main.Instruccion_While(actual);
                case "repeat":
                    return Main.Repeat(actual);
            }
            return null;
        }

    }
}
