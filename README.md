# Instructions

In order to be able to run this project you need to provide `.env` file in root of project or you can keep using appsettings.json

## `.env` file should contain these variables

```shell
ConnectionStrings__SunflowerECommerce=""
ConnectionStrings__HolooConnectionString=""
SiteSettings__CommonSetting__ProtectionSecretKey=""
SiteSettings__DefaultEmailSetting__FromName=""
SiteSettings__DefaultEmailSetting__FromEmail=""
SiteSettings__DefaultEmailSetting__SmtpServer=""
SiteSettings__DefaultEmailSetting__Port=""
SiteSettings__DefaultEmailSetting__Username=""
SiteSettings__DefaultEmailSetting__Password=""
SiteSettings__IdentitySetting__IdentitySecretKey=""
SiteSettings__IdentitySetting__EncryptKey=""
SiteSettings__IdentitySetting__JwtTtl=""
SiteSettings__IdentitySetting__RefreshTokenTtl=""
SiteSettings__IdentitySetting__Audience=""
SiteSettings__IdentitySetting__Issuer=""
SiteSettings__IdentitySetting__NotBeforeMinutes=""
SiteSettings__IdentitySetting__PasswordRequireDigit=""
SiteSettings__IdentitySetting__PasswordRequiredLength=""
SiteSettings__IdentitySetting__PasswordRequireNoneAlphanumeric=""
SiteSettings__IdentitySetting__PasswordRequireUppercase=""
SiteSettings__IdentitySetting__PasswordRequireLowercase=""
SiteSettings__IdentitySetting__RequireUniqueEmail=""
SiteSettings__SanadSettings__col_Code=""
SiteSettings__SanadSettings__moien_Code=""
SiteSettings__SanadSettings__tafzili_Code=""
SiteSettings__SanadSettings__merchantId=""
SiteSettings__SanadSettings__terminalId=""
SiteSettings__SanadSettings__terminalKey=""
CustomerCode=""
SmsIr__apikey=""
SmsIr__apiName=""
SmsIr__url=""
SmsIr__invoiceTemplateId=""
SmsIr__authenticationTemplateId=""
InvoiceNumbers__num1=""
InvoiceNumbers__num2=""
InvoiceNumbers__num3=""
InvoiceNumbers__num4=""
```

## `ECommerce.Front.BolouriGroup/appsettings.json` file should contain these variables

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "FrontSetting": {
    "SiteName": "تجهیزات بلوری",
    "BaseAddress": "https://localhost:7257/"
  },
  "AllowedHosts": "*",

  "SmsIr": {
    "apikey": "<your-api-key>",
    "apiName": "<your-api-name>",
    "url": "https://api.sms.ir/v1/send/verify",
    "invoiceTemplateId": "<invoice-template-id>",
    "authenticationTemplateId": "<authentication-template-id>"
  },
  "SiteSettings": {
    "SanadSettings": {
      "merchantId": "<merchand-id>",
      "terminalId": "<terminal-id>",
      "terminalKey": "<terminal-key>"
    }
  },
  "InvoiceNumbers": {
    "num1": "<phone-number>",
    "num2": "<phone-number>",
    "num3": "<phone-number>",
    "num4": "<phone-number>"
  },
  "TopHeaderWelcome": "به فروشگاه تجهیزات صنعتی بلوری خوش آمدید"
}
```

## `ECommerce.Front.API/appsettings.json` file should contain these variables

```json
{
  "Serilog": {
    "Using": ["Serilog.Sinks.Graylog", "Serilog.Sinks.Console"],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Error",
        "Microsoft.EntityFrameworkCore.Database.Command": "Error"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}"
        }
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs\\log-.txt",
          "rollingInterval": "Day",
          "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}"
        }
      }
    ],
    "Properties": {
      "Application": "Centralized logging application"
    }
  },
  "ConnectionStrings": {
    "SunflowerECommerce": "<sunflower-database-connection-string>",
    "HolooConnectionString": "<holoo-databse-connection-string>"
  },
  "SiteSettings": {
    "CommonSetting": {
      "ProtectionSecretKey": "<your-protection-secret-key>"
    },
    "DefaultEmailSetting": {
      "FromName": "<from-name>",
      "FromEmail": "<from-email>",
      "SmtpServer": "<smtp-server>",
      "Port": "<smtp-port>",
      "Username": "<username>",
      "Password": "<password>"
    },
    "IdentitySetting": {
      "IdentitySecretKey": "<identity-secret-key>",
      "EncryptKey": "<encrypt-key>",
      "JwtTtl": "1", // month
      "RefreshTokenTtl": "7", // month
      "Audience": "ECommerce",
      "Issuer": "ECommerce",
      "NotBeforeMinutes": "0",
      "PasswordRequireDigit": "false",
      "PasswordRequiredLength": "4",
      "PasswordRequireNoneAlphanumeric": "false",
      "PasswordRequireUppercase": "false",
      "PasswordRequireLowercase": "false",
      "RequireUniqueEmail": "true"
    },
    "SanadSettings": {
      "col_Code": "<col-code>",
      "moien_Code": "<moein-code>",
      "tafzili_Code": "tafzili-code>"
    }
  },
  "AllowedHosts": "*",
  "CustomerCode": "<customer-code>" //Boloori
}
```
