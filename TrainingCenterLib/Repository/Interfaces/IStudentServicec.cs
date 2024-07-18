﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenterLib.Entities;

namespace TrainingCenterLib.Repository.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student> GetByIdAsync(int id);
        Task <Student> FindStudentByPhoneNumberAsync(string PhoneNumber);
        Task AddStudentAsync(Student student, int UserId);
        void AddStudent(Student student);
        Task UpdateStudentAsync(Student student, int UserId);
        Task SoftDeleteStudentAsync(int studentId, int UserId);
    }
}
