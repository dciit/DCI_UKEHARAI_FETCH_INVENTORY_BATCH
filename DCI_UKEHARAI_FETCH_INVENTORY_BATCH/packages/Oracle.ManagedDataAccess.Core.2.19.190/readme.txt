Oracle.ManagedDataAccess.Core NuGet Package 2.19.190 README
===========================================================
Release Notes: Oracle Data Provider for .NET Core

March 2023

This provider supports .NET 6 & .NET 7.

This document provides information that supplements the Oracle Data Provider for .NET (ODP.NET) documentation.

You have downloaded Oracle Data Provider for .NET. The license agreement is available here:
https://www.oracle.com/downloads/licenses/distribution-license.html


Bug Fixes since Oracle.ManagedDataAccess.Core NuGet Package 2.19.180
====================================================================
Bug 35155436 MAPPING FROM .NET TIMEZONE TO ORACLE TIMEZONE IS NOT TERRITORY/REGION BASED
Bug 35100428 CONNECTION CREATION FAILURES CAUSE THREADS AND POOLMANAGER OBJECTS TO ACCUMULATE 
Bug 34873260 MORE SESSIONS CREATED THAN NECESSARY WHEN USING PROXY CONNECTIONS
Bug 32812583 NULLREFERENCEEXCEPTION WHEN HAVING ADDRESS_LIST WITHIN ADDRESS_LIST

Known Issues and Limitations
============================
1) BindToDirectory throws NullReferenceException on Linux when LdapConnection AuthType is Anonymous

https://github.com/dotnet/runtime/issues/61683

This issue is observed when using System.DirectoryServices.Protocols, version 6.0.0.
To workaround the issue, use System.DirectoryServices.Protocols, version 5.0.1.

 Copyright (c) 2021, 2023, Oracle and/or its affiliates. 
