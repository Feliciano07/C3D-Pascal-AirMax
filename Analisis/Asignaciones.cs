using C3D_Pascal_AirMax.Abstract;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Text;
using C3D_Pascal_AirMax.Expresion.Asignaciones;
using C3D_Pascal_AirMax.Instruccion.Variables;

namespace C3D_Pascal_AirMax.Analisis
{
    public static class Asignaciones
    {
        public static Nodo Tipo_asignacion(ParseTreeNode entrada)
        {
            string toke = entrada.ChildNodes[0].Term.Name;

            switch (toke)
            {
                case "Id":
                    return Variable_unica(entrada);
                case "acceso_objeto":
                    return Acceso_Objeto(entrada);
                    
                case "acceso_array":
                    return Acceso_arreglo(entrada);
                    
            }
            return null;
        }
        public static Nodo Variable_unica(ParseTreeNode entrada)
        {
            int linea = entrada.ChildNodes[0].Span.Location.Line;
            int columna = entrada.ChildNodes[0].Span.Location.Column;

            string nombre_variable = entrada.ChildNodes[0].Token.Text;

            Nodo expresion = Expresion.evaluar(entrada.ChildNodes[2]);

            AsignacionId asignacionId =  new AsignacionId(linea, columna, nombre_variable, null);

            return new Asignacion(linea, columna, asignacionId, expresion);
        }


        public static Nodo Acceso_Objeto(ParseTreeNode entrada)
        {
            Nodo expresion = Expresion.evaluar(entrada.ChildNodes[2]);
            return Primer_Nivel(entrada.ChildNodes[0], expresion);
        }

        public static Nodo Primer_Nivel(ParseTreeNode entrada, Nodo expresion)
        {
            //TODO: validar si es id o array
            int linea = entrada.ChildNodes[0].Span.Location.Line;
            int columna = entrada.ChildNodes[0].Span.Location.Column;
            if (entrada.ChildNodes.Count == 3)
            {

                string nombre = entrada.ChildNodes[0].Token.Text;

                AsignacionId primero = new AsignacionId(linea, columna, nombre, null);

                AsignacionId retorno = Niveles_abajo(entrada.ChildNodes[2], primero);

                return new Asignacion(linea, columna, retorno, expresion);
            }
            else if (entrada.ChildNodes.Count == 6)
            {
                //TODO: acceso a arreglo de objeto
            }

            return null;
        }

        public static AsignacionId Niveles_abajo(ParseTreeNode entrada, AsignacionId primero)
        {
            AsignacionId auxiliar = null;

            for (int i = 0; i < entrada.ChildNodes.Count; i++)
            {
                //TODO: Validar si es solo id o un arreglo


                if (i == 0)
                {
                    AsignacionId acceso = new AsignacionId();
                    acceso = Llamada_id(entrada.ChildNodes[i]);
                    acceso.setAnterior(primero);
                    auxiliar = acceso;
                }
                else
                {
                    AsignacionId acceso = new AsignacionId();
                    acceso = Llamada_id(entrada.ChildNodes[i]);
                    acceso.setAnterior(auxiliar);
                    auxiliar = acceso;
                }
            }
            return auxiliar;
        }

        public static AsignacionId Llamada_id(ParseTreeNode entrada)
        {
            // prueba solo con id

            string toke = entrada.Term.Name;

            switch (toke)
            {
                case "Id":
                    {
                        int linea = entrada.Span.Location.Line;
                        int columna = entrada.Span.Location.Column;

                        string id_variable = entrada.Token.Text;

                        return new AsignacionId(linea, columna, id_variable, null);
                    }
                case "acceso_array":
                    {
                        //TODO: acceso arreglo
                        break;
                    }

            }

            return null;
        }


        public static Nodo Acceso_arreglo(ParseTreeNode entrada)
        {
            int linea = entrada.ChildNodes[0].Span.Location.Line;
            int columna = entrada.ChildNodes[0].Span.Location.Column;
            Nodo expresion = Expresion.evaluar(entrada.ChildNodes[2]);
            return new Asignacion(linea, columna, Arreglo_Unico(entrada.ChildNodes[0]), expresion);
        }

        public static Nodo Arreglo_Unico(ParseTreeNode entrada)
        {
            int linea = entrada.ChildNodes[0].Span.Location.Line;
            int columna = entrada.ChildNodes[0].Span.Location.Column;

            string nombre = entrada.ChildNodes[0].Token.Text;

            LinkedList<Nodo> dimensiones = Main.lista_expresion(entrada.ChildNodes[2]);

            return new AsignacionArray(linea, columna, nombre, dimensiones);
        }

    }
}
