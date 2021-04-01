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
            string label_true = Master.getInstancia.newLabel();
            string label_false = Master.getInstancia.newLabel();
            string temporal_variable = Master.getInstancia.newTemporal();

            //TODO: falta ver cuando estan en diferentes entornos y si el valor esta en el heap o stack

            if (comportamiento)// descendente
            {
                Master.getInstancia.addLabel(label_retorno);
                Master.getInstancia.addGetStack(temporal_variable, iterador.sym.getPosicion());
                Master.getInstancia.addif(temporal_variable, condicion.getValor(), ">=", label_true);
                Master.getInstancia.addGoto(label_false);
                Master.getInstancia.addLabel(label_true);
                Ejecutar(entorno);
                Master.getInstancia.addBinaria(temporal_variable, temporal_variable, "1", "-");
                Master.getInstancia.addSetStack(iterador.sym.getPosicion(), temporal_variable);
                Master.getInstancia.addGoto(label_retorno);
                Master.getInstancia.addLabel(label_false);

            }else// ascendente
            {
                Master.getInstancia.addLabel(label_retorno);
                Master.getInstancia.addGetStack(temporal_variable, iterador.sym.getPosicion());
                Master.getInstancia.addif(temporal_variable, condicion.getValor(), "<=", label_true);
                Master.getInstancia.addGoto(label_false);
                Master.getInstancia.addLabel(label_true);
                Ejecutar(entorno);
                Master.getInstancia.addBinaria(temporal_variable, temporal_variable, "1", "+");
                Master.getInstancia.addSetStack(iterador.sym.getPosicion(), temporal_variable);
                Master.getInstancia.addGoto(label_retorno);
                Master.getInstancia.addLabel(label_false);
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
    }
}
