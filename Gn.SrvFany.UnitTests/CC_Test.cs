using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Heroina.UnitTest
{
    [TestClass]
    public class CC_Test
    {
        [TestMethod]
        public void Test5000CasosPrueba()
        {
            // Prepara
            string llave;
            string nroAuto;
            string factNro;
            string clienteNit;
            DateTime fechaEmision;
            decimal monto;
            string cc;
            string fileTxt = @"..\..\5000CasosPruebaCCVer7.txt";
            StreamReader reader = new StreamReader(fileTxt);
            ReadHeader(reader);

            // Ejecuta
            int i = 1;
            string linea = reader.ReadLine();
            while (linea != null)
            {
                string ccOut = null;
                try
                {
                    bool datosOk = GetDatosPrueba(linea, out llave, out nroAuto, out factNro, out clienteNit, out fechaEmision, out monto, out cc);
                    Assert.IsTrue(datosOk, string.Format("Línea {0}", i));
                    ccOut = Gn.SrvFany.ServiciosFacturacion.CodigoControl(llave, nroAuto, factNro, clienteNit, fechaEmision, monto);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("excepción en Línea {0}!!!", i), ex);
                }

                // Prueba
                Assert.AreEqual(cc, ccOut, string.Format("Línea {0}", i));
                linea = reader.ReadLine();
                i++;
            }
            reader.Close();
        }

        private bool GetDatosPrueba(string linea, out string llave, out string nroAuto, out string factNro, out string clienteNit, 
                                        out DateTime fechaEmision, out decimal monto, out string cc)
        {
            //Console.WriteLine("Nro Línea: {0}", partes[0]);
            //Console.WriteLine("Nro Autorización: {0}", partes[1]);
            //Console.WriteLine("Nro Factura: {0}", partes[2]);
            //Console.WriteLine("NIT/CI Cliente: {0}", partes[3]);
            //Console.WriteLine("Fecha: {0}", partes[4]);
            //Console.WriteLine("Monto: {0}", partes[5]);
            //Console.WriteLine("Llave: {0}", partes[6]);
            //Console.WriteLine("Base: {0}", partes[7]);
            //Console.WriteLine("Verhoeff: {0}", partes[8]);
            //Console.WriteLine("Cadena: {0}", partes[9]);
            //Console.WriteLine("Sumatoria Producto: {0}", partes[10]);
            //Console.WriteLine("Base 64: {0}", partes[11]);
            //Console.WriteLine("Código de Control: {0}", partes[12]);

            llave = null;
            nroAuto = null;
            factNro = null;
            clienteNit = null;
            fechaEmision = new DateTime();
            monto = -1;
            cc = null;

            string[] partes = linea.Split('\t');
            if(partes.Length != 13)
                return false;

            llave = partes[6];
            nroAuto = partes[1];
            factNro = partes[2];
            clienteNit = partes[3];
            string[] fechaPartes = partes[4].Split('/');
            fechaEmision = new DateTime(int.Parse(fechaPartes[0]), int.Parse(fechaPartes[1]), int.Parse(fechaPartes[2]));
            string[] montoPartes = partes[5].Split('"');
            monto = decimal.Parse(montoPartes.Length > 1 ? montoPartes[1] : montoPartes[0]);
            cc = partes[12];

            return true;
        }

        private void ReadHeader(StreamReader reader)
        {
            reader.ReadLine();
            reader.ReadLine();
            reader.ReadLine();
            reader.ReadLine();
        }

    }
}
