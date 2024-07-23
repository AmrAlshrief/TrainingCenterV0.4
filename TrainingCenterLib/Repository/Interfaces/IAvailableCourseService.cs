﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenterLib.Entities;

namespace TrainingCenterLib.Repository.Interfaces
{
    public interface IAvailableCourseService
    {
        Task<IEnumerable<AvailableCours>> GetAllAvailableCoursesAsync();
        Task<AvailableCours> GetAvailableCourseByIdAsync(int id);
        Task CreateAvailableCourseAsync(AvailableCours availableCours);
        Task UpdateAvailableCourseAsync(AvailableCours availableCours);
        Task DeleteAvailableCourseAsync(int id);
    }
}
