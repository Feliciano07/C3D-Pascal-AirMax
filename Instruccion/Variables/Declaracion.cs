using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.TipoDatos;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Instruccion.Variables
{
    public class Declaracion : Nodo
    {
        private string[] ids;
        private Nodo expresion;
        private Objeto.TipoObjeto tipo;
        private string nombre_tipo;

        public Declaracion(int linea, int columna, string[] ids, Nodo exp, Objeto.TipoObjeto tipo, string nombre) : base(linea, columna)
        {
            this.ids = ids;
            this.expresion = exp;
            this.tipo = tipo;
            this.nombre_tipo = nombre;
        }

        public override Retorno compilar(Entorno entorno)
        {
            Retorno valor = this.expresion.compilar(entorno);
            if(!base.Verificar_Tipo(valor.getTipo(), this.tipo))
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "Tipos de datos diferentes: " + valor.getTipo().ToString() + "," + this.tipo.ToString());
                throw new Exception("Tipos de datos diferentes: " + valor.getTipo().ToString() + "," + this.tipo.ToString());
            }

            //TODO: falta validar si es object o array y ver si coinciden


            foreach(string str in ids)
            {

            }
        }
    }
}
