using System;
using System.Collections.Generic;
using System.Text;

namespace Gn.SrvFany
{
    public class GeneradoCodigoControl
    {
        private static string digitosVerhoeff = string.Empty;
        private static List<string> datosConcatenados = new List<string>(5);
        private static string cifradoRC4 = string.Empty;
        private static List<string> sumatoriasAscii = new List<string>(6);
        private static string enBase64 = string.Empty;
        private static string codigoDeControl = string.Empty;

        private static string llaveDosificacion = string.Empty;
        private static string numeroAutorizacion = string.Empty;

        private static string numeroFactura = string.Empty;
        private static string nitCliente = string.Empty;
        private static string fechaFactura = string.Empty;
        private static string montoFactura = string.Empty;

        public static string GenerarCodigoControl(string llave, string autorizacion,
            string numero, string nit, string fecha, Int64 monto)
        {
            llaveDosificacion = llave;
            numeroAutorizacion = autorizacion;

            numeroFactura = numero;
            nitCliente = nit;
            fechaFactura = fecha;
            montoFactura = monto.ToString();

            GenerarDigitosVerhoeff();
            ConcatenarDatos();
            GenerarCifradoRC4();
            ObtenerSumatoriaASCII();
            ObtenerBase64(); //ojo con este metodo
            ObtenerCodigoControl();

            return codigoDeControl;
        }

        private static void ObtenerCodigoControl()
        {
            codigoDeControl = string.Empty;
            string codigoResultado = AllegedRC4.GenerarRC4(enBase64, llaveDosificacion + digitosVerhoeff);

            for (int i = 0; i < codigoResultado.Length; i+=2)
            {
                if (i == 0)
                    codigoDeControl += codigoResultado.Substring(i, 2);
                else
                    codigoDeControl += "-" + codigoResultado.Substring(i, 2);
            }
        }

        /// <summary>
        /// Obtiene el valor Base64 codificado.
        /// </summary>
        private static void ObtenerBase64()
        {
            Int64 suma = 0;

            #region Sumar 1 a cada digito Verhoeff
            char[] digitos = digitosVerhoeff.ToCharArray();
            int[] digitosNum = new int[5];

            for (int i = 0; i < digitos.Length; i++)
            {
                int digito = Convert.ToInt32(digitos[i].ToString()) + 1;
                digitosNum[i] = digito;
            }
            #endregion

            for (int i = 1; i < sumatoriasAscii.Count; i++)
            {
                Int64 multiplicacion = Convert.ToInt64(sumatoriasAscii[0]) * Convert.ToInt64(sumatoriasAscii[i]);
                Int64 division = multiplicacion / digitosNum[i - 1];
                
                suma += division;
            }

            #region Base64 Encode
            //byte[] bytesEncode = ASCIIEncoding.ASCII.GetBytes(suma.ToString());
            //base64Encode = Convert.ToBase64String(bytesEncode);
            int base64 = 64;
            string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz+/";
            StringBuilder cadena = new StringBuilder();
            do
            {
                int temp = (int)(suma % base64);
                cadena.Insert(0, chars[temp]);
                suma = (suma - temp) / base64;
            } while (suma > 0);

            enBase64 = cadena.ToString();
            #endregion

            #region Base64 Decode
            //byte[] bytesDecode = Convert.FromBase64String(base64Encode);
            //string base64Decode = ASCIIEncoding.ASCII.GetString(bytesDecode);
            #endregion
        }

        /// <summary>
        /// Obtiene el valor de las sumas aplicadas al cifrado RC4
        /// </summary>
        private static void ObtenerSumatoriaASCII()
        {
            byte[] asciiBytes = Encoding.ASCII.GetBytes(cifradoRC4);
            
            int sumaTotalRC4 = 0;
            int sumaParcial1 = 0;
            int sumaParcial2 = 0;
            int sumaParcial3 = 0;
            int sumaParcial4 = 0;
            int sumaParcial5 = 0;

            int j = 0;
            int k = 1;
            int l = 2;
            int m = 3;
            int n = 4;

            for (int i = 0; i < asciiBytes.Length; i++)
            {
                sumaTotalRC4 += (int)asciiBytes[i];

                if (j < asciiBytes.Length && i == j)
                {
                    sumaParcial1 += (int)asciiBytes[j];
                    j += 5;
                }
                else if (k < asciiBytes.Length && i == k)
                {
                    sumaParcial2 += (int)asciiBytes[k];
                    k += 5;
                }
                else if (l < asciiBytes.Length && i == l)
                {
                    sumaParcial3 += (int)asciiBytes[l];
                    l += 5;
                }
                else if (m < asciiBytes.Length && i == m)
                {
                    sumaParcial4 += (int)asciiBytes[m];
                    m += 5;
                }
                else if (n < asciiBytes.Length && i == n)
                {
                    sumaParcial5 += (int)asciiBytes[n];
                    n += 5;
                }
            }

            sumatoriasAscii = new List<string>(6);
            sumatoriasAscii.Add(sumaTotalRC4.ToString());
            sumatoriasAscii.Add(sumaParcial1.ToString());
            sumatoriasAscii.Add(sumaParcial2.ToString());
            sumatoriasAscii.Add(sumaParcial3.ToString());
            sumatoriasAscii.Add(sumaParcial4.ToString());
            sumatoriasAscii.Add(sumaParcial5.ToString());
        }

        /// <summary>
        /// Genera el valor cifrado RC4.
        /// </summary>
        private static void GenerarCifradoRC4()
        {
            string cadenaConcatenada = string.Empty;
            foreach (string dato in datosConcatenados)
            {
                cadenaConcatenada += dato;
            }

            string llaveConcatenada = llaveDosificacion + digitosVerhoeff;

            cifradoRC4 = AllegedRC4.GenerarRC4(cadenaConcatenada, llaveConcatenada);
        }

        /// <summary>
        /// Concatena las subcadenas obtenidas de la llave de dosificacion con los datos de la factura.
        /// </summary>
        private static void ConcatenarDatos()
        {
            #region Sumar 1 a cada digito Verhoeff
            char[] digitos = digitosVerhoeff.ToCharArray();
            int[] digitosNum = new int[5];

            for (int i = 0; i < digitos.Length; i++)
            {
                int digito = Convert.ToInt32(digitos[i].ToString()) + 1;
                digitosNum[i] = digito;
            }
            #endregion

            #region Obtener subcadenas de la llave
            string[] subcadenas = new string[5];

            string auxLlave = llaveDosificacion;
            for (int i = 0; i < 5; i++)
            {
                subcadenas[i] = auxLlave.Substring(0, digitosNum[i]);
                auxLlave = auxLlave.Remove(0, digitosNum[i]);
            }
            #endregion

            #region Concatenar datos
            datosConcatenados = new List<string>(5);
            datosConcatenados.Add(numeroAutorizacion + subcadenas[0]); //Numero de Autorizacion
            datosConcatenados.Add(GenerarDigitoVerhoeff(numeroFactura, 2) + subcadenas[1]); //Numero de Factura
            datosConcatenados.Add(GenerarDigitoVerhoeff(nitCliente, 2) + subcadenas[2]); //NIT del Cliente
            datosConcatenados.Add(GenerarDigitoVerhoeff(fechaFactura, 2) + subcadenas[3]); //Fecha de la Transaccion
            datosConcatenados.Add(GenerarDigitoVerhoeff(montoFactura, 2) + subcadenas[4]); //Monto de la Transaccion
            #endregion
        }

        /// <summary>
        /// Genera los 5 digitos Verhoeff requeridos.
        /// </summary>
        private static void GenerarDigitosVerhoeff()
        {
            #region Sumar datos
            Int64 sumaDatos = Convert.ToInt64(GenerarDigitoVerhoeff(numeroFactura, 2)) +
                              Convert.ToInt64(GenerarDigitoVerhoeff(nitCliente, 2)) +
                              Convert.ToInt64(GenerarDigitoVerhoeff(fechaFactura, 2)) +
                              Convert.ToInt64(GenerarDigitoVerhoeff(montoFactura, 2));
            #endregion

            string sumaVerhoeff = GenerarDigitoVerhoeff(sumaDatos.ToString(), 5);

            digitosVerhoeff = sumaVerhoeff.Substring(sumaVerhoeff.Length - 5);
        }

        private static string GenerarDigitoVerhoeff(string numero, int ciclos)
        {
            string resultado = numero;

            for (int i = 0; i < ciclos; i++)
            {
                resultado += Verhoeff.GenerarVerhoeff(resultado);
            }

            return resultado;
        }
    }
}
