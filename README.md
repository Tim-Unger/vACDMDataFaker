# vACDMDataFaker

A program that creates and continuously updates fake data for [vACDM](https://github.com/vACDM) and [ECFMP](https://ecfmp.vatsim.net) mainly for development purposes. \
The vACDM-Faker requires/interacts with a vACDM-Instance (Setup [here](https://vacdm.net/docs/developer/installation)), to put the data somewhere.

## Docker Compose

Make sure to set the Environment Variables accordingly \
I highly recommend running the vACDM-Faker only inside a Vatsim-Dev-Environment, you shouldn't run this with actual CIDs \
The ECFMP-Faker runs on itself and does not interact with any other Vatsim-Services
Unless explicitly allowed by the Env-Variable, the vACDM-Faker will only run with Dev-CIDs (more info [here](https://vatsim.dev/services/connect/sandbox/))

### Minimal Docker Compose

```yaml

version: '1.1'
  
services:
  vacdm:
    image: timunger/vacdmdatafaker-vacdm:latest
    container_name: datafaker-vacdm
    restart: unless-stopped
    ports: 
      - '6001:6001'
    environment:
      - VACDM_CID=
      - VACDM_PASSWORD=
      - VACDM_URL=
      - ASPNETCORE_URLS=http://*:6001
        
  flowmeasures:
    image: timunger/vacdmdatafaker-flowmeasures:latest
    container_name: datafaker-ecfmp
    restart: unless-stopped
    ports: 
      - '6002:6002'
    environment:
      - ECFMP_USER=
      - ECFMP_PASSWORD=
      - ASPNETCORE_URLS=http://*:6002
```

### Environment Variables

**Bold is required** The rest is optional

**vACDM:**

| Name                            | Type   | Description                                                                                                                                                 |
| ---------                       |-----   | -----------                                                                                                                                                 |
| **VACDM_CID**                  | int    | CID of the vACDM User (used for API-Requests)                                                                                                               |
| **VACDM_PASSWORD**            | string | Password for the CID above                                                                                                                                  |
| **VACDM_URL**                  | string | URL of the vACDM-Server. Please format as follows: {VACDM_URL}/api/v1/pilots.                                                                               |
| UPDATE_AUTOMATICALLY (optional) | bool   | Whether the data should be updated automatically (If not, new pilots can only be added through the API)  *(Default = true)*                                   |
| UPDATE_INTERVAL (optional)      | int    | Amount in minutes the data will be updated automatically *(Default = 10 min.)*                                                                               |
| MINIMUM_AMOUNT (optional)       | int    | Minimum amount of pilots that should be present. Will only be used by the automatic update, is ignored when making a request through the API *(Default = 10)* |
| MAXIMUM_AMOUNT (optional)       | int    | Maximum amount of pilots that should be present. Will only be used by the automatic update, is ignored when making a request through the API *(Default = 50)* |
| REQUIRE_AUTH_FOR_LOGS (optional)| bool   | Whether the GET Endpoint that is used to access the Log-JSON requires authentication *(Default = true)*                                                      |
| ALLOW_NON_DEV_CIDS (optional)   | bool   | Whether the program can be used with non-dev CIDs **(Not recommended)** *(Default = false)*                                                                 |
| AIRPORTS (optional)             | string | Comma separated List of airports that should be checked/used for the fake data (only Vatsim-Flights that use this ICAO are used, so don't use dead airports) *(Default = EDDF,EDDS,EDDK,EDDM,EDDL,EDDH,EDDB)*|

**ECFMP**

| **Name**                       | **Type**   | **Description**                                                                                                                                            |
| ---------                       |-----        | -----------                                                                                                                                                 |
| **ECFMP_USER**                | string       | Username used for authentication with the API. You can choose any username you like                                                                         |
| **ECFMP_PASSWORD**            | string      | Password used for authentication with the API                                                                                                               |
| UPDATE_AUTOMATICALLY (optional) | bool        | Whether the data should be updated automatically (If not, new pilots can only be added through the API) *(Default = true)*                                   |
| UPDATE_INTERVAL (optional)      | int         | Amount in minutes the data will be updated automatically *(Default = 10 min.)*                                                                               |
| MINIMUM_AMOUNT (optional)       | int         | Minimum amount of measures that should be present. Will only be used by the automatic update, is ignored when making a request through the API *(Default = 10)* |
| REQUIRE_AUTH_FOR_LOGS (optional)| bool        | Whether the GET Endpoint that is used to access the Log-JSON requires authentication *(Default = true)*                                                       |

## API

Both programs have a small API to access and update/delete the data. The API-Docs can be found at the root of the URL of your instance
