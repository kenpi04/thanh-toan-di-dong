﻿using AutoMapper;
//using PlanX.Admin.Models.Blogs;
//using PlanX.Admin.Models.Catalog;
using PlanX.Admin.Models.Cms;
using PlanX.Admin.Models.Common;
using PlanX.Admin.Models.Customers;
using PlanX.Admin.Models.Directory;
//using PlanX.Admin.Models.Discounts;
using PlanX.Admin.Models.ExternalAuthentication;
//using PlanX.Admin.Models.Forums;
using PlanX.Admin.Models.Localization;
using PlanX.Admin.Models.Logging;
using PlanX.Admin.Models.Messages;
using PlanX.Admin.Models.News;
//using PlanX.Admin.Models.Orders;
//using PlanX.Admin.Models.Payments;
using PlanX.Admin.Models.Plugins;
//using PlanX.Admin.Models.Polls;
using PlanX.Admin.Models.Settings;
//using PlanX.Admin.Models.Shipping;
using PlanX.Admin.Models.Stores;
//using PlanX.Admin.Models.Tax;
//using PlanX.Admin.Models.Templates;
using PlanX.Admin.Models.Topics;
using PlanX.Core.Domain.Banner;
//using PlanX.Core.Domain.Blogs;
//using PlanX.Core.Domain.Catalog;
using PlanX.Core.Domain.Common;
using PlanX.Core.Domain.Customers;
using PlanX.Core.Domain.Directory;
//using PlanX.Core.Domain.Discounts;
//using PlanX.Core.Domain.Forums;
using PlanX.Core.Domain.Localization;
using PlanX.Core.Domain.Logging;
using PlanX.Core.Domain.Media;
using PlanX.Core.Domain.Messages;
using PlanX.Core.Domain.News;
//using PlanX.Core.Domain.Orders;
//using PlanX.Core.Domain.Polls;
//using PlanX.Core.Domain.Shipping;
using PlanX.Core.Domain.Stores;
//using PlanX.Core.Domain.Tax;
using PlanX.Core.Domain.Topics;
//using PlanX.Core.Domain.Vendors;
using PlanX.Core.Plugins;
using PlanX.Services.Authentication.External;
using PlanX.Services.Cms;
//using PlanX.Services.Payments;
//using PlanX.Services.Shipping;
//using PlanX.Services.Tax;
using PlanX.Core.Domain.ClickBay;
using PlanX.Admin.Models.ClickBay;

namespace PlanX.Admin
{
    public static class MappingExtensions
    {
        #region ClickBay

        #region Booking
        public static BookingModel ToModel(this Booking entity)
        {
            return Mapper.Map<Booking, BookingModel>(entity);
        }

        public static Booking ToEntity(this BookingModel model)
        {
            return Mapper.Map<BookingModel, Booking>(model);
        }

        public static Booking ToEntity(this BookingModel model, Booking destination)
        {
            return Mapper.Map(model, destination);
        }
        #endregion
        #region BookingInfoFlight
        public static BookingInfoFlightModel ToModel(this BookingInfoFlight entity)
        {
            return Mapper.Map<BookingInfoFlight, BookingInfoFlightModel>(entity);
        }

        public static BookingInfoFlight ToEntity(this BookingInfoFlightModel model)
        {
            return Mapper.Map<BookingInfoFlightModel, BookingInfoFlight>(model);
        }

        public static BookingInfoFlight ToEntity(this BookingInfoFlightModel model, BookingInfoFlight destination)
        {
            return Mapper.Map(model, destination);
        }
        #endregion
        #region BookingPassenger
        public static BookingPassengerModel ToModel(this BookingPassenger entity)
        {
            return Mapper.Map<BookingPassenger, BookingPassengerModel>(entity);
        }

        public static BookingPassenger ToEntity(this BookingPassengerModel model)
        {
            return Mapper.Map<BookingPassengerModel, BookingPassenger>(model);
        }

        public static BookingPassenger ToEntity(this BookingPassengerModel model, BookingPassenger destination)
        {
            return Mapper.Map(model, destination);
        }
        #endregion
        #region BookingPriceDetail
        public static BookingPriceDetailModel ToModel(this BookingPriceDetail entity)
        {
            return Mapper.Map<BookingPriceDetail, BookingPriceDetailModel>(entity);
        }

        public static BookingPriceDetail ToEntity(this BookingPriceDetailModel model)
        {
            return Mapper.Map<BookingPriceDetailModel, BookingPriceDetail>(model);
        }

        public static BookingPriceDetail ToEntity(this BookingPriceDetailModel model, BookingPriceDetail destination)
        {
            return Mapper.Map(model, destination);
        }
        #endregion
        #region BookingBaggage
        public static BookingBaggageModel ToModel(this BookingBaggage entity)
        {
            return Mapper.Map<BookingBaggage, BookingBaggageModel>(entity);
        }

        public static BookingBaggage ToEntity(this BookingBaggageModel model)
        {
            return Mapper.Map<BookingBaggageModel, BookingBaggage>(model);
        }

        public static BookingBaggage ToEntity(this BookingBaggageModel model, BookingBaggage destination)
        {
            return Mapper.Map(model, destination);
        }
        #endregion
        #region BookingInfoCondition
        public static BookingInfoConditionModel ToModel(this BookingInfoCondition entity)
        {
            return Mapper.Map<BookingInfoCondition, BookingInfoConditionModel>(entity);
        }

        public static BookingInfoCondition ToEntity(this BookingInfoConditionModel model)
        {
            return Mapper.Map<BookingInfoConditionModel, BookingInfoCondition>(model);
        }

        public static BookingInfoCondition ToEntity(this BookingInfoConditionModel model, BookingInfoCondition destination)
        {
            return Mapper.Map(model, destination);
        }
        #endregion
        #region BookingTicketNote
        public static BookTicketNoteModel ToModel(this BookTicketNote entity)
        {
            return Mapper.Map<BookTicketNote, BookTicketNoteModel>(entity);
        }

        public static BookTicketNote ToEntity(this BookTicketNoteModel model)
        {
            return Mapper.Map<BookTicketNoteModel, BookTicketNote>(model);
        }

        public static BookTicketNote ToEntity(this BookTicketNoteModel model, BookTicketNote destination)
        {
            return Mapper.Map(model, destination);
        }
        #endregion

        #region Airlines Conditions
        public static AirlinesConditionModel ToModel(this AirlinesCondition entity)
        {
            return Mapper.Map<AirlinesCondition, AirlinesConditionModel>(entity);
        }

        public static AirlinesCondition ToEntity(this AirlinesConditionModel model)
        {
            return Mapper.Map<AirlinesConditionModel, AirlinesCondition>(model);
        }

        public static AirlinesCondition ToEntity(this AirlinesConditionModel model, AirlinesCondition destination)
        {
            return Mapper.Map(model, destination);
        }
        #endregion
        #region Airlines Baggage Conditions
        public static ArilinesBaggageConditionModel ToModel(this ArilinesBaggageCondition entity)
        {
            return Mapper.Map<ArilinesBaggageCondition, ArilinesBaggageConditionModel>(entity);
        }

        public static ArilinesBaggageCondition ToEntity(this ArilinesBaggageConditionModel model)
        {
            return Mapper.Map<ArilinesBaggageConditionModel, ArilinesBaggageCondition>(model);
        }

        public static ArilinesBaggageCondition ToEntity(this ArilinesBaggageConditionModel model, ArilinesBaggageCondition destination)
        {
            return Mapper.Map(model, destination);
        }
        #endregion

        #region Clickbay settings
        public static ClickBaySettingsModel ToModel(this ClickBaySettings entity)
        {
            return Mapper.Map<ClickBaySettings, ClickBaySettingsModel>(entity);
        }
        public static ClickBaySettings ToEntity(this ClickBaySettingsModel model)
        {
            return Mapper.Map<ClickBaySettingsModel, ClickBaySettings>(model);
        }
        public static ClickBaySettings ToEntity(this ClickBaySettingsModel model, ClickBaySettings destination)
        {
            return Mapper.Map(model, destination);
        }
        #endregion
        #endregion

        #region Category

        //public static CategoryModel ToModel(this Category entity)
        //{
        //    return Mapper.Map<Category, CategoryModel>(entity);
        //}

        //public static Category ToEntity(this CategoryModel model)
        //{
        //    return Mapper.Map<CategoryModel, Category>(model);
        //}

        //public static Category ToEntity(this CategoryModel model, Category destination)
        //{
        //    return Mapper.Map(model, destination);
        //}

        #endregion

        #region Manufacturer

        //public static ManufacturerModel ToModel(this Manufacturer entity)
        //{
        //    return Mapper.Map<Manufacturer, ManufacturerModel>(entity);
        //}

        //public static Manufacturer ToEntity(this ManufacturerModel model)
        //{
        //    return Mapper.Map<ManufacturerModel, Manufacturer>(model);
        //}

        //public static Manufacturer ToEntity(this ManufacturerModel model, Manufacturer destination)
        //{
        //    return Mapper.Map(model, destination);
        //}

        #endregion

        #region Products

        //public static ProductModel ToModel(this Product entity)
        //{
        //    return Mapper.Map<Product, ProductModel>(entity);
        //}

        //public static Product ToEntity(this ProductModel model)
        //{
        //    return Mapper.Map<ProductModel, Product>(model);
        //}

        //public static Product ToEntity(this ProductModel model, Product destination)
        //{
        //    return Mapper.Map(model, destination);
        //}

        #endregion

        #region Product attributes

        //public static ProductAttributeModel ToModel(this ProductAttribute entity)
        //{
        //    return Mapper.Map<ProductAttribute, ProductAttributeModel>(entity);
        //}

        //public static ProductAttribute ToEntity(this ProductAttributeModel model)
        //{
        //    return Mapper.Map<ProductAttributeModel, ProductAttribute>(model);
        //}

        //public static ProductAttribute ToEntity(this ProductAttributeModel model, ProductAttribute destination)
        //{
        //    return Mapper.Map(model, destination);
        //}

        #endregion

        #region Specification attributes

        //attributes
        //public static SpecificationAttributeModel ToModel(this SpecificationAttribute entity)
        //{
        //    return Mapper.Map<SpecificationAttribute, SpecificationAttributeModel>(entity);
        //}

        //public static SpecificationAttribute ToEntity(this SpecificationAttributeModel model)
        //{
        //    return Mapper.Map<SpecificationAttributeModel, SpecificationAttribute>(model);
        //}

        //public static SpecificationAttribute ToEntity(this SpecificationAttributeModel model, SpecificationAttribute destination)
        //{
        //    return Mapper.Map(model, destination);
        //}

        //attribute options
        //public static SpecificationAttributeOptionModel ToModel(this SpecificationAttributeOption entity)
        //{
        //    return Mapper.Map<SpecificationAttributeOption, SpecificationAttributeOptionModel>(entity);
        //}

        //public static SpecificationAttributeOption ToEntity(this SpecificationAttributeOptionModel model)
        //{
        //    return Mapper.Map<SpecificationAttributeOptionModel, SpecificationAttributeOption>(model);
        //}

        //public static SpecificationAttributeOption ToEntity(this SpecificationAttributeOptionModel model, SpecificationAttributeOption destination)
        //{
        //    return Mapper.Map(model, destination);
        //}
        #endregion

        #region Checkout attributes

        //attributes
        //public static CheckoutAttributeModel ToModel(this CheckoutAttribute entity)
        //{
        //    return Mapper.Map<CheckoutAttribute, CheckoutAttributeModel>(entity);
        //}

        //public static CheckoutAttribute ToEntity(this CheckoutAttributeModel model)
        //{
        //    return Mapper.Map<CheckoutAttributeModel, CheckoutAttribute>(model);
        //}

        //public static CheckoutAttribute ToEntity(this CheckoutAttributeModel model, CheckoutAttribute destination)
        //{
        //    return Mapper.Map(model, destination);
        //}

        #endregion

        #region Languages

        public static LanguageModel ToModel(this Language entity)
        {
            return Mapper.Map<Language, LanguageModel>(entity);
        }

        public static Language ToEntity(this LanguageModel model)
        {
            return Mapper.Map<LanguageModel, Language>(model);
        }

        public static Language ToEntity(this LanguageModel model, Language destination)
        {
            return Mapper.Map(model, destination);
        }
        
        #endregion

        #region Email account

        public static EmailAccountModel ToModel(this EmailAccount entity)
        {
            return Mapper.Map<EmailAccount, EmailAccountModel>(entity);
        }

        public static EmailAccount ToEntity(this EmailAccountModel model)
        {
            return Mapper.Map<EmailAccountModel, EmailAccount>(model);
        }

        public static EmailAccount ToEntity(this EmailAccountModel model, EmailAccount destination)
        {
            return Mapper.Map(model, destination);
        }

        #endregion

        #region Message templates

        public static MessageTemplateModel ToModel(this MessageTemplate entity)
        {
            return Mapper.Map<MessageTemplate, MessageTemplateModel>(entity);
        }

        public static MessageTemplate ToEntity(this MessageTemplateModel model)
        {
            return Mapper.Map<MessageTemplateModel, MessageTemplate>(model);
        }

        public static MessageTemplate ToEntity(this MessageTemplateModel model, MessageTemplate destination)
        {
            return Mapper.Map(model, destination);
        }

        #endregion

        #region Queued email

        public static QueuedEmailModel ToModel(this QueuedEmail entity)
        {
            return Mapper.Map<QueuedEmail, QueuedEmailModel>(entity);
        }

        public static QueuedEmail ToEntity(this QueuedEmailModel model)
        {
            return Mapper.Map<QueuedEmailModel, QueuedEmail>(model);
        }

        public static QueuedEmail ToEntity(this QueuedEmailModel model, QueuedEmail destination)
        {
            return Mapper.Map(model, destination);
        }

        #endregion

        #region Campaigns

        public static CampaignModel ToModel(this Campaign entity)
        {
            return Mapper.Map<Campaign, CampaignModel>(entity);
        }

        public static Campaign ToEntity(this CampaignModel model)
        {
            return Mapper.Map<CampaignModel, Campaign>(model);
        }

        public static Campaign ToEntity(this CampaignModel model, Campaign destination)
        {
            return Mapper.Map(model, destination);
        }

        #endregion

        #region Topics

        public static TopicModel ToModel(this Topic entity)
        {
            return Mapper.Map<Topic, TopicModel>(entity);
        }

        public static Topic ToEntity(this TopicModel model)
        {
            return Mapper.Map<TopicModel, Topic>(model);
        }

        public static Topic ToEntity(this TopicModel model, Topic destination)
        {
            return Mapper.Map(model, destination);
        }

        #endregion

        #region Log

        public static LogModel ToModel(this Log entity)
        {
            return Mapper.Map<Log, LogModel>(entity);
        }

        public static Log ToEntity(this LogModel model)
        {
            return Mapper.Map<LogModel, Log>(model);
        }

        public static Log ToEntity(this LogModel model, Log destination)
        {
            return Mapper.Map(model, destination);
        }

        public static ActivityLogTypeModel ToModel(this ActivityLogType entity)
        {
            return Mapper.Map<ActivityLogType, ActivityLogTypeModel>(entity);
        }

        public static ActivityLogModel ToModel(this ActivityLog entity)
        {
            return Mapper.Map<ActivityLog, ActivityLogModel>(entity);
        }

        #endregion
        
        #region Currencies

        //public static CurrencyModel ToModel(this Currency entity)
        //{
        //    return Mapper.Map<Currency, CurrencyModel>(entity);
        //}

        //public static Currency ToEntity(this CurrencyModel model)
        //{
        //    return Mapper.Map<CurrencyModel, Currency>(model);
        //}

        //public static Currency ToEntity(this CurrencyModel model, Currency destination)
        //{
        //    return Mapper.Map(model, destination);
        //}
        #endregion

        #region Measure weights

        //public static MeasureWeightModel ToModel(this MeasureWeight entity)
        //{
        //    return Mapper.Map<MeasureWeight, MeasureWeightModel>(entity);
        //}

        //public static MeasureWeight ToEntity(this MeasureWeightModel model)
        //{
        //    return Mapper.Map<MeasureWeightModel, MeasureWeight>(model);
        //}

        //public static MeasureWeight ToEntity(this MeasureWeightModel model, MeasureWeight destination)
        //{
        //    return Mapper.Map(model, destination);
        //}

        #endregion

        #region Measure dimension

        //public static MeasureDimensionModel ToModel(this MeasureDimension entity)
        //{
        //    return Mapper.Map<MeasureDimension, MeasureDimensionModel>(entity);
        //}

        //public static MeasureDimension ToEntity(this MeasureDimensionModel model)
        //{
        //    return Mapper.Map<MeasureDimensionModel, MeasureDimension>(model);
        //}

        //public static MeasureDimension ToEntity(this MeasureDimensionModel model, MeasureDimension destination)
        //{
        //    return Mapper.Map(model, destination);
        //}

        #endregion

        #region Tax providers

        //public static TaxProviderModel ToModel(this ITaxProvider entity)
        //{
        //    return Mapper.Map<ITaxProvider, TaxProviderModel>(entity);
        //}

        #endregion

        #region Tax categories

        //public static TaxCategoryModel ToModel(this TaxCategory entity)
        //{
        //    return Mapper.Map<TaxCategory, TaxCategoryModel>(entity);
        //}

        //public static TaxCategory ToEntity(this TaxCategoryModel model)
        //{
        //    return Mapper.Map<TaxCategoryModel, TaxCategory>(model);
        //}

        //public static TaxCategory ToEntity(this TaxCategoryModel model, TaxCategory destination)
        //{
        //    return Mapper.Map(model, destination);
        //}

        #endregion
        
        #region Shipping rate computation method

        //public static ShippingRateComputationMethodModel ToModel(this IShippingRateComputationMethod entity)
        //{
        //    return Mapper.Map<IShippingRateComputationMethod, ShippingRateComputationMethodModel>(entity);
        //}

        #endregion

        #region Shipping methods

        //public static ShippingMethodModel ToModel(this ShippingMethod entity)
        //{
        //    return Mapper.Map<ShippingMethod, ShippingMethodModel>(entity);
        //}

        //public static ShippingMethod ToEntity(this ShippingMethodModel model)
        //{
        //    return Mapper.Map<ShippingMethodModel, ShippingMethod>(model);
        //}

        //public static ShippingMethod ToEntity(this ShippingMethodModel model, ShippingMethod destination)
        //{
        //    return Mapper.Map(model, destination);
        //}

        #endregion

        #region Delivery dates

        //public static DeliveryDateModel ToModel(this DeliveryDate entity)
        //{
        //    return Mapper.Map<DeliveryDate, DeliveryDateModel>(entity);
        //}

        //public static DeliveryDate ToEntity(this DeliveryDateModel model)
        //{
        //    return Mapper.Map<DeliveryDateModel, DeliveryDate>(model);
        //}

        //public static DeliveryDate ToEntity(this DeliveryDateModel model, DeliveryDate destination)
        //{
        //    return Mapper.Map(model, destination);
        //}

        #endregion
        
        #region Payment methods

        //public static PaymentMethodModel ToModel(this IPaymentMethod entity)
        //{
        //    return Mapper.Map<IPaymentMethod, PaymentMethodModel>(entity);
        //}

        #endregion

        #region External authentication methods

        public static AuthenticationMethodModel ToModel(this IExternalAuthenticationMethod entity)
        {
            return Mapper.Map<IExternalAuthenticationMethod, AuthenticationMethodModel>(entity);
        }

        #endregion
        
        #region Widgets

        public static WidgetModel ToModel(this IWidgetPlugin entity)
        {
            return Mapper.Map<IWidgetPlugin, WidgetModel>(entity);
        }

        #endregion

        #region Address

        public static AddressModel ToModel(this Address entity)
        {
            return Mapper.Map<Address, AddressModel>(entity);
        }

        public static Address ToEntity(this AddressModel model)
        {
            return Mapper.Map<AddressModel, Address>(model);
        }

        public static Address ToEntity(this AddressModel model, Address destination)
        {
            return Mapper.Map(model, destination);
        }

        #endregion

        #region NewsLetter subscriptions

        public static NewsLetterSubscriptionModel ToModel(this NewsLetterSubscription entity)
        {
            return Mapper.Map<NewsLetterSubscription, NewsLetterSubscriptionModel>(entity);
        }

        public static NewsLetterSubscription ToEntity(this NewsLetterSubscriptionModel model)
        {
            return Mapper.Map<NewsLetterSubscriptionModel, NewsLetterSubscription>(model);
        }

        public static NewsLetterSubscription ToEntity(this NewsLetterSubscriptionModel model, NewsLetterSubscription destination)
        {
            return Mapper.Map(model, destination);
        }

        #endregion

        #region Discounts

        //public static DiscountModel ToModel(this Discount entity)
        //{
        //    return Mapper.Map<Discount, DiscountModel>(entity);
        //}

        //public static Discount ToEntity(this DiscountModel model)
        //{
        //    return Mapper.Map<DiscountModel, Discount>(model);
        //}

        //public static Discount ToEntity(this DiscountModel model, Discount destination)
        //{
        //    return Mapper.Map(model, destination);
        //}

        #endregion

        #region Forums

        //forum groups
        //public static ForumGroupModel ToModel(this ForumGroup entity)
        //{
        //    return Mapper.Map<ForumGroup, ForumGroupModel>(entity);
        //}

        //public static ForumGroup ToEntity(this ForumGroupModel model)
        //{
        //    return Mapper.Map<ForumGroupModel, ForumGroup>(model);
        //}

        //public static ForumGroup ToEntity(this ForumGroupModel model, ForumGroup destination)
        //{
        //    return Mapper.Map(model, destination);
        //}
        //forums
        //public static ForumModel ToModel(this Forum entity)
        //{
        //    return Mapper.Map<Forum, ForumModel>(entity);
        //}

        //public static Forum ToEntity(this ForumModel model)
        //{
        //    return Mapper.Map<ForumModel, Forum>(model);
        //}

        //public static Forum ToEntity(this ForumModel model, Forum destination)
        //{
        //    return Mapper.Map(model, destination);
        //}
        #endregion

        #region Blog

        //blog posts
        //public static BlogPostModel ToModel(this BlogPost entity)
        //{
        //    return Mapper.Map<BlogPost, BlogPostModel>(entity);
        //}

        //public static BlogPost ToEntity(this BlogPostModel model)
        //{
        //    return Mapper.Map<BlogPostModel, BlogPost>(model);
        //}

        //public static BlogPost ToEntity(this BlogPostModel model, BlogPost destination)
        //{
        //    return Mapper.Map(model, destination);
        //}

        #endregion

        #region News

        //news items
        public static NewsItemModel ToModel(this NewsItem entity)
        {
            return Mapper.Map<NewsItem, NewsItemModel>(entity);
        }

        public static NewsItem ToEntity(this NewsItemModel model)
        {
            return Mapper.Map<NewsItemModel, NewsItem>(model);
        }

        public static NewsItem ToEntity(this NewsItemModel model, NewsItem destination)
        {
            return Mapper.Map(model, destination);
        }

        #endregion

        #region Polls

        //news items
        //public static PollModel ToModel(this Poll entity)
        //{
        //    return Mapper.Map<Poll, PollModel>(entity);
        //}

        //public static Poll ToEntity(this PollModel model)
        //{
        //    return Mapper.Map<PollModel, Poll>(model);
        //}

        //public static Poll ToEntity(this PollModel model, Poll destination)
        //{
        //    return Mapper.Map(model, destination);
        //}

        #endregion

        #region Customers/users/customer roles
        //customer roles
        public static CustomerRoleModel ToModel(this CustomerRole entity)
        {
            return Mapper.Map<CustomerRole, CustomerRoleModel>(entity);
        }

        public static CustomerRole ToEntity(this CustomerRoleModel model)
        {
            return Mapper.Map<CustomerRoleModel, CustomerRole>(model);
        }

        public static CustomerRole ToEntity(this CustomerRoleModel model, CustomerRole destination)
        {
            return Mapper.Map(model, destination);
        }

        #endregion

        #region Gift Cards

        //public static GiftCardModel ToModel(this GiftCard entity)
        //{
        //    return Mapper.Map<GiftCard, GiftCardModel>(entity);
        //}

        //public static GiftCard ToEntity(this GiftCardModel model)
        //{
        //    return Mapper.Map<GiftCardModel, GiftCard>(model);
        //}

        //public static GiftCard ToEntity(this GiftCardModel model, GiftCard destination)
        //{
        //    return Mapper.Map(model, destination);
        //}

        #endregion

        #region Countries / states

        public static CountryModel ToModel(this Country entity)
        {
            return Mapper.Map<Country, CountryModel>(entity);
        }

        public static Country ToEntity(this CountryModel model)
        {
            return Mapper.Map<CountryModel, Country>(model);
        }

        public static Country ToEntity(this CountryModel model, Country destination)
        {
            return Mapper.Map(model, destination);
        }

        public static StateProvinceModel ToModel(this StateProvince entity)
        {
            return Mapper.Map<StateProvince, StateProvinceModel>(entity);
        }

        public static StateProvince ToEntity(this StateProvinceModel model)
        {
            return Mapper.Map<StateProvinceModel, StateProvince>(model);
        }

        public static StateProvince ToEntity(this StateProvinceModel model, StateProvince destination)
        {
            return Mapper.Map(model, destination);
        }


        #endregion

        #region Settings

        //public static TaxSettingsModel ToModel(this TaxSettings entity)
        //{
        //    return Mapper.Map<TaxSettings, TaxSettingsModel>(entity);
        //}
        //public static TaxSettings ToEntity(this TaxSettingsModel model)
        //{
        //    return Mapper.Map<TaxSettingsModel, TaxSettings>(model);
        //}
        //public static TaxSettings ToEntity(this TaxSettingsModel model, TaxSettings destination)
        //{
        //    return Mapper.Map(model, destination);
        //}


        //public static ShippingSettingsModel ToModel(this ShippingSettings entity)
        //{
        //    return Mapper.Map<ShippingSettings, ShippingSettingsModel>(entity);
        //}
        //public static ShippingSettings ToEntity(this ShippingSettingsModel model)
        //{
        //    return Mapper.Map<ShippingSettingsModel, ShippingSettings>(model);
        //}
        //public static ShippingSettings ToEntity(this ShippingSettingsModel model, ShippingSettings destination)
        //{
        //    return Mapper.Map(model, destination);
        //}


        //public static ForumSettingsModel ToModel(this ForumSettings entity)
        //{
        //    return Mapper.Map<ForumSettings, ForumSettingsModel>(entity);
        //}
        //public static ForumSettings ToEntity(this ForumSettingsModel model)
        //{
        //    return Mapper.Map<ForumSettingsModel, ForumSettings>(model);
        //}
        //public static ForumSettings ToEntity(this ForumSettingsModel model, ForumSettings destination)
        //{
        //    return Mapper.Map(model, destination);
        //}


        //public static BlogSettingsModel ToModel(this BlogSettings entity)
        //{
        //    return Mapper.Map<BlogSettings, BlogSettingsModel>(entity);
        //}
        //public static BlogSettings ToEntity(this BlogSettingsModel model)
        //{
        //    return Mapper.Map<BlogSettingsModel, BlogSettings>(model);
        //}
        //public static BlogSettings ToEntity(this BlogSettingsModel model, BlogSettings destination)
        //{
        //    return Mapper.Map(model, destination);
        //}


        //public static VendorSettingsModel ToModel(this VendorSettings entity)
        //{
        //    return Mapper.Map<VendorSettings, VendorSettingsModel>(entity);
        //}
        //public static VendorSettings ToEntity(this VendorSettingsModel model)
        //{
        //    return Mapper.Map<VendorSettingsModel, VendorSettings>(model);
        //}
        //public static VendorSettings ToEntity(this VendorSettingsModel model, VendorSettings destination)
        //{
        //    return Mapper.Map(model, destination);
        //}


        public static NewsSettingsModel ToModel(this NewsSettings entity)
        {
            return Mapper.Map<NewsSettings, NewsSettingsModel>(entity);
        }
        public static NewsSettings ToEntity(this NewsSettingsModel model)
        {
            return Mapper.Map<NewsSettingsModel, NewsSettings>(model);
        }
        public static NewsSettings ToEntity(this NewsSettingsModel model, NewsSettings destination)
        {
            return Mapper.Map(model, destination);
        }


        //public static CatalogSettingsModel ToModel(this CatalogSettings entity)
        //{
        //    return Mapper.Map<CatalogSettings, CatalogSettingsModel>(entity);
        //}
        //public static CatalogSettings ToEntity(this CatalogSettingsModel model)
        //{
        //    return Mapper.Map<CatalogSettingsModel, CatalogSettings>(model);
        //}
        //public static CatalogSettings ToEntity(this CatalogSettingsModel model, CatalogSettings destination)
        //{
        //    return Mapper.Map(model, destination);
        //}


        //public static RewardPointsSettingsModel ToModel(this RewardPointsSettings entity)
        //{
        //    return Mapper.Map<RewardPointsSettings, RewardPointsSettingsModel>(entity);
        //}
        //public static RewardPointsSettings ToEntity(this RewardPointsSettingsModel model)
        //{
        //    return Mapper.Map<RewardPointsSettingsModel, RewardPointsSettings>(model);
        //}
        //public static RewardPointsSettings ToEntity(this RewardPointsSettingsModel model, RewardPointsSettings destination)
        //{
        //    return Mapper.Map(model, destination);
        //}


        //public static OrderSettingsModel ToModel(this OrderSettings entity)
        //{
        //    return Mapper.Map<OrderSettings, OrderSettingsModel>(entity);
        //}
        //public static OrderSettings ToEntity(this OrderSettingsModel model)
        //{
        //    return Mapper.Map<OrderSettingsModel, OrderSettings>(model);
        //}
        //public static OrderSettings ToEntity(this OrderSettingsModel model, OrderSettings destination)
        //{
        //    return Mapper.Map(model, destination);
        //}


        //public static ShoppingCartSettingsModel ToModel(this ShoppingCartSettings entity)
        //{
        //    return Mapper.Map<ShoppingCartSettings, ShoppingCartSettingsModel>(entity);
        //}
        //public static ShoppingCartSettings ToEntity(this ShoppingCartSettingsModel model)
        //{
        //    return Mapper.Map<ShoppingCartSettingsModel, ShoppingCartSettings>(model);
        //}
        //public static ShoppingCartSettings ToEntity(this ShoppingCartSettingsModel model, ShoppingCartSettings destination)
        //{
        //    return Mapper.Map(model, destination);
        //}


        public static MediaSettingsModel ToModel(this MediaSettings entity)
        {
            return Mapper.Map<MediaSettings, MediaSettingsModel>(entity);
        }
        public static MediaSettings ToEntity(this MediaSettingsModel model)
        {
            return Mapper.Map<MediaSettingsModel, MediaSettings>(model);
        }
        public static MediaSettings ToEntity(this MediaSettingsModel model, MediaSettings destination)
        {
            return Mapper.Map(model, destination);
        }

        //customer/user settings
        public static CustomerUserSettingsModel.CustomerSettingsModel ToModel(this CustomerSettings entity)
        {
            return Mapper.Map<CustomerSettings, CustomerUserSettingsModel.CustomerSettingsModel>(entity);
        }
        public static CustomerSettings ToEntity(this CustomerUserSettingsModel.CustomerSettingsModel model)
        {
            return Mapper.Map<CustomerUserSettingsModel.CustomerSettingsModel, CustomerSettings>(model);
        }
        public static CustomerSettings ToEntity(this CustomerUserSettingsModel.CustomerSettingsModel model, CustomerSettings destination)
        {
            return Mapper.Map(model, destination);
        }
        public static CustomerUserSettingsModel.AddressSettingsModel ToModel(this AddressSettings entity)
        {
            return Mapper.Map<AddressSettings, CustomerUserSettingsModel.AddressSettingsModel>(entity);
        }
        public static AddressSettings ToEntity(this CustomerUserSettingsModel.AddressSettingsModel model)
        {
            return Mapper.Map<CustomerUserSettingsModel.AddressSettingsModel, AddressSettings>(model);
        }
        public static AddressSettings ToEntity(this CustomerUserSettingsModel.AddressSettingsModel model, AddressSettings destination)
        {
            return Mapper.Map(model, destination);
        }
        #endregion

        #region Plugins

        public static PluginModel ToModel(this PluginDescriptor entity)
        {
            return Mapper.Map<PluginDescriptor, PluginModel>(entity);
        }

        #endregion

        #region Stores

        public static StoreModel ToModel(this Store entity)
        {
            return Mapper.Map<Store, StoreModel>(entity);
        }

        public static Store ToEntity(this StoreModel model)
        {
            return Mapper.Map<StoreModel, Store>(model);
        }

        public static Store ToEntity(this StoreModel model, Store destination)
        {
            return Mapper.Map(model, destination);
        }

        #endregion

        #region Templates

        //public static CategoryTemplateModel ToModel(this CategoryTemplate entity)
        //{
        //    return Mapper.Map<CategoryTemplate, CategoryTemplateModel>(entity);
        //}

        //public static CategoryTemplate ToEntity(this CategoryTemplateModel model)
        //{
        //    return Mapper.Map<CategoryTemplateModel, CategoryTemplate>(model);
        //}

        //public static CategoryTemplate ToEntity(this CategoryTemplateModel model, CategoryTemplate destination)
        //{
        //    return Mapper.Map(model, destination);
        //}


        //public static ManufacturerTemplateModel ToModel(this ManufacturerTemplate entity)
        //{
        //    return Mapper.Map<ManufacturerTemplate, ManufacturerTemplateModel>(entity);
        //}

        //public static ManufacturerTemplate ToEntity(this ManufacturerTemplateModel model)
        //{
        //    return Mapper.Map<ManufacturerTemplateModel, ManufacturerTemplate>(model);
        //}

        //public static ManufacturerTemplate ToEntity(this ManufacturerTemplateModel model, ManufacturerTemplate destination)
        //{
        //    return Mapper.Map(model, destination);
        //}


        //public static ProductTemplateModel ToModel(this ProductTemplate entity)
        //{
        //    return Mapper.Map<ProductTemplate, ProductTemplateModel>(entity);
        //}

        //public static ProductTemplate ToEntity(this ProductTemplateModel model)
        //{
        //    return Mapper.Map<ProductTemplateModel, ProductTemplate>(model);
        //}

        //public static ProductTemplate ToEntity(this ProductTemplateModel model, ProductTemplate destination)
        //{
        //    return Mapper.Map(model, destination);
        //}
        #endregion

        #region newscate

        public static CategoryNewsModel ToModel(this CategoryNews entity)
        {
            return Mapper.Map<CategoryNews, CategoryNewsModel>(entity);
        }

        public static CategoryNews ToEntity(this CategoryNewsModel model)
        {
            return Mapper.Map<CategoryNewsModel, CategoryNews>(model);
        }

        public static CategoryNews ToEntity(this CategoryNewsModel model, CategoryNews destination)
        {
            return Mapper.Map(model, destination);
        }

        #endregion

        #region Banner

        public static BannerModel ToModel(this Banner entity)
        {
            return Mapper.Map<Banner, BannerModel>(entity);
        }

        public static Banner ToEntity(this BannerModel model)
        {
            return Mapper.Map<BannerModel, Banner>(model);
        }

        public static Banner ToEntity(this BannerModel model, Banner destination)
        {
            return Mapper.Map(model, destination);
        }

        #endregion
      

    }
}