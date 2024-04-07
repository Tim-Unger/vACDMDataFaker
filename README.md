# Docker Compose
Sample Docker Compose

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
