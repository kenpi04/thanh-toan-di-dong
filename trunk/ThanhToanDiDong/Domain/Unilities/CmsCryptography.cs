#region using directives
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.IO;
#endregion


/// <summary>
/// Summary description for DigitalSignatureLib
/// </summary>
namespace Domain.Unilities
{
    public class CmsCryptography
    {
        private X509Certificate2 _SignerCert, _RecipientCert;
        private Encoding _Enc = Encoding.Default;//Encoding.UTF8;
        public CmsCryptography()
        {

        }

        /// <summary>
        /// The CharacterEncoding property sets/gets encoding character. If value is null, UTF8 encoding is set.
        /// </summary>
        public string CharacterEncoding
        {
            get { return this._Enc.EncodingName; }
            set
            {
                if (value == null || string.Empty.Equals(value.Trim()))
                    this._Enc = Encoding.UTF8;
                else
                    _Enc = Encoding.GetEncoding(value);
            }
        }
        
        /// <summary>
        /// The RecipientPublicCertPath property sets recipient public certificate path.
        /// </summary>
        public string RecipientPublicCertPath
        {
            set
            {
                _RecipientCert = new X509Certificate2(value);
            }
        }

        #region Public Methods

        /// <summary>
        /// Load signer credential.
        /// </summary>
        public void LoadSignerCredential(string signerCertPath, string signerCertPassword)
        {
            try
            {
                _SignerCert = new X509Certificate2(signerCertPath, signerCertPassword);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Load signer credential.
        /// </summary>
        public void LoadSignerCredential(byte [] datas, string signerCertPassword)
        {
            try
            {
                _SignerCert = new X509Certificate2(datas, signerCertPassword);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Creates a signature to the CMS/PKCS #7 message. 
        /// </summary>
        public string Sign(string TextData)
        {
            try
            {
                byte[] signedBytes = ComputeSignature(_Enc.GetBytes(TextData));
                return Convert.ToBase64String(signedBytes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Verifies the digital signatures on the signed CMS/PKCS #7 message.
        /// </summary>
        public bool Verify(string TextData, string DigitalSignature)
        {
            try
            {
				byte[] encodeMessage = null;
				try
				{
					encodeMessage = Convert.FromBase64String(DigitalSignature);
				}
				catch
				{
					return false;
				}
                return VerifySignature(_Enc.GetBytes(TextData), encodeMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Private Methods

        private bool VerifySignature(byte[] messageBytes, byte[] encodeMessage)
        {
            try
            {
                ContentInfo cont = new ContentInfo(messageBytes);
                SignedCms verified = new SignedCms(cont, true);
                verified.Decode(encodeMessage);
                X509Certificate2Collection Collection =
                new X509Certificate2Collection(_RecipientCert);
                try
                {
                    verified.CheckSignature(Collection, true);
                    if (verified.Certificates.Count > 0)
                    {
                        if (!verified.Certificates[0].Equals(_RecipientCert))
                        {
                            return false;
                        }
                    }
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private byte[] ComputeSignature(byte[] messageBytes)
        {
            try
            {
                ContentInfo cont = new ContentInfo(messageBytes);
                SignedCms signed = new SignedCms(cont, true);
                CmsSigner signer = new CmsSigner(_SignerCert);
                signer.IncludeOption = X509IncludeOption.None;
                signed.ComputeSignature(signer);
                return signed.Encode();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}