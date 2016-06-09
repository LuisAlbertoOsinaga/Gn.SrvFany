using System;

namespace Gn.SrvFany
{
    class AllegedRC4
    {
        public static string GenerarRC4(string valor, string llave)
        {
            int[] estado = new int[256 + 1];

            string mensaje = String.Empty;
            string llaveAux = String.Empty;
            string mensajeCifrado = String.Empty;

            int X = 0;
            int Y = 0;
            int indice1 = 0;
            int indice2 = 0;

            int nMen = 0;
            int i = 0;
            short op1 = 0;
            int aux = 0;
            int op2 = 0;
            string nroHex = String.Empty;

            X = 0;
            Y = 0;
            indice1 = 0;
            indice2 = 0;
            mensaje = valor;
            llaveAux = llave;

            for (i = 0; i <= 255.0; i += 1)
            {
                estado[i] = i;
            }

            for (i = 0; i <= 255.0; i += 1)
            {
                op1 = (short)(llaveAux.Substring(indice1 + 1 - 1, 1)[0]);
                indice2 = (op1 + estado[i] + indice2) % 256;
                aux = estado[i];
                estado[i] = estado[indice2];
                estado[indice2] = aux;
                indice1 = (indice1 + 1) % llaveAux.Length;
            }

            for (i = 0; i <= mensaje.Length - 1; i += 1)
            {
                X = (X + 1) % 256;
                Y = (estado[X] + Y) % 256;
                aux = estado[X];
                estado[X] = estado[Y];
                estado[Y] = aux;
                op1 = (short)(mensaje.Substring(i + 1 - 1, 1)[0]);
                op2 = estado[(estado[X] + estado[Y]) % 256];
                nMen = op1 ^ op2;
                nroHex = nMen.ToString("X");

                if (nroHex.Length == 1)
                {
                    nroHex = "0" + nroHex;
                }

                mensajeCifrado = mensajeCifrado + nroHex;
            }

            mensajeCifrado = mensajeCifrado.Substring(mensajeCifrado.Length - (mensajeCifrado.Length));

            return mensajeCifrado;
        }
    }
}
