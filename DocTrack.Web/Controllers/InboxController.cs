using System.Linq;
using System.Web.Mvc;
using static DocTrack.Common.GlobalEnum;

using DocTrack.Web.ViewModel;
using DocTrack.Data.Repository;
using DocTrack.Entity.Models;
using DocTrack.Web.Models;
using DocTrack.Common;
using Mapster;

namespace DocTrack.Web.Controllers
{
    public class InboxController : Controller
    {
        private readonly IDocumentHeaderRepository _documentHeaderRepository;
        private readonly IDocumentLineRepository _documentLineRepository;
        private readonly IDocumentTypeRepository _documentTypeRepository;
        private readonly IUtilityRepository _utilityRepository;
        private readonly ISequenceNumberRepository _sequenceNumberRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IVendorRepository _vendorRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILookupValueRepository _lookupValueRepository;
        private readonly IDocumentLinePurchaseOrderRepository _documentLinePurchaseOrderRepository;
        private readonly IDocumentLineCashAdvanceRepository _documentLineCashAdvanceRepository;
        private readonly IDocumentLineMemoRepository _documentLineMemoRepository;
        private readonly IDocumentLineSettlementRepository _documentLineSettlementRepository;

        private readonly int _documentNumberCode = 5;
        private readonly int _purchaseOrderType = 1002;
        private readonly int _cashAdvanceDocument = 1003;
        private object data;

        public InboxController(IDocumentHeaderRepository documentHeaderRepository, 
            IDocumentLineRepository documentLineRepository, 
            IDocumentTypeRepository documentTypeRepository,
            IUtilityRepository utilityRepository,
            ISequenceNumberRepository sequenceNumberRepository,
            ICompanyRepository companyRepository,
            IVendorRepository vendorRepository,
            ILookupValueRepository lookupValueRepository,
            IDocumentLinePurchaseOrderRepository documentLinePurchaseOrderRepository,
            IDocumentLineCashAdvanceRepository documentLineCashAdvanceRepository,
            IDepartmentRepository departmentRepository,
            IDocumentLineMemoRepository documentLineMemoRepository,
            IDocumentLineSettlementRepository documentLineSettlementRepository)
        {
            _documentHeaderRepository = documentHeaderRepository;
            _documentLineRepository = documentLineRepository;
            _documentTypeRepository = documentTypeRepository;
            _utilityRepository = utilityRepository;
            _sequenceNumberRepository = sequenceNumberRepository;
            _companyRepository = companyRepository;
            _vendorRepository = vendorRepository;
            _lookupValueRepository = lookupValueRepository;
            _documentLinePurchaseOrderRepository = documentLinePurchaseOrderRepository;
            _documentLineCashAdvanceRepository = documentLineCashAdvanceRepository;
            _departmentRepository = departmentRepository;
            _documentLineMemoRepository = documentLineMemoRepository;
            _documentLineSettlementRepository = documentLineSettlementRepository;
        }

        [Route("inbox/GetDocumentLineDatatable")]
        [HttpPost]
        public JsonResult GetDocumentLineDatatable()
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
            var recordsTotal = _documentHeaderRepository.GetWhere(a => a.IsHidden == false).Count();
            var recordsFiltered = _documentLineRepository.GetTotalDocumentLineFiltered(dataTableRequest.Search);
            var data = _documentLineRepository.GetDocumentLineDataTable(dataTableRequest);

            return Json(new { draw, recordsFiltered, recordsTotal, data });
        }

        [Route("inbox/get-document/{documentId?}")]
        [HttpPost]
        public JsonResult GetDocument(int? documentId)
        {
            if (!documentId.HasValue)
            {
                DocumentHeader documentHeader = new DocumentHeader();

                documentHeader.DocumentSequenceNumber =  _sequenceNumberRepository.GetSequenceNumber(_documentNumberCode);
                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), null, documentHeader));
            }

            data = _documentHeaderRepository.GetById(documentId);
            return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), null, data));
        }

        // GET: Inbox
        public ActionResult Index()
        {
            return View();
        }

        [Route("inbox/new-document")]
        public ActionResult NewDocument()
        {
            DocumentHeaderVM documentHeaderVM = new DocumentHeaderVM
            {
                DocumentType = new SelectList(_documentTypeRepository.GetWhere(document => document.IsHidden == false).ToList(), "DocumentTypeId", "DocumentTypeName")
            };

            return View(documentHeaderVM);
        }

        [Route("inbox/create-document")]
        [HttpPost]
        public ActionResult CreateDocument(DocumentHeaderVM documentHeaderVM)
        {
            var mapToModel = documentHeaderVM.DocumentHeader.Adapt<DocumentHeader>();
            mapToModel.DocumentStatus = DocumentStatus.Initiate.ToString();
            mapToModel.FlowStatus = FlowStatus.New.ToString();

            var getActionUtility = _utilityRepository.GetWhere(action => action.DocumentTypeId == mapToModel.DocumentTypeId).FirstOrDefault();

            _documentHeaderRepository.Save(mapToModel, Session["dht-username"].ToString());
            _sequenceNumberRepository.UpdateSequenceNumber(_documentNumberCode);

            return RedirectToAction(getActionUtility.RedirectTo, "Inbox", new { documentId = mapToModel.DocumentId });
        }

        [Route("purchase-order/GetDocument/{documentId?}")]
        [HttpPost]
        public JsonResult GetDocumentPurchaseOrder(int documentId)
        {
            var documentLine = _documentLinePurchaseOrderRepository.GetWhere(document => document.DocumentId == documentId).FirstOrDefault();
            var documentHeader = _documentHeaderRepository.GetById(documentId);

            if (documentLine == null)
            {
                DocumentLinePurchaseOrderView purchaseOrderView = new DocumentLinePurchaseOrderView
                {
                    DocumentLineId = 0,
                    DocumentId = documentHeader.DocumentId,
                    DocumentSequenceNumber = documentHeader.DocumentSequenceNumber,
                    DocumentTypeId = documentHeader.DocumentTypeId
                };

                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), null, purchaseOrderView));
            }
            
            return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), null, documentLine));
        }

        [Route("purchase-order/save/")]
        [HttpPost]
        public JsonResult SavePurchaseOrder(DocumentLinePurchaseOrderView purchaseOrderView)
        {
            var mapToDomainModel = purchaseOrderView.Adapt<DocumentLinePurchaseOrder>();
            var documentHeader = _documentHeaderRepository.GetById(mapToDomainModel.DocumentId);
            documentHeader.VendorId = mapToDomainModel.VendorId;
            documentHeader.Amount = mapToDomainModel.AmountToRecord;

            _documentHeaderRepository.Update(mapToDomainModel.DocumentId, documentHeader, Session["dht-username"].ToString());

            if (mapToDomainModel.DocumentLineId == 0)
            {
                data = _documentLinePurchaseOrderRepository.Save(mapToDomainModel, Session["dht-username"].ToString());
                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), GlobalNamespace.Saved, data));
            }

            data = _documentLinePurchaseOrderRepository.Update(mapToDomainModel.DocumentLineId, mapToDomainModel, Session["dht-username"].ToString());

            return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), GlobalNamespace.Updated, data));
        }

        [Route("purchase-order/submit/")]
        [HttpPost]
        public ActionResult SubmitPurchaseOrder(DocumentLinePurchaseOrderView purchaseOrderView)
        {
            var mapToDomainModel = purchaseOrderView.Adapt<DocumentLinePurchaseOrder>();
            var documentHeader = _documentHeaderRepository.GetById(mapToDomainModel.DocumentId);
            documentHeader.DocumentStatus = DocumentStatus.Progress.ToString();
            documentHeader.FlowStatus = FlowStatus.SendTo.ToString();

            data = _documentHeaderRepository.Update(mapToDomainModel.DocumentId, documentHeader, Session["dht-username"].ToString());
            return RedirectToAction("Index", "Inbox");
        }

        [Route("purhcase-order/{documentId?}")]
        public ActionResult PurchaseOrder(int documentId)
        {
            var documentHeader = _documentHeaderRepository.GetById(documentId);

            if (documentHeader.FlowStatus != FlowStatus.New.ToString())
            {
                return RedirectToAction("Index", "Inbox");
            }

            if (documentHeader == null)
            {
                return RedirectToAction("Index", "Inbox");
            }

            DocumentLinePurchaseOrderVM purchaseOrderVM = new DocumentLinePurchaseOrderVM
            {
                Companies = new SelectList(_companyRepository.GetWhere(company => company.IsHidden == false).ToList(), "CompanyId", "CompanyName"),
                Vendors = new SelectList(_vendorRepository.GetWhere(vendor => vendor.IsHidden == false).ToList(), "VendorId", "VendorName"),
                PurchaseOrderType = new SelectList(_lookupValueRepository.GetWhere(potype => potype.LookupType == _purchaseOrderType).ToList(), "LookupValueId", "Description")
            };

            ViewBag.DocumentUrl = Url.Action("GetDocumentPurchaseOrder", "Inbox");

            return View(purchaseOrderVM);
        }

        // Cash Advance

        [Route("cash-advance/GetDocument/{documentId?}")]
        [HttpPost]
        public JsonResult GetDocumentCashAdvance(int documentId)
        {
            var documentLine = _documentLineCashAdvanceRepository.GetWhere(document => document.DocumentId == documentId).FirstOrDefault();
            var documentHeader = _documentHeaderRepository.GetById(documentId);

            if (documentLine == null)
            {
                DocumentLineCashAdvanceView cashAdvanceView = new DocumentLineCashAdvanceView
                {
                    DocumentLineId = 0,
                    DocumentId = documentHeader.DocumentId,
                    DocumentSequenceNumber = documentHeader.DocumentSequenceNumber,
                    DocumentTypeId = documentHeader.DocumentTypeId
                };

                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), null, cashAdvanceView));
            }

            return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), null, documentLine));
        }

        [Route("cash-advance/save/")]
        [HttpPost]
        public JsonResult SaveCashAdvance(DocumentLineCashAdvanceView cashAdvanceView)
        {
            var mapToDomainModel = cashAdvanceView.Adapt<DocumentLineCashAdvance>();
            var documentHeader = _documentHeaderRepository.GetById(mapToDomainModel.DocumentId);
            documentHeader.VendorId = mapToDomainModel.VendorId;
            documentHeader.Amount = mapToDomainModel.Amount;

            _documentHeaderRepository.Update(mapToDomainModel.DocumentId, documentHeader, Session["dht-username"].ToString());

            if (mapToDomainModel.DocumentLineId == 0)
            {
                data = _documentLineCashAdvanceRepository.Save(mapToDomainModel, Session["dht-username"].ToString());
                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), GlobalNamespace.Saved, data));
            }

            data = _documentLineCashAdvanceRepository.Update(mapToDomainModel.DocumentLineId, mapToDomainModel, Session["dht-username"].ToString());

            return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), GlobalNamespace.Updated, data));
        }

        [Route("cash-advance/submit/")]
        [HttpPost]
        public ActionResult SubmitCashAdvance(DocumentLineCashAdvanceView cashAdvanceView)
        {
            var mapToDomainModel = cashAdvanceView.Adapt<DocumentLineCashAdvance>();
            var documentHeader = _documentHeaderRepository.GetById(mapToDomainModel.DocumentId);
            documentHeader.DocumentStatus = DocumentStatus.Progress.ToString();
            documentHeader.FlowStatus = FlowStatus.SendTo.ToString();

            data = _documentHeaderRepository.Update(mapToDomainModel.DocumentId, documentHeader, Session["dht-username"].ToString());
            return RedirectToAction("Index", "Inbox");
        }

        [Route("cash-advance/{documentId?}")]
        public ActionResult CashAdvance(int documentId)
        {
            var documentHeader = _documentHeaderRepository.GetById(documentId);

            if (documentHeader.FlowStatus != FlowStatus.New.ToString())
            {
                return RedirectToAction("Index", "Inbox");
            }

            if (documentHeader == null)
            {
                return RedirectToAction("Index", "Inbox");
            }

            DocumentLineCashAdvanceVM cashAdvanceVM = new DocumentLineCashAdvanceVM
            {
                Companies = new SelectList(_companyRepository.GetWhere(company => company.IsHidden == false).ToList(), "CompanyId", "CompanyName"),
                Vendors = new SelectList(_vendorRepository.GetWhere(vendor => vendor.IsHidden == false).ToList(), "VendorId", "VendorName"),
                Departments = new SelectList(_departmentRepository.GetWhere(department => department.IsHidden == false).ToList(), "DepartmentId", "DepartmentName"),
                DocumentStatus = new SelectList(_lookupValueRepository.GetWhere(cash => cash.LookupType == _cashAdvanceDocument).ToList(), "LookupValueId", "Description")
            };

            ViewBag.DocumentUrl = Url.Action("GetDocumentCashAdvance", "Inbox");

            return View(cashAdvanceVM);
        }

        // Memo

        [Route("memo/GetDocument/{documentId?}")]
        [HttpPost]
        public JsonResult GetDocumentMemo(int documentId)
        {
            var documentLine = _documentLineMemoRepository.GetWhere(document => document.DocumentId == documentId).FirstOrDefault();
            var documentHeader = _documentHeaderRepository.GetById(documentId);

            if (documentLine == null)
            {
                DocumentLineMemoView memoView = new DocumentLineMemoView
                {
                    DocumentLineId = 0,
                    DocumentId = documentHeader.DocumentId,
                    DocumentSequenceNumber = documentHeader.DocumentSequenceNumber,
                    DocumentTypeId = documentHeader.DocumentTypeId
                };

                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), null, memoView));
            }

            return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), null, documentLine));
        }

        [Route("memo/save/")]
        [HttpPost]
        public JsonResult SaveMemo(DocumentLineMemoView memoView)
        {
            var mapToDomainModel = memoView.Adapt<DocumentLineMemo>();
            var documentHeader = _documentHeaderRepository.GetById(mapToDomainModel.DocumentId);
            documentHeader.VendorId = mapToDomainModel.VendorId;
            documentHeader.Amount = mapToDomainModel.TotalAmount;

            _documentHeaderRepository.Update(mapToDomainModel.DocumentId, documentHeader, Session["dht-username"].ToString());

            if (mapToDomainModel.DocumentLineId == 0)
            {
                data = _documentLineMemoRepository.Save(mapToDomainModel, Session["dht-username"].ToString());
                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), GlobalNamespace.Saved, data));
            }

            data = _documentLineMemoRepository.Update(mapToDomainModel.DocumentLineId, mapToDomainModel, Session["dht-username"].ToString());

            return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), GlobalNamespace.Updated, data));
        }

        [Route("memo/submit/")]
        [HttpPost]
        public ActionResult SubmitMemo(DocumentLineMemoView memoView)
        {
            var mapToDomainModel = memoView.Adapt<DocumentLineMemo>();
            var documentHeader = _documentHeaderRepository.GetById(mapToDomainModel.DocumentId);
            documentHeader.DocumentStatus = DocumentStatus.Progress.ToString();
            documentHeader.FlowStatus = FlowStatus.SendTo.ToString();

            data = _documentHeaderRepository.Update(mapToDomainModel.DocumentId, documentHeader, Session["dht-username"].ToString());
            return RedirectToAction("Index", "Inbox");
        }

        [Route("memo/{documentId?}")]
        public ActionResult Memo(int documentId)
        {
            var documentHeader = _documentHeaderRepository.GetById(documentId);

            if (documentHeader.FlowStatus != FlowStatus.New.ToString())
            {
                return RedirectToAction("Index", "Inbox");
            }

            if (documentHeader == null)
            {
                return RedirectToAction("Index", "Inbox");
            }

            DocumentLineMemoVM memoVM = new DocumentLineMemoVM
            {
                Companies = new SelectList(_companyRepository.GetWhere(company => company.IsHidden == false).ToList(), "CompanyId", "CompanyName"),
                Vendors = new SelectList(_vendorRepository.GetWhere(vendor => vendor.IsHidden == false).ToList(), "VendorId", "VendorName"),
                Departments = new SelectList(_departmentRepository.GetWhere(department => department.IsHidden == false).ToList(), "DepartmentId", "DepartmentName"),
                DocumentStatus = new SelectList(_lookupValueRepository.GetWhere(cash => cash.LookupType == _cashAdvanceDocument).ToList(), "LookupValueId", "Description")
            };

            ViewBag.DocumentUrl = Url.Action("GetDocumentMemo", "Inbox");

            return View(memoVM);
        }

        // Settlement

        [Route("settlement/GetDocument/{documentId?}")]
        [HttpPost]
        public JsonResult GetDocumentSettlement(int documentId)
        {
            var documentLine = _documentLineSettlementRepository.GetWhere(document => document.DocumentId == documentId).FirstOrDefault();
            var documentHeader = _documentHeaderRepository.GetById(documentId);

            if (documentLine == null)
            {
                DocumentLineSettlementView settlementView = new DocumentLineSettlementView
                {
                    DocumentLineId = 0,
                    DocumentId = documentHeader.DocumentId,
                    DocumentSequenceNumber = documentHeader.DocumentSequenceNumber,
                    DocumentTypeId = documentHeader.DocumentTypeId
                };

                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), null, settlementView));
            }

            return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), null, documentLine));
        }

        [Route("settlement/save/")]
        [HttpPost]
        public JsonResult SaveSettlement(DocumentLineSettlementView settlementView)
        {
            var mapToDomainModel = settlementView.Adapt<DocumentLineSettlement>();
            var documentHeader = _documentHeaderRepository.GetById(mapToDomainModel.DocumentId);
            documentHeader.VendorId = mapToDomainModel.VendorId;
            documentHeader.Amount = mapToDomainModel.TotalAmount;

            _documentHeaderRepository.Update(mapToDomainModel.DocumentId, documentHeader, Session["dht-username"].ToString());

            if (mapToDomainModel.DocumentLineId == 0)
            {
                data = _documentLineSettlementRepository.Save(mapToDomainModel, Session["dht-username"].ToString());
                return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), GlobalNamespace.Saved, data));
            }

            data = _documentLineSettlementRepository.Update(mapToDomainModel.DocumentLineId, mapToDomainModel, Session["dht-username"].ToString());

            return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), GlobalNamespace.Updated, data));
        }

        [Route("settlement/submit/")]
        [HttpPost]
        public ActionResult SubmitSettlement(DocumentLineSettlementView settlementView)
        {
            var mapToDomainModel = settlementView.Adapt<DocumentLineSettlement>();
            var documentHeader = _documentHeaderRepository.GetById(mapToDomainModel.DocumentId);
            documentHeader.DocumentStatus = DocumentStatus.Progress.ToString();
            documentHeader.FlowStatus = FlowStatus.SendTo.ToString();

            data = _documentHeaderRepository.Update(mapToDomainModel.DocumentId, documentHeader, Session["dht-username"].ToString());
            return RedirectToAction("Index", "Inbox");
        }

        [Route("settlement/{documentId?}")]
        public ActionResult Settlement(int documentId)
        {
            var documentHeader = _documentHeaderRepository.GetById(documentId);
            var documentType = _documentTypeRepository.GetWhere(docs => docs.DocumentTypeId == documentHeader.DocumentTypeId).FirstOrDefault();

            if (documentHeader.FlowStatus != FlowStatus.New.ToString())
            {
                return RedirectToAction("Index", "Inbox");
            }

            if (documentHeader == null)
            {
                return RedirectToAction("Index", "Inbox");
            }

            DocumentLineSettlementVM settlementVM = new DocumentLineSettlementVM
            {
                Companies = new SelectList(_companyRepository.GetWhere(company => company.IsHidden == false).ToList(), "CompanyId", "CompanyName"),
                Vendors = new SelectList(_vendorRepository.GetWhere(vendor => vendor.IsHidden == false).ToList(), "VendorId", "VendorName"),
                Departments = new SelectList(_departmentRepository.GetWhere(department => department.IsHidden == false).ToList(), "DepartmentId", "DepartmentName"),
                DocumentStatus = new SelectList(_lookupValueRepository.GetWhere(cash => cash.LookupType == _cashAdvanceDocument).ToList(), "LookupValueId", "Description"),
                DocumentReference = new SelectList(_documentHeaderRepository.GetWhere(document => document.DocumentTypeId == documentType.DocumentReference).ToList(), "DocumentId", "DocumentSequenceNumber")
            };

            ViewBag.DocumentUrl = Url.Action("GetDocumentMemo", "Inbox");

            return View(settlementVM);
        }

        [Route("inbox/edit/{documentId?}")]
        public ActionResult EditDocument(int? documentId)
        {
            var document = _documentHeaderRepository.GetById(documentId);
            var utility = _utilityRepository.GetWhere(target => target.DocumentTypeId == document.DocumentTypeId).FirstOrDefault();

            return RedirectToAction(utility.RedirectTo, "Inbox");
        }

        [Route("inbox/delete/{documentId?}")]
        [HttpPost]
        public JsonResult DeleteDocument(int documentId)
        {
            var checkData = _documentHeaderRepository.GetById(documentId);

            if (checkData == null)
            {
                return Json(ResultStatus.Response(ResponseStatus.Failed.ToString(), GlobalNamespace.NotFound, null));
            }

            _documentHeaderRepository.Delete(documentId, Session["dht-username"].ToString());
            return Json(ResultStatus.Response(ResponseStatus.Success.ToString(), GlobalNamespace.Deleted, null));
        }

    }
}