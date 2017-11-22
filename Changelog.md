### 11/21/2017 - 2.0.0
- Fixed problem with validating enums that are a property of the client by using reference outside anonymous function scope. 

### 9/12/2017 - 1.9.4
- For `"collectionFormat": "multi"` in `query` parameters, if the item type is null or undefined then we treat it as an empty string
- Removing the check that primaryType in the formatted reference value should only be a string. It can be any other primary type apart from "string".
 
### 9/5/2017 - 1.9.3
- Added support for `"collectionFormat": "multi"` in `query` parameters https://github.com/Azure/autorest/issues/717.