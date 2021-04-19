using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.TipoDatos;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Instruccion.Control
{
    public class For : Nodo
    {
        private Nodo asignacion;
        private Nodo expresion;
        private LinkedList<Nodo> instrucciones;
        private bool comportamiento;

        public For(int linea, int columna, Nodo asignacion, Nodo exp, LinkedList<Nodo> instru, bool compor):base(linea, columna)
        {
            this.asignacion = asignacion;
            this.expresion = exp;
            this.instrucciones = instru;
            this.comportamiento = compor;
        }

        public override Retorno compilar(Entorno entorno)
        {
            Retorno iterador = this.asignacion.compilar(entorno);
            isInteger(iterador.getTipo());
            Retorno condicion = this.expresion.compilar(entorno);
            isInteger(condicion.getTipo());
            string label_retorno = Master.getInstancia.newLabel();
            string label_conteo = Master.getInstancia.newLabel();
            string label_true = Master.getInstancia.newLabel();
            string label_false = Master.getInstancia.newLabel();
            string temporal_variable = Master.getInstancia.newTemporal();


            //Guardo el retorno que sirve como continue
            //Guardo el label_false que sirve como brea;
            entorno.setContinue(label_conteo);
            entorno.setBreak(label_false);

            if (comportamiento)// descendente
            {
                if(iterador.sym.isReferencia == false)
                {
                    Master.getInstancia.addComentario("Inicial el for");
                    Master.getInstancia.addLabel(label_retorno);
                    Master.getInstancia.addGetStack(temporal_variable, iterador.getValor());
                    Master.getInstancia.addif(temporal_variable, condicion.getValor(), ">=", label_true);
                    Master.getInstancia.addGoto(label_false);
                    Master.getInstancia.addLabel(label_true);
                    Ejecutar(entorno);
                    Master.getInstancia.addLabel(label_conteo);

                    Master.getInstancia.addComentario("Se cambia el valor");
                    Master.getInstancia.addBinaria(temporal_variable, temporal_variable, "1", "-");
                    Master.getInstancia.addSetStack(iterador.getValor(), temporal_variable);
                    Master.getInstancia.addGoto(label_retorno);
                    Master.getInstancia.addLabel(label_false);
                    Master.getInstancia.addComentario("fin del for");
                }
                else
                {

                    Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                        "Contador de for ilegal");
                    Master.getInstancia.addError(error);
                    throw new Exception("Contador de for ilegal");
                }



            }else// ascendente
            {
                if(iterador.sym.isReferencia == false)
                {
                    Master.getInstancia.addComentario("Inicial el for");
                    Master.getInstancia.addLabel(label_retorno);
                    Master.getInstancia.addGetStack(temporal_variable, iterador.getValor());
                    Master.getInstancia.addif(temporal_variable, condicion.getValor(), "<=", label_true);
                    Master.getInstancia.addGoto(label_false);
                    Master.getInstancia.addLabel(label_true);
                    Ejecutar(entorno);
                    Master.getInstancia.addLabel(label_conteo);
                    Master.getInstancia.addComentario("Se cambia el valor");
                    Master.getInstancia.addBinaria(temporal_variable, temporal_variable, "1", "+");
                    Master.getInstancia.addSetStack(iterador.getValor(), temporal_variable);
                    Master.getInstancia.addGoto(label_retorno);
                    Master.getInstancia.addLabel(label_false);
                    Master.getInstancia.addComentario("fin del for");
                }
                else
                {

                    Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                        "Contador de for ilegal");
                    Master.getInstancia.addError(error);
                    throw new Exception("Contador de for ilegal");
                }
                
            }
            return null;
        }

        public void Ejecutar(Entorno entorno)
        {
            foreach (Nodo instruccion in this.instrucciones)
            {
                if (instruccion != null)
                {
                    try
                    {
                        Retorno retorno = instruccion.compilar(entorno);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        public void isInteger(Objeto.TipoObjeto tipo)
        {
            if(tipo != Objeto.TipoObjeto.INTEGER)
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "Los valores dentro del for deben de ser integer");
                Master.getInstancia.addError(error);
                throw new Exception("El valor del for debe ser integer");
            }
        }

        public void Asignar_Valor_Referencia(Simbolo simbolo, string valor)
        {
            string posicion_stack = Master.getInstancia.newTemporalEntero();
            string posicion = Master.getInstancia.newTemporalEntero();

            Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, (simbolo.posicion + 1).ToString(), "+");

            string tipo = Master.getInstancia.newTemporal();

            Master.getInstancia.addGetStack(tipo, posicion_stack);

            Master.getInstancia.addBinaria(posicion_stack, posicion_stack, "1", "-");
            Master.getInstancia.addGetStack(posicion, posicion_stack);
            string label_true = Master.getInstancia.newLabel();
            string label_false = Master.getInstancia.newLabel();
            string salida = Master.getInstancia.newLabel();

            Master.getInstancia.addif(tipo, "0", "==", label_true);
            Master.getInstancia.addGoto(label_false);
            Master.getInstancia.addLabel(label_true);
            Master.getInstancia.addSetStack(posicion, valor);
            Master.getInstancia.addGoto(salida);
            Master.getInstancia.addLabel(label_false);
            Master.getInstancia.addSetHeap(posicion, valor);
            Master.getInstancia.addLabel(salida);
        }

        public string Obtener_Valor_Referencia(Simbolo sym)
        {
            string posicion_stack = Master.getInstancia.newTemporalEntero();
            string valor = Master.getInstancia.newTemporal();
            string posicion = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, (sym.posicion + 1).ToString(), "+");
            Master.getInstancia.addGetStack(valor, posicion_stack);

            string label_true = Master.getInstancia.newLabel();
            string label_false = Master.getInstancia.newLabel();
            string salida = Master.getInstancia.newLabel();

            Master.getInstancia.addBinaria(posicion_stack, posicion_stack, "1", "-");
            Master.getInstancia.addGetStack(posicion, posicion_stack);

            Master.getInstancia.addif(valor, "0", "==", label_true);
            Master.getInstancia.addGoto(label_false);

            Master.getInstancia.addLabel(label_true);
            Master.getInstancia.addGetStack(valor, posicion);

            Master.getInstancia.addGoto(salida);

            Master.getInstancia.addLabel(label_false);
            Master.getInstancia.addGetHeap(valor, posicion);

            Master.getInstancia.addLabel(salida);

            return valor;
        }
        
    }
}
