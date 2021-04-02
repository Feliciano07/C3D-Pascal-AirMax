using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.TipoDatos;
using C3D_Pascal_AirMax.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Instruccion.Variables
{
    public class Declaracion : Nodo
    {
        private string[] ids;
        private Nodo expresion;
        private Objeto tipo;


        public Declaracion(int linea, int columna, string[] ids, Nodo exp, Objeto tipo) : base(linea, columna)
        {
            this.ids = ids;
            this.expresion = exp;
            this.tipo = tipo;
        }
        //TODO: falta manejar variables locales
        public override Retorno compilar(Entorno entorno)
        {
            if(this.tipo.getTipo() != Objeto.TipoObjeto.TYPES)
            {
                Crear_Variable_Primitiva(entorno);
            }
            else
            {
                Crear_Variable_Objeto(entorno);
            }
            return null;
        }

        public void Crear_Variable_Primitiva(Entorno entorno)
        {
            Retorno valor = this.expresion.compilar(entorno);
            if (!base.Verificar_Tipo(valor.getTipo(), this.tipo.getTipo()))
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "Tipos de datos diferentes: " + valor.getTipo().ToString() + "," + this.tipo.ToString());
                Master.getInstancia.addError(error);
                throw new Exception("Tipos de datos diferentes: " + valor.getTipo().ToString() + "," + this.tipo.ToString());
            }

            foreach (string str in ids)
            {
                Simbolo newVar = entorno.addSimbolo(str, this.tipo.getTipo(), Simbolo.Rol.VARIABLE, Simbolo.Pointer.STACK);
                if (newVar == null)
                {
                    Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                        "La variable: " + str + " ya existe en el ambito");
                    Master.getInstancia.addError(error);
                    throw new Exception("La variable: " + str + " ya existe en el ambito");
                }

                Master.getInstancia.addComentario("variable: " + str);
                string posicion_stack = Master.getInstancia.newTemporalEntero();
                Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, newVar.getPosicion(), "+");

                if (this.tipo.getTipo() == Objeto.TipoObjeto.BOOLEAN)
                {
                    string label_salida = Master.getInstancia.newLabel();
                    Master.getInstancia.addLabel(valor.trueLabel);
                    Master.getInstancia.addSetStack(posicion_stack, "1");
                    Master.getInstancia.addGoto(label_salida);
                    Master.getInstancia.addLabel(valor.falseLabel);
                    Master.getInstancia.addSetStack(posicion_stack, "0");
                    Master.getInstancia.addLabel(label_salida);
                }
                else
                {
                    Master.getInstancia.addSetStack(posicion_stack, valor.getValor());
                }

            }
        }


        public void Crear_Variable_Objeto(Entorno entorno)
        {
            SimboloObjeto sym_obj = entorno.searchObjeto(this.tipo.getObjetoId());
            if(sym_obj == null)
            {
                throw new Exception("El objeto: " + this.tipo.getObjetoId() + " No existe");
            }

            Master.getInstancia.addComentario("Inicio del objeto: " + this.tipo.getObjetoId());
            foreach(string nombre in this.ids)
            {
                string inicio_objeto = Master.getInstancia.newTemporal();
                Master.getInstancia.addUnaria(inicio_objeto, Master.getInstancia.heap_p);
                Simbolo newVar = entorno.addSimbolo(nombre, Objeto.TipoObjeto.OBJECTS, Simbolo.Rol.VARIABLE, Simbolo.Pointer.HEAP);

                if (newVar == null)
                {
                    Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                        "La variable: " + nombre + " ya existe en el ambito");
                    Master.getInstancia.addError(error);
                    throw new Exception("La variable: " + nombre + " ya existe en el ambito");
                }

                foreach (Atributo atr in sym_obj.GetAtributos())
                {
                    switch (atr.getObjeto().getTipo())
                    {
                        case Objeto.TipoObjeto.INTEGER:
                        case Objeto.TipoObjeto.REAL:
                        case Objeto.TipoObjeto.BOOLEAN:
                            Master.getInstancia.addSetHeap(Master.getInstancia.heap_p, "0");
                            break;
                        case Objeto.TipoObjeto.STRING:
                            Master.getInstancia.addSetHeap(Master.getInstancia.heap_p, "-1");
                            break;

                    }
                    Master.getInstancia.nextHeap();
                }

            }
            Master.getInstancia.addComentario("Fin del objeto****");

        }

    }
}
