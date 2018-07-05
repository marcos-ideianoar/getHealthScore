using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Linq.Csv;
using System.IO;
using System.Data.SqlClient;
using System.Threading;


namespace HealthScore
{
    class Program
    {
        public static void Main(string[] args)
        {
            //Esse é um teste

            string[] HealthScoreLog = new string[100];

            Console.WriteLine("Buscando usuários e senhas...");
            Thread.Sleep(2000);

            string filePathLog = @"C:\Users\Ideia no Ar\Desktop\HealthScoreLog.csv";

            using (StreamWriter sw = new StreamWriter(filePathLog))
            {

                sw.WriteLine("dbName,Vendedores,Pedidos,Orçamento,Produtos,Portfólio");

                string filePath = @"C:\HealthScore.csv";
                char delimiter = ',';

                var x = 0;

                using (StreamReader sr = new StreamReader(filePath))
                {

                    //ler linha por linha e separa usuario de senha
                    String line = sr.ReadToEnd();
                    String[] substrings = line.Split(delimiter);
                    /* foreach (var substring in substrings)
                     {
                        // Console.WriteLine(substring);

                     }*/


                    Console.WriteLine("Todos os dados foram carregados!");
                    Console.WriteLine("Tecle [ENTER] para continuar a pesquisa SQL");


                    Console.Read();

                    var resultVendedor = "";
                    var resultPedido = "";
                    var resultOrce = "";
                    var resultProduto = "";
                    var resultPortfolio = "";

                    var serverName = "";
                    var dbUser = "dbadmin";
                    var dbName = "";
                    var password = "";

                    var trade = 0;

                    while (true)
                    {
                        while (trade == 0)
                        {
                            if (x % 2 == 0)
                            {
                                dbName = substrings[x];
                            }
                            else
                            {
                                password = substrings[x];
                                trade = 1;
                            }
                            x += 1;
                        }
                        trade = 0;



                        /*Console.Write("Digite o nome do banco de dados: ");
                        dbName = Console.ReadLine();

                        Console.Write("Digite a senha do banco de dados: ");
                        password = Console.ReadLine();*/

                        //inicia processo de conexão ao db e pesquisa
                        Console.WriteLine(dbName + " - Carregando...");
                        serverName = dbName + ".database.windows.net,1433";


                        resultVendedor = ExecuteQueryComStringRetorno("SELECT count(*) as Vendedores FROM AspNetUsers  WHERE Discriminator = 'Seller'", dbName, serverName, password, dbUser);
                        resultPedido = ExecuteQueryComStringRetorno("SELECT count(*) as Pedidos FROM [Order]", dbName, serverName, password, dbUser);
                        resultOrce = ExecuteQueryComStringRetorno("SELECT count(*) as Orçamentos FROM Quote", dbName, serverName, password, dbUser);
                        resultProduto = ExecuteQueryComStringRetorno("SELECT count(*) as Produtos FROM Product", dbName, serverName, password, dbUser);
                        resultPortfolio = ExecuteQueryComStringRetorno("SELECT count(*) as Portfólio FROM Portfolio", dbName, serverName, password, dbUser);

                        HealthScoreLog[x] = dbName + "," + resultVendedor + "," + resultPedido + "," + resultOrce + "," + resultProduto + "," + resultPortfolio;

                        Console.WriteLine();
                        Console.WriteLine("--------");
                        Console.WriteLine();
                        Console.WriteLine("::. " + dbName + ".::");
                        Console.Write("Vendedores: ");
                        Console.WriteLine(resultVendedor);
                        Console.Write("Pedidos: ");
                        Console.WriteLine(resultPedido);
                        Console.Write("Orçamento: ");
                        Console.WriteLine(resultOrce);
                        Console.Write("Produtos: ");
                        Console.WriteLine(resultProduto);
                        Console.Write("Portfólio: ");
                        Console.Write(resultPortfolio);
                        Console.WriteLine();
                        Console.WriteLine();

                        Console.WriteLine(HealthScoreLog[x]);

                        Console.WriteLine("--------");
                        Console.WriteLine();
                        Console.WriteLine();

                        sw.Write(HealthScoreLog[x]);

                    }

                }





            }





        }



        public static SqlConnection Conexao(string dbName, string serverName, string password, string dbUser)
        {
            /*
             * 
            var dbName = "artebasedev";
            var serverName = dbName + ".database.windows.net,1433";
            var password = "NWtWnxWU2HL3P4dy";
            var dbUser = "dbadmin";
             */

            string conec = "Data Source=" + serverName + ";Initial Catalog=" + dbName + ";User ID=" + dbUser + ";Password=" + password + ";Language=Portuguese";

            SqlConnection cn = new SqlConnection(conec);

            return cn;
        }

        public static SqlConnection AbrirConexao(string dbName, string serverName, string password, string dbUser)
        {


            SqlConnection cn = Conexao(dbName, serverName, password, dbUser);

            try
            {
                cn.Open();
                return cn;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void fecharConexao(SqlConnection cn)
        {
            try
            {
                cn.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static DataTable ExecuteQuery(string sql, string dbName, string serverName, string password, string dbUser)
        {



            try
            {
                SqlCommand sqlComm = new SqlCommand(sql, AbrirConexao(dbName, serverName, password, dbUser));

                sqlComm.ExecuteNonQuery();

                SqlDataAdapter da = new SqlDataAdapter(sqlComm);

                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string ExecuteQueryComStringRetorno(string sql, string dbName, string serverName, string password, string dbUser)
        {
            try
            {
                string dado;

                SqlCommand sqlComm = new SqlCommand(sql, AbrirConexao(dbName, serverName, password, dbUser));

                sqlComm.ExecuteNonQuery();

                SqlDataAdapter da = new SqlDataAdapter(sqlComm);

                DataTable dt = new DataTable();
                da.Fill(dt);

                dado = dt.Rows[0][0].ToString();

                return dado;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
