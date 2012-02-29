# Scutex

Scutex, pronounced (sec-u-techs), is a 100% managed .Net Framework licensing platform for your applications. Scutex is a flexible licensing system providing multiple licensing schemes allowing you the most control over how you license your products. Unlike any other licensing system on the market, Scutex provides 4 distinct licensing schemes, allowing you to protect your products at different levels using completely different licensing schemes, key types and protection systems.

Using Scutex you can create trial versions of your software products and allow your users to buy and use license keys or keyfiles to unlock your product. You can also have edition based licensing to unlock only select features based on the level of the license. Scutex supports upgrade licenses as well as allowing your customers who have an existing version of your product to upgrade to a new version.

## Features

	* Multiple License Keys
	* Backend Service
	* Multi Use Keys
	* Offline Keys
	* Hardware Locking (Online and Offline)
	* Edition Management
	* Upgrade Keys

## License

Licensed under the Microsoft Public License (MS-PL)

## Resources

* **WaveTech's Home page:** <http://www.wtdt.com>
* **Scutex's Home Page:** <http://www.wtdt.com/Products/Scutex.aspx>
* **CodePlex:** <http://scutex.codeplex.com/>
* **Issue Tracker:** <http://scutex.codeplex.com/workitem/list/basic>
* **Forum:** <http://scutex.codeplex.com/discussions>
* **Discussion list:** <http://groups.google.com/group/scutex-dev>

## Infragistics Components

As of the 10/30/2011 checkin the Infragistics dependancy has been removed.

## Official Builds & Releases

There are a number of additional processes that occur for preparing Scutex to be used in your applications. Because of that there are two places to obtain official builds that you should install and use in your products. To obtain official builds just got to the Scutex homepage at http://www.wtdt.com/Products/Scutex.aspx and click on the Downloads link or go to the CodePlex site http://scutex.codeplex.com/ and download the latest release build there. Official release builds will be a big bold download link and you can get access to development builds from our CI server.

## Prerequisites

You will need the .Net Framework 4 and SQL Server or SQL Express to use Scutex. To develop Scutex you will need the following installed.

	* Visual Studio 2010 (WPF/MSTest)
	* SQL Server 2005/2008/2008 R2 or SQL Express 2005/2008 R2
	* Microsoft MOLES & Pex Power Tools <http://research.microsoft.com/en-us/projects/pex/>
	* Infragistics WPF v10.3 (Temporary)
	* IIS, IIS Express or Casini for testing Services

## Unit Testing

I moved the unit tests from NUnit to MSTest to get integrated MOLES support, and to play around with Pex a little. Unfortunately the way MSTest plays and moves things around it makes getting the paths for required files a major issue. The ReSharper Unit Test Runner does not have that issue, and still runes the MOLES tests. If you run the unit tests from within Visual Studio using MSTest all the test dependent on location will fail. Additionally the performance of the unit tests went downhill due to how MSTest wires things up. Eventually I'll get back in there are refactor the Setup and Teardowns so that they work with MSTest.

## Supported License Key Types

There are two license key types supported:

    * Small Static License Key (SSK) XXX-XXXXXX-XXXX
	* Large Static License Key (LSK) XXXXX-XXXXX-XXXXX-XXXXX-XXXXX
	
The small static key only supports basic license key functionality, while the large static key supports many licensing scenarios.

