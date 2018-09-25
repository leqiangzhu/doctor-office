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

        }
        public static List<Specialty> GetAll()
        {

        }
        public static Find Specialty(int id)
        {

        }
        public void AddDoctor(Doctor newDoctor)
        {

        }
        public List<Doctor> GetDoctor()
        {
            
        }
    }
}