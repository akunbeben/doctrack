using DocTrack.Common;
using DocTrack.Data.Repository;
using DocTrack.Data.Validation;
using DocTrack.Entity.Models;
using DocTrack.Web.Models;
using DocTrack.Web.ViewModel;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static DocTrack.Common.GlobalEnum;

namespace DocTrack.Web.Controllers
{
    public class PersonInChargeController : Controller
    {
        private readonly IPersonInChargeRepository _personInChargeRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public PersonInChargeController(IPersonInChargeRepository personInChargeRepository, IDepartmentRepository departmentRepository)
        {
            _personInChargeRepository = personInChargeRepository;
            _departmentRepository = departmentRepository;
        }

        // GET: PersonInCharge
        [Route("person-in-charge")]
        public ActionResult Index()
        {
            PersonInChargeVM personInChargeVM = new PersonInChargeVM
            {
                Departments = new SelectList(_departmentRepository.GetWhere(department => department.IsHidden == false), "DepartmentId", "DepartmentName")
            };
            return View(personInChargeVM);
        }

        [Route("person-in-charge/getPersonInChargeDatatable")]
        [HttpPost]
        public JsonResult GetPersonInChargeDatatable()
        {
            DatatablesRequest datatablesRequest = new DatatablesRequest
            {
                Search = Request.Form.Get("search[value]"),
                Length = int.Parse(Request.Form.Get("length")),
                Start = int.Parse(Request.Form.Get("start")),
                SortColumnName = Request.Form.Get("columns[" + int.Parse(Request.Form.Get("order[0][column]")) + "][name]"),
                SortDir = Request.Form.Get("order[0][dir]")
            };

            var draw = int.Parse(Request.Form.Get("draw"));
            var recordsTotal = _personInChargeRepository.GetWhere(a => a.IsHidden == false).Count();
            var recordsFiltered = _personInChargeRepository.GetTotalPersonInChargeFiltered(datatablesRequest.Search);
            var data = _personInChargeRepository.GetPersonInChargeDataTable(datatablesRequest);

            return Json(new { draw, recordsFiltered, recordsTotal, data });
        }

        [Route("person-in-charge/GetPersonInCharge/{personInChargeId?}")]
        [HttpPost]
        public JsonResult GetPersonInCharge(int? personInChargeId)
        {
            if (!personInChargeId.HasValue)
            {
                PersonInChargeView personInCharge = new PersonInChargeView();

                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), null, personInCharge));
            }

            var data = _personInChargeRepository.GetById(personInChargeId);
            return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), null, data));
        }

        [Route("person-in-charge/create")]
        [HttpPost]
        public JsonResult Create(PersonInChargeVM personInChargeView)
        {
            if (!ModelState.IsValid)
            {
                var errorList = GetModelStateErrors();
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), errorList, null));
            }

            var mapToDomainModel = personInChargeView.PersonInCharge.Adapt<PersonInCharge>();
            var validate = GenericValidation<PersonInCharge>.NameMustUnique(mapToDomainModel, "PersonInChargeName", mapToDomainModel.PersonInChargeName, person => person.PersonInChargeName == mapToDomainModel.PersonInChargeName);

            if (validate)
            {
                var data = _personInChargeRepository.Save(mapToDomainModel, Session["dht-username"].ToString());
                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), GlobalNamespace.Created, data));
            }

            return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), string.Format(GlobalNamespace.Unique, personInChargeView.PersonInCharge.PersonInChargeName), null));
        }

        [Route("person-in-charge/update/{personInChargeId?}")]
        [HttpPost]
        public JsonResult Update(int personInChargeId, PersonInChargeVM personInChargeView)
        {
            if (!ModelState.IsValid)
            {
                var errorList = GetModelStateErrors();
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), errorList, null));
            }

            var mapToDomainModel = personInChargeView.PersonInCharge.Adapt<PersonInCharge>();
            var validate = GenericValidation<PersonInCharge>.NameMustUnique(mapToDomainModel, "PersonInChargeName", mapToDomainModel.PersonInChargeName, person => person.PersonInChargeName == mapToDomainModel.PersonInChargeName);

            if (validate)
            {
                var data = _personInChargeRepository.Update(personInChargeId, mapToDomainModel, Session["dht-username"].ToString());
                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), GlobalNamespace.Updated, data));
            }

            return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), string.Format(GlobalNamespace.Unique, personInChargeView.PersonInCharge.PersonInChargeName), null));
        }

        [Route("person-in-charge/delete/{personInChargeId?}")]
        [HttpPost]
        public JsonResult Delete(int personInChargeId)
        {
            var checkData = _personInChargeRepository.GetById(personInChargeId);

            if (checkData == null)
            {
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), GlobalNamespace.NotFound, null));
            }

            _personInChargeRepository.Delete(personInChargeId, Session["dht-username"].ToString());
            return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), GlobalNamespace.Deleted, null));
        }

        private object GetModelStateErrors()
        {
            var errorList = ModelState.Where(kvp => kvp.Value.Errors.Count > 0).ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return errorList;
        }

        protected override void Dispose(bool disposing)
        {
            _personInChargeRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}