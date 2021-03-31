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
                    break;
                case "acceso_array":
                    break;

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
    }
}
