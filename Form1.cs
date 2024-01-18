using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using LIBERADOR_PEDIDOS.Class;


namespace LIBERADOR_PEDIDOS
{
    public partial class Form1 : Form
    {
        // PPCR40360, PPCR39534, PPCR42172 PPCR42760 PINMEXDataSet.autorizacion_pedidos.Fill,GetData (@PedidoIng)

        // Attributes
        private Connection conn = new Connection();

        // Methods
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void fillToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Query = "SELECT U_CODIGO_RUTA FROM PMP.PEDIDO WHERE PEDIDO = '" + tb_pedido.Text + "'";
                conn.Select();
                while (conn.Reader.Read())
                {
                    if (!conn.Reader.IsDBNull(0))
                    {
                        conn.Query = "[dbo].[autorizacion_pedidos]";
                        conn.ExecuteProcedure(tb_pedido.Text);

                        this.assignControls();

                        Verificar_Saldo_Positivo_Negativo();
                        Verificar_Cantidad_Disponible();

                        // Habilitamos controles.
                        lbl_autorizado2.Text = "SI ACREDITA";
                        pedido_tb.Enabled = true;
                    }
                    else
                    {
                        pedido_tb.Enabled = false;
                        this.cleanControls();
                        MessageBox.Show("Al pedido '" + tb_pedido.Text + "' le falta el código de ruta, no se puede procesar.");
                    }
                        
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void Verificar_Saldo_Positivo_Negativo()
        {
            try
            {
                float saldo = float.Parse(tb_saldo.Text.ToString());
                if (saldo > 0)
                {
                    tb_saldo.BackColor = Color.FromArgb(255, 0, 0);
                }
                else
                {
                    tb_saldo.BackColor = Color.FromArgb(0, 255, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Verificar_Cantidad_DisponibleSD_AUTORIZA()
        {
            try
            {
                float lc = float.Parse(tb_limite_credito.Text.ToString());
                //lc = (lc);
                float sl = float.Parse(tb_saldo.Text.ToString());
                lc = (lc - sl);
                float mt = float.Parse(tb_total_facturar.Text.ToString());
                tb_disponible.Text = lc.ToString();
                float ddf = float.Parse(tb_disponible.Text.ToString());
                ddf = (ddf - mt);
                tb_disponible_dff.Text = ddf.ToString();
                if (ddf >= 0)
                {
                    tb_disponible_dff.BackColor = Color.FromArgb(0, 255, 0);
                    lbl_autoriza.Text = "AUTORIZADO";
                    lbl_autoriza.BackColor = Color.FromArgb(0, 255, 0);
                    //
                    lbl_autorizado2.Text = "AUTORIZADO";
                    lbl_autorizado2.BackColor = Color.FromArgb(0, 255, 0);
                    String pedtxt = "";
                    String clietxt = "";
                    pedtxt = pedido_tb.Text.ToString();
                    clietxt = cLIENTETextBox.Text.ToString();
                    Actualizar_pedido(tb_pedido.Text.ToString(), clietxt);
                    //MessageBox.Show("El pedido fue autorizado");
                }
                else
                {
                    tb_disponible_dff.BackColor = Color.FromArgb(255, 0, 0);
                    lbl_autoriza.Text = "NO AUTORIZADO";
                    lbl_autoriza.BackColor = Color.FromArgb(255, 0, 0);
                    //
                    lbl_autorizado2.Text = "NO AUTORIZADO";
                    lbl_autorizado2.BackColor = Color.FromArgb(255, 0, 0);
                    MessageBox.Show("Solicita la autorización a Crédito y Cobranza");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("El pedido no existe");
            }
        }

        public void Verificar_Cantidad_DisponibleSD_AUTORIZA_MANUAL()
        {
            try
            {
                float lc = float.Parse(tb_limite_credito.Text.ToString());
                //lc = (lc);
                float sl = float.Parse(tb_saldo.Text.ToString());
                lc = (lc - sl);
                float mt = float.Parse(tb_total_facturar.Text.ToString());
                tb_disponible.Text = lc.ToString();

                float ddf = float.Parse(tb_disponible.Text.ToString());
                ddf = (ddf - mt);
                tb_disponible_dff.Text = ddf.ToString();
                
                    tb_disponible_dff.BackColor = Color.FromArgb(0, 255, 0);
                    lbl_autoriza.Text = "AUTORIZADO";
                    lbl_autoriza.BackColor = Color.FromArgb(0, 255, 0);
                    //
                    lbl_autorizado2.Text = "AUTORIZADO";
                    lbl_autorizado2.BackColor = Color.FromArgb(0, 255, 0);

                    String pedtxt = "";
                    String clietxt = "";
                    pedtxt = pedido_tb.Text.ToString();
                    clietxt = cLIENTETextBox.Text.ToString();
                    Actualizar_pedido(tb_pedido.Text.ToString(), clietxt);
            }
            catch (Exception err)
            {
                MessageBox.Show("El pedido no existe, se presento el siguiente error : "+err.Message.ToString());
            }
        }

        private void pedido_tb_Click(object sender, EventArgs e)
        {
            DialogResult result2 = MessageBox.Show("Seguro que desea solicitar aprobación el pedido", "Regresar", MessageBoxButtons.YesNoCancel);
            if (result2 == DialogResult.Yes)
            {
                conn.Query = "[dbo].[autorizacion_pedidos]";
                conn.ExecuteProcedure(tb_pedido.Text);

                this.assignControls();

                Verificar_Saldo_Positivo_Negativo();
                Verificar_Cantidad_DisponibleSD_AUTORIZA();
            }
            else if (result2 == DialogResult.No)
            {
                MessageBox.Show("No ha sido aprobado el pedido");
            }
            else if (result2 == DialogResult.Cancel)
            {
                MessageBox.Show("No ha sido aprobado el pedido");
            }
        }

        public void Verificar_Cantidad_Disponible()
        {
            try
            {
                float lc = float.Parse(tb_limite_credito.Text.ToString());
                //lc = (lc);
                float sl = float.Parse(tb_saldo.Text.ToString());
                lc = (lc - sl);
                float mt = float.Parse(tb_total_facturar.Text.ToString());
                tb_disponible.Text = lc.ToString();
                float ddf = float.Parse(tb_disponible.Text.ToString());
                ddf = (ddf - mt);
                tb_disponible_dff.Text = ddf.ToString();
                
                if (ddf >= 0)
                {
                    tb_disponible_dff.BackColor = Color.FromArgb(0, 255, 0);
                    lbl_autoriza.Text = "AUTORIZADO";
                    lbl_autoriza.BackColor = Color.FromArgb(0, 255, 0);
                    //
                    lbl_autorizado2.Text = "AUTORIZADO";
                    lbl_autorizado2.BackColor = Color.FromArgb(0, 255, 0);
                    String pedtxt = "";
                    String clietxt = "";
                    pedtxt = pedido_tb.Text.ToString();
                    clietxt = cLIENTETextBox.Text.ToString();
                }
                else
                {
                    tb_disponible_dff.BackColor = Color.FromArgb(255, 0, 0);
                    lbl_autoriza.Text = "NO AUTORIZADO";
                    lbl_autoriza.BackColor = Color.FromArgb(255, 0, 0);
                    //
                    lbl_autorizado2.Text = "NO AUTORIZADO";
                    lbl_autorizado2.BackColor = Color.FromArgb(255, 0, 0);
                    MessageBox.Show("Solicita la autorización a Crédito y Cobranza");
                }

            }
            catch (Exception err)
            {
                MessageBox.Show("El pedido no existe");
            }
        }

        public void Actualizar_pedido(String ped, String clien)
        {
            try
            {
                conn.Query = "UPDATE PMP.PEDIDO SET ESTADO = 'A', AUTORIZADO = 'S', FECHA_APROBACION = GETDATE() WHERE PEDIDO = '" + ped + "' AND CLIENTE = '" + clien + "'";
                if (conn.Update() == 1)
                    MessageBox.Show("PEDIDO : " + ped + " AUTORIZADO EN FACTURACIÓN");

                conn.Query = "UPDATE PMP.PEDIDO_AUTORIZA SET APROBADA = 'S',APROBADA_CREDITO='S',APROBADA_VENTAS = 'S',COMENTARIO = 'APROBACIÓN AUTOMATICA',USUARIO_VENTAS ='SA',FECHA_HORA_VENTAS = GETDATE(),USUARIO_CREDITO = 'SA',FECHA_HORA_CREDITO=GETDATE(),USUARIO='SA',FECHA_HORA=GETDATE() WHERE PEDIDO = '" + ped + "'";
                if (conn.Update() == 1)
                    MessageBox.Show("PEDIDO : " + ped + " AUTORIZADO EN CRÉDITO Y COBRANZA");
            }
            catch (Exception)
            {
                MessageBox.Show("Ocurrió un error al aprobar el pedido");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result2 = MessageBox.Show("Seguro que desea APROBAR el pedido", "Regresar", MessageBoxButtons.YesNoCancel);
            if (result2 == DialogResult.Yes)
            {
                conn.Query = "[dbo].[autorizacion_pedidos]";
                conn.ExecuteProcedure(tb_pedido.Text);

                this.assignControls();

                Verificar_Saldo_Positivo_Negativo();
                Verificar_Cantidad_DisponibleSD_AUTORIZA();
            }
            else if (result2 == DialogResult.No)
            {
                MessageBox.Show("No ha sido aprobado el pedido");
            }
            else if (result2 == DialogResult.Cancel)
            {
                MessageBox.Show("No ha sido aprobado el pedido");
            }
        }

        private void assignControls()
        {
            try
            {
                foreach (DataRow dr in Global.DataSet.Tables[0].Rows)
                {
                    cLIENTETextBox.Text = dr["CLIENTE"].ToString();
                    nOMBRE_CLIENTETextBox.Text = dr["NOMBRE_CLIENTE"].ToString();
                    tb_saldo.Text = dr["SALDO"].ToString();
                    tb_total_facturar.Text = dr["TOTAL_A_FACTURAR"].ToString();
                    tb_limite_credito.Text = dr["LIMITE_CREDITO"].ToString();
                    tb_forma_pago.Text = dr["FORMA_PAGO"].ToString();
                    tOTAL_MERCADERIATextBox.Text = dr["TOTAL_MERCADERIA"].ToString();
                    tOTAL_IMPUESTO1TextBox.Text = dr["TOTAL_IMPUESTO1"].ToString();
                    mONTO_DESCUENTO1TextBox.Text = dr["MONTO_DESCUENTO1"].ToString();
                    eMAIL_DOC_ELECTRONICOTextBox.Text = dr["EMAIL_DOC_ELECTRONICO"].ToString();
                    bACKORDERTextBox.Text = dr["BACKORDER"].ToString();
                    bODEGATextBox.Text = dr["BODEGA"].ToString();
                    cONDICION_PAGOTextBox.Text = dr["CONDICION_PAGO"].ToString();
                    fORMA_PAGOTextBox.Text = dr["FORMA_PAGO"].ToString();
                    uSO_CFDITextBox.Text = dr["USO_CFDI"].ToString();
                    u_CODIGO_RUTATextBox.Text = dr["U_CODIGO_RUTA"].ToString();
                    zONATextBox.Text = dr["ZONA"].ToString();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }

        private void cleanControls()
        {
            try
            {
                foreach (DataRow dr in Global.DataSet.Tables[0].Rows)
                {
                    cLIENTETextBox.Text = "";
                    nOMBRE_CLIENTETextBox.Text = "";
                    tb_saldo.Text = "";
                    tb_total_facturar.Text = "";
                    tb_limite_credito.Text = "";
                    tb_forma_pago.Text = "";
                    tOTAL_MERCADERIATextBox.Text = "";
                    tOTAL_IMPUESTO1TextBox.Text = "";
                    mONTO_DESCUENTO1TextBox.Text = "";
                    eMAIL_DOC_ELECTRONICOTextBox.Text = "";
                    bACKORDERTextBox.Text = "";
                    bODEGATextBox.Text = "";
                    cONDICION_PAGOTextBox.Text = "";
                    fORMA_PAGOTextBox.Text = "";
                    uSO_CFDITextBox.Text = "";
                    u_CODIGO_RUTATextBox.Text = "";
                    zONATextBox.Text = "";
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
    }
}
