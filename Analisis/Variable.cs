using System;
using System.Collections.Generic;
using System.Text;
using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.TipoDatos;
using C3D_Pascal_AirMax.Expresion.Constantes;
using Irony.Parsing;
using C3D_Pascal_AirMax.Instruccion.Variables;
using C3D_Pascal_AirMax.Manejador;

namespace C3D_Pascal_AirMax.Analisis
{
    public static class Variable
    {
        /*
         * Maneja lo que es var a,b,c:real;
         */

        public static Nodo Lista_variables(ParseTreeNode entrada)
        {
            foreach (ParseTreeNode node in entrada.ChildNodes)
            {
                Master.getInstancia.addCompilar(Evaluar_Variable(node));
            }
            return null;
        }

        /*
         * para empezar a ver lo de las constantes
         */

        public static Nodo Lista_Constante(ParseTreeNode entrada)
        {
            foreach(ParseTreeNode node in entrada.ChildNodes)
            {
                Master.getInstancia.addCompilar(Evaluar_Constante(node));
            }
            return null;
        }

        public static Nodo Evaluar_Variable(ParseTreeNode entrada)
        {
            if(entrada.ChildNodes.Count == 4)// declaracion de variables sin iniciar
            {
                string type = entrada.ChildNodes[0].Term.Name;
                int linea = entrada.ChildNodes[0].Span.Location.Line;
                int columna = entrada.ChildNodes[0].Span.Location.Column;

                switch (type)
                {
                    case "Id":
                        {
                            string[] nombre = new string[] { entrada.ChildNodes[0].Token.Text };
                            Objeto.TipoObjeto tipo = getTipo(entrada.ChildNodes[2]);
                            string nombre_type = Nombre_Tipo(entrada.ChildNodes[2]);
                            return new Declaracion(linea, columna, nombre, getObjeto(tipo), tipo, nombre_type);
                            
                        }
                    case "lista_id":
                        {
                            string[] nombre = getId(entrada.ChildNodes[0]);
                            Objeto.TipoObjeto tipo = getTipo(entrada.ChildNodes[2]);
                            string nombre_type = Nombre_Tipo(entrada.ChildNodes[2]);
                            return new Declaracion(linea, columna, nombre, getObjeto(tipo), tipo, nombre_type);
                        }
                }

            }else if(entrada.ChildNodes.Count == 6)
            {
                string type = entrada.ChildNodes[0].Term.Name;
                int linea = entrada.ChildNodes[0].Span.Location.Line;
                int columna = entrada.ChildNodes[0].Span.Location.Column;
                switch (type)
                {
                    case "Id":
                        {
                            string[] nombre = new string[] { entrada.ChildNodes[0].Token.Text };
                            Objeto.TipoObjeto tipo = getTipo(entrada.ChildNodes[2]);
                            string nombre_type = Nombre_Tipo(entrada.ChildNodes[2]);
                            return new Declaracion(linea, columna, nombre, Expresion.evaluar(entrada.ChildNodes[4]), tipo, nombre_type);
                        }
                }
            }
            return null;
        }

        public static Nodo Evaluar_Constante(ParseTreeNode entrada)
        {
            if(entrada.ChildNodes.Count == 4)
            {
                int linea = entrada.ChildNodes[0].Span.Location.Line;
                int columna = entrada.ChildNodes[0].Span.Location.Column;
                string nombre = entrada.ChildNodes[0].Token.Text;
                return new DeclaracionConstante(linea, columna, nombre, Expresion.evaluar(entrada.ChildNodes[2]), Objeto.TipoObjeto.CONST);
            }else if(entrada.ChildNodes.Count == 6)
            {
                int linea = entrada.ChildNodes[0].Span.Location.Line;
                int columna = entrada.ChildNodes[0].Span.Location.Column;
                string nombre = entrada.ChildNodes[0].Token.Text;
                Objeto.TipoObjeto tipo = getTipo(entrada.ChildNodes[2]);
                return new DeclaracionConstante(linea, columna, nombre, Expresion.evaluar(entrada.ChildNodes[4]), tipo);
            }
            return null;
        }


        /*
         * DEVUELVE EL TIPO DE DATO QUE GUARDAR ESA VARIABLES
         */
        public static Objeto.TipoObjeto getTipo(ParseTreeNode entrada)
        {
            String tipo = entrada.ChildNodes[0].Term.Name;
            switch (tipo)
            {
                case "integer":
                    return Objeto.TipoObjeto.INTEGER;
                case "string":
                    return Objeto.TipoObjeto.STRING;
                case "real":
                    return Objeto.TipoObjeto.REAL;
                case "boolean":
                    return Objeto.TipoObjeto.BOOLEAN;
                default:
                    return Objeto.TipoObjeto.TYPES;
            }
        }
        /*
         * Devuelve el tipo de dato de la variable pero en string
         */
        public static string Nombre_Tipo(ParseTreeNode entrada)
        {
            return entrada.ChildNodes[0].Token.Text;
        }

        /*
         * devuelve un arreglo con los nombres en una declaracion
         * a,b,c,d,e,f
         */

        public static string[] getId(ParseTreeNode entrada)
        {
            string[] ids = new string[entrada.ChildNodes.Count];
            for (int i = 0; i < entrada.ChildNodes.Count; i++)
            {
                ids[i] = entrada.ChildNodes[i].Token.Text;
            }
            return ids;
        }

        /*
         * Retornar los valores iniciales de las variables primitivas que solo fueron declaradas
         */
        public static Nodo getObjeto(Objeto.TipoObjeto tipo)
        {
            switch (tipo)
            {
                case Objeto.TipoObjeto.INTEGER:
                    return new PrimitivoC(0, 0, Objeto.TipoObjeto.INTEGER, Valores_defecto(tipo));
                case Objeto.TipoObjeto.REAL:
                    return new PrimitivoC(0, 0, Objeto.TipoObjeto.REAL, Valores_defecto(tipo));
                case Objeto.TipoObjeto.STRING:
                    return new StringC(0, 0, Objeto.TipoObjeto.STRING, Valores_defecto(tipo));
                case Objeto.TipoObjeto.BOOLEAN:
                    return new PrimitivoC(0, 0, Objeto.TipoObjeto.BOOLEAN, Valores_defecto(tipo));

            }
            return null;
        }

        /*
         * Retornar los valores por defecto que pueden tener las variables
         */
        public static object Valores_defecto(Objeto.TipoObjeto tipo)
        {
            switch (tipo)
            {
                case Objeto.TipoObjeto.INTEGER:
                    return 0;
                case Objeto.TipoObjeto.REAL:
                    return 0.0;
                case Objeto.TipoObjeto.BOOLEAN:
                    return false;
                case Objeto.TipoObjeto.STRING:
                    return "\' \'";

            }
            return null;
        }
    }
}
