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

    }
}
