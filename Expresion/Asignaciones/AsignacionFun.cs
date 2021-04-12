using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.TipoDatos;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Expresion.Asignaciones
{
    public class AsignacionFun : Nodo
    {
        private string id;
        private LinkedList<Nodo> parametros;

        public AsignacionFun(int linea, int columna, string id, LinkedList<Nodo> nodos):base(linea, columna)
        {
            this.id = id;
            this.parametros = nodos;
        }

        public override Retorno compilar(Entorno entorno)
        {
            /*
             * Realizamos la busquedad de la funcion
             */
            //TODO: verificar si es funcion o proc
            SimboloFuncion simboloFuncion = entorno.searchFuncion(this.id);
            if(simboloFuncion == null)
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "La funcion: " + this.id + " no se encontro");
                Master.getInstancia.addError(error);
                throw new Exception("La funcion: " + this.id + " no se encontro");
            }

            
            
            //TODO: tengo que mandar a guardar mis temporales, cuando se hace una llamada

            string temp = Master.getInstancia.newTemporalEntero();
            this.getValores(entorno, temp);
            /*
             * Cambio del entorno formal
             */
            Master.getInstancia.plusStack(entorno.size.ToString());
            Master.getInstancia.callFuncion(simboloFuncion.id);
            Master.getInstancia.addBinaria(temp, Master.getInstancia.stack_p, "0", "+");
            string tem2 = Master.getInstancia.newTemporal();// posicion inicio de nueva cadena
            Master.getInstancia.addGetStack(tem2, temp);
            Master.getInstancia.substracStack(entorno.getSize().ToString());


            return this.Devolver_valor_funcion(tem2, simboloFuncion);
        }

        public void getValores(Entorno entorno, string temp)
        {
            LinkedList<Retorno> valores_parametro = new LinkedList<Retorno>();
            if(this.parametros.Count > 0)
            {
                Master.getInstancia.addBinaria(temp, Master.getInstancia.stack_p, entorno.size.ToString(), "+");
                foreach (Nodo instruccion in this.parametros)
                {

                    Retorno retorno = instruccion.compilar(entorno);
                    this.Simulacion_Entorno(entorno, retorno, temp);
                }
            }
            
            
        }

        public void Simulacion_Entorno(Entorno entorno, Retorno aux, string temp)
        {
            if (aux.getObjeto().getTipo() != Objeto.TipoObjeto.BOOLEAN)
            {
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, aux.getValor());
            }
            else
            {
                string salida = Master.getInstancia.newLabel();
                Master.getInstancia.addLabel(aux.trueLabel);
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, "1");
                Master.getInstancia.addGoto(salida);
                Master.getInstancia.addLabel(aux.falseLabel);
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, "0");
                Master.getInstancia.addLabel(salida);
            }
        }

        public Retorno Devolver_valor_funcion(string tem, SimboloFuncion simboloFuncion)
        {
            if(simboloFuncion.objeto.getTipo() == Objeto.TipoObjeto.VOID)
            {
                return null;
            }else if(simboloFuncion.objeto.getTipo() == Objeto.TipoObjeto.BOOLEAN)
            {
                //TODO: retornar un booleano
                return null;
            }
            else
            {
                return new Retorno(tem, true, simboloFuncion.objeto);
            }
        }

    }
}
