﻿using Birder2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Birder2.Services
{
    public interface IObservationRepository
    {
        Task<IEnumerable<Bird>> AllBirdsList();
        Task<IEnumerable<Observation>> MyObservationsList(ApplicationUser user);
        Task<Observation> GetObservationDetails(int? id);
        Task<Observation> AddObservation(Observation observation);
        Task<Observation> UpdateObservation(Observation observation);
        Task<bool> ObservationExists(int id);
        Task<Observation> DeleteObservation(int id);
    }
}