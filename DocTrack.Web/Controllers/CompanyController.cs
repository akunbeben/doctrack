using System.Linq;
using System.Web.Mvc;

using DocTrack.Common;
using DocTrack.Data.Repository;
using DocTrack.Data.Validation;
using DocTrack.Entity.Models;
using DocTrack.Web.Models;
using Mapster;
using static DocTrack.Common.GlobalEnum;

namespace DocTrack.Web.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ISequenceNumberRepository _sequenceNumberRepository;

        private readonly int _companySequenceCode = 2;

        public CompanyController(ICompanyRepository companyRepository, ISequenceNumberRepository sequenceNumberRepository)
        {
            _companyRepository = companyRepository;
            _sequenceNumberRepository = sequenceNumberRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Route("company/getCompanyDatatable")]
        [HttpPost]
        public JsonResult GetCompanyDataTable()
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
            var recordsTotal = _companyRepository.GetWhere(a => a.IsHidden == false).Count();
            var recordsFiltered = _companyRepository.GetTotalCompanyFiltered(dataTableRequest.Search);
            var data = _companyRepository.GetCompanyDataTable(dataTableRequest);

            return Json(new { draw, recordsFiltered, recordsTotal, data });
        }

        [Route("company/GetCompany/{companyId?}")]
        [HttpPost]
        public JsonResult GetCompany(int? companyId)
        {
            if (!companyId.HasValue)
            {
                CompanyView companyView = new CompanyView();

                companyView.CompanyNumber = _sequenceNumberRepository.GetSequenceNumber(_companySequenceCode);
                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), null, companyView));
            }

            var data = _companyRepository.GetById(companyId);

            if (data == null)
            {
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), GlobalNamespace.NotFound, null));
            }

            return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), null, data));
        }

        [Route("company/create")]
        [HttpPost]
        public JsonResult Create(CompanyView company)
        {
            if (!ModelState.IsValid)
            {
                var errorList = GetModelStateErrors();
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), errorList, null));
            }

            var mapToModel = company.Adapt<Company>();
            bool validate = GenericValidation<Company>.NameMustUnique(mapToModel, "CompanyName", mapToModel.CompanyName, companies => companies.CompanyName == mapToModel.CompanyName);

            if (validate)
            {
                _sequenceNumberRepository.UpdateSequenceNumber(_companySequenceCode);
                var data = this._companyRepository.Save(mapToModel, Session["dht-username"].ToString());
                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), GlobalNamespace.Created, data));
            }

            return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), string.Format(GlobalNamespace.Unique, company.CompanyName), null));
        }

        [Route("company/update/{companyId}")]
        [HttpPost]
        public JsonResult Update(int companyId, CompanyView company)
        {
            if (!ModelState.IsValid)
            {
                var errorList = GetModelStateErrors();
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), errorList, null));
            }

            var mapToModel = company.Adapt<Company>();
            var validate = GenericValidation<Company>.NameMustUnique(mapToModel, "CompanyName", mapToModel.CompanyName, companies => companies.CompanyName == mapToModel.CompanyName);

            if (validate)
            {
                var data = this._companyRepository.Update(companyId, mapToModel, Session["dht-username"].ToString());
                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), string.Format(GlobalNamespace.Updated, data.CompanyName), data));
            }

            return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), string.Format(GlobalNamespace.Unique, company.CompanyName), null));
        }

        [Route("company/delete/{companyId?}")]
        [HttpPost]
        public JsonResult Delete(int companyId)
        {
            var checkData = _companyRepository.GetById(companyId);

            if (checkData == null)
            {
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), GlobalNamespace.NotFound, null));
            }

            _companyRepository.Delete(companyId, Session["dht-username"].ToString());
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
            _companyRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}