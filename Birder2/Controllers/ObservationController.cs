﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Birder2.Models;
using Birder2.Services;
using Microsoft.AspNetCore.Authorization;
using Birder2.ViewModels;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Birder2.Controllers
{
    [Authorize]
    public class ObservationController : Controller
    {
        private readonly IApplicationUserAccessor _userAccessor;
        private readonly IObservationRepository _observationRepository;
        private readonly IMachineClockDateTime _systemClock;

        public ObservationController(IApplicationUserAccessor userAccessor,
                                     IObservationRepository observationRepository,
                                     IMachineClockDateTime systemClock)
        {
            _userAccessor = userAccessor;
            _observationRepository = observationRepository;
            _systemClock = systemClock;
        }

        // GET: Observation
        //  **** This is My Observations.  Need to overload to request my + mates' observations
        public async Task<IActionResult> Index()
        {
            var user = await _userAccessor.GetUser();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(await _observationRepository.MyObservationsList(user.Id)); // --> do not get user twice! await _userAccessor.GetUser()));
        }

        // GET: Observation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //ToDo: check for logged in user here?
            if (id == null)
            {
                return NotFound();
            }
            var observation = await _observationRepository.GetObservationDetails(id);

            if (observation == null)
            {
                return NotFound();
            }
            return View(observation);  //ToDo: if user == logged in user, then allow edit/delete etc.  Might need a viewmodel...
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateObservationViewModel()
            {
                Observation = new Observation() { ObservationDateTime = _systemClock.Now },
                MessageToClient = "",
                Birds = await _observationRepository.AllBirdsList(),
            };
            return View(model);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //public async Task<IActionResult> Create([Bind("ObservationId,ObservationDateTime,Location,Note,BirdId,LocationLatitude,LocationLongitude")] Observation observation)
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> Post([FromBody]CreateObservationViewModel viewModel)
        {
            var user = await _userAccessor.GetUser();
            if (user == null)
            {
                return Json(JsonConvert.SerializeObject(viewModel));

                //return Json(new { newLocation = "/Sales/Index/" });
            }

            if (viewModel.ObservedSpecies.Count == 0)
            {
                //ModelState.AddModelError("ObservredSpeciesCollection", "You must choose at least one species of bird.");
                //string errors = JsonConvert.SerializeObject(ModelState.Values
                //                .SelectMany(state => state.Errors)
                //                .Select(error => error.ErrorMessage));

                viewModel.IsModelStateValid = false;
                viewModel.MessageToClient = "You must choose at least one observed bird species.";

                //return Json(JsonConvert.SerializeObject(ModelState));
                return Json(JsonConvert.SerializeObject(viewModel));
            }

            //loop here to set the bird for earch observation?
            if (ModelState.IsValid)
            {

                //roll back in case any cannot be updated?
                foreach (ObservedSpeciesViewModel observedSpecies in viewModel.ObservedSpecies)
                {
                    try
                    {
                        Observation observationToAdd = new Observation();
                        observationToAdd.ObservationDateTime = viewModel.Observation.ObservationDateTime;
                        observationToAdd.LocationLatitude = viewModel.Observation.LocationLatitude;
                        observationToAdd.LocationLongitude = viewModel.Observation.LocationLongitude;
                        observationToAdd.NoteGeneral = viewModel.Observation.NoteGeneral;
                        observationToAdd.NoteHabitat = viewModel.Observation.NoteHabitat;
                        observationToAdd.NoteWeather = viewModel.Observation.NoteWeather;
                        observationToAdd.NoteAppearance = viewModel.Observation.NoteAppearance;
                        observationToAdd.NoteBehaviour = viewModel.Observation.NoteBehaviour;
                        observationToAdd.NoteVocalisation = viewModel.Observation.NoteVocalisation;

                        observationToAdd.ApplicationUser = user;

                        observationToAdd.CreationDate = _systemClock.Now;
                        observationToAdd.LastUpdateDate = _systemClock.Now;
                        observationToAdd.Bird = await _observationRepository.GetSelectedBird(observedSpecies.BirdId);
                        observationToAdd.Quantity = observedSpecies.Quantity;
                        await _observationRepository.AddObservation(observationToAdd);
                    }
                    catch
                    {
                        return Json(JsonConvert.SerializeObject(viewModel));
                        //return Json(new { newLocation = "/Sales/Index/" });
                    }
                }

                viewModel.IsModelStateValid = true;
                return Json(JsonConvert.SerializeObject(viewModel));
            }
            else
            {
                string errors = JsonConvert.SerializeObject(ModelState.Values
                                .SelectMany(state => state.Errors)
                               .Select(error => error.ErrorMessage));

                viewModel.IsModelStateValid = false;
                viewModel.MessageToClient = errors;
                return Json(JsonConvert.SerializeObject(viewModel));
                //return Json(JsonConvert.SerializeObject(ModelState));
            }
        }




        // GET: Observation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var model = new EditObservationViewModel
                {
                    Birds = await _observationRepository.AllBirdsList(),
                    Observation = await _observationRepository.GetObservationDetails(id)
                };

                if (model.Observation == null)
                {
                    return NotFound();
                }
                return View(model);
            }
            catch
            {
                //ToDo: Logging / return user to create view, like below?
                return NotFound("could not edit the observation");
            }
        }

        // POST: Observation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ObservationId,ObservationDateTime,Location,Note,BirdId,ApplicationUserId")] Observation observation)
        {
            // ToDo: Look into this update method.

            if (id != observation.ObservationId)
            {
                return NotFound();
            }

            var user = await _userAccessor.GetUser();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            observation.ApplicationUser = user;

            if (ModelState.IsValid)
            {
                try
                {
                    await _observationRepository.UpdateObservation(observation);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _observationRepository.ObservationExists(observation.ObservationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));  //return to details view?
            }
            var birds = await _observationRepository.AllBirdsList();
            ViewData["BirdId"] = new SelectList(birds, "BirdId", "EnglishName", observation.BirdId);
            return View(observation);
        }

        // GET: Observation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var observation = await _observationRepository.GetObservationDetails(id);

            if (observation == null)
            {
                return NotFound();
            }

            return View(observation);
        }

        // POST: Observation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _observationRepository.DeleteObservation(id);
            }
            catch
            {
                //logging
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ListLife()
        {
            var user = await _userAccessor.GetUser();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = await _observationRepository.GetLifeList(user.Id);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ListYear()
        {
            return View();
        }
    }
}


