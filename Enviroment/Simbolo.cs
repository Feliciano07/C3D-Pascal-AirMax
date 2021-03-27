using C3D_Pascal_AirMax.TipoDatos;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Enviroment
{
    public class Simbolo
    {

        public enum Rol:int{
            VARIABLE_GLOBAL = 1,
            CONSTANTE =3,
            VARIABLE_LOCAL = 4
        }
        public enum Pointer:int
        {
            STACK = 1,
            HEAP = 2
        }

        private Objeto.TipoObjeto tipo;
        private string nombre;
        private int posicion;
        private Rol rol;
        private Pointer pointer;
        private HashSet<string> ambito;

        // para guardar variables locales
        public Simbolo(string nombre, Objeto.TipoObjeto tipo, Rol rol, Pointer pointer, int posicion)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.rol = rol;
            this.pointer = pointer;
            this.posicion = posicion;
            this.ambito = new HashSet<string>();
        }

        public void addAmbito(string ambito)
        {
            this.ambito.Add(ambito);
        }

        public string getNombre()
        {
            return this.nombre;
        }

        public string getEntorno()
        {
            string salida = "[";
            foreach(string str in this.ambito)
            {
                salida += str + ";";
            }
            salida += "]";
            return salida;
        }

        public Objeto.TipoObjeto getTipo()
        {
            return this.tipo;
        }

        public Simbolo.Rol getRol()
        {
            return this.rol;
        }

        public int getPosicion()
        {
            return this.posicion;
        }
    }
}
