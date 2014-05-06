using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
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

        public PayooService()
        {
            _providerService = new ProviderService();
            _cardMobileService = new CardMobileService();
            _service = new ServicesService();
            _vuiExe = new VUService.UniGWSSoapClient();
           
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

                        var listCardMobile = _cardMobileService.GetAll(x=>x.CategoryCardMobileId==categoryCardMobileId).ToList();

                        foreach (var cardmobile in listCardMobile)
                        {
                            if (!ListCardValue.Contains(cardmobile.UnitPrice))
                               _cardMobileService.Delete(cardmobile);
                        }

                        listCardMobile =_cardMobileService.GetAll(x=>x.CategoryCardMobileId==categoryCardMobileId).ToList();
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
                var cardMobile =_cardMobileService.GetById(cardMobileId);
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
        public bool UpdateFrtPayooServiceListFromPayoo()
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
                string strRequestTime = DateTime.Now.ToString("dd/MM/yyyy HHmmss");
                request.RequestTime = strRequestTime;
                // Checksum = MD5(RequestTime + ClientPassword)
                request.Checksum = GetMD5Hash(strRequestTime + clientPassword);
                // End
                request.Operation = CommonSettings.MMS_GetServices;

                GetServicesRequest objGetServicesRequest = new GetServicesRequest();

                data = XmlSerializeLib.XmlSerialize(objGetServicesRequest);

                request.Signature = SignData(data, privateKeyUrl, passwordForPrivateKey);

                request.RequestData = data;

                UniGWSResponse objResponse = uniGW.Execute2(request);

                // Verify signature.

                bool isCorrect = Verify(objResponse.ResponseData, objResponse.Signature, publicKey);
                if (!isCorrect)
                {
                    return false;
                }
                else
                {
                    // Deserialize ResponseData in UniGWSResponse object for check ReturnCode

                    GetServicesResult objGetServicesResult = XmlSerializeLib.Deserialize(objResponse.ResponseData, typeof(GetServicesResult))
                        as GetServicesResult;

                    if (objGetServicesResult.ReturnCode == 0) // Sucessfully
                    {
                        if (objGetServicesResult.Services != null)
                        {
                            var listdbServiceIds = GetAllListFrtPayooService().Select(p => p.ServiceId).ToList();
                            var listPayooServiceIds = objGetServicesResult.Services.Select(p => p.ServiceId);
                            listdbServiceIds.ForEach(id =>
                            {
                                if (!listPayooServiceIds.Contains(id))
                                {
                                    var dbService = GetFrtPayooServiceByServiceId(id);
                                    if (dbService != null)
                                        DeleteFrtPayooService(dbService);
                                }
                            });
                            objGetServicesResult.Services.ToList().ForEach(payooservice =>
                            {
                                var frtpayooservice = GetFrtPayooServiceByServiceId(payooservice.ServiceId);
                                if (frtpayooservice == null)
                                {
                                    frtpayooservice = new FrtPayooService()
                                    {
                                        ServiceId = payooservice.ServiceId,
                                        ServiceName = payooservice.ServiceName,
                                        MatchProviderCount = payooservice.MatchProviderCount,
                                        CreatedOnUtc = DateTime.Now,
                                        LastUpdatedDateUtc = DateTime.Now
                                    };

                                    InsertFrtPayooService(frtpayooservice);
                                    if (payooservice.Issuers != null)
                                    {
                                        payooservice.Issuers.ToList().ForEach(payooprovider =>
                                        {
                                            var frtpayooProvider = new FrtPayooProvider()
                                            {
                                                ServiceId = frtpayooservice.ServiceId,
                                                ProviderName = payooprovider.IssuerName,
                                                ProviderId = payooprovider.IssuerName,
                                                IsOnline = payooprovider.IsOnline,
                                                CreatedOnUtc = DateTime.Now,
                                                LastUpdatedDateUtc = DateTime.Now,
                                                ServiceEntityId = frtpayooservice.Id
                                            };

                                            frtpayooservice.PayooProviders.Add(frtpayooProvider);

                                        });

                                        UpdateFrtPayooService(frtpayooservice);
                                    }

                                }
                                else
                                {
                                    if (payooservice.Issuers != null)
                                    {
                                        payooservice.Issuers.ToList().ForEach(payooprovider =>
                                        {
                                            var frtpayooProvider = GetFrtPayooProviderByProviderId(payooprovider.IssuerId);
                                            if (frtpayooProvider == null)
                                            {
                                                frtpayooProvider = new FrtPayooProvider()
                                                {
                                                    ServiceId = frtpayooservice.ServiceId,
                                                    ProviderName = payooprovider.IssuerName,
                                                    ProviderId = payooprovider.IssuerName,
                                                    IsOnline = payooprovider.IsOnline,
                                                    CreatedOnUtc = DateTime.Now,
                                                    LastUpdatedDateUtc = DateTime.Now
                                                };

                                                frtpayooservice.PayooProviders.Add(frtpayooProvider);
                                            }
                                            else
                                            {
                                                frtpayooProvider.ProviderName = frtpayooProvider.ProviderName;
                                                frtpayooProvider.IsOnline = frtpayooProvider.IsOnline;
                                                UpdateFrtPayooProvider(frtpayooProvider);
                                            }

                                        });


                                        UpdateFrtPayooService(frtpayooservice);
                                    }
                                }

                            });
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

        public bool UpdateFrtPayooProviderListFromPayoo()
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
                string strRequestTime = DateTime.Now.ToString("dd/MM/yyyy HHmmss");
                request.RequestTime = strRequestTime;
                // Checksum = MD5(RequestTime + ClientPassword)
                request.Checksum =MD5Encrypt.MD5Hash(strRequestTime + clientPassword);
                // End
                request.Operation = CommonSettings.MMS_GetProviders;

                GetProvidersRequest objGetProvidersRequest = new GetProvidersRequest();

                data = XmlSerializeLib.XmlSerialize(objGetProvidersRequest);

                request.Signature = SignData(data, privateKeyUrl, passwordForPrivateKey);

                request.RequestData = data;

                UniGWSResponse objResponse = uniGW.Execute2(request);

                // Verify signature.

                bool isCorrect = Verify(objResponse.ResponseData, objResponse.Signature, publicKey);
                if (!isCorrect)
                {
                    return false;
                }
                else
                {
                    // Deserialize ResponseData in UniGWSResponse object for check ReturnCode

                    GetProvidersResult objGetProvidersResult = XmlSerializeLib.Deserialize(objResponse.ResponseData, typeof(GetProvidersResult))
                        as GetProvidersResult;

                    if (objGetProvidersResult.ReturnCode == 0) // Sucessfully
                    {

                        if (objGetProvidersResult.Providers != null)
                        {
                            var listdbProviderId = _providerService.GetAll().Select(p => p.ProviderCode).ToList();
                            var listPayooProviderId = objGetProvidersResult.Providers.Select(p => p.ProviderId);
                            listdbProviderId.ForEach(id =>
                            {
                                if (!listPayooProviderId.Contains(id))
                                {
                                    var dbProvider = _providerService.GetAll(x=>x.ProviderCode==id).FirstOrDefault();
                                    if (dbProvider != null)
                                        _providerService.Delete(dbProvider);
                                }
                            });
                            objGetProvidersResult.Providers.ToList().ForEach(payooprovider =>
                            {
                                var frtpayooService =_service.GetAll(x=>x.ServiceCode==payooprovider.ServiceId).FirstOrDefault();
                                if (frtpayooService != null)
                                {
                                    var frtpayooProvider = _providerService.GetAll(x => x.ProviderCode == payooprovider.ProviderName).FirstOrDefault();
                                    if (frtpayooProvider == null)
                                    {
                                        frtpayooProvider = new Domain.Entity.Provider()
                                        {
                                            ServiceId = frtpayooService.Id,
                                            ProviderName = payooprovider.ProviderName,
                                            ProviderCode = payooprovider.ProviderId,
                                            IsOnline = true,                                          
                                        };
                                   //    _providerService.InsertOrUpdate(frtpayooProvider);
                                       frtpayooService.Providers.Add(frtpayooProvider);
                                       _service.InsertOrUpdate(frtpayooService);
                                    }
                                    else
                                    {
                                        frtpayooProvider.ProviderName = payooprovider.ProviderName;
                                        frtpayooProvider.ServiceCode = payooprovider.ServiceId;
                                        _providerService.InsertOrUpdate(frtpayooProvider);
                                    }
                                }
                            });


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
