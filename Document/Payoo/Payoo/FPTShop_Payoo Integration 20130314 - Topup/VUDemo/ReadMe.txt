HƯỚNG DẪN CHẠY DEBUG CHO TESTAPP.

Cần thay đổi một thống thông tin trong file app.config:

1: Url cho private key
<add key="PrivateKeyUrl" value="D:\VietUnion\VUDemo\VUDemo\bin\FPTSHOP_pkcs12.p12"/> 

2: Url cho public key mà Payoo gửi qua để hổ trợ cho việc Verify chữ ký.
<add key="PublicKeyUrl" value="D:\VietUnion\VUDemo\VUDemo\bin\PAYOO_Cert.pem"/>

3: Password để đọc private key
<add key="PasswordForPrivateKey" value="123456789"/>

4: Thông tin về Credential mà Payoo gửi qua
<add key="ClientId" value="FPTSHOP"/>
<add key="ClientPassword" value="SMART"/>
