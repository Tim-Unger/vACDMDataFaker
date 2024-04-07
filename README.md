# Docker Compose
**Sample Docker Compose**

Make sure to set the Environment Variables accordingly (I highly recommend using a [Vatsim-Dev](https://vatsim.dev/services/connect/sandbox/) environment for the vACDM user, **please don't run this on a prod server**).\
The program will warn you on startup if you do not use any of the Dev-CIDs.
Please format the VACDM_URL as follows: {VACDM_URL}/api/v1/pilots.

For the ECFMP User and Password you can choose any username and password of your choice (The ECFMP Credentials are used when doing the Update via the API POST Endpoint)

```yaml

version: '1.0'
  
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
    container_name: datafaker-flowmeasures
    restart: unless-stopped
    ports: 
      - '6000:6000'
    environment:
      - ECFMP_USER=
      - ECFMP_PASSWORD=
```
