using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Integration.Mvc;
using PlanX.Core;
using PlanX.Core.Caching;
using PlanX.Core.Configuration;
using PlanX.Core.Data;
using PlanX.Core.Fakes;
using PlanX.Core.Infrastructure;
using PlanX.Core.Infrastructure.DependencyManagement;
using PlanX.Core.Plugins;
using PlanX.Data;
//using PlanX.Services.Affiliates;
using PlanX.Services.Authentication;
using PlanX.Services.Authentication.External;
//using PlanX.Services.Blogs;
//using PlanX.Services.Catalog;
using PlanX.Services.Cms;
using PlanX.Services.Common;
using PlanX.Services.Configuration;
using PlanX.Services.Customers;
using PlanX.Services.Directory;
//using PlanX.Services.Discounts;
using PlanX.Services.Events;
//using PlanX.Services.ExportImport;
//using PlanX.Services.Forums;
using PlanX.Services.Helpers;
using PlanX.Services.Installation;
using PlanX.Services.Localization;
using PlanX.Services.Logging;
using PlanX.Services.Media;
using PlanX.Services.Messages;
//using PlanX.Services.News;
//using PlanX.Services.Orders;
//using PlanX.Services.Payments;
//using PlanX.Services.Polls;
using PlanX.Services.Security;
using PlanX.Services.Seo;
//using PlanX.Services.Shipping;
using PlanX.Services.Stores;
using PlanX.Services.Tasks;
//using PlanX.Services.Tax;
using PlanX.Services.Topics;
//using PlanX.Services.Vendors;
using PlanX.Web.Framework.EmbeddedViews;
using PlanX.Web.Framework.Mvc.Routes;
using PlanX.Web.Framework.Themes;
using PlanX.Web.Framework.UI;
using PlanX.Web.Framework.UI.Editor;
using PlanX.Services.ClickBay;
namespace PlanX.Web.Framework
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //HTTP context and other related stuff
            builder.Register(c => 
                //register FakeHttpContext when HttpContext is not available
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                (new FakeHttpContext("~/") as HttpContextBase))
                .As<HttpContextBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerHttpRequest();

            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerHttpRequest();

            //controllers
            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());

            //data layer
            var dataSettingsManager = new DataSettingsManager();
            var dataProviderSettings = dataSettingsManager.LoadSettings();
            builder.Register(c => dataSettingsManager.LoadSettings()).As<DataSettings>();
            builder.Register(x => new EfDataProviderManager(x.Resolve<DataSettings>())).As<BaseDataProviderManager>().InstancePerDependency();


            builder.Register(x => x.Resolve<BaseDataProviderManager>().LoadDataProvider()).As<IDataProvider>().InstancePerDependency();

            if (dataProviderSettings != null && dataProviderSettings.IsValid())
            {
                var efDataProviderManager = new EfDataProviderManager(dataSettingsManager.LoadSettings());
                var dataProvider = efDataProviderManager.LoadDataProvider();
                dataProvider.InitConnectionFactory();

                builder.Register<IDbContext>(c => new NopObjectContext(dataProviderSettings.DataConnectionString)).InstancePerHttpRequest();
            }
            else
            {
                builder.Register<IDbContext>(c => new NopObjectContext(dataSettingsManager.LoadSettings().DataConnectionString)).InstancePerHttpRequest();
            }


            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerHttpRequest();
            
            //plugins
            builder.RegisterType<PluginFinder>().As<IPluginFinder>().InstancePerHttpRequest();

            //cache manager
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("nop_cache_static").SingleInstance();
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().Named<ICacheManager>("nop_cache_per_request").InstancePerHttpRequest();


            //work context
            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerHttpRequest();
            //store context
            builder.RegisterType<WebStoreContext>().As<IStoreContext>().InstancePerHttpRequest();

            //services
            //builder.RegisterType<BackInStockSubscriptionService>().As<IBackInStockSubscriptionService>().InstancePerHttpRequest();
            //builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerHttpRequest();
            //builder.RegisterType<CompareProductsService>().As<ICompareProductsService>().InstancePerHttpRequest();
            //builder.RegisterType<RecentlyViewedProductsService>().As<IRecentlyViewedProductsService>().InstancePerHttpRequest();
            //builder.RegisterType<ManufacturerService>().As<IManufacturerService>().InstancePerHttpRequest();
            //builder.RegisterType<PriceCalculationService>().As<IPriceCalculationService>().InstancePerHttpRequest();
            //builder.RegisterType<PriceFormatter>().As<IPriceFormatter>().InstancePerHttpRequest();
            //builder.RegisterType<ProductAttributeFormatter>().As<IProductAttributeFormatter>().InstancePerLifetimeScope();
            //builder.RegisterType<ProductAttributeParser>().As<IProductAttributeParser>().InstancePerHttpRequest();
            //builder.RegisterType<ProductAttributeService>().As<IProductAttributeService>().InstancePerHttpRequest();
            //builder.RegisterType<ProductService>().As<IProductService>().InstancePerHttpRequest();
            //builder.RegisterType<CopyProductService>().As<ICopyProductService>().InstancePerHttpRequest();
            //builder.RegisterType<SpecificationAttributeService>().As<ISpecificationAttributeService>().InstancePerHttpRequest();
            //builder.RegisterType<ProductTemplateService>().As<IProductTemplateService>().InstancePerHttpRequest();
            //builder.RegisterType<CategoryTemplateService>().As<ICategoryTemplateService>().InstancePerHttpRequest();
            //builder.RegisterType<ManufacturerTemplateService>().As<IManufacturerTemplateService>().InstancePerHttpRequest();
            //pass MemoryCacheManager as cacheManager (cache settings between requests)
            //builder.RegisterType<ProductTagService>().As<IProductTagService>()
            //    .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"))
            //    .InstancePerHttpRequest();

            //builder.RegisterType<AffiliateService>().As<IAffiliateService>().InstancePerHttpRequest();
            //builder.RegisterType<VendorService>().As<IVendorService>().InstancePerHttpRequest();

            builder.RegisterType<ClickbayService>().As<IClickBayService>().InstancePerHttpRequest();


            builder.RegisterType<AddressService>().As<IAddressService>().InstancePerHttpRequest();
            builder.RegisterType<SearchTermService>().As<ISearchTermService>().InstancePerHttpRequest();
            builder.RegisterType<GenericAttributeService>().As<IGenericAttributeService>().InstancePerHttpRequest();
            builder.RegisterType<FulltextService>().As<IFulltextService>().InstancePerHttpRequest();
            builder.RegisterType<MaintenanceService>().As<IMaintenanceService>().InstancePerHttpRequest();

            
            builder.RegisterType<CustomerService>().As<ICustomerService>().InstancePerHttpRequest();
            builder.RegisterType<CustomerRegistrationService>().As<ICustomerRegistrationService>().InstancePerHttpRequest();
            builder.RegisterType<CustomerReportService>().As<ICustomerReportService>().InstancePerHttpRequest();

            //pass MemoryCacheManager as cacheManager (cache settings between requests)
            builder.RegisterType<PermissionService>().As<IPermissionService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"))
                .InstancePerHttpRequest();
            //pass MemoryCacheManager as cacheManager (cache settings between requests)
            builder.RegisterType<AclService>().As<IAclService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"))
                .InstancePerHttpRequest();

            builder.RegisterType<GeoLookupService>().As<IGeoLookupService>().InstancePerHttpRequest();
            builder.RegisterType<CountryService>().As<ICountryService>().InstancePerHttpRequest();
            //builder.RegisterType<CurrencyService>().As<ICurrencyService>().InstancePerHttpRequest();
            //builder.RegisterType<MeasureService>().As<IMeasureService>().InstancePerHttpRequest();
            builder.RegisterType<StateProvinceService>().As<IStateProvinceService>().InstancePerHttpRequest();

            builder.RegisterType<StoreService>().As<IStoreService>().InstancePerHttpRequest();
            //pass MemoryCacheManager as cacheManager (cache settings between requests)
            builder.RegisterType<StoreMappingService>().As<IStoreMappingService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"))
                .InstancePerHttpRequest();

            //builder.RegisterType<DiscountService>().As<IDiscountService>().InstancePerHttpRequest();


            //pass MemoryCacheManager as cacheManager (cache settings between requests)
            builder.RegisterType<SettingService>().As<ISettingService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"))
                .InstancePerHttpRequest();
            builder.RegisterSource(new SettingsSource());

            //pass MemoryCacheManager as cacheManager (cache locales between requests)
            builder.RegisterType<LocalizationService>().As<ILocalizationService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"))
                .InstancePerHttpRequest();

            //pass MemoryCacheManager as cacheManager (cache locales between requests)
            builder.RegisterType<LocalizedEntityService>().As<ILocalizedEntityService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"))
                .InstancePerHttpRequest();
            builder.RegisterType<LanguageService>().As<ILanguageService>().InstancePerHttpRequest();

            builder.RegisterType<DownloadService>().As<IDownloadService>().InstancePerHttpRequest();
            builder.RegisterType<PictureService>().As<IPictureService>().InstancePerHttpRequest();

            builder.RegisterType<MessageTemplateService>().As<IMessageTemplateService>().InstancePerHttpRequest();
            builder.RegisterType<QueuedEmailService>().As<IQueuedEmailService>().InstancePerHttpRequest();
            builder.RegisterType<NewsLetterSubscriptionService>().As<INewsLetterSubscriptionService>().InstancePerHttpRequest();
            builder.RegisterType<CampaignService>().As<ICampaignService>().InstancePerHttpRequest();
            builder.RegisterType<EmailAccountService>().As<IEmailAccountService>().InstancePerHttpRequest();
            builder.RegisterType<WorkflowMessageService>().As<IWorkflowMessageService>().InstancePerHttpRequest();
            builder.RegisterType<MessageTokenProvider>().As<IMessageTokenProvider>().InstancePerHttpRequest();
            builder.RegisterType<Tokenizer>().As<ITokenizer>().InstancePerHttpRequest();
            builder.RegisterType<EmailSender>().As<IEmailSender>().InstancePerHttpRequest();

            //builder.RegisterType<CheckoutAttributeFormatter>().As<ICheckoutAttributeFormatter>().InstancePerHttpRequest();
            //builder.RegisterType<CheckoutAttributeParser>().As<ICheckoutAttributeParser>().InstancePerHttpRequest();
            //builder.RegisterType<CheckoutAttributeService>().As<ICheckoutAttributeService>().InstancePerHttpRequest();
            //builder.RegisterType<GiftCardService>().As<IGiftCardService>().InstancePerHttpRequest();
            //builder.RegisterType<OrderService>().As<IOrderService>().InstancePerHttpRequest();
            //builder.RegisterType<OrderReportService>().As<IOrderReportService>().InstancePerHttpRequest();
            //builder.RegisterType<OrderProcessingService>().As<IOrderProcessingService>().InstancePerHttpRequest();
            //builder.RegisterType<OrderTotalCalculationService>().As<IOrderTotalCalculationService>().InstancePerHttpRequest();
            //builder.RegisterType<ShoppingCartService>().As<IShoppingCartService>().InstancePerHttpRequest();

            //builder.RegisterType<PaymentService>().As<IPaymentService>().InstancePerHttpRequest();

            builder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerHttpRequest();
            builder.RegisterType<FormsAuthenticationService>().As<IAuthenticationService>().InstancePerHttpRequest();


            //pass MemoryCacheManager as cacheManager (cache settings between requests)
            builder.RegisterType<UrlRecordService>().As<IUrlRecordService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"))
                .InstancePerHttpRequest();

            //builder.RegisterType<ShipmentService>().As<IShipmentService>().InstancePerHttpRequest();
            //builder.RegisterType<ShippingService>().As<IShippingService>().InstancePerHttpRequest();

            //builder.RegisterType<TaxCategoryService>().As<ITaxCategoryService>().InstancePerHttpRequest();
            //builder.RegisterType<TaxService>().As<ITaxService>().InstancePerHttpRequest();
            //builder.RegisterType<TaxCategoryService>().As<ITaxCategoryService>().InstancePerHttpRequest();

            builder.RegisterType<DefaultLogger>().As<ILogger>().InstancePerHttpRequest();
            builder.RegisterType<CustomerActivityService>().As<ICustomerActivityService>().InstancePerHttpRequest();

            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["UseFastInstallationService"]) &&
                Convert.ToBoolean(ConfigurationManager.AppSettings["UseFastInstallationService"]))
            {
                builder.RegisterType<SqlFileInstallationService>().As<IInstallationService>().InstancePerHttpRequest();
            }
            else
            {
                builder.RegisterType<CodeFirstInstallationService>().As<IInstallationService>().InstancePerHttpRequest();
            }

            //builder.RegisterType<ForumService>().As<IForumService>().InstancePerHttpRequest();

            //builder.RegisterType<PollService>().As<IPollService>().InstancePerHttpRequest();
            //builder.RegisterType<BlogService>().As<IBlogService>().InstancePerHttpRequest();
            builder.RegisterType<WidgetService>().As<IWidgetService>().InstancePerHttpRequest();
            builder.RegisterType<TopicService>().As<ITopicService>().InstancePerHttpRequest();
            //builder.RegisterType<NewsService>().As<INewsService>().InstancePerHttpRequest();

            builder.RegisterType<DateTimeHelper>().As<IDateTimeHelper>().InstancePerHttpRequest();
            builder.RegisterType<SitemapGenerator>().As<ISitemapGenerator>().InstancePerHttpRequest();
            builder.RegisterType<PageHeadBuilder>().As<IPageHeadBuilder>().InstancePerHttpRequest();

            builder.RegisterType<ScheduleTaskService>().As<IScheduleTaskService>().InstancePerHttpRequest();

            builder.RegisterType<TelerikLocalizationServiceFactory>().As<Telerik.Web.Mvc.Infrastructure.ILocalizationServiceFactory>().InstancePerHttpRequest();

            //builder.RegisterType<ExportManager>().As<IExportManager>().InstancePerHttpRequest();
            //builder.RegisterType<ImportManager>().As<IImportManager>().InstancePerHttpRequest();
            builder.RegisterType<MobileDeviceHelper>().As<IMobileDeviceHelper>().InstancePerHttpRequest();
            builder.RegisterType<PdfService>().As<IPdfService>().InstancePerHttpRequest();
            builder.RegisterType<ThemeProvider>().As<IThemeProvider>().InstancePerHttpRequest();
            builder.RegisterType<ThemeContext>().As<IThemeContext>().InstancePerHttpRequest();


            builder.RegisterType<ExternalAuthorizer>().As<IExternalAuthorizer>().InstancePerHttpRequest();
            builder.RegisterType<OpenAuthenticationService>().As<IOpenAuthenticationService>().InstancePerHttpRequest();
           
                
            builder.RegisterType<EmbeddedViewResolver>().As<IEmbeddedViewResolver>().SingleInstance();
            builder.RegisterType<RoutePublisher>().As<IRoutePublisher>().SingleInstance();

            //HTML Editor services
            builder.RegisterType<NetAdvDirectoryService>().As<INetAdvDirectoryService>().InstancePerHttpRequest();
            builder.RegisterType<NetAdvImageService>().As<INetAdvImageService>().InstancePerHttpRequest();

            //Register event consumers
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
            {
                builder.RegisterType(consumer)
                    .As(consumer.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IConsumer<>)))
                    .InstancePerHttpRequest();
            }
            builder.RegisterType<EventPublisher>().As<IEventPublisher>().SingleInstance();
            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().SingleInstance();

        }

        public int Order
        {
            get { return 0; }
        }
    }


    public class SettingsSource : IRegistrationSource
    {
        static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
            "BuildRegistration",
            BindingFlags.Static | BindingFlags.NonPublic);

        public IEnumerable<IComponentRegistration> RegistrationsFor(
                Service service,
                Func<Service, IEnumerable<IComponentRegistration>> registrations)
        {
            var ts = service as TypedService;
            if (ts != null && typeof(ISettings).IsAssignableFrom(ts.ServiceType))
            {
                var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
                yield return (IComponentRegistration)buildMethod.Invoke(null, null);
            }
        }

        static IComponentRegistration BuildRegistration<TSettings>() where TSettings : ISettings, new()
        {
            return RegistrationBuilder
                .ForDelegate((c, p) =>
                {
                    var currentStoreId = c.Resolve<IStoreContext>().CurrentStore.Id;
                    //uncomment the code below if you want load settings per store only when you have two stores installed.
                    //var currentStoreId = c.Resolve<IStoreService>().GetAllStores().Count > 1
                    //    c.Resolve<IStoreContext>().CurrentStore.Id : 0;

                    //although it's better to connect to your database and execute the following SQL:
                    //DELETE FROM [Setting] WHERE [StoreId] > 0
                    return c.Resolve<ISettingService>().LoadSetting<TSettings>(currentStoreId);
                })
                .InstancePerHttpRequest()
                .CreateRegistration();
        }

        public bool IsAdapterForIndividualComponents { get { return false; } }
    }

}
