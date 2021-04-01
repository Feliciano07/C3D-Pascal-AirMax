using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Instruccion.Funciones;
using C3D_Pascal_AirMax.Instruccion.Structu;
using C3D_Pascal_AirMax.TipoDatos;
using C3D_Pascal_AirMax.Utilidades;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Analisis
{
    public static class TypeObjeto
    {
        public static void Definicion_Objeto(ParseTreeNode entrada)
        {
            string nombre = entrada.ChildNodes[1].Token.Text;
            int linea = entrada.ChildNodes[1].Span.Location.Line;
            int columna = entrada.ChildNodes[1].Span.Location.Column;
            if (entrada.ChildNodes.Count == 7)
            {
                LinkedList<Atributo> atributos = Lista_atributos(entrada.ChildNodes[4]);
                Manejador.Master.getInstancia.addCompilar(new Estructura(linea, columna, nombre, atributos));
            }
            else if (entrada.ChildNodes.Count == 6)
            {
                LinkedList<Atributo> atributos = new LinkedList<Atributo>();
                Manejador.Master.getInstancia.addCompilar(new Estructura(linea, columna, nombre, atributos));
            }
        }

        public static LinkedList<Atributo> Lista_atributos(ParseTreeNode entrada)
        {
            LinkedList<Atributo> atributos = new LinkedList<Atributo>();
            foreach(ParseTreeNode node in entrada.ChildNodes)
            {
                Declaracion_atributo(node.ChildNodes[0], atributos);
            }
            return atributos;
        }

        public static void Declaracion_atributo(ParseTreeNode entrada, LinkedList<Atributo> atributos)
        {
           
            string token = entrada.Term.Name;
            switch (token)
            {
                case "variable":
                    Lista_Atributos_Var(entrada.ChildNodes[1], atributos);
                    break;
                case "constante":
                    //TODO: preguntar que hacer con las constantes
                    break;
            }
  
        }

        public static void Lista_Atributos_Var(ParseTreeNode entrada, LinkedList<Atributo> atributos)
        {
            foreach(ParseTreeNode node in entrada.ChildNodes)
            {
                Parametros_tipo_var(node, atributos);
            }
        }



        public static void Parametros_tipo_var(ParseTreeNode entrada,LinkedList<Atributo> atributos)
        {
            if(entrada.ChildNodes.Count == 4)
            {
                string type = entrada.ChildNodes[0].Term.Name;
                int linea = entrada.ChildNodes[0].Span.Location.Line;
                int columna = entrada.ChildNodes[0].Span.Location.Column;
                switch (type)
                {
                    case "Id":
                        {
                            string id = entrada.ChildNodes[0].Token.Text;
                            Objeto objeto = new Objeto(Variable.getTipo(entrada.ChildNodes[2]), Variable.Nombre_Tipo(entrada.ChildNodes[2]));
                            atributos.AddLast(new Atributo(id, objeto));
                        }
                        break;
                    case "lista_id":
                        {
                            Objeto objeto = new Objeto(Variable.getTipo(entrada.ChildNodes[2]), Variable.Nombre_Tipo(entrada.ChildNodes[2]));
                            Recorrer_lista_id_type(entrada.ChildNodes[0], atributos, objeto);
                        }
                            
                        break;
                }
            }
        }

        public static void Recorrer_lista_id_type(ParseTreeNode entrada, LinkedList<Atributo> atributos, Objeto objeto)
        {
            foreach(ParseTreeNode node in entrada.ChildNodes)
            {
                string id = node.Token.Text;
                atributos.AddLast(new Atributo(id, objeto));
            }
        }


    }
}
