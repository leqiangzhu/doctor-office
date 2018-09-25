using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using DoctorOffice;

namespace DoctorOffice.Models
{
    public class Specialty
    {
        private int _id;
        private string _specialty;

        public Specialty(string newSpecialty, int newId=0)
        {
            _id = newId;
            _specialty = newSpecialty;
        }

        public string GetSpecialty()
        {
            return _specialty;
        }
        public int GetId()
        {
            return _id;
        }
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd =conn.CreateCommand() as MySqlCommand;
            cmd.CommandText="@INSERT INTO specialties (specialty_name) VALUES (@newSpecialty);";

            MySqlParameter newSpecialty=new MySqlParameter();
            newSpecialty.ParameterName="@newSpecialty";
            newSpecialty.Value= this._specialty;
            cmd.Parameters.Add(newSpecialty);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public static List<Specialty> GetAll()
        {
            List<Specialty> allSpecialty =new List<Specialty>(){};
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd =conn.CreateCommand() as MySqlCommand;
            cmd.CommandText="@SELECT * FROM specialties;";

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read()){
                int specialtyId =rdr.GetInt32(0);
                string specialtyName=rdr.GetString(1);
                Specialty newSpecialty=new Specialty(specialtyName, specialtyId);
                allSpecialty.Add(newSpecialty);
            }
             conn.Close();
            if (conn !=null)
            {
                conn.Dispose();
            }
            return allSpecialty;
        }


        public static Specialty Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd =conn.CreateCommand() as MySqlCommand;
            cmd.CommandText=@"SELECT * FROM specialties WHERE id =(@searchId);";

            MySqlParameter searchId =new MySqlParameter();
            searchId.ParameterName="@searchId";
            searchId.Value=id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int specialtyId =0;
            string  specialtyName="";
            while(rdr.Read())
            {
                specialtyId = rdr.GetInt32(0);
                specialtyName = rdr.GetString(1);
            }
            Specialty newSpecialty =new Specialty(specialtyName,specialtyId);

             conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
            return newSpecialty;


        }
        public void AddDoctor(Doctor newDoctor)
        {
            MySqlConnection conn=DB.Connection();
            conn.Open();   
                
            var cmd =conn.CreateCommand() as MySqlCommand;
            cmd.CommandText=@"INSERT INTO doctors_specialties (specialties_id, doctors_id) VALUES (@SpecialtyId, @DoctorId);";

            MySqlParameter specialties_id=new MySqlParameter();
            specialties_id.ParameterName="@SpecialtyId";
            specialties_id.Value = _id;
            cmd.Parameters.Add(specialties_id);

            MySqlParameter doctors_id=new MySqlParameter();
            doctors_id.ParameterName="@DoctorId";
            doctors_id.Value = newDoctor.GetId();
            cmd.Parameters.Add(doctors_id);

            cmd.ExecuteNonQuery();

            conn.Close();
            if(conn !=null)
            {
                conn.Dispose();
            }

        }
        public List<Doctor> GetDoctor()
        {   
            MySqlConnection conn=DB.Connection();
            conn.Open();   

            var cmd =conn.CreateCommand() as MySqlCommand;
            cmd.CommandText=@"SELECT specialties.* FROM doctors 
            JOIN doctors_specialties ON (doctors.id = doctors_specialties.doctors_id)
            JOIN doctors_specialties ON (specialties.id=doctors_specialties.specialties_id)
            WHERE doctors.id=@doctorIdParameter;";

            MySqlParameter doctorIdParameter = new MySqlParameter();
            doctorIdParameter.ParameterName = "@doctorIdParameter";
            doctorIdParameter.Value = this._id;
            cmd.Parameters.Add(doctorIdParameter);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Doctor> doctors = new List<Doctor> {};

            while(rdr.Read())
            {
                int doctorId = rdr.GetInt32(0);
                string doctorName = rdr.GetString(1);
                Doctor newDoctor = new Doctor(doctorName, doctorId);
                doctors.Add(newDoctor); 
            }

            conn.Close();
            if(conn !=null)
            {
                conn.Dispose();
            }
            return doctors;
        }
    }
}