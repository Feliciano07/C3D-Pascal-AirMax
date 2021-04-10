using System;
using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Expresion.Aritmeticas;
using C3D_Pascal_AirMax.TipoDatos;
using C3D_Pascal_AirMax.Expresion.Constantes;
using Irony.Parsing;
using C3D_Pascal_AirMax.Expresion.Relacionales;
using C3D_Pascal_AirMax.Expresion.Logicas;
using C3D_Pascal_AirMax.Expresion.Accesos;
using System.Collections.Generic;

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
                    default:
                        return evaluar(entrada.ChildNodes[1]);

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
                            return new PrimitivoC(linea, columna, new Objeto(Objeto.TipoObjeto.INTEGER), valor);
                        }
                    case "cadena":
                        {
                            string valor = entrada.ChildNodes[0].Token.Text;
                            return new StringC(linea, columna, new Objeto(Objeto.TipoObjeto.STRING), valor);
                        }
                    case "decimal":
                        {
                            string valor = entrada.ChildNodes[0].Token.Text;
                            return new PrimitivoC(linea, columna, new Objeto(Objeto.TipoObjeto.REAL), valor);
                        }
                    case "true":
                        {
                            string valor = entrada.ChildNodes[0].Token.Text;
                            return new PrimitivoC(linea, columna, new Objeto(Objeto.TipoObjeto.BOOLEAN), true);
                            
                        }
                    case "false":
                        {
                            string valor = entrada.ChildNodes[0].Token.Text;
                            return new PrimitivoC(linea, columna, new Objeto(Objeto.TipoObjeto.BOOLEAN), false);
                            
                        }
                    case "Id":
                        {
                            string valor = entrada.ChildNodes[0].Token.Text;
                            return new AccesoId(linea, columna, valor, null);
                        }
                    case "acceso_objeto":
                        {
                            return Primer_Nivel(entrada.ChildNodes[0]);
                        }
                    case "acceso_array":
                        {
                            return Arreglo_Unico(entrada.ChildNodes[0]);
                        }

                }

            }
            return null;
        }

        public static Nodo Primer_Nivel(ParseTreeNode entrada)
        {
            int linea = entrada.ChildNodes[0].Span.Location.Line;
            int columna = entrada.ChildNodes[0].Span.Location.Column;
            if (entrada.ChildNodes.Count == 3)
            {

                string nombre = entrada.ChildNodes[0].Token.Text;

                AccesoId primero = new AccesoId(linea, columna, nombre, null);

                Nodo retorno = Niveles_abajo(entrada.ChildNodes[2], primero);

                return retorno;
            }
            else
            {
                string nombre = entrada.ChildNodes[0].Token.Text;
                LinkedList<Nodo> dimensiones = Main.lista_expresion(entrada.ChildNodes[2]);
                AccesoArray primero = new AccesoArray(linea, columna, nombre, dimensiones);
                primero.setAnterior(null);

                Nodo retorno = Niveles_abajo(entrada.ChildNodes[5], primero);

                return retorno;
            }
        }

        public static Nodo Niveles_abajo(ParseTreeNode entrada, Nodo primero)
        {
            Nodo auxiliar = null;

            for (int i = 0; i < entrada.ChildNodes.Count; i++)
            {
                

                if (i == 0)
                {
                    Nodo salida = Llamada_id(entrada.ChildNodes[i]);

                    if(salida is AccesoId)
                    {
                        AccesoId acceso = new AccesoId();
                        acceso = (AccesoId)salida;
                        acceso.setAnterior(primero);
                        auxiliar = acceso;
                    }
                    else
                    {
                        AccesoArray acceso = new AccesoArray();
                        acceso = (AccesoArray)salida;
                        acceso.setAnterior(primero);
                        auxiliar = acceso;
                    }

                    
                }
                else
                {
                    Nodo salida = Llamada_id(entrada.ChildNodes[i]);
                    if(salida is AccesoId)
                    {
                        AccesoId acceso = new AccesoId();
                        acceso = (AccesoId)salida;
                        acceso.setAnterior(auxiliar);
                        auxiliar = acceso;
                    }
                    else
                    {
                        AccesoArray acceso = new AccesoArray();
                        acceso = (AccesoArray)salida;
                        acceso.setAnterior(auxiliar);
                        auxiliar = acceso;
                    }
                    
                }
            }
            return auxiliar;
        }


        public static Nodo Llamada_id(ParseTreeNode entrada)
        {
            string toke = entrada.Term.Name;
            
            switch (toke)
            {
                case "Id":
                    {
                        int linea = entrada.Span.Location.Line;
                        int columna = entrada.Span.Location.Column;

                        string id_variable = entrada.Token.Text;

                        return new AccesoId(linea, columna, id_variable, null);
                    }
                case "acceso_array":
                    {
                        return Arreglo_Unico(entrada);
                        
                    }

            }

            return null;
        }

        public static Nodo Arreglo_Unico(ParseTreeNode entrada)
        {
            int linea = entrada.ChildNodes[0].Span.Location.Line;
            int columna = entrada.ChildNodes[0].Span.Location.Column;

            string nombre = entrada.ChildNodes[0].Token.Text;

            LinkedList<Nodo> dimensiones = Main.lista_expresion(entrada.ChildNodes[2]);

            return new AccesoArray(linea, columna, nombre, dimensiones);
        }

    }
}
