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
        private Objeto.TipoObjeto tipo;

        // constante
        public Retorno(string valor, bool temp, Objeto.TipoObjeto tipo)
        {
            this.valor = valor;
            this.isTemp = temp;
            this.tipo = tipo;
        }

        public string getValor()
        {
            return this.valor;
        }

        public Objeto.TipoObjeto getTipo()
        {
            return this.tipo;
        }

    }
}
