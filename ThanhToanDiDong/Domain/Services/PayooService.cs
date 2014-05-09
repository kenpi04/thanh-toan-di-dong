using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Unilities;
using Domain.VUService;

namespace Domain.Services
{
    public class PayooService
    {
        VUService.UniGWSRequest request;
        VUService.UniGWSSoapClient _vuiExe;
        private CardMobileService _cardMobileService;
        private ProviderService _providerService;
        private ServicesService _service;
        private OrderService _OrderService;

        public PayooService()
        {
            _providerService = new ProviderService();
            _cardMobileService = new CardMobileService();
            _service = new ServicesService();
            _vuiExe = new VUService.UniGWSSoapClient();
            _OrderService = new OrderService();
            request = new UniGWSRequest();

        }

        public bool UpdateListFrtCardMobileFromPayoo(int categoryCardMobileId)
        {
            // Create new instance.


            // Set UniGWS Request



            request.ClientId = CommonSettings.ClientId;
            string clientPassword = CommonSettings.ClientPassword;
            string privateKeyUrl = CommonSettings.PrivateKeyUrl;
            string passwordForPrivateKey = CommonSettings.PasswordForPrivateKey;
            string publicKey = CommonSettings.PublicKeyUrl;
            // End


            System.Net.ServicePointManager.CertificatePolicy = new CertPolicy(); // Ho tro cho viec goi webservice theo giao thuc https.

            string data = string.Empty;

            try
            {
                var cateCardMobile = _cardMobileService.GetAll().FirstOrDefault();
                if (cateCardMobile == null)
                    return false;
                // Demo for QueryBillEx
                string strRequestTime = DateTime.Now.ToString("dd/MM/yyyy HHmmss");
                request.RequestTime = strRequestTime;
                // Checksum = MD5(RequestTime + ClientPassword)
                request.Checksum = MD5Encrypt.MD5Hash(strRequestTime + clientPassword);
                // End



                request.Operation = "MMS_GetCardProviderList";// Ten cua API giong nhu trong tai lieu API ma Payoo gui. Tuy vao chuc nang ma co Ten khac nhau.

                GetServicesRequest objQueryGetCardProviderList = new GetServicesRequest()
                {
                    AgentId = CommonSettings.AgentId,
                    UserId = CommonSettings.UserId
                };

                data = XmlSerializeLib.XmlSerialize(objQueryGetCardProviderList);

                request.Signature = SignData(data, privateKeyUrl, passwordForPrivateKey);

                request.RequestData = data;

                //bool isCorrect = Verify(data, request.Signature, publicKey);

                string demo = XmlSerializeLib.XmlSerialize(request);

                // Call to VU Service


                VUService.UniGWSResponse objResponse = _vuiExe.Execute2(request);

                // End

                // Verify signature.

                bool isCorrect = Verify(objResponse.ResponseData, objResponse.Signature, publicKey);
                if (!isCorrect)
                {
                    return false;
                }
                else
                {
                    // Deserialize ResponseData in UniGWSResponse object for check ReturnCode

                    GetCardProviderListResult objQueryGetCardProviderListResult = XmlSerializeLib.Deserialize(objResponse.ResponseData, typeof(GetCardProviderListResult))
                        as GetCardProviderListResult;

                    if (objQueryGetCardProviderListResult.ReturnCode == 0) // Sucessfully
                    {
                        var objQueryGetCardProvider = objQueryGetCardProviderListResult.ProviderList.FirstOrDefault(p => p.ProviderCode == cateCardMobile.Name);
                        var ListCardValue = objQueryGetCardProvider.CardValues.Select(p => p.CardValue);

                        var listCardMobile = _cardMobileService.GetAll(x => x.CategoryCardMobileId == categoryCardMobileId).ToList();

                        foreach (var cardmobile in listCardMobile)
                        {
                            if (!ListCardValue.Contains(cardmobile.UnitPrice))
                                _cardMobileService.Delete(cardmobile);
                        }

                        listCardMobile = _cardMobileService.GetAll(x => x.CategoryCardMobileId == categoryCardMobileId).ToList();
                        foreach (var cardvalue in ListCardValue)
                        {
                            var cardMobie = listCardMobile.FirstOrDefault(p => p.UnitPrice == cardvalue);
                            if (cardMobie == null)
                            {
                                cardMobie = new CardMobile()
                                {
                                    Name = cateCardMobile.Name + "_" + cardvalue,
                                    CategoryCardMobileId = cateCardMobile.Id,
                                    UnitPrice = cardvalue,
                                    UnitSellingPrice = cardvalue

                                };
                                _cardMobileService.InsertOrUpdate(cardMobie);
                            }
                        }



                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdatePriceCardMobileFromPaycode(string providerId, int cardMobileId)
        {
            // Create new instance.
            UniGWSSoapClient uniGW = new UniGWSSoapClient();

            // Set UniGWS Request

            UniGWSRequest request = new UniGWSRequest();

            request.ClientId = CommonSettings.ClientId;
            string clientPassword = CommonSettings.ClientPassword;
            string privateKeyUrl = CommonSettings.PrivateKeyUrl;
            string passwordForPrivateKey = CommonSettings.PasswordForPrivateKey;
            string publicKey = CommonSettings.PublicKeyUrl;
            // End


            System.Net.ServicePointManager.CertificatePolicy = new CertPolicy(); // Ho tro cho viec goi webservice theo giao thuc https.

            string data = string.Empty;

            try
            {
                var cardMobile = _cardMobileService.GetById(cardMobileId);
                if (cardMobile == null)
                    return false;
                // Demo for QueryBillEx
                string strRequestTime = DateTime.Now.ToString("dd/MM/yyyy HHmmss");
                request.RequestTime = strRequestTime;
                // Checksum = MD5(RequestTime + ClientPassword)
                request.Checksum = MD5Encrypt.MD5Hash(strRequestTime + clientPassword);
                // End



                request.Operation = "MMS_PaycodeInquiryBE";// Ten cua API giong nhu trong tai lieu API ma Payoo gui. Tuy vao chuc nang ma co Ten khac nhau.

                PaycodeInquiryBERequest objpaycodeInquiryBERequest = new PaycodeInquiryBERequest()
                {
                    AgentId = CommonSettings.AgentId,
                    CardValue = cardMobile.UnitPrice,
                    ProviderId = providerId,
                    Quantity = 1,
                    UserId = CommonSettings.UserId
                };

                data = XmlSerializeLib.XmlSerialize(objpaycodeInquiryBERequest);

                request.Signature = SignData(data, privateKeyUrl, passwordForPrivateKey);

                request.RequestData = data;

                //bool isCorrect = Verify(data, request.Signature, publicKey);

                string demo = XmlSerializeLib.XmlSerialize(request);

                // Call to VU Service


                UniGWSResponse objResponse = uniGW.Execute2(request);

                // End

                // Verify signature.

                bool isCorrect = Verify(objResponse.ResponseData, objResponse.Signature, publicKey);
                if (!isCorrect)
                {
                    return false;
                }
                else
                {
                    // Deserialize ResponseData in UniGWSResponse object for check ReturnCode

                    PaycodeInquiryBEResult objPaycodeInquiryBEResult = XmlSerializeLib.Deserialize(objResponse.ResponseData, typeof(PaycodeInquiryBEResult))
                        as PaycodeInquiryBEResult;

                    if (objPaycodeInquiryBEResult.ReturnCode == 0) // Sucessfully
                    {
                        cardMobile.UnitSellingPrice = objPaycodeInquiryBEResult.ReferPrice;
                        //  cardMobile.PurchasingPrice = objPaycodeInquiryBEResult.PurchasingPrice;
                        cardMobile.Name = providerId + "_" + cardMobile.UnitPrice;
                        _cardMobileService.InsertOrUpdate(cardMobile);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #region Service & Provider
        //public bool UpdateFrtPayooServiceListFromPayoo()
        //{
        //    // Create new instance.
        //    UniGWSSoapClient uniGW = new UniGWSSoapClient();

        //    // Set UniGWS Request

        //    UniGWSRequest request = new UniGWSRequest();

        //    request.ClientId = CommonSettings.ClientId;
        //    string clientPassword = CommonSettings.ClientPassword;
        //    string privateKeyUrl = CommonSettings.PrivateKeyUrl;
        //    string passwordForPrivateKey = CommonSettings.PasswordForPrivateKey;
        //    string publicKey = CommonSettings.PublicKeyUrl;
        //    // End


        //    System.Net.ServicePointManager.CertificatePolicy = new CertPolicy(); // Ho tro cho viec goi webservice theo giao thuc https.

        //    string data = string.Empty;

        //    try
        //    {
        //        string strRequestTime = DateTime.Now.ToString("dd/MM/yyyy HHmmss");
        //        request.RequestTime = strRequestTime;
        //        // Checksum = MD5(RequestTime + ClientPassword)
        //        request.Checksum = GetMD5Hash(strRequestTime + clientPassword);
        //        // End
        //        request.Operation = CommonSettings.MMS_GetServices;

        //        GetServicesRequest objGetServicesRequest = new GetServicesRequest();

        //        data = XmlSerializeLib.XmlSerialize(objGetServicesRequest);

        //        request.Signature = SignData(data, privateKeyUrl, passwordForPrivateKey);

        //        request.RequestData = data;

        //        UniGWSResponse objResponse = uniGW.Execute2(request);

        //        // Verify signature.

        //        bool isCorrect = Verify(objResponse.ResponseData, objResponse.Signature, publicKey);
        //        if (!isCorrect)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            // Deserialize ResponseData in UniGWSResponse object for check ReturnCode

        //            GetServicesResult objGetServicesResult = XmlSerializeLib.Deserialize(objResponse.ResponseData, typeof(GetServicesResult))
        //                as GetServicesResult;

        //            if (objGetServicesResult.ReturnCode == 0) // Sucessfully
        //            {
        //                if (objGetServicesResult.Services != null)
        //                {
        //                    var listdbServiceIds = GetAllListFrtPayooService().Select(p => p.ServiceId).ToList();
        //                    var listPayooServiceIds = objGetServicesResult.Services.Select(p => p.ServiceId);
        //                    listdbServiceIds.ForEach(id =>
        //                    {
        //                        if (!listPayooServiceIds.Contains(id))
        //                        {
        //                            var dbService = GetFrtPayooServiceByServiceId(id);
        //                            if (dbService != null)
        //                                DeleteFrtPayooService(dbService);
        //                        }
        //                    });
        //                    objGetServicesResult.Services.ToList().ForEach(payooservice =>
        //                    {
        //                        var frtpayooservice = GetFrtPayooServiceByServiceId(payooservice.ServiceId);
        //                        if (frtpayooservice == null)
        //                        {
        //                            frtpayooservice = new FrtPayooService()
        //                            {
        //                                ServiceId = payooservice.ServiceId,
        //                                ServiceName = payooservice.ServiceName,
        //                                MatchProviderCount = payooservice.MatchProviderCount,
        //                                CreatedOn = DateTime.Now,
        //                                LastUpdatedDateUtc = DateTime.Now
        //                            };

        //                            InsertFrtPayooService(frtpayooservice);
        //                            if (payooservice.Issuers != null)
        //                            {
        //                                payooservice.Issuers.ToList().ForEach(payooprovider =>
        //                                {
        //                                    var frtpayooProvider = new FrtPayooProvider()
        //                                    {
        //                                        ServiceId = frtpayooservice.ServiceId,
        //                                        ProviderName = payooprovider.IssuerName,
        //                                        ProviderId = payooprovider.IssuerName,
        //                                        IsOnline = payooprovider.IsOnline,
        //                                        CreatedOn = DateTime.Now,
        //                                        LastUpdatedDateUtc = DateTime.Now,
        //                                        ServiceEntityId = frtpayooservice.Id
        //                                    };

        //                                    frtpayooservice.PayooProviders.Add(frtpayooProvider);

        //                                });

        //                                UpdateFrtPayooService(frtpayooservice);
        //                            }

        //                        }
        //                        else
        //                        {
        //                            if (payooservice.Issuers != null)
        //                            {
        //                                payooservice.Issuers.ToList().ForEach(payooprovider =>
        //                                {
        //                                    var frtpayooProvider = GetFrtPayooProviderByProviderId(payooprovider.IssuerId);
        //                                    if (frtpayooProvider == null)
        //                                    {
        //                                        frtpayooProvider = new FrtPayooProvider()
        //                                        {
        //                                            ServiceId = frtpayooservice.ServiceId,
        //                                            ProviderName = payooprovider.IssuerName,
        //                                            ProviderId = payooprovider.IssuerName,
        //                                            IsOnline = payooprovider.IsOnline,
        //                                            CreatedOn = DateTime.Now,
        //                                            LastUpdatedDateUtc = DateTime.Now
        //                                        };

        //                                        frtpayooservice.PayooProviders.Add(frtpayooProvider);
        //                                    }
        //                                    else
        //                                    {
        //                                        frtpayooProvider.ProviderName = frtpayooProvider.ProviderName;
        //                                        frtpayooProvider.IsOnline = frtpayooProvider.IsOnline;
        //                                        UpdateFrtPayooProvider(frtpayooProvider);
        //                                    }

        //                                });


        //                                UpdateFrtPayooService(frtpayooservice);
        //                            }
        //                        }

        //                    });
        //                }

        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        //public bool UpdateFrtPayooProviderListFromPayoo()
        //{
        //    // Create new instance.
        //    UniGWSSoapClient uniGW = new UniGWSSoapClient();

        //    // Set UniGWS Request

        //    UniGWSRequest request = new UniGWSRequest();

        //    request.ClientId = CommonSettings.ClientId;
        //    string clientPassword = CommonSettings.ClientPassword;
        //    string privateKeyUrl = CommonSettings.PrivateKeyUrl;
        //    string passwordForPrivateKey = CommonSettings.PasswordForPrivateKey;
        //    string publicKey = CommonSettings.PublicKeyUrl;
        //    // End


        //    System.Net.ServicePointManager.CertificatePolicy = new CertPolicy(); // Ho tro cho viec goi webservice theo giao thuc https.

        //    string data = string.Empty;

        //    try
        //    {
        //        string strRequestTime = DateTime.Now.ToString("dd/MM/yyyy HHmmss");
        //        request.RequestTime = strRequestTime;
        //        // Checksum = MD5(RequestTime + ClientPassword)
        //        request.Checksum =MD5Encrypt.MD5Hash(strRequestTime + clientPassword);
        //        // End
        //        request.Operation = CommonSettings.MMS_GetProviders;

        //        GetProvidersRequest objGetProvidersRequest = new GetProvidersRequest();

        //        data = XmlSerializeLib.XmlSerialize(objGetProvidersRequest);

        //        request.Signature = SignData(data, privateKeyUrl, passwordForPrivateKey);

        //        request.RequestData = data;

        //        UniGWSResponse objResponse = uniGW.Execute2(request);

        //        // Verify signature.

        //        bool isCorrect = Verify(objResponse.ResponseData, objResponse.Signature, publicKey);
        //        if (!isCorrect)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            // Deserialize ResponseData in UniGWSResponse object for check ReturnCode

        //            GetProvidersResult objGetProvidersResult = XmlSerializeLib.Deserialize(objResponse.ResponseData, typeof(GetProvidersResult))
        //                as GetProvidersResult;

        //            if (objGetProvidersResult.ReturnCode == 0) // Sucessfully
        //            {

        //                if (objGetProvidersResult.Providers != null)
        //                {
        //                    var listdbProviderId = _providerService.GetAll().Select(p => p.ProviderCode).ToList();
        //                    var listPayooProviderId = objGetProvidersResult.Providers.Select(p => p.ProviderId);
        //                    listdbProviderId.ForEach(id =>
        //                    {
        //                        if (!listPayooProviderId.Contains(id))
        //                        {
        //                            var dbProvider = _providerService.GetAll(x=>x.ProviderCode==id).FirstOrDefault();
        //                            if (dbProvider != null)
        //                                _providerService.Delete(dbProvider);
        //                        }
        //                    });
        //                    objGetProvidersResult.Providers.ToList().ForEach(payooprovider =>
        //                    {
        //                        var frtpayooService =_service.GetAll(x=>x.ServiceCode==payooprovider.ServiceId).FirstOrDefault();
        //                        if (frtpayooService != null)
        //                        {
        //                            var frtpayooProvider = _providerService.GetAll(x => x.ProviderCode == payooprovider.ProviderName).FirstOrDefault();
        //                            if (frtpayooProvider == null)
        //                            {
        //                                frtpayooProvider = new Domain.Entity.Provider()
        //                                {
        //                                    ServiceId = frtpayooService.Id,
        //                                    ProviderName = payooprovider.ProviderName,
        //                                    ProviderCode = payooprovider.ProviderId,
        //                                    IsOnline = true,                                          
        //                                };
        //                           //    _providerService.InsertOrUpdate(frtpayooProvider);
        //                               frtpayooService.Providers.Add(frtpayooProvider);
        //                               _service.InsertOrUpdate(frtpayooService);
        //                            }
        //                            else
        //                            {
        //                                frtpayooProvider.ProviderName = payooprovider.ProviderName;
        //                                frtpayooProvider.ServiceCode = payooprovider.ServiceId;
        //                                _providerService.InsertOrUpdate(frtpayooProvider);
        //                            }
        //                        }
        //                    });


        //                }
        //                return true;
        //            }

        //            else
        //            {
        //                return false;
        //            }
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
        #endregion
        #region Payment

        public virtual QueryBillExResult BillQueryBE(string customerId, int providerEntityId)
        {
            UniGWSSoapClient uniGW = new UniGWSSoapClient();
            UniGWSRequest request = new UniGWSRequest();
            request.ClientId = CommonSettings.ClientId;
            // End
            System.Net.ServicePointManager.CertificatePolicy = new CertPolicy(); // Ho tro cho viec goi webservice theo giao thuc https.
            string data = string.Empty;
            try
            {
                var payooProvider = _providerService.GetById(providerEntityId);

                // QueryBillEx
                string serviceId = payooProvider.ServiceCode;
                string serviceName = payooProvider.Service.ServiceName;
                string strRequestTime = DateTime.Now.ToString("dd/MM/yyyy HHmmss");
                request.RequestTime = strRequestTime;

                request.Checksum = MD5Encrypt.MD5Hash(strRequestTime + CommonSettings.ClientPassword);
                //// End
                request.Operation = CommonSettings.MMS_QueryBillEx;

                QueryBillExRequest queryInput = new QueryBillExRequest();
                queryInput.AgentId = CommonSettings.AgentId;
                queryInput.CustomerId = customerId;
                queryInput.ProviderId = payooProvider.ProviderCode;
                queryInput.ProviderName = payooProvider.ProviderName;
                //string subNumber = customerPhone.Substring(0, 4);
                //bool numberTest = customerPhone[1] == '9';
                //if (numberTest)
                //    subNumber = subNumber.Remove(3, 1);
                //switch (subNumber)
                //{
                //    case "096":
                //    case "097":
                //    case "098":
                //    case "0162":
                //    case "0163":
                //    case "0164":
                //    case "0165":
                //    case "0166":
                //    case "0167":
                //    case "0168":
                //    case "0169":
                //        queryInput.ProviderId = "VIETTEL";
                //        queryInput.ProviderName = "VIETTEL";
                //        break;
                //    case "091":
                //    case "094":
                //    case "0123":
                //    case "0124":
                //    case "0125":
                //    case "0127":
                //    case "0129":
                //        queryInput.ProviderId = "VINASG";
                //        queryInput.ProviderName = "Vinaphone";
                //        break;
                //    case "090":
                //    case "093":
                //    case "0120":
                //    case "0121":
                //    case "0122":
                //    case "0126":
                //    case "0128":
                //        queryInput.ProviderId = "MOBI";
                //        queryInput.ProviderName = "Mobiphoe";
                //        break;
                //    //case "092":
                //    //case "0188":
                //    //    queryInput.ProviderId = "VNMOBILE";
                //    //    queryInput.ProviderName = "VietnamMobile";
                //    //    break;

                //    //case "095":
                //    //    queryInput.ProviderId = "SFONE";
                //    //    queryInput.ProviderName = "sfone";
                //        //break;
                //    default:
                //        return null;
                //}

                queryInput.UserId = CommonSettings.UserId;
                queryInput.ServiceId = serviceId;
                queryInput.ServiceName = serviceName;

                data = XmlSerializeLib.XmlSerialize(queryInput);

                request.Signature = SignData(data, CommonSettings.PrivateKeyUrl, CommonSettings.PasswordForPrivateKey);

                request.RequestData = data;


                string strRequest = XmlSerializeLib.XmlSerialize(request);

                UniGWSResponse objResponse = uniGW.Execute2(request);


                bool isCorrect = Verify(objResponse.ResponseData, objResponse.Signature, CommonSettings.PublicKeyUrl);
                if (!isCorrect)
                {
                    return null;
                }
                else
                {
                    // Deserialize ResponseData in UniGWSResponse object for check ReturnCode

                    QueryBillExResult objQueryBillExResult = XmlSerializeLib.Deserialize(objResponse.ResponseData, typeof(QueryBillExResult))
                        as QueryBillExResult;


                    return objQueryBillExResult;


                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //public bool BillPaymentBE(Order order, string systemtraceEx = null)
        //{
        //    try
        //    {
        //        var objQueryBillExResult = BillQueryBE(order.NumberPhone, order.ProviderId);
        //        if (objQueryBillExResult != null)
        //        {
        //            if (objQueryBillExResult.ReturnCode == 0) // Sucessfully
        //            {
        //                ////////////////////////////////////
        //                UniGWSSoapClient uniGW = new UniGWSSoapClient();

        //                // Set UniGWS Request
        //                string strRequestTime = DateTime.Now.ToString("dd/MM/yyyy HHmmss");
        //                UniGWSRequest requestPayment = new UniGWSRequest();
        //                requestPayment.ClientId = CommonSettings.ClientId;
        //                requestPayment.RequestTime = strRequestTime;
        //                // Checksum = MD5(RequestTime + ClientPassword)
        //                requestPayment.Checksum =MD5Encrypt.MD5Hash(strRequestTime + CommonSettings.ClientPassword);
        //                // End
        //                requestPayment.Operation = CommonSettings.MMS_PayOnlineBillEx;// Ten cua API giong nhu trong tai lieu API ma Payoo gui. Tuy vao chuc nang ma co Ten khac nhau.

        //                PayOnlineBillExRequest objPayOnlineBillExRequest = new PayOnlineBillExRequest();
        //                objPayOnlineBillExRequest.AgentId = CommonSettings.AgentId;
        //                objPayOnlineBillExRequest.UserId = CommonSettings.UserId;
        //                objPayOnlineBillExRequest.Bills = objQueryBillExResult.Bills.Select(p =>
        //                {
        //                    var b = new BillInfoEx()
        //                    {
        //                        BillId = p.BillId,
        //                        ServiceId = objQueryBillExResult.Services[0].ServiceId,
        //                        ProviderId = objQueryBillExResult.Services[0].Issuers[0].IssuerId,
        //                        Month = p.Month,
        //                        MoneyAmount = p.MoneyAmount,
        //                        PaymentFee = p.PaymentFee,
        //                        CustomerName = p.CustomerName,
        //                        Address = p.Address,
        //                        ExpiredDate = p.ExpiredDate,
        //                        IsPrepaid = p.IsPrepaid,
        //                        PaymentRange = p.PaymentRange,
        //                        RenewalDate = p.RenewalDate,
        //                        ServiceName = objQueryBillExResult.Services[0].ServiceName,
        //                        ProviderName = objQueryBillExResult.Services[0].Issuers[0].IssuerName,
        //                        MonthAmount = "1"
        //                    };
        //                    int monthamout;
        //                    bool test = int.TryParse(p.MonthAmount, out monthamout);
        //                    if (test)
        //                        b.MonthAmount = p.MonthAmount;
        //                    return b;
        //                }).ToArray();
        //                objPayOnlineBillExRequest.ContactAddress = order.NumberPhone;
        //                objPayOnlineBillExRequest.ContactName = "thuê bao " + order.NumberPhone;
        //                objPayOnlineBillExRequest.ContactPhone = order.NumberPhone;
        //                objPayOnlineBillExRequest.CustomerId = order.NumberPhone;
        //                objPayOnlineBillExRequest.SystemTraceEx = systemtraceEx;
        //                objPayOnlineBillExRequest.SystemTrace = order.OrderGuid.ToString();
        //                objPayOnlineBillExRequest.InvoiceNo = order.OrderGuid.ToString();
        //                objPayOnlineBillExRequest.TransactionTime = DateTime.Now.ToString("yyyyMMddHHmmss");


        //                string data = XmlSerializeLib.XmlSerialize(objPayOnlineBillExRequest);

        //                requestPayment.Signature = SignData(data, CommonSettings.PrivateKeyUrl, CommonSettings.PasswordForPrivateKey);

        //                requestPayment.RequestData = data;



        //                UniGWSResponse objResponsePayment = uniGW.Execute2(requestPayment);

        //                bool isCorrect = Verify(objResponsePayment.ResponseData, objResponsePayment.Signature, CommonSettings.PublicKeyUrl);
        //                if (!isCorrect)
        //                {
        //                    order.OrderNotes.Add(new OrderNote()
        //                    {
        //                        Note = "Payoo - Giao dịch thanh toán thất bại - Verify signature is not valid",

        //                        CreatedOn = DateTime.UtcNow,

        //                        FunctionName = CommonSettings.MMS_PayOnlineBillEx,
        //                        FunctionReturnCode = -112,
        //                       FunctionMessage = CommonSettings.GetPayOnlineBillExMessage(-112),
        //                    });
        //                    order.ResultCode = -112;
        //                    order.FunctionFinalReturnCode = -112;
        //                    order.FunctionNameFinalCall = CommonSettings.MMS_PayOnlineBillEx;
        //                    //order..TransactionDescription = CommonSettings.GetPayOnlineBillExMessage(-112);
        //                   _OrderService.InsertOrUpdate(order);
        //                    return false;
        //                }
        //                else
        //                {
        //                    // Deserialize ResponseData in UniGWSResponse object for check ReturnCode

        //                    PayOnlineBillExResult objPayOnlineBillExResult = XmlSerializeLib.Deserialize(objResponsePayment.ResponseData, typeof(PayOnlineBillExResult))
        //                        as PayOnlineBillExResult;
        //                    order.FunctionNameFinalCall = CommonSettings.MMS_PayOnlineBillEx;
        //                    order.ResultCode = objPayOnlineBillExResult.ReturnCode;
        //                    switch (objPayOnlineBillExResult.ReturnCode)
        //                    {
        //                        case 0:
        //                            order.OrderNotes.Add(new OrderNote()
        //                            {
        //                                Note = "Payoo - Giao dịch thanh toán thành công",
        //                                CreatedOn = DateTime.UtcNow,
        //                                FunctionName = CommonSettings.MMS_PayOnlineBillEx,
        //                               FunctionReturnCode = objPayOnlineBillExResult.ReturnCode,
        //                               FunctionMessage = CommonSettings.GetPayOnlineBillExMessage(objPayOnlineBillExResult.ReturnCode),
        //                            });
        //                            order.OrderStatusId = (int)OrderStatusEnum.COMPLETE;
        //                            order.ResultCode = objPayOnlineBillExResult.ReturnCode;
        //                            order.FunctionFinalReturnCode = objPayOnlineBillExResult.ReturnCode;
        //                            order.FunctionNameFinalCall = CommonSettings.MMS_PayOnlineBillEx;
        //                          //  order.TransactionDescription = CommonSettings.GetPayOnlineBillExMessage(objPayOnlineBillExResult.ReturnCode);
        //                           _OrderService.InsertOrUpdate(order);
        //                            return true;
        //                        case 1:
        //                            order.OrderNotes.Add(new OrderNote()
        //                            {
        //                                Note = "Payoo - Giao dịch thanh toán thành công",
        //                                CreatedOn = DateTime.UtcNow,
        //                               FunctionName = CommonSettings.MMS_PayOnlineBillEx,
        //                                FunctionReturnCode = objPayOnlineBillExResult.ReturnCode,
        //                               FunctionMessage = CommonSettings.GetPayOnlineBillExMessage(objPayOnlineBillExResult.ReturnCode),
        //                            });
        //                            order.OrderStatusId = (int)OrderStatusEnum.COMPLETE;
        //                            order.ResultCode = objPayOnlineBillExResult.ReturnCode;
        //                            order.FunctionFinalReturnCode = objPayOnlineBillExResult.ReturnCode;
        //                            order.FunctionNameFinalCall = CommonSettings.MMS_PayOnlineBillEx;
        //                           // orderTransactionDescription = CommonSettings.GetPayOnlineBillExMessage(objPayOnlineBillExResult.ReturnCode);
        //                           _OrderService.InsertOrUpdate(order);
        //                           // GetTransactionStatusBE(order);
        //                            return true;
        //                        case -1:
        //                        case -7:
        //                        case -8:
        //                        case -17:
        //                        case -19:
        //                            order.OrderNotes.Add(new OrderNote()
        //                            {
        //                                Note = "Payoo - Giao dịch thanh toán thất bại",
        //                                CreatedOn = DateTime.UtcNow,
        //                                FunctionName = CommonSettings.MMS_PayOnlineBillEx,
        //                                FunctionReturnCode = objPayOnlineBillExResult.ReturnCode,
        //                                FunctionMessage = CommonSettings.GetPayOnlineBillExMessage(objPayOnlineBillExResult.ReturnCode),
        //                            });
        //                            order.OrderStatusId = (int)OrderStatus.Refunded;
        //                            order.PayooReturnCode = objPayOnlineBillExResult.ReturnCode;
        //                            order.PayooFunctionFinalReturnCode = objPayOnlineBillExResult.ReturnCode;
        //                            order.FunctionNameFinalCall = CommonSettings.MMS_PayOnlineBillEx;
        //                            order.PayooTransactionDescription = CommonSettings.GetPayOnlineBillExMessage(objPayOnlineBillExResult.ReturnCode);
        //                            _OrderService.InsertOrUpdate(order);

        //                            return false;
        //                        case -3:
        //                            order.OrderNotes.Add(new OrderNote()
        //                            {
        //                                Note = "Payoo - Giao dịch thanh toán time out",
        //                                CreatedOn = DateTime.UtcNow,
        //                                FunctionName = CommonSettings.MMS_PayOnlineBillEx,
        //                                FunctionReturnCode = objPayOnlineBillExResult.ReturnCode,
        //                                FunctionMessage = CommonSettings.GetPayOnlineBillExMessage(objPayOnlineBillExResult.ReturnCode),
        //                            });
        //                            order.OrderStatusId = (int)OrderStatus.IsSent;
        //                            order.PayooReturnCode = objPayOnlineBillExResult.ReturnCode;
        //                            order.PayooFunctionFinalReturnCode = objPayOnlineBillExResult.ReturnCode;
        //                            order.FunctionNameFinalCall = CommonSettings.MMS_PayOnlineBillEx;
        //                            order.PayooTransactionDescription = CommonSettings.GetPayOnlineBillExMessage(objPayOnlineBillExResult.ReturnCode);
        //                            _OrderService.InsertOrUpdate(order);
        //                            GetTransactionStatusBE(order);
        //                            return true;
        //                        default:
        //                            break;
        //                    }

        //                }
        //                ////////////////////////////////////////////////

        //                return true;
        //            }
        //            else
        //            {
        //                order.OrderNotes.Add(new OrderNote()
        //                {
        //                    Note = "Payoo - " + CommonSettings.GetQueryBillExMessage(objQueryBillExResult.ReturnCode),
        //                    CreatedOn = DateTime.UtcNow,
        //                    FunctionName = CommonSettings.MMS_QueryBillEx,
        //                    FunctionReturnCode = objQueryBillExResult.ReturnCode,
        //                    FunctionMessage = CommonSettings.GetQueryBillExMessage(objQueryBillExResult.ReturnCode),
        //                });
        //                order.OrderStatusId = (int)OrderStatus.Pending;
        //                order.PayooReturnCode = objQueryBillExResult.ReturnCode;
        //                order.PayooFunctionFinalReturnCode = objQueryBillExResult.ReturnCode;
        //                order.FunctionNameFinalCall = CommonSettings.MMS_QueryBillEx;
        //                order.PayooTransactionDescription = CommonSettings.GetQueryBillExMessage(objQueryBillExResult.ReturnCode);
        //                _OrderService.InsertOrUpdate(order);
        //                return false;
        //            }
        //        }

        //        return true;
        //    }
        //    catch (Exception)
        //    {

        //        order.OrderNotes.Add(new OrderNote()
        //        {
        //            Note = "Payoo - " + CommonSettings.GetQueryBillExMessage(-111),
        //            CreatedOn = DateTime.UtcNow,
        //            FunctionName = CommonSettings.MMS_QueryBillEx,
        //            FunctionReturnCode = -111,
        //            FunctionMessage = CommonSettings.GetQueryBillExMessage(-111),
        //        });

        //        order.OrderStatusId = (int)OrderStatus.Pending;
        //        order.PayooReturnCode = -111;
        //        order.PayooFunctionFinalReturnCode = -111;
        //        order.FunctionName = CommonSettings.MMS_QueryBillEx;
        //        order.FunctionNameFinalCall = CommonSettings.MMS_QueryBillEx;
        //        order.PayooTransactionDescription = CommonSettings.GetQueryBillExMessage(-111);
        //        _OrderService.InsertOrUpdate(order);
        //        return false;
        //    }
        //}
        #endregion

        //#region Paycode
        public bool CodePaymentBE(Order order)
        {
            // Create new instance.
            UniGWSSoapClient uniGW = new UniGWSSoapClient();
            // Set UniGWS Request
            UniGWSRequest request = new UniGWSRequest();
            request.ClientId = CommonSettings.ClientId;
            // End

            System.Net.ServicePointManager.CertificatePolicy = new CertPolicy();

            string data = string.Empty;

            try
            {
                string strRequestTime = DateTime.Now.ToString("dd/MM/yyyy HHmmss");
                request.RequestTime = strRequestTime;
                request.Checksum = MD5Encrypt.MD5Hash(strRequestTime + CommonSettings.ClientPassword);

                request.Operation = CommonSettings.MMS_CodePaymentBE;
                var cardmobile = _cardMobileService.GetById(order.CardMobileId);

                CodePaymentBERequest objCodePaymentBERequest = new CodePaymentBERequest();
                objCodePaymentBERequest.CardValue = cardmobile.UnitPrice;
                objCodePaymentBERequest.InvoiceNo = order.OrderGuid.ToString();
                objCodePaymentBERequest.ProviderId = cardmobile.Name;
                objCodePaymentBERequest.Quantity = 1;
                objCodePaymentBERequest.SystemTrace = order.OrderGuid.ToString();
                objCodePaymentBERequest.TotalPurchasingAmount = order.TotalAmount;
                objCodePaymentBERequest.TotalReferAmount = order.TotalAmount;
                objCodePaymentBERequest.TransactionTime = DateTime.Now.ToString("yyyyMMddHHmmss");

                data = XmlSerializeLib.XmlSerialize(objCodePaymentBERequest);
                request.Signature = SignData(data, CommonSettings.PrivateKeyUrl, CommonSettings.PasswordForPrivateKey);
                request.RequestData = data;


                //string strRequest = XmlSerializeLib.XmlSerialize(request);

                // Call to Service
                order.DataSign = request.Signature;
                order.OrderNotes.Add(new OrderNote()
                {
                    Note = "Payoo Requesting - Giao dịch mua thẻ ",
                    CreatedOn = DateTime.UtcNow,


                });
                _OrderService.InsertOrUpdate(order);

                UniGWSResponse objResponse = uniGW.Execute2(request);
                // End


                // Verify signature.
                bool isCorrect = Verify(objResponse.ResponseData, objResponse.Signature, CommonSettings.PublicKeyUrl);
                if (!isCorrect)
                {
                    order.OrderNotes.Add(new OrderNote()
                    {
                        Note = "Payoo Response - Giao dịch mua thẻ thất bại - Verify signature is not valid",
                        CreatedOn = DateTime.UtcNow,
                        FunctionName = CommonSettings.MMS_CodePaymentBE,
                        FunctionReturnCode = -112,
                        FunctionMessage = CommonSettings.GetTopupPaymentMessage(-112),
                    });
                    order.ResultCode = -112;
                    order.FunctionFinalReturnCode = -112;
                    order.FunctionNameFinalCall = CommonSettings.MMS_CodePaymentBE;
                    order.ResultName = CommonSettings.GetCodePaymentMessage(-112);
                    _OrderService.InsertOrUpdate(order);
                    return false;
                }
                else
                {
                    // Deserialize ResponseData in UniGWSResponse object for check ReturnCode

                    CodePaymentBEResult objCodePaymentBEResult = XmlSerializeLib.Deserialize(objResponse.ResponseData, typeof(CodePaymentBEResult))
                        as CodePaymentBEResult;

                    order.FunctionNameFinalCall = CommonSettings.MMS_CodePaymentBE;
                    order.ResultCode = objCodePaymentBEResult.ReturnCode;

                    switch (objCodePaymentBEResult.ReturnCode)
                    {
                        case 0:
                            order.OrderNotes.Add(new OrderNote()
                            {
                                Note = "Payoo Response - Giao dịch mua thẻ thành công",
                                CreatedOn = DateTime.UtcNow,
                                FunctionName = CommonSettings.MMS_CodePaymentBE,
                                FunctionReturnCode = objCodePaymentBEResult.ReturnCode,
                                FunctionMessage = CommonSettings.GetCodePaymentMessage(objCodePaymentBEResult.ReturnCode),
                            });
                            order.OrderStatusId = (int)OrderStatusEnum.COMPLETE;
                            order.ResultCode = objCodePaymentBEResult.ReturnCode;
                            order.FunctionNameFinalCall = CommonSettings.MMS_CodePaymentBE;
                            order.ResultName = CommonSettings.GetCodePaymentMessage(objCodePaymentBEResult.ReturnCode);
               
                            // SendSMSToCustomer(order, objCodePaymentBEResult.PayCodes.ToList());
                           // _workflowMessageService.PayooSendEmailToCustumerBuyCard(order, objCodePaymentBEResult.PayCodes.ToList(), 3);
                            if (objCodePaymentBEResult.PayCodes.Count() > 0)
                            {
                                var payCodes = objCodePaymentBEResult.PayCodes[0];
                                order.Expired = payCodes.Expired;
                                order.TypeCard = payCodes.TypeCard;
                                order.SeriNumber = payCodes.SeriNumber;
                                order.CardId = payCodes.CardId;
                            }
                             _OrderService.InsertOrUpdate(order);

                            return true;

                        case -3:
                            order.OrderNotes.Add(new OrderNote()
                            {
                                Note = "Payoo Response - Giao dịch mua thẻ bị time out",
                                CreatedOn = DateTime.UtcNow,
                                FunctionName = CommonSettings.MMS_CodePaymentBE,
                                FunctionReturnCode = objCodePaymentBEResult.ReturnCode,
                                FunctionMessage = CommonSettings.GetCodePaymentMessage(objCodePaymentBEResult.ReturnCode),
                            });
                            order.OrderStatusId = (int)OrderStatusEnum.PROCESSING;
                            order.ResultCode = objCodePaymentBEResult.ReturnCode;
                            _OrderService.InsertOrUpdate(order);
                            //var objCodeGetCardListBEResult = CodeGetCardListBE(order);
                            //switch (objCodeGetCardListBEResult.ReturnCode)
                            //{
                            //    case 0:
                            //        order.OrderNotes.Add(new OrderNote()
                            //        {
                            //            Note = "Payoo - Giao dịch mua thẻ thành công",
                            //            CreatedOn = DateTime.UtcNow,
                            //            FunctionName = CommonSettings.MMS_CodeGetCardListBE,
                            //            FunctionReturnCode = objCodeGetCardListBEResult.ReturnCode,
                            //            FunctionMessage = CommonSettings.GetCardListMessage(objCodeGetCardListBEResult.ReturnCode),
                            //        });
                            //        order.OrderStatusId = (int)OrderStatus.Complete;
                            //        order.PayooFunctionFinalReturnCode = objCodeGetCardListBEResult.ReturnCode;
                            //        order.FunctionNameFinalCall = CommonSettings.MMS_CodeGetCardListBE;
                            //        order.PayooTransactionDescription = CommonSettings.GetCardListMessage(objCodeGetCardListBEResult.ReturnCode);
                            //        _OrderService.InsertOrUpdate(order);

                            //        SendSMSToCustomer(order, objCodeGetCardListBEResult.PayCodes.ToList());
                            //        _workflowMessageService.PayooSendEmailToCustumerBuyCard(order, objCodeGetCardListBEResult.PayCodes.ToList(), 3);
                            //        return true;
                            //    case -1:

                            //        order.OrderNotes.Add(new OrderNote()
                            //        {
                            //            Note = "Payoo - " + CommonSettings.GetCardListMessage(objCodeGetCardListBEResult.ReturnCode),
                            //            CreatedOn = DateTime.UtcNow,
                            //            FunctionName = CommonSettings.MMS_CodeGetCardListBE,
                            //            FunctionReturnCode = objCodeGetCardListBEResult.ReturnCode,
                            //            FunctionMessage = CommonSettings.GetCardListMessage(objCodeGetCardListBEResult.ReturnCode),
                            //        });

                            //        order.OrderStatusId = (int)OrderStatus.IsSent;
                            //        order.PayooFunctionFinalReturnCode = objCodeGetCardListBEResult.ReturnCode;
                            //        order.FunctionNameFinalCall = CommonSettings.MMS_CodeGetCardListBE;
                            //        order.PayooTransactionDescription = CommonSettings.GetCardListMessage(objCodeGetCardListBEResult.ReturnCode);
                            //        _OrderService.InsertOrUpdate(order);
                            //        return false;
                            //    case -2:
                            //        order.OrderNotes.Add(new OrderNote()
                            //        {
                            //            Note = "Payoo - " + CommonSettings.GetCardListMessage(objCodeGetCardListBEResult.ReturnCode),
                            //            CreatedOn = DateTime.UtcNow,
                            //            FunctionName = CommonSettings.MMS_CodeGetCardListBE,
                            //            FunctionReturnCode = objCodeGetCardListBEResult.ReturnCode,
                            //            FunctionMessage = CommonSettings.GetCardListMessage(objCodeGetCardListBEResult.ReturnCode),
                            //        });
                            //        order.OrderStatusId = (int)OrderStatus.Refunded;
                            //        order.PayooFunctionFinalReturnCode = objCodeGetCardListBEResult.ReturnCode;
                            //        order.FunctionNameFinalCall = CommonSettings.MMS_CodeGetCardListBE;
                            //        order.PayooTransactionDescription = CommonSettings.GetCardListMessage(objCodeGetCardListBEResult.ReturnCode);
                            //        _OrderService.InsertOrUpdate(order);
                            //        return false;
                            //    default:
                            //return false;
                            //}
                            return false;
                        case -1:
                        case -5:
                        case -6:
                        case -7:
                            order.OrderNotes.Add(new OrderNote()
                            {
                                Note = "Payoo Response - Thanh toán mã thẻ thất bại.",
                                CreatedOn = DateTime.UtcNow,
                                FunctionName = CommonSettings.MMS_CodePaymentBE,
                                FunctionReturnCode = objCodePaymentBEResult.ReturnCode,
                                FunctionMessage = CommonSettings.GetCodePaymentMessage(objCodePaymentBEResult.ReturnCode),
                            });

                            order.OrderStatusId = (int)OrderStatusEnum.PROCESSING;
                            order.FunctionFinalReturnCode = objCodePaymentBEResult.ReturnCode;
                            order.FunctionNameFinalCall = CommonSettings.MMS_CodePaymentBE;
                            order.ResultName = CommonSettings.GetCodePaymentMessage(objCodePaymentBEResult.ReturnCode);
                            _OrderService.InsertOrUpdate(order);
                            return false;
                        default:
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                order.OrderNotes.Add(new OrderNote()
                {
                    Note = "Payoo Response - " + CommonSettings.GetCodePaymentMessage(-111),
                    CreatedOn = DateTime.UtcNow,
                    FunctionName = CommonSettings.MMS_CodePaymentBE,
                    FunctionReturnCode = -111,
                    FunctionMessage = CommonSettings.GetCodePaymentMessage(-111),
                });

                //order.OrderStatusId = (int)OrderStatus.Pending;
                order.ResultCode = -111;
                order.FunctionFinalReturnCode = -111;
                order.FunctionNameFinalCall = CommonSettings.MMS_CodePaymentBE;
                order.ResultName = CommonSettings.GetCodePaymentMessage(-111);
                _OrderService.InsertOrUpdate(order);
                return false;
            }
        }

        //public void GetCardCodeList(FrtPayooOrder order)
        //{

        //    var objCodeGetCardListBEResult = CodeGetCardListBE(order);

        //    switch (objCodeGetCardListBEResult.ReturnCode)
        //    {
        //        case 0:
        //            order.OrderNotes.Add(new OrderNote()
        //            {
        //                Note = "Payoo Response - Giao dịch mua thẻ thành công",
        //                CreatedOn = DateTime.UtcNow,
        //                FunctionName = CommonSettings.MMS_CodeGetCardListBE,
        //                FunctionReturnCode = objCodeGetCardListBEResult.ReturnCode,
        //                FunctionMessage = CommonSettings.GetCardListMessage(objCodeGetCardListBEResult.ReturnCode),
        //            });
        //            order.OrderStatusId = (int)OrderStatus.Complete;
        //            order.PayooFunctionFinalReturnCode = objCodeGetCardListBEResult.ReturnCode;
        //            order.FunctionNameFinalCall = CommonSettings.MMS_CodeGetCardListBE;
        //            order.PayooTransactionDescription = CommonSettings.GetCardListMessage(objCodeGetCardListBEResult.ReturnCode);
        //            _OrderService.InsertOrUpdate(order);

        //            SendSMSToCustomer(order, objCodeGetCardListBEResult.PayCodes.ToList());
        //            _workflowMessageService.PayooSendEmailToCustumerBuyCard(order, objCodeGetCardListBEResult.PayCodes.ToList(), 3);
        //            return;
        //        case -1:

        //            order.OrderNotes.Add(new OrderNote()
        //            {
        //                Note = "Payoo Response - " + CommonSettings.GetCardListMessage(objCodeGetCardListBEResult.ReturnCode),
        //                CreatedOn = DateTime.UtcNow,
        //                FunctionName = CommonSettings.MMS_CodeGetCardListBE,
        //                FunctionReturnCode = objCodeGetCardListBEResult.ReturnCode,
        //                FunctionMessage = CommonSettings.GetCardListMessage(objCodeGetCardListBEResult.ReturnCode),
        //            });

        //            order.OrderStatusId = (int)OrderStatus.IsSent;
        //            order.PayooFunctionFinalReturnCode = objCodeGetCardListBEResult.ReturnCode;
        //            order.FunctionNameFinalCall = CommonSettings.MMS_CodeGetCardListBE;
        //            order.PayooTransactionDescription = CommonSettings.GetCardListMessage(objCodeGetCardListBEResult.ReturnCode);
        //            _OrderService.InsertOrUpdate(order);
        //            return;
        //        case -2:
        //            order.OrderNotes.Add(new OrderNote()
        //            {
        //                Note = "Payoo Response - " + CommonSettings.GetCardListMessage(objCodeGetCardListBEResult.ReturnCode),
        //                CreatedOn = DateTime.UtcNow,
        //                FunctionName = CommonSettings.MMS_CodeGetCardListBE,
        //                FunctionReturnCode = objCodeGetCardListBEResult.ReturnCode,
        //                FunctionMessage = CommonSettings.GetCardListMessage(objCodeGetCardListBEResult.ReturnCode),
        //            });
        //            order.OrderStatusId = (int)OrderStatus.Refunded;
        //            order.PayooFunctionFinalReturnCode = objCodeGetCardListBEResult.ReturnCode;
        //            order.FunctionNameFinalCall = CommonSettings.MMS_CodeGetCardListBE;
        //            order.PayooTransactionDescription = CommonSettings.GetCardListMessage(objCodeGetCardListBEResult.ReturnCode);
        //            _OrderService.InsertOrUpdate(order);
        //            return;
        //        default:
        //            return;
        //    }

        //}

        //private void SendSMSToCustomer(FrtPayooOrder order, List<PayCodeInfo> paycodes)
        //{

        //    string smsTemp = "", sms = "";
        //    int returncode = 0;

        //    try
        //    {
        //        smsPortTypeClient client = new smsPortTypeClient();

        //        string customerPhone = "84" + order.CustomerPhone.Remove(0, 1);

        //        smsTemp = "Chao {0}, Ma the quy khach da mua la : ";
        //        foreach (var code in paycodes)
        //        {
        //            smsTemp += code.CardId + ", ";
        //        }
        //        smsTemp = smsTemp.Remove(smsTemp.Length - 2, 2);
        //        sms = string.Format(smsTemp, order.CustomerName);
        //        returncode = client.sendSms(SMSParameters.Code, SMSParameters.Account, customerPhone, SMSParameters.BrandName, sms);

        //        order.OrderNotes.Add(new OrderNote()
        //        {
        //            Note = sms + ", MessageID : " + returncode + ", PhoneNumber : " + customerPhone,
        //            CreatedOn = DateTime.UtcNow,
        //            FunctionName = CommonSettings.MMS_CodeGetCardListBE,
        //        });
        //        _OrderService.InsertOrUpdate(order);
        //    }
        //    catch (Exception ex)
        //    {
        //        order.OrderNotes.Add(new OrderNote()
        //        {
        //            Note = "error send message : " + ex.Message,
        //            CreatedOn = DateTime.UtcNow,
        //            FunctionName = "SendMessage",
        //        });
        //        _OrderService.InsertOrUpdate(order);
        //    }

        //}

        //public PaycodeInquiryBEResult PaycodeInquiryBE(PaycodeInquiryBERequest objtPaycodeInquiryBERequest)
        //{
        //    // Create new instance.
        //    UniGWSSoapClient uniGW = new UniGWSSoapClient();

        //    // Set UniGWS Request

        //    UniGWSRequest request = new UniGWSRequest();

        //    request.ClientId = CommonSettings.ClientId;
        //    string clientPassword = CommonSettings.ClientPassword;
        //    string privateKeyUrl = CommonSettings.PrivateKeyUrl;
        //    string passwordForPrivateKey = CommonSettings.PasswordForPrivateKey;
        //    string publicKey = CommonSettings.PublicKeyUrl;
        //    // End


        //    System.Net.ServicePointManager.CertificatePolicy = new CertPolicy(); // Ho tro cho viec goi webservice theo giao thuc https.

        //    string data = string.Empty;

        //    try
        //    {

        //        // Demo for QueryBillEx
        //        string strRequestTime = DateTime.Now.ToString("dd/MM/yyyy HHmmss");
        //        request.RequestTime = strRequestTime;
        //        // Checksum = MD5(RequestTime + ClientPassword)
        //        request.Checksum = GetMD5Hash(strRequestTime + clientPassword);
        //        // End

        //        request.Operation = CommonSettings.MMS_PaycodeInquiryBE;// Ten cua API giong nhu trong tai lieu API ma Payoo gui. Tuy vao chuc nang ma co Ten khac nhau.

        //        data = XmlSerializeLib.XmlSerialize(objtPaycodeInquiryBERequest);

        //        request.Signature = SignData(data, privateKeyUrl, passwordForPrivateKey);

        //        request.RequestData = data;

        //        // Call to Service

        //        UniGWSResponse objResponse = uniGW.Execute2(request);

        //        // End

        //        // Verify signature.

        //        bool isCorrect = Verify(objResponse.ResponseData, objResponse.Signature, publicKey);
        //        if (!isCorrect)
        //        {
        //            return new PaycodeInquiryBEResult() { ReturnCode = -112 };
        //        }
        //        else
        //        {
        //            // Deserialize ResponseData in UniGWSResponse object for check ReturnCode

        //            PaycodeInquiryBEResult objPaycodeInquiryBEResult = XmlSerializeLib.Deserialize(objResponse.ResponseData, typeof(PaycodeInquiryBEResult))
        //                as PaycodeInquiryBEResult;
        //            return objPaycodeInquiryBEResult;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return new PaycodeInquiryBEResult() { ReturnCode = -111 };
        //    }
        //}

        //public CodeGetCardListBEResult CodeGetCardListBE(FrtPayooOrder order)
        //{
        //    // Create new instance.
        //    UniGWSSoapClient uniGW = new UniGWSSoapClient();

        //    // Set UniGWS Request

        //    UniGWSRequest request = new UniGWSRequest();

        //    request.ClientId = CommonSettings.ClientId;
        //    string clientPassword = CommonSettings.ClientPassword;
        //    string privateKeyUrl = CommonSettings.PrivateKeyUrl;
        //    string passwordForPrivateKey = CommonSettings.PasswordForPrivateKey;
        //    string publicKey = CommonSettings.PublicKeyUrl;
        //    // End


        //    System.Net.ServicePointManager.CertificatePolicy = new CertPolicy(); // Ho tro cho viec goi webservice theo giao thuc https.

        //    string data = string.Empty;

        //    try
        //    {

        //        // Demo for QueryBillEx
        //        string strRequestTime = DateTime.Now.ToString("dd/MM/yyyy HHmmss");
        //        request.RequestTime = strRequestTime;
        //        // Checksum = MD5(RequestTime + ClientPassword)
        //        request.Checksum = GetMD5Hash(strRequestTime + clientPassword);
        //        // End

        //        request.Operation = CommonSettings.MMS_GetTransactionStatusBE;// Ten cua API giong nhu trong tai lieu API ma Payoo gui. Tuy vao chuc nang ma co Ten khac nhau.


        //        var objCodeGetCardListBERequest = new CodeGetCardListBERequest()
        //        {
        //            SystemTrace = order.PayooOrderGuid.ToString()
        //        };
        //        data = XmlSerializeLib.XmlSerialize(objCodeGetCardListBERequest);

        //        request.Signature = SignData(data, privateKeyUrl, passwordForPrivateKey);

        //        request.RequestData = data;

        //        // Call to Service
        //        order.OrderNotes.Add(new OrderNote()
        //        {
        //            Note = "Payoo Request - Lấy lại mã thẻ thất bại",
        //            CreatedOn = DateTime.UtcNow,
        //        });
        //        _OrderService.InsertOrUpdate(order);

        //        UniGWSResponse objResponse = uniGW.Execute2(request);

        //        // End

        //        // Verify signature.

        //        bool isCorrect = Verify(objResponse.ResponseData, objResponse.Signature, publicKey);
        //        if (!isCorrect)
        //        {
        //            order.OrderNotes.Add(new OrderNote()
        //            {
        //                Note = "Payoo Response - Lấy lại mã thẻ thất bại - Verify signature is not valid",
        //                CreatedOn = DateTime.UtcNow,
        //                FunctionName = CommonSettings.MMS_CodeGetCardListBE,
        //                FunctionReturnCode = -112,
        //                FunctionMessage = CommonSettings.GetTopupPaymentMessage(-112),
        //            });
        //            order.OrderStatusId = (int)OrderStatus.Pending;
        //            order.PayooFunctionFinalReturnCode = -112;
        //            order.FunctionNameFinalCall = CommonSettings.MMS_CodeGetCardListBE;
        //            order.PayooTransactionDescription = CommonSettings.GetCardListMessage(-112);
        //            _OrderService.InsertOrUpdate(order);
        //            return null;
        //        }
        //        else
        //        {
        //            // Deserialize ResponseData in UniGWSResponse object for check ReturnCode

        //            CodeGetCardListBEResult objCodeGetCardListBEResult = XmlSerializeLib.Deserialize(objResponse.ResponseData, typeof(CodeGetCardListBEResult))
        //                as CodeGetCardListBEResult;

        //            return objCodeGetCardListBEResult;



        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        order.OrderNotes.Add(new OrderNote()
        //        {
        //            Note = "Payoo Response - " + CommonSettings.GetCardListMessage(-111),
        //            CreatedOn = DateTime.UtcNow,
        //            FunctionName = CommonSettings.MMS_CodeGetCardListBE,
        //            FunctionReturnCode = -111,
        //            FunctionMessage = CommonSettings.GetTopupPaymentMessage(-111),
        //        });
        //        order.OrderStatusId = (int)OrderStatus.Pending;
        //        order.PayooFunctionFinalReturnCode = -111;
        //        order.FunctionNameFinalCall = CommonSettings.MMS_CodeGetCardListBE;
        //        order.PayooTransactionDescription = CommonSettings.GetCardListMessage(-111);
        //        _OrderService.InsertOrUpdate(order);
        //        return null;
        //    }
        //}
        //#endregion

        #region Topup

        public int TopupPaymentBE(Order order)
        {
            // Create new instance.
            UniGWSSoapClient uniGW = new UniGWSSoapClient();

            // Set UniGWS Request

            UniGWSRequest request = new UniGWSRequest();

            request.ClientId = CommonSettings.ClientId;
            string clientPassword = CommonSettings.ClientPassword;
            string privateKeyUrl = CommonSettings.PrivateKeyUrl;
            string passwordForPrivateKey = CommonSettings.PasswordForPrivateKey;
            string publicKey = CommonSettings.PublicKeyUrl;
            // End


            System.Net.ServicePointManager.CertificatePolicy = new CertPolicy(); // Ho tro cho viec goi webservice theo giao thuc https.

            string data = string.Empty;

            try
            {
                // Demo for QueryBillEx
                string strRequestTime = DateTime.Now.ToString("dd/MM/yyyy HHmmss");
                request.RequestTime = strRequestTime;
                // Checksum = MD5(RequestTime + ClientPassword)
                request.Checksum = MD5Encrypt.MD5Hash(strRequestTime + clientPassword);
                // End



                request.Operation = CommonSettings.MMS_TopupPaymentBE;// Ten cua API giong nhu trong tai lieu API ma Payoo gui. Tuy vao chuc nang ma co Ten khac nhau.
                TopupPaymentBERequest objtopupPaymentBErequest = new TopupPaymentBERequest();
                objtopupPaymentBErequest.CardValue = order.TotalAmount;
                objtopupPaymentBErequest.InvoiceNo = order.OrderGuid.ToString();
                objtopupPaymentBErequest.PrimaryAccount = order.NumberPhone;
                objtopupPaymentBErequest.SystemTrace = order.OrderGuid.ToString();
                objtopupPaymentBErequest.TotalPurchasingAmount = order.TotalAmount;
                objtopupPaymentBErequest.TotalReferAmount = order.TotalAmount;
                objtopupPaymentBErequest.TransactionTime = DateTime.Now.ToString("yyyyMMddHHmmss");

                data = XmlSerializeLib.XmlSerialize(objtopupPaymentBErequest);

                request.Signature = SignData(data, privateKeyUrl, passwordForPrivateKey);

                request.RequestData = data;


                //string demo = XmlSerializeLib.XmlSerialize(request);

                // Call to Service
                order.DataSign = request.Signature;
                order.OrderNotes.Add(new OrderNote()
                {
                    Note = "Payoo Request - Giao dịch nạp topup",
                    CreatedOn = DateTime.UtcNow,
                });
                _OrderService.InsertOrUpdate(order);

                UniGWSResponse objResponse = uniGW.Execute2(request);

                // End

                // Verify signature.

                bool isCorrect = Verify(objResponse.ResponseData, objResponse.Signature, publicKey);
                if (!isCorrect)
                {
                    order.OrderNotes.Add(new OrderNote()
                    {
                        Note = "Payoo Response - Giao dịch nạp topup thất bại - Verify signature is not valid",
                        CreatedOn = DateTime.UtcNow,
                        FunctionName = CommonSettings.MMS_TopupPaymentBE,
                        FunctionReturnCode = -112,
                        FunctionMessage = CommonSettings.GetTopupPaymentMessage(-112),
                    });
                    order.FunctionNameFinalCall = CommonSettings.MMS_TopupPaymentBE;
                    order.ResultCode = -112;
                    order.FunctionFinalReturnCode = -112;
                    order.ResultName = CommonSettings.GetTopupPaymentMessage(-112);
                    _OrderService.InsertOrUpdate(order);
                    return -112;
                }
                else
                {
                    // Deserialize ResponseData in UniGWSResponse object for check ReturnCode

                    TopupPaymentBEResult objTopupPaymentBEResult = XmlSerializeLib.Deserialize(objResponse.ResponseData, typeof(TopupPaymentBEResult))
                        as TopupPaymentBEResult;
                    order.ResultCode = objTopupPaymentBEResult.ReturnCode;
                    order.FunctionNameFinalCall = CommonSettings.MMS_TopupPaymentBE;

                    switch (objTopupPaymentBEResult.ReturnCode)
                    {
                        case 0:
                            order.OrderNotes.Add(new OrderNote()
                            {
                                Note = "Payoo Response - Giao dịch nạp topup thành công",
                                CreatedOn = DateTime.UtcNow,
                                FunctionName = CommonSettings.MMS_TopupPaymentBE,
                                FunctionReturnCode = objTopupPaymentBEResult.ReturnCode,
                                FunctionMessage = CommonSettings.GetTopupPaymentMessage(objTopupPaymentBEResult.ReturnCode),
                            });
                            // order. = "Nạp topup thành công cho số " + order.CustomerPhone + " số tiền " + _priceFormatter.FormatPrice(order.OrderCashAmount);
                            order.OrderStatusId = (int)OrderStatusEnum.COMPLETE;

                            order.FunctionFinalReturnCode = objTopupPaymentBEResult.ReturnCode;
                            order.ResultName = "Nạp topup thành công cho số " + order.NumberPhone + " số tiền " + order.TotalAmount;
                            _OrderService.InsertOrUpdate(order);
                            break;

                        case -3:
                            order.OrderNotes.Add(new OrderNote()
                            {
                                Note = "Payoo Response - Giao dịch nạp topup bị time out",
                                CreatedOn = DateTime.UtcNow,
                                FunctionName = CommonSettings.MMS_TopupPaymentBE,
                                FunctionReturnCode = objTopupPaymentBEResult.ReturnCode,
                                FunctionMessage = CommonSettings.GetTopupPaymentMessage(objTopupPaymentBEResult.ReturnCode),
                            });
                            _OrderService.InsertOrUpdate(order);
                            var transactionstatuscode = GetTransactionStatusBE(order);

                            break;
                        case -1:
                        case -5:
                        case -7:
                            order.OrderNotes.Add(new OrderNote()
                            {
                                Note = "Payoo Response - Giao dịch nạp topup thất bại",
                                CreatedOn = DateTime.UtcNow,
                                FunctionName = CommonSettings.MMS_TopupPaymentBE,
                                FunctionReturnCode = objTopupPaymentBEResult.ReturnCode,
                                FunctionMessage = CommonSettings.GetTopupPaymentMessage(objTopupPaymentBEResult.ReturnCode),
                            });

                            order.OrderStatusId = (int)OrderStatusEnum.PROCESSING;
                            order.FunctionFinalReturnCode = objTopupPaymentBEResult.ReturnCode;
                            order.FunctionNameFinalCall = CommonSettings.MMS_TopupPaymentBE;
                            order.ResultName = CommonSettings.GetTopupPaymentMessage(objTopupPaymentBEResult.ReturnCode);
                            _OrderService.InsertOrUpdate(order);
                            break;
                        default:
                            break;
                    }
                    return objTopupPaymentBEResult.ReturnCode;
                }
            }
            catch (Exception ex)
            {
                order.OrderNotes.Add(new OrderNote()
                {
                    Note = "Payoo Response - Giao dịch nạp topup thất bại - có lỗi xảy ra :" + ex.Message,

                    CreatedOn = DateTime.UtcNow
                });
                order.ResultCode = -111;
                order.FunctionFinalReturnCode = -111;
                order.FunctionNameFinalCall = CommonSettings.MMS_TopupPaymentBE;
                order.FunctionNameFinalCall = CommonSettings.MMS_TopupPaymentBE;
                order.ResultName = CommonSettings.GetTopupPaymentMessage(-111);
                _OrderService.InsertOrUpdate(order);
                return -111;
            }
        }

        public GetTopupValueListResult GetTopupValueList(string phoneNumber)
        {
            // Create new instance.
            UniGWSSoapClient uniGW = new UniGWSSoapClient();

            // Set UniGWS Request

            UniGWSRequest request = new UniGWSRequest();

            request.ClientId = CommonSettings.ClientId;
            //string clientPassword = ClientPassword;
            //string privateKeyUrl = PrivateKeyUrl;
            //string passwordForPrivateKey = PasswordForPrivateKey;
            //string publicKey = PublicKeyUrl;
            // End


            System.Net.ServicePointManager.CertificatePolicy = new CertPolicy(); // Ho tro cho viec goi webservice theo giao thuc https.

            string data = string.Empty;

            try
            {
                // Demo for QueryBillEx
                string strRequestTime = DateTime.Now.ToString("dd/MM/yyyy HHmmss");
                request.RequestTime = strRequestTime;
                // Checksum = MD5(RequestTime + ClientPassword)
                request.Checksum = MD5Encrypt.MD5Hash(strRequestTime + CommonSettings.ClientPassword);
                // End



                request.Operation = CommonSettings.MMS_GetTopupValueList;// Ten cua API giong nhu trong tai lieu API ma Payoo gui. Tuy vao chuc nang ma co Ten khac nhau.


                var objGetTopupValueListRequest = new GetTopupValueListRequest();
                objGetTopupValueListRequest.PhoneNo = phoneNumber;
                data = XmlSerializeLib.XmlSerialize(objGetTopupValueListRequest);

                request.Signature = SignData(data, CommonSettings.PrivateKeyUrl, CommonSettings.PasswordForPrivateKey);

                request.RequestData = data;

                //bool isCorrect = Verify(data, request.Signature, publicKey);

                string demo = XmlSerializeLib.XmlSerialize(request);

                // Call to VU Service


                UniGWSResponse objResponse = uniGW.Execute2(request);

                // End

                // Verify signature.

                bool isCorrect = Verify(objResponse.ResponseData, objResponse.Signature, CommonSettings.PublicKeyUrl);
                if (!isCorrect)
                {
                    return new GetTopupValueListResult() { ReturnCode = -112 };
                }
                else
                {
                    // Deserialize ResponseData in UniGWSResponse object for check ReturnCode

                    GetTopupValueListResult objGetTopupValueListResult = XmlSerializeLib.Deserialize(objResponse.ResponseData, typeof(GetTopupValueListResult))
                        as GetTopupValueListResult;

                    return objGetTopupValueListResult;

                }
            }
            catch (Exception)
            {
                return new GetTopupValueListResult() { ReturnCode = -111 };
            }
        }



        #endregion

        #region Unities
        public static bool Verify(string signedData, string signature, string publicKeyUrl)
        {
            //Log.Write(Encoding.Default.BodyName, "PKCS7");
            bool result = true;

            ContentInfo content = new ContentInfo(Encoding.Default.GetBytes(signedData));
            SignedCms signatureVerifier = new SignedCms(content, true);

            try
            {
                X509Certificate2 cert = new X509Certificate2(publicKeyUrl);
                signatureVerifier.Decode(Convert.FromBase64String(signature));
                X509Certificate2Collection certCollection = new X509Certificate2Collection(cert);
                signatureVerifier.CheckSignature(certCollection, true);
                if (signatureVerifier.Certificates.Count > 0)
                {
                    if (!signatureVerifier.Certificates[0].Equals(cert))
                    {
                        result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log.Write(ex, "PKCS7");
                result = false;
            }

            return result;
        }
        private string SignData(string data, string privateKeyUrl, string passwordForPrivateKey)
        {
            try
            {
                byte[] datas = File.ReadAllBytes(privateKeyUrl);
                CmsCryptography objSign = new CmsCryptography();
                //objSign.CharacterEncoding = Encoding.Default.BodyName;
                objSign.LoadSignerCredential(datas,
                    passwordForPrivateKey);
                return objSign.Sign(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string CheckReponseCodeWriteNodes(string responseCode)
        {
            string message;

            if (responseCode == "1")
                message = "Ngân hàng từ chối giao dịch.";
            else if (responseCode == "3")
                message = "MerchantID không tồn tại.";
            else if (responseCode == "4")
                message = "AccessCode không đúng.";
            else if (responseCode == "5")
                message = "Số tiền không hợp lệ.";
            else if (responseCode == "6")
                message = "Mã tiền tệ không tồn tại.";
            else if (responseCode == "7")
                message = "Lỗi cổng OnePAY không xác định.";
            else if (responseCode == "8")
                message = "Số thẻ không chính xác.";
            else if (responseCode == "9")
                message = "Tên chủ thẻ không đúng.";
            else if (responseCode == "10")
                message = "Thẻ hết hạn/Thẻ bị khóa";
            else if (responseCode == "11")
                message = "Thẻ chưa đăng kí sử dụng dịch vụ.";
            else if (responseCode == "12")
                message = "Ngày phát hành/Hết hạn không đúng.";
            else if (responseCode == "13")
                message = "Số tiền thanh toán vượt quá hạn mức thanh toán.";
            else if (responseCode == "21")
                message = "Số tiền trong tài khoản không đủ để thanh toán.";
            else if (responseCode == "99")
                message = "Khách hàng hủy giao dịch.";
            else
                message = "Quá trình thanh toán lỗi!";

            return message;
        }
        #endregion
        #region TransactionStatus
        public int GetTransactionStatusBE(Order order)
        {
            // Create new instance.
            UniGWSSoapClient uniGW = new UniGWSSoapClient();

            // Set UniGWS Request

            UniGWSRequest request = new UniGWSRequest();

            request.ClientId = CommonSettings.ClientId;
            string clientPassword = CommonSettings.ClientPassword;
            string privateKeyUrl = CommonSettings.PrivateKeyUrl;
            string passwordForPrivateKey = CommonSettings.PasswordForPrivateKey;
            string publicKey = CommonSettings.PublicKeyUrl;
            // End


            System.Net.ServicePointManager.CertificatePolicy = new CertPolicy(); // Ho tro cho viec goi webservice theo giao thuc https.

            string data = string.Empty;

            try
            {

                // Demo for QueryBillEx
                string strRequestTime = DateTime.Now.ToString("dd/MM/yyyy HHmmss");
                request.RequestTime = strRequestTime;
                // Checksum = MD5(RequestTime + ClientPassword)
                request.Checksum = MD5Encrypt.MD5Hash(strRequestTime + clientPassword);
                // End

                request.Operation = CommonSettings.MMS_GetTransactionStatusBE;// Ten cua API giong nhu trong tai lieu API ma Payoo gui. Tuy vao chuc nang ma co Ten khac nhau.


                var objGetTransactionStatusBERequest = new GetTransactionStatusBERequest()
                {
                    RequestTime = strRequestTime,
                    InvoiceNo = order.OrderGuid.ToString()
                };
                data = XmlSerializeLib.XmlSerialize(objGetTransactionStatusBERequest);

                request.Signature = SignData(data, privateKeyUrl, passwordForPrivateKey);

                request.RequestData = data;

                // Call to Service
                order.OrderNotes.Add(new OrderNote()
                {
                    Note = "Payoo Request - Update transaction status",
                    CreatedOn = DateTime.UtcNow,
                });

                _OrderService.InsertOrUpdate(order);

                UniGWSResponse objResponse = uniGW.Execute2(request);

                // End

                // Verify signature.

                bool isCorrect = Verify(objResponse.ResponseData, objResponse.Signature, publicKey);
                if (!isCorrect)
                {
                    order.OrderNotes.Add(new OrderNote()
                    {
                        Note = "Payoo Reponse - Giao dịch thất bại - Verify signature is not valid",
                        CreatedOn = DateTime.UtcNow,
                        FunctionName = CommonSettings.MMS_GetTransactionStatusBE,
                        FunctionReturnCode = -112,
                        FunctionMessage = CommonSettings.GetTransactionStatusMessage(-112),
                    });
                    order.FunctionFinalReturnCode = -112;
                    order.FunctionNameFinalCall = CommonSettings.MMS_GetTransactionStatusBE;
                    order.ResultName = CommonSettings.GetTransactionStatusMessage(-112);
                    _OrderService.InsertOrUpdate(order);
                    return -112;
                }
                else
                {
                    // Deserialize ResponseData in UniGWSResponse object for check ReturnCode

                    GetTransactionStatusBEResult objGetTransactionStatusBEResult = XmlSerializeLib.Deserialize(objResponse.ResponseData, typeof(GetTransactionStatusBEResult))
                        as GetTransactionStatusBEResult;


                    switch (objGetTransactionStatusBEResult.ReturnCode)
                    {
                        case -1:
                            order.OrderNotes.Add(new OrderNote()
                            {
                                Note = "Payoo Reponse - " + CommonSettings.GetTransactionStatusMessage(objGetTransactionStatusBEResult.ReturnCode),
                                CreatedOn = DateTime.UtcNow,
                                FunctionName = CommonSettings.MMS_GetTransactionStatusBE,
                                FunctionReturnCode = objGetTransactionStatusBEResult.ReturnCode,
                                FunctionMessage = CommonSettings.GetTransactionStatusMessage(objGetTransactionStatusBEResult.ReturnCode),
                            });
                            order.OrderStatusId = (int)OrderStatusEnum.PROCESSING;
                            order.FunctionFinalReturnCode = objGetTransactionStatusBEResult.ReturnCode;
                            order.FunctionNameFinalCall = CommonSettings.MMS_GetTransactionStatusBE;
                            order.ResultName = CommonSettings.GetTransactionStatusMessage(objGetTransactionStatusBEResult.ReturnCode);
                            _OrderService.InsertOrUpdate(order);
                            break;
                        case -2:
                            order.OrderNotes.Add(new OrderNote()
                            {
                                Note = "Payoo Reponse - " + CommonSettings.GetTransactionStatusMessage(objGetTransactionStatusBEResult.ReturnCode),
                                CreatedOn = DateTime.UtcNow,
                                FunctionName = CommonSettings.MMS_GetTransactionStatusBE,
                                FunctionReturnCode = objGetTransactionStatusBEResult.ReturnCode,
                                FunctionMessage = CommonSettings.GetTransactionStatusMessage(objGetTransactionStatusBEResult.ReturnCode),
                            });
                            order.OrderStatusId = (int)OrderStatusEnum.PENDING;
                            order.FunctionFinalReturnCode = objGetTransactionStatusBEResult.ReturnCode;
                            order.FunctionNameFinalCall = CommonSettings.MMS_GetTransactionStatusBE;
                            order.ResultName = CommonSettings.GetTransactionStatusMessage(objGetTransactionStatusBEResult.ReturnCode);
                            _OrderService.InsertOrUpdate(order);
                            break;
                        default:
                            break;
                    }
                    if (objGetTransactionStatusBEResult.ReturnCode == 0)
                    {
                        switch (objGetTransactionStatusBEResult.Status)
                        {
                            case 0:
                            case 3:
                            case 4:
                                order.OrderNotes.Add(new OrderNote()
                                {
                                    Note = "Payoo Reponse - Chưa biết được kết quả cuối cùng từ Payoo",
                                    CreatedOn = DateTime.UtcNow,
                                    FunctionName = CommonSettings.MMS_GetTransactionStatusBE,
                                    FunctionReturnCode = objGetTransactionStatusBEResult.ReturnCode,
                                    FunctionMessage = CommonSettings.GetTransactionStatusMessage(objGetTransactionStatusBEResult.ReturnCode),
                                });

                                order.OrderStatusId = (int)OrderStatusEnum.PROCESSING;
                                order.FunctionFinalReturnCode = objGetTransactionStatusBEResult.ReturnCode;
                                order.FunctionNameFinalCall = CommonSettings.MMS_GetTransactionStatusBE;
                                order.ResultName = CommonSettings.GetTransactionStatusMessage(objGetTransactionStatusBEResult.ReturnCode);
                                _OrderService.InsertOrUpdate(order);
                                break;

                            case 1:
                                order.OrderNotes.Add(new OrderNote()
                                {
                                    Note = "Payoo Reponse - Trạng thái giao dịch thành công",
                                    CreatedOn = DateTime.UtcNow,
                                    FunctionName = CommonSettings.MMS_GetTransactionStatusBE,
                                    FunctionReturnCode = objGetTransactionStatusBEResult.ReturnCode,
                                    FunctionMessage = CommonSettings.GetTransactionStatusMessage(objGetTransactionStatusBEResult.ReturnCode),
                                });

                                order.OrderStatusId = (int)OrderStatusEnum.COMPLETE;
                                order.FunctionFinalReturnCode = objGetTransactionStatusBEResult.ReturnCode;
                                order.FunctionNameFinalCall = CommonSettings.MMS_GetTransactionStatusBE;
                                order.ResultName = CommonSettings.GetTransactionStatusMessage(objGetTransactionStatusBEResult.ReturnCode);
                                _OrderService.InsertOrUpdate(order);
                                break;
                            case 5:
                            case 2:
                                order.OrderNotes.Add(new OrderNote()
                                {
                                    Note = "Payoo Reponse - Giao dịch thất bại",
                                    CreatedOn = DateTime.UtcNow,
                                    FunctionName = CommonSettings.MMS_GetTransactionStatusBE,
                                    FunctionReturnCode = objGetTransactionStatusBEResult.ReturnCode,
                                    FunctionMessage = CommonSettings.GetTransactionStatusMessage(objGetTransactionStatusBEResult.ReturnCode),
                                });

                                order.OrderStatusId = (int)OrderStatusEnum.CANCEL;
                                order.FunctionFinalReturnCode = objGetTransactionStatusBEResult.ReturnCode;
                                order.FunctionNameFinalCall = CommonSettings.MMS_GetTransactionStatusBE;
                                order.ResultName = CommonSettings.GetTransactionStatusMessage(objGetTransactionStatusBEResult.ReturnCode);
                                _OrderService.InsertOrUpdate(order);
                                break;
                        }
                    }
                    return objGetTransactionStatusBEResult.ReturnCode;
                }
            }
            catch (Exception ex)
            {
                order.OrderNotes.Add(new OrderNote()
                {
                    Note = "Payoo - Giao dịch nạp topup thất bại - có lỗi xảy ra :" + ex.Message,

                    CreatedOn = DateTime.UtcNow
                });
                order.FunctionFinalReturnCode = -111;
                order.FunctionNameFinalCall = CommonSettings.MMS_TopupPaymentBE;
                order.ResultName = CommonSettings.GetTransactionStatusMessage(-111);
                _OrderService.InsertOrUpdate(order);
                return -111;
            }
        }


        #endregion
    }
    public class CertPolicy : ICertificatePolicy
    {
        /// <author>Dung Do</author>
        ///<cm>H</cm>
        /// <summary>
        /// Handles CheckValidationResult of the ICertificatePolicy.
        /// </summary>
        /// <param name="srvPoint">The ServicePoint of remote service.</param>
        /// <param name="certificate">Certificate to be checked.</param>
        /// <param name="request">The current webservice request.</param>
        /// <param name="certificateProblem">Problem code.</param>
        /// <returns>Always returns true.</returns>
        public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request, int certificateProblem)
        {
            // You can do your own certificate checking.
            // You can obtain the error values from WinError.h.
            // Return true so that any certificate will work with this service.
            return true;
        }

    }
}
