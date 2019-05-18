using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToSql
{
    class Member
    {
        public string ime;
        public string prezime;
        public string email;
        public string phone;
        public string klub;
        public string year;
        public string occupation;
        public string fieldOfWork;
        public string role;
        public string hobby;
    }
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"I:\Desktop\Rotaract\Distrikt\Adresar ljudi\Rotaract members' information.csv";
            StreamReader sr = new StreamReader(filePath);
            List<Member> members = new List<Member>();
            var lines = new List<string[]>();
            int Row = 0;
            while (!sr.EndOfStream)
            {
                string rl = sr.ReadLine();
                rl = rl.Replace("\",\"", "`");
                string[] Line = rl.Split('`');
                lines.Add(Line);
                Row++;
                // Console.WriteLine(Row);
            }
            int i = 0;
            foreach (var line in lines)
            {
                if (i++ == 0)
                    continue;
                Member m = new Member();
                m.ime = line[1].Replace("\"", "");
                m.prezime = line[2].Replace("\"", "");
                m.email = line[3].Replace("\"", "");
                m.phone = line[4].Replace("\"", "");
                m.klub = line[5].Replace("\"", "");
                m.year = line[6].Replace("\"", "");
                m.occupation = line[7].Replace("\"", "");
                m.fieldOfWork = line[8].Replace("\"", "");
                m.role = line[9].Replace("\"", "");
                m.hobby = line[10].Replace("\"", "");
                for (int j = 11; j < line.Length; j++)
                {
                    m.hobby += ("," + line[j]);
                }
                members.Add(m);
            }
            ;
            foreach (var m in members)
            {

                using (SqlConnection connection = new SqlConnection("data source=ALEKSA\\SQLEXPRESS;initial catalog=RTC;integrated security=True"))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;            // <== lacking
                        command.CommandType = CommandType.Text;
                        command.CommandText = "INSERT into [dbo].[Members] ([Ime],[Prezime],[Email],[Klub],[Godina],[Zanimanje],[Oblast],[Pozicija],[Hobi],[Telefon]) VALUES (@ime, @prezime, @email, @klub, @godina, @posao, @polje, @pozicija, @hobi, @telefon)";
                        command.Parameters.AddWithValue("@ime", m.ime);
                        command.Parameters.AddWithValue("@prezime", m.prezime);
                        command.Parameters.AddWithValue("@email", m.email);
                        command.Parameters.AddWithValue("@klub", m.klub);
                        command.Parameters.AddWithValue("@godina", m.year);
                        command.Parameters.AddWithValue("@posao", m.occupation);
                        command.Parameters.AddWithValue("@polje", m.fieldOfWork);
                        command.Parameters.AddWithValue("@pozicija", m.role);
                        command.Parameters.AddWithValue("@telefon", m.phone);
                        command.Parameters.AddWithValue("@hobi", m.hobby);

                        try
                        {
                            connection.Open();
                            int recordsAffected = command.ExecuteNonQuery();
                        }
                        catch (SqlException)
                        {
                            Console.Write("greska");
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }

        }
    }
}

/*
INSERT INTO [dbo].[Members]
           ([Ime],[Prezime],[Email],[Klub],[Godina],[Zanimanje],[Oblast],[Pozicija],[Hobi]) VALUES (@ime, @prezime, @email, @klub, @godina, @posao, @polje, @pozicija, @hobi)
GO
*/
