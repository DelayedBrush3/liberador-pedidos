using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LIBERADOR_PEDIDOS
{
    public partial class Form1 : Form
    {
        // PPCR40360, PPCR39534

        SqlConnection con = new SqlConnection("Data Source=10.10.0.14;Initial Catalog=PINMEX;User ID=BRYANPC;Password=Pinmex1;");

        public Form1()
        {
            InitializeComponent();
        }

        private void fillToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.autorizacion_pedidosTableAdapter.Fill(this.pINMEXDataSet.autorizacion_pedidos, tb_pedido.Text);
                Verificar_Saldo_Positivo_Negativo();
                Verificar_Cantidad_Disponible();
                lbl_autorizado2.Text = "SI ACREDITA";
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
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
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
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
                    MessageBox.Show("El pedido fue autorizado");
                }
                else
                {
                    tb_disponible_dff.BackColor = Color.FromArgb(255, 0, 0);
                    lbl_autoriza.Text = "NO AUTORIZADO";
                    lbl_autoriza.BackColor = Color.FromArgb(255, 0, 0);
                    //
                    lbl_autorizado2.Text = "NO AUTORIZADO";
                    lbl_autorizado2.BackColor = Color.FromArgb(255, 0, 0);
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
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pedido_tb_Click(object sender, EventArgs e)
        {
            DialogResult result2 = MessageBox.Show("Seguro que desea solicitar aprobación el pedido", "Regresar", MessageBoxButtons.YesNoCancel);
            if (result2 == DialogResult.Yes)
            {
                this.autorizacion_pedidosTableAdapter.Fill(this.pINMEXDataSet.autorizacion_pedidos, tb_pedido.Text);
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
                String query_FACT = "";
                String query_CRED = "";
                con.Open();
                query_FACT = "UPDATE PMP.PEDIDO SET ESTADO='A',AUTORIZADO = 'S',FECHA_APROBACION=GETDATE() WHERE PEDIDO = '" + ped + "' AND CLIENTE = '" + clien + "'";
                SqlCommand cmd1 = new SqlCommand(query_FACT, con);
                int q1 = 0;
                q1 = cmd1.ExecuteNonQuery();
                con.Close();

                //DOS
                con.Open();
                query_CRED = "UPDATE PMP.PEDIDO_AUTORIZA SET APROBADA = 'S',APROBADA_CREDITO='S',APROBADA_VENTAS = 'S',COMENTARIO = 'APROBACIÓN AUTOMATICA',USUARIO_VENTAS ='SA',FECHA_HORA_VENTAS = GETDATE(),USUARIO_CREDITO = 'SA',FECHA_HORA_CREDITO=GETDATE(),USUARIO='SA',FECHA_HORA=GETDATE() WHERE PEDIDO = '" + ped + "'";
                SqlCommand cmd2 = new SqlCommand(query_CRED, con);
                int q2 = 0;
                q2 = cmd2.ExecuteNonQuery();
                con.Close();

                if (q1 == 1)
                    MessageBox.Show("PEDIDO : " + ped + " AUTORIZADO EN FACTURACIÓN");

                if (q2 == 1)
                    MessageBox.Show("PEDIDO : " + ped + " AUTORIZADO EN CRÉDITO Y COBRANZA");
            }
            catch (Exception)
            {
                MessageBox.Show("Ocurrió un error al aprobar el pedido");
            }
        }

        private void tOTAL_MERCADERIATextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void tb_pedido_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void tb_pedido_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                    this.autorizacion_pedidosTableAdapter.Fill(this.pINMEXDataSet.autorizacion_pedidos, tb_pedido.Text);
                    Verificar_Saldo_Positivo_Negativo();
                    Verificar_Cantidad_Disponible();
                lbl_autorizado2.Text = "SI ACREDITA";
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result2 = MessageBox.Show("Seguro que desea APROBAR el pedido", "Regresar", MessageBoxButtons.YesNoCancel);
            if (result2 == DialogResult.Yes)
            {
                this.autorizacion_pedidosTableAdapter.Fill(this.pINMEXDataSet.autorizacion_pedidos, tb_pedido.Text);
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
    }
}
