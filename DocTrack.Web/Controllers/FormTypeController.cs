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
    public class FormTypeController : Controller
    {
        private readonly ILookupValueRepository _lookupValueRepository;
        private readonly IFormTypeRepository _formTypeRepository;

        private readonly int _lookupType = 1001;

        public FormTypeController(ILookupValueRepository lookupValueRepository, IFormTypeRepository formTypeRepository)
        {
            _lookupValueRepository = lookupValueRepository;
            _formTypeRepository = formTypeRepository;
        }

        // GET: FormType
        [Route("form-type")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("form-type/GetFormType/{formTypeId?}")]
        [HttpPost]
        public JsonResult GetFormType(int? formTypeId)
        {
            if (!formTypeId.HasValue)
            {
                FormTypeView formTypeView = new FormTypeView();

                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), null, formTypeView));
            }

            var data = _lookupValueRepository.GetById(formTypeId);

            if (data == null)
            {
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), GlobalNamespace.NotFound, null));
            }

            return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), null, data));
        }

        [Route("form-type/getFormTypeDatatable")]
        [HttpPost]
        public JsonResult GetFormTypeDataTable()
        {
            DatatablesRequest dataTableRequest = new DatatablesRequest
            {
                Search = Request.Form.Get("search[value]"),
                Length = int.Parse(Request.Form.Get("length")),
                Start = int.Parse(Request.Form.Get("start")),
                SortColumnName = Request.Form.Get("columns[" + int.Parse(Request.Form.Get("order[0][column]")) + "][name]"),
                SortDir = Request.Form.Get("order[0][dir]")
            };

            var draw = int.Parse(Request.Form.Get("draw"));
            var recordsTotal = _lookupValueRepository.GetWhere(a => a.LookupType == _lookupType).Count();
            var recordsFiltered = _formTypeRepository.GetTotalFormTypeFiltered(dataTableRequest.Search);
            var data = _formTypeRepository.GetFormTypeDatatable(dataTableRequest);

            return Json(new { draw, recordsFiltered, recordsTotal, data });
        }

        [Route("form-type/create")]
        [HttpPost]
        public JsonResult Create(FormTypeView formTypeView)
        {
            if (!ModelState.IsValid)
            {
                var errorList = GetModelStateErrors();
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), errorList, null));
            }

            var mapToDomainModel = formTypeView.Adapt<LookupValue>();
            mapToDomainModel.Description = formTypeView.FormName;
            mapToDomainModel.Abbreviation = formTypeView.FormName;
            mapToDomainModel.LookupType = _lookupType;

            bool validate = GenericValidation<LookupValue>.UniqueName(mapToDomainModel, "Description", mapToDomainModel.Description, lookupvalue => lookupvalue.Description == mapToDomainModel.Description);

            if (validate)
            {
                var data = _lookupValueRepository.Save(mapToDomainModel, Session["dht-username"].ToString());
                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), GlobalNamespace.Created, data));
            }

            return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), string.Format(GlobalNamespace.Unique, mapToDomainModel.Description), null));
        }

        [Route("form-type/update/{formTypeId?}")]
        [HttpPost]
        public JsonResult Update(int formTypeId, FormTypeView formTypeView)
        {
            if (!ModelState.IsValid)
            {
                var errorList = GetModelStateErrors();
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), errorList, null));
            }

            var mapToDomainModel = formTypeView.Adapt<LookupValue>();
            mapToDomainModel.Description = formTypeView.FormName;
            mapToDomainModel.Abbreviation = formTypeView.FormName;
            mapToDomainModel.LookupType = _lookupType;

            bool validate = GenericValidation<LookupValue>.UniqueName(mapToDomainModel, "Description", mapToDomainModel.Description, lookupvalue => lookupvalue.Description == mapToDomainModel.Description);

            if (validate)
            {
                var data = _lookupValueRepository.Update(formTypeId, mapToDomainModel, Session["dht-username"].ToString());
                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), GlobalNamespace.Updated, data));
            }

            return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), string.Format(GlobalNamespace.Unique, mapToDomainModel.Description), null));
        }

        [Route("form-type/delete/{formTypeId?}")]
        [HttpPost]
        public JsonResult Delete(int formTypeId)
        {
            var checkData = _lookupValueRepository.GetById(formTypeId);

            if (checkData == null)
            {
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), GlobalNamespace.NotFound, null));
            }

            _lookupValueRepository.Delete(formTypeId, Session["dht-username"].ToString());
            return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), GlobalNamespace.Deleted, true));
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
            _formTypeRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}