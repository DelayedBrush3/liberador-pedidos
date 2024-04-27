using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LIBERADOR_PEDIDOS.Class
{
    internal class Connection
    {
        // Attributes
        private SqlConnectionStringBuilder _builder = new SqlConnectionStringBuilder();
        private SqlConnection _conn = null;
        private string _query = null;
        private SqlCommand _command = null;
        private SqlDataReader _reader = null;
        private string[] _arguments = null;


        // Properties
        private SqlConnectionStringBuilder Builder
        {
            get => _builder;
            set => _builder = value;
        }

        private SqlConnection Conn
        {
            get => _conn;
            set => _conn = value;
        }

        public string Query
        {
            get => _query;
            set => _query = value;
        }

        private SqlCommand Command
        {
            get => _command;
            set => _command = value;
        }

        public SqlDataReader Reader
        {
            get => _reader;
            set => _reader = value;
        }

        private string[] Arguments
        {
            get => _arguments;
            set => _arguments = value;
        }


        // Methods
        [DllImport("Seg70000")]
        private static extern void GetProxyPassword(string usuario, string proxyUsuario, IntPtr proxyPassword);

        private void getArguments()
        {
            try
            {
                // $U $P $C $B $S Parametros de conexión de opciones configurables de softland
                // username password compañia bdName server Parametros de conexión local
                Arguments = Environment.GetCommandLineArgs();

                if (Arguments[1] != "SA" && Arguments[1] != "sa")
                {
                    // Get User
                    IntPtr proxyPassword = Marshal.StringToHGlobalAnsi(new string(Convert.ToChar(0), 30));
                    Global.GlobalUser = "X" + Arguments[1];
                    GetProxyPassword(Arguments[1].ToUpper(), Global.GlobalUser.ToUpper(), proxyPassword);

                    // Get Pass
                    var password = Marshal.PtrToStringAnsi(proxyPassword, 30);
                    Global.GlobalPass = password.Substring(0, password.IndexOf(Convert.ToChar(0), 1));

                    // Get Company
                    Global.GlobalComp = Arguments[3];

                    // Get DB
                    Global.GlobalDB = Arguments[4];

                    // Get Server
                    Global.GlobalServ = Arguments[5];
                }
                else
                {
                    // Get User
                    Global.GlobalUser = Arguments[1];

                    // Get Pass
                    Global.GlobalPass = Arguments[2];

                    // Get Company
                    Global.GlobalComp = Arguments[3];

                    // Get DB
                    Global.GlobalDB = Arguments[4];

                    // Get Server
                    Global.GlobalServ = Arguments[5];
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }

        private void Connect()
        {
            try
            {
                // Get Arguments in global variables
                this.getArguments();

                // Build string connection
                Builder.TrustServerCertificate = true;
                Builder.DataSource = Global.GlobalServ;
                Builder.UserID = Global.GlobalUser;
                Builder.Password = Global.GlobalPass;
                Builder.InitialCatalog = Global.GlobalDB;

                Conn = new SqlConnection(Builder.ConnectionString);
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }

        public void Select()
        {
            try
            {
                // Establecer conexión
                this.Connect();

                // Inicializar command
                using (Command = new SqlCommand(Query, Conn))
                {
                    // Se abre la conexión
                    Conn.Open();

                    // Se ejecuta el procedimiento
                    Reader = Command.ExecuteReader();
                }
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }

        public void ExecuteProcedure(string pedido)
        {
            try
            {
                // Establecer conexión
                this.Connect();

                // Inicializar command
                using (Command = new SqlCommand())
                {
                    // Al comando se le asigna una conexión
                    Command.Connection = Conn;

                    // Se le indica el tipo de comando y el nombre
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.CommandText = Query;

                    // Se añaden los parámetros de entrada
                    Command.Parameters.AddWithValue("@PedidoIng", pedido);

                    // Se abre la conexión
                    Conn.Open();

                    // Declaramos un DataAdapter a partir del SqlCommand y un DataSet para almacenar los datos.
                    Global.GlobalAutorizacion = new SqlDataAdapter(Command);
                    Global.DataSet = new DataSet();

                    // Por último, rellenamos el DataSet
                    Global.GlobalAutorizacion.Fill(Global.DataSet);

                    // Retornamos DataSet
                    // return dataSet;
                }
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }

        public int Update()
        {
            try
            {
                // Establecer conexión
                this.Connect();

                // Inicializar command
                using (Command = new SqlCommand(Query, Conn))
                {
                    // Se abre la conexión
                    Conn.Open();

                    // Retornamos el query
                    return Command.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
    }
}
