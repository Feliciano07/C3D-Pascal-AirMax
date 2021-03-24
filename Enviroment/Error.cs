using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Enviroment
{
    public class Error
    {
        public int linea { get; set; }
        public int columna { get; set; }
        public string tipoError { get; set; }
        public string descripcion { get; set; }
        public string ambito { get; set; }

        public enum Errores
        {
            Lexico,
            Sintactico,
            Semantico
        }

        public Error(int linea, int columna, Errores tipo, string descripcion)
        {
            this.linea = linea;
            this.columna = columna;
            this.tipoError = tipo.ToString();
            this.descripcion = descripcion;
        }

    }
}
