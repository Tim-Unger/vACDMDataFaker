# Docker Compose

Make sure to set the Environment Variables accordingly 
(I highly recommend using a [Vatsim-Dev](https://vatsim.dev/services/connect/sandbox/) environment for the vACDM user, **please don't run this on a prod server**). The program will add Pilots with nonsense Flightplans, SIDs and Runways to the vACDM-Data\
The program will warn you on startup if you do not use any of the Dev-CIDs. 

## Sample Docker Compose

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
        
  flowmeasures:
    image: timunger/vacdmdatafaker-flowmeasures:latest
    container_name: datafaker-ecfmp
    restart: unless-stopped
    ports: 
      - '6000:6000'
    environment:
      - ECFMP_USER=
      - ECFMP_PASSWORD=
```

## Environment Variables

**vACDM:**

| Name                            | Type   | Description                                                                                                                                                 |
| ---------                       |-----   | -----------                                                                                                                                                 |
| **VACDM_CID**                  | int    | CID of the vACDM User (used for API-Requests)                                                                                                               |
| **VACDM_PASSWORD**            | string | Password for the CID above                                                                                                                                  |
| **VACDM_URL**                  | string | URL of the vACDM-Server. Please format as follows: {VACDM_URL}/api/v1/pilots.                                                                               |
| UPDATE_AUTOMATICALLY (optional) | bool   | Whether the data should be updated automatically (If not, new pilots can only be added through the API)                                                     |
| UPDATE_INTERVAL (optional)      | int    | Amount in minutes the data will be updated automatically (Default = 10 min.)                                                                                |
| MINIMUM_AMOUNT (optional)       | int    | Minimum amount of pilots that should be present. Will only be used by the automatic update, is ignored when making a request through the API (Default = 10) |
| MAXIMUM_AMOUNT (optional)       | int    | Maximum amount of pilots that should be present. Will only be used by the automatic update, is ignored when making a request through the API (Default = 50) |

**ECFMP**

| **Name**                       | **Type**   | **Description**                                                                                                                                            |
| ---------                       |-----        | -----------                                                                                                                                                 |
| **ECFMP_USER**                | string       | Username used for authentication with the API. You can choose any username you like                                                                         |
| **ECFMP_PASSWORD**            | string      | Password used for authentication with the API                                                                                                               |
| UPDATE_AUTOMATICALLY (optional) | bool        | Whether the data should be updated automatically (If not, new pilots can only be added through the API)                                                     |
| UPDATE_INTERVAL (optional)      | int         | Amount in minutes the data will be updated automatically (Default = 10 min.)                                                                                |
| MINIMUM_AMOUNT (optional)       | int         | Minimum amount of pilots that should be present. Will only be used by the automatic update, is ignored when making a request through the API (Default = 10) |