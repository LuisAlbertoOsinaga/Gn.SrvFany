using System;

namespace Gn.SrvFany
{
    class Verhoeff
    {
        //Tabla de multiplicacion
        private static int[,] d = new int[,]
        {
            {0, 1, 2, 3, 4, 5, 6, 7, 8, 9}, 
            {1, 2, 3, 4, 0, 6, 7, 8, 9, 5}, 
            {2, 3, 4, 0, 1, 7, 8, 9, 5, 6}, 
            {3, 4, 0, 1, 2, 8, 9, 5, 6, 7}, 
            {4, 0, 1, 2, 3, 9, 5, 6, 7, 8}, 
            {5, 9, 8, 7, 6, 0, 4, 3, 2, 1}, 
            {6, 5, 9, 8, 7, 1, 0, 4, 3, 2}, 
            {7, 6, 5, 9, 8, 2, 1, 0, 4, 3}, 
            {8, 7, 6, 5, 9, 3, 2, 1, 0, 4}, 
            {9, 8, 7, 6, 5, 4, 3, 2, 1, 0}
        };

        //Tabla de permutacion
        private static int[,] p = new int[,]
        {
            {0, 1, 2, 3, 4, 5, 6, 7, 8, 9}, 
            {1, 5, 7, 6, 2, 8, 3, 0, 9, 4}, 
            {5, 8, 0, 3, 7, 9, 6, 1, 4, 2}, 
            {8, 9, 1, 6, 0, 4, 3, 5, 2, 7}, 
            {9, 4, 5, 3, 1, 2, 6, 8, 7, 0}, 
            {4, 2, 8, 6, 5, 7, 3, 9, 0, 1}, 
            {2, 7, 9, 3, 8, 0, 6, 4, 1, 5}, 
            {7, 0, 4, 6, 9, 1, 3, 2, 5, 8}
        };

        //Tabla inversa
        private static int[] inv = { 0, 4, 3, 2, 1, 5, 6, 7, 8, 9 };

        /// <summary>
        /// Valida si el numero ingresado es Verhoeff compatible.
        /// Nota: Asegurese que el digito de verificacion sea el ultimo de la cadena.
        /// </summary>
        /// <param name="numero"></param>
        /// <returns>True si es Verhoeff compatible, False en otro caso</returns>
        public static bool ValidarVerhoeff(string numero)
        {
            int c = 0;
            int[] coleccion = CadenaAArregloEnteroInvero(numero);

            for (int i = 0; i < coleccion.Length; i++)
            {
                c = d[c, p[(i % 8), coleccion[i]]];
            }

            return c == 0;
        }

        /// <summary>
        /// Genera el digio de verificacion Verhoeff.
        /// Nota: Anexar este digito de control al numero ingresado.
        /// </summary>
        /// <param name="numero"></param>
        /// <returns>Digito de verificacion Verhoeff como cadena (string)</returns>
        public static string GenerarVerhoeff(string numero)
        {
            int c = 0;
            int[] coleccion = CadenaAArregloEnteroInvero(numero);

            for (int i = 0; i < coleccion.Length; i++)
            {
                c = d[c, p[((i + 1) % 8), coleccion[i]]];
            }

            return inv[c].ToString();
        }

        /// <summary>
        /// Convierte una cadena a un arreglo de enteros inverso.
        /// </summary>
        /// <param name="numero"></param>
        /// <returns>Arreglo de enteros invertido</returns>
        private static int[] CadenaAArregloEnteroInvero(string numero)
        {
            int[] coleccion = new int[numero.Length];

            for (int i = 0; i < numero.Length; i++)
            {
                coleccion[i] = int.Parse(numero.Substring(i, 1));
            }

            Array.Reverse(coleccion);

            return coleccion;
        }
    }
}
