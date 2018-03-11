﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Birder2.Services;
using Microsoft.AspNetCore.Authorization;
using Birder2.ViewModels;
using Microsoft.Extensions.Logging;
using Birder2.Extensions;

/*
 * Perhaps use IEnumerable - not IQueryable -  for the Birds list because it is likely to be repeatedly searched and filtered?
 * Total size is just 600 rows.
*/

/*
 * Tabular all bird data view
 * 
 */

namespace Birder2.Controllers
{
    [Authorize]
    public class BirdController : Controller
    {
        private readonly IBirdRepository _birdRepository;
        private readonly IFlickrService _flickrService;
        private readonly ILogger _logger;

        public BirdController(IBirdRepository birdRepository, IFlickrService flickrService
                                ,ILogger<BirdController> logger)
        {
            _birdRepository = birdRepository;
            _flickrService = flickrService;
            _logger = logger;
        }

        // GET: All Bird Species
        public async Task<IActionResult> Index(SortFilterBirdIndexOptions options)
        {
            _logger.LogInformation(LoggingEvents.ListItems, "Bird Index called");
            try
            {
                BirdIndexViewModel viewModel = new BirdIndexViewModel();
                viewModel.AllBirdsDropDownList = await _birdRepository.AllBirdsList();
                if (options.SelectedBirdId == 0)
                {
                    viewModel.BirdsList = await _birdRepository.AllBirdsList();
                }
                else
                {
                    viewModel.BirdsList = await _birdRepository.AllBirdsList(options.SelectedBirdId);
                }
                return View(viewModel);
            }
            catch (Exception ex)
            {
                // What to log?
                _logger.LogError(LoggingEvents.GetItemNotFound, ex, "Index({birdId}) error", options.SelectedBirdId);
                //_logger.LogError($"failed to return Birds details page: {ex}");//  <--
                return NotFound();
            }
        }

        // GET: Bird/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting bird {ID}", id);
            if (id == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "Details({ID}) - ID PARAMETER IS NULL", id);
                return NotFound();
            }

            var model = new BirdDetailViewModel();

            try
            {
                model.Bird = await _birdRepository.GetBirdDetails(id);
                model.BirdPhotos = _flickrService.GetFlickrPhotoCollection(model.Bird.Species);

                if (model.Bird == null)
                {
                    _logger.LogWarning(LoggingEvents.GetItemNotFound, "Details({ID}) BIRD NOT FOUND", id);
                    return NotFound();
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                // What to log?
                _logger.LogError(LoggingEvents.GetItemNotFound, ex, "Details({ID}) error", id);
                //_logger.LogError($"failed to return Birds details page: {ex}");//  <--
                return NotFound();
            }
        }
    }
}