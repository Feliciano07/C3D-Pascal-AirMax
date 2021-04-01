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
            //TODO: validar que no se pueda asignar valor a una constate
            Retorno tem = this.variable.compilar(entorno);
            Retorno value = this.valor.compilar(entorno);

            if(!base.Verificar_Tipo(value.getTipo(), tem.getTipo()))
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "Tipos de datos diferentes");
                Master.getInstancia.addError(error);
                throw new Exception("Tipos de datos diferentes");
            }

            Simbolo simbolo = tem.sym;

            if (simbolo.getGlobal())
            {
                string posicion_stack = Master.getInstancia.newTemporalEntero();
                

                if (value.getTipo() == TipoDatos.Objeto.TipoObjeto.BOOLEAN)
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
                    return new Retorno(simbolo.getPosicion(), false, simbolo.getTipo(), simbolo);
                }
                else
                {
                    Master.getInstancia.addSetStack(posicion_stack, value.getValor());
                    return new Retorno(simbolo.getPosicion(), false, simbolo.getTipo(), simbolo);
                }
            }
            else
            {
                // TODO: asignacion a variables no globales
            }

            return null;
        }
    }
}
