# DocumentGenerator

This repository holds the source code for the full stack of applications developed to generate laboratory PDF reports using iTextSharp 5.

The following applications are provided in this repository:

1. DocumentGenerator
2. DocumentGenerator.WebAPI
3. GenerateLabResultReport

## Overall Disclaimers
- Applications in this repository rely on iTextSharp 5 under the AGPLv3 license.
- These applications are developed under .NET Framework 4.7.2 and are developed using Visual Studio.
- Development for these applications only support Windows and do not support any other operating systems.

## Document Generator
- This is an class library that holds the business logic to generate laboratory PDF reports.
- This library provides data models to structure information such as document formatting, patient demographics, and test results.
- Input data models are formatted and written to PDFs using iTextSharp 5
- The source for this application is founder under the ./DocumentGenerator/DocumentGenerator/DocumentGenerator folder of this repository.

## DocumentGenerator.WebAPI
- This is an ASP.NET Web Application that functions as a Web API to serve the methods of the DocumentGenerator library.
- This application provides POST methods to take JSON inputs from web requests and convert them into the appropriate data model to be funneled into generating PDFs using the DocumentGenerator library.
- Generated PDFs are not returned over the network, instead being written to a particular output folder path as given in the web request.
- The source for this application is founder under the ./DocumentGenerator/DocumentGenerator/DocumentGenerator.WebAPI folder of this repository.

## GenerateLabResultReport
- This is a console application that interacts with the DocumentGenerator.WebAPI to request the generation of laboratory reports.
- This application takes two command line arguments:
    - A single sample ID OR a file path to a list of sample IDs. Recognized sample ID(s) will have their PDFs generated.
    - The output folder path to hold the generated PDFs.
- From the command line arguments, data models are generated from the application's configuration settings and from querying a Microsoft SQL Server Database.
- Generated data models are sent in JSON format as POST requests to the hosted instance of the DocumentGenerator.WebAPI.
- This application is not returned the PDF over the network. The application is instead returned success or failure status codes from the DocumentGenerator.WebAPI instance.
- An optional command file for running the built application is provided to handle cleanup and writing logs. This file is found under ./GenerateLabResultReport/CommandFiles of this repository
- Local configurations must be made to the command file and the application's configuration settings, either through the build's configuration file or the project's configuration file, to adapt to the system environment.
- The source for this application is founder under the ./GenerateLabResultReport/GenerateLabResultReport/GenerateLabResultReport folder of this repository.