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

        public Doctor(string newName, int newId =0)
        {
            _name = newName;
            _id = newId;
        }
        public string GetName()
        {
            return _name;
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
                bool nameEquality = this.GetName() == newDoctor.GetName();
                return(idEquality && nameEquality);
            }
        }
        public override int GetHashCode()
        {
            string allHash = this.GetName();
            return allHash.GetHashCode();
        }
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO doctors (doctor_name) VALUES (@doctorName);";

            MySqlParameter doctorName = new MySqlParameter();
            doctorName.ParameterName = "@doctorName";
            doctorName.Value = this._name;
            cmd.Parameters.Add(doctorName);

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
            cmd.CommandText = "@SELECT * FROM doctors;";

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            
            while (rdr.Read())
            {
                int doctorId = rdr.GetInt32(0);
                string doctorName = rdr.GetString(1);
                Doctor newDoctor = new Doctor(doctorName, doctorId);
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
            MySqlConnection conn = DB.Connection();
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

            while(rdr.Read())
            {
                doctorId = rdr.GetInt32(0);
                doctorName = rdr.GetString(1);
            }

            Doctor newDoctor = new Doctor(doctorName, doctorId);

            conn.Close();
            if (conn!=null)
            {
                conn.Dispose();
            }
            return newDoctor;
        }

        public void AddPatient(Patient newPatient)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO doctors_patients (doctor_id, patient_id) VALUES (@DoctorId, @PatientId);";

            MySqlParameter doctor_id = new MySqlParameter();
            doctor_id.ParameterName = "@DoctorId";
            doctor_id.Value = _id;
            cmd.Parameters.Add(doctor_id);

            MySqlParameter patient_id = new MySqlParameter();
            patient_id.ParameterName = "@PatientId";
            patient_id.Value = newPatient.GetId();
            cmd.Parameters.Add(patient_id);

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Patient> GetPatient()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

            cmd.CommandText = @"SELECT patients.* FROM doctors
            JOIN doctors_patients ON (doctors.id = doctors_patients.doctor_id)
            JOIN doctors_patients ON (doctors_patients.patient_id = patients.id)
            WHERE doctors.id = @DoctorsId;";

            MySqlParameter doctorIdParameter = new MySqlParameter();
            doctorIdParameter.ParameterName = @"DoctorsId";
            doctorIdParameter.Value = _id;
            cmd.Parameters.Add(doctorIdParameter);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Patient> patients = new List<Patient> {};

            while(rdr.Read())
            {
                int patientId = rdr.GetInt32(0);
                string patientName = rdr.GetString(1);
                DateTime patientBirthday = rdr.GetDateTime(2);
                Patient newPatient = new Patient(patientName, patientBirthday, patientId);
                patients.Add(newPatient);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return patients;
        }

       
            public void AddSpecialty(Specialty newSpecialty)
            {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO doctors_specialties (doctor_id, specialties_id) VALUES (@DoctorId, @SpecialtyId);";

            MySqlParameter doctor_id = new MySqlParameter();
            doctor_id.ParameterName = "@DoctorId";
            doctor_id.Value = _id;
            cmd.Parameters.Add(doctor_id);

            MySqlParameter specialties_id = new MySqlParameter();
            specialties_id.ParameterName = "@SpecialtyId";
            specialties_id.Value = newSpecialty.GetId();
            cmd.Parameters.Add(specialties_id);

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Specialty> GetSpecialty()
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
            List<Specialty> specialties = new List<Specialty> {};

            while(rdr.Read())
            {
                int SpecialtyId = rdr.GetInt32(0);
                string SpecialtyName = rdr.GetString(1);
                Specialty newSpecialty = new Specialty(SpecialtyName, SpecialtyId);
                specialties.Add(newSpecialty); 
            }

            conn.Close();
            if(conn !=null)
            {
                conn.Dispose();
            }
            return specialties;

        }
                 
    }
}
