using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Expresion.Accesos
{
    public class AccesoId : Nodo
    {
        private string id;
        private AccesoId anterior;

        public AccesoId(int linea, int columna, string id, AccesoId anterior) : base(linea, columna)
        {
            this.id = id;
            this.anterior = anterior;
        }

        public override Retorno compilar(Entorno entorno)
        {
            //TODO: falta ver la forma de acceso a un objeto y acceso a locales
            if(this.anterior == null)
            {
                Simbolo sym = entorno.getSimbolo(this.id);
                if(sym == null)
                {
                    Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                        "No existe la variable: " + this.id);
                    Master.getInstancia.addError(error);
                    throw new Exception("No existe la variable: " + this.id);
                }
                string tem = Master.getInstancia.newTemporal();
                if (sym.getGlobal())
                {
                    Master.getInstancia.addGetStack(tem, sym.getPosicion());
                    if(sym.getTipo() != TipoDatos.Objeto.TipoObjeto.BOOLEAN)
                    {
                        return new Retorno(tem, true, sym.getTipo());
                    }
                    Retorno retorno = new Retorno("", false, TipoDatos.Objeto.TipoObjeto.BOOLEAN);
                    this.trueLabel = this.trueLabel == "" ? Master.getInstancia.newLabel() : this.trueLabel;
                    this.falseLabel = this.falseLabel == "" ? Master.getInstancia.newLabel() : this.falseLabel;
                    Master.getInstancia.addif(tem, "1", "==", this.trueLabel);
                    Master.getInstancia.addGoto(this.falseLabel);
                    retorno.trueLabel = this.trueLabel;
                    retorno.falseLabel = this.falseLabel;
                    return retorno;
                }
                else
                {
                    //TODO: variables no locales
                }
            }
            else
            {
                //TODO: acesso a objeto
            }
            return null;
        }
    }
}
