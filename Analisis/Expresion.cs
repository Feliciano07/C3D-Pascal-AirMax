using System;
using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Expresion.Aritmeticas;
using C3D_Pascal_AirMax.TipoDatos;
using C3D_Pascal_AirMax.Expresion.Constantes;
using Irony.Parsing;
using C3D_Pascal_AirMax.Expresion.Relacionales;
using C3D_Pascal_AirMax.Expresion.Logicas;

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
                        return new Modulo(linea, columna, evaluar(entrada.ChildNodes[0]), evaluar(entrada.ChildNodes[2]));
                    case "or":
                        return new Or(linea, columna, evaluar(entrada.ChildNodes[0]), evaluar(entrada.ChildNodes[2]));
                        
                    case "and":
                        return new And(linea, columna, evaluar(entrada.ChildNodes[0]), evaluar(entrada.ChildNodes[2]));
                    case ">":
                        return new MayorQ(linea, columna, evaluar(entrada.ChildNodes[0]), evaluar(entrada.ChildNodes[2]));
                    case "<":
                        return new MenorQ(linea, columna, evaluar(entrada.ChildNodes[0]), evaluar(entrada.ChildNodes[2]));
                    case ">=":
                        return new MayorIgual(linea, columna, evaluar(entrada.ChildNodes[0]), evaluar(entrada.ChildNodes[2]));
                    case "<=":
                        return new MenorIgual(linea, columna, evaluar(entrada.ChildNodes[0]), evaluar(entrada.ChildNodes[2]));
                    case "=":
                        return new Igual(linea, columna, evaluar(entrada.ChildNodes[0]), evaluar(entrada.ChildNodes[2]));
                        
                    case "<>":
                        return new NoIgual(linea, columna, evaluar(entrada.ChildNodes[0]), evaluar(entrada.ChildNodes[2]));
                    case "div":
                        return new Div(linea, columna, evaluar(entrada.ChildNodes[0]), evaluar(entrada.ChildNodes[2]));

                }


            }else if(entrada.ChildNodes.Count == 2)
            {
                String toke = entrada.ChildNodes[0].Term.Name;
                int linea = entrada.ChildNodes[0].Span.Location.Line;
                int columna = entrada.ChildNodes[0].Span.Location.Column;
                switch (toke)
                {
                    case "not":
                        return new Not(linea, columna, evaluar(entrada.ChildNodes[1]));
                    case "-":
                        return new Negativo(linea, columna, evaluar(entrada.ChildNodes[1]));
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
                            return new StringC(linea, columna, Objeto.TipoObjeto.STRING, valor);
                        }
                    case "decimal":
                        {
                            string valor = entrada.ChildNodes[0].Token.Text;
                            return new PrimitivoC(linea, columna, Objeto.TipoObjeto.REAL, valor);
                        }
                    case "true":
                        {
                            string valor = entrada.ChildNodes[0].Token.Text;
                            return new PrimitivoC(linea, columna, Objeto.TipoObjeto.BOOLEAN, true);
                            
                        }
                    case "false":
                        {
                            string valor = entrada.ChildNodes[0].Token.Text;
                            return new PrimitivoC(linea, columna, Objeto.TipoObjeto.BOOLEAN, false);
                            
                        }

                }

            }
            return null;
        }
    }
}
