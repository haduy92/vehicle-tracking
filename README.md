# VehicleTracking

Vehicle Tracking is a sample application built using ASP.NET Core and Entity Framework Core. The architecture and design of the project is referenced from:

* [JasonGT/NorthwindTraders Github Repo](https://github.com/JasonGT/NorthwindTraders)
* [Clean Architecture with ASP.NET Core 2.1](https://youtu.be/_lwCVE_XgqI)

## Getting Started
Use these instructions to get the project up and running.

### Prerequisites
You will need the following tools:

* [Visual Studio 2017](https://www.visualstudio.com/downloads/)
* [.NET Core SDK 2.2](https://www.microsoft.com/net/download/dotnet-core/2.2)

### Setup
Follow these steps to get your development environment set up:

  1. Open project by Visual Studio 2017
     
  2. Next, build the solution by:
     ```
     ctrl + shift + b
	 ```
  3. Once the building has finished, launch the project by pressing:
     ```
	 F5 / ctrl + F5
	 ```
### API methods
  1. Login:
   
     - URL: [http://localhost:5000/users/login](http://localhost:5000/users/login).
     - Method: POST.
     - Data load: `{ "EmailAddress": "admin@mail.com", "Password": "admin@123" }`
     - Data tyle: json.
     - Response: A Bearer Token string which must be included in other requests.
     
  2. Register vehicle:
  
     - URL: [http://localhost:5000/vehicles/create](http://localhost:5000/vehicles/create).
     - Method: POST.
     - Data load: 
     ```
     { 
        "VehicleCode": "[vehicle_code]", 
        "DeviceCode": "[device_code]", 
        "ExtendedProperty_1": "[extended_property_1]", 
        "ExtendedProperty_2": "[extended_property_2]", 
        ... 
     }
     ```
     - Data tyle: json.
     - Response: No Content.
     
  3. Register vehicle position:
  
     - URL: [http://localhost:5000/tracking/create](http://localhost:5000/tracking/create).
     - Method: POST.
     - Data load: 
     ```
     { 
        "VehicleCode": "[vehicle_code]", 
        "DeviceCode": "[device_code]", 
        "Latitude": "[latitude]", 
        "Longitude": "[longitude]", 
        "RecordedDate": "[recorded_date]" 
     }
     ```
     - Data tyle: json.
     - Response: No Content.
     
  4. Get current vehicle position:
  
     - URL: [http://localhost:5000/tracking/get](http://localhost:5000/tracking/get).
     - Method: GET.
     - Data load: 
     ```
     {
        "VehicleCode": "[vehicle_code]", 
        "DeviceCode": "[device_code]" 
     }
     ```
     - Data tyle: json.
     - Response:
     ```
     {
        "vehicleCode": "[vehicle_code]", 
        "latitude": "[latitude]", 
        "longitude": "[longitude]", 
        "localityName": "[locality_name]", 
        "recordedDate": "[recorded_date]" 
     }
     ```
     
  5. Get vehicle journey:
  
     - URL: [http://localhost:5000/tracking/getlist](http://localhost:5000/tracking/getlist).
     - Method: GET.
     - Data load: 
     ```
     {
        "VehicleCode": "[vehicle_code]", 
        "DeviceCode": "[device_code]",
        "FromDate": "[from_date]",
        "ToDate": "[to_date]"
     }
     ```
     - Data tyle: json.
     - Response:
     ```
     {
        "vehicleCode": "vehicle_code",
        "trackingRecords": [
           {
              "latitude": "[latitude]",
              "longitude": "[longitude]",
              "localityName": "locality_name",
              "recordedDate": "[recorded_date]"
           },
           ...
        ]
     }
     ```

## Technologies
* .NET Core 2.2
* ASP.NET Core 2.2
* Entity Framework Core 2.2