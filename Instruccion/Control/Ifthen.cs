using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Instruccion.Control
{
    public class Ifthen : Nodo
    {
        private Nodo condicion;
        private LinkedList<Nodo> instrucciones;

        public Ifthen(int linea, int columna, Nodo condicion, LinkedList<Nodo> instrucciones) : base(linea, columna)
        {
            this.condicion = condicion;
            this.instrucciones = instrucciones;
        }

        public override Retorno compilar(Entorno entorno)
        {
            Master.getInstancia.addComentario("if then");
            Retorno aux = this.condicion.compilar(entorno);
            if (aux.getTipo() == TipoDatos.Objeto.TipoObjeto.BOOLEAN)
            {
                Master.getInstancia.addLabel(aux.trueLabel);

                foreach(Nodo instruccion in this.instrucciones)
                {
                    if(instruccion != null)
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
                Master.getInstancia.addLabel(aux.falseLabel);
            }
            Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                "La condicion no es booleana en el ifthen: " + aux.getTipo().ToString());
            Master.getInstancia.addError(error);
            throw new Exception("La condicion no es booleana en el ifthen");
        }
    }
}
