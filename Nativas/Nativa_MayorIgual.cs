using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Nativas
{
    public class Nativa_MayorIgual : Nodo
    {

        public Nativa_MayorIgual(int linea,int columna) : base(linea, columna)
        {

        }

        public override Retorno compilar(Entorno entorno)
        {
            Crear_mayorIgual();
            return null;
        }

        public void Crear_mayorIgual()
        {
            Master.getInstancia.addFuncion("native_mayorIgual_str");
            string posicion_primera_cadena = Master.getInstancia.newTemporalEntero();
            string posicion_segunda_cadena = Master.getInstancia.newTemporalEntero();

            Master.getInstancia.addBinaria(posicion_primera_cadena, Master.getInstancia.stack_p, "1", "+");
            Master.getInstancia.addBinaria(posicion_segunda_cadena, Master.getInstancia.stack_p, "2", "+");

            string primera_cadena_heap = Master.getInstancia.newTemporalEntero();
            string segunda_cadena_heap = Master.getInstancia.newTemporalEntero();

            Master.getInstancia.addGetStack(primera_cadena_heap, posicion_primera_cadena);
            Master.getInstancia.addGetStack(segunda_cadena_heap, posicion_segunda_cadena);

            string contador1 = Master.getInstancia.newTemporalEntero();
            string contador2 = Master.getInstancia.newTemporalEntero();

            string retorno = Master.getInstancia.newLabel();
            Master.getInstancia.addLabel(retorno);

            string contador_cadena1 = Master.getInstancia.newTemporal();
            string contador_cadena2 = Master.getInstancia.newTemporal();

            Master.getInstancia.addGetHeap(contador_cadena1, primera_cadena_heap);
            Master.getInstancia.addGetHeap(contador_cadena2, segunda_cadena_heap);

            string true_if1 = Master.getInstancia.newLabel();
            string false_if1 = Master.getInstancia.newLabel();

            Master.getInstancia.addif(contador_cadena1, "-1", "!=", true_if1);
            Master.getInstancia.addGoto(false_if1);

            Master.getInstancia.addLabel(true_if1);

            string true_if2 = Master.getInstancia.newLabel();
            string false_if2 = Master.getInstancia.newLabel();

            Master.getInstancia.addif(contador_cadena2, "-1", "!=", true_if2);
            Master.getInstancia.addGoto(false_if2);

            Master.getInstancia.addLabel(true_if2);

            string true_if3 = Master.getInstancia.newLabel();
            string false_if3 = Master.getInstancia.newLabel();

            Master.getInstancia.addif(contador_cadena1, contador_cadena2, ">=", true_if3);
            Master.getInstancia.addGoto(false_if3);

            Master.getInstancia.addLabel(true_if3);

            string desicion = Master.getInstancia.newTemporal();

            Master.getInstancia.addUnaria(desicion, "1");
            Master.getInstancia.addBinaria(primera_cadena_heap, primera_cadena_heap, "1", "+");
            Master.getInstancia.addBinaria(segunda_cadena_heap, segunda_cadena_heap, "1", "+");
            Master.getInstancia.addGoto(retorno);

            Master.getInstancia.addLabel(false_if1);

            string true_if4 = Master.getInstancia.newLabel();
            string false_if4 = Master.getInstancia.newLabel();

            Master.getInstancia.addif(contador_cadena2, "-1", "==", true_if4);
            Master.getInstancia.addGoto(false_if4);

            Master.getInstancia.addLabel(true_if4);
            Master.getInstancia.addUnaria(desicion, "1");
            string label_salida = Master.getInstancia.newLabel();

            Master.getInstancia.addGoto(label_salida);

            Master.getInstancia.addLabel(false_if2);
            Master.getInstancia.addUnaria(desicion, "1");
            Master.getInstancia.addGoto(label_salida);


            Master.getInstancia.addLabel(false_if3+ ":\n" + false_if4);
            Master.getInstancia.addUnaria(desicion, "0");


            Master.getInstancia.addLabel(label_salida);

            string setear_return = Master.getInstancia.newTemporalEntero();

            Master.getInstancia.addBinaria(setear_return, Master.getInstancia.stack_p, "0", "+");
            Master.getInstancia.addSetStack(setear_return, desicion);
            Master.getInstancia.Retorno_funcion();
            Master.getInstancia.addFinFuncion();


        }
    }
}
