/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 *
 * Code generated by Microsoft (R) AutoRest Code Generator.
 * Changes may cause incorrect behavior and will be lost if the code is regenerated.
 */

import { BaseResource, CloudError } from "ms-rest-azure";
import * as moment from "moment";

export {

  BaseResource,
  CloudError
};

export interface StorageAccountCheckNameAvailabilityParameters {
  name: string;
  type?: string;
}

/**
 * The CheckNameAvailability operation response.
*/
export interface CheckNameAvailabilityResult {
  /**
   * Gets a boolean value that indicates whether the name is available for you to use. If true, the
   * name is available. If false, the name has already been taken or invalid and cannot be used.
  */
  nameAvailable?: boolean;
  /**
   * Gets the reason that a storage account name could not be used. The Reason element is only
   * returned if NameAvailable is false. Possible values include: 'AccountNameInvalid',
   * 'AlreadyExists'
  */
  reason?: string;
  /**
   * Gets an error message explaining the Reason value in more detail.
  */
  message?: string;
}

export interface Resource extends BaseResource {
  /**
   * Resource Id
  */
  readonly id?: string;
  /**
   * Resource name
  */
  readonly name?: string;
  /**
   * Resource type
  */
  readonly type?: string;
  /**
   * Resource location
  */
  location: string;
  /**
   * Resource tags
  */
  tags?: { [propertyName: string]: string };
}

/**
 * The parameters to provide for the account.
*/
export interface StorageAccountCreateParameters extends Resource {
  /**
   * Gets or sets the account type. Possible values include: 'Standard_LRS', 'Standard_ZRS',
   * 'Standard_GRS', 'Standard_RAGRS', 'Premium_LRS'
  */
  accountType?: string;
}

/**
 * The URIs that are used to perform a retrieval of a public blob, queue or table object.
*/
export interface Bar {
  /**
   * Recursive Endpoints
  */
  recursivePoint?: Endpoints;
}

/**
 * The URIs that are used to perform a retrieval of a public blob, queue or table object.
*/
export interface Foo {
  /**
   * Bar point
  */
  barPoint?: Bar;
}

/**
 * The URIs that are used to perform a retrieval of a public blob, queue or table object.
*/
export interface Endpoints {
  /**
   * Gets the blob endpoint.
  */
  blob?: string;
  /**
   * Gets the queue endpoint.
  */
  queue?: string;
  /**
   * Gets the table endpoint.
  */
  table?: string;
  /**
   * Dummy EndPoint
  */
  dummyEndPoint?: Endpoints;
  /**
   * Foo point
  */
  fooPoint?: Foo;
}

/**
 * The custom domain assigned to this storage account. This can be set via Update.
*/
export interface CustomDomain {
  /**
   * Gets or sets the custom domain name. Name is the CNAME source.
  */
  name?: string;
  /**
   * Indicates whether indirect CName validation is enabled. Default value is false. This should
   * only be set on updates
  */
  useSubDomain?: boolean;
}

/**
 * The storage account.
*/
export interface StorageAccount extends Resource {
  /**
   * Gets the status of the storage account at the time the operation was called. Possible values
   * include: 'Creating', 'ResolvingDNS', 'Succeeded'
  */
  provisioningState?: string;
  /**
   * Gets the type of the storage account. Possible values include: 'Standard_LRS', 'Standard_ZRS',
   * 'Standard_GRS', 'Standard_RAGRS', 'Premium_LRS'
  */
  accountType?: string;
  /**
   * Gets the URLs that are used to perform a retrieval of a public blob, queue or table
   * object.Note that StandardZRS and PremiumLRS accounts only return the blob endpoint.
  */
  primaryEndpoints?: Endpoints;
  /**
   * Gets the location of the primary for the storage account.
  */
  primaryLocation?: string;
  /**
   * Gets the status indicating whether the primary location of the storage account is available or
   * unavailable. Possible values include: 'Available', 'Unavailable'
  */
  statusOfPrimary?: string;
  /**
   * Gets the timestamp of the most recent instance of a failover to the secondary location. Only
   * the most recent timestamp is retained. This element is not returned if there has never been a
   * failover instance. Only available if the accountType is StandardGRS or StandardRAGRS.
  */
  lastGeoFailoverTime?: Date;
  /**
   * Gets the location of the geo replicated secondary for the storage account. Only available if
   * the accountType is StandardGRS or StandardRAGRS.
  */
  secondaryLocation?: string;
  /**
   * Gets the status indicating whether the secondary location of the storage account is available
   * or unavailable. Only available if the accountType is StandardGRS or StandardRAGRS. Possible
   * values include: 'Available', 'Unavailable'
  */
  statusOfSecondary?: string;
  /**
   * Gets the creation date and time of the storage account in UTC.
  */
  creationTime?: Date;
  /**
   * Gets the user assigned custom domain assigned to this storage account.
  */
  customDomain?: CustomDomain;
  /**
   * Gets the URLs that are used to perform a retrieval of a public blob, queue or table object
   * from the secondary location of the storage account. Only available if the accountType is
   * StandardRAGRS.
  */
  secondaryEndpoints?: Endpoints;
}

/**
 * The access keys for the storage account.
*/
export interface StorageAccountKeys {
  /**
   * Gets the value of key 1.
  */
  key1?: string;
  /**
   * Gets the value of key 2.
  */
  key2?: string;
}

/**
 * The parameters to update on the account.
*/
export interface StorageAccountUpdateParameters extends Resource {
  /**
   * Gets or sets the account type. Note that StandardZRS and PremiumLRS accounts cannot be changed
   * to other account types, and other account types cannot be changed to StandardZRS or
   * PremiumLRS. Possible values include: 'Standard_LRS', 'Standard_ZRS', 'Standard_GRS',
   * 'Standard_RAGRS', 'Premium_LRS'
  */
  accountType?: string;
  /**
   * User domain assigned to the storage account. Name is the CNAME source. Only one custom domain
   * is supported per storage account at this time. To clear the existing custom domain, use an
   * empty string for the custom domain name property.
  */
  customDomain?: CustomDomain;
}

export interface StorageAccountRegenerateKeyParameters {
  /**
   * Possible values include: 'key1', 'key2'
  */
  keyName?: string;
}

/**
 * The Usage Names.
*/
export interface UsageName {
  /**
   * Gets a string describing the resource name.
  */
  value?: string;
  /**
   * Gets a localized string describing the resource name.
  */
  localizedValue?: string;
}

/**
 * Describes Storage Resource Usage.
*/
export interface Usage {
  /**
   * Gets the unit of measurement. Possible values include: 'Count', 'Bytes', 'Seconds', 'Percent',
   * 'CountsPerSecond', 'BytesPerSecond'
  */
  unit?: string;
  /**
   * Gets the current count of the allocated resources in the subscription.
  */
  currentValue?: number;
  /**
   * Gets the maximum count of the resources that can be allocated in the subscription.
  */
  limit?: number;
  /**
   * Gets the name of the type of usage.
  */
  name?: UsageName;
}

/**
 * The List Usages operation response.
*/
export interface UsageListResult {
  /**
   * Gets or sets the list Storage Resource Usages.
  */
  value?: Usage[];
}

export interface SubResource extends BaseResource {
  /**
   * Resource Id
  */
  id?: string;
}

/**
 * The list storage accounts operation response.
*/
export interface StorageAccountListResult extends Array<StorageAccount> {
  /**
   * Gets the link to the next set of results. Currently this will always be empty as the API does
   * not support pagination.
  */
  nextLink?: string;
}
