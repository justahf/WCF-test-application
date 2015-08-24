using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;
using System.ServiceModel.Description;

using System.Data.SQLite;
using System.Collections;
using System.IO;

namespace HostProcess
{
    class DataBase
    {
        private SQLiteConnection dbConnection;

        public DataBase(string name)
        {
            if (!File.Exists(name))
            {
                SQLiteConnection.CreateFile(name);
                dbConnection = new SQLiteConnection("Data Source="+name+";Version=3;");
                InitDataBase();
            }
            else
            {
                dbConnection = new SQLiteConnection("Data Source=" + name + ";Version=3;");
            }
        }

        ~DataBase()
        {
            dbConnection.Close();
        }

        private void InitDataBase()
        {
            SqlCommand("CREATE TABLE phonebook (name VARCHAR(25), phonenumber INTEGER PRIMARY KEY)");
            SqlCommand("INSERT INTO phonebook (name, phonenumber) VALUES ('Ivan', 12345600)");
            SqlCommand("INSERT INTO phonebook (name, phonenumber) VALUES ('Vasya', 12345601)");
            SqlCommand("INSERT INTO phonebook (name, phonenumber) VALUES ('Pyotr', 12345602)");
            SqlCommand("INSERT INTO phonebook (name, phonenumber) VALUES ('Maria', 12345603)");
            SqlCommand("INSERT INTO phonebook (name, phonenumber) VALUES ('Sveta', 12345604)");
            SqlCommand("INSERT INTO phonebook (name, phonenumber) VALUES ('Misha', 12345605)");
            SqlCommand("INSERT INTO phonebook (name, phonenumber) VALUES ('Alla', 12345606)");
        }

        public void SqlCommand(string command)
        {
            if (dbConnection == null) return;
            if (dbConnection.State != System.Data.ConnectionState.Open)
                dbConnection.Open();
            SQLiteCommand sqlcmd = new SQLiteCommand(command, dbConnection);
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex) {}
            dbConnection.Close();
        }

        public List<List<string>> SqlCommandReturn(string command)
        {
            if (dbConnection == null) return null;
            if (dbConnection.State != System.Data.ConnectionState.Open)
                dbConnection.Open();
            List<List<string>> results = new List<List<string>>();
            SQLiteCommand sqlcmd = new SQLiteCommand(command, dbConnection);
            SQLiteDataReader reader;
            try
            {
                reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {
                    List<String> row = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                        row.Add(reader[i].ToString());
                    results.Add(row);
                }
            }
            catch (Exception ex) {}
            dbConnection.Close();
            return results;
        }
    }

    class Program
    {
        const string serviceAddress1 = "http://localhost:8080/testService";
        const string dbFileName = "database.sqlite";
        public static DataBase dbase;

        [ServiceContract]
        public interface ITestService
        {
            [OperationContract]
            string SearchName(int phone);
            [OperationContract]
            List<int> SearchPhone(string name);
            [OperationContract]
            List<List<string>> GetAllContacts();
            [OperationContract]
            int PushContact(string name, int phonenumber);
            [OperationContract]
            int ModifyContact(int phonenumber, int newnumber);
            [OperationContract]
            int DeleteContact(int phonenumber);
        }



        public class TestService : ITestService
        {
            public string SearchName(int phone)
            {
                List<List<string>> result = dbase.SqlCommandReturn("SELECT name FROM phonebook WHERE phonenumber = " + phone.ToString());
                if (result.Count > 0) return result[0][0];
                else return null;
            }

            public List<int> SearchPhone(string name)
            {
                List<List<string>> result = dbase.SqlCommandReturn("SELECT phonenumber FROM phonebook WHERE name = '" + name + "'");
                if (result.Count == 0) return null;
                else
                {
                    List<int> numbers = new List<int>();
                    foreach (List<string> row in result)
                        numbers.Add(Convert.ToInt32(row[0]));
                    return numbers;
                }
            }

            public List<List<string>> GetAllContacts()
            {
                List<List<string>> result = dbase.SqlCommandReturn("SELECT * FROM phonebook");
                return result;
            }

            public int PushContact(string name, int phonenumber)
            {
                if (name.Length == 0 || name.Length > 15) return 1;
                else dbase.SqlCommand("INSERT INTO phonebook (name, phonenumber) VALUES ('" + name + "', " + phonenumber + ")");
                return 0;
            }

            public int ModifyContact(int phonenumber, int newnumber)
            {
                dbase.SqlCommand("UPDATE phonebook SET phonenumber=" + newnumber + " WHERE phonenumber=" + phonenumber);
                return 0;
            }

            public int DeleteContact(int phonenumber)
            {
                dbase.SqlCommand("DELETE FROM phonebook WHERE phonenumber=" + phonenumber);
                return 0;
            }

        }

        static void Main(string[] args)
        {
            dbase = new DataBase(dbFileName);
            List<List<string>> dbContent = dbase.SqlCommandReturn("SELECT * FROM phonebook");
            Console.WriteLine("DATABASE CONTENT:");
            foreach (List<String> row in dbContent)
                Console.WriteLine("NAME: {0} \tPHONE: {1}", row[0], row[1]);

            Uri baseAddress = new Uri(serviceAddress1);

            using (ServiceHost host = new ServiceHost(typeof(TestService), baseAddress))
            {
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);
                host.Open();
                Console.WriteLine("\n\nSercive Address: {0}", baseAddress);
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();
                host.Close();
            }
        }
    }
}
