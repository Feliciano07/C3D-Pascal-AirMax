using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Instruccion.Control
{
    public class Repeat : Nodo
    {

        private Nodo condicion;
        private LinkedList<Nodo> instrucciones;

        public Repeat(int linea, int columna, Nodo condicion, LinkedList<Nodo> instrucciones) : base(linea, columna)
        {
            this.condicion = condicion;
            this.instrucciones = instrucciones;
        }

        public override Retorno compilar(Entorno entorno)
        {
            String label_retorno = Master.getInstancia.newLabel();
            Master.getInstancia.addComentario("inicia el repeat");
            Master.getInstancia.addLabel(label_retorno);
            Ejecutar(entorno);
            Retorno retorno = this.condicion.compilar(entorno);
            if(retorno.getTipo() == TipoDatos.Objeto.TipoObjeto.BOOLEAN)
            {
                //Guardo el retorno que sirve como continue
                //Guardo el label_false que sirve como brea;
                entorno.setContinue(label_retorno);
                entorno.setBreak(retorno.falseLabel);

                Master.getInstancia.addLabel(retorno.falseLabel);
                Master.getInstancia.addGoto(label_retorno);
                Master.getInstancia.addLabel(retorno.trueLabel);
                Master.getInstancia.addComentario("finaliza el repeat");
                return null;
            }
            else
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                  "La sentencia repeat en su condicion solo acepta valores booleanos" +
                  "el tipo de dato actual es: " + retorno.getTipo().ToString());
                Master.getInstancia.addError(error);
                throw new Exception("condicion no booleana en el repeat");
            }
        }

        public void Ejecutar(Entorno entorno)
        {
            foreach (Nodo instruccion in this.instrucciones)
            {
                if (instruccion != null)
                {
                    try
                    {
                        Retorno salida = instruccion.compilar(entorno);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
    }
}
