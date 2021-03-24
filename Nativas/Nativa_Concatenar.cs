using System;
using System.Collections.Generic;
using System.Text;
using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;

namespace C3D_Pascal_AirMax.Nativas
{
    public class Nativa_Concatenar : Nodo
    {
        
        public Nativa_Concatenar(int linea, int columna):base(linea, columna)
        {

        }


        public override Retorno compilar(Entorno entorno)
        {
            Crear_native_concat_str(entorno);
            Concatenar_String();
            return null;
        }

        public void Concatenar_String()
        {

            string posicion_parametro = Master.getInstancia.newTemporal();
            string posicion_cadena = Master.getInstancia.newTemporal();
            string tem = Master.getInstancia.newTemporal();
            // posicion 1
            Master.getInstancia.addFuncion("native_concat_str");
            Master.getInstancia.addBinaria(posicion_parametro, Master.getInstancia.stack_p, "1", "+");
            Master.getInstancia.addGetStack(posicion_cadena, posicion_parametro);

            Master.getInstancia.addUnaria(tem, Master.getInstancia.heap_p);

            string label_retorno = Master.getInstancia.newLabel();
            string label_true = Master.getInstancia.newLabel();
            string label_false = Master.getInstancia.newLabel();

            string temAux = Master.getInstancia.newTemporal();

            Master.getInstancia.addLabel(label_retorno);
            Master.getInstancia.addGetHeap(temAux, posicion_cadena);

            Master.getInstancia.addif(temAux, "-1", "!=", label_true);
            Master.getInstancia.addGoto(label_false);
            Master.getInstancia.addLabel(label_true);

            Master.getInstancia.addSetHeap(Master.getInstancia.heap_p, temAux);
            Master.getInstancia.nextHeap();
            Master.getInstancia.addBinaria(posicion_cadena, posicion_cadena , "1", "+");
            Master.getInstancia.addGoto(label_retorno);

            Master.getInstancia.addLabel(label_false);

            Master.getInstancia.addBinaria(posicion_parametro, Master.getInstancia.stack_p, "2", "+");
            Master.getInstancia.addGetStack(posicion_cadena, posicion_parametro);


            string label_retorno1 = Master.getInstancia.newLabel();
            string label_true1 = Master.getInstancia.newLabel();
            string label_false1 = Master.getInstancia.newLabel();

            Master.getInstancia.addLabel(label_retorno1);
            Master.getInstancia.addGetHeap(temAux, posicion_cadena);

            Master.getInstancia.addif(temAux, "-1", "!=", label_true1);
            Master.getInstancia.addGoto(label_false1);
            Master.getInstancia.addLabel(label_true1);

            Master.getInstancia.addSetHeap(Master.getInstancia.heap_p, temAux);
            Master.getInstancia.nextHeap();
            Master.getInstancia.addBinaria(posicion_cadena, posicion_cadena, "1", "+");
            Master.getInstancia.addGoto(label_retorno1);

            Master.getInstancia.addLabel(label_false1);
            Master.getInstancia.addSetHeap(Master.getInstancia.heap_p, "-1");
            Master.getInstancia.nextHeap();

            string tem_retorno = Master.getInstancia.newTemporal();
            Master.getInstancia.addBinaria(tem_retorno, Master.getInstancia.stack_p, "0", "+");
            Master.getInstancia.addSetStack(tem_retorno, tem);
            Master.getInstancia.Retorno_funcion();
            Master.getInstancia.addFinFuncion();
        }


        public void Crear_native_concat_str(Entorno entorno)
        {
            Simbolo nuevo = new Simbolo("native_concat_str", TipoDatos.Objeto.TipoObjeto.STRING, Simbolo.Rol.FUNCT, Simbolo.Pointer.STACK,
            Master.getInstancia.AumentarStack());
            nuevo.addAmbito(entorno.getNombreEntorno());
            entorno.addSimbolo(nuevo);

            Simbolo str1 = new Simbolo("native_concat_str_1", TipoDatos.Objeto.TipoObjeto.STRING, Simbolo.Rol.VAR, Simbolo.Pointer.STACK,
                Master.getInstancia.AumentarStack());
            str1.addAmbito(entorno.getNombreEntorno());
            str1.addAmbito(nuevo.getNombre());
            entorno.addSimbolo(str1);

            Simbolo str2 = new Simbolo("native_concat_str_2", TipoDatos.Objeto.TipoObjeto.STRING, Simbolo.Rol.VAR, Simbolo.Pointer.STACK,
            Master.getInstancia.AumentarStack());
            str2.addAmbito(entorno.getNombreEntorno());
            str2.addAmbito(nuevo.getNombre());
            entorno.addSimbolo(str2);
        }

    }
}
