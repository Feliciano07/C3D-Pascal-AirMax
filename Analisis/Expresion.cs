using System;
using System.Collections.Generic;
using System.Text;
using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Expresion;
using C3D_Pascal_AirMax.TipoDatos;
using Irony.Parsing;

namespace C3D_Pascal_AirMax.Analisis
{
    public static class Expresion
    {
        // operacion binaria 
        public static Nodo evaluar(ParseTreeNode entrada)
        {
            if(entrada.ChildNodes.Count == 3)
            {
                String toke = entrada.ChildNodes[1].Term.Name;
                int linea = entrada.ChildNodes[1].Span.Location.Line;
                int columna = entrada.ChildNodes[1].Span.Location.Column;

                switch (toke)
                {
                    case "+":

                        break;
                    case "-":

                        break;
                    case "*":

                        break;
                    case "/":

                        break;
                    case "mod":

                        break;
                    case "or":

                        break;
                    case "and":

                        break;
                    case ">":

                        break;
                    case "<":

                        break;
                    case ">=":

                        break;
                    case "<=":

                        break;
                    case "=":

                        break;
                    case "<>":

                        break;
                    case "div":

                        break;

                }


            }
            else if(entrada.ChildNodes.Count == 1)
            {
                string type = entrada.ChildNodes[0].Term.Name;
                int linea = entrada.ChildNodes[0].Span.Location.Line;
                int columna = entrada.ChildNodes[0].Span.Location.Column;

                switch (type)
                {
                    case "entero":
                        {
                            string valor = entrada.ChildNodes[0].Token.Text;
                            return new Constante(linea, columna, new Primitivo(Objeto.TipoObjeto.INTEGER, valor));
                        }
                    case "cadena":
                        {
                            string valor = entrada.ChildNodes[0].Token.Text;
                            return new Constante(linea, columna, new Primitivo(Objeto.TipoObjeto.STRING, valor));
                        }
                    case "decimal":
                        {
                            string valor = entrada.ChildNodes[0].Token.Text;
                            return new Constante(linea, columna, new Primitivo(Objeto.TipoObjeto.REAL, valor));
                        }
                    case "true":
                        {
                            string valor = entrada.ChildNodes[0].Token.Text;
                            return new Constante(linea, columna, new Primitivo(Objeto.TipoObjeto.BOOLEAN, valor));
                        }
                    case "false":
                        {
                            string valor = entrada.ChildNodes[0].Token.Text;
                            return new Constante(linea, columna, new Primitivo(Objeto.TipoObjeto.BOOLEAN, valor));
                        }

                }

            }
            return null;
        }
    }
}
