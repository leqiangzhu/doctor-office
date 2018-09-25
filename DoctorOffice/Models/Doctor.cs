using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using DoctorOffice;

namespace DoctorOffice.Models
{
    public class Doctor
    {
        private int _id;
        private string _name;
        private string _specialty;

        public Doctor(string newName, string newSpecialty, int newId =0)
        {
            _name = newName;
            _specialty = newSpecialty;
            _id = newId;
        }
        public string GetName()
        {
            return _name;
        }
        public string GetSpecialty()
        {
            return _specialty;
        }
        public int GetId()
        {
            return _id;
        }
        public override bool Equals(System.Object otherDoctor)
        {
            if (!(otherDoctor is Doctor))
            {
                return false;
            }
            else
            {
                Doctor newDoctor = (Doctor) otherDoctor;
                bool idEquality = this.GetId() == newDoctor.GetId();
                bool specialtyEquality = this.GetSpecialty() == newDoctor.GetSpecialty();
                bool nameEquality = this.GetName() == newDoctor.GetName();
                return(idEquality && specialtyEquality && nameEquality);
            }
        }
        public override int GetHashCode()
        {
            string allHash = this.GetName() + this.GetSpecialty();
            return allHash;
        }
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateComman() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO doctors (doctor_name, doctor_specialty) VALUES (@doctorName, @doctorSpecialty);";

            MySqlParameter doctorName = new MySqlParameter();
            doctorName.ParameterName = "@doctorName";
            doctorName.Value = this._name;
            cmd.Parameters.Add(doctorName);

            MySqlParameter doctorSpecialty = new MySqlParameter();
            doctorSpecialty.ParameterName = "@doctorSpecialty";
            doctorSpecialty.Value = this._specialty;
            cmd.Parameters.Add(doctorSpecialty);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public static List<Doctor> GetAll()
        {
            List<Doctor> allDoctors = new List<Doctor>() {};
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = "@SELECT * FROM dcotors;";

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            
            while (rdr.Read())
            {
                int doctorId = rdr.GetInt32(0);
                string doctorName = rdr.GetString(1);
                string doctorSpecialty = rdr.GetString(2);
                Doctor newDoctor = new Doctor(doctorName, doctorSpecialty, doctorId);
                allDoctors.Add(newDoctor);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allDoctors;
        }

        public static Doctor Find(int id)
        {
            MySqlConnection conn = MySqlConnection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM doctors WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int doctorId = 0;
            string doctorName = "";
            string doctorSpecialty = "";

            while(rdr.Read())
            {
                doctorId = rdr.GetInt32(0);
                doctorName = rdr.GetString(1);
                doctorSpecialty = rdr.GetString(2);
            }

            Doctor newDoctor = new Doctor(doctorName, doctorSpecialty, doctorId);

            conn.Close();
            if (conn!=null)
            {
                conn.Dispose();
            }
            return newDoctor;
        }
                 
    }
}
