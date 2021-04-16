using C3D_Pascal_AirMax.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using C3D_Pascal_AirMax.TipoDatos;
using C3D_Pascal_AirMax.Utilidades;
using C3D_Pascal_AirMax.Instruccion.Funciones;

namespace C3D_Pascal_AirMax.Enviroment
{
    public class Entorno
    {
        public Dictionary<string, Simbolo> variables;
        private Dictionary<string, SimboloObjeto> objetos;
        private Dictionary<string, SimboloArreglo> arreglos;
        private Dictionary<string, SimboloFuncion> funciones;
        private string nombre_entorno;
        public int size;
        private Entorno anterior;

        //Para manejar algo de master
        public SimboloFuncion actual_funcion;
        public string label_return;
        

        //Para manejar los break y continue

        public Stack<string> label_break;
        public Stack<string> label_continue;


        public Entorno( string nombre)
        {
            this.variables = new Dictionary<string, Simbolo>();
            this.objetos = new Dictionary<string, SimboloObjeto>();
            this.arreglos = new Dictionary<string, SimboloArreglo>();
            this.funciones = new Dictionary<string, SimboloFuncion>();

            this.label_break = new Stack<string>();
            this.label_continue = new Stack<string>();

            this.nombre_entorno = nombre;
            this.size = 0;
            this.anterior = null;
        }

        public Entorno(Entorno anterior, string nombre)
        {
            this.variables = new Dictionary<string, Simbolo>();
            this.objetos = new Dictionary<string, SimboloObjeto>();
            this.arreglos = new Dictionary<string, SimboloArreglo>();
            this.funciones = new Dictionary<string, SimboloFuncion>();

            this.label_break = new Stack<string>();
            this.label_continue = new Stack<string>();

            this.nombre_entorno = nombre;
            this.size = 0;
            this.anterior = anterior;
        }

        public void setFuncion(string id,SimboloFuncion simboloFuncion, string label_return )
        {
            this.nombre_entorno = id;
            this.actual_funcion = simboloFuncion;
            this.label_return = label_return;
        }


        public string getNombreEntorno()
        {
            return this.nombre_entorno;
        }

        // Agrega una variable a la tabla de simbolos
        public Simbolo addSimbolo(string id, Objeto tipo, Simbolo.Rol rol, Simbolo.Pointer pointer)
        {
            id = id.ToLower();
            if(this.variables.ContainsKey(id) == true)
            {
                return null;
            }
            this.size++;
            Simbolo simbolo = new Simbolo(id, tipo, rol, pointer, this.size, this.nombre_entorno,
                 this.anterior == null ? true : false);
            simbolo.isReferencia = false;
            this.variables.Add(id, simbolo);
            return simbolo;
        }

        public Simbolo addSimboloFuncion(string id, Objeto tipo, Simbolo.Rol rol, Simbolo.Pointer pointer, Parametro.Tipo_Parametro tipo_Parametro)
        {
            id = id.ToLower();
            if (this.variables.ContainsKey(id) == true)
            {
                return null;
            }
            this.size++;
            Simbolo simbolo = new Simbolo(id, tipo, rol, pointer, this.size, this.nombre_entorno,
                 this.anterior == null ? true : false);
            
            if(tipo_Parametro == Parametro.Tipo_Parametro.VALOR)
            {
                simbolo.isReferencia = false;
            }
            else
            {
                this.size++;
                simbolo.isReferencia = true;
            }
            this.variables.Add(id, simbolo);
            return simbolo;
        }

        // Falta ver como manejar los entornos enlazados
        public Simbolo getSimbolo(string id)
        {
            Entorno aux = this;

            while(aux != null)
            {
                id = id.ToLower();
                if (aux.variables.ContainsKey(id) == true)
                {
                    Simbolo sym;
                    aux.variables.TryGetValue(id, out sym);
                    return sym;
                }
                aux = aux.anterior;
            }
            return null;
        }

        public bool addObjeto(string id, int size, LinkedList<Atributo> atributos)
        {
            id = id.ToLower();
            if (this.objetos.ContainsKey(id))
            {
                return true;
            }
            this.objetos.Add(id, new SimboloObjeto(id, size, atributos));
            return false;
        }

        public SimboloObjeto searchObjeto(string id)
        {
            Entorno aux = this;

            while(aux != null)
            {
                id = id.ToLower();
                if (aux.objetos.ContainsKey(id))
                {
                    SimboloObjeto objeto;
                    aux.objetos.TryGetValue(id, out objeto);
                    return objeto;
                }
                aux = aux.anterior;
            }
            return null;
        }

        public bool addArreglo(string id, LinkedList<Dimension> dimensiones, Objeto objeto)
        {
            id = id.ToLower();
            if (this.arreglos.ContainsKey(id))
            {
                return true;
            }
            this.arreglos.Add(id, new SimboloArreglo(id, dimensiones, objeto));
            return false;
        }

        public SimboloArreglo searchArreglo(string id)
        {
            Entorno aux = this;

            while(aux != null)
            {
                id = id.ToLower();
                if (aux.arreglos.ContainsKey(id))
                {
                    SimboloArreglo arreglo;
                    aux.arreglos.TryGetValue(id, out arreglo);
                    return arreglo;
                }
                aux = aux.anterior;
            }
            return null;
        }

        public bool addFuncion(string id, Objeto objeto, LinkedList<Parametro> parametros )
        {
            id = id.ToLower();
            if (this.funciones.ContainsKey(id)){
                return true;
            }
            this.funciones.Add(id, new SimboloFuncion(id, parametros, objeto));
            return false;
        }

        public SimboloFuncion getFuncion(string id)
        {
            id = id.ToLower();
            SimboloFuncion funcion;
            this.funciones.TryGetValue(id, out funcion);
            return funcion;
        }

        public void setBreak(string label)
        {
            this.label_break.Push(label);
        }
        public string getBreak()
        {
            return this.label_break.Pop();
        }

        public void setContinue(string label)
        {
            this.label_continue.Push(label);
        }

        public string getContinue()
        {
            return this.label_continue.Pop();
        }

        public SimboloFuncion searchFuncion(string id)
        {
            Entorno aux = this;

            while(aux != null)
            {
                id = id.ToLower();
                if (aux.funciones.ContainsKey(id))
                {
                    SimboloFuncion simboloFuncion;
                    aux.funciones.TryGetValue(id, out simboloFuncion);
                    return simboloFuncion;
                }
                aux = aux.anterior;
            }
            return null;
        }

        public int getSize()
        {
            return this.size;
        }

        public string Retornar_Simbolos()
        {
            string salida = "";
            foreach(KeyValuePair<string,Simbolo> kvp in this.variables)
            {
                salida += "<tr>";
                salida += "<td>" + kvp.Value.getId() + "</td>\n";
                salida += "<td>" + kvp.Value.getTipo().ToString() + "</td>\n";
                salida += "<td>" + kvp.Value.getEntorno() + "</td>\n";
                salida += "<td>" + kvp.Value.getRol().ToString()+ "</td>\n";
                salida += "<td>" + kvp.Value.getPosicion() + "</td>\n";
                salida += "</tr>";
            }
            return salida;
        }
        public string Simbolo_Objetos()
        {
            string salida = "";
            foreach(KeyValuePair<string, SimboloObjeto> kvp in this.objetos)
            {
                salida += "<tr>";
                salida += "<td>" + kvp.Value.id + "</td>\n";
                salida += "<td>" + Objeto.TipoObjeto.OBJECTS.ToString() + "</td>\n";
                salida += "<td>" + "Global"+ "</td>\n";
                salida += "<td>" + "Type object" + "</td>\n";
                salida += "<td>" + "-" + "</td>\n";
                salida += "</tr>";
                int contador = 0;
                foreach (Atributo atributo in kvp.Value.GetAtributos())
                {
                    
                    salida += "<tr>";
                    salida += "<td>" + atributo.id + "</td>\n";
                    salida += "<td>" + atributo.getObjeto().getTipo() + "</td>\n";
                    salida += "<td>" + kvp.Key + "</td>\n";
                    salida += "<td>" + "Atributo de "+kvp.Key + "</td>\n";
                    salida += "<td>" + contador + "</td>\n";
                    salida += "</tr>";
                    contador++;
                }
            }

            return salida;
        }


        public void TablaGeneral()
        {
            string ruta = @"C:\compiladores2";
            StreamWriter fichero = new StreamWriter(ruta + "\\" + "Tabla_general" + ".html");
            fichero.WriteLine("<html>");
            fichero.WriteLine("<head><title>Simbolos</title></head>");
            fichero.WriteLine("<body>");
            fichero.WriteLine("<h2>" + "Simbolos" + "</h2>");
            fichero.WriteLine("<br></br>");
            fichero.WriteLine("<center>" +
            "<table border=3 width=60% height=7%>");
            fichero.WriteLine("<tr>");
            fichero.WriteLine("<th>Nombre</th>");
            fichero.WriteLine("<th>Tipo</th>");
            fichero.WriteLine("<th>Ambito</th>");
            fichero.WriteLine("<th>Rol</th>");
            fichero.WriteLine("<th>Apuntador</th>");
            fichero.WriteLine("</tr>");
            fichero.Write(this.Retornar_Simbolos());
            fichero.Write(this.Simbolo_Objetos());
            fichero.Write("</table>");
            fichero.WriteLine("</center>" + "</body>" + "</html>");
            fichero.Close();
        }

    }
}
