using DocTrack.Data.Repository;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace DocTrack
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<ISequenceNumberRepository, SequenceNumberRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<ILookupValueRepository, LookupValueRepository>();

            container.RegisterType<ICompanyRepository, CompanyRepository>();
            container.RegisterType<IDepartmentRepository, DepartmentRepository>();
            container.RegisterType<IVendorRepository, VendorRepository>();
            container.RegisterType<IPersonInChargeRepository, PersonInChargeRepository>();
            container.RegisterType<IDocumentTypeRepository, DocumentTypeRepository>();
            container.RegisterType<IFormTypeRepository, FormTypeRepository>();
            container.RegisterType<IDocumentHeaderRepository, DocumentHeaderRepository>();
            container.RegisterType<IDocumentLineRepository, DocumentLineRepository>();
            container.RegisterType<IDocumentLinePurchaseOrderRepository, DocumentLinePurchaseOrderRepository>();
            container.RegisterType<IDocumentLineCashAdvanceRepository, DocumentLineCashAdvanceRepository>();
            container.RegisterType<IDocumentLineMemoRepository, DocumentLineMemoRepository>();
            container.RegisterType<IDocumentLineSettlementRepository, DocumentLineSettlementRepository>();
            container.RegisterType<IUtilityRepository, UtilityRepository>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}