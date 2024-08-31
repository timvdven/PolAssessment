# Recommendations for production environments

[Back to main](../README.md)

- Usage of OTAP
- Usage of CI/CD
- Change default passwords (especially on A and P)
  - MariaDB
  - Client Secrets
  - Default Users
- Restrict access to all services very strictly
  - Only allow access to Front End and Web API with proper settings like
    - Https enforcement
    - Proper https certifcate
    - Usage of a CDN, especially Front End should be cached entirely!
  - Keep OWASP in mind
  - Properly set CORS rules
  - Properly set CSP rules
  - Prevent Cross Site Request Forgery (CSRF) calls
  - Add `.well-known/security.txt` for [Vulnerability Disclosure](https://www.ncsc.nl/documenten/publicaties/2019/mei/01/cvd-leidraad) reasons
  - Regularly have the level of securiy of the solution determined by external parties
    - E.g. (automated) pen tests
    - Vulnerability tests
    - Determine the minimum desired level of security and reserve time and budget for security fixes
- Use a proper logging, monitoring, alerting mechanism
  - Application Insights
  - Graylog
  - etc.
- Use a configuration management database (CMDB)
  - Set an alert for expiring token
    - E.g. ApiKey of the LocationEnricher
    - Some other things that may expire (like Azure client secrets)
  - Set an alert for changes of the external APIs
  - Record all 3rd party dependencies (NuGet packages)
    - Set a monthly alert for checking and updating 3rd party dependencies
- Create, maintain and regularly invoke automatic tests for regression
  - Use something like Selenium
