---
uid: azure-arm-parameterflattening
summary: *content

---
**This package will eventually be replaced by our new TypeScript-based isomorphic SDKs. Learn more at the [GitHub repository for our new packages](https://github.com/Azure/azure-sdk-for-js).**
## Microsoft Azure SDK for Node.js - AutoRestParameterFlattening
This project provides a Node.js package for accessing Azure. Right now it supports:
- **Node.js version 6.x.x or higher**

### Features


### How to Install

```bash
npm install azure-arm-parameterflattening
```

### How to use

#### Authentication, client creation and update availabilitySets as an example.

```javascript
const msRest = require("ms-rest");
const AutoRestParameterFlattening = require("azure-arm-parameterflattening");
const token = "<access_token>";
const creds = new msRest.TokenCredentials(token);
const subscriptionId = "<Subscription_Id>";
const client = new AutoRestParameterFlattening(creds, subscriptionId);
const resourceGroupName = "testresourceGroupName";
const avset = "testavset";
const tags1 = {
  tags: { "key1": "testtags" }
};
client.availabilitySets.update(resourceGroupName, avset, tags1).then((result) => {
  console.log("The result is:");
  console.log(result);
}).catch((err) => {
  console.log('An error occurred:');
  console.dir(err, {depth: null, colors: true});
});

### Related projects

- [Microsoft Azure SDK for Node.js](https://github.com/Azure/azure-sdk-for-node)
