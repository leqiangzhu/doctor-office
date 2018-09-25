   @"SELECT patients.* FROM doctors
        JOIN doctors_patients ON (doctors.id = doctors_patients.doctor_id)
        JOIN doctors_patients ON (doctors_patients.patient_id = patients.id)
        WHERE doctors.id = @DoctorsId;";