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
    public class VendorController : Controller
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ISequenceNumberRepository _sequenceNumberRepository;

        private readonly int _vendorSequenceCode = 4;

        public VendorController(
            IVendorRepository vendorRepository,
            ICompanyRepository companyRepository,
            ISequenceNumberRepository sequenceNumberRepository
            )
        {
            _vendorRepository = vendorRepository;
            _companyRepository = companyRepository;
            _sequenceNumberRepository = sequenceNumberRepository;
        }

        [Route("vendor/getVendorDatatable")]
        [HttpPost]
        public JsonResult GetVendorDataTable()
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
            var recordsTotal = _vendorRepository.GetWhere(a => a.IsHidden == false).Count();
            var recordsFiltered = _vendorRepository.GetTotalVendorFiltered(dataTableRequest.Search);
            var data = _vendorRepository.GetVendorDataTable(dataTableRequest);

            return Json(new { draw, recordsFiltered, recordsTotal, data });
        }

        // GET: Vendor
        public ActionResult Index()
        {
            VendorVM viewModel = new VendorVM
            {
                Companies = new SelectList(_companyRepository.GetWhere(c => c.IsHidden == false).ToList(), "CompanyId", "CompanyName")
            };

            return View(viewModel);
        }

        [Route("vendor/GetVendor/{vendorId?}")]
        [HttpPost]
        public JsonResult GetVendor(int? vendorId)
        {
            if (!vendorId.HasValue)
            {
                VendorView vendorView = new VendorView();

                vendorView.VendorAccount = _sequenceNumberRepository.GetSequenceNumber(_vendorSequenceCode);
                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), null, vendorView));
            }

            var data = _vendorRepository.GetById(vendorId);

            if (data == null)
            {
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), GlobalNamespace.NotFound, null));
            }

            return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), null, data));
        }

        [Route("vendor/create")]
        [HttpPost]
        public JsonResult Create(VendorVM vendor)
        {
            if (!ModelState.IsValid)
            {
                var errorList = GetModelStateErrors();
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), errorList, null));
            }

            var mapToDomainModel = vendor.Vendors.Adapt<Vendor>();
            var validate = GenericValidation<Vendor>.NameMustUnique(mapToDomainModel, "VendorName", mapToDomainModel.VendorName, vendors => vendors.VendorName == mapToDomainModel.VendorName);

            if (validate)
            {
                var data = _vendorRepository.Save(mapToDomainModel, Session["dht-username"].ToString());
                _sequenceNumberRepository.UpdateSequenceNumber(_vendorSequenceCode);
                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), GlobalNamespace.Created, data));
            }

            return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), string.Format(GlobalNamespace.Unique, vendor.Vendors.VendorName), null));
        }

        [Route("vendor/update/{vendorId?}")]
        [HttpPost]
        public JsonResult Update(int vendorId, VendorVM vendor)
        {
            if (!ModelState.IsValid)
            {
                var errorList = GetModelStateErrors();
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), errorList, null));
            }

            var mapToDomainModel = vendor.Vendors.Adapt<Vendor>();
            var validate = GenericValidation<Vendor>.NameMustUnique(mapToDomainModel, "VendorName", mapToDomainModel.VendorName, vendors => vendors.VendorName == mapToDomainModel.VendorName);

            if (validate)
            {
                var data = _vendorRepository.Update(vendorId, mapToDomainModel, Session["dht-username"].ToString());
                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), GlobalNamespace.Updated, data));
            }

            return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), string.Format(GlobalNamespace.Unique, vendor.Vendors.VendorName), null));
        }

        [Route("vendor/delete/{vendorId?}")]
        [HttpPost]
        public JsonResult Delete(int vendorId)
        {
            var checkData = _vendorRepository.GetById(vendorId);

            if (checkData == null)
            {
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), GlobalNamespace.NotFound, null));
            }

            _vendorRepository.Delete(vendorId, Session["dht-username"].ToString());
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
            _vendorRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}