using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.TipoDatos;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Instruccion.Variables
{
    public class DeclaracionConstante : Nodo
    {
        private string nombre;
        private Nodo expresion;
        private Objeto.TipoObjeto tipo;

        public DeclaracionConstante(int linea, int columna, string nombre, Nodo exp, Objeto.TipoObjeto tipo) : base(linea, columna)
        {
            this.nombre = nombre;
            this.expresion = exp;
            this.tipo = tipo;
        }

        public override Retorno compilar(Entorno entorno)
        {
            if(base.pre_compilar == true)
            {
                return Crear_simbolo_constante(entorno);
            }
            else
            {
                return Crear_Constante(entorno);
            }
        }

        public Retorno Crear_simbolo_constante(Entorno entorno)
        {
            
            Simbolo newVar = entorno.addSimbolo(this.nombre, new Objeto(this.tipo), Simbolo.Rol.CONSTANTE, Simbolo.Pointer.STACK);
            if (newVar == null)
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "La constate: " + this.nombre + " ya existe en el ambito");
                Master.getInstancia.addError(error);
                throw new Exception("La constate: " + this.nombre + " ya existe en el ambito");
            }
            return null;
        }
    
        
        public Retorno Crear_Constante(Entorno entorno)
        {
            Retorno valor = this.expresion.compilar(entorno);
            /*
             * Se verifica si el valor fue asignado un tipo explicito
             */
            if (this.tipo == Objeto.TipoObjeto.CONST)
            {
                this.tipo = valor.getTipo();
            }

            if (!base.Verificar_Tipo(valor.getObjeto(), new Objeto(this.tipo)))
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "Tipos de datos diferentes: " + valor.getTipo().ToString() + "," + this.tipo.ToString());
                Master.getInstancia.addError(error);
                throw new Exception("Tipos de datos diferentes: " + valor.getTipo().ToString() + "," + this.tipo.ToString());
            }


            Simbolo newVar = entorno.getSimbolo(this.nombre);
            newVar.getObjeto().seTipo(this.tipo);

            if (newVar == null)
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "La constate: " + this.nombre + " no existe en el ambito");
                Master.getInstancia.addError(error);
                throw new Exception("La constate: " + this.nombre + " no existe en el ambito");
            }

            Master.getInstancia.addComentario("Constante: " + this.nombre);
            string posicion_stack = Master.getInstancia.newTemporalEntero();

            if (this.tipo == Objeto.TipoObjeto.BOOLEAN)
            {
                string label_salida = Master.getInstancia.newLabel();
                Master.getInstancia.addLabel(valor.trueLabel);
                Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, newVar.getPosicion(), "+");
                Master.getInstancia.addSetStack(posicion_stack, "1");
                Master.getInstancia.addGoto(label_salida);
                Master.getInstancia.addLabel(valor.falseLabel);
                Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, newVar.getPosicion(), "+");
                Master.getInstancia.addSetStack(posicion_stack, "0");
                Master.getInstancia.addLabel(label_salida);
            }
            else
            {
                Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, newVar.getPosicion(), "+");
                Master.getInstancia.addSetStack(posicion_stack, valor.getValor());
            }

            return null;
        }

    }
}
