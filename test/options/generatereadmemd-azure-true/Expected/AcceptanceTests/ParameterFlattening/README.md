---
uid: azure-arm-parameterflattening
summary: *content

---
# Microsoft Azure SDK for Node.js - AutoRestParameterFlattening
This project provides a Node.js package for accessing Azure. Right now it supports:
- **Node.js version 6.x.x or higher**

## Features


## How to Install

```bash
npm install azure-arm-parameterflattening
```

## How to use

### Authentication, client creation and update availabilitySets as an example.

```javascript
const msRestAzure = require("ms-rest-azure");
const AutoRestParameterFlattening = require("azure-arm-parameterflattening");
msRestAzure.interactiveLogin().then((creds) => {
    const subscriptionId = "<Subscription_Id>";
    const client = new AutoRestParameterFlattening(creds, subscriptionId);
    const resourceGroupName = "testresourceGroupName";
    const avset = "testavset";
    const tags1 = {
      tags: { "key1": "testtags" }
    };
    return client.availabilitySets.update(resourceGroupName, avset, tags1).then((result) => {
      console.log("The result is:");
      console.log(result);
    });
}).catch((err) => {
  console.log('An error occurred:');
  console.dir(err, {depth: null, colors: true});
});

## Related projects

- [Microsoft Azure SDK for Node.js](https://github.com/Azure/azure-sdk-for-node)
