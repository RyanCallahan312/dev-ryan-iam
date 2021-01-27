# dev-ryan-iam
An iam service for current and future projects. Uses Asp.Net Identity, Postgres, and Jwt to authenticate users, authorize users, and manage roles/permissions.

This is still very much a work in progress.

Once this IAM service hits the 1.0 release I will be making an API Gateway in order to provide an extra layer protection and abstraction for all services that may use this.Expect the read me to update at that time.


For now here is a little bit about the project.

This is a ASP.Net core 5 project that is targeting .Net 5. For a DB I am using postgresql running on a linux machine. For Authentication I am using Asp.Net Identity. For Authorization I am using IdentityServer4, a oidc and oauth2 framework.
