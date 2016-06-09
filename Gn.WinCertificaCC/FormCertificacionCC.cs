using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Gn.WinCertificaCC
{
    public partial class FormCertificacionCC : Form
    {
        public FormCertificacionCC()
        {
            InitializeComponent();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtLlave.Text = string.Empty;
            txtNroAutorizacion.Text = string.Empty;
            txtNroFactura.Text = string.Empty;
            txtNitCliente.Text = string.Empty;
            dtpFechaEmision.Value = new DateTime(2000, 01, 01);
            txtMonto.Text = string.Empty;
            txtCodigoControl.Text = string.Empty;

            lblMensaje.Text = string.Empty;
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            decimal monto;
            if(!decimal.TryParse(txtMonto.Text, out monto))
            {
                lblMensaje.Text = "Error en monto";
                return;
            }

            try
            {
                txtCodigoControl.Text = Gn.SrvFany.ServiciosFacturacion.CodigoControl(txtLlave.Text,
                                                                    txtNroAutorizacion.Text,
                                                                    txtNroFactura.Text,
                                                                    txtNitCliente.Text,
                                                                    dtpFechaEmision.Value,
                                                                    monto);
                lblMensaje.Text = "OK!";
            }
            catch (Exception ex)
            {
                lblMensaje.Text = string.Format("Error en CodigoControl. {0}-{1}", ex.GetType(), ex.Message);
            }
        }
    }
}
