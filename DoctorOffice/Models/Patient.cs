using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using DoctorOffice;

namespace DoctorOffice.Models
{
    public class Patient
    {
        private int _id;
        private string _name;
        private DateTime _birthday;

        public Patient (string newName, DateTime newBirthday, int newId=0)
        {
            _name = newName;
            _birthday = newBirthday;
            _id = id;
        }

        public string GetName()
        {
            return _name;
        }
        public DateTime GetBirthday()
        {
            return _birthday;
        }
        public int GetId()
        {
            return _id;
        }

        public void Save()
        {
            MySqlConnection conn= DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText="@INSERT INTO patients (patient_name,patient_birthday) VALUES (@name,@newBirthday);";
            
            MySqlParameter newName = new MySqlParameter();
            newName.ParameterName = "@name";
            newName.Value = this._name;
            cmd.Parameters.Add(newName);

            MySqlParameter newBirthday = new MySqlParameter();
            newBirthday.ParameterName = "@newBirthday";
            newBirthday.Value = this._birthday;
            cmd.Parameters.Add(newBirthday);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

        }
        public static List<Patient> GetAll()
        {
            List <Patient> allPatients = new List<Patient>(){};
            MySqlConnection conn =DB.Connection();
            conn.Open();

            var cmd=conn.CreateCommand() as MySqlCommand;
            cmd.CommandText="@SELECT * FROM patients;";

            var rdr =cmd.ExecuteReader() as MySqlDataReader;

            while (rdr.Read())
            {
             int patientId = rdr.GetInt32(0);
             string patientName = rdr.GetString(1);
             DateTime patientBirthday = new DateTime(2);
             Patient newPatient = new Patient(patientName, patientBirthday, patientId);
             allPatients.Add(newPatient);
            }
            
            conn.Close();
            if (conn !=null)
            {
                conn.Dispose();
            }
            return allPatients;

        }
        
        public static Patient Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM patients WEHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int patientId = 0;
            string patientName = "";
            DateTime patientBirthday = new DateTime(2012, 1, 23);

            while(rdr.Read())
            {
                patientId = rdr.GetInt32(0);
                patientName = rdr.GetString(1);
                patientBirthday = rdr.GetDateTime(2);
            }
            Patient newPatient = new Patient(patientName, patientBirthday, patientId);

            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
            return newPatient;
        }
  

    }
}
