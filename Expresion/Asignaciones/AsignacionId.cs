using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Expresion.Asignaciones
{
    public class AsignacionId : Nodo
    {
        private string id;
        private Nodo anterior;

        public AsignacionId(int linea, int columna, string id, Nodo anterior):base(linea, columna)
        {
            this.id = id;
            this.anterior = anterior;
        }
        public  AsignacionId() : base(0, 0)
        {

        }

        public void setAnterior(Nodo anterior)
        {
            this.anterior = anterior;
        }

        public override Retorno compilar(Entorno entorno)
        {
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


                if (sym.getGlobal())
                {
                    return new Retorno(sym.getPosicion(), false, sym.getObjeto(),sym);
                }
            }
            else
            {
                Retorno res_retorno = this.anterior.compilar(entorno);
                
                if(res_retorno.getTipo() == TipoDatos.Objeto.TipoObjeto.OBJECTS)
                {
                    return Obtener_Atributo_Objeto(res_retorno);
                }
            }
            return null;
        }


        public Retorno Obtener_Atributo_Objeto(Retorno res_retorno)
        {
            SimboloObjeto simboloObjeto = res_retorno.getObjeto().symObj;
            Atributo_Index atributo = simboloObjeto.getAtributo(this.id);
            if (atributo == null)
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "El objeto: " + simboloObjeto.id + " no tiene el atributo: " + this.id);
                Master.getInstancia.addError(error);
                throw new Exception("El objeto: " + simboloObjeto.id + " no tiene el atributo: " + this.id);
            }
            string posicion = Master.getInstancia.newTemporalEntero();
            /*
             * verifico si el atributo esta en heap o stack
             * posicion = guarda la posicion donde inicial el objeto
             */
            string valor = Master.getInstancia.newTemporalEntero();
            /*
             * (primitivo) valor = guarda el valor como tal del atributo
             * (types) valor = guarda una posicion en el heap del atributo
             */
            if (res_retorno.sym.pointer == Simbolo.Pointer.HEAP)
            {
                
                Master.getInstancia.addGetHeap(posicion, res_retorno.getValor());
            }
            else
            {
                string contador = Master.getInstancia.newTemporalEntero();
                Master.getInstancia.addBinaria(contador, Master.getInstancia.stack_p, res_retorno.getValor(), "+");
                Master.getInstancia.addGetStack(posicion, contador);
            }

            Master.getInstancia.addBinaria(valor, posicion, atributo.index.ToString(), "+");

            /*
             * retorno el valor y un simbolo que posee la posicion relativa al objeto encontrado
             */
            return new Retorno(valor, true, atributo.atributo.getObjeto(), new Simbolo(this.id, atributo.atributo.getObjeto(),
                Simbolo.Rol.VARIABLE, Simbolo.Pointer.HEAP,atributo.index, "", false));
        }

    }
}
