using C3D_Pascal_AirMax.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using C3D_Pascal_AirMax.TipoDatos;
using C3D_Pascal_AirMax.Utilidades;

namespace C3D_Pascal_AirMax.Enviroment
{
    public class Entorno
    {
        private Dictionary<string, Simbolo> variables;
        private Dictionary<string, SimboloObjeto> objetos;
        private string nombre_entorno;
        private int size;
        private Entorno anterior;

        public Entorno( string nombre)
        {
            this.variables = new Dictionary<string, Simbolo>();
            this.objetos = new Dictionary<string, SimboloObjeto>();
            this.nombre_entorno = nombre;
            this.size = 0;
            this.anterior = null;
        }

        public string getNombreEntorno()
        {
            return this.nombre_entorno;
        }

        // Agrega una variable a la tabla de simbolos
        public Simbolo addSimbolo(string id,Objeto.TipoObjeto tipo, Simbolo.Rol rol, Simbolo.Pointer pointer)
        {
            id = id.ToLower();
            if(this.variables.ContainsKey(id) == true)
            {
                return null;
            }
            Simbolo simbolo = new Simbolo(id, tipo, rol, pointer, this.size++, this.nombre_entorno,
                this.anterior == null ? true:false);
            this.variables.Add(id, simbolo);
            return simbolo;
        }

        // Falta ver como manejar los entornos enlazados
        public Simbolo getSimbolo(string id)
        {
            id = id.ToLower();
            if(this.variables.ContainsKey(id)== true)
            {
                Simbolo sym;
                this.variables.TryGetValue(id, out sym);
                return sym;
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
            fichero.Write("</table>");
            fichero.WriteLine("</center>" + "</body>" + "</html>");
            fichero.Close();
        }

    }
}
