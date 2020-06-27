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
    public class DocumentTypeController : Controller
    {
        private readonly IDocumentTypeRepository _documentTypeRepository;
        private readonly ILookupValueRepository _lookupValueRepository;

        private readonly int _lookupType = 1001;

        public DocumentTypeController(IDocumentTypeRepository documentTypeRepository, ILookupValueRepository lookupValueRepository)
        {
            _documentTypeRepository = documentTypeRepository;
            _lookupValueRepository = lookupValueRepository;
        }

        [Route("documents-type/getDocumentTypeDataTable")]
        [HttpPost]
        public JsonResult GetDocumentTypeDataTable()
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
            var recordsTotal = _documentTypeRepository.GetWhere(document => document.IsHidden == false).Count();
            var recordsFiltered = _documentTypeRepository.GetTotalDocumentTypeFiltered(dataTableRequest.Search);
            var data = _documentTypeRepository.GetDocumentTypeDataTable(dataTableRequest);

            return Json(new { draw, recordsFiltered, recordsTotal, data });
        }

        [Route("documents-type/GetDocumentType/{documentTypeId?}")]
        [HttpPost]
        public JsonResult GetDocumentType(int? documentTypeId)
        {
            if (!documentTypeId.HasValue)
            {
                DocumentTypeView documentTypeView = new DocumentTypeView();

                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), null, documentTypeView));
            }

            var data = _documentTypeRepository.GetById(documentTypeId);

            if (data == null)
            {
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), GlobalNamespace.NotFound, null));
            }

            return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), null, data));
        }

        // GET: DocumentType
        [Route("documents-type")]
        public ActionResult Index()
        {
            DocumentTypeVM documentTypeVM = new DocumentTypeVM
            {
                FormType = new SelectList(_lookupValueRepository.GetWhere(lookupvalue => lookupvalue.LookupType == _lookupType).ToList(), "LookupValueId", "Description"),
                DocumentReference = new SelectList(_documentTypeRepository.GetWhere(document => document.IsHidden == false).ToList(), "DocumentTypeId", "DocumentTypeName")
            };

            return View(documentTypeVM);
        }

        [Route("documents-type/create")]
        [HttpPost]
        public JsonResult Create(DocumentTypeVM documentTypeVM)
        {
            if (!ModelState.IsValid)
            {
                var errorList = GetModelStateErrors();
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), errorList, null));
            }

            var mapToDomainModel = documentTypeVM.DocumentType.Adapt<DocumentType>();
            var validate = GenericValidation<DocumentType>.NameMustUnique(mapToDomainModel, "DocumentTypeName", mapToDomainModel.DocumentTypeName, document => document.DocumentTypeName == mapToDomainModel.DocumentTypeName);

            if (validate)
            {
                var data = _documentTypeRepository.Save(mapToDomainModel, Session["dht-username"].ToString());
                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), GlobalNamespace.Created, data));
            }

            return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), string.Format(GlobalNamespace.Unique, documentTypeVM.DocumentType.DocumentTypeName), null));
        }

        [Route("documents-type/update/{documentTypeId?}")]
        [HttpPost]
        public JsonResult Update(int documentTypeId, DocumentTypeVM documentTypeVM)
        {
            if (!ModelState.IsValid)
            {
                var errorList = GetModelStateErrors();
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), errorList, null));
            }

            var mapToDomainModel = documentTypeVM.DocumentType.Adapt<DocumentType>();
            var validate = GenericValidation<DocumentType>.NameMustUnique(mapToDomainModel, "DocumentTypeName", mapToDomainModel.DocumentTypeName, document => document.DocumentTypeName == mapToDomainModel.DocumentTypeName);

            if (validate)
            {
                var data = _documentTypeRepository.Update(documentTypeId, mapToDomainModel, Session["dht-username"].ToString());
                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), GlobalNamespace.Updated, data));
            }

            return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), string.Format(GlobalNamespace.Unique, documentTypeVM.DocumentType.DocumentTypeName), null));
        }

        [Route("documents-type/delete/{documentTypeId?}")]
        [HttpPost]
        public JsonResult Delete(int documentTypeId)
        {
            var checkData = _documentTypeRepository.GetById(documentTypeId);

            if (checkData == null)
            {
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), GlobalNamespace.NotFound, null));
            }

            _documentTypeRepository.Delete(documentTypeId, Session["dht-username"].ToString());
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
            _documentTypeRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}