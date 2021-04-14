using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Instruccion.Control
{
    public class WhileDo : Nodo
    {
        private Nodo condicion;
        private LinkedList<Nodo> instrucciones;

        public WhileDo(int linea, int columna, Nodo cond, LinkedList<Nodo> instru) : base(linea, columna)
        {
            this.condicion = cond;
            this.instrucciones = instru;
        }

        public override Retorno compilar(Entorno entorno)
        {
            string label_retorno = Master.getInstancia.newLabel();
            Master.getInstancia.addLabel(label_retorno);
            Retorno retorno = this.condicion.compilar(entorno);

            if (retorno.getTipo() == TipoDatos.Objeto.TipoObjeto.BOOLEAN)
            {
                //Guardo el retorno que sirve como continue
                //Guardo el label_false que sirve como brea;
                entorno.setContinue(label_retorno);
                entorno.setBreak(retorno.falseLabel);

                Master.getInstancia.addComentario("Inicia el while do");
                
                Master.getInstancia.addLabel(retorno.trueLabel);
                Ejecutar(entorno);
                Master.getInstancia.addGoto(label_retorno);
                Master.getInstancia.addLabel(retorno.falseLabel);
                return null;

            }
            else
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "La sentencia while do en su condicion solo acepta valores booleanos" +
                    "el tipo de dato actual es: " + retorno.getTipo().ToString());
                Master.getInstancia.addError(error);
                throw new Exception("condicion no booleana en el while do");
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
