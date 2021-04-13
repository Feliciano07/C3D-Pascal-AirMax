using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.TipoDatos;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Manejador
{
    public class Retorno
    {
        private string valor;
        public bool isTemp;
        private Objeto objeto;
        public string trueLabel;
        public string falseLabel;
        public Simbolo sym;
        public string posicion;
        // constante
        public Retorno(string valor, bool temp, Objeto objeto)
        {
            this.valor = valor;
            this.isTemp = temp;
            this.objeto = objeto;
            this.trueLabel = this.falseLabel = "";
        }
        public Retorno(string valor, bool temp, Objeto objeto, Simbolo simbolo)
        {
            this.valor = valor;
            this.isTemp = temp;
            this.objeto = objeto;
            this.trueLabel = this.falseLabel = "";
            this.sym = simbolo;
        }

        public Retorno(string valor, bool temp, Objeto objeto, Simbolo simbolo, string posicion)
        {
            this.valor = valor;
            this.isTemp = temp;
            this.objeto = objeto;
            this.trueLabel = this.falseLabel = "";
            this.sym = simbolo;
            this.posicion = posicion;
        }

        public string getValor()
        {
            return this.valor;
        }

        public Objeto getObjeto()
        {
            return this.objeto;
        }

        public Objeto.TipoObjeto getTipo()
        {
            return this.objeto.getTipo();
        }

    }
}
