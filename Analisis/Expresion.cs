﻿using System;
using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Expresion.Aritmeticas;
using C3D_Pascal_AirMax.TipoDatos;
using C3D_Pascal_AirMax.Expresion.Constantes;
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
                        return new Suma(linea, columna, evaluar(entrada.ChildNodes[0]), evaluar(entrada.ChildNodes[2]));
                        
                    case "-":
                        return new Resta(linea, columna, evaluar(entrada.ChildNodes[0]), evaluar(entrada.ChildNodes[2]));
                        
                    case "*":
                        return new Multiplicacion(linea, columna, evaluar(entrada.ChildNodes[0]), evaluar(entrada.ChildNodes[2]));
                        
                    case "/":
                        return new Division(linea, columna, evaluar(entrada.ChildNodes[0]), evaluar(entrada.ChildNodes[2]));
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
                            return new PrimitivoC(linea, columna, Objeto.TipoObjeto.INTEGER, valor);
                        }
                    case "cadena":
                        {
                            string valor = entrada.ChildNodes[0].Token.Text;
                            break;
                        }
                    case "decimal":
                        {
                            string valor = entrada.ChildNodes[0].Token.Text;
                            return new PrimitivoC(linea, columna, Objeto.TipoObjeto.REAL, valor);
                        }
                    case "true":
                        {
                            string valor = entrada.ChildNodes[0].Token.Text;
                            break;
                        }
                    case "false":
                        {
                            string valor = entrada.ChildNodes[0].Token.Text;
                            break;
                        }

                }

            }
            return null;
        }
    }
}
