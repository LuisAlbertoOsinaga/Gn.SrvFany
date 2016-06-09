using System;

namespace Gn.SrvFany
{
    public static class ServiciosFacturacion
    {
        public static string CodigoControl(string llave, 
                                            string nroAuto, 
                                            string factNro, 
                                            string clienteNit, 
                                            DateTime fechaEmision, 
                                            decimal monto)
        {
            if (monto - Math.Floor(monto) == 0.50M)
                monto += 0.01M;
            return GeneradoCodigoControl.GenerarCodigoControl(llave, nroAuto, factNro, clienteNit, fechaEmision.ToString("yyyyMMdd"), 
                                                                (long) Math.Round(monto, 0));
        }

    }
}
