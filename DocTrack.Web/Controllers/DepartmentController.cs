using DocTrack.Common;
using DocTrack.Data.Repository;
using DocTrack.Data.Validation;
using DocTrack.Entity.Models;
using DocTrack.Web.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static DocTrack.Common.GlobalEnum;

namespace DocTrack.Web.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ISequenceNumberRepository _sequenceNumberRepository;

        private readonly int _departmentSequenceCode = 3;

        public DepartmentController(IDepartmentRepository departmentRepository, ISequenceNumberRepository sequenceNumberRepository)
        {
            _departmentRepository = departmentRepository;
            _sequenceNumberRepository = sequenceNumberRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Route("company/getDepartmentDatatable")]
        [HttpPost]
        public JsonResult GetDepartmentDataTable()
        {
            DatatablesRequest dataTableRequest = new DatatablesRequest
            {
                Search = Request.Form.Get("search[value]"),
                Start = int.Parse(Request.Form.Get("start")),
                Length = int.Parse(Request.Form.Get("length")),
                SortColumnName = Request.Form.Get("columns[" + int.Parse(Request.Form.Get("order[0][column]")) + "][name]"),
                SortDir = Request.Form.Get("order[0][dir]")
            };

            var draw = int.Parse(Request.Form.Get("draw").ToString());
            var recordsTotal = _departmentRepository.GetWhere(a => a.IsHidden == false).Count();
            var recordsFiltered = _departmentRepository.GetTotalDepartmentFiltered(dataTableRequest.Search);
            var data = _departmentRepository.GetDepartmentDataTable(dataTableRequest);

            return Json(new { draw, recordsFiltered, recordsTotal, data });
        }

        [Route("company/GetDepartment/{departmentId?}")]
        [HttpPost]
        public JsonResult GetDepartment(int? departmentId)
        {
            if (!departmentId.HasValue)
            {
                DepartmentView departmentView = new DepartmentView();

                departmentView.DepartmentNumber = _sequenceNumberRepository.GetSequenceNumber(_departmentSequenceCode);
                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), null, departmentView));
            }

            var data = _departmentRepository.GetById(departmentId);

            if (data == null)
            {
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), GlobalNamespace.NotFound, null));
            }

            return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), null, data));
        }

        [Route("department/create")]
        [HttpPost]
        public JsonResult Create(DepartmentView department)
        {
            if (!ModelState.IsValid)
            {
                var errorList = GetModelStateErrors();
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), errorList, null));
            }

            var mapToDomainModel = department.Adapt<Department>();
            var validate = GenericValidation<Department>.NameMustUnique(mapToDomainModel, "DepartmentName", mapToDomainModel.DepartmentName, departments => departments.DepartmentName == mapToDomainModel.DepartmentName);

            if (validate)
            {
                var data = _departmentRepository.Save(mapToDomainModel, Session["dht-username"].ToString());
                _sequenceNumberRepository.UpdateSequenceNumber(_departmentSequenceCode);
                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), GlobalNamespace.Created, data));
            }

            return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), GlobalNamespace.Unique, null));
        }

        [Route("department/update/{departmentId?}")]
        [HttpPost]
        public JsonResult Update(int departmentId, DepartmentView department)
        {
            if (!ModelState.IsValid)
            {
                var errorList = GetModelStateErrors();
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), errorList, null));
            }

            var mapToDomainModel = department.Adapt<Department>();
            var validate = GenericValidation<Department>.NameMustUnique(mapToDomainModel, "DepartmentName", mapToDomainModel.DepartmentName, departments => departments.DepartmentName == mapToDomainModel.DepartmentName);

            if (validate)
            {
                var data = _departmentRepository.Update(departmentId, mapToDomainModel, Session["dht-username"].ToString());
                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), GlobalNamespace.Created, data));
            }

            return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), GlobalNamespace.Unique, null));
        }

        [Route("department/delete/{departmentId?}")]
        [HttpPost]
        public JsonResult Delete(int departmentId)
        {
            var checkData = _departmentRepository.GetById(departmentId);

            if (checkData == null)
            {
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), GlobalNamespace.NotFound, null));
            }

            _departmentRepository.Delete(departmentId, Session["dht-username"].ToString());
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
            _departmentRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}