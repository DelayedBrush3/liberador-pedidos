using System.Data;
using System.Data.SqlClient;

namespace LIBERADOR_PEDIDOS.Class
{
    static class Global
    {
        // Attributes
        private static string _globalUser = "";
        private static string _globalPass = "";
        private static string _globalComp = "";
        private static string _globalDB = "";
        private static string _globalServ = "";
        private static SqlDataAdapter _autorizacion_pedidosTableAdapter;
        private static DataSet _dataSet;

        // Properties
        public static string GlobalUser
        {
            get => _globalUser;
            set => _globalUser = value;
        }

        public static string GlobalPass
        {
            get => _globalPass;
            set => _globalPass = value;
        }

        public static string GlobalComp
        {
            get => _globalComp;
            set => _globalComp = value;
        }

        public static string GlobalDB
        {
            get => _globalDB;
            set => _globalDB = value;
        }

        public static string GlobalServ
        {
            get => _globalServ;
            set => _globalServ = value;
        }

        public static SqlDataAdapter GlobalAutorizacion
        {
            get => _autorizacion_pedidosTableAdapter;
            set => _autorizacion_pedidosTableAdapter = value;
        }

        public static DataSet DataSet
        {
            get => _dataSet;
            set => _dataSet = value;
        }
    }
}
