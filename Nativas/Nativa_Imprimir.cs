using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Nativas
{
    public class Nativa_Imprimir : Nodo
    {
        public Nativa_Imprimir() : base(0, 0)
        {

        }
        public override Retorno compilar(Entorno entorno)
        {
            Crear_Funcion();
            return null;
        }

        public void Crear_Funcion()
        {
            Master.getInstancia.addFuncion("native_imprimir_str");

            string posicion_stack = Master.getInstancia.newTemporalEntero();
            string posicion_heap = Master.getInstancia.newTemporalEntero();

            Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, "1", "+");

            Master.getInstancia.addGetStack(posicion_heap, posicion_stack);

            string label_retorno = Master.getInstancia.newLabel();

            Master.getInstancia.addLabel(label_retorno);

            string tem_contador = Master.getInstancia.newTemporal();

            Master.getInstancia.addGetHeap(tem_contador, posicion_heap);

            string label_true = Master.getInstancia.newLabel();
            string label_false = Master.getInstancia.newLabel();

            Master.getInstancia.addif(tem_contador, "-1", "!=", label_true);
            Master.getInstancia.addGoto(label_false);

            Master.getInstancia.addLabel(label_true);

            Master.getInstancia.addPrint("c", tem_contador, "char");
            Master.getInstancia.addBinaria(posicion_heap, posicion_heap, "1", "+");
            Master.getInstancia.addGoto(label_retorno);

            Master.getInstancia.addLabel(label_false);

            Master.getInstancia.Retorno_funcion();
            Master.getInstancia.addFinFuncion();

        }

    }
}
