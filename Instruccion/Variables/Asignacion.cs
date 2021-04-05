using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Instruccion.Variables
{
    public class Asignacion : Nodo
    {
        private Nodo variable;
        private Nodo valor;

        public Asignacion(int linea, int columna, Nodo var, Nodo valor):base(linea, columna)
        {
            this.variable = var;
            this.valor = valor;
        }

        public override Retorno compilar(Entorno entorno)
        {
            Retorno asig = this.variable.compilar(entorno);
            Retorno value = this.valor.compilar(entorno);

            if(!base.Verificar_Tipo(value.getTipo(), asig.getTipo()))
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "Tipos de datos diferentes");
                Master.getInstancia.addError(error);
                throw new Exception("Tipos de datos diferentes");
            }

            Simbolo simbolo = asig.sym;

            if (simbolo.getGlobal())
            {
                string posicion_stack = Master.getInstancia.newTemporalEntero();
                

                if (asig.getTipo() == TipoDatos.Objeto.TipoObjeto.BOOLEAN)
                {
                    string aux = Master.getInstancia.newLabel();
                    Master.getInstancia.addLabel(value.trueLabel);
                    Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, simbolo.getPosicion(), "+");
                    Master.getInstancia.addSetStack(posicion_stack, "1");
                    Master.getInstancia.addGoto(aux);
                    Master.getInstancia.addLabel(value.falseLabel);
                    Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, simbolo.getPosicion(), "+");
                    Master.getInstancia.addSetStack(posicion_stack, "0");
                    Master.getInstancia.addLabel(aux);
                    return new Retorno(simbolo.getPosicion(), false, simbolo.getObjeto(), simbolo);
                }else if(asig.getTipo() == TipoDatos.Objeto.TipoObjeto.OBJECTS)
                {
                    //TODO: hacer una copia de los atributos
                }
                else
                {
                    Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, simbolo.getPosicion(), "+");
                    Master.getInstancia.addSetStack(posicion_stack, value.getValor());
                    return new Retorno(simbolo.getPosicion(), false, simbolo.getObjeto(), simbolo);
                }
            }else if(simbolo.pointer == Simbolo.Pointer.HEAP)
            {
                

                if(asig.getTipo() == TipoDatos.Objeto.TipoObjeto.BOOLEAN)
                {
                    string aux = Master.getInstancia.newLabel();
                    Master.getInstancia.addLabel(value.trueLabel);
                   /*
                    * asig.getValor() = retorna la posicion del heap donde esta guardado un atributo
                    */
                    Master.getInstancia.addSetHeap(asig.getValor(), "1");
                    Master.getInstancia.addGoto(aux);
                    Master.getInstancia.addLabel(value.falseLabel);
                    
                    Master.getInstancia.addSetHeap(asig.getValor(), "0");
                    Master.getInstancia.addLabel(aux);
                    return new Retorno(asig.getValor(), false, simbolo.getObjeto(), simbolo);
                }else if(asig.getTipo() == TipoDatos.Objeto.TipoObjeto.OBJECTS)
                {
                    //TODO: hacer una copia de los atributos
                }
                else
                {
                    
                    Master.getInstancia.addSetHeap(asig.getValor(), value.getValor());
                    return new Retorno(asig.getValor(), false, simbolo.getObjeto(), simbolo);
                }
            }

            return null;
        }

        public Retorno Compiar_Objeto()
        {
            return null;
        }
    }
}
